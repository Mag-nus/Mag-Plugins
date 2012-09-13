using System;
using System.Collections.Generic;
using System.Threading;

using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Spells;

namespace Mag_SuitBuilder.Search
{
	internal class SuitSearcher
	{
		private readonly SuitSearcherConfiguration config;
		private readonly EquipmentGroup equipment;

		public event Action<Dictionary<Constants.EquippableSlotFlags, EquipmentPiece>> SuitCreated;
		public event Action SearchCompleted;

		private readonly SuitBuilder baseSuit = new SuitBuilder();

		public SuitSearcher(SuitSearcherConfiguration config, IEnumerable<EquipmentPiece> pieces)
		{
			this.config = config;

			// Sort the list by highest base armor level armor first
			List<EquipmentPiece> list = new List<EquipmentPiece>(pieces);
			list.Sort((a, b) =>
				          {
					          if (a.BaseArmorLevel > b.BaseArmorLevel) return -1;
					          if (a.BaseArmorLevel < b.BaseArmorLevel) return 1;
					          return 0;
				          });

			// Initialize and build our master equipment group
			equipment = new EquipmentGroup();

			foreach (EquipmentPiece piece in list)
				equipment.Add(piece);

			// Build our base suit from locked in pieces
			for (int i = equipment.Count - 1; i >= 0; i--)
			{
				if (equipment[i].Locked)
				{
					baseSuit.Push(equipment[i], equipment[i].EquipableSlots);
					equipment.RemoveAt(i);
				}
			}

			// Remove pieces we can't add to our base suit, or pieces that can provide no beneficial spell
			for (int i = equipment.Count - 1; i >= 0; i--)
			{
				if (!baseSuit.SlotIsOpen(equipment[i].EquipableSlots) || !baseSuit.CanGetBeneficialSpellFrom(equipment[i]))
					equipment.RemoveAt(i);
			}

			// Remove pieces that don't meet our minimum requirements
			for (int i = equipment.Count - 1; i >= 0; i--)
			{
				if (!ItemPassesRules(equipment[i]))
					equipment.RemoveAt(i);
			}

			// Remove surpassed pieces
			for (int i = equipment.Count - 1; i >= 0; i--)
			{
				if (equipment.ItemIsSurpassed(equipment[i]))
					equipment.RemoveAt(i);
			}

			// Go through our equipment and remove/disable any extra spells that we're not looking for
			// todo hack fix
		}

		public bool ItemPassesRules(EquipmentPiece item)
		{
			if (config.MinimumArmorLevelPerPiece > 0 && item.BaseArmorLevel != 0 &&
			    config.MinimumArmorLevelPerPiece > item.BaseArmorLevel)
				return false;

			if (config.CantripsToLookFor.Count > 0)
			{
				bool found = false;

				foreach (Spell cantrip in config.CantripsToLookFor)
				{
					foreach (Spell itemSpell in item.Spells)
					{
						if (itemSpell.IsSameOrSurpasses(cantrip))
						{
							found = true;
							break;
						}
					}

					if (found)
						break;
				}

				if (!found)
					return false;
			}

			// If we're don't want to use any set pieces, remove them
			if (config.PrimaryArmorSet == ArmorSet.NoArmorSet && config.SecondaryArmorSet == ArmorSet.NoArmorSet &&
			    (item.EquipableSlots & Constants.EquippableSlotFlags.AllBodyArmor) != 0 && item.ArmorSet != ArmorSet.NoArmorSet)
				return false;
			// If we're building a two set armor suit, and we don't want any blanks or fillers, remove any pieces of armor of other sets
			if (config.PrimaryArmorSet != ArmorSet.NoArmorSet && config.SecondaryArmorSet != ArmorSet.NoArmorSet &&
				config.PrimaryArmorSet != ArmorSet.AnyArmorSet && config.SecondaryArmorSet != ArmorSet.AnyArmorSet &&
			    (item.EquipableSlots & Constants.EquippableSlotFlags.AllBodyArmor) != 0 &&
			    item.ArmorSet != config.PrimaryArmorSet && item.ArmorSet != config.SecondaryArmorSet)
				return false;

			// Check to see if we only want pieces with armor
			if (config.OnlyAddPiecesWithArmor && item.BaseArmorLevel == 0)
				return false;

			return true;
		}

		public bool Running { get; private set; }

		public void Start()
		{
			if (Running)
				return;

			Running = true;

			StartSearch();
		}

		public void Stop()
		{
			if (!Running)
				return;

			Running = false;
		}

		private class Bucket : List<EquipmentPiece>
		{
			public readonly Constants.EquippableSlotFlags Slot;
			public readonly bool IsBodyArmor;

			public Bucket(Constants.EquippableSlotFlags slot)
			{
				Slot = slot;
				IsBodyArmor = (slot & Constants.EquippableSlotFlags.AllBodyArmor) != 0;
			}
		}

