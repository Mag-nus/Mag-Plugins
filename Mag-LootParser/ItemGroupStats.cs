using System.Text;

namespace Mag_LootParser
{
    class ItemGroupStats
    {
        public int TotalItems;

        public readonly SpellStats SpellStats = new SpellStats();


        public void ProcessItem(IdentResponse item)
        {
            TotalItems++;

            SpellStats.ProcessItem(item);
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Total Items: " + TotalItems.ToString("N0"));
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
