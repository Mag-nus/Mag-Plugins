using System;

namespace Mag_LootLogger
{
	static class Util
	{
		/// <summary>
		/// This will return a skills name by its id.
		/// For example, 1 returns "Axe".
		/// If the skill is unknown the following is returned: "Unknown skill id: " + id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string GetSkillNameById(int id)
		{
			// This list was taken from the Alinco source
			if (id == 0x1) return "Axe";
			if (id == 0x2) return "Bow";
			if (id == 0x3) return "Crossbow";
			if (id == 0x4) return "Dagger";
			if (id == 0x5) return "Mace";
			if (id == 0x6) return "Melee Defense";
			if (id == 0x7) return "Missile Defense";
			// 0x8
			if (id == 0x9) return "Spear";
			if (id == 0xA) return "Staff";
			if (id == 0xB) return "Sword";
			if (id == 0xC) return "Thrown Weapons";
			if (id == 0xD) return "Unarmed Combat";
			if (id == 0xE) return "Arcane Lore";
			if (id == 0xF) return "Magic Defense";
			if (id == 0x10) return "Mana Conversion";
			if (id == 0x12) return "Item Tinkering";
			if (id == 0x13) return "Assess Person";
			if (id == 0x14) return "Deception";
			if (id == 0x15) return "Healing";
			if (id == 0x16) return "Jump";
			if (id == 0x17) return "Lockpick";
			if (id == 0x18) return "Run";
			if (id == 0x1B) return "Assess Creature";
			if (id == 0x1C) return "Weapon Tinkering";
			if (id == 0x1D) return "Armor Tinkering";
			if (id == 0x1E) return "Magic Item Tinkering";
			if (id == 0x1F) return "Creature Enchantment";
			if (id == 0x20) return "Item Enchantment";
			if (id == 0x21) return "Life Magic";
			if (id == 0x22) return "War Magic";
			if (id == 0x23) return "Leadership";
			if (id == 0x24) return "Loyalty";
			if (id == 0x25) return "Fletching";
			if (id == 0x26) return "Alchemy";
			if (id == 0x27) return "Cooking";
			if (id == 0x28) return "Salvaging";
			if (id == 0x29) return "Two Handed Combat";
			// 0x2A
			if (id == 0x2B) return "Void";
			if (id == 0x2C) return "Heavy Weapons";
			if (id == 0x2D) return "Light Weapons";
			if (id == 0x2E) return "Finesse Weapons";
			if (id == 0x2F) return "Missile Weapons";
			if (id == 0x30) return "Shield";
			if (id == 0x31) return "Dual Wield";
			if (id == 0x32) return "Recklessness";
			if (id == 0x33) return "Sneak Attack";
			if (id == 0x34) return "Dirty Fighting";

			return "Unknown skill id: " + id;
		}

		/// <summary>
		/// This will return a mastery name by its id.
		/// For example, 1 returns "Unearmed Weapon".
		/// If the skill is unknown the following is returned: "Unknown mastery id: " + id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string GetMasteryNameById(int id)
		{
			// This list was taken from the Alinco source
			if (id == 0x1) return "Unarmed Weapon";
			if (id == 0x2) return "Sword";
			if (id == 0x3) return "Axe";
			if (id == 0x4) return "Mace";
			if (id == 0x5) return "Spear";
			if (id == 0x6) return "Dagger";
			if (id == 0x7) return "Staff";
			if (id == 0x8) return "Bow";
			if (id == 0x9) return "Crossbow";
			if (id == 0xA) return "Thrown";
			if (id == 0xB) return "Two Handed Combat";

			return "Unknown mastery id: " + id;
		}
	}
}
