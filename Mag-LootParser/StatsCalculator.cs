using System.Collections.Generic;

namespace Mag_LootParser
{
    static class StatsCalculator
    {
        public static readonly Dictionary<int, Stats> StatsByLootTier = new Dictionary<int, Stats>();

        public static readonly Dictionary<string, Stats> StatsByContainerName = new Dictionary<string, Stats>();

        public static void Calculate(Dictionary<string, Dictionary<int, List<IdentResponse>>> containersLoot)
        {
            StatsByLootTier.Clear();

            StatsByContainerName.Clear();

            // Create empty stats for every loot tier
            for (int i = -1; i <= 8; i++)
                StatsByLootTier[i] = new Stats();

            foreach (var kvp in containersLoot)
            {
                var containerByTier = StatsByLootTier[TierCalculator.GetTierByContainerName(kvp.Key)];
                var containerStats = new Stats();

                foreach (var container in kvp.Value)
                {
                    foreach (var item in container.Value)
                    {
                        containerByTier.ProcessItem(item);
                        containerStats.ProcessItem(item);
                    }
                }

                StatsByContainerName[kvp.Key] = containerStats;
            }
        }
    }
}
