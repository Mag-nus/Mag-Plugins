using System;

namespace MagTools.Trackers.Corpse
{
	public class TrackedCorpse
	{
		public readonly int Id;

		public DateTime TimeStamp;

		public int LandBlock;

		public double LocationX;
		public double LocationY;
		public double LocationZ;

		public string Description;

		public bool Opened;

		public TrackedCorpse(int id)
		{
			Id = id;
		}

		public TrackedCorpse(int id, DateTime timeStamp, int landBlock, double locationX, double locaitonY, double locationZ, string description, bool opened = false) : this(id)
		{
			TimeStamp = timeStamp;

			LandBlock = landBlock;

			LocationX = locationX;
			LocationY = locaitonY;
			LocationZ = locationZ;

			Description = description;

			Opened = opened;
		}
	}
}
