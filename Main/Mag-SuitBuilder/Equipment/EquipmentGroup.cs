
using System.ComponentModel;
using Mag_SuitBuilder.Spells;

using Mag.Shared;

namespace Mag_SuitBuilder.Equipment
{
	class EquipmentGroup : SortableBindingList<SuitBuildableMyWorldObject>
	{
		public bool ItemIsSurpassed(SuitBuildableMyWorldObject item)
		{
			foreach (SuitBuildableMyWorldObject compareItem in this)
			{/*
				if (compareItem == item)
				{
					// For armor pieces, we can through through the entire list and find the piece with the highest AL.
					if ((item.EquipableSlots & EquippableSlotFlags.AllBodyArmor) != 0)
						continue;
					
					// For non-armor pieces, we cannot compare an item against the entire list or we may end up removing
					// the item itself, and then the items it matches.
					// To prevent that, we just do a top down approach
					break;
				}
				*/
				// Items must be of the same armor set
				if (compareItem.ItemSet != item.ItemSet)
					continue;

				// This checks to see that the compare item covers at least all the slots that the passed item does
				long compareSlots = 0;
				if (compareItem.LongValues.ContainsKey(10)) compareSlots = compareItem.LongValues[10];
				long itemSlots = 0;
				if (item.LongValues.ContainsKey(10)) itemSlots = item.LongValues[10];
				if ((compareSlots & itemSlots) != itemSlots)
					continue;
				/*
				// Does this item have a spell that the compare item does not?
				{
					bool itemHasSpellSurpasingPreviousItem = false;

					foreach (Spell itemSpell in item.Spells)
					{
						bool itemSpellSurpasesPreviousItemSpells = false;
						bool spellOfSameFamilyAndGroupFound = false;

						foreach (Spell previousSpell in compareItem.Spells)
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
				*/
				// If this item has higher AL, it's not surpassed
				if (compareItem.CalcedStartingArmorLevel > 0 && item.CalcedStartingArmorLevel > 0 && compareItem.CalcedStartingArmorLevel < item.CalcedStartingArmorLevel)
					continue;

				return true;
			}

			return false;
		}
	}
}
