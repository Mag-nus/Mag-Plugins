using System;
using System.Collections.Generic;

using Decal.Adapter;
using Decal.Adapter.Wrappers;
using Decal.Filters;

namespace MagTools.ItemInfo
{
	/// <summary>
	/// Instantiate this object with the item you want info for.
	/// ToString() this object for the info.
	/// </summary>
	class ItemInfo
	{
		private readonly WorldObject wo;

		public ItemInfo(WorldObject worldObject)
		{
			wo = worldObject;
		}

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append(wo.Name);

			int set = wo.Values((LongValueKey)265, 0);
			if (set != 0)
			{
				sb.Append(", ");
				sb.Append(Util.GetAttributeSetNameById(set));
			}

			if (wo.Values(LongValueKey.ArmorLevel) > 0)
				sb.Append(", AL " + wo.Values(LongValueKey.ArmorLevel));

			if (wo.Values(LongValueKey.Imbued) > 0)
			{
				sb.Append(",");
				if ((wo.Values(LongValueKey.Imbued) & 1) == 1) sb.Append(" CS");
				if ((wo.Values(LongValueKey.Imbued) & 2) == 2) sb.Append(" CB");
				if ((wo.Values(LongValueKey.Imbued) & 4) == 4) sb.Append(" AR");
				if ((wo.Values(LongValueKey.Imbued) & 8) == 8) sb.Append(" SlashRend");
				if ((wo.Values(LongValueKey.Imbued) & 16) == 16) sb.Append(" PierceRend");
				if ((wo.Values(LongValueKey.Imbued) & 32) == 32) sb.Append(" BludgeRend");
				if ((wo.Values(LongValueKey.Imbued) & 64) == 64) sb.Append(" AcidRend");
				if ((wo.Values(LongValueKey.Imbued) & 128) == 128) sb.Append(" FrostRend");
				if ((wo.Values(LongValueKey.Imbued) & 256) == 256) sb.Append(" LightRend");
				if ((wo.Values(LongValueKey.Imbued) & 512) == 512) sb.Append(" FireRend");
				if ((wo.Values(LongValueKey.Imbued) & 1024) == 1024) sb.Append(" MeleeImbue");
				if ((wo.Values(LongValueKey.Imbued) & 8192) == 8192) sb.Append(" Hematited");
				if ((wo.Values(LongValueKey.Imbued) & 536870912) == 536870912) sb.Append(" MagicAbsorb");
			}

			if (wo.Values(LongValueKey.NumberTimesTinkered) > 0)
				sb.Append(", Tinks " + wo.Values(LongValueKey.NumberTimesTinkered));

			if (wo.Values(LongValueKey.MaxDamage) != 0 && wo.Values(DoubleValueKey.Variance) != 0)
				sb.Append(", " + (wo.Values(LongValueKey.MaxDamage) - (wo.Values(LongValueKey.MaxDamage) * wo.Values(DoubleValueKey.Variance))).ToString("N2") + "-" + wo.Values(LongValueKey.MaxDamage));
			else if (wo.Values(LongValueKey.MaxDamage) != 0 && wo.Values(DoubleValueKey.Variance) == 0)
				sb.Append(", " + wo.Values(LongValueKey.MaxDamage));

			if (wo.Values(LongValueKey.ElementalDmgBonus, 0) != 0)
				sb.Append(", +" + wo.Values(LongValueKey.ElementalDmgBonus));

			if (wo.Values(DoubleValueKey.DamageBonus, 1) != 1)
				sb.Append(", +" + Math.Round(((wo.Values(DoubleValueKey.DamageBonus) - 1) * 100)) + "%");

			if (wo.Values(DoubleValueKey.ElementalDamageVersusMonsters, 1) != 1)
				sb.Append(", +" + Math.Round(((wo.Values(DoubleValueKey.ElementalDamageVersusMonsters) - 1) * 100)) + "%vs. Monsters");

			if (wo.Values(DoubleValueKey.AttackBonus, 1) != 1)
				sb.Append(", +" + Math.Round(((wo.Values(DoubleValueKey.AttackBonus) - 1) * 100)) + "%a");

			if (wo.Values(DoubleValueKey.MeleeDefenseBonus, 1) != 1)
				sb.Append(", " + Math.Round(((wo.Values(DoubleValueKey.MeleeDefenseBonus) - 1) * 100)) + "%md");

			if (wo.Values(DoubleValueKey.MagicDBonus, 1) != 1)
				sb.Append(", " + Math.Round(((wo.Values(DoubleValueKey.MagicDBonus) - 1) * 100), 1) + "%mgc.d");

