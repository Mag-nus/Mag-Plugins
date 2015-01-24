using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	class Bucket : List<SuitBuildableMyWorldObject>
	{
		public readonly EquippableSlotFlags Slot;

		public Bucket(EquippableSlotFlags slot)
		{
			Slot = slot;
		}
	}
}
