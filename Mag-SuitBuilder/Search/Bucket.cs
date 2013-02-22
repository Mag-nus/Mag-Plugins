using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;

namespace Mag_SuitBuilder.Search
{
	class Bucket : List<EquipmentPiece>
	{
		public readonly Constants.EquippableSlotFlags Slot;
		public readonly bool IsBodyArmor;

		public Bucket(Constants.EquippableSlotFlags slot)
		{
			Slot = slot;
			IsBodyArmor = (slot & Constants.EquippableSlotFlags.AllBodyArmor) != 0;
		}
	}
}
