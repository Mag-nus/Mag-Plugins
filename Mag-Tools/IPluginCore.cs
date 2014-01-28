
using MagTools.Trackers.Equipment;

namespace MagTools
{
	public interface IPluginCore
	{
		Macros.IInventoryPacker InventoryPacker { get; }

		IEquipmentTracker EquipmentTracker { get; }

		Macros.ILooter Looter { get; }

		bool ProcessMTCommand(string mtCommand);
	}
}
