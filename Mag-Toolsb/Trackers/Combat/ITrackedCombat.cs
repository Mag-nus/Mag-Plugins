using System;

namespace MagTools.Trackers.Combat
{
	interface ITrackedCombat
	{
		ICombatInfo this[AttackDirection direction] { get; }
	}
}
