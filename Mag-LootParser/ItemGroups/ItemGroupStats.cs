using System.Collections.Generic;
using System.Text;

namespace Mag_LootParser.ItemGroups
{
    class ItemGroupStats
    {
        public readonly List<IdentResponse> Items = new List<IdentResponse>();

        public readonly SpellStats SpellStats = new SpellStats();


        public virtual void ProcessItem(IdentResponse item)
        {
            Items.Add(item);

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
