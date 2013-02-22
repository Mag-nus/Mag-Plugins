
using Mag_SuitBuilder.Spells;

namespace Mag_SuitBuilder.Equipment
{
	class EquipmentGroup : SortableBindingList<EquipmentPiece>
	{
		public bool ItemIsSurpassed(EquipmentPiece item)
		{
			foreach (EquipmentPiece compareItem in this)
			{
				if (compareItem == item)
				{
					// For armor pieces, we can through through the entire list and find the piece with the highest AL.
					if ((item.EquipableSlots & Constants.EquippableSlotFlags.AllBodyArmor) != 0)
						continue;
					
					// For non-armor pieces, we cannot compare an item against the entire list or we may end up removing
					// the item itself, and then the items it matches.
					// To prevent that, we just do a top down approach
					break;
				}

				// Items must be of the same armor set
				if (compareItem.ArmorSet != item.ArmorSet)
					continue;

				// This checks to see that the compare item covers at least all the slots that the passed item does
				if ((compareItem.EquipableSlots & item.EquipableSlots) != item.EquipableSlots)
					continue;

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

				// If this item has higher AL, it's not surpassed
				if (compareItem.BaseArmorLevel != 0 && item.BaseArmorLevel != 0 && compareItem.BaseArmorLevel < item.BaseArmorLevel)
					continue;

				return true;
			}

			return false;
		}
	}
}
