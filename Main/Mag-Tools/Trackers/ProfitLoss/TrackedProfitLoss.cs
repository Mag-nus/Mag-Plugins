using System;
using System.Collections.Generic;

namespace MagTools.Trackers.ProfitLoss
{
	class TrackedProfitLoss
	{
		public readonly string Name;

		class SnapShot
		{
			public readonly DateTime TimeStamp;
			public readonly int Value;

			public SnapShot(DateTime timeStamp, int value)
			{
				TimeStamp = timeStamp;
				Value = value;
			}
		}
		private readonly List<SnapShot> snapShots = new List<SnapShot>();

		public TrackedProfitLoss(string name)
		{
			Name = name;
		}

		public TrackedProfitLoss(string name, DateTime timeAtValue, int value) : this(name)
		{
			snapShots.Add(new SnapShot(timeAtValue, value));
		}

		public void AddValue(DateTime timeAtCount, int value)
		{
			for (int i = 0 ; i< snapShots.Count ; i++)
			{
				if (snapShots[i].TimeStamp == timeAtCount)
				{
					snapShots[i] = new SnapShot(timeAtCount, value);
					return;
				}
			}

			snapShots.Add(new SnapShot(timeAtCount, value));

			// We should reduce the history a bit here. Older times should have less resolution
		}

		public int LastKnownValue
		{
			get
			{
				if (snapShots.Count > 0)
					return snapShots[snapShots.Count - 1].Value;

				return 0;
			}
		}

		/// <summary>
		/// Returns the value difference over the history of period.
		/// </summary>
		public double GetValueDifference(TimeSpan historyPeriod, TimeSpan usagePeriod)
		{
			if (snapShots.Count == 1)
				return 0;

			var closestPastTarget = GetSnapShotClosestToTime(DateTime.Now - historyPeriod);

			return (LastKnownValue - closestPastTarget.Value) * (usagePeriod.TotalMinutes / (DateTime.Now - closestPastTarget.TimeStamp).TotalMinutes);
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
