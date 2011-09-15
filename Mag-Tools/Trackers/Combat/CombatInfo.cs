using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MagTools.Trackers.Combat
{
	class CombatInfo : ICombatInfo
	{
		Dictionary<DamageElement, CombatInfo> combatInfoByElement = new Dictionary<DamageElement, CombatInfo>();
		Dictionary<AttackType, CombatInfo> combatInfoByAttackType = new Dictionary<AttackType, CombatInfo>();

		public void AddFromCombatEventArgs(CombatEventArgs e)
		{
			PopulateData(this, e);


			if (!combatInfoByElement.ContainsKey(e.DamageElemenet))
				combatInfoByElement.Add(e.DamageElemenet, new CombatInfo());

			PopulateData(combatInfoByElement[e.DamageElemenet], e);

			if (!combatInfoByElement[e.DamageElemenet].combatInfoByAttackType.ContainsKey(e.AttackType))
				combatInfoByElement[e.DamageElemenet].combatInfoByAttackType.Add(e.AttackType, new CombatInfo());

			PopulateData(combatInfoByElement[e.DamageElemenet].combatInfoByAttackType[e.AttackType], e);


			if (!combatInfoByAttackType.ContainsKey(e.AttackType))
				combatInfoByAttackType.Add(e.AttackType, new CombatInfo());

			PopulateData(combatInfoByAttackType[e.AttackType], e);

			if (!combatInfoByAttackType[e.AttackType].combatInfoByElement.ContainsKey(e.DamageElemenet))
				combatInfoByAttackType[e.AttackType].combatInfoByElement.Add(e.DamageElemenet, new CombatInfo());

			PopulateData(combatInfoByAttackType[e.AttackType].combatInfoByElement[e.DamageElemenet], e);
		}

		private uint totalNonCritAttacks = 0;
		private uint nonCritAttackTotal = 0;

		private uint totalCritAttacks = 0;
		private uint critAttackTotal = 0;

		private static void PopulateData(CombatInfo combatInfo, CombatEventArgs e)
		{
			combatInfo.TotalAttacks++;

			combatInfo.TotalDamage += e.DamageAmount;

			if (e.IsFailedAttack)
				combatInfo.FailedAttacks++;

			if (e.IsCriticalHit)
				combatInfo.Crits++;

			if (e.IsKillingBlow)
				combatInfo.KillingBlows++;

			if (!e.IsFailedAttack)
			{
				if (!e.IsCriticalHit)
				{
					combatInfo.totalNonCritAttacks++;
					combatInfo.nonCritAttackTotal += (uint)e.DamageAmount;

					combatInfo.AverageNonCritAttack = (int)(combatInfo.nonCritAttackTotal / combatInfo.totalNonCritAttacks);

					if (e.DamageAmount > combatInfo.MaxNonCritAttack)
						combatInfo.MaxNonCritAttack = e.DamageAmount;
				}
				else
				{
					combatInfo.totalCritAttacks++;
					combatInfo.critAttackTotal += (uint)e.DamageAmount;

					combatInfo.AverageCritAttack = (int)(combatInfo.critAttackTotal / combatInfo.totalCritAttacks);

					if (e.DamageAmount > combatInfo.MaxCritAttack)
						combatInfo.MaxCritAttack = e.DamageAmount;
				}
			}
		}


		public int TotalAttacks { get; private set; }

		public int SucceededAttacks
		{
			get
			{
				return TotalAttacks - FailedAttacks;
			}
		}

		public float AttackSuccessPercent
		{
			get
			{
				return (SucceededAttacks / (float)TotalAttacks) * 100;
			}
		}

		public int FailedAttacks { get; private set; }


		public int AverageNonCritAttack { get; private set; }

		public int MaxNonCritAttack { get; private set; }


		public int Crits { get; private set; }

		public int AverageCritAttack { get; private set; }

		public int MaxCritAttack { get; private set; }


		public int KillingBlows { get; private set; }


		public int TotalDamage { get; private set; }


		public ICombatInfo this[DamageElement element]
		{
			get
			{
				if (combatInfoByElement.ContainsKey(element))
					return combatInfoByElement[element];

				return new CombatInfo();
			}
		}

		public ICombatInfo this[AttackType type]
		{
			get
			{
				if (combatInfoByAttackType.ContainsKey(type))
					return combatInfoByAttackType[type];

				return new CombatInfo();
			}
		}
	}
}
