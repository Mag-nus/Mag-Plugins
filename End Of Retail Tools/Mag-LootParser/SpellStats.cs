using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mag.Shared.Constants;
using Mag.Shared.Spells;

namespace Mag_LootParser
{
    class SpellStats
    {
        public int TotalItems;
        public int TotalSpells;

        public int MinSpells;
        public int MaxSpells;


        public Dictionary<Spell.CantripLevels, int> HitsByCantripLevel = new Dictionary<Spell.CantripLevels, int>();

        public Dictionary<int, int> CantripCountHits = new Dictionary<int, int>();

        public Dictionary<int, int> CantripHitsBySpellID = new Dictionary<int, int>();

        public Dictionary<int, int> CantripHitsBySpellCategory = new Dictionary<int, int>();


		public Dictionary<Spell.BuffLevels, int> HitsByBuffLevel = new Dictionary<Spell.BuffLevels, int>();

        public Dictionary<int, int> BuffCountHits = new Dictionary<int, int>();

        public Dictionary<int, int> BuffHitsBySpellID = new Dictionary<int, int>();

        public Dictionary<int, int> BuffHitsBySpellCategory = new Dictionary<int, int>();


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
                    HitsByBuffLevel.TryGetValue(spell.BuffLevel, out var count1);
                    count1++;
                    HitsByBuffLevel[spell.BuffLevel] = count1;

                    buffCount++;


					BuffHitsBySpellID.TryGetValue(id, out var count3);
                    count3++;
                    BuffHitsBySpellID[id] = count3;

                    BuffHitsBySpellCategory.TryGetValue(spell.Family, out var count4);
                    count4++;
                    BuffHitsBySpellCategory[spell.Family] = count4;
				}

