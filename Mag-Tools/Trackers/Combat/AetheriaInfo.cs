using MagTools.Trackers.Combat.Aetheria;

namespace MagTools.Trackers.Combat
{
	class AetheriaInfo
	{
		public readonly string SourceName;
		public readonly string TargetName;

		public AetheriaInfo(string sourceName, string targetName)
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
