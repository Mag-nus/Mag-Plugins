
namespace MagTools
{
	public interface IPluginCore
	{
		Macros.IInventoryPacker InventoryPacker { get; }

		Trackers.Mana.IManaTracker ManaTracker { get; }

		Trackers.Combat.ICombatTracker CombatTracker { get; }
	}
}
