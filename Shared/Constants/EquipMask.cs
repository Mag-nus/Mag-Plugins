using System;

namespace Mag.Shared.Constants
{
	/// <summary>
	/// This is the mapping for LongValueKey.EquippableSlots.
	/// It represents where you can drag items to on your paper doll.
	/// </summary>
	[Flags]
	public enum EquipMask : uint
	{
        None                = 0x00000000,

        HeadWear            = 0x00000001,

        ChestWear           = 0x00000002,
        AbdomenWear         = 0x00000004,
        UpperArmWear        = 0x00000008,
        LowerArmWear        = 0x00000010,

        HandWear            = 0x00000020,

        UpperLegWear        = 0x00000040,
        LowerLegWear        = 0x00000080,

        FootWear            = 0x00000100,
        ChestArmor          = 0x00000200,
        AbdomenArmor        = 0x00000400,
        UpperArmArmor       = 0x00000800,
        LowerArmArmor       = 0x00001000,
        UpperLegArmor       = 0x00002000,
        LowerLegArmor       = 0x00004000,

        NeckWear            = 0x00008000,
        WristWearLeft       = 0x00010000,
        WristWearRight      = 0x00020000,
        FingerWearLeft      = 0x00040000,
        FingerWearRight     = 0x00080000,

        MeleeWeapon         = 0x00100000,
        Shield              = 0x00200000,
        MissileWeapon       = 0x00400000,
        MissileAmmo         = 0x00800000,
        Held                = 0x01000000,
        TwoHanded           = 0x02000000,

        TrinketOne          = 0x04000000,
        Cloak               = 0x08000000,

        SigilOne            = 0x10000000, // Blue
        SigilTwo            = 0x20000000, // Yellow
        SigilThree          = 0x40000000, // Red

        Clothing            = 0x80000000 | HeadWear | ChestWear | AbdomenWear | UpperArmWear | LowerArmWear | HandWear | UpperLegWear | LowerLegWear | FootWear,
        Armor               = ChestArmor | AbdomenArmor | UpperArmArmor | LowerArmArmor | UpperLegArmor | LowerLegArmor | FootWear,
        ArmorExclusive      = ChestArmor | AbdomenArmor | UpperArmArmor | LowerArmArmor | UpperLegArmor | LowerLegArmor,
        Extremity           = HeadWear | HandWear | FootWear,
        Jewelry             = NeckWear | WristWearLeft | WristWearRight | FingerWearLeft | FingerWearRight | TrinketOne | Cloak | SigilOne | SigilTwo | SigilThree,
        WristWear           = WristWearLeft | WristWearRight,
        FingerWear          = FingerWearLeft | FingerWearRight,
        Sigil               = SigilOne | SigilTwo | SigilThree,
        ReadySlot           = Held | TwoHanded | TrinketOne | Cloak | SigilOne | SigilTwo,
        Weapon              = SigilTwo | TrinketOne | Held,
        WeaponReadySlot     = SigilOne | SigilTwo | TrinketOne | Held,
        Selectable          = MeleeWeapon | Shield | MissileWeapon | Held | TwoHanded,
        SelectablePlusAmmo  = Selectable | MissileAmmo,
        All                 = 0x7FFFFFFF,
        CanGoInReadySlot    = 0x7FFFFFFF
	}

	public static class EquipMaskExtensions
	{
		public static int GetTotalBitsSet(this EquipMask value)
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

		// Some feet armor have EquipMask.Feet | EquipMask.PantsLowerLegs

		public static bool IsBodyArmor(this EquipMask value)
		{
			return ((int)value & 0x00007F21) != 0;
		}

		public static bool IsCoreBodyArmor(this EquipMask value)
		{
			return (value & (EquipMask.ChestArmor | EquipMask.UpperArmArmor | EquipMask.LowerArmArmor | EquipMask.AbdomenArmor | EquipMask.UpperLegArmor | EquipMask.LowerLegArmor)) != 0;
		}

		public static bool IsExtremityBodyArmor(this EquipMask value)
		{
			return (value & (EquipMask.FootWear | EquipMask.HandWear | EquipMask.HeadWear)) != 0;
		}

		public static bool IsUnderwear(this EquipMask value)
		{
			if (value == (EquipMask.FootWear | EquipMask.LowerLegWear))
				return false;

			return ((int)value & 0x000000DE) != 0;
		}

		public static bool IsShirt(this EquipMask value)
		{
			return ((int)value & 0x0000001A) != 0;
		}

		public static bool IsPants(this EquipMask value)
		{
			if (value == (EquipMask.FootWear | EquipMask.LowerLegWear))
				return false;

			return ((int)value & 0x000000C4) != 0;
		}
	}
}
