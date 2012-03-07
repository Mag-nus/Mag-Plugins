using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mag_SuitBuilder
{
	class EquipmentGroup
	{
		private Collection<EquipmentPiece> equipmentPieces = new Collection<EquipmentPiece>();

		public Dictionary<string, int> ArmorSetPieces
		{
			get
			{
				Dictionary<string, int> sets = new Dictionary<string, int>();

				foreach (EquipmentPiece piece in equipmentPieces)
				{
					if (String.IsNullOrEmpty(piece.ArmorSet))
						continue;

					sets[piece.ArmorSet]++;
				}

				return sets;
			}
		}

		public int TotalPotentialTinkedArmorLevel
		{
			get
			{
				int totalPotentialTinkedArmorLevel = 0;

				foreach (EquipmentPiece piece in equipmentPieces)
				{
					if (piece.IsUnderwear)
						totalPotentialTinkedArmorLevel += piece.PotentialTinkedArmorLevel * piece.UnderwearCoverageSlots;
					else
						totalPotentialTinkedArmorLevel += piece.PotentialTinkedArmorLevel;
				}

				return totalPotentialTinkedArmorLevel;
			}
		}

		public Collection<Spell> Epics
		{
			get
			{
				return GetSpellsOfLevel(SpellLevel.Epic);
			}
		}

		public Collection<Spell> Majors
		{
			get
			{
				Collection<Spell> Majors = GetSpellsOfLevel(SpellLevel.Major);

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

		private Collection<Spell> GetSpellsOfLevel(SpellLevel level)
		{
			Collection<Spell> spells = new Collection<Spell>();

			foreach (EquipmentPiece piece in equipmentPieces)
			{
				foreach (Spell spell in piece.Spells)
				{
					if (spell.Level == level)
						spells.Add(spell);
				}
			}

			return spells;
		}

		private bool SpellsContainsOrSurpassingSpell(Collection<Spell> spells, Spell searchForEqualOrBetter)
		{
			foreach (Spell spell in spells)
			{
				if (spell.Level >= searchForEqualOrBetter.Level && spell.NameWithoutLevel == searchForEqualOrBetter.NameWithoutLevel)
					return true;
			}

			return false;
		}

		public override string ToString()
		{
			string output = "AL: " + TotalPotentialTinkedArmorLevel + ", Epics: " + Epics.Count + ", Majors: " + Majors.Count;

			foreach (string armorSet in ArmorSetPieces.Keys)
			{
				output += ", " + armorSet + ":" + ArmorSetPieces[armorSet];
			}

			return output;
		}

		public bool IsFull
		{
			get
			{
				return Count >= 17;
			}
		}

		public bool CanAdd(EquipmentPiece equipmentPiece, Constants.EquippableSlotFlags forceSlot = Constants.EquippableSlotFlags.Any)
		{
			if (IsFull)
				return false;

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Necklace) != 0 && Necklace == null) return true;

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Trinket) != 0 && Trinket == null) return true;

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.LeftBracelet) != 0 && LeftBracelet == null) return true;
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.RightBracelet) != 0 && RightBracelet == null) return true;

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.LeftRing) != 0 && LeftRing == null) return true;
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.RightRing) != 0 && RightRing == null) return true;

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Head) != 0 && Head == null) return true;
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Chest) != 0 && Chest == null) return true;
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.UpperArms) != 0 && UpperArms == null) return true;
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.LowerArms) != 0 && LowerArms == null) return true;
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Hands) != 0 && Hands == null) return true;
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Abdomen) != 0 && Abdomen == null) return true;
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.UpperLegs) != 0 && UpperLegs == null) return true;
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.LowerLegs) != 0 && LowerLegs == null) return true;
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Feet) != 0 && Feet == null) return true;

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Shirt) != 0 && Shirt == null) return true;
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Pants) != 0 && Pants == null) return true;

			return false;
		}

		public bool CanOfferBeneficialSpell(EquipmentPiece equipmentPiece)
		{
			foreach (Spell spell in equipmentPiece.Spells)
			{
				bool betterOrSameFound = false;

				foreach (EquipmentPiece piece in equipmentPieces)
				{
					foreach (Spell pieceSpell in piece.Spells)
					{
						if (pieceSpell.Level >= spell.Level && pieceSpell.NameWithoutLevel == spell.NameWithoutLevel)
							betterOrSameFound = true;
					}
				}

				if (!betterOrSameFound)
					return true;
			}

			return false;
		}

		public bool Add(EquipmentPiece equipmentPiece, Constants.EquippableSlotFlags forceSlot = Constants.EquippableSlotFlags.Any)
		{
			if (IsFull)
				return false;

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Necklace) != 0 && Necklace == null) { equipmentPieces.Add(equipmentPiece); Necklace = equipmentPiece; return true; }

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Trinket) != 0 && Trinket == null) { equipmentPieces.Add(equipmentPiece); Trinket = equipmentPiece; return true; }

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.LeftBracelet) != 0 && LeftBracelet == null) { equipmentPieces.Add(equipmentPiece); LeftBracelet = equipmentPiece; return true; }
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.RightBracelet) != 0 && RightBracelet == null) { equipmentPieces.Add(equipmentPiece); RightBracelet = equipmentPiece; return true; }

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.LeftRing) != 0 && LeftRing == null) { equipmentPieces.Add(equipmentPiece); LeftRing = equipmentPiece; return true; }
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.RightRing) != 0 && RightRing == null) { equipmentPieces.Add(equipmentPiece); RightRing = equipmentPiece; return true; }

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Head) != 0 && Head == null) { equipmentPieces.Add(equipmentPiece); Head = equipmentPiece; return true; }
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Chest) != 0 && Chest == null) { equipmentPieces.Add(equipmentPiece); Chest = equipmentPiece; return true; }
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.UpperArms) != 0 && UpperArms == null) { equipmentPieces.Add(equipmentPiece); UpperArms = equipmentPiece; return true; }
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.LowerArms) != 0 && LowerArms == null) { equipmentPieces.Add(equipmentPiece); LowerArms = equipmentPiece; return true; }
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Hands) != 0 && Hands == null) { equipmentPieces.Add(equipmentPiece); Hands = equipmentPiece; return true; }
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Abdomen) != 0 && Abdomen == null) { equipmentPieces.Add(equipmentPiece); Abdomen = equipmentPiece; return true; }
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.UpperLegs) != 0 && UpperLegs == null) { equipmentPieces.Add(equipmentPiece); UpperLegs = equipmentPiece; return true; }
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.LowerLegs) != 0 && LowerLegs == null) { equipmentPieces.Add(equipmentPiece); LowerLegs = equipmentPiece; return true; }
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Feet) != 0 && Feet == null) { equipmentPieces.Add(equipmentPiece); Feet = equipmentPiece; return true; }

			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Shirt) != 0 && Shirt == null) { equipmentPieces.Add(equipmentPiece); Shirt = equipmentPiece; return true; }
			if (((equipmentPiece.EquipableSlots & forceSlot) & Constants.EquippableSlotFlags.Pants) != 0 && Pants == null) { equipmentPieces.Add(equipmentPiece); Pants = equipmentPiece; return true; }

			return false;
		}

		public int Count
		{
			get
			{
				return equipmentPieces.Count;
			}
		}

		public EquipmentPiece Necklace { get; private set; }

		public EquipmentPiece Trinket { get; private set; }

		public EquipmentPiece LeftBracelet { get; private set; }
		public EquipmentPiece RightBracelet { get; private set; }

		public EquipmentPiece LeftRing { get; private set; }
		public EquipmentPiece RightRing { get; private set; }

		public EquipmentPiece Head { get; private set; }
		public EquipmentPiece Chest { get; private set; }
		public EquipmentPiece UpperArms { get; private set; }
		public EquipmentPiece LowerArms { get; private set; }
		public EquipmentPiece Hands { get; private set; }
		public EquipmentPiece Abdomen { get; private set; }
		public EquipmentPiece UpperLegs { get; private set; }
		public EquipmentPiece LowerLegs { get; private set; }
		public EquipmentPiece Feet { get; private set; }

		public EquipmentPiece Shirt { get; private set; }
		public EquipmentPiece Pants { get; private set; }

		public EquipmentGroup Clone()
		{
			EquipmentGroup equipmentGroup = new EquipmentGroup();

			foreach (EquipmentPiece equipmentPiece in equipmentPieces)
				equipmentGroup.equipmentPieces.Add(equipmentPiece);

			equipmentGroup.Necklace = Necklace;

			equipmentGroup.Trinket = Trinket;

			equipmentGroup.LeftBracelet = LeftBracelet;
			equipmentGroup.RightBracelet = RightBracelet;

			equipmentGroup.LeftRing = LeftRing;
			equipmentGroup.RightRing = RightRing;

			equipmentGroup.Head = Head;
			equipmentGroup.Chest = Chest;
			equipmentGroup.UpperArms = UpperArms;
			equipmentGroup.LowerArms = LowerArms;
			equipmentGroup.Hands = Hands;
			equipmentGroup.Abdomen = Abdomen;
			equipmentGroup.UpperLegs = UpperLegs;
			equipmentGroup.LowerLegs = LowerLegs;
			equipmentGroup.Feet = Feet;

			equipmentGroup.Shirt = Shirt;
			equipmentGroup.Pants = Pants;

			return equipmentGroup;
		}
	}
}
