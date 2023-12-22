using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mag.Shared;
using Mag.Shared.Constants;

using Mag_LootParser.ItemGroups;

namespace Mag_LootParser
{
    class Stats
    {
        public readonly string ContainerName;
        public readonly int Tier;

        public int TotalContainers;
        public int TotalItems;

        public readonly Dictionary<int, int> WorkmanshipHits = new Dictionary<int, int>();

        public readonly Dictionary<MaterialType, int> MaterialHits = new Dictionary<MaterialType, int>();

        /// <summary>
        /// SpellStats for items with Workmanship
        /// </summary>
        public readonly SpellStats SpellStats = new SpellStats();

        public readonly Dictionary<ObjectClass, ItemGroupStats> ObjectClasses = new Dictionary<ObjectClass, ItemGroupStats>
        {
            { ObjectClass.MeleeWeapon, new MeleeWeaponStats(true) },
            { ObjectClass.MissileWeapon, new MissileWeaponStats(true) },
            { ObjectClass.WandStaffOrb, new WandStaffOrbStats(true) },

            { ObjectClass.Armor, new WieldableStats(true) },
            { ObjectClass.Clothing, new ClothingStats(true) },

			{ ObjectClass.Jewelry, new WieldableStats(true) },

            { ObjectClass.Gem, new GemStats() },

            { ObjectClass.Scroll, new ItemGroupStats() },
            { ObjectClass.SpellComponent, new ItemGroupStats() },

            { ObjectClass.HealingKit, new ItemGroupStats() },
            { ObjectClass.Food, new ItemGroupStats() },

            { ObjectClass.BaseCooking, new ItemGroupStats() },
            { ObjectClass.CraftedAlchemy, new ItemGroupStats() },

            { ObjectClass.ManaStone, new ItemGroupStats() },

            { ObjectClass.Key, new ItemGroupStats() },
            { ObjectClass.Lockpick, new ItemGroupStats() },

            { ObjectClass.Book, new ItemGroupStats() },
            { ObjectClass.Journal, new ItemGroupStats() },

            { ObjectClass.Money, new ItemGroupStats() },
            { ObjectClass.TradeNote, new ItemGroupStats() },

            { ObjectClass.Bundle, new ItemGroupStats() },

            { ObjectClass.Misc, new MiscGroupStats() },

            { ObjectClass.Unknown, new ItemGroupStats() },
        };

        public Stats(string containerName, int tier)
        {
            ContainerName = containerName;
            Tier = tier;
        }

        public void ProcessItem(IdentResponse item)
        {
            TotalItems++;

            if (item.LongValues.ContainsKey(IntValueKey.ItemWorkmanship))
            {
                if (!WorkmanshipHits.ContainsKey(item.LongValues[IntValueKey.ItemWorkmanship]))
                    WorkmanshipHits[item.LongValues[IntValueKey.ItemWorkmanship]] = 0;

                WorkmanshipHits[item.LongValues[IntValueKey.ItemWorkmanship]]++;
            }

            if (item.LongValues.ContainsKey(IntValueKey.MaterialType))
            {
                if (!MaterialHits.ContainsKey((MaterialType)item.LongValues[IntValueKey.MaterialType]))
                    MaterialHits[(MaterialType)item.LongValues[IntValueKey.MaterialType]] = 0;

                MaterialHits[(MaterialType)item.LongValues[IntValueKey.MaterialType]]++;
            }

            if (item.LongValues.ContainsKey(IntValueKey.ItemWorkmanship))
                SpellStats.ProcessItem(item);

            ObjectClasses[item.ObjectClass].ProcessItem(item);
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Tier: " + Tier);
            sb.AppendLine("Total Containers: " + TotalContainers.ToString("N0"));
            sb.AppendLine("Total Items: " + TotalItems.ToString("N0"));
            sb.AppendLine();

            sb.AppendLine("Workmanship Hits: ");
            var totalWorkmanshipHits = WorkmanshipHits.Values.Sum();
            foreach (var kvp in WorkmanshipHits.OrderBy(i => i.Key))
                sb.AppendLine(kvp.Key.ToString().PadLeft(2) + " [" + kvp.Value.ToString("N0").PadLeft(6) + " " + (kvp.Value / (float)totalWorkmanshipHits * 100).ToString("N1").PadLeft(4) + "%]");
            sb.AppendLine();

            // This is really dependant on the type of object being dropped
            sb.AppendLine("Material Hits: ");
            var totalMaterialHits = MaterialHits.Values.Sum();
            foreach (var kvp in MaterialHits.OrderBy(i => i.Key))
                sb.AppendLine(kvp.Key.ToString().PadRight(15) + " [" + kvp.Value.ToString("N0").PadLeft(6) + " " + (kvp.Value / (float)totalMaterialHits * 100).ToString("N1").PadLeft(4) + "%]");
            sb.AppendLine();

            sb.AppendLine("Total Spell Stats for Items With Workmanship: ");
            sb.AppendLine(SpellStats.ToString());
            sb.AppendLine();

            foreach (var kvp in ObjectClasses)
            {
                if (kvp.Value.Items.Count == 0)
                    continue;

                sb.AppendLine();
                sb.AppendLine("==============================================================================");
                sb.AppendLine(kvp.Key + ": ");
                sb.Append(kvp.Value);
                sb.AppendLine("==============================================================================");
			}

            return sb.ToString();
        }
    }
}
