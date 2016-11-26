
using Decal.Adapter.Wrappers;

namespace MagTools.Trackers.Inventory
{
	class TrackedInventory : ValueSnapShotGroup
	{
		public readonly string Name;
		public readonly ObjectClass ObjectClass;
		public readonly int Icon;
		public readonly int ItemValue;

		public TrackedInventory(string name, ObjectClass objectClass, int icon, int itemValue, int minutesToRetain) : base(minutesToRetain)
		{
			Name = name;
			ObjectClass = objectClass;
			Icon = icon;
			ItemValue = itemValue;
		}
	}
}
