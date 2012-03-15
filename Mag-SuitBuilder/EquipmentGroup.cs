using System;
using System.Collections.Generic;

namespace Mag_SuitBuilder
{
	class EquipmentGroup
	{
		public const int MaximumPieces = 17;
		const int avgNumOfSpellsPerPiece = 3;

		public readonly IEquipmentPiece[] EquipmentPieces = new IEquipmentPiece[MaximumPieces];
		private readonly int[] equipmentPieceSlots = new int[MaximumPieces];
		public int EquipmentPieceCount;

		private Constants.EquippableSlotFlags equippedSlots;

		public readonly Spell[] Spells = new Spell[MaximumPieces * avgNumOfSpellsPerPiece];
		public int SpellCount;

		public bool CanAdd(IEquipmentPiece equipmentPiece, Constants.EquippableSlotFlags forceSlot = Constants.EquippableSlotFlags.Any)
		{
			if ((equippedSlots & (equipmentPiece.EquipableSlots & forceSlot)) != 0)
				return false;

			// Don't add more than 5 of any one armor set to a group
			if (equipmentPiece.ArmorSet != null)
			{
				int similarArmorSetPiecesFound = 0;

				for (int i = 0 ; i < EquipmentPieceCount ; i++)
				{
					if (equipmentPiece.ArmorSet == EquipmentPieces[i].ArmorSet)
						similarArmorSetPiecesFound++;
				}

				if (similarArmorSetPiecesFound >= 5)
					return false;
			}

			return true;
		}

		public bool CanOfferBeneficialSpell(IEquipmentPiece equipmentPiece)
		{
			// Does the this item have a spell that the current group doesn't have?
			foreach (Spell pieceSpell in equipmentPiece.Spells)
			{
				for (int j = 0 ; j <= this.SpellCount ; j++)
				{
					// Yes it does
					if (j == this.SpellCount)
						return true;

					if (this.Spells[j].IsSameOrSurpasses(pieceSpell))
						break;
				}
			}

			return false;
		}

		/// <summary>
		/// Make sure you call CanAdd before using this funciton to add a piece.
		/// </summary>
		/// <param name="equipmentPiece"></param>
		/// <param name="forceSlot"></param>
		/// <returns></returns>
		public void Add(IEquipmentPiece equipmentPiece, Constants.EquippableSlotFlags forceSlot = Constants.EquippableSlotFlags.Any)
		{
			EquipmentPieces[EquipmentPieceCount] = equipmentPiece;
			equipmentPieceSlots[EquipmentPieceCount] = (int)(equipmentPiece.EquipableSlots & forceSlot);
			EquipmentPieceCount++;

			equippedSlots |= (equipmentPiece.EquipableSlots & forceSlot);

			foreach (Spell spell in equipmentPiece.Spells)
			{
				Spells[SpellCount] = spell;
				SpellCount++;
			}
		}

		public EquipmentGroup Clone()
		{
			EquipmentGroup equipmentGroup = new EquipmentGroup();

			Array.Copy(EquipmentPieces, equipmentGroup.EquipmentPieces, EquipmentPieceCount);
			Buffer.BlockCopy(equipmentPieceSlots, 0, equipmentGroup.equipmentPieceSlots, 0, EquipmentPieceCount * 4);
			equipmentGroup.EquipmentPieceCount = EquipmentPieceCount;

			equipmentGroup.equippedSlots = equippedSlots;

			Array.Copy(Spells, equipmentGroup.Spells, SpellCount);
			equipmentGroup.SpellCount = SpellCount;

			return equipmentGroup;
		}

		private IEquipmentPiece GetPiece(Constants.EquippableSlotFlags slot)
		{
			for (int i = 0 ; i < EquipmentPieceCount ; i++)
			{
				if (((Constants.EquippableSlotFlags)equipmentPieceSlots[i] & slot) != 0)
					return EquipmentPieces[i];
			}

			return null;
		}

		public IEquipmentPiece Necklace { get { return GetPiece(Constants.EquippableSlotFlags.Necklace); } }

		public IEquipmentPiece Trinket { get { return GetPiece(Constants.EquippableSlotFlags.Trinket); } }

		public IEquipmentPiece LeftBracelet { get { return GetPiece(Constants.EquippableSlotFlags.LeftBracelet); } }
		public IEquipmentPiece RightBracelet { get { return GetPiece(Constants.EquippableSlotFlags.RightBracelet); } }

		public IEquipmentPiece LeftRing { get { return GetPiece(Constants.EquippableSlotFlags.LeftRing); } }
		public IEquipmentPiece RightRing { get { return GetPiece(Constants.EquippableSlotFlags.RightRing); } }

		public IEquipmentPiece Head { get { return GetPiece(Constants.EquippableSlotFlags.Head); } }
		public IEquipmentPiece Chest { get { return GetPiece(Constants.EquippableSlotFlags.Chest); } }
		public IEquipmentPiece UpperArms { get { return GetPiece(Constants.EquippableSlotFlags.UpperArms); } }
		public IEquipmentPiece LowerArms { get { return GetPiece(Constants.EquippableSlotFlags.LowerArms); } }
		public IEquipmentPiece Hands { get { return GetPiece(Constants.EquippableSlotFlags.Hands); } }
		public IEquipmentPiece Abdomen { get { return GetPiece(Constants.EquippableSlotFlags.Abdomen); } }
		public IEquipmentPiece UpperLegs { get { return GetPiece(Constants.EquippableSlotFlags.UpperLegs); } }
		public IEquipmentPiece LowerLegs { get { return GetPiece(Constants.EquippableSlotFlags.LowerLegs); } }
		public IEquipmentPiece Feet { get { return GetPiece(Constants.EquippableSlotFlags.Feet); } }

