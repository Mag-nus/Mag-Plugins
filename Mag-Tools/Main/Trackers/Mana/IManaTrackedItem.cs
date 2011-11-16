using System;

namespace MagTools.Trackers.Mana
{
	public interface IManaTrackedItem
	{
		event Action<IManaTrackedItem> Changed;

		int Id { get; }

		int MaximumMana { get; }

		ManaTrackedItemState ItemState { get; }

		int CalculatedCurrentMana { get; }

		int ManaNeededToRefill { get; }

		TimeSpan ManaTimeRemaining { get; }
	}
}
