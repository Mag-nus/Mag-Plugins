using System.Collections.Generic;

using Decal.Adapter.Wrappers;

namespace MagTools
{
	public static class Constants
	{
		/// <summary>
		/// Returns a dictionary of skill ids vs names
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, string> GetSkillInfo()
		{
			Dictionary<int, string> skillInfo = new Dictionary<int, string>
			{
				// This list was taken from the Alinco source
				{ 0x1, "Axe" },
				{ 0x2, "Bow" },
				{ 0x3, "Crossbow" },
				{ 0x4, "Dagger" },
				{ 0x5, "Mace" },
				{ 0x6, "Melee Defense" },
				{ 0x7, "Missile Defense" },
				// 0x8
				{ 0x9, "Spear" },
				{ 0xA, "Staff" },
				{ 0xB, "Sword" },
				{ 0xC, "Thrown Weapons" },
				{ 0xD, "Unarmed Combat" },
				{ 0xE, "Arcane Lore" },
				{ 0xF, "Magic Defense" },
				{ 0x10, "Mana Conversion" },
				{ 0x12, "Item Tinkering" },
				{ 0x13, "Assess Person" },
				{ 0x14, "Deception" },
				{ 0x15, "Healing" },
				{ 0x16, "Jump" },
				{ 0x17, "Lockpick" },
				{ 0x18, "Run" },
				{ 0x1B, "Assess Creature" },
				{ 0x1C, "Weapon Tinkering" },
				{ 0x1D, "Armor Tinkering" },
				{ 0x1E, "Magic Item Tinkering" },
				{ 0x1F, "Creature Enchantment" },
				{ 0x20, "Item Enchantment" },
				{ 0x21, "Life Magic" },
				{ 0x22, "War Magic" },
				{ 0x23, "Leadership" },
				{ 0x24, "Loyalty" },
				{ 0x25, "Fletching" },
				{ 0x26, "Alchemy" },
				{ 0x27, "Cooking" },
				{ 0x28, "Salvaging" },
				{ 0x29, "Two Handed Combat" },
				// 0x2A
				{ 0x2B, "Void" },
				{ 0x2C, "Heavy Weapons" },
				{ 0x2D, "Light Weapons" },
				{ 0x2E, "Finesse Weapons" },
				{ 0x2F, "Missile Weapons" },
				{ 0x30, "Shield" },
				{ 0x31, "Dual Wield" },
				{ 0x32, "Recklessness" },
				{ 0x33, "Sneak Attack" },
				{ 0x34, "Dirty Fighting" },
			};

			return skillInfo;
		}

		/// <summary>
		/// Returns a dictionary of mastery ids vs names
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, string> GetMasteryInfo()
		{
			Dictionary<int, string> masteryInfo = new Dictionary<int, string>
			{
				{ 1, "Unarmed Weapon" },
				{ 2, "Sword" },
				{ 3, "Axe" },
				{ 4, "Mace" },
				{ 5, "Spear" },
				{ 6, "Dagger" },
				{ 7, "Staff" },
				{ 8, "Bow" },
				{ 9, "Crossbow" },
				{ 10, "Thrown" },
				{ 11, "Two Handed Combat" },
			};

			return masteryInfo;
		}

