using System;
using System.Collections.Generic;

namespace MagTools.Trackers
{
	abstract class TrackedSnapShot<T>
	{
		protected class SnapShot
		{
			public readonly DateTime TimeStamp;
			public readonly T Value;

			public SnapShot(DateTime timeStamp, T value)
			{
				TimeStamp = timeStamp;
				Value = value;
			}
		}

		protected readonly List<SnapShot> SnapShots = new List<SnapShot>();

		/// <summary>
		/// This will either update an existing snapshot at the same TimeStamp, or add a new SnapShot.<para />
		/// If a new SnapShot is added, it will also try to prune older snap shots to conserve memory using the following format:<para />
		///   0-   1  Minutes: Every  1 Second<para />
		///   1- 180  Minutes: Every  5 Minutes<para />
		///	180-1440  Minutes: Every  1 Hour<para />
		///     1440+ Minutes: Every  1 Day<para />
		/// </summary>
		public void AddSnapShot(DateTime timeStamp, T value, bool prune = true)
		{
			for (int i = 0; i < SnapShots.Count; i++)
			{
				if (SnapShots[i].TimeStamp == timeStamp)
				{
					SnapShots[i] = new SnapShot(timeStamp, value);
					return;
				}
			}

			SnapShots.Add(new SnapShot(timeStamp, value));

			// We should reduce the history a bit here. Older times should have less resolution
			if (prune)
			{
				var snapShotsToKeep = new List<SnapShot>();
				snapShotsToKeep.Add(SnapShots[SnapShots.Count - 1]);

				var timeSpanIncrement = TimeSpan.FromSeconds(1);

				for (DateTime time = DateTime.Now ; time >= SnapShots[0].TimeStamp ; time -= timeSpanIncrement)
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
				for (int i = SnapShots.Count - 2 ; i > 0 ; i--)
				{
					if (!snapShotsToKeep.Contains(SnapShots[i]))
						SnapShots.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// This will get the SnapShot closest to the time.
		/// </summary>
		protected SnapShot GetSnapShotClosestToTime(DateTime time, SnapShot excludeSnapShot = null)
		{
			var closestPastTarget = SnapShots[SnapShots.Count - 1];

			for (int i = SnapShots.Count - 2; i >= 0; i--)
			{
				if (excludeSnapShot != null && SnapShots[i] == excludeSnapShot)
					continue;

				if (Math.Abs((time - SnapShots[i].TimeStamp).TotalMinutes) < Math.Abs((time - closestPastTarget.TimeStamp).TotalMinutes))
					closestPastTarget = SnapShots[i];
			}

			return closestPastTarget;
		}
	}
}
