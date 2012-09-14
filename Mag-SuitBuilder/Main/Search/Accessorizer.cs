using System;
using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;

namespace Mag_SuitBuilder.Search
{
	/// <summary>
	/// The accessorizer takes equipment (underwear, jewelry) and applies them to a base suit pushing out the top permutations.
	/// </summary>
	class Accessorizer : Searcher
	{
		public Accessorizer(SearcherConfiguration config, IEnumerable<EquipmentPiece> accessories, CompletedSuit startingSuit = null) : base(config, accessories, startingSuit)
		{

		}

		//int totalArmorBucketsWithItems;
		//int highestArmorCountSuitBuilt;
		//Dictionary<int, List<int>> highestArmorSuitsBuilt;
		List<CompletedSuit> completedSuits;

		protected override void StartSearch()
		{
			OnSuitCreated(SuitBuilder.CreateCompletedSuit());

			BucketSorter sorter = new BucketSorter();

			// All these slots can have armor
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.Shirt)) sorter.Add(new Bucket(Constants.EquippableSlotFlags.Shirt));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.Pants)) sorter.Add(new Bucket(Constants.EquippableSlotFlags.Pants));

			// All these slots have no armor
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.Necklace))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Necklace));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.RightBracelet))sorter.Add(new Bucket(Constants.EquippableSlotFlags.RightBracelet));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.LeftBracelet))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.LeftBracelet));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.RightRing))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.RightRing));
			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.LeftRing))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.LeftRing));

			if (SuitBuilder.SlotIsOpen(Constants.EquippableSlotFlags.Trinket))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Trinket));

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
				if ((a.Slot & Constants.EquippableSlotFlags.AllBodyArmor) != 0 && (b.Slot & Constants.EquippableSlotFlags.AllBodyArmor) == 0) return -1;
				if ((a.Slot & Constants.EquippableSlotFlags.AllBodyArmor) == 0 && (b.Slot & Constants.EquippableSlotFlags.AllBodyArmor) != 0) return 1;
				return a.Count.CompareTo(b.Count);
			});

			// Reset our variables
			//highestArmorCountSuitBuilt = 0;
			//highestArmorSuitsBuilt = new Dictionary<int, List<int>>();
			//for (int i = 1; i <= sorter.Count; i++)
			//	highestArmorSuitsBuilt.Add(i, new List<int>(10));
			completedSuits = new List<CompletedSuit>();

			// Do the actual search here
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

			// Are we at the end of the line?
			if (buckets.Count <= index)
			{
				/*
				if (totalArmorBucketsWithItems > 0 && index > 0 && buckets[index - 1].IsBodyArmor && SuitBuilder.Count > highestArmorCountSuitBuilt)
					highestArmorCountSuitBuilt = SuitBuilder.Count;

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
				*/
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

			// Only continue to build any suits with a minimum potential of no less than 1 armor pieces less than our largest built suit so far
			//if (SuitBuilder.Count + 1 <= highestArmorCountSuitBuilt - (totalArmorBucketsWithItems - index))
			//	return;

			//for (int i = 0; i < buckets[index].Count ; i++)
			foreach (EquipmentPiece piece in buckets[index]) // Using foreach: 10.85s, for: 11s
			{
				if (SuitBuilder.SlotIsOpen(buckets[index].Slot) && SuitBuilder.CanGetBeneficialSpellFrom(piece))
				{
					SuitBuilder.Push(piece, buckets[index].Slot);

					//if (buckets[index].IsBodyArmor && SuitBuilder.Count > highestArmorCountSuitBuilt)
					//	highestArmorCountSuitBuilt = SuitBuilder.Count;

					SearchThroughBuckets(buckets, index + 1);

					SuitBuilder.Pop();
				}
			}

			SearchThroughBuckets(buckets, index + 1);
		}
	}
}
