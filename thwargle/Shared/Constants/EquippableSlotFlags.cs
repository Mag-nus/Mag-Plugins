using System;

namespace Mag.Shared.Constants
{
	/// <summary>
	/// This is the mapping for LongValueKey.EquippableSlots.
	/// It represents where you can drag items to on your paper doll.
	/// </summary>
	[Flags]
	public enum EquippableSlotFlags
	{
		None			= 0,

		Head			= 0x00000001,

		ShirtChest		= 0x00000002,
		PantsAbdomen	= 0x00000004,
		ShirtUpperArms	= 0x00000008,
		ShirtLowerArms	= 0x00000010,

		Hands			= 0x00000020,

		PantsUpperLegs	= 0x00000040,
		PantsLowerLegs	= 0x00000080,

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

		MeleeWeapon		= 0x00100000,
		Shield			= 0x00200000,
		MissileWeapon	= 0x00400000,
		MissileAmmo		= 0x00800000,
		Wand			= 0x01000000,
		TwoHandWeapon	= 0x02000000,

		Trinket			= 0x04000000,
		Cloak			= 0x08000000,

		BlueAetheria	= 0x10000000,
		YellowAetheria	= 0x20000000,
		RedAetheria		= 0x40000000,
		//				= 0x80000000,
	}

	public static class EquippableSlotFlagsExtensions
	{
		public static int GetTotalBitsSet(this EquippableSlotFlags value)
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

		// Some feet armor have EquippableSlotFlags.Feet | EquippableSlotFlags.PantsLowerLegs

		public static bool IsBodyArmor(this EquippableSlotFlags value)
		{
			return ((int)value & 0x00007F21) != 0;
		}

		public static bool IsCoreBodyArmor(this EquippableSlotFlags value)
		{
			return (value & (EquippableSlotFlags.Chest | EquippableSlotFlags.UpperArms | EquippableSlotFlags.LowerArms | EquippableSlotFlags.Abdomen | EquippableSlotFlags.UpperLegs | EquippableSlotFlags.LowerLegs)) != 0;
		}

		public static bool IsExtremityBodyArmor(this EquippableSlotFlags value)
		{
			return (value & (EquippableSlotFlags.Feet | EquippableSlotFlags.Hands | EquippableSlotFlags.Head)) != 0;
		}

		public static bool IsUnderwear(this EquippableSlotFlags value)
		{
			if (value == (EquippableSlotFlags.Feet | EquippableSlotFlags.PantsLowerLegs))
				return false;

			return ((int)value & 0x000000DE) != 0;
		}

		public static bool IsShirt(this EquippableSlotFlags value)
		{
			return ((int)value & 0x0000001A) != 0;
		}

		public static bool IsPants(this EquippableSlotFlags value)
		{
			if (value == (EquippableSlotFlags.Feet | EquippableSlotFlags.PantsLowerLegs))
				return false;

			return ((int)value & 0x000000C4) != 0;
		}
	}
}