			if (wo.Values(DoubleValueKey.MissileDBonus, 1) != 1)
				sb.Append(", " + Math.Round(((wo.Values(DoubleValueKey.MissileDBonus) - 1) * 100), 1) + "%msl.d");

			if (wo.Values(DoubleValueKey.ManaCBonus) != 0)
				sb.Append(", " + Math.Round((wo.Values(DoubleValueKey.ManaCBonus) * 100)) + "%mc");

			if (wo.Values(LongValueKey.SpellCount) > 0)
			{
				FileService service = CoreManager.Current.Filter<FileService>();

				List<int> itemActiveSpells = new List<int>();

				for (int i = 0 ; i < wo.Values(LongValueKey.SpellCount) ; i++)
					itemActiveSpells.Add(wo.Spell(i));

				itemActiveSpells.Sort();
				itemActiveSpells.Reverse();

				foreach (int spell in itemActiveSpells)
				{
					Spell spellById = service.SpellTable.GetById(spell);

					// If the item is not loot generated, show all spells
					if (!wo.LongKeys.Contains((int)LongValueKey.Material))
						goto ShowSpell;

					// Always show Minor/Major/Epic Impen
					if (spellById.Name.Contains("Minor Impenetrability") || spellById.Name.Contains("Major Impenetrability") || spellById.Name.Contains("Epic Impenetrability"))
						goto ShowSpell;

					// Always show trinket spells
					if (spellById.Name.Contains("Augmented"))
						goto ShowSpell;

					if (wo.Values(LongValueKey.Unenchantable, 0) != 0)
					{
						// Show banes and impen on unenchantable equipment
						if (spellById.Name.Contains(" Bane") || spellById.Name.Contains("Impen") || spellById.Name.StartsWith("Brogard"))
							goto ShowSpell;
					}
					else
					{
						// Hide banes and impen on enchantable equipment
						if (spellById.Name.Contains(" Bane") || spellById.Name.Contains("Impen") || spellById.Name.StartsWith("Brogard"))
							continue;
					}

					//Debug.WriteToChat(spellById.Name + ", Difficulty: " + spellById.Difficulty + ", Family: " + spellById.Family + ", Generation: " + spellById.Generation + ", Type: " + spellById.Type + ", " + spellById.Unknown1 + " " + spellById.Unknown2 + " " + spellById.Unknown3 + " " + spellById.Unknown4 + " " + spellById.Unknown5 + " " + spellById.Unknown6 + " " + spellById.Unknown7 + " " + spellById.Unknown8 + " " + spellById.Unknown9 + " " + spellById.Unknown10);
					// <{Mag-Tools}>: Major Coordination,				Difficulty: 15,		Family: 267,	Generation: 1, Type: 1,		0	1		1 2572 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Epic Magic Resistance,			Difficulty: 20,		Family: 299,	Generation: 1, Type: 1,		0	1		1 4704 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Epic Life Magic Aptitude,			Difficulty: 20,		Family: 357,	Generation: 1, Type: 1,		0	1		1 4700 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Epic Endurance,					Difficulty: 20,		Family: 263,	Generation: 1, Type: 1,		0	1		1 4226 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Essence Glutton,					Difficulty: 30,		Family: 279,	Generation: 1, Type: 1,		0	0		1 2666 -2.07525870829232E+20 0 0 0 0 0

					// <{Mag-Tools}>: Might of the Lugians,				Difficulty: 300,	Family: 1,		Generation: 1, Type: 1,		0	0		1 2087 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Executor's Blessing,				Difficulty: 300,	Family: 115,	Generation: 1, Type: 1,		0	0		1 2053 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Regeneration Other Incantation,	Difficulty: 400,	Family: 93,		Generation: 1, Type: 1,		5	0.25	1 3982 -2.07525870829232E+20 0 0 0 0 0

					// Focusing stone
					// <{Mag-Tools}>: Brilliance,						Difficulty: 250,	Family: 15,		Generation: 1, Type: 1,		5	0.25	1 2348 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Concentration,					Difficulty: 100,	Family: 13,		Generation: 1, Type: 1,		0	0		1 2347 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Malediction,						Difficulty: 50,		Family: 284,	Generation: 1, Type: 1,		0	0		1 2346 -2.07525870829232E+20 0 0 0 0 0

					// Weapon buffs
					// <{Mag-Tools}>: Elysa's Sight,					Difficulty: 300,	Family: 152,	Generation: 1, Type: 1,		25	0		1 2106 -2.07525870829232E+20 0 0 0 0 0 (Attack Skill)
					// <{Mag-Tools}>: Infected Caress,					Difficulty: 300,	Family: 154,	Generation: 1, Type: 1,		25	0		1 2096 -2.07525870829232E+20 0 0 0 0 0 (Damage)
					// <{Mag-Tools}>: Infected Spirit Caress,			Difficulty: 300,	Family: 154,	Generation: 1, Type: 1,		25	0		1 3259 -2.07525870829232E+20 0 0 0 0 0 (Damage)
					// <{Mag-Tools}>: Cragstone's Will,					Difficulty: 300,	Family: 156,	Generation: 1, Type: 1,		25	0		1 2101 -2.07525870829232E+20 0 0 0 0 0 (Defense)
					// <{Mag-Tools}>: Atlan's Alacrity,					Difficulty: 300,	Family: 158,	Generation: 1, Type: 1,		25	0		1 2116 -2.07525870829232E+20 0 0 0 0 0 (Speed)
					// <{Mag-Tools}>: Mystic's Blessing,				Difficulty: 300,	Family: 195,	Generation: 1, Type: 1,		25	0		1 2117 -2.07525870829232E+20 0 0 0 0 0 (Mana C)
					// <{Mag-Tools}>: Vision of the Hunter,				Difficulty: 500,	Family: 325,	Generation: 1, Type: 1,		25	0		1 2968 -2.07525870829232E+20 0 0 0 0 0 (Damage Mod)

					if ((spellById.Family >= 152 && spellById.Family <= 158) || spellById.Family == 195 || spellById.Family == 325)
					{
						// This is a weapon buff

						// Lvl 6
						if (spellById.Difficulty == 250)
							continue;

						// Lvl 7
						if (spellById.Difficulty == 300)
							goto ShowSpell;

						// Lvl 8+
						if (spellById.Difficulty >= 400)
							goto ShowSpell;

						continue;
					}

					// This is not a weapon buff.

					// Filter all 1-5 spells
					if (spellById.Name.EndsWith(" I") || spellById.Name.EndsWith(" II") || spellById.Name.EndsWith(" III") || spellById.Name.EndsWith(" IV") || spellById.Name.EndsWith(" V"))
						continue;

					// Filter 6's
					if (spellById.Name.EndsWith(" VI"))
						continue;

					// Filter 7's
					if (spellById.Difficulty == 300)
						continue;

					// Filter 8's
					if (spellById.Name.Contains("Incantation"))
						continue;

				ShowSpell:

					sb.Append(", " + spellById.Name);
				}
			}

