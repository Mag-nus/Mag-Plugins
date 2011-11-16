
namespace MagTools
{
	public interface IPluginCore
	{
		Macros.IInventoryPacker InventoryPacker { get; }

		Trackers.Mana.IManaTracker ManaTracker { get; }

		Macros.IChestLooter ChestLooter { get; }
	}
}
