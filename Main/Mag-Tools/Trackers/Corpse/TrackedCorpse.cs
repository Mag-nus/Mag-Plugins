using System;

namespace MagTools.Trackers.Corpse
{
	class TrackedCorpse
	{
		public readonly int Id;

		public readonly DateTime TimeStamp;

		public readonly double LocationX;
		public readonly double LocationY;
		public readonly double LocationZ;

		public readonly string Description;

		public TrackedCorpse(int id, DateTime timeStamp, double locationX, double locaitonY, double locationZ, string description)
		{
			Id = id;

			TimeStamp = timeStamp;

			LocationX = locationX;
			LocationY = locaitonY;
			LocationZ = locationZ;

			Description = description;
		}
	}
}
