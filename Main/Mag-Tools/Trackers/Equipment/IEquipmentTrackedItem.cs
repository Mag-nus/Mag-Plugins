using System;

namespace MagTools.Trackers.Equipment
{
	public interface IEquipmentTrackedItem
	{
		event Action<IEquipmentTrackedItem> Changed;

		int Id { get; }

		int MaximumMana { get; }

		EquipmentTrackedItemState ItemState { get; }

		int CalculatedCurrentMana { get; }

		int ManaNeededToRefill { get; }

		TimeSpan ManaTimeRemaining { get; }
	}
}
