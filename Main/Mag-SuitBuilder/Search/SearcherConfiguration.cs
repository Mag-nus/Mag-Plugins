using System.Collections.Generic;
using System.Collections.ObjectModel;

using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Spells;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	class SearcherConfiguration
	{
		public SearcherConfiguration()
		{
			CantripsToLookFor = new Collection<Spell>();
		}

		public ICollection<Spell> CantripsToLookFor { get; set; }

		/// <summary>
		/// Armor set Id. 0 = None, 255 = Any
		/// </summary>
		public int PrimaryArmorSet { get; set; }

		/// <summary>
		/// Armor set Id. 0 = None, 255 = Any
		/// </summary>
		public int SecondaryArmorSet { get; set; }


		public bool ItemPassesRules(SuitBuildableMyWorldObject item)
		{
			if (CantripsToLookFor.Count > 0)
			{
				foreach (Spell cantrip in CantripsToLookFor)
				{
					foreach (Spell itemSpell in item.CachedSpells)
					{
						if (itemSpell.IsSameOrSurpasses(cantrip))
							goto end;
					}
				}

				end: ;
			}

			// If we're don't want to use any set pieces, remove them
			if (PrimaryArmorSet == 0 && SecondaryArmorSet == 0 && item.EquippableSlots.IsBodyArmor() && item.ItemSetId != 0)
				return false;

			// If we're building a two set armor suit, and we don't want any blanks or fillers, remove any pieces of armor of other sets
			if (PrimaryArmorSet != 0 && SecondaryArmorSet != 0 && PrimaryArmorSet != 255 && SecondaryArmorSet != 255 &&
				item.EquippableSlots.IsBodyArmor() && item.ItemSetId != PrimaryArmorSet && item.ItemSetId != SecondaryArmorSet)
				return false;

			return true;
		}

		public bool SpellPassesRules(Spell spell)
		{
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
