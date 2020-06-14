using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	/// <summary>
	/// The armor searcher equipment (armor/clothes/underwear) with armor and applies them to a base suit pushing out the top permutations.
	/// </summary>
	internal class ArmorSearcher : Searcher
	{
		public ArmorSearcher(SearcherConfiguration config, IEnumerable<LeanMyWorldObject> pieces, CompletedSuit startingSuit = null) : base(config, pieces, startingSuit)
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

		List<Bucket> buckets;

		int totalArmorBucketsWithItems;
		int highestArmorCountSuitBuilt;
		Dictionary<int, List<int>> highestArmorSuitsBuilt;
		List<CompletedSuit> completedSuits;

		protected override void StartSearch()
		{
			buckets = new List<Bucket>();

			// All these slots can have armor
			if (SuitBuilder.SlotIsOpen(EquipMask.HeadWear))			buckets.Add(new Bucket(EquipMask.HeadWear));
			if (SuitBuilder.SlotIsOpen(EquipMask.HandWear))			buckets.Add(new Bucket(EquipMask.HandWear));
			if (SuitBuilder.SlotIsOpen(EquipMask.FootWear))			buckets.Add(new Bucket(EquipMask.FootWear));
			if (SuitBuilder.SlotIsOpen(EquipMask.ChestArmor))		buckets.Add(new Bucket(EquipMask.ChestArmor));
			if (SuitBuilder.SlotIsOpen(EquipMask.AbdomenArmor))		buckets.Add(new Bucket(EquipMask.AbdomenArmor));
			if (SuitBuilder.SlotIsOpen(EquipMask.UpperArmArmor))	buckets.Add(new Bucket(EquipMask.UpperArmArmor));
			if (SuitBuilder.SlotIsOpen(EquipMask.LowerArmArmor))	buckets.Add(new Bucket(EquipMask.LowerArmArmor));
			if (SuitBuilder.SlotIsOpen(EquipMask.UpperLegArmor))	buckets.Add(new Bucket(EquipMask.UpperLegArmor));
			if (SuitBuilder.SlotIsOpen(EquipMask.LowerLegArmor))	buckets.Add(new Bucket(EquipMask.LowerLegArmor));

			if (SuitBuilder.SlotIsOpen(EquipMask.ChestWear))		buckets.Add(new Bucket(EquipMask.ChestWear));
			if (SuitBuilder.SlotIsOpen(EquipMask.UpperLegWear))		buckets.Add(new Bucket(EquipMask.UpperLegWear));
		
			// Put all of our inventory into its appropriate bucket
			foreach (var piece in Equipment)
			{
				if (piece.EquippableSlots == (EquipMask.LowerLegWear | EquipMask.FootWear)) // Some shoes cover both feet/lower legs but can only go in the feet slot
					buckets.PutItemInBuckets(piece, EquipMask.FootWear);
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
							if (option == CoverageMask.Head)					buckets.PutItemInBuckets(piece, EquipMask.HeadWear);
							else if (option == CoverageMask.OuterwearChest)		buckets.PutItemInBuckets(piece, EquipMask.ChestArmor);
							else if (option == CoverageMask.OuterwearUpperArms) buckets.PutItemInBuckets(piece, EquipMask.UpperArmArmor);
							else if (option == CoverageMask.OuterwearLowerArms) buckets.PutItemInBuckets(piece, EquipMask.LowerArmArmor);
							else if (option == CoverageMask.Hands)				buckets.PutItemInBuckets(piece, EquipMask.HandWear);
							else if (option == CoverageMask.OuterwearAbdomen)	buckets.PutItemInBuckets(piece, EquipMask.AbdomenArmor);
							else if (option == CoverageMask.OuterwearUpperLegs) buckets.PutItemInBuckets(piece, EquipMask.UpperLegArmor);
							else if (option == CoverageMask.OuterwearLowerLegs) buckets.PutItemInBuckets(piece, EquipMask.LowerLegArmor);
							else if (option == CoverageMask.Feet)				buckets.PutItemInBuckets(piece, EquipMask.FootWear);
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

					if (clone.SlotIsOpen(buckets[index].Slot))
					{
						if ((!piece.EquippableSlots.IsBodyArmor() || clone.HasRoomForArmorSet(Config.PrimaryArmorSet, Config.SecondaryArmorSet, piece.ItemSetId)) && clone.CanGetBeneficialSpellFrom(piece))
						{
							clone.Push(piece, buckets[index].Slot);

							SearchThroughBuckets(clone, index + 1);

							clone.Pop();
						}
					}
				});
			}
			else
			{
				if (builder.SlotIsOpen(buckets[index].Slot))
				{
					foreach (var piece in buckets[index])
					{
						if (builder.CanGetBeneficialSpellFrom(piece) && (!piece.EquippableSlots.IsBodyArmor() || builder.HasRoomForArmorSet(Config.PrimaryArmorSet, Config.SecondaryArmorSet, piece.ItemSetId)))
						{
							builder.Push(piece, buckets[index].Slot);

							SearchThroughBuckets(builder, index + 1);

							builder.Pop();
						}
					}
				}
			}

			SearchThroughBuckets(builder, index + 1);
		}
	}
}
