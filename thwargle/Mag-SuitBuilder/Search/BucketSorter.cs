using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	class BucketSorter : List<Bucket>
	{
		public bool PutItemInBuckets(SuitBuildableMyWorldObject piece)
		{
			return PutItemInBuckets(piece, piece.EquippableSlots);
		}

		public bool PutItemInBuckets(SuitBuildableMyWorldObject piece, EquippableSlotFlags slot)
		{
			bool foundBucket = false;

			foreach (Bucket bucket in this)
			{
				if ((slot & bucket.Slot) == bucket.Slot)
				{
					foundBucket = true;
					bucket.Add(piece);
				}
			}

			return foundBucket;
		}
	}
}
