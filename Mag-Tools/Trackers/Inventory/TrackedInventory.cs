using System;

using Decal.Adapter.Wrappers;

namespace MagTools.Trackers.Inventory
{
	class TrackedInventory : TrackedSnapShot<int>
	{
		public readonly string Name;
		public readonly ObjectClass ObjectClass;
		public readonly int Icon;
		public readonly int ItemValue;

		public TrackedInventory(string name, ObjectClass objectClass, int icon, int itemValue)
		{
			Name = name;
			ObjectClass = objectClass;
			Icon = icon;
			ItemValue = itemValue;
		}

		public TrackedInventory(string name, ObjectClass objectClass, int icon, int itemValue, DateTime timeAtCount, int count) : this(name, objectClass, icon, itemValue)
		{
			SnapShots.Add(new SnapShot(timeAtCount, count));
		}

		public int LastKnownCount
		{
			get
			{
				if (SnapShots.Count > 0)
					return SnapShots[SnapShots.Count - 1].Value;

				return 0;
			}
		}

		/// <summary>
		/// Returns the item count difference over the history of period.
		/// </summary>
		public double GetItemsCountDifference(TimeSpan historyPeriod, TimeSpan usagePeriod)
		{
			if (SnapShots.Count == 1)
				return 0;

			var closestPastTarget = GetSnapShotClosestToTime(DateTime.Now - historyPeriod);

			return (LastKnownCount - closestPastTarget.Value) * (usagePeriod.TotalMinutes / (DateTime.Now - closestPastTarget.TimeStamp).TotalMinutes);
		}

		/// <summary>
		/// This will return the estimated time to depletion given the history recorded over period.
		/// </summary>
		public TimeSpan GetTimeToDepletion(TimeSpan period)
		{
			if (SnapShots.Count == 1 || LastKnownCount == 0)
				return TimeSpan.Zero;

			var closestPastTarget = GetSnapShotClosestToTime(DateTime.Now - period);

			if (LastKnownCount >= closestPastTarget.Value)
				return TimeSpan.MaxValue;

			return TimeSpan.FromSeconds(LastKnownCount / ((closestPastTarget.Value - LastKnownCount) / (DateTime.Now - closestPastTarget.TimeStamp).TotalSeconds));
		}
	}
}
