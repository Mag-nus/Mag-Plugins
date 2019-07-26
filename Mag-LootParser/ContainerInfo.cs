using System;
using System.Collections.Generic;
using System.Linq;

namespace Mag_LootParser
{
    class ContainerInfo
    {
        public uint Id;

        public string Name;

        public int Landcell;

        public string Location;

        public readonly List<IdentResponse> Items = new List<IdentResponse>();

        private int? tier;

        /// <summary>
        /// This is the tier calculated based on Items.
        /// It may not be the actual tier for the parent creature.
        ///  -1 = Player Corpse
        ///   0 = Not Identified
        /// 1-8 = Retail Tier
        /// </summary>
        public int Tier
        {
            get
            {
                if (!tier.HasValue)
                    tier = TierCalculator.Calculate(Items);

                return tier.Value;
            }
        }
    }

    static class ContainerInfoExtensions
    {
        /// <summary>
        /// Make sure you have called UpdateVariablesAfterAllItmesHaveBeenAdded() on all the containerInfos first
        /// Will return -1 for player, 0 if a tier was unable to be calculated, or the tier #.
        /// </summary>
        public static int GetTier(this ICollection<ContainerInfo> containerInfos)
        {
            int tier = 0;

            foreach (var containerInfo in containerInfos)
            {
                if (containerInfo.Tier == -1)
                    return -1;

                if (containerInfo.Tier > tier)
                    tier = containerInfo.Tier;
            }

            return tier;
        }

        public static Dictionary<int, List<ContainerInfo>> GroupContainerInfosByTier(this ICollection<ContainerInfo> containerInfos)
        {
            var results = new Dictionary<int, List<ContainerInfo>>();

            var name = containerInfos.First().Name;

            // Test Code
            /*var sorted = containerInfos.OrderBy(r => r.Tier).ToList();

            var output = "";

            foreach (var sort in sorted)
                output += sort.Tier + " " + sort.Landcell.ToString("X8") + " " + sort.Location + Environment.NewLine;

            System.Windows.Forms.Clipboard.SetText(output);*/

            if (name == "Corpse of Panumbris Shadow")
            {
                results[4] = new List<ContainerInfo>(); // Panumbris Shadow (80)
                results[8] = new List<ContainerInfo>(); // Panumbris Shadow (240)

                foreach (var containerInfo in containerInfos)
                {
                    if (containerInfo.Tier >= 5)
                        results[8].Add(containerInfo);
                    else
                        results[4].Add(containerInfo);
                }
            }
            else if (name == "Corpse of Tumerok Major")
            {
                results[2] = new List<ContainerInfo>(); // Tumerok Major (50)
                results[3] = new List<ContainerInfo>(); // Tumerok Major (80)

                foreach (var containerInfo in containerInfos)
                {
                    if (containerInfo.Tier >= 3)
                        results[3].Add(containerInfo);
                    else
                        results[2].Add(containerInfo);
                }
            }
            else if (name == "Corpse of Olthoi Larvae")
            {
                // todo these come in different tiers, loot tiers seen: 0,2,3,4,5,6
            }
            else if (name == "Corpse of Falatacot Consort")
            {
                // todo these come in different tiers, loot tiers seen: 2,3,4,5,6,7
            }
            else if (name == "Corpse of Zombie")
            {
                results[1] = new List<ContainerInfo>(); // Zombie (Level 15)
                results[2] = new List<ContainerInfo>(); // ??
                results[3] = new List<ContainerInfo>(); // Zombie (Level 50)

                foreach (var containerInfo in containerInfos)
                {
                    if (containerInfo.Tier >= 3)
                        results[3].Add(containerInfo);
                    else if (containerInfo.Tier >= 2)
                        results[2].Add(containerInfo);
                    else
                        results[1].Add(containerInfo);
                }
            }
            else if (name == "Corpse of Mercenary")
            {
                results[3] = new List<ContainerInfo>(); // Mercenary (Level 80)
                results[4] = new List<ContainerInfo>(); // Mercenary (Level 115)

                foreach (var containerInfo in containerInfos)
                {
                    if (containerInfo.Landcell >> 16 == 0x004A)
                        results[4].Add(containerInfo);
                    else
                        results[3].Add(containerInfo);
                }
            }
            else if (name == "Corpse of Raven Hunter")
            {
                results[5] = new List<ContainerInfo>(); // Raven Hunter (Level 135)
                results[6] = new List<ContainerInfo>(); // Raven Hunter (Level 160)

                foreach (var containerInfo in containerInfos)
                {
                    if (containerInfo.Tier >= 6)
                        results[6].Add(containerInfo);
                    else
                        results[5].Add(containerInfo);
                }
            }
            else
            {
                var tier = containerInfos.GetTier();

                results[tier] = new List<ContainerInfo>(containerInfos);
            }

            return results;
        }
    }
}
