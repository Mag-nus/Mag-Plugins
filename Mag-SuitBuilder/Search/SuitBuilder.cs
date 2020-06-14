
using Mag.Shared.Constants;
//using Mag.Shared.Spells;

namespace Mag_SuitBuilder.Search
{
	class SuitBuilder
	{
		public SuitBuilder()
		{
			for (int i = 0; i < slotCache.Length; i++)
				slotCache[i] = new PieceSlotCache();
		}

		private class PieceSlotCache
		{
			public LeanMyWorldObject Piece;
			public EquipMask Slot;
			//public int SpellCount; // Used for the old search compare method
		}

		readonly PieceSlotCache[] slotCache = new PieceSlotCache[17];
		readonly int[] spellBitmaps = new int[17];
		int nextOpenCacheIndex;

		EquipMask occupiedSlots = EquipMask.None;

		//readonly Spell[] spells = new Spell[17 * 6]; // Used for the old search compare method
		//int nextOpenSpellIndex;

		readonly int[] armorSetCountById = new int[256];

		public int TotalBaseArmorLevel { get; private set; }

		public int TotalBodyArmorPieces { get; private set; }

		public void Push(LeanMyWorldObject item, EquipMask slot)
		{
			slotCache[nextOpenCacheIndex].Piece = item;
			slotCache[nextOpenCacheIndex].Slot = slot;
			//slotCache[nextOpenCacheIndex].SpellCount = item.SpellsToUseInSearch.Count; // Used for the old search compare method

			occupiedSlots |= slot;

			// Used for the old search compare method
			/*for (int i = 0; i < item.SpellsToUseInSearch.Count; i++)
			{
				spells[nextOpenSpellIndex] = item.SpellsToUseInSearch[i];
				nextOpenSpellIndex++;
			}*/

			if (nextOpenCacheIndex == 0)
				spellBitmaps[nextOpenCacheIndex] = item.SpellBitmap;
			else
				spellBitmaps[nextOpenCacheIndex] = spellBitmaps[nextOpenCacheIndex - 1] | item.SpellBitmap;

			nextOpenCacheIndex++;

			if (item.ItemSetId != -1)
				armorSetCountById[item.ItemSetId]++;

			if (item.CalcedStartingArmorLevel > 0)
				TotalBaseArmorLevel += (item.CalcedStartingArmorLevel * slot.GetTotalBitsSet());

			if (slot.IsBodyArmor())
				TotalBodyArmorPieces++;
		}

		public void Pop()
		{
			occupiedSlots ^= slotCache[nextOpenCacheIndex - 1].Slot;

			//nextOpenSpellIndex -= slotCache[nextOpenCacheIndex - 1].SpellCount; // Used for the old search compare method

			armorSetCountById[slotCache[nextOpenCacheIndex - 1].Piece.ItemSetId]--;

			if (slotCache[nextOpenCacheIndex - 1].Piece.CalcedStartingArmorLevel > 0)
				TotalBaseArmorLevel -= (slotCache[nextOpenCacheIndex - 1].Piece.CalcedStartingArmorLevel * slotCache[nextOpenCacheIndex - 1].Slot.GetTotalBitsSet());

			if (slotCache[nextOpenCacheIndex - 1].Slot.IsBodyArmor())
				TotalBodyArmorPieces--;

			nextOpenCacheIndex--;
		}

		public bool SlotIsOpen(EquipMask slot)
		{
			return ((occupiedSlots & slot) == 0);
		}

		public bool HasRoomForArmorSet(int primarySetToBuild, int secondarySetToBuild, int setPieceToAdd)
		{
			if (primarySetToBuild == 255 || secondarySetToBuild == 255)
				return true;

			if (primarySetToBuild != setPieceToAdd && secondarySetToBuild != setPieceToAdd)
				return false;

			if (primarySetToBuild == setPieceToAdd && armorSetCountById[setPieceToAdd] >= 5)
				return false;

			if (secondarySetToBuild == setPieceToAdd && armorSetCountById[setPieceToAdd] >= 4)
				return false;

			return true;
		}

		public bool CanGetBeneficialSpellFrom(LeanMyWorldObject item)
		{
			if (nextOpenCacheIndex == 0)
				return true;

			return (spellBitmaps[nextOpenCacheIndex - 1] | item.SpellBitmap) != spellBitmaps[nextOpenCacheIndex - 1];

			// Used for the old search compare method
			// This whole approach needs to be optimized.
			// This is the biggest time waster in the entire search process.

			/*foreach (Spell itemSpell in item.SpellsToUseInSearch)
			{
				for (int j = 0; j < nextOpenSpellIndex; j++) // For here is faster than foreach
				{
					if (spells[j].IsSameOrSurpasses(itemSpell))
						goto end;
				}

				return true;

				end: ;
			}

			return false;*/
		}

		public int Count
		{
			get { return nextOpenCacheIndex; }
		}

		public SuitBuilder Clone()
		{
			SuitBuilder newSuit = new SuitBuilder();

			for (int i = 0; i < nextOpenCacheIndex; i++)
				newSuit.Push(slotCache[i].Piece, slotCache[i].Slot);

			return newSuit;
		}

		public CompletedSuit CreateCompletedSuit()
		{
			CompletedSuit suit = new CompletedSuit();

			for (int i = 0; i < nextOpenCacheIndex; i++)
				suit.AddItem(slotCache[i].Slot, slotCache[i].Piece);

			return suit;
		}
	}
}
