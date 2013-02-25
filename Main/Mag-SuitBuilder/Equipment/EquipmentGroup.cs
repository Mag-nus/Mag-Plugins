
using Mag_SuitBuilder.Spells;

using Mag.Shared;

namespace Mag_SuitBuilder.Equipment
{
	class EquipmentGroup : SortableBindingList<SuitBuildableMyWorldObject>
	{
		public bool ItemIsSurpassed(SuitBuildableMyWorldObject item)
		{
			if (item.ObjectClass != (int)ObjectClass.Armor && item.ObjectClass != (int)ObjectClass.Clothing && item.ObjectClass != (int)ObjectClass.Jewelry)
				return false;

			foreach (SuitBuildableMyWorldObject compareItem in this)
			{
				if (compareItem == item)
				{
					// For armor pieces, we can through through the entire list and find the piece with the highest AL.
					if (item.EquippableSlots.IsBodyArmor())
						continue;
					
					// For non-armor pieces, we cannot compare an item against the entire list or we may end up removing
					// the item itself, and then the items it matches.
					// To prevent that, we just do a top down approach
					break;
				}

				if (compareItem.Exclude)
					continue;

				// Items must be of the same armor set
				if (compareItem.ItemSetId != item.ItemSetId)
					continue;

				// This checks to see that the compare item covers at least all the slots that the passed item does
				if (compareItem.Coverage.IsBodyArmor() && item.Coverage.IsBodyArmor())
				{
					if ((compareItem.Coverage & item.Coverage) != item.Coverage)
						continue;
				}
				else if ((compareItem.EquippableSlots & item.EquippableSlots) != item.EquippableSlots)
					continue;

				// Does this item have a spell that the compare item does not?
				{
					bool itemHasSpellSurpasingPreviousItem = false;

					foreach (Spell itemSpell in item.CachedSpells)
					{
						if (itemSpell.CantripLevel < Spell.CantripLevels.Epic)
							continue;

						bool itemSpellSurpasesPreviousItemSpells = false;
						bool spellOfSameFamilyAndGroupFound = false;

						foreach (Spell previousSpell in compareItem.CachedSpells)
						{
							if (itemSpell.Surpasses(previousSpell))
							{
								itemSpellSurpasesPreviousItemSpells = true;
								spellOfSameFamilyAndGroupFound = true;
							}
							else if (itemSpell.IsOfSameFamilyAndGroup(previousSpell))
								spellOfSameFamilyAndGroupFound = true;
						}

						if (itemSpellSurpasesPreviousItemSpells || !spellOfSameFamilyAndGroupFound)
						{
							itemHasSpellSurpasingPreviousItem = true;
							break;
						}
					}

					if (itemHasSpellSurpasingPreviousItem)
						continue;
				}

				// If this item has higher AL, it's not surpassed
				if (compareItem.CalcedStartingArmorLevel < item.CalcedStartingArmorLevel)
					continue;

				return true;
			}

			return false;
		}
	}
}
