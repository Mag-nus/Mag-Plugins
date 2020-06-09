using System;

namespace Mag_LootParser.ItemGroups
{
	class ClothingStats : WieldableStats
	{
		public ClothingStats(bool limitStatsToOnlyItemsWithWorkmanship = false) : base(limitStatsToOnlyItemsWithWorkmanship)
		{
		}
	}
}
