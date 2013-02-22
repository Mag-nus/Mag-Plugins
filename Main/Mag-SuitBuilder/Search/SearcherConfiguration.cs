using System.Collections.Generic;
using System.Collections.ObjectModel;

using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Spells;

namespace Mag_SuitBuilder.Search
{
	class SearcherConfiguration
	{
		public SearcherConfiguration()
		{
			CantripsToLookFor = new Collection<Spell>();
		}


		/// <summary>
		/// This should be the minimum untinked, unbuffed (but including minor/major/epic impen) armor level.
		/// </summary>
		public int MinimumArmorLevelPerPiece { get; set; }

		public ICollection<Spell> CantripsToLookFor { get; set; }

		public ArmorSet PrimaryArmorSet { get; set; }

		public ArmorSet SecondaryArmorSet { get; set; }

		public bool OnlyAddPiecesWithArmor { get; set; }


		public bool ItemPassesRules(EquipmentPiece item)
		{
			if (MinimumArmorLevelPerPiece > 0 && item.EquipableSlots.IsBodyArmor() && item.BaseArmorLevel != 0 && MinimumArmorLevelPerPiece > item.BaseArmorLevel)
				return false;

			if (CantripsToLookFor.Count > 0)
			{
				bool found = false;

				foreach (Spell cantrip in CantripsToLookFor)
				{
					foreach (Spell itemSpell in item.Spells)
					{
						if (itemSpell.IsSameOrSurpasses(cantrip))
						{
							found = true;
							break;
						}
					}

					if (found)
						break;
				}

				if (!found)
					return false;
			}

			// If we're don't want to use any set pieces, remove them
			if (PrimaryArmorSet == ArmorSet.NoArmorSet && SecondaryArmorSet == ArmorSet.NoArmorSet &&
				(item.EquipableSlots & Constants.EquippableSlotFlags.AllBodyArmor) != 0 && item.ArmorSet != ArmorSet.NoArmorSet)
				return false;
			// If we're building a two set armor suit, and we don't want any blanks or fillers, remove any pieces of armor of other sets
			if (PrimaryArmorSet != ArmorSet.NoArmorSet && SecondaryArmorSet != ArmorSet.NoArmorSet &&
				PrimaryArmorSet != ArmorSet.AnyArmorSet && SecondaryArmorSet != ArmorSet.AnyArmorSet &&
				(item.EquipableSlots & Constants.EquippableSlotFlags.AllBodyArmor) != 0 &&
				item.ArmorSet != PrimaryArmorSet && item.ArmorSet != SecondaryArmorSet)
				return false;

			// Check to see if we only want pieces with armor
			if (OnlyAddPiecesWithArmor && item.BaseArmorLevel == 0)
				return false;

			return true;
		}

		public bool SpellPassesRules(Spell spell)
		{
			// If this spell is not a cantrip, or its an impen, it dosn't pass the search rules.
			if (spell.CantripLevel == Spell.CantripLevels.None || spell.IsOfSameFamilyAndGroup(Spell.GetSpell("Epic Impenetrability")))
				return false;

			if (CantripsToLookFor.Count == 0)
				return true;

			foreach (Spell cantrip in CantripsToLookFor)
			{
				if (spell.IsSameOrSurpasses(cantrip))
					return true;
			}

			return false;
		}
	}
}
