using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Spells;

namespace Mag_SuitBuilder.Search
{
	class SuitBuilder
	{
		public SuitBuilder()
		{
			for (int i = 0; i < cache.Length; i++)
				cache[i] = new PieceSlotCache();

			foreach (ArmorSet set in ArmorSet.GetAllArmorSets())
				armorSetCount.Add(set, 0);
		}

		private class PieceSlotCache
		{
			public EquipmentPiece Piece;
			public Constants.EquippableSlotFlags Slot;
			public int SpellCount;
		}

		readonly PieceSlotCache[] cache = new PieceSlotCache[17];

		int nextOpenCacheIndex;

		Constants.EquippableSlotFlags occupiedSlots = Constants.EquippableSlotFlags.None;

		readonly Spell[] spells = new Spell[17 * 5];
		int nextOpenSpellIndex;

		readonly Dictionary<ArmorSet, int> armorSetCount = new Dictionary<ArmorSet, int>();

		//public int TotalBaseArmorLevel { get; private set; }

		public void Push(EquipmentPiece item, Constants.EquippableSlotFlags slot)
		{
			cache[nextOpenCacheIndex].Piece = item;
			cache[nextOpenCacheIndex].Slot = slot;
			cache[nextOpenCacheIndex].SpellCount = item.Spells.Count;

			occupiedSlots |= slot;

			for (int i = 0; i < item.Spells.Count; i++)
			{
				spells[nextOpenSpellIndex] = item.Spells[i];
				nextOpenSpellIndex++;
			}

			nextOpenCacheIndex++;

			armorSetCount[item.ArmorSet]++;

			//TotalBaseArmorLevel += (item.BaseArmorLevel * item.BodyPartsCovered);
		}

		public void Pop()
		{
			occupiedSlots ^= cache[nextOpenCacheIndex - 1].Slot;

			nextOpenSpellIndex -= cache[nextOpenCacheIndex - 1].SpellCount;

			armorSetCount[cache[nextOpenCacheIndex - 1].Piece.ArmorSet]--;

			//TotalBaseArmorLevel -= cache[nextOpenCacheIndex - 1].Piece.BaseArmorLevel * cache[nextOpenCacheIndex - 1].Piece.BodyPartsCovered;

			nextOpenCacheIndex--;
		}

		public bool SlotIsOpen(Constants.EquippableSlotFlags slot)
		{
			return ((occupiedSlots & slot) == 0);
		}

		public bool HasRoomForArmorSet(ArmorSet primarySetToBuild, ArmorSet secondarySetToBuild, ArmorSet setPieceToAdd)
		{
			if (primarySetToBuild == ArmorSet.AnyArmorSet || secondarySetToBuild == ArmorSet.AnyArmorSet)
				return true;

			if (primarySetToBuild != setPieceToAdd && secondarySetToBuild != setPieceToAdd)
				return false;

			if (primarySetToBuild == setPieceToAdd && armorSetCount[setPieceToAdd] >= 5)
				return false;

			if (secondarySetToBuild == setPieceToAdd && armorSetCount[setPieceToAdd] >= 4)
				return false;

			return true;
		}

		public bool CanGetBeneficialSpellFrom(EquipmentPiece item)
		{
			foreach (Spell itemSpell in item.Spells)
			//for (int i = 0 ; i < item.Spells.Count ; i++) // This is actually slower
			{
				for (int j = 0; j < nextOpenSpellIndex; j++) // For here is faster than foreach
				{
					if (spells[j].IsSameOrSurpasses(itemSpell))
						goto end;
				}

				return true;

				end: ;
			}

			return false;
		}

		public int Count
		{
			get { return nextOpenCacheIndex; }
		}

		public Dictionary<Constants.EquippableSlotFlags, EquipmentPiece> GetCopyOfCompletedSuit()
		{
			CompletedSuit suit = new CompletedSuit();

			for (int i = 0; i < nextOpenCacheIndex; i++)
				suit.Add(cache[i].Slot, cache[i].Piece);

			return suit;
		}
	}
}
