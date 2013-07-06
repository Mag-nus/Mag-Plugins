using System.Collections.Generic;

using MagTools.Trackers.Combat.Standard;

namespace MagTools.Trackers.Combat
{
	class CombatInfo
	{
		public readonly string SourceName;
		public readonly string TargetName;

		public CombatInfo(string sourceName, string targetName)
		{
			SourceName = sourceName;
			TargetName = targetName;
		}

		public void AddFromCombatEventArgs(CombatEventArgs combatEventArgs)
		{
			if (combatEventArgs.IsKillingBlow)
				KillingBlows++;

			if (!DamageByAttackTypes.ContainsKey(combatEventArgs.AttackType))
				DamageByAttackTypes.Add(combatEventArgs.AttackType, new DamageByAttackType());

			DamageByAttackType damageByAttackType = DamageByAttackTypes[combatEventArgs.AttackType];

			if (!damageByAttackType.DamageByElements.ContainsKey(combatEventArgs.DamageElemenet))
				damageByAttackType.DamageByElements.Add(combatEventArgs.DamageElemenet, new DamageByAttackType.DamageByElement());

			DamageByAttackType.DamageByElement damageByElement = damageByAttackType.DamageByElements[combatEventArgs.DamageElemenet];

			damageByElement.TotalAttacks++;

			if (combatEventArgs.IsFailedAttack)
				damageByElement.FailedAttacks++;

			if (combatEventArgs.IsCriticalHit)
				damageByElement.Crits++;

			if (!combatEventArgs.IsCriticalHit)
			{
				damageByElement.TotalNormalDamage += combatEventArgs.DamageAmount;

				if (combatEventArgs.DamageAmount > damageByElement.MaxNormalDamage)
					damageByElement.MaxNormalDamage = combatEventArgs.DamageAmount;
			}
			else
			{
				damageByElement.TotalCritDamage += combatEventArgs.DamageAmount;

				if (combatEventArgs.DamageAmount > damageByElement.MaxCritDamage)
					damageByElement.MaxCritDamage = combatEventArgs.DamageAmount;
			}
		}


		public int KillingBlows;


		public class DamageByAttackType
		{
			public class DamageByElement
			{
				public int TotalAttacks;

				public int FailedAttacks;

				public int Crits;

				public int TotalNormalDamage;

				public int MaxNormalDamage;

				public int TotalCritDamage;

				public int MaxCritDamage;

				public int Damage
				{
					get
					{
						return TotalNormalDamage + TotalCritDamage;
					}
				}
			}

			public readonly Dictionary<DamageElement, DamageByElement> DamageByElements = new Dictionary<DamageElement, DamageByElement>();


			public int TotalAttacks
			{
				get
				{
					int value = 0;
					foreach (DamageByElement damageByElement in DamageByElements.Values) value += damageByElement.TotalAttacks;
					return value;
				}
			}

			public int FailedAttacks
			{
				get
				{
					int value = 0;
					foreach (DamageByElement damageByElement in DamageByElements.Values) value += damageByElement.FailedAttacks;
					return value;
				}
			}

			public int TotalNormalDamage
			{
				get
				{
					int value = 0;
					foreach (DamageByElement damageByElement in DamageByElements.Values)
						value += damageByElement.TotalNormalDamage;
					return value;
				}
			}

			public int MaxNormalDamage
			{
				get
				{
					int value = 0;
					foreach (DamageByElement damageByElement in DamageByElements.Values)
						if (value < damageByElement.MaxNormalDamage)
							value = damageByElement.MaxNormalDamage;
					return value;
				}
			}

			public int Crits
			{
				get
				{
					int value = 0;
					foreach (DamageByElement damageByElement in DamageByElements.Values) value += damageByElement.Crits;
					return value;
				}
			}

			public int TotalCritDamage
			{
				get
				{
					int value = 0;
					foreach (DamageByElement damageByElement in DamageByElements.Values)
						value += damageByElement.TotalCritDamage;
					return value;
				}
			}

			public int MaxCritDamage
			{
				get
				{
					int value = 0;
					foreach (DamageByElement damageByElement in DamageByElements.Values)
						if (value < damageByElement.MaxCritDamage)
							value = damageByElement.MaxCritDamage;
					return value;
				}
			}

			public int Damage
			{
				get
				{
					int value = 0;
					foreach (DamageByElement damageByElement in DamageByElements.Values) value += damageByElement.Damage;
					return value;
				}
			}
		}

		public readonly Dictionary<AttackType, DamageByAttackType> DamageByAttackTypes = new Dictionary<AttackType, DamageByAttackType>();


		public int TotalAttacks
		{
			get
			{
				int value = 0;
				foreach (DamageByAttackType damageByAttackType in DamageByAttackTypes.Values) value += damageByAttackType.TotalAttacks;
				return value;
			}
		}

		public int FailedAttacks
		{
			get
			{
				int value = 0;
				foreach (DamageByAttackType damageByAttackType in DamageByAttackTypes.Values) value += damageByAttackType.FailedAttacks;
				return value;
			}
		}

		public int TotalNormalDamage
		{
			get
			{
				int value = 0;
				foreach (DamageByAttackType damageByAttackType in DamageByAttackTypes.Values)
					value += damageByAttackType.TotalNormalDamage;
				return value;
			}
		}

		public int MaxNormalDamage
		{
			get
			{
				int value = 0;
				foreach (DamageByAttackType damageByAttackType in DamageByAttackTypes.Values)
					if (value < damageByAttackType.MaxNormalDamage)
						value = damageByAttackType.MaxNormalDamage;
				return value;
			}
		}

		public int Crits
		{
			get
			{
				int value = 0;
				foreach (DamageByAttackType damageByAttackType in DamageByAttackTypes.Values) value += damageByAttackType.Crits;
				return value;
			}
		}

		public int TotalCritDamage
		{
			get
			{
				int value = 0;
				foreach (DamageByAttackType damageByAttackType in DamageByAttackTypes.Values)
					value += damageByAttackType.TotalCritDamage;
				return value;
			}
		}

		public int MaxCritDamage
		{
			get
			{
				int value = 0;
				foreach (DamageByAttackType damageByAttackType in DamageByAttackTypes.Values)
					if (value < damageByAttackType.MaxCritDamage)
						value = damageByAttackType.MaxCritDamage;
				return value;
			}
		}

		public int Damage
		{
			get
			{
				int value = 0;
				foreach (DamageByAttackType damageByAttackType in DamageByAttackTypes.Values) value += damageByAttackType.Damage;
				return value;
			}
		}
	}
}