                if (spell.CantripLevel != Spell.CantripLevels.None)
                {
                    HitsByCantripLevel.TryGetValue(spell.CantripLevel, out var count2);
                    count2++;
                    HitsByCantripLevel[spell.CantripLevel] = count2;

                    cantripCount++;


                    CantripHitsBySpellID.TryGetValue(id, out var count3);
                    count3++;
                    CantripHitsBySpellID[id] = count3;

                    CantripHitsBySpellCategory.TryGetValue(spell.Family, out var count4);
                    count4++;
                    CantripHitsBySpellCategory[spell.Family] = count4;
				}
			}

            if (!BuffCountHits.ContainsKey(buffCount))
                BuffCountHits.Add(buffCount, 0);
            BuffCountHits[buffCount]++;

            if (!CantripCountHits.ContainsKey(cantripCount))
                CantripCountHits.Add(cantripCount, 0);
            CantripCountHits[cantripCount]++;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Total Items: " + TotalItems.ToString("N0") + "    Total Spells: " + TotalSpells.ToString("N0") + "    Min Spells: " + MinSpells.ToString("N0") + "    Max Spells: " + MaxSpells.ToString("N0"));

            {
	            {
		            sb.AppendLine("Hits By Buff Level: ");
		            var totalHitsByBuffLevels = HitsByBuffLevel.Values.Sum();
		            var sortedKeys = HitsByBuffLevel.Keys.ToList();
		            sortedKeys.Sort();
		            foreach (var key in sortedKeys)
			            sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByBuffLevel[key].ToString("N0").PadLeft(7) + " " + (HitsByBuffLevel[key] / (float) totalHitsByBuffLevels * 100).ToString("N1").PadLeft(6) + "%]");
	            }

	            sb.AppendLine("Buff Count Probabilities: ");
	            if (BuffCountHits.Count > 0)
	            {
		            for (int i = BuffCountHits.Keys.Min(); i <= BuffCountHits.Keys.Max(); i++)
		            {
			            if (BuffCountHits.ContainsKey(i))
				            sb.AppendLine(i.ToString().PadLeft(9) + " [" + BuffCountHits[i].ToString("N0").PadLeft(7) + " " + (BuffCountHits[i] / (float) TotalItems * 100).ToString("N3").PadLeft(6) + "%]");
			            else
				            sb.AppendLine(i.ToString().PadLeft(9) + " [" + "0".PadLeft(7) + " " + "]");
		            }
	            }

	            {
		            sb.AppendLine("Buff Hits By Spell ID: ");
		            var sortedKeys = BuffHitsBySpellID.Keys.ToList();
		            sortedKeys.Sort();
		            foreach (var key in sortedKeys)
			            sb.AppendLine(key.ToString().PadLeft(4) + " [" + BuffHitsBySpellID[key].ToString("N0").PadLeft(7) + " " + (BuffHitsBySpellID[key] / (float) TotalSpells * 100).ToString("N1").PadLeft(4) + "%] " + SpellTools.GetSpell(key).Name);
	            }

	            {
		            sb.AppendLine("Buff Hits By Spell Category: ");
		            var sortedKeys = BuffHitsBySpellCategory.Keys.ToList();
		            sortedKeys.Sort();
		            foreach (var key in sortedKeys)
			            sb.AppendLine(key.ToString().PadLeft(4) + " [" + BuffHitsBySpellCategory[key].ToString("N0").PadLeft(7) + " " + (BuffHitsBySpellCategory[key] / (float) TotalSpells * 100).ToString("N1").PadLeft(4) + "%] " + (SpellCategory)key);
				}
            }

			sb.AppendLine();

            {
	            {
		            sb.AppendLine("Hits By Cantrip Level: ");
		            var totalHitsByCantrip = HitsByCantripLevel.Values.Sum();
		            var sortedKeys = HitsByCantripLevel.Keys.ToList();
		            sortedKeys.Sort();
		            foreach (var key in sortedKeys)
			            sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByCantripLevel[key].ToString("N0").PadLeft(7) + " " + (HitsByCantripLevel[key] / (float) totalHitsByCantrip * 100).ToString("N1").PadLeft(6) + "%]");
	            }

	            sb.AppendLine("Cantrip Count Probabilities: ");
	            if (CantripCountHits.Count > 0)
	            {
		            for (int i = CantripCountHits.Keys.Min(); i <= CantripCountHits.Keys.Max(); i++)
		            {
			            if (CantripCountHits.ContainsKey(i))
				            sb.AppendLine(i.ToString().PadLeft(9) + " [" + CantripCountHits[i].ToString("N0").PadLeft(7) + " " + (CantripCountHits[i] / (float) TotalItems * 100).ToString("N3").PadLeft(6) + "%]");
			            else
				            sb.AppendLine(i.ToString().PadLeft(9) + " [" + "0".PadLeft(7) + " " + "]");
		            }
	            }

	            {
		            sb.AppendLine("Cantrip Hits By Spell ID: ");
		            var sortedKeys = CantripHitsBySpellID.Keys.ToList();
		            sortedKeys.Sort();
		            foreach (var key in sortedKeys)
			            sb.AppendLine(key.ToString().PadLeft(4) + " [" + CantripHitsBySpellID[key].ToString("N0").PadLeft(7) + " " + (CantripHitsBySpellID[key] / (float) TotalSpells * 100).ToString("N1").PadLeft(4) + "%] " + SpellTools.GetSpell(key).Name);
	            }

	            {
		            sb.AppendLine("Cantrip Hits By Spell Category: ");
		            var sortedKeys = CantripHitsBySpellCategory.Keys.ToList();
		            sortedKeys.Sort();
		            foreach (var key in sortedKeys)
			            sb.AppendLine(key.ToString().PadLeft(4) + " [" + CantripHitsBySpellCategory[key].ToString("N0").PadLeft(7) + " " + (CantripHitsBySpellCategory[key] / (float) TotalSpells * 100).ToString("N1").PadLeft(4) + "%] " + (SpellCategory)key);
	            }

            }

			return sb.ToString();
        }
    }
}
