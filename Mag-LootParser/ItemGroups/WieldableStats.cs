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

			if (!LimitStatsToOnlyItemsWithWorkmanship || item.LongValues.ContainsKey(IntValueKey.ItemWorkmanship))
			{
				if (item.LongValues.ContainsKey(IntValueKey.WieldRequirements) && item.LongValues.ContainsKey(IntValueKey.WieldDifficulty))
				{
					var wieldDifficulty = item.LongValues[IntValueKey.WieldDifficulty];

					if (item.LongValues[IntValueKey.WieldRequirements] == (int)WieldRequirement.RawSkill)
					{
						if (!HitsByRawSkillWieldReqValue.ContainsKey(wieldDifficulty))
							HitsByRawSkillWieldReqValue[wieldDifficulty] = 1;
						else
							HitsByRawSkillWieldReqValue[wieldDifficulty]++;
					}
					else if (item.LongValues[IntValueKey.WieldRequirements] == (int)WieldRequirement.Level)
					{
						if (!HitsByLevelWieldReqValue.ContainsKey(wieldDifficulty))
							HitsByLevelWieldReqValue[wieldDifficulty] = 1;
						else
							HitsByLevelWieldReqValue[wieldDifficulty]++;
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
				sb.AppendLine("Hits By WieldRequirements: RawSkill, WeidlReqValue: ");
				var totalHitsByBuffLevels = HitsByRawSkillWieldReqValue.Values.Sum();
				var sortedKeys = HitsByRawSkillWieldReqValue.Keys.ToList();
				sortedKeys.Sort();
				foreach (var key in sortedKeys)
					sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByRawSkillWieldReqValue[key].ToString("N0").PadLeft(7) + " " + (HitsByRawSkillWieldReqValue[key] / (float)totalHitsByBuffLevels * 100).ToString("N1").PadLeft(6) + "%]");
			}

			if (HitsByLevelWieldReqValue.Count > 0)
			{
				sb.AppendLine("Hits By WieldRequirements: Level, WeidlReqValue: ");
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
