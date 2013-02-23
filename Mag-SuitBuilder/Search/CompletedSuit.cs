using System;
using System.Collections;
using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Spells;

using Mag.Shared;

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
	class CompletedSuit : IEnumerable<KeyValuePair<Constants.EquippableSlotFlags, EquipmentPiece>>
	{
		readonly Dictionary<Constants.EquippableSlotFlags, EquipmentPiece> items = new Dictionary<Constants.EquippableSlotFlags, EquipmentPiece>();

		readonly HashSet<EquipmentPiece> piecesHashSet = new HashSet<EquipmentPiece>();

		/// <summary>
		/// Gets the number of equipment pieces in the suit.
		/// </summary>
		public int Count { get { return piecesHashSet.Count; } }

		public int TotalBaseArmorLevel { get; private set; }

		readonly List<Spell> effectiveSpells = new List<Spell>();

		/// <summary>
		/// Gets the effective spells of the suit, meaning, it returns only the best spell for any spell/family covered.
		/// </summary>
		public IEnumerable<Spell> EffectiveSpells { get { return effectiveSpells; } }

		public int TotalEffectiveEpics { get; private set; }
		public int TotalEffectiveMajors { get; private set; }

		readonly Dictionary<ArmorSet, int> armorSetCounts = new Dictionary<ArmorSet, int>();

		/// <exception cref="ArgumentException">Trying to add an item that covers a slot already filled.</exception>
		public void AddItem(Constants.EquippableSlotFlags slots, EquipmentPiece item)
		{
			// Make sure we don't overlap a slot
			foreach (var o in this)
			{
				if ((o.Key & slots) != 0)
					throw new ArgumentException("Do not add items that overlap an existing items covered slot(s).", "slots");
			}

			items.Add(slots, item);
			piecesHashSet.Add(item);

			// This should use the coverage flags instead
			// todo hack fix
			if (item.EquipableSlots.IsBodyArmor())
			{
				if (item.EquipableSlots.IsUnderwear())
					TotalBaseArmorLevel += (item.BaseArmorLevel * item.BodyPartsCovered);
				else
					TotalBaseArmorLevel += item.BaseArmorLevel;
			}

			foreach (Spell itemSpell in item.Spells)
			{
				// Don't count impen as an effective spell
				if (itemSpell.IsOfSameFamilyAndGroup(Spell.GetSpell("Epic Impenetrability")))
					continue;

				foreach (Spell suitSpell in effectiveSpells)
				{
					if (suitSpell.IsSameOrSurpasses(itemSpell))
						goto end;
				}

				effectiveSpells.Add(itemSpell);

				end:;
			}

			TotalEffectiveEpics = 0;
			TotalEffectiveMajors = 0;
			foreach (Spell spell in EffectiveSpells)
			{
				if (spell.CantripLevel >= Spell.CantripLevels.Epic)
					TotalEffectiveEpics++;
				else if (spell.CantripLevel >= Spell.CantripLevels.Major)
					TotalEffectiveMajors++;
			}

			if (item.ArmorSet != ArmorSet.NoArmorSet)
			{
				if (armorSetCounts.ContainsKey(item.ArmorSet))
					armorSetCounts[item.ArmorSet]++;
				else
					armorSetCounts.Add(item.ArmorSet, 1);
			}
		}

		public EquipmentPiece this[Constants.EquippableSlotFlags slot]
		{
			get
			{
				foreach (KeyValuePair<Constants.EquippableSlotFlags, EquipmentPiece> kvp in items)
				{
					if ((kvp.Key & slot) == slot)
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

		public IEnumerator<KeyValuePair<Constants.EquippableSlotFlags, EquipmentPiece>> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override string ToString()
		{
			string sets = null;

			foreach (KeyValuePair<ArmorSet, int> kvp in armorSetCounts)
			{
				if (sets != null)
					sets += ", ";

				sets += kvp.Key + " " + kvp.Value;

			}

			return piecesHashSet.Count + ", AL: " + TotalBaseArmorLevel + ", Epics: " + TotalEffectiveEpics + ", Majors: " + TotalEffectiveMajors + ", " + sets;
		}
	}
}
