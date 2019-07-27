using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mag.Shared.Spells;

namespace Mag_LootParser
{
    class SpellStats
    {
        public int TotalItems;
        public int TotalSpells;

        public int MinSpells;
        public int MaxSpells;

        /// <summary>
        /// cantrip id : hits
        /// </summary>
        public Dictionary<Spell.CantripLevels, int> HitsByCantrip = new Dictionary<Spell.CantripLevels, int>();

        public Dictionary<int, int> CantripCountHits = new Dictionary<int, int>();

        /// <summary>
        /// buff level : hits
        /// </summary>
        public Dictionary<Spell.BuffLevels, int> HitsByBuffLevels = new Dictionary<Spell.BuffLevels, int>();

        public Dictionary<int, int> BuffLevelCountHits = new Dictionary<int, int>();

        /// <summary>
        /// spell id : hits
        /// </summary>
        public Dictionary<int, int> HitsBySpellID = new Dictionary<int, int>();


        private static readonly List<string> unknownSpells = new List<string> { "Id,Name,Difficulty,Duration" };

        public void ProcessItem(IdentResponse item)
        {
            TotalItems++;
            TotalSpells += item.Spells.Count;

            if (item.Spells.Count < MinSpells) MinSpells = item.Spells.Count;
            if (item.Spells.Count > MaxSpells) MaxSpells = item.Spells.Count;

            var cantripCount = 0;
            var buffCount = 0;

            foreach (var id in item.Spells)
            {
                var spell = SpellTools.GetSpell(id);

                if (spell.BuffLevel != Spell.BuffLevels.None)
                {
                    HitsByBuffLevels.TryGetValue(spell.BuffLevel, out var count1);
                    count1++;
                    HitsByBuffLevels[spell.BuffLevel] = count1;

                    buffCount++;
                }

                if (spell.CantripLevel != Spell.CantripLevels.None)
                {
                    HitsByCantrip.TryGetValue(spell.CantripLevel, out var count2);
                    count2++;
                    HitsByCantrip[spell.CantripLevel] = count2;

                    cantripCount++;
                }

                HitsBySpellID.TryGetValue(id, out var count3);
                count3++;
                HitsBySpellID[id] = count3;
            }

            if (!BuffLevelCountHits.ContainsKey(buffCount))
                BuffLevelCountHits.Add(buffCount, 0);
            BuffLevelCountHits[buffCount]++;

            if (!CantripCountHits.ContainsKey(cantripCount))
                CantripCountHits.Add(cantripCount, 0);
            CantripCountHits[cantripCount]++;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Total Items: " + TotalItems.ToString("N0") + "    Total Spells: " + TotalSpells.ToString("N0") + "    Min Spells: " + MinSpells.ToString("N0") + "    Max Spells: " + MaxSpells.ToString("N0"));

            sb.AppendLine("Hits By Buff Level: ");
            var totalHitsByBuffLevels = HitsByBuffLevels.Values.Sum();
            var sortedKeys1 = HitsByBuffLevels.Keys.ToList();
            sortedKeys1.Sort();
            foreach (var key in sortedKeys1)
                sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByBuffLevels[key].ToString("N0").PadLeft(7) + " " + (HitsByBuffLevels[key] / (float)totalHitsByBuffLevels * 100).ToString("N1").PadLeft(6) + "%]");

            sb.AppendLine("Buff Count Probabilities: ");
            if (BuffLevelCountHits.Count > 0)
            {
                for (int i = BuffLevelCountHits.Keys.Min(); i <= BuffLevelCountHits.Keys.Max(); i++)
                {
                    if (BuffLevelCountHits.ContainsKey(i))
                        sb.AppendLine(i.ToString().PadLeft(9) + " [" + BuffLevelCountHits[i].ToString("N0").PadLeft(7) + " " + (BuffLevelCountHits[i] / (float)TotalItems * 100).ToString("N3").PadLeft(6) + "%]");
                    else
                        sb.AppendLine(i.ToString().PadLeft(9) + " [" + "0".PadLeft(7) + " " + "]");
                }
            }

            sb.AppendLine("Hits By Cantrip ID: ");
            var totalHitsByCantrip = HitsByCantrip.Values.Sum();
            var sortedKeys2 = HitsByCantrip.Keys.ToList();
            sortedKeys2.Sort();
            foreach (var key in sortedKeys2)
                sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByCantrip[key].ToString("N0").PadLeft(7) + " " + (HitsByCantrip[key] / (float)totalHitsByCantrip * 100).ToString("N1").PadLeft(6) + "%]");

            sb.AppendLine("Cantrip Count Probabilities: ");
            if (CantripCountHits.Count > 0)
            {
                for (int i = CantripCountHits.Keys.Min(); i <= CantripCountHits.Keys.Max(); i++)
                {
                    if (CantripCountHits.ContainsKey(i))
                        sb.AppendLine(i.ToString().PadLeft(9) + " [" + CantripCountHits[i].ToString("N0").PadLeft(7) + " " + (CantripCountHits[i] / (float)TotalItems * 100).ToString("N3").PadLeft(6) + "%]");
                    else
                        sb.AppendLine(i.ToString().PadLeft(9) + " [" + "0".PadLeft(7) + " " + "]");
                }
            }

            sb.AppendLine("Hits By Spell ID: ");
            var sortedKeys3 = HitsBySpellID.Keys.ToList();
            sortedKeys3.Sort();
            //foreach (var key in sortedKeys3)
            //    sb.AppendLine(key.ToString().PadLeft(4) + " [" + HitsBySpellID[key].ToString("N0").PadLeft(7) + " " + (HitsBySpellID[key] / (float)TotalSpells * 100).ToString("N1").PadLeft(4) + "%] " + SpellTools.GetSpell(key).Name);


            return sb.ToString();
        }
    }
}
