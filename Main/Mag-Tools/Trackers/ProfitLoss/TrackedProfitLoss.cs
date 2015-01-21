using System;

namespace MagTools.Trackers.ProfitLoss
{
	class TrackedProfitLoss : TrackedSnapShot<int>
	{
		public readonly string Name;

		public TrackedProfitLoss(string name)
		{
			Name = name;
		}

		public TrackedProfitLoss(string name, DateTime timeAtValue, int value) : this(name)
		{
			SnapShots.Add(new SnapShot(timeAtValue, value));
		}

		public int LastKnownValue
		{
			get
			{
				if (SnapShots.Count > 0)
					return SnapShots[SnapShots.Count - 1].Value;

				return 0;
			}
		}

		/// <summary>
		/// Returns the value difference over the history of period.
		/// </summary>
		public double GetValueDifference(TimeSpan historyPeriod, TimeSpan usagePeriod)
		{
			if (SnapShots.Count == 1)
				return 0;

			var closestPastTarget = GetSnapShotClosestToTime(DateTime.Now - historyPeriod);

			return (LastKnownValue - closestPastTarget.Value) * (usagePeriod.TotalMinutes / (DateTime.Now - closestPastTarget.TimeStamp).TotalMinutes);
		}
	}
}
