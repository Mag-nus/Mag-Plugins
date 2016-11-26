
namespace MagTools.Trackers.ProfitLoss
{
	class TrackedProfitLoss : ValueSnapShotGroup
	{
		public readonly string Name;

		public TrackedProfitLoss(string name, int minutesToRetain) : base(minutesToRetain)
		{
			Name = name;
		}
	}
}