		/// <summary>
		/// Returns a dictionary of attribute set ids vs names
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, string> GetAttributeSetInfo()
		{
			Dictionary<int, string> attributeSetInfo = new Dictionary<int, string>
			{
				// This list was taken from Virindi Tank Loot Editor
				// 01
				// 02
				// 03
				// 04
				{ 05, "Noble Relic Set" },
				{ 06, "Ancient Relic Set" },
				{ 07, "Relic Alduressa Set" },
				{ 08, "Shou-jen Set" },
				{ 09, "Empyrean Rings Set" },
				{ 10, "Arm, Mind, Heart Set" },
				{ 11, "Coat of the Perfect Light Set" },
				{ 12, "Leggings of Perfect Light Set" },
				{ 13, "Soldier's Set" },
				{ 14, "Adept's Set" },
				{ 15, "Archer's Set" },
				{ 16, "Defender's Set" },
				{ 17, "Tinker's Set" },
				{ 18, "Crafter's Set" },
				{ 19, "Hearty Set" },
				{ 20, "Dexterous Set" },
				{ 21, "Wise Set" },
				{ 22, "Swift Set" },
				{ 23, "Hardenend Set" },
				{ 24, "Reinforced Set" },
				{ 25, "Interlocking Set" },
				{ 26, "Flame Proof Set" },
				{ 27, "Acid Proof Set" },
				{ 28, "Cold Proof Set" },
				{ 29, "Lightning Proof Set" },
				{ 30, "Dedication Set" },
				{ 31, "Gladiatorial Clothing Set" },
				{ 32, "Protective Clothing Set" },
				// 33
				// 34
				{ 35, "Sigil of Defense" },
				{ 36, "Sigil of Destruction" },
				{ 37, "Sigil of Fury" },
				{ 38, "Sigil of Growth" },
				{ 39, "Sigil of Vigor" },
				{ 40, "Heroic Protector Set" },
				{ 41, "Heroic Destroyer Set" },
				// 42
				// 43
				// 44
				// 45
				// 46
				// 47
				// 48
				{ 49, "Alchemy Set" },
				{ 50, "Arcane Lore Set" },
				{ 51, "Armor Tinkering Set" },
				{ 52, "Assess Person Set" },
				{ 53, "Axe Set" },
				{ 54, "Bow Set" },
				{ 55, "Cooking Set" },
				{ 56, "Creature Enchantment Set" },
				{ 57, "Crossbow Set" },
				{ 58, "Dagger Set" },
				{ 59, "Deception Set" },
				{ 60, "Fletching Set" },
				{ 61, "Healing Set" },
				{ 62, "Item Enchantment Set" },
				{ 63, "Item Tinkering Set" },
				{ 64, "Leadership Set" },
				{ 65, "Life Magic Set" },
				{ 66, "Loyalty Set" },
				{ 67, "Mace Set" },
				{ 68, "Magic Defense Set" },
				{ 69, "Magic Item Tinkering Set" },
				{ 70, "Mana Conversion Set" },
				{ 71, "Melee Defense Set" },
				{ 72, "Missile Defense Set" },
				{ 73, "Salvaging Set" },
				{ 74, "Spear Set" },
				{ 75, "Staff Set" },
				{ 76, "Sword Set" },
				{ 77, "Thrown Weapons Set" },
				{ 78, "Two Handed Combat Set" },
				{ 79, "Unarmed Combat Set" },
				{ 80, "Void Magic Set" },
				{ 81, "War Magic Set" },
				{ 82, "Weapon Tinkering Set" },
				{ 83, "Assess Creature  Set" },
			};

			return attributeSetInfo;
		}

