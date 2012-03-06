using System;

namespace MagTools.Trackers.Combat
{
	public class CombatEventArgs : EventArgs
	{
		public string MonsterName { get; private set; }

		public AttackDirection AttackDirection { get; private set; }
		public AttackType AttackType { get; private set; }
		public DamageElement DamageElemenet { get; private set; }

		public bool IsFailedAttack { get; private set; }
		public bool IsKillingBlow { get; private set; }
		public bool IsCriticalHit { get; private set; }

		public int DamageAmount { get; private set; }

		public CombatEventArgs(string monsterName, AttackDirection attackDirection, AttackType attackType, DamageElement damageElemenet, bool isFailedAttack, bool isKillingBlow, bool isCriticalHit, int damageAmount)
		{
			this.MonsterName = monsterName;

			this.AttackDirection = attackDirection;
			this.AttackType = attackType;
			this.DamageElemenet = damageElemenet;

			this.IsFailedAttack = isFailedAttack;
			this.IsKillingBlow = isKillingBlow;
			this.IsCriticalHit = isCriticalHit;

			this.DamageAmount = damageAmount;
		}
	}
}
