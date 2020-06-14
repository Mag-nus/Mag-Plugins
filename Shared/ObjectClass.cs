using System;

using Mag.Shared.Constants;

namespace Mag.Shared
{
	public enum ObjectClass
	{
		Unknown = 0,
		MeleeWeapon = 1,
		Armor = 2,
		Clothing = 3,
		Jewelry = 4,
		Monster = 5,
		Food = 6,
		Money = 7,
		Misc = 8,
		MissileWeapon = 9,
		Container = 10,
		Gem = 11,
		SpellComponent = 12,
		Key = 13,
		Portal = 14,
		TradeNote = 15,
		ManaStone = 16,
		Plant = 17,
		BaseCooking = 18,
		BaseAlchemy = 19,
		BaseFletching = 20,
		CraftedCooking = 21,
		CraftedAlchemy = 22,
		CraftedFletching = 23,
		Player = 24,
		Vendor = 25,
		Door = 26,
		Corpse = 27,
		Lifestone = 28,
		HealingKit = 29,
		Lockpick = 30,
		WandStaffOrb = 31,
		Bundle = 32,
		Book = 33,
		Journal = 34,
		Sign = 35,
		Housing = 36,
		Npc = 37,
		Foci = 38,
		Salvage = 39,
		Ust = 40,
		Services = 41,
		Scroll = 42,
		NumObjectClasses = 43,
	}

	public static class ObjectClassTools
	{
		/// <summary>
		/// Converts a decal specific IntValueKey to the actual IntValueKey.
		/// If this is not an IntValueKey, 0 will be returned.
		/// </summary>
		public static ObjectClass FromWeenieType(ItemType itemType, WeenieType weenieType)
		{
			var result = ObjectClass.Unknown;

			if ((itemType & ItemType.MeleeWeapon) != 0)
				result = ObjectClass.MeleeWeapon;
			else if ((itemType & ItemType.Armor) != 0)
				result = ObjectClass.Armor;
			else if ((itemType & ItemType.Clothing) != 0)
				result = ObjectClass.Clothing;
			else if ((itemType & ItemType.Jewelry) != 0)
				result = ObjectClass.Jewelry;
			else if ((itemType & ItemType.Creature) != 0)
				result = ObjectClass.Monster;
			else if ((itemType & ItemType.Food) != 0)
				result = ObjectClass.Food;
			else if ((itemType & ItemType.Money) != 0)
				result = ObjectClass.Money;
			else if ((itemType & ItemType.Misc) != 0)
				result = ObjectClass.Misc;
			else if ((itemType & ItemType.MissileWeapon) != 0)
				result = ObjectClass.MissileWeapon;
			else if ((itemType & ItemType.Container) != 0)
				result = ObjectClass.Container;
			else if ((itemType & ItemType.Useless) != 0)
				result = ObjectClass.Bundle;
			else if ((itemType & ItemType.Gem) != 0)
				result = ObjectClass.Gem;
			else if ((itemType & ItemType.SpellComponents) != 0)
				result = ObjectClass.SpellComponent;
			else if ((itemType & ItemType.Key) != 0)
				result = ObjectClass.Key;
			else if ((itemType & ItemType.Caster) != 0)
				result = ObjectClass.WandStaffOrb;
			else if ((itemType & ItemType.Portal) != 0)
				result = ObjectClass.Portal;
			else if ((itemType & ItemType.PromissoryNote) != 0)
				result = ObjectClass.TradeNote;
			else if ((itemType & ItemType.ManaStone) != 0)
				result = ObjectClass.ManaStone;
			else if ((itemType & ItemType.Service) != 0)
				result = ObjectClass.Services;
			else if ((itemType & ItemType.MagicWieldable) != 0)
				result = ObjectClass.Plant;
			else if ((itemType & ItemType.CraftCookingBase) != 0)
				result = ObjectClass.BaseCooking;
			else if ((itemType & ItemType.CraftAlchemyBase) != 0)
				result = ObjectClass.BaseAlchemy;
			//else if ((itemType & ItemType.01000000)
			//	result = ObjectClass.BaseFletching;
			else if ((itemType & ItemType.CraftFletchingBase) != 0)
				result = ObjectClass.CraftedCooking;
			else if ((itemType & ItemType.CraftAlchemyIntermediate) != 0)
				result = ObjectClass.CraftedAlchemy;
			else if ((itemType & ItemType.CraftFletchingIntermediate) != 0)
				result = ObjectClass.CraftedFletching;
			else if ((itemType & ItemType.TinkeringTool) != 0)
				result = ObjectClass.Ust;
			else if ((itemType & ItemType.TinkeringMaterial) != 0)
				result = ObjectClass.Salvage;

			/*
			if (Behavior & 0x00000008)
				result = ObjectClass.Player;
			else if (Behavior & 0x00000200)
				result = ObjectClass.Vendor;
			else if (Behavior & 0x00001000)
				result = ObjectClass.Door;
			else if (Behavior & 0x00002000)
				result = ObjectClass.Corpse;
			else if (Behavior & 0x00004000)
				result = ObjectClass.Lifestone;
			else if (Behavior & 0x00008000)
				result = ObjectClass.Food;
			else if (Behavior & 0x00010000)
				result = ObjectClass.HealingKit;
			else if (Behavior & 0x00020000)
				result = ObjectClass.Lockpick;
			else if (Behavior & 0x00040000)
				result = ObjectClass.Portal;
			else if (Behavior & 0x00800000)
				result = ObjectClass.Foci;
			else if (Behavior & 0x00000001)
				result = ObjectClass.Container;
			*/

			/*if (((itemType & ItemType.Writable) != 0) && (Behavior & 0x00000100) && result == ObjectClass.Unknown)
			{
				if (pCreate->m_Behavior & 0x00000002)
					result = ObjectClass.Journal;
				else if (pCreate->m_Behavior & 0x00000004)
					result = ObjectClass.Sign;
				else if (!(pCreate->m_Behavior & 0x0000000F))
					result = ObjectClass.Book;
			}*/

			/*if (((itemType & ItemType.Writable) != 0) && ((GameDataFlags1 & 0x00400000) != 0))
				result = ObjectClass.Scroll;*/

			//throw new Exception($"Unable to convert WeenieType {input} to an ObjectClass.");

			return result;
		}
	}
}
