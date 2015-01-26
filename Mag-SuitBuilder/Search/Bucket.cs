using System.Collections.Generic;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	class Bucket : List<LeanMyWorldObject>
	{
		public readonly EquippableSlotFlags Slot;

		public Bucket(EquippableSlotFlags slot)
		{
			Slot = slot;
		}
	}
}
