using System;
using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;

namespace Mag_SuitBuilder.Search
{
	/// <summary>
	/// The armor searcher takes body armor and underwear with armor and applies them to a base suit pushing out the top permutations.
	/// </summary>
	internal class ArmorSearcher : Searcher
	{
		public ArmorSearcher(SearcherConfiguration config, IEnumerable<EquipmentPiece> pieces, CompletedSuit startingSuit = null) : base(config, pieces, startingSuit)
		{
			// Sort the list with the highest armor first
			Equipment.Sort((a, b) => b.BaseArmorLevel.CompareTo(a.BaseArmorLevel));

			// Remove any pieces that have no armor
			for (int i = Equipment.Count - 1 ; i >= 0 ; i--)
			{
				if (Equipment[i].BaseArmorLevel == 0)
					Equipment.RemoveAt(i);
			}
		}

		int totalArmorBucketsWithItems;
		int highestArmorCountSuitBuilt;
		Dictionary<int, List<int>> highestArmorSuitsBuilt;
		List<CompletedSuit> completedSuits;

		protected override void StartSearch()
		{
			BucketSorter sorter = new BucketSorter();

			// All these slots can have armor
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.Head))			sorter.Add(new Bucket(Constants.EquippableSlotFlags.Head));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.Hands))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Hands));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.Feet))			sorter.Add(new Bucket(Constants.EquippableSlotFlags.Feet));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.Chest))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Chest));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.Abdomen))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Abdomen));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.UpperArms))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.UpperArms));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.LowerArms))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.LowerArms));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.UpperLegs))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.UpperLegs));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.LowerLegs))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.LowerLegs));

			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.Shirt)) sorter.Add(new Bucket(Constants.EquippableSlotFlags.Shirt));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.Pants)) sorter.Add(new Bucket(Constants.EquippableSlotFlags.Pants));
		
			// Put all of our inventory into its appropriate bucket
			foreach (EquipmentPiece piece in Equipment)
				sorter.PutItemInBuckets(piece);

			// Remove any empty buckets
			for (int i = sorter.Count - 1; i >= 0; i--)
			{
				if (sorter[i].Count == 0)
					sorter.RemoveAt(i);
			}

			// We should sort the buckets based on number of items, least amount first, with all armor buckets first
			sorter.Sort((a, b) =>
			{
				if (a.Slot.IsBodyArmor() && !b.Slot.IsBodyArmor()) return -1;
				if (!a.Slot.IsBodyArmor() && b.Slot.IsBodyArmor()) return 1;
				return a.Count.CompareTo(b.Count);
			});

			// Calculate the total number of armor buckets we have with pieces in them.
			totalArmorBucketsWithItems = 0;
			foreach (Bucket bucket in sorter)
			{
				if (bucket.Slot.IsBodyArmor())
					totalArmorBucketsWithItems++;
			}

			// Reset our variables
			highestArmorCountSuitBuilt = 0;
			highestArmorSuitsBuilt = new Dictionary<int, List<int>>();
			for (int i = 1; i <= 17; i++)
				highestArmorSuitsBuilt.Add(i, new List<int>(5));
			completedSuits = new List<CompletedSuit>();

			// Do the actual search here
			if (sorter.Count > 0)
				SearchThroughBuckets(sorter, 0);

			// If we're not running, the search was stopped before it could complete
			if (!Running)
				return;

			Stop();

			OnSearchCompleted();
		}

		void SearchThroughBuckets(List<Bucket> buckets, int index)
		{
			if (!Running)
				return;

			// Only continue to build any suits with a minimum potential of no less than 1 armor pieces less than our largest built suit so far
			if (SuitBuilder.Count + 1 < highestArmorCountSuitBuilt - (totalArmorBucketsWithItems - Math.Min(index, totalArmorBucketsWithItems)))
				return;

			// Are we at the end of the line?
			if (buckets.Count <= index)
			{
				if (SuitBuilder.Count == 0)
					return;

				if (SuitBuilder.TotalBodyArmorPieces > highestArmorCountSuitBuilt)
					highestArmorCountSuitBuilt = SuitBuilder.TotalBodyArmorPieces;

				// We should keep track of the highest AL suits we built for every number of armor count suits built, and only push out ones that fall within our top X
				List<int> list = highestArmorSuitsBuilt[SuitBuilder.Count];
				if (list.Count < list.Capacity)
				{
					if (!list.Contains(SuitBuilder.TotalBaseArmorLevel))
					{
						list.Add(SuitBuilder.TotalBaseArmorLevel);

						if (list.Count == list.Capacity)
							list.Sort();
					}
				}
				else
				{
					if (list[list.Count - 1] > SuitBuilder.TotalBaseArmorLevel)
						return;

					if (list[list.Count - 1] < SuitBuilder.TotalBaseArmorLevel && !list.Contains(SuitBuilder.TotalBaseArmorLevel))
					{
						list[list.Count - 1] = SuitBuilder.TotalBaseArmorLevel;
						list.Sort();
					}
				}

				CompletedSuit newSuit = SuitBuilder.CreateCompletedSuit();

				// We should also keep track of all the suits we've built and make sure we don't push out a suit with the same exact pieces in swapped slots
				foreach (CompletedSuit suit in completedSuits)
				{
					if (newSuit.IsSubsetOf(suit))
						return;
				}
				completedSuits.Add(newSuit);

				OnSuitCreated(newSuit);

				return;
			}

			foreach (EquipmentPiece piece in buckets[index])
			{
				if (SuitBuilder.SlotIsOpen(buckets[index].Slot) && (!piece.EquipableSlots.IsBodyArmor() ||  SuitBuilder.HasRoomForArmorSet(Config.PrimaryArmorSet, Config.SecondaryArmorSet, piece.ArmorSet)) && SuitBuilder.CanGetBeneficialSpellFrom(piece))
				{
					SuitBuilder.Push(piece, buckets[index].Slot);

					SearchThroughBuckets(buckets, index + 1);

					SuitBuilder.Pop();
				}
			}

			SearchThroughBuckets(buckets, index + 1);
		}
	}
}
