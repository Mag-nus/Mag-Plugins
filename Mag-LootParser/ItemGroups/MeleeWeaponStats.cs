using System;

namespace Mag_LootParser.ItemGroups
{
	class MeleeWeaponStats : WieldableStats
	{
		public MeleeWeaponStats(bool limitStatsToOnlyItemsWithWorkmanship = false) : base(limitStatsToOnlyItemsWithWorkmanship)
		{
		}
	}
}
