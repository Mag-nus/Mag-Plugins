using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mag.Shared.Constants;

namespace Mag_LootParser.ItemGroups
{
	class MeleeWeaponStats : WieldableStats
	{
		public MeleeWeaponStats(bool limitStatsToOnlyItemsWithWorkmanship = false) : base(limitStatsToOnlyItemsWithWorkmanship)
		{
		}

		private class Stat
		{
			public readonly Dictionary<int /* damage */, int /* hits */> DamageHits = new Dictionary<int, int>();

			public void Add(IdentResponse item)
			{
				var damage = item.LongValues[IntValueKey.MaxDamage_Decal];

				if (!DamageHits.ContainsKey(damage))
					DamageHits[damage] = 1;
				else
					DamageHits[damage]++;
			}
		}

		private readonly Dictionary<Skill, Dictionary<WeaponType, Dictionary<int /* WieldDifficulty */, Stat>>> allStatsBySkill = new Dictionary<Skill, Dictionary<WeaponType, Dictionary<int /* WieldDifficulty */, Stat>>>();

		public override void ProcessItem(IdentResponse item)
		{
			base.ProcessItem(item);

			if (!LimitStatsToOnlyItemsWithWorkmanship || item.LongValues.ContainsKey(IntValueKey.ItemWorkmanship))
			{
				var skill = GetSkill(item);

				if (!allStatsBySkill.TryGetValue(skill, out var statsBySkill))
				{
					statsBySkill = new Dictionary<WeaponType, Dictionary<int, Stat>>();
					allStatsBySkill[skill] = statsBySkill;
				}

				var weaponType = GetWeaponType(item);

				if (!statsBySkill.TryGetValue(weaponType, out var skillStatsByWeaponType))
				{
					skillStatsByWeaponType = new Dictionary<int, Stat>();
					statsBySkill[weaponType] = skillStatsByWeaponType;
				}

				if (item.LongValues.ContainsKey(IntValueKey.WieldRequirements) && item.LongValues.ContainsKey(IntValueKey.WieldDifficulty))
				{
					if (item.LongValues[IntValueKey.WieldRequirements] == (int)WieldRequirement.RawSkill)
					{
						var wieldDifficulty = item.LongValues[IntValueKey.WieldDifficulty];

						if (!skillStatsByWeaponType.TryGetValue(wieldDifficulty, out var skillStatsByWeaponTypeAndWieldDifficulty))
						{
							skillStatsByWeaponTypeAndWieldDifficulty = new Stat();
							skillStatsByWeaponType[wieldDifficulty] = skillStatsByWeaponTypeAndWieldDifficulty;
						}

						skillStatsByWeaponTypeAndWieldDifficulty.Add(item);
					}
				}
			}
		}


		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append(base.ToString());

			sb.AppendLine();

			var sortedKeys = allStatsBySkill.Keys.ToList();
			sortedKeys.Sort();
			foreach (var key in sortedKeys)
			{
				sb.AppendLine($"allStatsBySkill {key}");
				var statsBySkill = allStatsBySkill[key];

				var sortedKeys2 = statsBySkill.Keys.ToList();
				sortedKeys2.Sort();
				foreach (var key2 in sortedKeys2)
				{
					sb.AppendLine($"WeaponType {key2},");
					var statsByWeaponType = statsBySkill[key2];

					var sortedKeys3 = statsByWeaponType.Keys.ToList();
					sortedKeys3.Sort();
					foreach (var key3 in sortedKeys3)
					{
						sb.AppendLine($"WieldDifficulty {key3},");
						var statsByWieldDifficulty = statsByWeaponType[key3];

						var sortedKeys4 = statsByWieldDifficulty.DamageHits.Keys.ToList();
						sortedKeys4.Sort();
						foreach (var key4 in sortedKeys4)
							sb.AppendLine($"Damage {key4}, hits {statsByWieldDifficulty.DamageHits[key4]},");

					}
				}
			}

			return sb.ToString();
		}
	}
}
