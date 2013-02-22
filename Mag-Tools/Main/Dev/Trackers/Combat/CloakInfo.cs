using MagTools.Trackers.Combat.Cloaks;

namespace MagTools.Trackers.Combat
{
	class CloakInfo
	{
		public readonly string SourceName;
		public readonly string TargetName;

		public CloakInfo(string sourceName, string targetName)
		{
			SourceName = sourceName;
			TargetName = targetName;
		}

		public void AddFromSurgeEventArgs(SurgeEventArgs surgeEventArgs)
		{
			TotalSurges++;
		}

		public int TotalSurges;
	}
}
