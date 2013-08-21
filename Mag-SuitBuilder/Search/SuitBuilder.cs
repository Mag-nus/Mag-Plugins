
using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Spells;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder.Search
{
	class SuitBuilder
	{
		public SuitBuilder()
		{
			for (int i = 0; i < cache.Length; i++)
				cache[i] = new PieceSlotCache();
		}

		private class PieceSlotCache
		{
			public SuitBuildableMyWorldObject Piece;
			public EquippableSlotFlags Slot;
			public int SpellCount;
		}

		readonly PieceSlotCache[] cache = new PieceSlotCache[17];
		int nextOpenCacheIndex;

		EquippableSlotFlags occupiedSlots = EquippableSlotFlags.None;

		readonly Spell[] spells = new Spell[17 * 5];
		int nextOpenSpellIndex;

		readonly int[] armorSetCountById = new int[256];

		public int TotalBaseArmorLevel { get; private set; }

		public int TotalBodyArmorPieces { get; private set; }

		public void Push(SuitBuildableMyWorldObject item, EquippableSlotFlags slot)
		{
			cache[nextOpenCacheIndex].Piece = item;
			cache[nextOpenCacheIndex].Slot = slot;
			cache[nextOpenCacheIndex].SpellCount = item.SpellsToUseInSearch.Count;

			occupiedSlots |= slot;

			for (int i = 0; i < item.SpellsToUseInSearch.Count; i++)
			{
				spells[nextOpenSpellIndex] = item.SpellsToUseInSearch[i];
				nextOpenSpellIndex++;
			}

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
			occupiedSlots ^= cache[nextOpenCacheIndex - 1].Slot;

			nextOpenSpellIndex -= cache[nextOpenCacheIndex - 1].SpellCount;

			armorSetCountById[cache[nextOpenCacheIndex - 1].Piece.ItemSetId]--;

			if (cache[nextOpenCacheIndex - 1].Piece.CalcedStartingArmorLevel > 0)
				TotalBaseArmorLevel -= (cache[nextOpenCacheIndex - 1].Piece.CalcedStartingArmorLevel * cache[nextOpenCacheIndex - 1].Slot.GetTotalBitsSet());

			if (cache[nextOpenCacheIndex - 1].Slot.IsBodyArmor())
				TotalBodyArmorPieces--;

			nextOpenCacheIndex--;
	}

		public bool SlotIsOpen(EquippableSlotFlags slot)
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

		public bool CanGetBeneficialSpellFrom(SuitBuildableMyWorldObject item)
		{
			// This whole approach needs to be optimized.
			// This is the biggest time waster in the entire search process.

			foreach (Spell itemSpell in item.SpellsToUseInSearch)
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

		public SuitBuilder Clone()
		{
			SuitBuilder newSuit = new SuitBuilder();

			for (int i = 0; i < nextOpenCacheIndex; i++)
				newSuit.Push(cache[i].Piece, cache[i].Slot);

			return newSuit;
		}

		public CompletedSuit CreateCompletedSuit()
		{
			CompletedSuit suit = new CompletedSuit();

			for (int i = 0; i < nextOpenCacheIndex; i++)
				suit.AddItem(cache[i].Slot, cache[i].Piece);

			return suit;
		}
	}
}
