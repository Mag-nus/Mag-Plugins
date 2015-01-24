using System;
using System.Collections.Generic;

namespace MagTools.Trackers
{
	abstract class SnapShotGroup<T>
	{
		protected readonly List<SnapShot<T>> SnapShots = new List<SnapShot<T>>();

		public enum PruneMethod
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
		public void AddSnapShot(DateTime timeStamp, T value, PruneMethod pruneMethod, int minutesToRetain = 0)
		{
			for (int i = 0; i < SnapShots.Count; i++)
			{
				if (SnapShots[i].TimeStamp == timeStamp)
				{
					SnapShots[i] = new SnapShot<T>(timeStamp, value);
					return;
				}
			}

			SnapShots.Add(new SnapShot<T>(timeStamp, value));

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
			if (pruneMethod == PruneMethod.DecreaseResolution)
			{
				var snapShotsToKeep = new List<SnapShot<T>>();
				snapShotsToKeep.Add(SnapShots[SnapShots.Count - 1]);

				var timeSpanIncrement = TimeSpan.FromSeconds(1);

				for (DateTime time = DateTime.Now ; time >= SnapShots[0].TimeStamp ; time -= timeSpanIncrement)
				{
					var closest = GetSnapShotClosestToTime(time, snapShotsToKeep[snapShotsToKeep.Count - 1]);
					if (closest == null)
						continue;
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
				for (int i = SnapShots.Count - 2 ; i > 0 ; i--)
				{
					if (!snapShotsToKeep.Contains(SnapShots[i]))
						SnapShots.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// This will get the SnapShot closest to the time.<para />
		/// This function can return null if no SnapShots are present, or the only one present is excludeSnapShot.
		/// </summary>
		protected SnapShot<T> GetSnapShotClosestToTime(DateTime time, SnapShot<T> excludeSnapShot = null)
		{
			SnapShot<T> closestPastTarget = null;

			for (int i = SnapShots.Count - 1; i >= 0; i--)
			{
				if (excludeSnapShot != null && SnapShots[i] == excludeSnapShot)
					continue;

				if (closestPastTarget == null || Math.Abs((time - SnapShots[i].TimeStamp).TotalMinutes) < Math.Abs((time - closestPastTarget.TimeStamp).TotalMinutes))
					closestPastTarget = SnapShots[i];
			}

			return closestPastTarget;
		}
	}
}
