using System;

namespace MagTools.Trackers.Combat
{
	public interface ICombatTracker
	{
		event Action<CombatEventArgs> CombatEvent;
	}
}
