using System.Collections.Generic;

namespace Mag.Shared.Constants
{
	public static class Dictionaries
	{
		/// <summary>
		/// Returns a dictionary of skill ids vs names
		/// </summary>
		/// <returns></returns>
		public static readonly Dictionary<int, string> SkillInfo = new Dictionary<int, string>
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
			// 0x35
			{ 0x36, "Summoning" },
		};

		/// <summary>
		/// Returns a dictionary of mastery ids vs names
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, string> MasteryInfo = new Dictionary<int, string>
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

		/// <summary>
		/// Returns a dictionary of attribute set ids vs names
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, string> AttributeSetInfo = new Dictionary<int, string>
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
			{ 47, "Upgraded Ancient Relic Set" },
			// 48
			{ 49, "Weave of Alchemy" },
			{ 50, "Weave of Arcane Lore" },
			{ 51, "Weave of Armor Tinkering" },
			{ 52, "Weave of Assess Person" },
			{ 53, "Weave of Light Weapons" },
			{ 54, "Weave of Missile Weapons" },
			{ 55, "Weave of Cooking" },
			{ 56, "Weave of Creature Enchantment" },
			{ 57, "Weave of Missile Weapons" },
			{ 58, "Weave of Finesse" },
			{ 59, "Weave of Deception" },
			{ 60, "Weave of Fletching" },
			{ 61, "Weave of Healing" },
			{ 62, "Weave of Item Enchantment" },
			{ 63, "Weave of Item Tinkering" },
			{ 64, "Weave of Leadership" },
			{ 65, "Weave of Life Magic" },
			{ 66, "Weave of Loyalty" },
			{ 67, "Weave of Light Weapons" },
			{ 68, "Weave of Magic Defense" },
			{ 69, "Weave of Magic Item Tinkering" },
			{ 70, "Weave of Mana Conversion" },
			{ 71, "Weave of Melee Defense" },
			{ 72, "Weave of Missile Defense" },
			{ 73, "Weave of Salvaging" },
			{ 74, "Weave of Light Weapons" },
			{ 75, "Weave of Light Weapons" },
			{ 76, "Weave of Heavy Weapons" },
			{ 77, "Weave of Missile Weapons" },
			{ 78, "Weave of Two Handed Combat" },
			{ 79, "Weave of Light Weapons" },
			{ 80, "Weave of Void Magic" },
			{ 81, "Weave of War Magic" },
			{ 82, "Weave of Weapon Tinkering" },
			{ 83, "Weave of Assess Creature " },
			{ 84, "Weave of Dirty Fighting" },
			{ 85, "Weave of Dual Wield" },
			{ 86, "Weave of Recklessness" },
			{ 87, "Weave of Shield" },
			{ 88, "Weave of Sneak Attack" },
			// 89
			{ 90, "Weave of Summoning" },
		};

		/// <summary>
		/// Returns a dictionary of material ids vs names
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, string> MaterialInfo = new Dictionary<int, string>
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

		public struct SpellInfo<T>
		{
			public readonly int Key;
			public readonly T Change;
			public readonly T Bonus;

			public SpellInfo(int key, T change, T bonus = default(T))
			{
				Key = key;
				Change = change;
				Bonus = bonus;
			}
		}

		// Taken from Decal.Adapter.Wrappers.LongValueKey
		const int LongValueKey_MaxDamage = 218103842;
		const int LongValueKey_ArmorLevel = 28;

		public static readonly Dictionary<int, SpellInfo<int>> LongValueKeySpellEffects = new Dictionary<int, SpellInfo<int>>()
		{
			// In 2012 they removed these item spells and converted them to auras that are cast on the player, not on the item.
			{ 1616, new SpellInfo<int>(LongValueKey_MaxDamage, 20)}, // Blood Drinker VI
			{ 2096, new SpellInfo<int>(LongValueKey_MaxDamage, 22)}, // Infected Caress
			//{ 5183, new SpellInfo<LongValueKey>(LongValueKey_MaxDamage, 22)}, // Incantation of Blood Drinker Pre Feb-2013
			//{ 4395, new SpellInfo<LongValueKey>(LongValueKey_MaxDamage, 24, 2)}, // Incantation of Blood Drinker, this spell on the item adds 2 more points of damage over a user casted 8 Pre Feb-2013
			{ 5183, new SpellInfo<int>(LongValueKey_MaxDamage, 24)}, // Incantation of Blood Drinker Post Feb-2013
			{ 4395, new SpellInfo<int>(LongValueKey_MaxDamage, 24)}, // Incantation of Blood Drinker Post Feb-2013

			{ 2598, new SpellInfo<int>(LongValueKey_MaxDamage, 2, 2)}, // Minor Blood Thirst
			{ 2586, new SpellInfo<int>(LongValueKey_MaxDamage, 4, 4)}, // Major Blood Thirst
			{ 4661, new SpellInfo<int>(LongValueKey_MaxDamage, 7, 7)}, // Epic Blood Thirst
			{ 6089, new SpellInfo<int>(LongValueKey_MaxDamage, 10, 10)}, // Legendary Blood Thirst

			{ 3688, new SpellInfo<int>(LongValueKey_MaxDamage, 300)}, // Prodigal Blood Drinker


			{ 1486, new SpellInfo<int>(LongValueKey_ArmorLevel, 200)}, // Impenetrability VI
			{ 2108, new SpellInfo<int>(LongValueKey_ArmorLevel, 220)}, // Brogard's Defiance
			{ 4407, new SpellInfo<int>(LongValueKey_ArmorLevel, 240)}, // Incantation of Impenetrability

			{ 2604, new SpellInfo<int>(LongValueKey_ArmorLevel, 20, 20)}, // Minor Impenetrability
			{ 2592, new SpellInfo<int>(LongValueKey_ArmorLevel, 40, 40)}, // Major Impenetrability
			{ 4667, new SpellInfo<int>(LongValueKey_ArmorLevel, 60, 60)}, // Epic Impenetrability
			{ 6095, new SpellInfo<int>(LongValueKey_ArmorLevel, 80, 80)}, // Legendary Impenetrability
		};

		// Taken from Decal.Adapter.Wrappers.DoubleValueKey
		const int DoubleValueKey_ElementalDamageVersusMonsters = 152;
		const int DoubleValueKey_AttackBonus = 167772172;
		const int DoubleValueKey_MeleeDefenseBonus = 29;
		const int DoubleValueKey_ManaCBonus = 144;

		public static readonly Dictionary<int, SpellInfo<double>> DoubleValueKeySpellEffects = new Dictionary<int, SpellInfo<double>>()
		{
			// In 2012 they removed these item spells and converted them to auras that are cast on the player, not on the item.
			{ 3258, new SpellInfo<double>(DoubleValueKey_ElementalDamageVersusMonsters, .06)}, // Spirit Drinker VI
			{ 3259, new SpellInfo<double>(DoubleValueKey_ElementalDamageVersusMonsters, .07)}, // Infected Spirit Caress
			//{ 5182, new SpellInfo<double>(DoubleValueKey_ElementalDamageVersusMonsters, .07)}, // Incantation of Spirit Drinker Pre Feb-2013
			//{ 4414, new SpellInfo<double>(DoubleValueKey_ElementalDamageVersusMonsters, .08, .01)}, // Incantation of Spirit Drinker, this spell on the item adds 1 more % of damage over a user casted 8 Pre Feb-2013
			{ 5182, new SpellInfo<double>(DoubleValueKey_ElementalDamageVersusMonsters, .08)}, // Incantation of Spirit Drinker Post Feb-2013
			{ 4414, new SpellInfo<double>(DoubleValueKey_ElementalDamageVersusMonsters, .08)}, // Incantation of Spirit Drinker, this spell on the item adds 1 more % of damage over a user casted 8 Post Feb-2013

			{ 3251, new SpellInfo<double>(DoubleValueKey_ElementalDamageVersusMonsters, .01, .01)}, // Minor Spirit Thirst
			{ 3250, new SpellInfo<double>(DoubleValueKey_ElementalDamageVersusMonsters, .03, .03)}, // Major Spirit Thirst
			{ 4670, new SpellInfo<double>(DoubleValueKey_ElementalDamageVersusMonsters, .05, .05)}, // Epic Spirit Thirst
			{ 6098, new SpellInfo<double>(DoubleValueKey_ElementalDamageVersusMonsters, .07, .07)}, // Legendary Spirit Thirst

			{ 3735, new SpellInfo<double>(DoubleValueKey_ElementalDamageVersusMonsters, .15)}, // Prodigal Spirit Drinker


			// In 2012 they removed these item spells and converted them to auras that are cast on the player, not on the item.
			{ 1592, new SpellInfo<double>(DoubleValueKey_AttackBonus, .15)}, // Heart Seeker VI
			{ 2106, new SpellInfo<double>(DoubleValueKey_AttackBonus, .17)}, // Elysa's Sight
			{ 4405, new SpellInfo<double>(DoubleValueKey_AttackBonus, .20)}, // Incantation of Heart Seeker

			{ 2603, new SpellInfo<double>(DoubleValueKey_AttackBonus, .03, .03)}, // Minor Heart Thirst
			{ 2591, new SpellInfo<double>(DoubleValueKey_AttackBonus, .05, .05)}, // Major Heart Thirst
			{ 4666, new SpellInfo<double>(DoubleValueKey_AttackBonus, .07, .07)}, // Epic Heart Thirst
			{ 6094, new SpellInfo<double>(DoubleValueKey_AttackBonus, .09, .09)}, // Legendary Heart Thirst


			// In 2012 they removed these item spells and converted them to auras that are cast on the player, not on the item.
			{ 1605, new SpellInfo<double>(DoubleValueKey_MeleeDefenseBonus, .15)}, // Defender VI
			{ 2101, new SpellInfo<double>(DoubleValueKey_MeleeDefenseBonus, .17)}, // Cragstone's Will
			//{ 4400, new SpellInfo<double>(DoubleValueKey_MeleeDefenseBonus, .17)}, // Incantation of Defender Pre Feb-2013
			{ 4400, new SpellInfo<double>(DoubleValueKey_MeleeDefenseBonus, .20)}, // Incantation of Defender Post Feb-2013

			{ 2600, new SpellInfo<double>(DoubleValueKey_MeleeDefenseBonus, .03, .03)}, // Minor Defender
			{ 3985, new SpellInfo<double>(DoubleValueKey_MeleeDefenseBonus, .04, .04)}, // Mukkir Sense
			{ 2588, new SpellInfo<double>(DoubleValueKey_MeleeDefenseBonus, .05, .05)}, // Major Defender
			{ 4663, new SpellInfo<double>(DoubleValueKey_MeleeDefenseBonus, .07, .07)}, // Epic Defender
			{ 6091, new SpellInfo<double>(DoubleValueKey_MeleeDefenseBonus, .09, .09)}, // Legendary Defender

			{ 3699, new SpellInfo<double>(DoubleValueKey_MeleeDefenseBonus, .25)}, // Prodigal Defender


			// In 2012 they removed these item spells and converted them to auras that are cast on the player, not on the item.
			{ 1480, new SpellInfo<double>(DoubleValueKey_ManaCBonus, 1.60)}, // Hermetic Link VI
			{ 2117, new SpellInfo<double>(DoubleValueKey_ManaCBonus, 1.70)}, // Mystic's Blessing
			{ 4418, new SpellInfo<double>(DoubleValueKey_ManaCBonus, 1.80)}, // Incantation of Hermetic Link

			{ 3201, new SpellInfo<double>(DoubleValueKey_ManaCBonus, 1.05, 1.05)}, // Feeble Hermetic Link
			{ 3199, new SpellInfo<double>(DoubleValueKey_ManaCBonus, 1.10, 1.10)}, // Minor Hermetic Link
			{ 3202, new SpellInfo<double>(DoubleValueKey_ManaCBonus, 1.15, 1.15)}, // Moderate Hermetic Link
			{ 3200, new SpellInfo<double>(DoubleValueKey_ManaCBonus, 1.20, 1.20)}, // Major Hermetic Link
			{ 6086, new SpellInfo<double>(DoubleValueKey_ManaCBonus, 1.25, 1.25)}, // Epic Hermetic Link
			{ 6087, new SpellInfo<double>(DoubleValueKey_ManaCBonus, 1.30, 1.30)}, // Legendary Hermetic Link
		};
	}
}
