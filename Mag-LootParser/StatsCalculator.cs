using System.Collections.Generic;

namespace Mag_LootParser
{
    static class StatsCalculator
    {
        public static readonly Dictionary<int, Stats> StatsByLootTier = new Dictionary<int, Stats>();

        public static readonly List<Stats> StatsByContainerNameAndTier = new List<Stats>();

        public static void Calculate(Dictionary<string, List<ContainerInfo>> containersLoot)
        {
            StatsByLootTier.Clear();

            StatsByContainerNameAndTier.Clear();

            // Create empty stats for every loot tier
            for (int i = 0; i <= 8; i++)
                StatsByLootTier[i] = new Stats(null, i);

            foreach (var kvp in containersLoot)
            {
                var containerInfoGroups = kvp.Value.GroupContainerInfosByTier();

                foreach (var containerInfoGroup in containerInfoGroups)
                {
                    if (containerInfoGroup.Key == -1) // Player container
                        continue;

                    var containerByTier = StatsByLootTier[containerInfoGroup.Key];
                    var containerStats = new Stats(kvp.Key, containerInfoGroup.Key);

                    foreach (var container in containerInfoGroup.Value)
                    {
                        containerByTier.TotalContainers++;
                        containerStats.TotalContainers++;

                        foreach (var item in container.Items)
                        {
                            containerByTier.ProcessItem(item);
                            containerStats.ProcessItem(item);
                        }
                    }

                    StatsByContainerNameAndTier.Add(containerStats);
                }
            }
        }
    }
}
