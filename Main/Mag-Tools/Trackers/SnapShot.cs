using System;

namespace MagTools.Trackers
{
	class SnapShot<T>
	{
		public readonly DateTime TimeStamp;
		public readonly T Value;

		public SnapShot(DateTime timeStamp, T value)
		{
			TimeStamp = timeStamp;
			Value = value;
		}
	}
}
