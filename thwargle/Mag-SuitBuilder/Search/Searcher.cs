using System;
using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	abstract class Searcher
	{
		protected readonly SearcherConfiguration Config = new SearcherConfiguration();
		protected readonly EquipmentGroup Equipment = new EquipmentGroup();
		protected readonly SuitBuilder SuitBuilder = new SuitBuilder();

		protected Searcher(SearcherConfiguration config, IEnumerable<SuitBuildableMyWorldObject> equipment, CompletedSuit startingSuit = null)
		{
			Config = config;

			foreach (var piece in equipment)
			{
				if (!piece.Exclude)
					Equipment.Add(piece);
			}

			// Remove pieces that don't meet our minimum requirements
			for (int i = Equipment.Count - 1; i >= 0; i--)
			{
				if (!config.ItemPassesRules(Equipment[i]))
					Equipment.RemoveAt(i);
			}

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
								if (option == CoverageFlags.Chest && SuitBuilder.SlotIsOpen(EquippableSlotFlags.Chest)) goto end;
								if (option == CoverageFlags.UpperArms && SuitBuilder.SlotIsOpen(EquippableSlotFlags.UpperArms)) goto end;
								if (option == CoverageFlags.LowerArms && SuitBuilder.SlotIsOpen(EquippableSlotFlags.LowerArms)) goto end;
								if (option == CoverageFlags.Abdomen && SuitBuilder.SlotIsOpen(EquippableSlotFlags.Abdomen)) goto end;
								if (option == CoverageFlags.UpperLegs && SuitBuilder.SlotIsOpen(EquippableSlotFlags.UpperLegs)) goto end;
								if (option == CoverageFlags.LowerLegs && SuitBuilder.SlotIsOpen(EquippableSlotFlags.LowerLegs)) goto end;
							}

							Equipment.RemoveAt(i);
						}
						else
						{
							if ((Equipment[i].EquippableSlots.HasFlag(EquippableSlotFlags.LeftRing) || Equipment[i].EquippableSlots.HasFlag(EquippableSlotFlags.RightRing)) && !SuitBuilder.SlotIsOpen(EquippableSlotFlags.LeftRing) && !SuitBuilder.SlotIsOpen(EquippableSlotFlags.RightRing)) { Equipment.RemoveAt(i); goto end; }
							if ((Equipment[i].EquippableSlots.HasFlag(EquippableSlotFlags.LeftBracelet) || Equipment[i].EquippableSlots.HasFlag(EquippableSlotFlags.RightBracelet)) && !SuitBuilder.SlotIsOpen(EquippableSlotFlags.LeftBracelet) && !SuitBuilder.SlotIsOpen(EquippableSlotFlags.RightBracelet)) { Equipment.RemoveAt(i); goto end; }
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
