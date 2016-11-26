using System;

namespace MagTools.Trackers
{
	class ValueSnapShotGroup : SnapShotGroup<int>
	{
		public ValueSnapShotGroup(int minutesToRetain) : base(minutesToRetain)
		{
		}

		/// <summary>
		/// For pruning to work properly and efficiently, it is assumed that you are adding items in order of oldest to newest.<para />
		/// Use a minutesToRetain greater than 0 to trim older SnapShots.
		/// </summary>
		public void AddSnapShot(DateTime timeStamp, int value, int minutesToRetain = 0)
		{
			for (int i = 0; i < SnapShots.Count; i++)
			{
				if (SnapShots[i].TimeStamp == timeStamp)
				{
					SnapShots[i] = new SnapShot<int>(timeStamp, SnapShots[i].Value + value);
					return;
				}
			}

			SnapShots.Add(new SnapShot<int>(timeStamp, value));

			if (minutesToRetain > 0)
			{
				for (int i = 0; i < SnapShots.Count; i++)
				{
					if (DateTime.UtcNow - SnapShots[i].TimeStamp <= TimeSpan.FromMinutes(minutesToRetain))
						break;

					SnapShots.RemoveAt(i);
					i--;
				}
			}
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

			var closestPastTarget = GetSnapShotClosestToTime(DateTime.UtcNow - historyPeriod);

			return (LastKnownValue - closestPastTarget.Value) * (usagePeriod.TotalMinutes / (DateTime.UtcNow - closestPastTarget.TimeStamp).TotalMinutes);
		}

		/// <summary>
		/// Returns the value total over the history of period.
		/// </summary>
		public int GetValueTotal(TimeSpan historyPeriod, out TimeSpan actualHistoryPeriodUsed)
		{
			actualHistoryPeriodUsed = TimeSpan.Zero;

			int total = 0;

			for (int i = SnapShots.Count - 1 ; i >= 0 ; i--)
			{
				if (DateTime.UtcNow - SnapShots[i].TimeStamp > historyPeriod)
					break;

				total += SnapShots[i].Value;
				actualHistoryPeriodUsed = DateTime.UtcNow - SnapShots[i].TimeStamp;
			}

			return total;
		}

		/// <summary>
		/// This will return the estimated time to depletion (Value of 0) given the history recorded over period.
		/// </summary>
		public TimeSpan GetTimeToDepletion(TimeSpan period)
		{
			if (SnapShots.Count == 1 || LastKnownValue == 0)
				return TimeSpan.Zero;

			var closestPastTarget = GetSnapShotClosestToTime(DateTime.UtcNow - period);

			if (LastKnownValue >= closestPastTarget.Value)
				return TimeSpan.MaxValue;

			return TimeSpan.FromSeconds(LastKnownValue / ((closestPastTarget.Value - LastKnownValue) / (DateTime.UtcNow - closestPastTarget.TimeStamp).TotalSeconds));
		}
	}
}
