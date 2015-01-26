using System.Collections.Generic;

using Mag.Shared;

namespace Mag_SuitBuilder.Search
{
	static class LeanMyWorldObjectExtensions
	{
		public static bool ItemIsSurpassed(this ICollection<LeanMyWorldObject> value, LeanMyWorldObject piece)
		{
			if (piece.ObjectClass != (int)ObjectClass.Armor && piece.ObjectClass != (int)ObjectClass.Clothing && piece.ObjectClass != (int)ObjectClass.Jewelry)
				return false;

			foreach (var compareItem in value)
			{
				if (compareItem == piece)
					continue;

				if (piece.IsSurpassedBy(compareItem))
					return true;
			}

			return false;
		}
	}
}
