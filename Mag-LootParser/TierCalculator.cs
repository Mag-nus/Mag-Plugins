using System;
using System.Collections.Generic;

using Mag.Shared.Constants;
using Mag.Shared.Spells;

namespace Mag_LootParser
{
    static class TierCalculator
    {
        /// <summary>
        /// Will return -1 for player, 0 if a tier was unable to be calculated, or the tier #.
        /// </summary>
        public static int Calculate(IEnumerable<IdentResponse> items)
        {
            int tier = 0;

            // Calculate the loot tiers
            // Reference: http://asheron.wikia.com/wiki/Loot
            foreach (var item in items)
            {
                if ((item.LongValues.ContainsKey(IntValueKey.NumberTimesTinkered) && item.LongValues[IntValueKey.NumberTimesTinkered] > 0) || item.ActiveSpells.Count > 0)
                    return -1;

                // Exclude trophy items from tier calculation
                if (!item.LongValues.ContainsKey(IntValueKey.Workmanship))
                    goto bypassNonRandomLootGenItems;

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
                    if (item.LongValues[IntValueKey.Workmanship] ==  1 && tier < 1) tier = 1;
                    if (item.LongValues[IntValueKey.Workmanship] ==  2 && tier < 1) tier = 1;
                    if (item.LongValues[IntValueKey.Workmanship] ==  3 && tier < 1) tier = 1;
                    if (item.LongValues[IntValueKey.Workmanship] ==  4 && tier < 1) tier = 1;
                    if (item.LongValues[IntValueKey.Workmanship] ==  5 && tier < 1) tier = 1;
                    if (item.LongValues[IntValueKey.Workmanship] ==  6 && tier < 2) tier = 2;
                    if (item.LongValues[IntValueKey.Workmanship] ==  7 && tier < 3) tier = 3;
                    if (item.LongValues[IntValueKey.Workmanship] ==  8 && tier < 4) tier = 4;
                    if (item.LongValues[IntValueKey.Workmanship] ==  9 && tier < 5) tier = 5;
                    if (item.LongValues[IntValueKey.Workmanship] == 10 && tier < 6) tier = 6;
                }

                bypassNonRandomLootGenItems:

                // Gems in higher tiers can come with low spells, so we don't do spell checks on these objects
                if (item.LongValues.ContainsKey(IntValueKey.Workmanship))
                {
                    foreach (var spellId in item.Spells)
                    {
                        var spell = SpellTools.GetSpell(spellId);

                        if (spell.BuffLevel == Spell.BuffLevels.I    && tier < 1) tier = 1;
                        if (spell.BuffLevel == Spell.BuffLevels.II   && tier < 1) tier = 1;
                        if (spell.BuffLevel == Spell.BuffLevels.III  && tier < 1) tier = 1;
                        if (spell.BuffLevel == Spell.BuffLevels.IV   && tier < 2) tier = 2;
                        if (spell.BuffLevel == Spell.BuffLevels.V    && tier < 2) tier = 2;
                        if (spell.BuffLevel == Spell.BuffLevels.VI   && tier < 3) tier = 3;
                        if (spell.BuffLevel == Spell.BuffLevels.VII  && tier < 5) tier = 5;
                        if (spell.BuffLevel == Spell.BuffLevels.VIII && tier < 7) tier = 7;

                        if (spell.CantripLevel == Spell.CantripLevels.Major     && tier < 2) tier = 2;
                        if (spell.CantripLevel == Spell.CantripLevels.Epic      && tier < 7) tier = 7;
                        if (spell.CantripLevel == Spell.CantripLevels.Legendary && tier < 8) tier = 8;
                    }
                }
            }

            return tier;
        }
    }
}
