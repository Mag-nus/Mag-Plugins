using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mag.Shared.Constants;

namespace Mag_LootParser.ItemGroups
{
	class MiscGroupStats : ItemGroupStats
	{
		public readonly Dictionary<int, int> SummoningGemHitsByTotalRating = new Dictionary<int, int>();

		public readonly Dictionary<IdentResponse, int> HighRatingSummoningGems = new Dictionary<IdentResponse, int>();

		public override void ProcessItem(IdentResponse item)
		{
			base.ProcessItem(item);

			if (item.StringValues[StringValueKey.Name].Contains("Essence"))
			{
				int sum = 0;

				if (item.LongValues.ContainsKey(IntValueKey.GearDamage)) sum += item.LongValues[IntValueKey.GearDamage];
				if (item.LongValues.ContainsKey(IntValueKey.GearDamageResist)) sum += item.LongValues[IntValueKey.GearDamageResist];
				if (item.LongValues.ContainsKey(IntValueKey.GearCrit)) sum += item.LongValues[IntValueKey.GearCrit];
				if (item.LongValues.ContainsKey(IntValueKey.GearCritDamage)) sum += item.LongValues[IntValueKey.GearCritDamage];
				if (item.LongValues.ContainsKey(IntValueKey.GearCritResist)) sum += item.LongValues[IntValueKey.GearCritResist];
				if (item.LongValues.ContainsKey(IntValueKey.GearCritDamageResist)) sum += item.LongValues[IntValueKey.GearCritDamageResist];
				if (item.LongValues.ContainsKey(IntValueKey.GearHealingBoost)) sum += item.LongValues[IntValueKey.GearHealingBoost];
				if (item.LongValues.ContainsKey(IntValueKey.GearMaxHealth)) sum += item.LongValues[IntValueKey.GearMaxHealth];

				if (!SummoningGemHitsByTotalRating.ContainsKey(sum))
					SummoningGemHitsByTotalRating[sum] = 1;
				else
					SummoningGemHitsByTotalRating[sum]++;

				if (HighRatingSummoningGems.Count < 5) // Keep the top 5 gems
					HighRatingSummoningGems[item] = sum;
				else
				{
					var lowest = HighRatingSummoningGems.FirstOrDefault(r => r.Value < sum);

					if (lowest.Key != null)
					{
						HighRatingSummoningGems.Remove(lowest.Key);

						HighRatingSummoningGems[item] = sum;
					}
				}
			}
		}


		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append(base.ToString());

			sb.AppendLine();

			{
				var sortedKeys = SummoningGemHitsByTotalRating.Keys.ToList();
				sortedKeys.Sort();

				var totalHits = SummoningGemHitsByTotalRating.Values.Sum();

				sb.AppendLine("Total Summoning Gems: " + totalHits);

				foreach (var key in sortedKeys)
					sb.AppendLine($"Summoning Gem Hits By Total Rating {key.ToString().PadLeft(2)} [" + SummoningGemHitsByTotalRating[key].ToString().PadRight(4) + " " + (SummoningGemHitsByTotalRating[key] / (float)totalHits * 100).ToString("N2").PadLeft(4) + " %]");
			}

			{
				sb.AppendLine("Top 5 Gems");

				var sortedGems = HighRatingSummoningGems.OrderBy(r => r.Value);

				foreach (var kvp in sortedGems)
				{
					var item = kvp.Key;

					sb.Append(item.StringValues[StringValueKey.Name] + " [");
					bool first = true;
					if (item.LongValues.ContainsKey(IntValueKey.GearDamage) && item.LongValues[IntValueKey.GearDamage] > 0) { sb.Append("D " + item.LongValues[IntValueKey.GearDamage]); first = false; }
					if (item.LongValues.ContainsKey(IntValueKey.GearDamageResist) && item.LongValues[IntValueKey.GearDamageResist] > 0) { if (!first) sb.Append(", "); sb.Append("DR " + item.LongValues[IntValueKey.GearDamageResist]); first = false; }
					if (item.LongValues.ContainsKey(IntValueKey.GearCrit) && item.LongValues[IntValueKey.GearCrit] > 0) { if (!first) sb.Append(", "); sb.Append("C " + item.LongValues[IntValueKey.GearCrit]); first = false; }
					if (item.LongValues.ContainsKey(IntValueKey.GearCritDamage) && item.LongValues[IntValueKey.GearCritDamage] > 0) { if (!first) sb.Append(", "); sb.Append("CD " + item.LongValues[IntValueKey.GearCritDamage]); first = false; }
					if (item.LongValues.ContainsKey(IntValueKey.GearCritResist) && item.LongValues[IntValueKey.GearCritResist] > 0) { if (!first) sb.Append(", "); sb.Append("CR " + item.LongValues[IntValueKey.GearCritResist]); first = false; }
					if (item.LongValues.ContainsKey(IntValueKey.GearCritDamageResist) && item.LongValues[IntValueKey.GearCritDamageResist] > 0) { if (!first) sb.Append(", "); sb.Append("CDR " + item.LongValues[IntValueKey.GearCritDamageResist]); first = false; }
					if (item.LongValues.ContainsKey(IntValueKey.GearHealingBoost) && item.LongValues[IntValueKey.GearHealingBoost] > 0) { if (!first) sb.Append(", "); sb.Append("HB " + item.LongValues[IntValueKey.GearHealingBoost]); first = false; }
					if (item.LongValues.ContainsKey(IntValueKey.GearMaxHealth) && item.LongValues[IntValueKey.GearMaxHealth] > 0) { if (!first) sb.Append(", "); sb.Append("V " + item.LongValues[IntValueKey.GearMaxHealth]); first = false; }
					sb.Append("]");
					sb.AppendLine();
				}
			}

			return sb.ToString();
		}
	}
}
