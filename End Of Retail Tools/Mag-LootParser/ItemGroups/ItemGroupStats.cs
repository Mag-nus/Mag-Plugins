using System.Collections.Generic;
using System.Text;

using Mag.Shared.Constants;

namespace Mag_LootParser.ItemGroups
{
    class ItemGroupStats
    {
        public readonly List<IdentResponse> Items = new List<IdentResponse>();

        public readonly bool LimitStatsToOnlyItemsWithWorkmanship;

        /// <summary>
        /// SpellStats for items with Workmanship
        /// </summary>
        public readonly SpellStats SpellStats = new SpellStats();


        public ItemGroupStats(bool limitStatsToOnlyItemsWithWorkmanship = false)
        {
            LimitStatsToOnlyItemsWithWorkmanship = limitStatsToOnlyItemsWithWorkmanship;
        }

        public virtual void ProcessItem(IdentResponse item)
        {
            Items.Add(item);

            if (!LimitStatsToOnlyItemsWithWorkmanship || item.LongValues.ContainsKey(IntValueKey.ItemWorkmanship))
	            SpellStats.ProcessItem(item);
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Total Items: " + Items.Count.ToString("N0"));
            sb.AppendLine();

            if (SpellStats.TotalSpells > 0)
            {
                sb.AppendLine("Spell Stats: ");
                sb.Append(SpellStats);
            }

            return sb.ToString();
        }
    }
}
