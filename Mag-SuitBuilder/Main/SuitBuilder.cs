
namespace Mag_SuitBuilder
{
	class SuitBuilder : EquipmentGroup
	{
		public bool SlotIsOpen(Constants.EquippableSlotFlags slot)
		{
			Constants.EquippableSlotFlags equippedSlots = Constants.EquippableSlotFlags.None;

			foreach (EquipmentPiece piece in this)
				equippedSlots |= piece.EquipableSlots;

			return ((equippedSlots & slot) == 0);
		}

		public bool CanAdd(EquipmentPiece item, Constants.EquippableSlotFlags forceSlot = Constants.EquippableSlotFlags.All)
		{
			Constants.EquippableSlotFlags equippedSlots = Constants.EquippableSlotFlags.None;

			foreach (EquipmentPiece piece in this)
				equippedSlots |= piece.EquipableSlots;

			return (equippedSlots & (item.EquipableSlots & forceSlot)) == 0;
		}

		public bool CanOfferBeneficialSpell(EquipmentPiece item)
		{
			foreach (Spell itemSpell in item.Spells)
			{
				foreach (EquipmentPiece piece in this)
				{
					foreach (Spell pieceSpell in piece.Spells)
					{
						if (pieceSpell.Surpasses(itemSpell))
							goto end;
					}
				}

				return true;

				end:;
			}

			return false;
		}
	}
}
