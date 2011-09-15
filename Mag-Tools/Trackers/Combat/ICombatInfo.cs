using System;

namespace MagTools.Trackers.Combat
{
	interface ICombatInfo
	{
		int TotalAttacks { get; }

		int SucceededAttacks { get; }

		float AttackSuccessPercent { get; }

		int FailedAttacks { get; }


		int AverageNonCritAttack { get; }

		int MaxNonCritAttack { get; }


		int Crits { get; }

		int AverageCritAttack { get; }

		int MaxCritAttack { get; }


		int KillingBlows { get; }


		int TotalDamage { get; }


		ICombatInfo this[DamageElement element] { get; }

		ICombatInfo this[AttackType type] { get; }
	}
}