			// Wield Lvl 180
			if (wo.Values(LongValueKey.WieldReqValue) > 0)
			{
				// I don't quite understand this.
				if (wo.Values(LongValueKey.WieldReqType) == 7 && wo.Values(LongValueKey.WieldReqAttribute) == 1)
					sb.Append(", Wield Lvl " + wo.Values(LongValueKey.WieldReqValue));
				else
					sb.Append(", " + Util.GetSkillNameById(wo.Values(LongValueKey.WieldReqAttribute)) + " " + wo.Values(LongValueKey.WieldReqValue) + " to Wield");
			}

			// Melee Defense 300 to Activate
			// If the activation is lower than the wield requirement, don't show it.
			if (wo.Values(LongValueKey.SkillLevelReq) > 0 && (wo.Values(LongValueKey.WieldReqAttribute) != wo.Values(LongValueKey.ActivationReqSkillId) || wo.Values(LongValueKey.WieldReqValue) < wo.Values(LongValueKey.SkillLevelReq)))
			{
				sb.Append(", " + Util.GetSkillNameById(wo.Values(LongValueKey.ActivationReqSkillId)) + " " + wo.Values(LongValueKey.SkillLevelReq) + " to Activate");
			}

			if (wo.Values(LongValueKey.LoreRequirement) > 0)
				sb.Append(", Diff " + wo.Values(LongValueKey.LoreRequirement));

			if (wo.Values(LongValueKey.Workmanship) > 0 && wo.Values(LongValueKey.NumberTimesTinkered) != 10) // Don't show the work if its already 10 tinked.
				sb.Append(", Craft " + wo.Values(LongValueKey.Workmanship));

			if (wo.ObjectClass == ObjectClass.Armor && wo.Values(LongValueKey.Unenchantable, 0) != 0)
			{
				sb.Append(", [" +
					wo.Values(DoubleValueKey.SlashProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.PierceProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.BludgeonProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.ColdProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.FireProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.AcidProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.LightningProt).ToString("N1") + "]");
			}

			return sb.ToString();
		}
	}
}
