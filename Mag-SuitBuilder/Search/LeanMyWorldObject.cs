using System.Collections.Generic;

using Mag.Shared.Constants;
using Mag.Shared.Spells;

using Mag_SuitBuilder.Equipment;

namespace Mag_SuitBuilder.Search
{
	class LeanMyWorldObject
	{
		public readonly ExtendedMyWorldObject ExtendedMyWorldObject;

		public readonly EquippableSlotFlags EquippableSlots;

		public readonly CoverageFlags Coverage;

		public readonly int ItemSetId;


		public readonly int ObjectClass;

		public readonly string Material;

		public readonly int CalcedStartingArmorLevel;


		private readonly int damRating;

		private readonly int damResistRating;

		private readonly int critRating;

		private readonly int critResistRating;

		private readonly int critDamRating;

		private readonly int critDamResistRating;

		private readonly int healBoostRating;

		private readonly int vitalityRating;


		public readonly List<Spell> SpellsToUseInSearch = new List<Spell>();

		public int SpellBitmap;


		public LeanMyWorldObject(ExtendedMyWorldObject myWorldObject)
		{
			ExtendedMyWorldObject = myWorldObject;

			EquippableSlots = myWorldObject.EquippableSlots;

			Coverage = myWorldObject.Coverage;

			ItemSetId = myWorldObject.IntValues.ContainsKey(265) ? myWorldObject.IntValues[265] : 0;


			ObjectClass = myWorldObject.ObjectClass;

			Material = myWorldObject.Material;

			CalcedStartingArmorLevel = myWorldObject.CalcedStartingArmorLevel;


			damRating = myWorldObject.DamRating;

			damResistRating = myWorldObject.DamResistRating;

			critRating = myWorldObject.CritRating;

			critResistRating = myWorldObject.CritResistRating;

			critDamRating = myWorldObject.CritDamRating;

			critDamResistRating = myWorldObject.CritDamResistRating;

			healBoostRating = myWorldObject.HealBoostRating;

			vitalityRating = myWorldObject.VitalityRating;
		}

		/// <summary>
		/// Before you use this function, BuiltItemSearchCache() must have been called on this object.
		/// </summary>
		/// <param name="compareItem"></param>
		/// <returns></returns>
		public bool IsSurpassedBy(LeanMyWorldObject compareItem)
		{
			// Items must be of the same armor set
			if (compareItem.ItemSetId != ItemSetId)
				return false;

			// This checks to see that the compare item covers at least all the slots that the passed item does
			if (compareItem.Coverage.IsBodyArmor() && Coverage.IsBodyArmor())
			{
				if ((compareItem.Coverage & Coverage) != Coverage)
					return false;
			}
			else if ((compareItem.EquippableSlots & EquippableSlots) != EquippableSlots)
				return false;

			// Find the highest level spell on this item
			Spell.CantripLevels highestCantrip = Spell.CantripLevels.None;

			foreach (Spell itemSpell in ExtendedMyWorldObject.CachedSpells)
			{
				if (itemSpell.CantripLevel > highestCantrip)
					highestCantrip = itemSpell.CantripLevel;
			}

			// Does this item have spells that equal or surpass this items at the highest cantrip level found?
			foreach (Spell itemSpell in ExtendedMyWorldObject.CachedSpells)
			{
				if (itemSpell.CantripLevel < highestCantrip)
					continue;

				foreach (Spell compareSpell in compareItem.ExtendedMyWorldObject.CachedSpells)
				{
					if (compareSpell.Surpasses(itemSpell))
						return true;

					if (compareSpell.IsSameOrSurpasses(itemSpell))
						goto next;
				}

				return false;

				next:;
			}

			if (compareItem.CalcedStartingArmorLevel > CalcedStartingArmorLevel)
				return true;

			if (compareItem.damRating > damRating && damRating > 0) return true;
			if (compareItem.damResistRating > damResistRating && damResistRating > 0) return true;
			if (compareItem.critRating > critRating && critRating > 0) return true;
			if (compareItem.critResistRating > critResistRating && critResistRating > 0) return true;
			if (compareItem.critDamRating > critDamRating && critDamRating > 0) return true;
			if (compareItem.critDamResistRating > critDamResistRating && critDamResistRating > 0) return true;
			if (compareItem.healBoostRating > healBoostRating && healBoostRating > 0) return true;
			if (compareItem.vitalityRating > vitalityRating && vitalityRating > 0) return true;

			return false;
		}
	}
}
