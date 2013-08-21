using System;
using System.Collections.Generic;

namespace Mag.Shared.Constants
{
	/// <summary>
	/// This is the mapping for LongValueKey.Coverage.
	/// It represents what body parts an armor piece covers when used in defensive/armor calculations.
	/// </summary>
	[Flags]
	public enum CoverageFlags
	{
		None			= 0,

		UnderAbdomenPre	= 0x00000001, // Original pants abdomen?
		UnderUpperLegs	= 0x00000002, // I think... 0x13 = Abdomen/UpperLegs
		UnderLowerLegs	= 0x00000004, // I think... 0x16 = Abdomen/UpperLegs/LowerLegs
		UnderChest		= 0x00000008,
		UnderAbdomen	= 0x00000010, // Original shirt abdomen?
		UnderUpperArms	= 0x00000020,
		UnderLowerArms	= 0x00000040,
		//				= 0x00000080,
		UpperLegs		= 0x00000100,
		LowerLegs		= 0x00000200,
		Chest			= 0x00000400,
		Abdomen			= 0x00000800,
		UpperArms		= 0x00001000,
		LowerArms		= 0x00002000,
		Head			= 0x00004000,
		Hands			= 0x00008000,
		Feet			= 0x00010000,

		Cloak			= 0x00020000,
	}

	public static class CoverageFlagsExtensions
	{
		public static int GetTotalBitsSet(this CoverageFlags value)
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

		public static bool IsBodyArmor(this CoverageFlags value) { return ((int)value & 0x0001FF00) != 0; }
		public static bool IsUnderwear(this CoverageFlags value) { return ((int)value & 0x0000007F) != 0; }
		public static bool IsShirt(this CoverageFlags value) { return ((int)value & 0x00000078) != 0; }
		public static bool IsPants(this CoverageFlags value) { return ((int)value & 0x00000017) != 0; }

		public static List<CoverageFlags> ReductionOptions(this CoverageFlags value)
		{
			List<CoverageFlags> options = new List<CoverageFlags>();

			if (value.GetTotalBitsSet() <= 1 || !value.IsBodyArmor())
				options.Add(value);
			else
			{
				if (value == (CoverageFlags.UpperArms | CoverageFlags.LowerArms))
				{
					options.Add(CoverageFlags.UpperArms);
					options.Add(CoverageFlags.LowerArms);
				}
				else if (value == (CoverageFlags.UpperLegs | CoverageFlags.LowerLegs))
				{
					options.Add(CoverageFlags.UpperLegs);
					options.Add(CoverageFlags.LowerLegs);
				}
				else if (value == (CoverageFlags.Chest | CoverageFlags.Abdomen))
					options.Add(CoverageFlags.Chest);
				else if (value == (CoverageFlags.Chest | CoverageFlags.Abdomen | CoverageFlags.UpperArms))
					options.Add(CoverageFlags.Chest);
				else if (value == (CoverageFlags.Chest | CoverageFlags.UpperArms | CoverageFlags.LowerArms))
					options.Add(CoverageFlags.Chest);
				else if (value == (CoverageFlags.Abdomen | CoverageFlags.UpperLegs | CoverageFlags.LowerLegs))
				{
					options.Add(CoverageFlags.Abdomen);
					options.Add(CoverageFlags.UpperLegs);
					options.Add(CoverageFlags.LowerLegs);
				}
				else if (value == (CoverageFlags.Chest | CoverageFlags.Abdomen | CoverageFlags.UpperArms | CoverageFlags.LowerArms))
					options.Add(CoverageFlags.Chest);
				else
					throw new Exception("Unable to determine reduction paths for CoverageFlags of " + value);
			}

			return options;
		}
	}
}
