using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mag.Shared.Spells;

namespace Mag_LootParser
{
    class SpellStats
    {
        public int TotalSpells;

        public int MinSpells;
        public int MaxSpells;

        /// <summary>
        /// buff level : hits
        /// </summary>
        public Dictionary<Spell.BuffLevels, int> HitsByBuffLevels = new Dictionary<Spell.BuffLevels, int>();

        /// <summary>
        /// cantrip id : hits
        /// </summary>
        public Dictionary<Spell.CantripLevels, int> HitsByCantrip = new Dictionary<Spell.CantripLevels, int>();

        /// <summary>
        /// spell id : hits
        /// </summary>
        public Dictionary<int, int> HitsBySpellID = new Dictionary<int, int>();


        public void ProcessItem(IdentResponse item)
        {
            TotalSpells += item.Spells.Count;

            if (item.Spells.Count < MinSpells) MinSpells = item.Spells.Count;
            if (item.Spells.Count > MaxSpells) MaxSpells = item.Spells.Count;

            foreach (var id in item.Spells)
            {
                var spell = SpellTools.GetSpell(id);

                HitsByBuffLevels.TryGetValue(spell.BuffLevel, out var count1);
                count1++;
                HitsByBuffLevels[spell.BuffLevel] = count1;

                HitsByCantrip.TryGetValue(spell.CantripLevel, out var count2);
                count2++;
                HitsByCantrip[spell.CantripLevel] = count2;

                HitsBySpellID.TryGetValue(id, out var count3);
                count3++;
                HitsBySpellID[id] = count3;
            }
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Total Spells: " + TotalSpells.ToString("N0") + "    Min Spells: " + MinSpells.ToString("N0") + "    Max Spells: " + MaxSpells.ToString("N0"));

            sb.AppendLine("Hits By Buff Level: ");
            var sortedKeys1 = HitsByBuffLevels.Keys.ToList();
            sortedKeys1.Sort();
            foreach (var key in sortedKeys1)
                sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByBuffLevels[key].ToString("N0").PadLeft(6) + " " + (HitsByBuffLevels[key] / (float)TotalSpells * 100).ToString("N1").PadLeft(4) + "%]");

            sb.AppendLine("Hits By Cantrip ID: ");
            var sortedKeys2 = HitsByCantrip.Keys.ToList();
            sortedKeys2.Sort();
            foreach (var key in sortedKeys2)
                sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByCantrip[key].ToString("N0").PadLeft(6) + " " + (HitsByCantrip[key] / (float)TotalSpells * 100).ToString("N1").PadLeft(4) + "%]");

            sb.AppendLine("Hits By Spell ID: ");
            var sortedKeys3 = HitsBySpellID.Keys.ToList();
            sortedKeys3.Sort();
            foreach (var key in sortedKeys3)
                sb.AppendLine(key.ToString().PadLeft(4) + " [" + HitsBySpellID[key].ToString("N0").PadLeft(6) + " " + (HitsBySpellID[key] / (float)TotalSpells * 100).ToString("N1").PadLeft(4) + "%] " + SpellTools.GetSpell(key).Name);

            return sb.ToString();
        }
    }
}
