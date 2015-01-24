using System.Collections.Generic;

using Mag.Shared;

namespace Mag_SuitBuilder.Equipment
{
	class EquipmentGroup : SortableBindingList<SuitBuildableMyWorldObject>
	{
		public bool ItemIsSurpassed(SuitBuildableMyWorldObject item)
		{
			if (item.ObjectClass != (int)ObjectClass.Armor && item.ObjectClass != (int)ObjectClass.Clothing && item.ObjectClass != (int)ObjectClass.Jewelry)
				return false;

			foreach (SuitBuildableMyWorldObject compareItem in this)
			{
				if (compareItem == item)
					continue;

				if (item.IsSurpassedBy(compareItem))
					return true;
			}

			return false;
		}

		public Dictionary<SuitBuildableMyWorldObject, List<SuitBuildableMyWorldObject>> GetUpgradeOptions(EquipmentGroup muleItems)
		{
			Dictionary<SuitBuildableMyWorldObject, List<SuitBuildableMyWorldObject>> upgrades = new Dictionary<SuitBuildableMyWorldObject, List<SuitBuildableMyWorldObject>>();

			foreach (var item in this)
			{
				List<SuitBuildableMyWorldObject> muleItemUpgrades = new List<SuitBuildableMyWorldObject>();

				foreach (var muleItem in muleItems)
				{
					if (item.IsSurpassedBy(muleItem))
						muleItemUpgrades.Add(muleItem);
				}

				if (muleItemUpgrades.Count > 0)
					upgrades.Add(item, muleItemUpgrades);
			}

			return upgrades;
		}
	}
}
