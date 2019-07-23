using System.Collections.Generic;

using Mag.Shared.Constants;
using Mag.Shared.Spells;

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
                int tier = 0;
                bool isPlayerCorpse = false;

                foreach (var container in kvp.Value)
                {
                    if (isPlayerCorpse)
                        break;

                    foreach (var item in container.Value)
                    {
                        if ((item.LongValues.ContainsKey(IntValueKey.NumberTimesTinkered) && item.LongValues[IntValueKey.NumberTimesTinkered] > 0) || item.ActiveSpells.Count > 0)
                        {
                            isPlayerCorpse = true;
                            break;
                        }

                        // Heavy/Light/Finesse
                        if (item.LongValues.ContainsKey(IntValueKey.WieldReqAttribute) && (item.LongValues[IntValueKey.WieldReqAttribute] == 0x2C || item.LongValues[IntValueKey.WieldReqAttribute] == 0x2D || item.LongValues[IntValueKey.WieldReqAttribute] == 0x2E) && item.LongValues.ContainsKey(IntValueKey.WieldReqValue))
                        {
                            switch (item.LongValues[IntValueKey.WieldReqValue])
                            {
                                case 250:
                                    if (tier < 2) tier = 2;
                                    break;
                                case 300:
                                    if (tier < 3) tier = 3;
                                    break;
                                case 350:
                                    if (tier < 5) tier = 5;
                                    break;
                                case 370:
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
                        if (item.LongValues.ContainsKey(IntValueKey.WieldReqAttribute) && item.LongValues[IntValueKey.WieldReqAttribute] == 0x2F && item.LongValues.ContainsKey(IntValueKey.WieldReqValue))
                        {
                            switch (item.LongValues[IntValueKey.WieldReqValue])
                            {
                                case 250:
                                    if (tier < 2) tier = 2;
                                    break;
                                case 270:
                                    if (tier < 3) tier = 3;
                                    break;
                                //case 290:
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
                        if (item.LongValues.ContainsKey(IntValueKey.WieldReqAttribute) && item.LongValues[IntValueKey.WieldReqAttribute] == 0x22 && item.LongValues.ContainsKey(IntValueKey.WieldReqValue))
                        {
                            switch (item.LongValues[IntValueKey.WieldReqValue])
                            {
                                //case 290:
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

                        if (item.LongValues.ContainsKey(IntValueKey.Workmanship))
                        {
                            if (item.LongValues[IntValueKey.Workmanship] == 8  && tier < 4) tier = 4;
                            if (item.LongValues[IntValueKey.Workmanship] == 9  && tier < 5) tier = 5;
                            if (item.LongValues[IntValueKey.Workmanship] == 10 && tier < 6) tier = 6;
                        }

                        foreach (var spellId in item.Spells)
                        {
                            var spell = SpellTools.GetSpell(spellId);

                            if (spell.CantripLevel >= Spell.CantripLevels.Epic      && tier < 7) tier = 7;
                            if (spell.CantripLevel >= Spell.CantripLevels.Legendary && tier < 8) tier = 8;
                        }
                    }
                }

                if (isPlayerCorpse)
                    LootTiers[kvp.Key] = -1;
                else
                    LootTiers[kvp.Key] = tier;
            }
        }

        /// <summary>
        /// Will return -1 if not found
        /// </summary>
        public static int GetTierByContainerName(string containerName)
        {
            if (LootTiers.TryGetValue(containerName, out var tier))
                return tier;

            return -1;
        }
    }
}