		private class BucketSorter : List<Bucket>
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

		int totalArmorBucketsWithItems;
		int highestArmorCountSuitBuilt;
		Dictionary<int, List<int>> highestArmorSuitsBuilt;

		void StartSearch()
		{
			if (SuitCreated != null)
				SuitCreated(baseSuit.GetCopyOfCompletedSuit());

			BucketSorter sorter = new BucketSorter();

			// All these slots can have armor
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Head))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Head));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Hands))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Hands));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Feet))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Feet));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Chest))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Chest));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Abdomen))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Abdomen));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.UpperArms))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.UpperArms));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.LowerArms))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.LowerArms));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.UpperLegs))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.UpperLegs));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.LowerLegs))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.LowerLegs));

			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Shirt)) sorter.Add(new Bucket(Constants.EquippableSlotFlags.Shirt));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Pants)) sorter.Add(new Bucket(Constants.EquippableSlotFlags.Pants));

			// All these slots have no armor
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Necklace))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Necklace));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.RightBracelet))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.RightBracelet));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.LeftBracelet))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.LeftBracelet));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.RightRing))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.RightRing));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.LeftRing))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.LeftRing));

			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Trinket))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.Trinket));
			
			// Put all of our inventory into its appropriate bucket
			foreach (EquipmentPiece piece in equipment)
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

			// Calculate the total number of armor buckets we have with pieces in them.
			foreach (Bucket bucket in sorter)
			{
				if ((bucket.Slot & Constants.EquippableSlotFlags.AllBodyArmor) != 0 && bucket.Count > 0)
					totalArmorBucketsWithItems++;
			}

			// Reset our variables
			highestArmorCountSuitBuilt = 0;
			highestArmorSuitsBuilt = new Dictionary<int, List<int>>();
			for (int i = 1; i <= sorter.Count; i++)
				highestArmorSuitsBuilt.Add(i, new List<int>(10));

			new Thread(() =>
			{
				DateTime starTime = DateTime.Now;

				// Do the actual search here
				SearchThroughBuckets(sorter, 0);

				DateTime endTime = DateTime.Now;

				//System.Windows.Forms.MessageBox.Show((endTime - starTime).TotalSeconds.ToString());

				// If we're not running, the search was stopped before it could complete
				if (!Running)
					return;

				Stop();

				if (SearchCompleted != null)
					SearchCompleted();
			}).Start();
		}

		void SearchThroughBuckets(List<Bucket> buckets, int index)
		{
			if (!Running)
				return;

			// Are we at the end of the line?
			if (buckets.Count <= index)
			{
				if (totalArmorBucketsWithItems > 0 && index > 0 && buckets[index - 1].IsBodyArmor && baseSuit.Count > highestArmorCountSuitBuilt)
					highestArmorCountSuitBuilt = baseSuit.Count;

				// We should keep track of the highest AL suits we built for every number of armor count suits built, and only push out ones that fall within our top X
				List<int> list = highestArmorSuitsBuilt[baseSuit.Count];
				if (list.Count < list.Capacity)
				{
					if (!list.Contains(baseSuit.TotalBaseArmorLevel))
					{
						list.Add(baseSuit.TotalBaseArmorLevel);

						if (list.Count == list.Capacity)
							list.Sort();
					}
				}
				else
				{
					if (list[list.Count - 1] > baseSuit.TotalBaseArmorLevel)
						return;

					if (list[list.Count - 1] < baseSuit.TotalBaseArmorLevel && !list.Contains(baseSuit.TotalBaseArmorLevel))
					{
						list[list.Count - 1] = baseSuit.TotalBaseArmorLevel;
						list.Sort();
					}
				}

				if (SuitCreated != null)
					SuitCreated(baseSuit.GetCopyOfCompletedSuit());

				return;
			}

			// Only continue to build any suits with a minimum potential of no less than 1 armor pieces less than our largest built suit so far
			if (baseSuit.Count + 1 <= highestArmorCountSuitBuilt - (totalArmorBucketsWithItems - index))
				return;

			//for (int i = 0; i < buckets[index].Count ; i++)
			foreach (EquipmentPiece piece in buckets[index]) // Using foreach: 10.85s, for: 11s
			{
				if (baseSuit.SlotIsOpen(buckets[index].Slot) && baseSuit.HasRoomForArmorSet(config.PrimaryArmorSet, config.SecondaryArmorSet, piece.ArmorSet) && baseSuit.CanGetBeneficialSpellFrom(piece))
				{
					baseSuit.Push(piece, buckets[index].Slot);

					if (buckets[index].IsBodyArmor && baseSuit.Count > highestArmorCountSuitBuilt)
						highestArmorCountSuitBuilt = baseSuit.Count;

					SearchThroughBuckets(buckets, index + 1);

					baseSuit.Pop();
				}
			}

			SearchThroughBuckets(buckets, index + 1);
		}
	}
}
