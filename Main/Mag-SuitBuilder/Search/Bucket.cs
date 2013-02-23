using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;

using Mag.Shared;

namespace Mag_SuitBuilder.Search
{
	class Bucket : List<SuitBuildableMyWorldObject>
	{
		public readonly EquippableSlotFlags Slot;
		public readonly bool IsBodyArmor;

		public Bucket(EquippableSlotFlags slot)
		{
			Slot = slot;
			IsBodyArmor = slot.IsBodyArmor();
		}
	}
}
