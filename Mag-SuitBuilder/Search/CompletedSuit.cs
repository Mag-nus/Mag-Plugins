using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Mag.Shared.Constants;
using Mag.Shared.Spells;

namespace Mag_SuitBuilder.Search
{
	/// <summary>
	/// Use this class to create a finished suit of equipment.
	/// Add all your equipment using AddItem().
	/// You can add multi-coverage pieces, just don't overlap the same slot.
	/// This class also gives you a fast way to check if your completed suits are subset or superset of other completed suits.
	/// If you want to find the piece that covering slot X, use the indexer [].
	/// Enumerate the class if you want to get a list of all items.
	/// When enumerating, keep in mind that SlotFlags could have one or more slot bits set for any piece.
	/// No pieces should have overlapping slots.
	/// </summary>
	class CompletedSuit : IEnumerable<KeyValuePair<EquipMask, LeanMyWorldObject>>
	{
		readonly Dictionary<EquipMask, LeanMyWorldObject> items = new Dictionary<EquipMask, LeanMyWorldObject>();

		readonly HashSet<LeanMyWorldObject> piecesHashSet = new HashSet<LeanMyWorldObject>();

		/// <summary>
		/// Gets the number of equipment pieces in the suit.
		/// </summary>
		public int Count { get { return piecesHashSet.Count; } }

		public int TotalBaseArmorLevel { get; private set; }

		readonly List<Spell> effectiveSpells = new List<Spell>();

		public int TotalEffectiveLegendaries { get; private set; }
		public int TotalEffectiveEpics { get; private set; }
		public int TotalEffectiveMajors { get; private set; }

		readonly Dictionary<int, int> armorSetCounts = new Dictionary<int, int>();

		/// <summary>
		/// This will try to add the item to the suit
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Trying to add an item that covers a slot already filled.</exception>
		public bool AddItem(LeanMyWorldObject item)
		{
			EquipMask slotToAddTo = item.EquippableSlots;

			if (item.EquippableSlots == (EquipMask.FootWear | EquipMask.LowerLegWear)) // Some armor boots
				slotToAddTo = EquipMask.FootWear;
			else if (item.EquippableSlots == (EquipMask.WristWearLeft | EquipMask.WristWearRight))
			{
				if (this[EquipMask.WristWearLeft] == null)
					slotToAddTo = EquipMask.WristWearLeft;
				else if (this[EquipMask.WristWearRight] == null)
					slotToAddTo = EquipMask.WristWearRight;
				else
					return false;
			}
			else if (item.EquippableSlots == (EquipMask.FingerWearLeft | EquipMask.FingerWearRight))
			{
				if (this[EquipMask.FingerWearLeft] == null)
					slotToAddTo = EquipMask.FingerWearLeft;
				else if (this[EquipMask.FingerWearRight] == null)
					slotToAddTo = EquipMask.FingerWearRight;
				else
					return false;
			}
			else if (item.EquippableSlots.IsShirt())
				slotToAddTo = EquipMask.ChestWear;
			else if (item.EquippableSlots.IsPants())
				slotToAddTo = EquipMask.UpperLegWear;

			if (this[slotToAddTo] != null)
				return false;

			AddItem(slotToAddTo, item);

			return true;
		}

		/// <exception cref="ArgumentException">Trying to add an item that covers a slot already filled.</exception>
		public void AddItem(EquipMask slots, LeanMyWorldObject item)
		{
			// Make sure we don't overlap a slot
			foreach (var o in this)
			{
				if ((o.Key & slots) != 0)
					throw new ArgumentException("Do not add items that overlap an existing items covered slot(s).", "slots");
			}

			items.Add(slots, item);
			piecesHashSet.Add(item);

			if (item.CalcedStartingArmorLevel > 0)
				TotalBaseArmorLevel += (item.CalcedStartingArmorLevel * slots.GetTotalBitsSet());

			foreach (Spell itemSpell in item.SpellsToUseInSearch)
			{
				foreach (Spell suitSpell in effectiveSpells)
				{
					if (suitSpell.IsSameOrSurpasses(itemSpell))
						goto end;
				}

				effectiveSpells.Add(itemSpell);

				end:;
			}

			TotalEffectiveLegendaries = 0;
			TotalEffectiveEpics = 0;
			TotalEffectiveMajors = 0;
			foreach (Spell spell in effectiveSpells)
			{
				if (spell.CantripLevel >= Spell.CantripLevels.Legendary)
					TotalEffectiveLegendaries++;
				else if (spell.CantripLevel >= Spell.CantripLevels.Epic)
					TotalEffectiveEpics++;
				else if (spell.CantripLevel >= Spell.CantripLevels.Major)
					TotalEffectiveMajors++;
			}

			if (item.ItemSetId != 0)
			{
				if (armorSetCounts.ContainsKey(item.ItemSetId))
					armorSetCounts[item.ItemSetId]++;
				else
					armorSetCounts.Add(item.ItemSetId, 1);
			}
		}

		public LeanMyWorldObject this[EquipMask slot]
		{
			get
			{
				foreach (KeyValuePair<EquipMask, LeanMyWorldObject> kvp in items)
				{
					if ((kvp.Key & slot) != 0)
						return kvp.Value;
				}

				return null;
			}
		}

		public bool IsProperSubsetOf(CompletedSuit other)
		{
			return piecesHashSet.IsProperSubsetOf(other.piecesHashSet);
		}

		public bool IsProperSupersetOf(CompletedSuit other)
		{
			return piecesHashSet.IsProperSupersetOf(other.piecesHashSet);
		}

		public bool IsSubsetOf(CompletedSuit other)
		{
			return piecesHashSet.IsSubsetOf(other.piecesHashSet);
		}

		public bool IsSupersetOf(CompletedSuit other)
		{
			return piecesHashSet.IsSupersetOf(other.piecesHashSet);
		}

		public IEnumerator<KeyValuePair<EquipMask, LeanMyWorldObject>> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override string ToString()
		{
			Dictionary<string, int> armorSets = new Dictionary<string, int>();

			foreach (KeyValuePair<int, int> kvp in armorSetCounts)
			{
				if (Dictionaries.AttributeSetInfo.ContainsKey(kvp.Key))
					armorSets.Add(Dictionaries.AttributeSetInfo[kvp.Key], kvp.Value);
				else
					armorSets.Add("Id:" + kvp.Key, kvp.Value);
			}

			armorSets = (from entry in armorSets orderby entry.Value descending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

			string sets = null;

			foreach (var kvp in armorSets)
			{
				if (sets != null)
					sets += ", ";

				sets += kvp.Key + " " + kvp.Value;

			}

			return piecesHashSet.Count + ", AL: " + TotalBaseArmorLevel + ", [L/E/M]: [" + TotalEffectiveLegendaries + "/" + TotalEffectiveEpics + "/" + TotalEffectiveMajors + "] " + sets;
		}
	}
}
