using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;

namespace Mag_SuitBuilder.Search
{
	class BucketSorter : List<Bucket>
	{
		public void PutItemInBuckets(SuitBuildableMyWorldObject piece)
		{
			foreach (Bucket bucket in this)
			{
				if ((piece.EquippableSlot & bucket.Slot) == bucket.Slot)
					bucket.Add(piece);
			}
		}
	}
}
