using System;

namespace MagTools.Trackers.Combat.Standard
{
	public class CombatEventArgs : EventArgs
	{
		public string SourceName { get; private set; }
		public string TargetName { get; private set; }

		public AttackType AttackType { get; private set; }
		public DamageElement DamageElemenet { get; private set; }

		public bool IsFailedAttack { get; private set; }
		public bool IsCriticalHit { get; private set; }
		public bool IsOverpower { get; private set; }
		public bool IsKillingBlow { get; private set; }

		public int DamageAmount { get; private set; }

		public CombatEventArgs(string sourceName, string targetName, AttackType attackType, DamageElement damageElemenet, bool isFailedAttack, bool isCriticalHit, bool isOverpower, bool isKillingBlow, int damageAmount)
		{
			SourceName = sourceName;
			TargetName = targetName;

			AttackType = attackType;
			DamageElemenet = damageElemenet;

			IsFailedAttack = isFailedAttack;
			IsCriticalHit = isCriticalHit;
			IsOverpower = isOverpower;
			IsKillingBlow = isKillingBlow;

			DamageAmount = damageAmount;
		}
	}
}
