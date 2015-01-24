using System;
using System.Collections.Generic;

namespace MagTools.Trackers
{
	class ValueSnapShotGroup : SnapShotGroup<int>
	{
		public enum CombineMethod
		{
			None,

			/// <summary>
			/// This will either update an existing snapshot at the same TimeStamp, or add a new SnapShot.<para />
			/// If a new SnapShot is added, it will also try to prune older snap shots to conserve memory using the following format:<para />
			///   0-   1  Minutes: Every  1 Second<para />
			///   1- 180  Minutes: Every  5 Minutes<para />
			///	180-1440  Minutes: Every  1 Hour<para />
			///     1440+ Minutes: Every  1 Day<para />
			/// </summary>
			DecreaseResolution,
		}

		/// <summary>
		/// For pruning to work properly and efficiently, it is assumed that you are adding items in order of oldest to newest.<para />
		/// Use a minutesToRetain greater than 0 to trim older SnapShots.
		/// </summary>
		public void AddSnapShot(DateTime timeStamp, int value, CombineMethod combineMethod, int minutesToRetain = 0)
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
					if (DateTime.Now - SnapShots[i].TimeStamp <= TimeSpan.FromMinutes(minutesToRetain))
						break;

					SnapShots.RemoveAt(i);
					i--;
				}
			}

			// We should reduce the history a bit here. Older times should have less resolution
			if (combineMethod == CombineMethod.DecreaseResolution)
			{
				var snapShotsToKeep = new List<SnapShot<int>>();
				snapShotsToKeep.Add(SnapShots[SnapShots.Count - 1]);

				var timeSpanIncrement = TimeSpan.FromSeconds(1);

				for (DateTime time = DateTime.Now; time >= SnapShots[0].TimeStamp; time -= timeSpanIncrement)
				{
					var closest = GetSnapShotClosestToTime(time, snapShotsToKeep[snapShotsToKeep.Count - 1]);
					snapShotsToKeep.Add(closest);

					var timeDifference = DateTime.Now.Subtract(time);

					if (timeDifference > TimeSpan.FromMinutes(1))
					{
						if (timeDifference <= TimeSpan.FromMinutes(180)) timeSpanIncrement = TimeSpan.FromMinutes(5);
						else if (timeDifference <= TimeSpan.FromMinutes(1440)) timeSpanIncrement = TimeSpan.FromHours(1);
						else timeSpanIncrement = TimeSpan.FromDays(1);
					}
				}

				// We'll never prune the most recent or oldest SnapShot
				for (int i = SnapShots.Count - 2; i > 0; i--)
				{
					if (!snapShotsToKeep.Contains(SnapShots[i]))
					{
						var closest = GetSnapShotClosestToTime(SnapShots[i].TimeStamp, SnapShots[i]);

						for (int j = 0 ; j < SnapShots.Count ; j++)
						{
							if (SnapShots[j] == closest)
							{
								SnapShots[j] = new SnapShot<int>(closest.TimeStamp, closest.Value + SnapShots[i].Value);
								break;
							}
						}

						SnapShots.RemoveAt(i);
					}
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

			var closestPastTarget = GetSnapShotClosestToTime(DateTime.Now - historyPeriod);

			return (LastKnownValue - closestPastTarget.Value) * (usagePeriod.TotalMinutes / (DateTime.Now - closestPastTarget.TimeStamp).TotalMinutes);
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
				if (DateTime.Now - SnapShots[i].TimeStamp > historyPeriod)
					break;

				total += SnapShots[i].Value;
				actualHistoryPeriodUsed = DateTime.Now - SnapShots[i].TimeStamp;
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

			var closestPastTarget = GetSnapShotClosestToTime(DateTime.Now - period);

			if (LastKnownValue >= closestPastTarget.Value)
				return TimeSpan.MaxValue;

			return TimeSpan.FromSeconds(LastKnownValue / ((closestPastTarget.Value - LastKnownValue) / (DateTime.Now - closestPastTarget.TimeStamp).TotalSeconds));
		}
	}
}
