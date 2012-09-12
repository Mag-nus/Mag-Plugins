using System;
using System.Collections.Generic;

namespace Mag_SuitBuilder
{
	internal class SuitSearcher
	{
		private readonly SuitSearcherConfiguration config;
		private readonly EquipmentGroup equipment;

		public event Action<SuitBuilder> SuitCreated;
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
					baseSuit.Add(equipment[i]);
					equipment.RemoveAt(i);
				}
			}

			// Remove pieces we can't add to our base suit, or pieces that can provide no beneficial spell
			for (int i = equipment.Count - 1; i >= 0; i--)
			{
				if (!baseSuit.CanAdd(equipment[i]) || !baseSuit.CanOfferBeneficialSpell(equipment[i]))
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
			if (config.PrimaryArmorSet.Equals("No Armor Set") && config.SecondaryArmorSet.Equals("No Armor Set") &&
			    (item.EquipableSlots & Constants.EquippableSlotFlags.AllBodyArmor) != 0 && !String.IsNullOrEmpty(item.ArmorSet))
				return false;
			// If we're building a two set armor suit, and we don't want any blanks or fillers, remove any pieces of armor of other sets
			if (!config.PrimaryArmorSet.Equals("No Armor Set") && !config.SecondaryArmorSet.Equals("No Armor Set") &&
			    !config.PrimaryArmorSet.Equals("Any Armor Set") && !config.SecondaryArmorSet.Equals("Any Armor Set") &&
			    (item.EquipableSlots & Constants.EquippableSlotFlags.AllBodyArmor) != 0 &&
			    !item.ArmorSet.Equals(config.PrimaryArmorSet) && !item.ArmorSet.Equals(config.SecondaryArmorSet))
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

		class Bucket : List<EquipmentPiece>
		{
			public readonly Constants.EquippableSlotFlags slot;

			public Bucket(Constants.EquippableSlotFlags slot)
			{
				this.slot = slot;
			}
		}

		class BucketSorter : List<Bucket>
		{
			public void PutItemInBuckets(EquipmentPiece piece)
			{
				foreach (Bucket bucket in this)
				{
					if ((piece.EquipableSlots & bucket.slot) == bucket.slot)
						bucket.Add(piece);
				}
			}
		}

		void StartSearch()
		{
			SuitCreated(baseSuit);

			BucketSorter sorter = new BucketSorter();

			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Head))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Head));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Hands))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Hands));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Feet))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Feet));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Chest))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Chest));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Abdomen))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Abdomen));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.UpperArms))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.UpperArms));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.LowerArms))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.LowerArms));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.UpperLegs))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.UpperLegs));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.LowerLegs))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.LowerLegs));

			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Necklace))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.Necklace));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.RightBracelet))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.RightBracelet));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.LeftBracelet))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.LeftBracelet));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.RightRing))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.RightRing));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.LeftRing))		sorter.Add(new Bucket(Constants.EquippableSlotFlags.LeftRing));

			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Trinket))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.Trinket));
			
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Shirt))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.Shirt));
			if (baseSuit.SlotIsOpen(Constants.EquippableSlotFlags.Pants))	sorter.Add(new Bucket(Constants.EquippableSlotFlags.Pants));

			// Put all of our inventory into its appropriate bucket
			foreach (EquipmentPiece piece in equipment)
				sorter.PutItemInBuckets(piece);

			// Do the actual search here
			SearchThroughBuckets(sorter, 0, baseSuit);

			// If we're not running, the search was stopped before it could complete
			if (!Running)
				return;

			Stop();

			if (SearchCompleted != null)
				SearchCompleted();
		}

		void SearchThroughBuckets(List<Bucket> buckets, int index, SuitBuilder workingSuit)
		{
			foreach (EquipmentPiece piece in buckets[index])
			{
				if (workingSuit.CanAdd(piece, buckets[index].slot))
				{
					workingSuit.Add(piece);

					if (buckets.Count > index + 1)
						SearchThroughBuckets(buckets, index + 1, workingSuit);
				}
			}

			if (buckets.Count > index + 1)
				SearchThroughBuckets(buckets, index + 1, workingSuit);
			else
				SuitCreated(workingSuit);
		}

		

		/*
		// http://www.dzone.com/snippets/depth-first-search-c
		class BinaryTreeNode
		{
			public BinaryTreeNode Left { get; set; }

			public BinaryTreeNode Right { get; set; }

			public int Data { get; set; }
		}

		class DepthFirstSearch
		{
			private Stack<BinaryTreeNode> _searchStack;
			private BinaryTreeNode _root;

			public DepthFirstSearch(BinaryTreeNode rootNode)
			{
				_root = rootNode;
				_searchStack = new Stack<BinaryTreeNode>();
			}

			public bool Search(int data)
			{
				BinaryTreeNode _current;
				_searchStack.Push(_root);

				while (_searchStack.Count != 0)
				{
					_current = _searchStack.Pop();

					if (_current.Data == data)
						return true;

					_searchStack.Push(_current.Right);
					_searchStack.Push(_current.Left);
				}

				return false;
			}
		}
		*/
		/*
		// http://blog.andreloker.de/post/2009/03/10/Algorithms-recursive-and-iterative-depth-first-search.aspx
		private void DFSRecursive(Node node)
		{
			DoSomethingWithNode(node);

			foreach (Transition t in node.Transitions)
			{
				node destNode = t.Destination;

				DFSRecursive(destNode);
			}
		}

		private void Traverse(Node node)
		{
			EnterNode(node);

			var t = node.FirstTransition;

			while (t != null)
			{
				var destNode = TakeTransition(t);

				Traverse(destNode);

				UndoTransition(t);

				t = t.NextSibling;
			}

			ExitNode(node);
		}
		*/
	}
}
