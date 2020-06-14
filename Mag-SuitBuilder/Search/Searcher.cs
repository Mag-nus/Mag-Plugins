using System;
using System.Collections.Generic;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	abstract class Searcher
	{
		protected readonly SearcherConfiguration Config = new SearcherConfiguration();
		protected readonly List<LeanMyWorldObject> Equipment = new List<LeanMyWorldObject>();
		protected readonly SuitBuilder SuitBuilder = new SuitBuilder();

		protected Searcher(SearcherConfiguration config, IEnumerable<LeanMyWorldObject> equipment, CompletedSuit startingSuit = null)
		{
			Config = config;

			foreach (var piece in equipment)
				Equipment.Add(piece);

			// Remove surpassed pieces
			for (int i = Equipment.Count - 1; i >= 0; i--)
			{
				if (Equipment.ItemIsSurpassed(Equipment[i]))
					Equipment.RemoveAt(i);
			}

			// If we were given a starting suit, lets start our SuitBuilder off with all those items
			if (startingSuit != null)
			{
				foreach (var o in startingSuit)
					SuitBuilder.Push(o.Value, o.Key);
			}

			// Remove pieces that can provide no beneficial spell
			for (int i = Equipment.Count - 1; i >= 0; i--)
			{
				if (!SuitBuilder.CanGetBeneficialSpellFrom(Equipment[i]))
					Equipment.RemoveAt(i);
			}

			// Remove pieces we can't add to our base suit
			for (int i = Equipment.Count - 1; i >= 0; i--)
			{
				if (!SuitBuilder.SlotIsOpen(Equipment[i].EquippableSlots))
				{
					if (Equipment[i].EquippableSlots.GetTotalBitsSet() == 1)
						Equipment.RemoveAt(i);
					else
					{
						if (Equipment[i].EquippableSlots.IsBodyArmor())
						{
							var reductionOptions = Equipment[i].Coverage.ReductionOptions();

							foreach (var option in reductionOptions)
							{
								if (option == CoverageMask.OuterwearChest && SuitBuilder.SlotIsOpen(EquipMask.ChestArmor)) goto end;
								if (option == CoverageMask.OuterwearUpperArms && SuitBuilder.SlotIsOpen(EquipMask.UpperArmArmor)) goto end;
								if (option == CoverageMask.OuterwearLowerArms && SuitBuilder.SlotIsOpen(EquipMask.LowerArmArmor)) goto end;
								if (option == CoverageMask.OuterwearAbdomen && SuitBuilder.SlotIsOpen(EquipMask.AbdomenArmor)) goto end;
								if (option == CoverageMask.OuterwearUpperLegs && SuitBuilder.SlotIsOpen(EquipMask.UpperLegArmor)) goto end;
								if (option == CoverageMask.OuterwearLowerLegs && SuitBuilder.SlotIsOpen(EquipMask.LowerLegArmor)) goto end;
							}

							Equipment.RemoveAt(i);
						}
						else
						{
							if ((Equipment[i].EquippableSlots.HasFlag(EquipMask.FingerWearLeft) || Equipment[i].EquippableSlots.HasFlag(EquipMask.FingerWearRight)) && !SuitBuilder.SlotIsOpen(EquipMask.FingerWearLeft) && !SuitBuilder.SlotIsOpen(EquipMask.FingerWearRight)) { Equipment.RemoveAt(i); goto end; }
							if ((Equipment[i].EquippableSlots.HasFlag(EquipMask.WristWearLeft) || Equipment[i].EquippableSlots.HasFlag(EquipMask.WristWearRight)) && !SuitBuilder.SlotIsOpen(EquipMask.WristWearLeft) && !SuitBuilder.SlotIsOpen(EquipMask.WristWearRight)) { Equipment.RemoveAt(i); goto end; }
						}
					}
				}

				end: ;
			}
		}
				
		public event Action<CompletedSuit> SuitCreated;

		protected virtual void OnSuitCreated(CompletedSuit obj)
		{
			Action<CompletedSuit> handler = SuitCreated;

			if (handler != null)
				handler(obj);
		}

		public event Action SearchCompleted;

		protected virtual void OnSearchCompleted()
		{
			Action handler = SearchCompleted;

			if (handler != null)
				handler();
		}

		public bool Running { get; private set; }

		public void Start()
		{
			if (Running)
				return;

			Running = true;

			StartSearch();
		}

		public void Stop()
		{
			if (!Running)
				return;

			Running = false;
		}

		protected abstract void StartSearch();
	}
}
