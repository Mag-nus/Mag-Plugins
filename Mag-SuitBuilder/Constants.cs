using System;
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
		}

		[Flags]
		public enum UnderwearCoverage
		{
			None			= 0,

			Chest			= 0x00000008,
			UpperArms		= 0x00000020,
			LowerArms		= 0x00000040,

			UpperLegs		= 0x00000100,
			LowerLegs		= 0x00000200,
			Abdomen			= 0x00000800,
		}

		
		public static EquippableSlotFlags GetEquippableSlots(string name)
		{
			// Built using info from: http://ac.wikkii.net/wiki/Armor_Coverage

			// Single Slot
			if (Regex.Match(name, "Helm").Success) return EquippableSlotFlags.Head;
			if (Regex.Match(name, "Coif ").Success) return EquippableSlotFlags.Head;
			if (Regex.Match(name, "Basinet").Success) return EquippableSlotFlags.Head;
			if (Regex.Match(name, "Cowl").Success) return EquippableSlotFlags.Head;
			if (Regex.Match(name, "Kabuton").Success) return EquippableSlotFlags.Head;
			if (Regex.Match(name, "Bandana ").Success) return EquippableSlotFlags.Head;
			if (Regex.Match(name, "Beret ").Success) return EquippableSlotFlags.Head;
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

			if (Regex.Match(name, "Breastplate").Success) return EquippableSlotFlags.Chest;
			if (Regex.Match(name, "Vest").Success) return EquippableSlotFlags.Chest;

			if (Regex.Match(name, "Pauldrons").Success) return EquippableSlotFlags.UpperArms;

			if (Regex.Match(name, "Bracers").Success) return EquippableSlotFlags.LowerArms;
			if (Regex.Match(name, "Vambraces").Success) return EquippableSlotFlags.LowerArms;
			if (Regex.Match(name, "Kote").Success) return EquippableSlotFlags.UpperArms;

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

			// Two Slot
			if (Regex.Match(name, "Sleeves").Success) return EquippableSlotFlags.UpperArms & EquippableSlotFlags.LowerArms;

			//if (Regex.Match(name, "Cuirass").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.Abdomen;
			if (Regex.Match(name, "Cuirass").Success) return EquippableSlotFlags.Chest;

			if (Regex.Match(name, "Leggings").Success) return EquippableSlotFlags.UpperLegs & EquippableSlotFlags.LowerLegs;

			// Three Slot
			if (Regex.Match(name, "Alduressa Coat").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.UpperArms & EquippableSlotFlags.LowerArms;
			if (Regex.Match(name, "Amuli Coat").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.UpperArms & EquippableSlotFlags.LowerArms;
			if (Regex.Match(name, "Chiran Coat").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.UpperArms & EquippableSlotFlags.LowerArms;

			//if (Regex.Match(name, "Shirt").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.Abdomen & EquippableSlotFlags.UpperArms;
			if (Regex.Match(name, "Shirt").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.UpperArms;

			if (Regex.Match(name, "Pants").Success) return EquippableSlotFlags.Abdomen & EquippableSlotFlags.UpperLegs & EquippableSlotFlags.LowerLegs;
			if (Regex.Match(name, "Leggings").Success) return EquippableSlotFlags.Abdomen & EquippableSlotFlags.UpperLegs & EquippableSlotFlags.LowerLegs;

			// Four and more slots
			//if (Regex.Match(name, "Hauberk").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.Abdomen & EquippableSlotFlags.UpperArms & EquippableSlotFlags.LowerArms;
			if (Regex.Match(name, "Hauberk").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.UpperArms & EquippableSlotFlags.LowerArms;
			//if (Regex.Match(name, "Jerkin").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.Abdomen & EquippableSlotFlags.UpperArms & EquippableSlotFlags.LowerArms;
			if (Regex.Match(name, "Jerkin").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.UpperArms & EquippableSlotFlags.LowerArms;
			//if (Regex.Match(name, "Leather Coat").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.Abdomen & EquippableSlotFlags.UpperArms & EquippableSlotFlags.LowerArms;
			if (Regex.Match(name, "Leather Coat").Success) return EquippableSlotFlags.Chest & EquippableSlotFlags.UpperArms & EquippableSlotFlags.LowerArms;

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
			if (Regex.Match(name, "Top").Success) return EquippableSlotFlags.Trinket;

			// Bracelet
			if (Regex.Match(name, "Bracelet").Success) return EquippableSlotFlags.LeftBracelet & EquippableSlotFlags.RightBracelet;

			// Ring
			if (Regex.Match(name, "Ring").Success) return EquippableSlotFlags.LeftBracelet & EquippableSlotFlags.RightBracelet;

			return EquippableSlotFlags.None;
		}

		public static UnderwearCoverage GetUnderwearCoverage(string name)
		{
			// Shirt
			if (Regex.Match(name, "Doublet").Success) return UnderwearCoverage.Chest;
			if (Regex.Match(name, "Jerkin").Success) return UnderwearCoverage.Chest;
			if (Regex.Match(name, "Vest").Success) return UnderwearCoverage.Chest;

			if (Regex.Match(name, "Tunic").Success) return UnderwearCoverage.Chest & UnderwearCoverage.UpperArms;
			if (Regex.Match(name, "Smock").Success) return UnderwearCoverage.Chest & UnderwearCoverage.UpperArms;

			if (Regex.Match(name, "Shirt").Success) return UnderwearCoverage.Chest & UnderwearCoverage.UpperArms & UnderwearCoverage.LowerArms;

			// Pants
			if (Regex.Match(name, "Breeches").Success) return UnderwearCoverage.Abdomen & UnderwearCoverage.UpperLegs;

			if (Regex.Match(name, "Pant").Success) return UnderwearCoverage.Abdomen & UnderwearCoverage.UpperLegs & UnderwearCoverage.LowerLegs;
			if (Regex.Match(name, "Trousers").Success) return UnderwearCoverage.Abdomen & UnderwearCoverage.UpperLegs & UnderwearCoverage.LowerLegs;

			return UnderwearCoverage.None;
		}
	}
}
