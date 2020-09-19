using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mag.Shared.Constants;
using Mag_LootParser.ItemGroups.Helpers;

namespace Mag_LootParser.ItemGroups
{
	class WieldableStats : ItemGroupStats
	{
		public readonly Dictionary<int, int> HitsByRawSkillWieldReqValue = new Dictionary<int, int>();
		public readonly Dictionary<int, int> HitsByLevelWieldReqValue = new Dictionary<int, int>();

		public readonly Dictionary<int, int> HitsByValidLocations = new Dictionary<int, int>();

		public readonly Dictionary<int, int> HitsByEquipmentSet = new Dictionary<int, int>();

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

				if (item.LongValues.ContainsKey(IntValueKey.EquipableSlots_Decal))
				{
					var value = item.LongValues[IntValueKey.EquipableSlots_Decal];

					if (!HitsByValidLocations.ContainsKey(value))
						HitsByValidLocations[value] = 1;
					else
						HitsByValidLocations[value] ++;
				}

				if (item.LongValues.ContainsKey(IntValueKey.EquipmentSetId))
				{
					var value = item.LongValues[IntValueKey.EquipmentSetId];

					if (!HitsByEquipmentSet.ContainsKey(value))
						HitsByEquipmentSet[value] = 1;
					else
						HitsByEquipmentSet[value]++;
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
				sb.AppendLine("Hits By RawSkillWieldReq: ");
				var totalHits = HitsByRawSkillWieldReqValue.Values.Sum();
				var sortedKeys = HitsByRawSkillWieldReqValue.Keys.ToList();
				sortedKeys.Sort();
				foreach (var key in sortedKeys)
					sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByRawSkillWieldReqValue[key].ToString("N0").PadLeft(7) + " " + (HitsByRawSkillWieldReqValue[key] / (float)totalHits * 100).ToString("N1").PadLeft(6) + "%]");
			}

