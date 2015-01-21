using System;
using System.Collections.Generic;

using Decal.Adapter.Wrappers;

namespace MagTools.Trackers.Inventory
{
	class TrackedInventory
	{
		public readonly string Name;
		public readonly ObjectClass ObjectClass;
		public readonly int Icon;
		public readonly int ItemValue;

		class SnapShot
		{
			public readonly DateTime TimeStamp;
			public readonly int Count;

			public SnapShot(DateTime timeStamp, int count)
			{
				TimeStamp = timeStamp;
				Count = count;
			}
		}
		private readonly List<SnapShot> snapShots = new List<SnapShot>();

		public TrackedInventory(string name, ObjectClass objectClass, int icon, int itemValue)
		{
			Name = name;
			ObjectClass = objectClass;
			Icon = icon;
			ItemValue = itemValue;
		}

		public TrackedInventory(string name, ObjectClass objectClass, int icon, int itemValue, DateTime timeAtCount, int count) : this(name, objectClass, icon, itemValue)
		{
			snapShots.Add(new SnapShot(timeAtCount, count));
		}

		public void AddCount(DateTime timeAtCount, int count)
		{
			for (int i = 0 ; i< snapShots.Count ; i++)
			{
				if (snapShots[i].TimeStamp == timeAtCount)
				{
					snapShots[i] = new SnapShot(timeAtCount, count);
					return;
				}
			}

			snapShots.Add(new SnapShot(timeAtCount, count));

			// We should reduce the history a bit here. Older times should have less resolution
		}

		public int LastKnownCount
		{
			get
			{
				if (snapShots.Count > 0)
					return snapShots[snapShots.Count - 1].Count;

				return 0;
			}
		}

		/// <summary>
		/// Returns the item count difference over the history of period.
		/// </summary>
		public double GetItemsCountDifference(TimeSpan historyPeriod, TimeSpan usagePeriod)
		{
			if (snapShots.Count == 1)
				return 0;

			var closestPastTarget = GetSnapShotClosestToTime(DateTime.Now - historyPeriod);

			return (LastKnownCount - closestPastTarget.Count) * (usagePeriod.TotalMinutes / (DateTime.Now - closestPastTarget.TimeStamp).TotalMinutes);
		}

		/// <summary>
		/// This will return the estimated time to depletion given the history recorded over period.
		/// </summary>
		public TimeSpan GetTimeToDepletion(TimeSpan period)
		{
			if (snapShots.Count == 1 || LastKnownCount == 0)
				return TimeSpan.Zero;

			var closestPastTarget = GetSnapShotClosestToTime(DateTime.Now - period);

			if (LastKnownCount >= closestPastTarget.Count)
				return TimeSpan.MaxValue;

			return TimeSpan.FromSeconds(LastKnownCount / ((closestPastTarget.Count - LastKnownCount) / (DateTime.Now - closestPastTarget.TimeStamp).TotalSeconds));
		}

		private SnapShot GetSnapShotClosestToTime(DateTime time)
		{
			SnapShot closestPastTarget = snapShots[snapShots.Count - 1];

			for (int i = snapShots.Count - 2; i >= 0; i--)
			{
				if (Math.Abs((time - snapShots[i].TimeStamp).TotalMinutes) < Math.Abs((time - closestPastTarget.TimeStamp).TotalMinutes))
					closestPastTarget = snapShots[i];
			}

			return closestPastTarget;
		}
	}
}
