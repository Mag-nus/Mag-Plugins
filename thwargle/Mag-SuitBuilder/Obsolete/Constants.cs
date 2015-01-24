/*using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mag_SuitBuilder
{
	public static class Constants
	{
		public static Mag.Shared.EquippableSlotFlags GetEquippableSlots(string name)
		{
			if (String.IsNullOrEmpty(name))
				return Mag.Shared.EquippableSlotFlags.None;

			// Built using info from: http://ac.wikkii.net/wiki/Armor_Coverage

			if (!IsUnderwear(name))
			{
				// Single Slot
				if (Regex.Match(name, "Helm").Success) return Mag.Shared.EquippableSlotFlags.Head;
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
				if (Regex.Match(name, "O-Yoroi Coat").Success) return EquippableSlotFlags.Chest; // Can only reduce to chest

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

			return Mag.Shared.EquippableSlotFlags.None;
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
	}
}
*/