		public IEquipmentPiece Shirt { get { return GetPiece(Constants.EquippableSlotFlags.Shirt); } }
		public IEquipmentPiece Pants { get { return GetPiece(Constants.EquippableSlotFlags.Pants); } }

		int totalPotentialTinkedArmorLevel;
		/// <summary>
		/// Only call this after you have added all of your pieces. It retains its calc'd value once its called.
		/// </summary>
		public int TotalPotentialTinkedArmorLevel
		{
			get
			{
				if (totalPotentialTinkedArmorLevel == 0)
				{
					// todo hack fix
					for (int i = 0 ; i < EquipmentPieceCount ; i++)
						totalPotentialTinkedArmorLevel += EquipmentPieces[i].ArmorLevel;
				}

				return totalPotentialTinkedArmorLevel;
			}
		}

		private Dictionary<string, int> ArmorSetPieces
		{
			get
			{
				Dictionary<string, int> sets = new Dictionary<string, int>();

				for (int i = 0 ; i < EquipmentPieceCount ; i++)
				{
					if (String.IsNullOrEmpty(EquipmentPieces[i].ArmorSet))
						continue;

					if (!sets.ContainsKey(EquipmentPieces[i].ArmorSet))
						sets.Add(EquipmentPieces[i].ArmorSet, 1);
					else
						sets[EquipmentPieces[i].ArmorSet]++;
				}

				return sets;
			}
		}

		/// <summary>
		/// Only call this once, after you've finished adding all of your equipment pieces
		/// </summary>
		/// <param name="masterEquipmentPieceList"></param>
		public void CalculateEquipmentPieceHash(IList<IEquipmentPiece> masterEquipmentPieceList)
		{
			for (int i = 0 ; i < EquipmentPieceCount ; i++)
			{
				EquipmentPieceHash += EquipmentPieces[i].GetHashCode();

				int index = masterEquipmentPieceList.IndexOf(EquipmentPieces[i]);

				if (index >= 0)
				{
					if (index <= 63)
						bitGroup1 |= (ulong)1 << (index);
					else if (index <= 127)
						bitGroup2 |= (ulong)1 << (index - 64);
					else if (index <= 191)
						bitGroup3 |= (ulong)1 << (index - 128);
					else if (index <= 255)
						bitGroup4 |= (ulong)1 << (index - 192);
					else
						throw new NotImplementedException("More than 256 equipment pieces is not supported.");
				}
			}
		}

		/// <summary>
		/// Only use this after you've added all your pieces and called CalculateEquipmentPieceHash()
		/// </summary>
		public int EquipmentPieceHash;

		private ulong bitGroup1;
		private ulong bitGroup2;
		private ulong bitGroup3;
		private ulong bitGroup4;

		public bool IsEquipmentSubsetOfGroup(EquipmentGroup compareGroup)
		{
			return (bitGroup1 & compareGroup.bitGroup1) == bitGroup1 &&
				(bitGroup2 & compareGroup.bitGroup2) == bitGroup2 &&
				(bitGroup3 & compareGroup.bitGroup3) == bitGroup3 &&
				(bitGroup4 & compareGroup.bitGroup4) == bitGroup4;

			#region ' old method '
			/*
			// Does the this group have an item that the compare group doesn't have?
			for (int i = 0 ; i < this.EquipmentPieceCount ; i++)
			{
				for (int j = 0 ; j <= compareGroup.EquipmentPieceCount ; j++)
				{
					// Yes it does
					if (j == compareGroup.EquipmentPieceCount)
						return false;

					if (this.EquipmentPieces[i] == compareGroup.EquipmentPieces[j])
						break;
				}
			}

			return true;
			*/
			#endregion
		}

		private int NumberOfEpics
		{
			get
			{
				int number = 0;

				for (int i = 0 ; i < SpellCount ; i++)
				{
					if (Spells[i].IsEpic)
						number++;
				}

				return number;
			}
		}

		/*
		private Collection<Spell> Epics
		{
			get
			{
				return GetSpellsOfLevel(false, true);
			}
		}

		private Collection<Spell> Majors
		{
			get
			{
				Collection<Spell> Majors = GetSpellsOfLevel(true, false);

				Collection<Spell> spells = new Collection<Spell>();

				foreach (Spell spell in Majors)
				{
					if (SpellsContainsOrSurpassingSpell(Epics, spell))
						continue;

					spells.Add(spell);
				}

				return spells;
			}
		}

		private Collection<Spell> GetSpellsOfLevel(bool majors, bool epics)
		{
			Collection<Spell> spells = new Collection<Spell>();

			foreach (IEquipmentPiece piece in equipmentPieces)
			{
				foreach (Spell spell in piece.Spells)
				{
					if ((majors && spell.IsMajor) || (epics && spell.IsEpic))
						spells.Add(spell);
				}
			}

			return spells;
		}

		private bool SpellsContainsOrSurpassingSpell(Collection<Spell> spells, Spell searchForEqualOrBetter)
		{
			foreach (Spell spell in spells)
			{
				if (spell.IsSame(searchForEqualOrBetter) || spell.Surpasses(searchForEqualOrBetter))
					return true;
			}

			return false;
		}
		*/

		public override string ToString()
		{
			string output = "AL: " + TotalPotentialTinkedArmorLevel + ", Epics: " + NumberOfEpics; // +", Majors: " + Majors.Count;

			foreach (string armorSet in ArmorSetPieces.Keys)
			{
				output += ", " + armorSet + ":" + ArmorSetPieces[armorSet];
			}

			foreach (IEquipmentPiece equipmentPiece in EquipmentPieces)
			{
				if (equipmentPiece == null)
					break;

				output += ", " + equipmentPiece;
			}

			return output;
		}
	}
}
