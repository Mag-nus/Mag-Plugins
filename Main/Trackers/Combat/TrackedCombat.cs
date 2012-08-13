using System;

namespace MagTools.Trackers.Combat
{
	/// <summary>
	/// This represents total tracked combat, sorted by Melee/Missle, and Magic
	/// </summary>
	class TrackedCombat : ITrackedCombat
	{
		private CombatInfo unknown = new CombatInfo();
		private CombatInfo PlayerReceived = new CombatInfo();
		private CombatInfo PlayerIniated = new CombatInfo();

		public void AddFromCombatEventArgs(CombatEventArgs e)
		{
			if (e.AttackDirection == AttackDirection.PlayerReceived)
				PlayerReceived.AddFromCombatEventArgs(e);
			else if (e.AttackDirection == AttackDirection.PlayerInitiated)
				PlayerIniated.AddFromCombatEventArgs(e);
			else
				unknown.AddFromCombatEventArgs(e);
		}

		public ICombatInfo this[AttackDirection direction]   // Indexer declaration
		{
			get
			{
				if (direction == AttackDirection.PlayerReceived)
					return PlayerReceived;
				if (direction == AttackDirection.PlayerInitiated)
					return PlayerIniated;

				if (direction != AttackDirection.Unknown)
					throw new NotImplementedException("Attack direction not implemented for: " + direction);

				return unknown;
			}
		}
	}
}