			if (HitsByLevelWieldReqValue.Count > 0)
			{
				sb.AppendLine("Hits By LevelWieldReq: ");
				var totalHits = HitsByLevelWieldReqValue.Values.Sum();
				var sortedKeys = HitsByLevelWieldReqValue.Keys.ToList();
				sortedKeys.Sort();
				foreach (var key in sortedKeys)
					sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByLevelWieldReqValue[key].ToString("N0").PadLeft(7) + " " + (HitsByLevelWieldReqValue[key] / (float)totalHits * 100).ToString("N1").PadLeft(6) + "%]");
			}

			if (HitsByValidLocations.Count > 0)
			{
				sb.AppendLine("Hits By ValidLocations: ");
				var totalHits = HitsByValidLocations.Values.Sum();
				var sortedKeys = HitsByValidLocations.Keys.ToList();
				sortedKeys.Sort();
				foreach (var key in sortedKeys)
					sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByValidLocations[key].ToString("N0").PadLeft(7) + " " + (HitsByValidLocations[key] / (float)totalHits * 100).ToString("N1").PadLeft(6) + "%] " + (EquipMask)key);
			}

			if (HitsByEquipmentSet.Count > 0)
			{
				sb.AppendLine("Hits By EquipmentSet: ");
				var totalHits = HitsByEquipmentSet.Values.Sum();
				var sortedKeys = HitsByEquipmentSet.Keys.ToList();
				sortedKeys.Sort();
				foreach (var key in sortedKeys)
				{
					if (Dictionaries.AttributeSetInfo.ContainsKey(key))
						sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByEquipmentSet[key].ToString("N0").PadLeft(7) + " " + (HitsByEquipmentSet[key] / (float) totalHits * 100).ToString("N1").PadLeft(6) + "%] " + Dictionaries.AttributeSetInfo[key]);
					else
						sb.AppendLine(key.ToString().PadLeft(9) + " [" + HitsByEquipmentSet[key].ToString("N0").PadLeft(7) + " " + (HitsByEquipmentSet[key] / (float)totalHits * 100).ToString("N1").PadLeft(6) + "%]");
				}

				sb.AppendLine("Percent chance of any item having a set: " + ((totalHits / (float)Items.Count) * 100).ToString("N1") + "%");
			}

			return sb.ToString();
		}


		protected enum WeaponType
		{
			Undef,
			Axe,
			Dagger,
			DaggerMS,
			Mace,
			MaceJitte,
			Spear,
			Staff,
			Sword,
			SwordMS,
			Unarmed,
			TwoHandedCleave,
			TwoHandedSpear,

			Bow,
			Crossbow,
			Atlatl,
			Slingshot,

			Caster,
		}

		protected static WeaponType GetWeaponType(IdentResponse item)
		{
			var itemName = item.StringValues[StringValueKey.Name];

			// axes
			if (TableContains(HeavyWeaponNames.HeavyAxes, itemName))
				return WeaponType.Axe;
			if (TableContains(LightWeaponNames.LightAxes, itemName))
				return WeaponType.Axe;
			if (TableContains(FinesseWeaponNames.FinesseAxes, itemName))
				return WeaponType.Axe;
			if (TableContains(TwoHandedWeaponNames.TwoHandedAxes, itemName))
				return WeaponType.Axe;

			// daggers
			if (TableContains(HeavyWeaponNames.HeavyDaggers_SingleStrike, itemName))
				return WeaponType.Dagger;
			if (TableContains(HeavyWeaponNames.HeavyDaggers_MultiStrike, itemName))
				return WeaponType.DaggerMS;
			if (TableContains(LightWeaponNames.LightDaggers_SingleStrike, itemName))
				return WeaponType.Dagger;
			if (TableContains(LightWeaponNames.LightDaggers_MultiStrike, itemName))
				return WeaponType.DaggerMS;
			if (TableContains(FinesseWeaponNames.FinesseDaggers_SingleStrike, itemName))
				return WeaponType.Dagger;
			if (TableContains(FinesseWeaponNames.FinesseDaggers_MultiStrike, itemName))
				return WeaponType.DaggerMS;

			// maces
			if (TableContains(HeavyWeaponNames.HeavyMaces, itemName))
				return WeaponType.Mace;
			if (TableContains(LightWeaponNames.LightMaces, itemName))
				return WeaponType.Mace;
			if (TableContains(FinesseWeaponNames.FinesseMaces_NonJittes, itemName))
				return WeaponType.Mace;
			if (TableContains(FinesseWeaponNames.FinesseMaces_Jittes, itemName))
				return WeaponType.MaceJitte;
			if (TableContains(TwoHandedWeaponNames.TwoHandedMaces, itemName))
				return WeaponType.Mace;

			// spears
			if (TableContains(HeavyWeaponNames.HeavySpears, itemName))
				return WeaponType.Spear;
			if (TableContains(LightWeaponNames.LightSpears, itemName))
				return WeaponType.Spear;
			if (TableContains(FinesseWeaponNames.FinesseSpears, itemName))
				return WeaponType.Spear;
			if (TableContains(TwoHandedWeaponNames.TwoHandedSpears, itemName))
				return WeaponType.Spear;

			// staffs
			if (TableContains(HeavyWeaponNames.HeavyStaffs, itemName))
				return WeaponType.Staff;
			if (TableContains(LightWeaponNames.LightStaffs, itemName))
				return WeaponType.Staff;
			if (TableContains(FinesseWeaponNames.FinesseStaffs, itemName))
				return WeaponType.Staff;

			// swords
			if (TableContains(HeavyWeaponNames.HeavySwords_SingleStrike, itemName))
				return WeaponType.Sword;
			if (TableContains(HeavyWeaponNames.HeavySwords_MultiStrike, itemName))
				return WeaponType.SwordMS;
			if (TableContains(LightWeaponNames.LightSwords_SingleStrike, itemName))
				return WeaponType.Sword;
			if (TableContains(LightWeaponNames.LightSwords_MultiStrike, itemName))
				return WeaponType.SwordMS;
			if (TableContains(FinesseWeaponNames.FinesseSwords_SingleStrike, itemName))
				return WeaponType.Sword;
			if (TableContains(FinesseWeaponNames.FinesseSwords_MultiStrike, itemName))
				return WeaponType.SwordMS;
			if (TableContains(TwoHandedWeaponNames.TwoHandedSwords, itemName))
				return WeaponType.Sword;

			// unarmed
			if (TableContains(HeavyWeaponNames.HeavyUnarmed, itemName))
				return WeaponType.Unarmed;
			if (TableContains(LightWeaponNames.LightUnarmed, itemName))
				return WeaponType.Unarmed;
			if (TableContains(FinesseWeaponNames.FinesseUnarmed, itemName))
				return WeaponType.Unarmed;

			// missile
			if (MissileNames.BowNames.Contains(itemName))
				return WeaponType.Bow;
			if (MissileNames.CrossbowNames.Contains(itemName))
				return WeaponType.Crossbow;
			if (MissileNames.AtlatlNames.Contains(itemName))
				return WeaponType.Atlatl;

			// casters
			if (CasterNames.CasterItems.Contains(itemName))
				return WeaponType.Caster;

			throw new Exception($"Couldn't find WeaponType for {itemName}");
			//Console.WriteLine($"Couldn't find WeaponType for {itemName}");

			return WeaponType.Undef;
		}

		protected static Skill GetSkill(IdentResponse item)
		{
			if (item.LongValues.ContainsKey(IntValueKey.WieldSkillType))
				return (Skill)item.LongValues[IntValueKey.WieldSkillType];

			// item is no wield req

			var itemName = item.StringValues[StringValueKey.Name];

			// axes
			if (TableContains(HeavyWeaponNames.HeavyAxes, itemName))
				return Skill.HeavyWeapons;
			if (TableContains(LightWeaponNames.LightAxes, itemName))
				return Skill.LightWeapons;
			if (TableContains(FinesseWeaponNames.FinesseAxes, itemName))
				return Skill.FinesseWeapons;
			if (TableContains(TwoHandedWeaponNames.TwoHandedAxes, itemName))
				return Skill.TwoHandedCombat;

			// daggers
			if (TableContains(HeavyWeaponNames.HeavyDaggers, itemName))
				return Skill.HeavyWeapons;
			if (TableContains(LightWeaponNames.LightDaggers, itemName))
				return Skill.LightWeapons;
			if (TableContains(FinesseWeaponNames.FinesseDaggers, itemName))
				return Skill.FinesseWeapons;

			// maces
			if (TableContains(HeavyWeaponNames.HeavyMaces, itemName))
				return Skill.HeavyWeapons;
			if (TableContains(LightWeaponNames.LightMaces, itemName))
				return Skill.LightWeapons;
			if (TableContains(FinesseWeaponNames.FinesseMaces, itemName))
				return Skill.FinesseWeapons;
			if (TableContains(TwoHandedWeaponNames.TwoHandedMaces, itemName))
				return Skill.TwoHandedCombat;

			// spears
			if (TableContains(HeavyWeaponNames.HeavySpears, itemName))
				return Skill.HeavyWeapons;
			if (TableContains(LightWeaponNames.LightSpears, itemName))
				return Skill.LightWeapons;
			if (TableContains(FinesseWeaponNames.FinesseSpears, itemName))
				return Skill.FinesseWeapons;
			if (TableContains(TwoHandedWeaponNames.TwoHandedSpears, itemName))
				return Skill.TwoHandedCombat;

			// staffs
			if (TableContains(HeavyWeaponNames.HeavyStaffs, itemName))
				return Skill.HeavyWeapons;
			if (TableContains(LightWeaponNames.LightStaffs, itemName))
				return Skill.LightWeapons;
			if (TableContains(FinesseWeaponNames.FinesseStaffs, itemName))
				return Skill.FinesseWeapons;

			// swords
			if (TableContains(HeavyWeaponNames.HeavySwords, itemName))
				return Skill.HeavyWeapons;
			if (TableContains(LightWeaponNames.LightSwords, itemName))
				return Skill.LightWeapons;
			if (TableContains(FinesseWeaponNames.FinesseSwords, itemName))
				return Skill.FinesseWeapons;
			if (TableContains(TwoHandedWeaponNames.TwoHandedSwords, itemName))
				return Skill.TwoHandedCombat;

			// unarmed
			if (TableContains(HeavyWeaponNames.HeavyUnarmed, itemName))
				return Skill.HeavyWeapons;
			if (TableContains(LightWeaponNames.LightUnarmed, itemName))
				return Skill.LightWeapons;
			if (TableContains(FinesseWeaponNames.FinesseUnarmed, itemName))
				return Skill.FinesseWeapons;

			// missile
			if (MissileNames.BowNames.Contains(itemName))
				return Skill.MissileWeapons;
			if (MissileNames.CrossbowNames.Contains(itemName))
				return Skill.MissileWeapons;
			if (MissileNames.AtlatlNames.Contains(itemName))
				return Skill.MissileWeapons;

			// casters
			if (CasterNames.CasterItems.Contains(itemName))
				return Skill.WarMagic;

			throw new Exception($"Couldn't find Skill for {itemName}");
			//Console.WriteLine($"Couldn't find Skill for {itemName}");

			return Skill.None;
		}

		private static bool TableContains(List<HashSet<string>> table, string itemName)
		{
			foreach (var subtable in table)
			{
				if (subtable.Contains(itemName))
					return true;
			}

			return false;
		}
	}
}
