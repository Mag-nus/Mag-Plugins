using System;

namespace MagTools.Trackers.Player
{
	[Flags]
	public enum PlayerType
	{
		None				= 0x00,

		NonPlayerKiller		= 0x01,
		PlayerKillerLight	= 0x02,
		PlayerKiller		= 0x04,
	}
}
