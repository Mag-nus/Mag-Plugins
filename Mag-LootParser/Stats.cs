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
            { ObjectClass.MeleeWeapon, new ItemGroupStats(true) },
            { ObjectClass.MissileWeapon, new ItemGroupStats(true) },
            { ObjectClass.WandStaffOrb, new ItemGroupStats(true) },

            { ObjectClass.Armor, new ItemGroupStats(true) },
            { ObjectClass.Clothing, new ItemGroupStats(true) },

            { ObjectClass.Jewelry, new ItemGroupStats(true) },

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

            { ObjectClass.Misc, new ItemGroupStats() },

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

            if (item.LongValues.ContainsKey(IntValueKey.Workmanship))
            {
                if (!WorkmanshipHits.ContainsKey(item.LongValues[IntValueKey.Workmanship]))
                    WorkmanshipHits[item.LongValues[IntValueKey.Workmanship]] = 0;

                WorkmanshipHits[item.LongValues[IntValueKey.Workmanship]]++;
            }

            if (item.LongValues.ContainsKey(IntValueKey.Material))
            {
                if (!MaterialHits.ContainsKey((MaterialType)item.LongValues[IntValueKey.Material]))
                    MaterialHits[(MaterialType)item.LongValues[IntValueKey.Material]] = 0;

                MaterialHits[(MaterialType)item.LongValues[IntValueKey.Material]]++;
            }

            if (item.LongValues.ContainsKey(IntValueKey.Workmanship))
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
                sb.AppendLine(kvp.Key + ": ");
                sb.Append(kvp.Value);
            }

            return sb.ToString();
        }
    }
}
