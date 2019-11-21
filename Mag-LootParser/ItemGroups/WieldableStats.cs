using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mag.Shared.Constants;

namespace Mag_LootParser.ItemGroups
{
	class WieldableStats : ItemGroupStats
	{
		public readonly Dictionary<int, int> HitsByRawSkillWieldReqValue = new Dictionary<int, int>();
		public readonly Dictionary<int, int> HitsByLevelWieldReqValue = new Dictionary<int, int>();

		public WieldableStats(bool limitStatsToOnlyItemsWithWorkmanship = false) : base(limitStatsToOnlyItemsWithWorkmanship)
		{
		}

		public override void ProcessItem(IdentResponse item)
		{
			base.ProcessItem(item);

			if (!LimitStatsToOnlyItemsWithWorkmanship || item.LongValues.ContainsKey(IntValueKey.Workmanship))
			{
				if (item.LongValues.ContainsKey(IntValueKey.WieldReqType) && item.LongValues.ContainsKey(IntValueKey.WieldReqValue))
				{
					var wieldReqValue = item.LongValues[IntValueKey.WieldReqValue];

					if (item.LongValues[IntValueKey.WieldReqType] == (int)WieldRequirement.RawSkill)
					{
						if (!HitsByRawSkillWieldReqValue.ContainsKey(wieldReqValue))
							HitsByRawSkillWieldReqValue[wieldReqValue] = 1;
						else
							HitsByRawSkillWieldReqValue[wieldReqValue]++;
					}
					else if (item.LongValues[IntValueKey.WieldReqType] == (int)WieldRequirement.Level)
					{
						if (!HitsByLevelWieldReqValue.ContainsKey(wieldReqValue))
							HitsByLevelWieldReqValue[wieldReqValue] = 1;
						else
							HitsByLevelWieldReqValue[wieldReqValue]++;
					}
				}
			}
		}


		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append(base.ToString());

			sb.AppendLine();

			if (HitsByRawSkillWieldReqValue.Count > 0)
			{
				sb.AppendLine("Hits By WieldReqType: RawSkill, WeidlReqValue: ");
				var totalHitsByBuffLevels = HitsByRawSkillWieldReqValue.Values.Sum();
				var sortedKeys = HitsByRawSkillWieldReqValue.Keys.ToList();
				sortedKeys.Sort();
				foreach (var key in sortedKeys)
					sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByRawSkillWieldReqValue[key].ToString("N0").PadLeft(7) + " " + (HitsByRawSkillWieldReqValue[key] / (float)totalHitsByBuffLevels * 100).ToString("N1").PadLeft(6) + "%]");
			}

			if (HitsByLevelWieldReqValue.Count > 0)
			{
				sb.AppendLine("Hits By WieldReqType: Level, WeidlReqValue: ");
				var totalHitsByBuffLevels = HitsByLevelWieldReqValue.Values.Sum();
				var sortedKeys = HitsByLevelWieldReqValue.Keys.ToList();
				sortedKeys.Sort();
				foreach (var key in sortedKeys)
					sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByLevelWieldReqValue[key].ToString("N0").PadLeft(7) + " " + (HitsByLevelWieldReqValue[key] / (float)totalHitsByBuffLevels * 100).ToString("N1").PadLeft(6) + "%]");
			}

			return sb.ToString();
		}
	}
}
