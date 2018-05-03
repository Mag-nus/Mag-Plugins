using System.Collections.Generic;

using Mag.Shared.Constants;

namespace Mag_LootParser
{
    static class TierCalculator
    {
        public static readonly Dictionary<string, int> LootTiers = new Dictionary<string, int>();

        public static void Calculate(Dictionary<string, Dictionary<int, List<IdentResponse>>> containersLoot)
        {
            // Calculate the loot tiers
            // Reference: http://asheron.wikia.com/wiki/Loot
            LootTiers.Clear();

            foreach (var kvp in containersLoot)
            {
                int tier = -1;

                foreach (var container in kvp.Value)
                {
                    foreach (var item in container.Value)
                    {
                        // Heavy/Light/Finesse
                        if (item.LongValues.ContainsKey(IntValueKey.WieldReqAttribute) && (item.LongValues[IntValueKey.WieldReqAttribute] == 0x2C || item.LongValues[IntValueKey.WieldReqAttribute] == 0x2D || item.LongValues[IntValueKey.WieldReqAttribute] == 0x2E) && item.LongValues.ContainsKey(IntValueKey.WieldReqValue))
                        {
                            switch (item.LongValues[IntValueKey.WieldReqValue])
                            {
                                case 250:
                                    if (tier < 2) tier = 2;
                                    break;
                                case 300:
                                    // Could be tier 3 as well
                                    if (tier < 4) tier = 4;
                                    break;
                                case 350:
                                    if (tier < 5) tier = 5;
                                    break;
                                case 400:
                                    if (tier < 6) tier = 6;
                                    break;
                                case 420:
                                    if (tier < 7) tier = 7;
                                    break;
                                case 430:
                                    tier = 8;
                                    break;
                            }
                        }

                        // Missile
                        if (tier == 0 && item.LongValues.ContainsKey(IntValueKey.WieldReqAttribute) && item.LongValues[IntValueKey.WieldReqAttribute] == 0x2F && item.LongValues.ContainsKey(IntValueKey.WieldReqValue))
                        {
                            switch (item.LongValues[IntValueKey.WieldReqValue])
                            {
                                case 250:
                                    if (tier < 2) tier = 2;
                                    break;
                                case 270:
                                    // Could be tier 3 as well
                                    if (tier < 4) tier = 4;
                                    break;
                                case 315:
                                    if (tier < 5) tier = 5;
                                    break;
                                case 360:
                                    if (tier < 6) tier = 6;
                                    break;
                                case 375:
                                    if (tier < 7) tier = 7;
                                    break;
                                case 385:
                                    tier = 8;
                                    break;
                            }
                        }

                        // Magic
                        if (tier == 0 && item.LongValues.ContainsKey(IntValueKey.WieldReqAttribute) && item.LongValues[IntValueKey.WieldReqAttribute] == 0x22 && item.LongValues.ContainsKey(IntValueKey.WieldReqValue))
                        {
                            switch (item.LongValues[IntValueKey.WieldReqValue])
                            {
                                case 310:
                                    if (tier < 5) tier = 5;
                                    break;
                                case 355:
                                    if (tier < 6) tier = 6;
                                    break;
                                case 375:
                                    if (tier < 7) tier = 7;
                                    break;
                                case 385:
                                    tier = 8;
                                    break;
                            }
                        }
                    }
                }

                LootTiers[kvp.Key] = tier;
            }
        }

        /// <summary>
        /// Will return -1 if not found
        /// </summary>
        public static  int GetTierByContainerName(string containerName)
        {
            if (LootTiers.TryGetValue(containerName, out var tier))
                return tier;

            return -1;
        }
    }
}
