using System.Collections.Generic;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	class Bucket : List<LeanMyWorldObject>
	{
		public readonly EquipMask Slot;

		public Bucket(EquipMask slot)
		{
			Slot = slot;
		}
	}
}
