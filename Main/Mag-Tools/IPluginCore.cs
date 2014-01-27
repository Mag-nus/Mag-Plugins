
using MagTools.Trackers.Equipment;

namespace MagTools
{
	public interface IPluginCore
	{
		Macros.IInventoryPacker InventoryPacker { get; }

		IEquipmentTracker EquipmentTracker { get; }

		Macros.ILooter Looter { get; }

		void ProcessMTCommand(string mtCommand);
	}
}
