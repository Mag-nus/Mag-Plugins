using System;
using System.Collections.Generic;

namespace MagTools.Trackers
{
	abstract class SnapShotGroup<T>
	{
		protected readonly List<SnapShot<T>> SnapShots = new List<SnapShot<T>>();

		private readonly int minutesToRetain;

		protected SnapShotGroup(int minutesToRetain)
		{
			this.minutesToRetain = minutesToRetain;
		}

		/// <summary>
		/// For pruning to work properly and efficiently, it is assumed that you are adding items in order of oldest to newest.<para />
		/// </summary>
		public void AddSnapShot(DateTime timeStamp, T value)
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
					if (DateTime.UtcNow - SnapShots[i].TimeStamp <= TimeSpan.FromMinutes(minutesToRetain))
						break;

					SnapShots.RemoveAt(i);
					i--;
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