		/// <summary>
		/// Returns a dictionary of material ids vs names
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, string> GetMaterialInfo()
		{
			Dictionary<int, string> materialInfo = new Dictionary<int, string>
			{
				{ 1, "Ceramic" },
				{ 2, "Porcelain" },
				// 3
				{ 4, "Linen" },
				{ 5, "Satin" },
				{ 6, "Silk" },
				{ 7, "Velvet" },
				{ 8, "Wool" },
				// 9

				{ 10, "Agate" },
				{ 11, "Amber" },
				{ 12, "Amethyst" },
				{ 13, "Aquamarine" },
				{ 14, "Azurite" },
				{ 15, "Black Garnet" },
				{ 16, "Black Opal" },
				{ 17, "Bloodstone" },
				{ 18, "Carnelian" },
				{ 19, "Citrine" },

				{ 20, "Diamond" },
				{ 21, "Emerald" },
				{ 22, "Fire Opal" },
				{ 23, "Green Garnet" },
				{ 24, "Green Jade" },
				{ 25, "Hematite" },
				{ 26, "Imperial Topaz" },
				{ 27, "Jet" },
				{ 28, "Lapis Lazuli" },
				{ 29, "Lavender Jade" },

				{ 30, "Malachite" },
				{ 31, "Moonstone" },
				{ 32, "Onyx" },
				{ 33, "Opal" },
				{ 34, "Peridot" },
				{ 35, "Red Garnet" },
				{ 36, "Red Jade" },
				{ 37, "Rose Quartz" },
				{ 38, "Ruby" },
				{ 39, "Sapphire" },

				{ 40, "Smokey Quartz" },
				{ 41, "Sunstone" },
				{ 42, "Tiger Eye" },
				{ 43, "Tourmaline" },
				{ 44, "Turquoise" },
				{ 45, "White Jade" },
				{ 46, "White Quartz" },
				{ 47, "White Sapphire" },
				{ 48, "Yellow Garnet" },
				{ 49, "Yellow Topaz" },

				{ 50, "Zircon" },
				{ 51, "Ivory" },
				{ 52, "Leather" },
				{ 53, "Armoredillo Hide" },
				{ 54, "Gromnie Hide" },
				{ 55, "Reed Shark Hide" },
				// 56
				{ 57, "Brass" },
				{ 58, "Bronze" },
				{ 59, "Copper" },

				{ 60, "Gold" },
				{ 61, "Iron" },
				{ 62, "Pyreal" },
				{ 63, "Silver" },
				{ 64, "Steel" },
				// 65
				{ 66, "Alabaster" },
				{ 67, "Granite" },
				{ 68, "Marble" },
				{ 69, "Obsidian" },

				{ 70, "Sandstone" },
				{ 71, "Serpentine" },
				{ 73, "Ebony" },
				{ 74, "Mahogany" },
				{ 75, "Oak" },
				{ 76, "Pine" },
				{ 77, "Teak" },
			};

			return materialInfo;
		}
		/*
		public struct SpellEffect
		{
			public readonly int SpellId;
			public readonly double Effect;

			public SpellEffect(int spellId, double effect)
			{
				SpellId = spellId;
				Effect = effect;
			}
		}

		public static readonly List<KeyValuePair<LongValueKey, SpellEffect>> LongValueKeySpellEffects = new List<KeyValuePair<LongValueKey, SpellEffect>>()
		{
			{ LongValueKey.MaxDamage, new SpellEffect(1616, 20)}, // Blood Drinker VI
			{ LongValueKey.MaxDamage, new SpellEffect(2096, 22)}, // Infected Caress
			{ LongValueKey.MaxDamage, new SpellEffect(5183, 22)}, // Incantation of Blood Drinker
			{ LongValueKey.MaxDamage, new SpellEffect(4395, 24)}, // Incantation of Blood Drinker, this spell on the item adds 2 more points of damage over a user casted 8

			{ LongValueKey.MaxDamage, new SpellEffect(2598, 2)}, // Minor Blood Thirst
			{ LongValueKey.MaxDamage, new SpellEffect(2586, 4)}, // Major Blood Thirst
			{ LongValueKey.MaxDamage, new SpellEffect(4661, 7)}, // Epic Blood Thirst

			{ LongValueKey.MaxDamage, new SpellEffect(3688, 50)}, // Prodigal Blood Drinker
		};

		public static readonly Dictionary<DoubleValueKey, SpellEffect> DoubleValueKeySpellEffects = new Dictionary<DoubleValueKey, SpellEffect>()
		{
			{ DoubleValueKey.ElementalDamageVersusMonsters, new SpellEffect(3258, .06)}, // Spirit Drinker VI
			{ DoubleValueKey.ElementalDamageVersusMonsters, new SpellEffect(3259, .07)}, // Infected Spirit Caress
			{ DoubleValueKey.ElementalDamageVersusMonsters, new SpellEffect(5182, .07)}, // Incantation of Spirit Drinker
			{ DoubleValueKey.ElementalDamageVersusMonsters, new SpellEffect(4414, .08)}, // Incantation of Spirit Drinker, this spell on the item adds 1 more % of damage over a user casted 8

			{ DoubleValueKey.ElementalDamageVersusMonsters, new SpellEffect(3251, .01)}, // Minor Spirit Thirst
			{ DoubleValueKey.ElementalDamageVersusMonsters, new SpellEffect(3250, .03)}, // Major Spirit Thirst
			{ DoubleValueKey.ElementalDamageVersusMonsters, new SpellEffect(4670, .04)}, // Epic Spirit Thirst

			{ DoubleValueKey.ElementalDamageVersusMonsters, new SpellEffect(3735, .15)}, // Prodigal Spirit Drinker


			{ DoubleValueKey.AttackBonus, new SpellEffect(1592, .15)}, // Heart Seeker VI
			{ DoubleValueKey.AttackBonus, new SpellEffect(2106, .17)}, // Elysa's Sight
			{ DoubleValueKey.AttackBonus, new SpellEffect(4405, .20)}, // Incantation of Heart Seeker

			{ DoubleValueKey.AttackBonus, new SpellEffect(2603, .03)}, // Minor Heart Thirst
			{ DoubleValueKey.AttackBonus, new SpellEffect(2591, .05)}, // Major Heart Thirst
			{ DoubleValueKey.AttackBonus, new SpellEffect(4666, .07)}, // Epic Heart Thirst


			{ DoubleValueKey.MeleeDefenseBonus, new SpellEffect(1605, .15)}, // Defender VI
			{ DoubleValueKey.MeleeDefenseBonus, new SpellEffect(2101, .17)}, // Cragstone's Will
			{ DoubleValueKey.MeleeDefenseBonus, new SpellEffect(4400, .17)}, // Incantation of Defender

			{ DoubleValueKey.MeleeDefenseBonus, new SpellEffect(2600, .03)}, // Minor Defender
			{ DoubleValueKey.MeleeDefenseBonus, new SpellEffect(3985, .04)}, // Mukkir Sense
			{ DoubleValueKey.MeleeDefenseBonus, new SpellEffect(2588, .05)}, // Major Defender
			{ DoubleValueKey.MeleeDefenseBonus, new SpellEffect(4663, .07)}, // Epic Defender

			{ DoubleValueKey.MeleeDefenseBonus, new SpellEffect(3699, .25)}, // Prodigal Defender


			{ DoubleValueKey.ManaCBonus, new SpellEffect(1480, 1.60)}, // Hermetic Link VI
			{ DoubleValueKey.ManaCBonus, new SpellEffect(2117, 1.70)}, // Mystic's Blessing
			{ DoubleValueKey.ManaCBonus, new SpellEffect(4418, 1.80)}, // Incantation of Hermetic Link

			{ DoubleValueKey.ManaCBonus, new SpellEffect(3201, 1.05)}, // Feeble Hermetic Link
			{ DoubleValueKey.ManaCBonus, new SpellEffect(3199, 1.10)}, // Minor Hermetic Link
			{ DoubleValueKey.ManaCBonus, new SpellEffect(3202, 1.15)}, // Moderate Hermetic Link
			{ DoubleValueKey.ManaCBonus, new SpellEffect(3200, 1.20)}, // Major Hermetic Link
		};
*/
		public struct SpellInfo<T>
		{
			public readonly T Key;
			public readonly double Change;

