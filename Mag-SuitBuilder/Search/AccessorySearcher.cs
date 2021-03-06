﻿using System.Collections.Generic;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	/// <summary>
	/// The class takes equipment (trinkets/underwear/jewelry) without armor and applies them to a base suit pushing out the top permutations.
	/// </summary>
	class AccessorySearcher : Searcher
	{
		public AccessorySearcher(SearcherConfiguration config, IEnumerable<LeanMyWorldObject> accessories, CompletedSuit startingSuit = null) : base(config, accessories, startingSuit)
		{
			// Sort the list with the highest amount of epics
			// As a temp fix we just sort based on spell count
			Equipment.Sort((a, b) =>
			{
				if (a.CalcedStartingArmorLevel > 0 && b.CalcedStartingArmorLevel > 0) return b.CalcedStartingArmorLevel.CompareTo(a.CalcedStartingArmorLevel);
				if (a.CalcedStartingArmorLevel > 0 && b.CalcedStartingArmorLevel == 0) return -1;
				if (a.CalcedStartingArmorLevel == 0 && b.CalcedStartingArmorLevel > 0) return 1;
				return b.SpellsToUseInSearch.Count.CompareTo(a.SpellsToUseInSearch.Count);
			});

			// Remove any pieces that have armor
			for (int i = Equipment.Count - 1; i >= 0; i--)
			{
				if (Equipment[i].CalcedStartingArmorLevel > 0)
					Equipment.RemoveAt(i);
			}
		}

		int highestCountSuitBuilt;
		Dictionary<int, List<int>> highestEffectiveSpellsSuitBuilt;
		List<CompletedSuit> completedSuits;

		protected override void StartSearch()
		{
			List<Bucket> sorter = new List<Bucket>();

			if (SuitBuilder.SlotIsOpen(EquipMask.TrinketOne))		sorter.Add(new Bucket(EquipMask.TrinketOne));

			if (SuitBuilder.SlotIsOpen(EquipMask.ChestWear))		sorter.Add(new Bucket(EquipMask.ChestWear));
			if (SuitBuilder.SlotIsOpen(EquipMask.UpperLegWear)) sorter.Add(new Bucket(EquipMask.UpperLegWear));

			if (SuitBuilder.SlotIsOpen(EquipMask.NeckWear))		sorter.Add(new Bucket(EquipMask.NeckWear));
			if (SuitBuilder.SlotIsOpen(EquipMask.WristWearRight))	sorter.Add(new Bucket(EquipMask.WristWearRight));
			if (SuitBuilder.SlotIsOpen(EquipMask.WristWearLeft))	sorter.Add(new Bucket(EquipMask.WristWearLeft));
			if (SuitBuilder.SlotIsOpen(EquipMask.FingerWearRight))		sorter.Add(new Bucket(EquipMask.FingerWearRight));
			if (SuitBuilder.SlotIsOpen(EquipMask.FingerWearLeft))		sorter.Add(new Bucket(EquipMask.FingerWearLeft));

			// Put all of our inventory into its appropriate bucket
			foreach (var piece in Equipment)
				sorter.PutItemInBuckets(piece);

			// Remove any empty buckets
			for (int i = sorter.Count - 1; i >= 0; i--)
			{
				if (sorter[i].Count == 0)
					sorter.RemoveAt(i);
			}

			// Reset our variables
			highestCountSuitBuilt = 0;
			highestEffectiveSpellsSuitBuilt = new Dictionary<int, List<int>>();
			for (int i = 1; i <= 17; i++)
				highestEffectiveSpellsSuitBuilt.Add(i, new List<int>(5));
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

			// Only continue to build any suits with a minimum potential of no less than 1 epics less than our largest built suit so far
			if (SuitBuilder.Count + 1 < highestCountSuitBuilt - (highestCountSuitBuilt - index))
				return;

			// Are we at the end of the line?
			if (buckets.Count <= index)
			{
				if (SuitBuilder.Count == 0)
					return;

				if (SuitBuilder.Count > highestCountSuitBuilt)
					highestCountSuitBuilt = SuitBuilder.TotalBodyArmorPieces;

				CompletedSuit newSuit = SuitBuilder.CreateCompletedSuit();

				// We should keep track of the highest epic suits we built for every number of item count suits built, and only push out ones that fall within our top X
				List<int> list = highestEffectiveSpellsSuitBuilt[SuitBuilder.Count];
				if (list.Count < list.Capacity)
				{
					list.Add(newSuit.TotalEffectiveEpics);

					if (list.Count == list.Capacity)
						list.Sort();
				}
				else
				{
					if (list[list.Count - 1] >= newSuit.TotalEffectiveEpics)
						return;

					list[list.Count - 1] = newSuit.TotalEffectiveEpics;
					list.Sort();
				}

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

			//for (int i = 0; i < buckets[index].Count ; i++)
			foreach (var piece in buckets[index]) // Using foreach: 10.85s, for: 11s
			{
				if (SuitBuilder.CanGetBeneficialSpellFrom(piece))
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
