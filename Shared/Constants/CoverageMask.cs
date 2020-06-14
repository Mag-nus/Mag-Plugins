using System;
using System.Collections.Generic;

namespace Mag.Shared.Constants
{
	/// <summary>
	/// This is the mapping for LongValueKey.Coverage.
	/// It represents what body parts an armor piece covers when used in defensive/armor calculations.
	/// </summary>
	[Flags]
	public enum CoverageMask
	{
		None				= 0,

		Unknown				= 0x00000001, // Original pants abdomen?

		UnderwearUpperLegs	= 0x00000002, // I think... 0x13 = Abdomen/UpperLegs
		UnderwearLowerLegs	= 0x00000004, // I think... 0x16 = Abdomen/UpperLegs/LowerLegs
		UnderwearChest		= 0x00000008,
		UnderwearAbdomen	= 0x00000010, // Original shirt abdomen?
		UnderwearUpperArms	= 0x00000020,
		UnderwearLowerArms	= 0x00000040,
		//					= 0x00000080,

		OuterwearUpperLegs	= 0x00000100,
		OuterwearLowerLegs	= 0x00000200,
		OuterwearChest		= 0x00000400,
		OuterwearAbdomen	= 0x00000800,
		OuterwearUpperArms	= 0x00001000,
		OuterwearLowerArms	= 0x00002000,

		Head				= 0x00004000,
		Hands				= 0x00008000,
		Feet				= 0x00010000,

		Cloak				= 0x00020000,
	}

	public enum CoverageMaskHelper : uint
	{
		// for server comparison only
		Underwear = CoverageMask.UnderwearUpperLegs | CoverageMask.UnderwearLowerLegs | CoverageMask.UnderwearChest | CoverageMask.UnderwearAbdomen | CoverageMask.UnderwearUpperArms | CoverageMask.UnderwearLowerArms,
		Outerwear = CoverageMask.OuterwearUpperLegs | CoverageMask.OuterwearLowerLegs | CoverageMask.OuterwearChest | CoverageMask.OuterwearAbdomen | CoverageMask.OuterwearUpperArms | CoverageMask.OuterwearLowerArms | CoverageMask.Head | CoverageMask.Hands | CoverageMask.Feet,

		UnderwearLegs = CoverageMask.UnderwearUpperLegs | CoverageMask.UnderwearLowerLegs,
		UnderwearArms = CoverageMask.UnderwearUpperArms | CoverageMask.UnderwearLowerArms,

		OuterwearLegs = CoverageMask.OuterwearUpperLegs | CoverageMask.OuterwearLowerLegs,
		OuterwearArms = CoverageMask.OuterwearUpperArms | CoverageMask.OuterwearLowerArms,

		// exclude abdomen for searching
		UnderwearShirt = CoverageMask.UnderwearChest | CoverageMask.UnderwearUpperArms | CoverageMask.UnderwearLowerArms,
		UnderwearPants = CoverageMask.UnderwearUpperLegs | CoverageMask.UnderwearLowerLegs
	}

	public static class CoverageMaskExtensions
	{
		public static int GetTotalBitsSet(this CoverageMask value)
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

		public static bool IsBodyArmor(this CoverageMask value) { return ((int)value & 0x0001FF00) != 0; }
		public static bool IsRobe(this CoverageMask value) { return ((int)value == 0x00013F00); }
		public static bool IsUnderwear(this CoverageMask value) { return ((int)value & 0x0000007F) != 0; }
		public static bool IsShirt(this CoverageMask value) { return ((int)value & 0x00000078) != 0; }
		public static bool IsPants(this CoverageMask value) { return ((int)value & 0x00000017) != 0; }

		public static List<CoverageMask> ReductionOptions(this CoverageMask value)
		{
			List<CoverageMask> options = new List<CoverageMask>();

			if (value.GetTotalBitsSet() <= 1 || !value.IsBodyArmor() || value.IsRobe())
				options.Add(value);
			else
			{
				if (value == (CoverageMask.OuterwearUpperArms | CoverageMask.OuterwearLowerArms))
				{
					options.Add(CoverageMask.OuterwearUpperArms);
					options.Add(CoverageMask.OuterwearLowerArms);
				}
				else if (value == (CoverageMask.OuterwearUpperLegs | CoverageMask.OuterwearLowerLegs))
				{
					options.Add(CoverageMask.OuterwearUpperLegs);
					options.Add(CoverageMask.OuterwearLowerLegs);
				}
				else if (value == (CoverageMask.OuterwearLowerLegs | CoverageMask.Feet))
					options.Add(CoverageMask.Feet);
				else if (value == (CoverageMask.OuterwearChest | CoverageMask.OuterwearAbdomen))
					options.Add(CoverageMask.OuterwearChest);
				else if (value == (CoverageMask.OuterwearChest | CoverageMask.OuterwearAbdomen | CoverageMask.OuterwearUpperArms))
					options.Add(CoverageMask.OuterwearChest);
				else if (value == (CoverageMask.OuterwearChest | CoverageMask.OuterwearUpperArms | CoverageMask.OuterwearLowerArms))
					options.Add(CoverageMask.OuterwearChest);
				else if (value == (CoverageMask.OuterwearChest | CoverageMask.OuterwearUpperArms))
					options.Add(CoverageMask.OuterwearChest);
				else if (value == (CoverageMask.OuterwearAbdomen | CoverageMask.OuterwearUpperLegs | CoverageMask.OuterwearLowerLegs))
				{
					options.Add(CoverageMask.OuterwearAbdomen);
					options.Add(CoverageMask.OuterwearUpperLegs);
					options.Add(CoverageMask.OuterwearLowerLegs);
				}
				else if (value == (CoverageMask.OuterwearChest | CoverageMask.OuterwearAbdomen | CoverageMask.OuterwearUpperArms | CoverageMask.OuterwearLowerArms))
					options.Add(CoverageMask.OuterwearChest);
				else if (value == (CoverageMask.OuterwearAbdomen | CoverageMask.OuterwearUpperLegs))
				{
					// This is a emu piece that follows the pre-2010 retail guidelines
					// https://asheron.fandom.com/wiki/Announcements_-_2010/04_-_Shedding_Skin
					// For now, we assume only abdomen reduction
					options.Add(CoverageMask.OuterwearAbdomen);
				}
				else
					throw new Exception("Unable to determine reduction paths for CoverageMask of " + value);
			}

			return options;
		}
	}
}
