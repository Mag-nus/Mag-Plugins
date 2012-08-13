using System;

namespace MagTools.Trackers.Combat.Cloaks
{
	public class SurgeEventArgs : EventArgs
	{
		public string SourceName { get; private set; }
		public string TargetName { get; private set; }

		public SurgeType SurgeType { get; private set; }

		public SurgeEventArgs(string sourceName, string targetName, SurgeType surgeType)
		{
			SourceName = sourceName;
			TargetName = targetName;

			SurgeType = surgeType;
		}
	}
}