			public SpellInfo(T key, double change)
			{
				Key = key;
				Change = change;
			}
		}

		public static readonly Dictionary<int, SpellInfo<LongValueKey>> LongValueKeySpellEffects = new Dictionary<int, SpellInfo<LongValueKey>>()
		{
			{ 1616, new SpellInfo<LongValueKey>(LongValueKey.MaxDamage, 20)}, // Blood Drinker VI
			{ 2096, new SpellInfo<LongValueKey>(LongValueKey.MaxDamage, 22)}, // Infected Caress
			{ 5183, new SpellInfo<LongValueKey>(LongValueKey.MaxDamage, 22)}, // Incantation of Blood Drinker
			{ 4395, new SpellInfo<LongValueKey>(LongValueKey.MaxDamage, 24)}, // Incantation of Blood Drinker, this spell on the item adds 2 more points of damage over a user casted 8

			{ 2598, new SpellInfo<LongValueKey>(LongValueKey.MaxDamage, 2)}, // Minor Blood Thirst
			{ 2586, new SpellInfo<LongValueKey>(LongValueKey.MaxDamage, 4)}, // Major Blood Thirst
			{ 4661, new SpellInfo<LongValueKey>(LongValueKey.MaxDamage, 7)}, // Epic Blood Thirst

			{ 3688, new SpellInfo<LongValueKey>(LongValueKey.MaxDamage, 300)}, // Prodigal Blood Drinker
		};

		public static readonly Dictionary<int, SpellInfo<DoubleValueKey>> DoubleValueKeySpellEffects = new Dictionary<int, SpellInfo<DoubleValueKey>>()
		{
			{ 3258, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .06)}, // Spirit Drinker VI
			{ 3259, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .07)}, // Infected Spirit Caress
			{ 5182, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .07)}, // Incantation of Spirit Drinker
			{ 4414, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .08)}, // Incantation of Spirit Drinker, this spell on the item adds 1 more % of damage over a user casted 8

			{ 3251, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .01)}, // Minor Spirit Thirst
			{ 3250, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .03)}, // Major Spirit Thirst
			{ 4670, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .04)}, // Epic Spirit Thirst

			{ 3735, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .15)}, // Prodigal Spirit Drinker


			{ 1592, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .15)}, // Heart Seeker VI
			{ 2106, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .17)}, // Elysa's Sight
			{ 4405, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .20)}, // Incantation of Heart Seeker

			{ 2603, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .03)}, // Minor Heart Thirst
			{ 2591, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .05)}, // Major Heart Thirst
			{ 4666, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .07)}, // Epic Heart Thirst


			{ 1605, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .15)}, // Defender VI
			{ 2101, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .17)}, // Cragstone's Will
			{ 4400, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .17)}, // Incantation of Defender

			{ 2600, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .03)}, // Minor Defender
			{ 3985, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .04)}, // Mukkir Sense
			{ 2588, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .05)}, // Major Defender
			{ 4663, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .07)}, // Epic Defender

			{ 3699, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .25)}, // Prodigal Defender


			{ 1480, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.60)}, // Hermetic Link VI
			{ 2117, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.70)}, // Mystic's Blessing
			{ 4418, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.80)}, // Incantation of Hermetic Link

			{ 3201, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.05)}, // Feeble Hermetic Link
			{ 3199, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.10)}, // Minor Hermetic Link
			{ 3202, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.15)}, // Moderate Hermetic Link
			{ 3200, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.20)}, // Major Hermetic Link
		};
	}
}
