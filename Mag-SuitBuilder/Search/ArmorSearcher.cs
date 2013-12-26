using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mag_SuitBuilder.Equipment;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	/// <summary>
	/// The armor searcher equipment (armor/clothes/underwear) with armor and applies them to a base suit pushing out the top permutations.
	/// </summary>
	internal class ArmorSearcher : Searcher
	{
		public ArmorSearcher(SearcherConfiguration config, IEnumerable<SuitBuildableMyWorldObject> pieces, CompletedSuit startingSuit = null) : base(config, pieces, startingSuit)
		{
			// Sort the list with the highest armor first
			Equipment.Sort((a, b) => b.CalcedStartingArmorLevel.CompareTo(a.CalcedStartingArmorLevel));

			// Remove any pieces that have no armor
			for (int i = Equipment.Count - 1 ; i >= 0 ; i--)
			{
				if (Equipment[i].CalcedStartingArmorLevel <= 0)
					Equipment.RemoveAt(i);
			}
		}

		BucketSorter buckets;

		int totalArmorBucketsWithItems;
		int highestArmorCountSuitBuilt;
		Dictionary<int, List<int>> highestArmorSuitsBuilt;
		List<CompletedSuit> completedSuits;

		protected override void StartSearch()
		{
			buckets = new BucketSorter();

			// All these slots can have armor
			if (SuitBuilder.SlotIsOpen(EquippableSlotFlags.Head))		buckets.Add(new Bucket(EquippableSlotFlags.Head));
			if (SuitBuilder.SlotIsOpen(EquippableSlotFlags.Hands))		buckets.Add(new Bucket(EquippableSlotFlags.Hands));
			if (SuitBuilder.SlotIsOpen(EquippableSlotFlags.Feet))		buckets.Add(new Bucket(EquippableSlotFlags.Feet));
			if (SuitBuilder.SlotIsOpen(EquippableSlotFlags.Chest))		buckets.Add(new Bucket(EquippableSlotFlags.Chest));
			if (SuitBuilder.SlotIsOpen(EquippableSlotFlags.Abdomen))	buckets.Add(new Bucket(EquippableSlotFlags.Abdomen));
			if (SuitBuilder.SlotIsOpen(EquippableSlotFlags.UpperArms))	buckets.Add(new Bucket(EquippableSlotFlags.UpperArms));
			if (SuitBuilder.SlotIsOpen(EquippableSlotFlags.LowerArms))	buckets.Add(new Bucket(EquippableSlotFlags.LowerArms));
			if (SuitBuilder.SlotIsOpen(EquippableSlotFlags.UpperLegs))	buckets.Add(new Bucket(EquippableSlotFlags.UpperLegs));
			if (SuitBuilder.SlotIsOpen(EquippableSlotFlags.LowerLegs))	buckets.Add(new Bucket(EquippableSlotFlags.LowerLegs));

			if (SuitBuilder.SlotIsOpen(EquippableSlotFlags.ShirtChest))		buckets.Add(new Bucket(EquippableSlotFlags.ShirtChest));
			if (SuitBuilder.SlotIsOpen(EquippableSlotFlags.PantsUpperLegs)) buckets.Add(new Bucket(EquippableSlotFlags.PantsUpperLegs));
		
			// Put all of our inventory into its appropriate bucket
			foreach (var piece in Equipment)
			{
				if (piece.EquippableSlots == (EquippableSlotFlags.PantsLowerLegs | EquippableSlotFlags.Feet)) // Some shoes cover both feet/lower legs but can only go in the feet slot
					buckets.PutItemInBuckets(piece, EquippableSlotFlags.Feet);
				else if (piece.EquippableSlots.IsBodyArmor() && piece.EquippableSlots.GetTotalBitsSet() != piece.Coverage.GetTotalBitsSet())
					MessageBox.Show("Unable to add " + piece + " into an appropriate bucket. EquippableSlots != Coverage" + Environment.NewLine + "EquippableSlots: " + piece.EquippableSlots + Environment.NewLine + "Coverage: " + piece.Coverage);
				else if (piece.EquippableSlots.IsBodyArmor() && piece.EquippableSlots.GetTotalBitsSet() > 1)
				{
					if (piece.Material == null) // Can't reduce non-loot gen pieces
						buckets.PutItemInBuckets(piece);
					else
					{
						// Lets try to reduce this
						foreach (var option in piece.Coverage.ReductionOptions())
						{
							if (option == CoverageFlags.Head)			buckets.PutItemInBuckets(piece, EquippableSlotFlags.Head);
							else if (option == CoverageFlags.Chest)		buckets.PutItemInBuckets(piece, EquippableSlotFlags.Chest);
							else if (option == CoverageFlags.UpperArms) buckets.PutItemInBuckets(piece, EquippableSlotFlags.UpperArms);
							else if (option == CoverageFlags.LowerArms) buckets.PutItemInBuckets(piece, EquippableSlotFlags.LowerArms);
							else if (option == CoverageFlags.Hands)		buckets.PutItemInBuckets(piece, EquippableSlotFlags.Hands);
							else if (option == CoverageFlags.Abdomen)	buckets.PutItemInBuckets(piece, EquippableSlotFlags.Abdomen);
							else if (option == CoverageFlags.UpperLegs) buckets.PutItemInBuckets(piece, EquippableSlotFlags.UpperLegs);
							else if (option == CoverageFlags.LowerLegs) buckets.PutItemInBuckets(piece, EquippableSlotFlags.LowerLegs);
							else if (option == CoverageFlags.Feet)		buckets.PutItemInBuckets(piece, EquippableSlotFlags.Feet);
							else
								MessageBox.Show("Unable to add " + piece + " into an appropriate bucket." + Environment.NewLine + "Reduction coverage option of " + option + " not expected.");
						}
					}
				}
				else
					buckets.PutItemInBuckets(piece);
			}

			// Remove any empty buckets
			for (int i = buckets.Count - 1; i >= 0; i--)
			{
				if (buckets[i].Count == 0)
					buckets.RemoveAt(i);
			}

			// We should sort the buckets based on number of items, least amount first, with all armor buckets first
			buckets.Sort((a, b) =>
			{
				if (a.Slot.IsBodyArmor() && !b.Slot.IsBodyArmor()) return -1;
				if (!a.Slot.IsBodyArmor() && b.Slot.IsBodyArmor()) return 1;
				return a.Count.CompareTo(b.Count);
			});

			// Calculate the total number of armor buckets we have with pieces in them.
			totalArmorBucketsWithItems = 0;
			foreach (Bucket bucket in buckets)
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
			if (buckets.Count > 0)
				SearchThroughBuckets(SuitBuilder.Clone(), 0);

			// If we're not running, the search was stopped before it could complete
			if (!Running)
				return;

			Stop();

			OnSearchCompleted();
		}

		object lockObject = new object();

		void SearchThroughBuckets(SuitBuilder builder, int index)
		{
			if (!Running)
				return;

			// Only continue to build any suits with a minimum potential of no less than 1 armor pieces less than our largest built suit so far
			if (builder.Count + 1 < highestArmorCountSuitBuilt - (totalArmorBucketsWithItems - Math.Min(index, totalArmorBucketsWithItems)))
				return;

			// Are we at the end of the line?
			if (buckets.Count <= index)
			{
				if (builder.Count == 0)
					return;

				lock (lockObject)
				{
					if (builder.TotalBodyArmorPieces > highestArmorCountSuitBuilt)
						highestArmorCountSuitBuilt = builder.TotalBodyArmorPieces;

					// We should keep track of the highest AL suits we built for every number of armor count suits built, and only push out ones that fall within our top X
					List<int> list = highestArmorSuitsBuilt[builder.Count];
					if (list.Count < list.Capacity)
					{
						if (!list.Contains(builder.TotalBaseArmorLevel))
						{
							list.Add(builder.TotalBaseArmorLevel);

							if (list.Count == list.Capacity)
								list.Sort();
						}
					}
					else
					{
						if (list[list.Count - 1] > builder.TotalBaseArmorLevel)
							return;

						if (list[list.Count - 1] < builder.TotalBaseArmorLevel && !list.Contains(builder.TotalBaseArmorLevel))
						{
							list[list.Count - 1] = builder.TotalBaseArmorLevel;
							list.Sort();
						}
					}

					CompletedSuit newSuit = builder.CreateCompletedSuit();

					// We should also keep track of all the suits we've built and make sure we don't push out a suit with the same exact pieces in swapped slots
					foreach (CompletedSuit suit in completedSuits)
					{
						if (newSuit.IsSubsetOf(suit))
							return;
					}
					completedSuits.Add(newSuit);

					OnSuitCreated(newSuit);
				}

				return;
			}

			if (index == 0) // If this is the first bucket we're searching through, multi-thread the subsearches
			{
				Parallel.ForEach(buckets[index], piece =>
				{
					SuitBuilder clone = builder.Clone();

					if (clone.SlotIsOpen(buckets[index].Slot) && (!piece.EquippableSlots.IsBodyArmor() || clone.HasRoomForArmorSet(Config.PrimaryArmorSet, Config.SecondaryArmorSet, piece.ItemSetId)) && clone.CanGetBeneficialSpellFrom(piece))
					{
						clone.Push(piece, buckets[index].Slot);

						SearchThroughBuckets(clone, index + 1);

						clone.Pop();
					}
				});
			}
			else
			{
				foreach (SuitBuildableMyWorldObject piece in buckets[index])
				{
					if (builder.SlotIsOpen(buckets[index].Slot) && (!piece.EquippableSlots.IsBodyArmor() || builder.HasRoomForArmorSet(Config.PrimaryArmorSet, Config.SecondaryArmorSet, piece.ItemSetId)) && builder.CanGetBeneficialSpellFrom(piece))
					{
						builder.Push(piece, buckets[index].Slot);

						SearchThroughBuckets(builder, index + 1);

						builder.Pop();
					}
				}
			}

			SearchThroughBuckets(builder, index + 1);
		}
	}
}
