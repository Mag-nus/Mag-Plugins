using System.Collections.Generic;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	static class BucketExtensions
	{
		public static bool PutItemInBuckets(this ICollection<Bucket> value, LeanMyWorldObject piece)
		{
			return value.PutItemInBuckets(piece, piece.EquippableSlots);
		}

		public static bool PutItemInBuckets(this ICollection<Bucket> value, LeanMyWorldObject piece, EquipMask slot)
		{
			bool foundBucket = false;

			foreach (Bucket bucket in value)
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
