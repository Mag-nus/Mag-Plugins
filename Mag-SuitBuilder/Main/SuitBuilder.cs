using System.Collections.Generic;

namespace Mag_SuitBuilder
{
	class SuitBuilder
	{
		readonly Stack<EquipmentPiece> pieces = new Stack<EquipmentPiece>(17);
		readonly Stack<Constants.EquippableSlotFlags> slots = new Stack<Constants.EquippableSlotFlags>(17);

		Constants.EquippableSlotFlags slotFlags = Constants.EquippableSlotFlags.None;

		readonly Stack<int> spellsForPiece = new Stack<int>(17);
		readonly Stack<Spell> spells = new Stack<Spell>(17 * 5);

		public void Push(EquipmentPiece item, Constants.EquippableSlotFlags slot)
		{
			pieces.Push(item);
			slots.Push(slot);

			slotFlags |= slot;

			spellsForPiece.Push(item.Spells.Count);

			//foreach (Spell spell in item.Spells)
			//	spells.Push(spell);
			for (int i = 0; i < item.Spells.Count; i++)
				spells.Push(item.Spells[i]);
		}

		public void Pop()
		{
			pieces.Pop();
			Constants.EquippableSlotFlags poppedSlot = slots.Pop();

			slotFlags ^= poppedSlot;

			int poppedSpells = spellsForPiece.Pop();

			for (int i = 0; i < poppedSpells; i++)
				spells.Pop();
		}


		public bool SlotIsOpen(Constants.EquippableSlotFlags slot)
		{
			return ((slotFlags & slot) == 0);
		}

		public bool CanOfferBeneficialSpell(EquipmentPiece item)
		{
			foreach (Spell itemSpell in item.Spells)
			//for (int i = 0 ; i < item.Spells.Count ; i++)
			{
				foreach (Spell spell in spells)
				//for (int j = 0 ; j < spells.Count ; j++)
				{
					if (spell.IsSameOrSurpasses(itemSpell))
						goto end;
				}

				return true;

				end: ;
			}

			return false;
		}

		public int Count
		{
			get { return pieces.Count; }
		}

		public SuitBuilder Clone()
		{
			SuitBuilder clone = new SuitBuilder();

			foreach (var item in pieces)
				clone.pieces.Push(item);

			foreach (var item in slots)
				clone.slots.Push(item);

			clone.slotFlags = slotFlags;

			foreach (var item in spellsForPiece)
				clone.spellsForPiece.Push(item);

			foreach (var item in spells)
				clone.spells.Push(item);

			return clone;
		}

		public Dictionary<Constants.EquippableSlotFlags, EquipmentPiece> GetEquipment()
		{
			Dictionary<Constants.EquippableSlotFlags, EquipmentPiece> equipment = new Dictionary<Constants.EquippableSlotFlags, EquipmentPiece>(pieces.Count);

			// This is hacky but its only used for viewing the suit.. can clean it up later
			int slotIndex = 0;
			foreach (var item in slots)
			{
				int pieceIndex = 0;
				foreach (var piece in pieces)
				{
					if (slotIndex == pieceIndex)
					{
						equipment.Add(item, piece);
						break;
					}
					pieceIndex++;
				}
				slotIndex++;
			}

			return equipment;
		}

		public override string ToString()
		{
			int totalBaseArmorLevel = 0;

			foreach (var item in pieces)
				totalBaseArmorLevel += (item.BaseArmorLevel * item.BodyPartsCovered);

			/*
			string output = "AL: " + TotalPotentialTinkedArmorLevel + ", Epics: " + NumberOfEpics; // +", Majors: " + Majors.Count;

			foreach (string armorSet in ArmorSetPieces.Keys)
			{
				output += ", " + armorSet + ":" + ArmorSetPieces[armorSet];
			}

			foreach (EquipmentPiece equipmentPiece in EquipmentPieces)
			{
				if (equipmentPiece == null)
					break;

				output += ", " + equipmentPiece;
			}

			return output;
			*/

			return totalBaseArmorLevel + " " + pieces.Count;
		}
	}
}
