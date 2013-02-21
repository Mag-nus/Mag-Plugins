using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mag_SuitBuilder
{
	public static class Constants
	{
		/// <summary>
		/// These are the same values from AC for jewelry and armor.
		/// Underclothes use different values to represent coverage.
		/// </summary>
		[Flags]
		public enum EquippableSlotFlags
		{
			None			= 0,

			Head			= 0x00000001,
			Hands			= 0x00000020,
			//				= 0x00000040,
			//				= 0x00000080,
			Feet			= 0x00000100,
			Chest			= 0x00000200,
			Abdomen			= 0x00000400,
			UpperArms		= 0x00000800,
			LowerArms		= 0x00001000,
			UpperLegs		= 0x00002000,
			LowerLegs		= 0x00004000,
				
			Necklace		= 0x00008000,
			RightBracelet	= 0x00010000,
			LeftBracelet	= 0x00020000,
			RightRing		= 0x00040000,
			LeftRing		= 0x00080000,

			Trinket			= 0x04000000,

			Shirt			= 0x20000000,
			Pants			= 0x40000000,

			// Combos
			AllBodyArmor	= 0x00007F21,
			Bracelet		= 0x00030000,
			Ring			= 0x000C0000,
			CanHaveArmor	= 0x60007F21,
			Underwear		= 0x60000000,

			All				= 0x7FFFFFFF,
		}

		public static EquippableSlotFlags GetEquippableSlots(string name)
		{
			if (String.IsNullOrEmpty(name))
				return EquippableSlotFlags.None;

			// Built using info from: http://ac.wikkii.net/wiki/Armor_Coverage

			if (!IsUnderwear(name))
			{
				// Single Slot
				if (Regex.Match(name, "Helm").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Coif").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Basinet").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Cowl").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Kabuton").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Bandana").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Beret").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Cap").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Fez").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Kasa").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Turban").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Armet").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Baigha").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Circlet").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Coronet").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Crown").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Diadem").Success) return EquippableSlotFlags.Head;
                if (Regex.Match(name, "Heaume").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Hood").Success) return EquippableSlotFlags.Head;
				if (Regex.Match(name, "Qafiya").Success) return EquippableSlotFlags.Head;

				if (Regex.Match(name, "Breastplate").Success) return EquippableSlotFlags.Chest;
				if (Regex.Match(name, "Vest").Success) return EquippableSlotFlags.Chest;

				if (Regex.Match(name, "Pauldrons").Success) return EquippableSlotFlags.UpperArms;

				if (Regex.Match(name, "Bracers").Success) return EquippableSlotFlags.LowerArms;
				if (Regex.Match(name, "Vambraces").Success) return EquippableSlotFlags.LowerArms;
				if (Regex.Match(name, "Kote").Success) return EquippableSlotFlags.LowerArms;

				if (Regex.Match(name, "Gauntlets").Success) return EquippableSlotFlags.Hands;
				if (Regex.Match(name, "Gloves").Success) return EquippableSlotFlags.Hands;

				if (Regex.Match(name, "Girth").Success) return EquippableSlotFlags.Abdomen;
				if (Regex.Match(name, "Shorts").Success) return EquippableSlotFlags.Abdomen;

				if (Regex.Match(name, "Tassets").Success) return EquippableSlotFlags.UpperLegs;

				if (Regex.Match(name, "Greaves").Success) return EquippableSlotFlags.LowerLegs;

				if (Regex.Match(name, "Boots").Success) return EquippableSlotFlags.Feet;
				if (Regex.Match(name, "Sandals").Success) return EquippableSlotFlags.Feet;
				if (Regex.Match(name, "Sollerets").Success) return EquippableSlotFlags.Feet;
				if (Regex.Match(name, "Slippers").Success) return EquippableSlotFlags.Feet;
				if (Regex.Match(name, "Loafers").Success) return EquippableSlotFlags.Feet;
				if (Regex.Match(name, "Shoes").Success) return EquippableSlotFlags.Feet;
				if (Regex.Match(name, "Boots").Success) return EquippableSlotFlags.Feet;

				if (Regex.Match(name, "Empowered Robe of the Perfect Light").Success) return EquippableSlotFlags.Chest;
				if (Regex.Match(name, "Over-robe").Success) return EquippableSlotFlags.Chest;

				// Two Slot
				if (Regex.Match(name, "Sleeves").Success) return EquippableSlotFlags.UpperArms | EquippableSlotFlags.LowerArms;

				//if (Regex.Match(name, "Cuirass").Success) return EquippableSlotFlags.Chest | EquippableSlotFlags.Abdomen;
				if (Regex.Match(name, "Cuirass").Success) return EquippableSlotFlags.Chest; // Can only reduce to chest

				if (Regex.Match(name, "Leggings").Success && Regex.Match(name, "Celdon|Chainmail|Diforsa|Leather|Nariyid|Olthoi Celdon|Platemail|Scalemail|Studded Leather|Yoroi").Success) return EquippableSlotFlags.UpperLegs | EquippableSlotFlags.LowerLegs;

				// Three Slot
				//if (Regex.Match(name, "Alduressa Coat").Success) return EquippableSlotFlags.Chest | EquippableSlotFlags.UpperArms | EquippableSlotFlags.LowerArms;
				if (Regex.Match(name, "Alduressa Coat").Success) return EquippableSlotFlags.Chest; // Can only reduce to chest
				//if (Regex.Match(name, "Amuli Coat").Success) return EquippableSlotFlags.Chest | EquippableSlotFlags.UpperArms | EquippableSlotFlags.LowerArms;
				if (Regex.Match(name, "Amuli Coat").Success) return EquippableSlotFlags.Chest; // Can only reduce to chest
				//if (Regex.Match(name, "Chiran Coat").Success) return EquippableSlotFlags.Chest | EquippableSlotFlags.UpperArms | EquippableSlotFlags.LowerArms;
				if (Regex.Match(name, "Chiran Coat").Success) return EquippableSlotFlags.Chest; // Can only reduce to chest

				//if (Regex.Match(name, "Shirt").Success) return EquippableSlotFlags.Chest | EquippableSlotFlags.Abdomen | EquippableSlotFlags.UpperArms;
				if (Regex.Match(name, "Shirt").Success) return EquippableSlotFlags.Chest; // Can only reduce to chest

				if (Regex.Match(name, "Pants").Success) return EquippableSlotFlags.Abdomen | EquippableSlotFlags.UpperLegs | EquippableSlotFlags.LowerLegs;
				if (Regex.Match(name, "Leggings").Success && Regex.Match(name, "Alduressa|Amuli|Chiran|Koujia|Lorica|Olthoi Alduressa|Olthoi Amuli|Olthoi Koujia|Tenassa").Success) return EquippableSlotFlags.Abdomen | EquippableSlotFlags.UpperLegs | EquippableSlotFlags.LowerLegs;

				// Four and more slots
				//if (Regex.Match(name, "Hauberk").Success) return EquippableSlotFlags.Chest | EquippableSlotFlags.Abdomen | EquippableSlotFlags.UpperArms | EquippableSlotFlags.LowerArms;
				if (Regex.Match(name, "Hauberk").Success) return EquippableSlotFlags.Chest; // Can only reduce to chest
				//if (Regex.Match(name, "Jerkin").Success) return EquippableSlotFlags.Chest | EquippableSlotFlags.Abdomen | EquippableSlotFlags.UpperArms | EquippableSlotFlags.LowerArms;
				if (Regex.Match(name, "Jerkin").Success) return EquippableSlotFlags.Chest; //Can only reduce to chest
				//if (Regex.Match(name, "Leather Coat").Success) return EquippableSlotFlags.Chest | EquippableSlotFlags.Abdomen | EquippableSlotFlags.UpperArms | EquippableSlotFlags.LowerArms;
				if (Regex.Match(name, "Leather Coat").Success) return EquippableSlotFlags.Chest; // Can only reduce to chest

				// Necklace
				if (Regex.Match(name, "Amulet").Success) return EquippableSlotFlags.Necklace;
				if (Regex.Match(name, "Necklace").Success) return EquippableSlotFlags.Necklace;
				if (Regex.Match(name, "Collar").Success) return EquippableSlotFlags.Necklace;
				if (Regex.Match(name, "Faur").Success) return EquippableSlotFlags.Necklace;
				if (Regex.Match(name, "Pendant").Success) return EquippableSlotFlags.Necklace;
				if (Regex.Match(name, "Gorget").Success) return EquippableSlotFlags.Necklace;

				// Trinket
				if (Regex.Match(name, "Compass").Success) return EquippableSlotFlags.Trinket;
				if (Regex.Match(name, "Goggles").Success) return EquippableSlotFlags.Trinket;
				if (Regex.Match(name, "Scarab").Success) return EquippableSlotFlags.Trinket;
				if (Regex.Match(name, "Puzzle Box").Success) return EquippableSlotFlags.Trinket;
				if (Regex.Match(name, "Pocket Watch").Success) return EquippableSlotFlags.Trinket;
				if (Regex.Match(name, "Top$").Success) return EquippableSlotFlags.Trinket;

				// Bracelet
				if (Regex.Match(name, "Bracelet").Success) return EquippableSlotFlags.Bracelet;

				// Ring
				if (Regex.Match(name, "Ring").Success) return EquippableSlotFlags.Ring;
			}
			else
			{
				UnderwearCoverage underwearCoverage = GetUnderwearCoverage(name);

				// Shirt
				if ((underwearCoverage & UnderwearCoverage.Chest) == UnderwearCoverage.Chest) return EquippableSlotFlags.Shirt;

				// Pants
				if ((underwearCoverage & UnderwearCoverage.Abdomen) == UnderwearCoverage.Abdomen) return EquippableSlotFlags.Pants;
			}

			return EquippableSlotFlags.None;
		}

		public static bool IsUnderwear(string name)
		{
			if (String.IsNullOrEmpty(name))
				return false;

			// Old Method
			//if (Regex.Match(name, "Leather Jerkin").Success) return false;
			//if (Regex.Match(name, "Leather Vest").Success) return false;
			//if (Regex.Match(name, "Chainmail Shirt").Success) return false;
			//if (Regex.Match(name, "Leather Shirt").Success) return false;
			//if (Regex.Match(name, "Leather Pants").Success) return false;

			// New Method
			if (Regex.Match(name, "Leather").Success) return false;
			if (Regex.Match(name, "Chainmail").Success) return false;
			if (Regex.Match(name, "Hide").Success) return false;

			if (GetUnderwearCoverage(name) != UnderwearCoverage.None)
				return true;

			return false;
		}

		[Flags]
		public enum UnderwearCoverage
		{
			None = 0,

			Chest		= 0x00000008,
			UpperArms	= 0x00000020,
			LowerArms	= 0x00000040,
			FullShirt	= 0x00000068,

			UpperLegs	= 0x00000100,
			LowerLegs	= 0x00000200,
			Abdomen		= 0x00000800,
			FullPants	= 0x00000B00,
		}

		public static UnderwearCoverage GetUnderwearCoverage(string name)
		{
			if (String.IsNullOrEmpty(name))
				return UnderwearCoverage.None;

			// Shirt
			if (Regex.Match(name, "Doublet").Success) return UnderwearCoverage.Chest;
			if (Regex.Match(name, "Jerkin").Success) return UnderwearCoverage.Chest;
			if (Regex.Match(name, "Vest").Success) return UnderwearCoverage.Chest;

			if (Regex.Match(name, "Tunic").Success) return UnderwearCoverage.Chest | UnderwearCoverage.UpperArms;
			if (Regex.Match(name, "Smock").Success) return UnderwearCoverage.Chest | UnderwearCoverage.UpperArms;

			if (Regex.Match(name, "Shirt").Success) return UnderwearCoverage.FullShirt;

			// Pants
			if (Regex.Match(name, "Breeches").Success) return UnderwearCoverage.Abdomen | UnderwearCoverage.UpperLegs;

			if (Regex.Match(name, "Pant").Success) return UnderwearCoverage.FullPants;
			if (Regex.Match(name, "Trousers").Success) return UnderwearCoverage.FullPants;

			return UnderwearCoverage.None;
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
			};

			return attributeSetInfo;
		}
	}

	public static class EnumExtensions
	{
		public static int GetTotalBitsSet(this Constants.EquippableSlotFlags value)
		{
			int slotFlags = (int)value;
			int bitsSet = 0;

			while (slotFlags != 0)
			{
				if ((slotFlags & 1) == 1)
					bitsSet++;
				slotFlags >>= 1;
			}

			return bitsSet;
		}

		public static bool IsBodyArmor(this Constants.EquippableSlotFlags value)
		{
			return (value & Constants.EquippableSlotFlags.AllBodyArmor) != 0;
		}

		public static int GetTotalBitsSet(this Constants.UnderwearCoverage value)
		{
			int slotFlags = (int)value;
			int bitsSet = 0;

			while (slotFlags != 0)
			{
				if ((slotFlags & 1) == 1)
					bitsSet++;
				slotFlags >>= 1;
			}

			return bitsSet;
		}
	}
}
