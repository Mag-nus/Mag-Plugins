using System;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class OneTouchHeal
	{
		public void Start()
		{
			int healthPointsFromMax = CoreManager.Current.Actions.Vital[VitalType.MaximumHealth] - CoreManager.Current.Actions.Vital[VitalType.CurrentHealth];

			if (healthPointsFromMax <= 0)
				return;

			// Try to use a healing kit
			if (CoreManager.Current.CharacterFilter.Skills[CharFilterSkillType.Healing].Training >= TrainingType.Trained)
			{
				// Find the healing kit with the least uses left.
				WorldObject kitWithLeastUses = null;

				// Try to find a kit that has id data
				foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (obj.ObjectClass == ObjectClass.HealingKit && obj.HasIdData)
					{
						if (kitWithLeastUses == null || obj.Values(LongValueKey.UsesRemaining) < kitWithLeastUses.Values(LongValueKey.UsesRemaining))
							kitWithLeastUses = obj;
					}
				}

				// No kits found with id data, lets just use the first kit we find
				if (kitWithLeastUses == null)
				{
					foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetInventory())
					{
						if (obj.ObjectClass == ObjectClass.HealingKit)
						{
							if (!obj.HasIdData)
								CoreManager.Current.Actions.RequestId(obj.Id);

							kitWithLeastUses = obj;

							break;
						}
					}
				}

				if (kitWithLeastUses != null)
				{
					int healingSkillRequired;

					if (CoreManager.Current.Actions.CombatMode == CombatState.Peace)
						healingSkillRequired = 2 * healthPointsFromMax;
					else
						healingSkillRequired = (int)Math.Ceiling(2.6 * healthPointsFromMax);

					int healingSkillWithKitBonus = CoreManager.Current.Actions.Skill[SkillType.CurrentHealing] + kitWithLeastUses.Values(LongValueKey.AffectsVitalAmt);

					// healingSkillRequired == healingSkillWithKitBonus is ~50%
					if (healingSkillRequired < healingSkillWithKitBonus || !DoWeHaveFood())
					{
						CoreManager.Current.Actions.ApplyItem(kitWithLeastUses.Id, CoreManager.Current.CharacterFilter.Id);

						return;
					}
				}
			}

			// Try to use a pot
			if (DoWeHaveFood())
			{
				if (UseFood())
					return;
			}

			// Are we in magic mode? Maybe we can cast a heal spell
		}

		private bool DoWeHaveFood()
		{
			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (obj.ObjectClass == ObjectClass.Food)
				{
					if (obj.Name.Contains("Heal") || obj.Name.Contains("Meat"))
						return true;
				}
			}

			return false;
		}

		private bool UseFood()
		{
			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (obj.ObjectClass == ObjectClass.Food)
				{
					if (obj.Name.Contains("Heal") || obj.Name.Contains("Meat"))
					{
						CoreManager.Current.Actions.UseItem(obj.Id, 0);
						return true;
					}
				}
			}

			return false;
		}
	}
}
