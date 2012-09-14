using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;

namespace Mag_SuitBuilder.Search
{
	class BucketSorter : List<Bucket>
	{
		public void PutItemInBuckets(EquipmentPiece piece)
		{
			foreach (Bucket bucket in this)
			{
				if ((piece.EquipableSlots & bucket.Slot) == bucket.Slot)
					bucket.Add(piece);
			}
		}
	}
}
