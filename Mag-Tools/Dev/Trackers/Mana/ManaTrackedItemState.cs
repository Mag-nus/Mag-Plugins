
namespace MagTools.Trackers.Mana
{
	public enum ManaTrackedItemState
	{
		/// <summary>
		/// We don't know what state the item is in yet. Probably because it doesn't have IdData
		/// </summary>
		Unknown,

		/// <summary>
		/// The item does not support activation, probably because it has no spells.
		/// </summary>
		NotActivatable,

		Active,

		NotActive,
	}
}
