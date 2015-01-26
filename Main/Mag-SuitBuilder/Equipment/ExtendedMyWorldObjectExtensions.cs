using System.Collections.Generic;

using Mag.Shared;

using Mag_SuitBuilder.Search;

namespace Mag_SuitBuilder.Equipment
{
	static class ExtendedMyWorldObjectExtensions
	{
		public static Dictionary<ExtendedMyWorldObject, List<ExtendedMyWorldObject>> GetUpgradeOptions(this ICollection<ExtendedMyWorldObject> value, ICollection<ExtendedMyWorldObject> muleItems)
		{
			Dictionary<ExtendedMyWorldObject, List<ExtendedMyWorldObject>> upgrades = new Dictionary<ExtendedMyWorldObject, List<ExtendedMyWorldObject>>();

			foreach (var item in value)
			{
				if (item.ObjectClass != (int)ObjectClass.Armor && item.ObjectClass != (int)ObjectClass.Clothing && item.ObjectClass != (int)ObjectClass.Jewelry)
					continue;

				var leanItem = new LeanMyWorldObject(item);

				List<ExtendedMyWorldObject> muleItemUpgrades = new List<ExtendedMyWorldObject>();

				foreach (var muleItem in muleItems)
				{
					if (muleItem.ObjectClass != (int)ObjectClass.Armor && muleItem.ObjectClass != (int)ObjectClass.Clothing && muleItem.ObjectClass != (int)ObjectClass.Jewelry)
						continue;

					var leanMuleItem = new LeanMyWorldObject(muleItem);

					if (leanItem.IsSurpassedBy(leanMuleItem))
						muleItemUpgrades.Add(muleItem);
				}

				if (muleItemUpgrades.Count > 0)
					upgrades.Add(item, muleItemUpgrades);
			}

			return upgrades;
		}
	}
}
