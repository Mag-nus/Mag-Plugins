using System;

namespace MagTools.Trackers.Player
{
	public class TrackedPlayer
	{
		public readonly string Name;

		public DateTime LastSeen;

		public int LandBlock;

		public double LocationX;
		public double LocationY;
		public double LocationZ;

		public int Id;

		public PlayerType PlayerType;

		public TrackedPlayer(string name)
		{
			Name = name;
		}

		public TrackedPlayer(string name, DateTime lastSeen, int landBlock, double locationX, double locaitonY, double locationZ, int id, PlayerType playerType = PlayerType.None) : this(name)
		{
			LastSeen = lastSeen;

			LandBlock = landBlock;

			LocationX = locationX;
			LocationY = locaitonY;
			LocationZ = locationZ;

			Id = id;

			PlayerType = playerType;
		}
	}
}
