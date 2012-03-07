using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Text;
using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace Mag_LootLogger
{
	public class Class1 : PluginBase
	{
		protected override void Startup()
		{
			Core.ContainerOpened += new EventHandler<ContainerOpenedEventArgs>(Core_ContainerOpened);
		}

		protected override void Shutdown()
		{
			Core.ContainerOpened -= new EventHandler<ContainerOpenedEventArgs>(Core_ContainerOpened);
		}

		void Core_ContainerOpened(object sender, ContainerOpenedEventArgs e)
		{
			try
			{
				WorldObject container = CoreManager.Current.WorldFilter[e.ItemGuid];

				if (container == null)
					return;

				// Do not loot housing chests
				if (container.Name == "Storage")
					return;

				if (container.ObjectClass == ObjectClass.Corpse)
					return;

				// Only loot chests and vaults, etc...
				if (container.Name.Contains("Chest") || container.Name.Contains("Vault") || container.Name.Contains("Reliquary"))
				{
					Start();
					return;
				}
			}
			catch { }
		}

		public bool IsRunning { get; private set; }

		readonly Collection<int> idsRequested = new Collection<int>();

		readonly Collection<int> itemsLogged = new Collection<int>();

		void Start()
		{
			if (IsRunning)
				return;

			idsRequested.Clear();

			CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);

			IsRunning = true;
		}

		void Stop()
		{
			if (!IsRunning)
				return;

			CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);

			IsRunning = false;
		}

		DateTime lastThought = DateTime.MinValue;

		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (DateTime.Now - lastThought < TimeSpan.FromMilliseconds(100))
					return;

				lastThought = DateTime.Now;

				Think();
			}
			catch { }
		}

		void Think()
		{
			if (CoreManager.Current.Actions.OpenedContainer == 0)
			{
				Stop();
				return;
			}

			// Sometimes it takes a bit before we actually get the item information for the items inside the container
			if (CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.Actions.OpenedContainer).Count == 0)
				return;

			bool stillWaiting = false;

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.Actions.OpenedContainer))
			{
				if (wo.ObjectClass == ObjectClass.MeleeWeapon || wo.ObjectClass == ObjectClass.MissileWeapon || wo.ObjectClass == ObjectClass.WandStaffOrb)
				{
					if (!wo.HasIdData)
					{
						if (!idsRequested.Contains(wo.Id))
						{
							idsRequested.Add(wo.Id);
							CoreManager.Current.Actions.RequestId(wo.Id);
						}

						stillWaiting = true;
					}
					else
						LogItem(wo);
				}
			}

			if (!stillWaiting)
			{
				CoreManager.Current.Actions.AddChatText("<{Mag-LootLogger}>: All items logged.", 5, 1);
				Stop();
			}
		}

		private void LogItem(WorldObject item)
		{
			if (itemsLogged.Contains(item.Id))
				return;

			itemsLogged.Add(item.Id);

			DirectoryInfo pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-LootLogger");

			if (!pluginPersonalFolder.Exists)
				pluginPersonalFolder.Create();

			FileInfo logFile = new FileInfo(pluginPersonalFolder.FullName + @"\Loot.csv");

			if (!logFile.Exists)
			{
				using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
				{
					writer.WriteLine("Id,Name,ObjectClass,EquipSkill,MasteryBonus,DamageType,Variance,MaxDamage,ElementalDmgBonus,DamageBonus,ElementalDamageVersusMonsters,AttackBonus,MeleeDefenseBonus,MagicDBonus,MissileDBonus,ManaCBonus,BuffedMaxDamage,BuffedElementalDmgBonus,BuffedDamageBonus,BuffedElementalDamageVersusMonsters,BuffedAttackBonus,BuffedMeleeDefenseBonus,BuffedManaCBonus,WieldReqValue,Work,Value,Burden");

					writer.Close();
				}
			}

			using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
			{
				StringBuilder output = new StringBuilder();

				output.Append(item.Id + ",");
				output.Append(item.Name + ",");
				output.Append(item.ObjectClass.ToString() + ",");

				output.Append((item.Values(LongValueKey.EquipSkill) > 0 ? Util.GetSkillNameById(item.Values(LongValueKey.EquipSkill)) : String.Empty) + ",");
				output.Append((item.Values((LongValueKey)353) > 0 ? Util.GetMasteryNameById(item.Values((LongValueKey)353)) : String.Empty) + ",");

				if (item.Values(LongValueKey.DamageType) > 0)
				{
					if ((item.Values(LongValueKey.DamageType) & 1) == 1) output.Append("Slash");
					if ((item.Values(LongValueKey.DamageType) & 2) == 2) output.Append("Pierce");
					if ((item.Values(LongValueKey.DamageType) & 4) == 4) output.Append("Bludge");
					if ((item.Values(LongValueKey.DamageType) & 8) == 8) output.Append("Cold");
					if ((item.Values(LongValueKey.DamageType) & 16) == 16) output.Append("Fire");
					if ((item.Values(LongValueKey.DamageType) & 32) == 32) output.Append("Acid");
					if ((item.Values(LongValueKey.DamageType) & 64) == 64) output.Append("Electrical");
				}
				else if (item.Values(LongValueKey.WandElemDmgType) > 0)
				{
					if ((item.Values(LongValueKey.WandElemDmgType) & 1) == 1) output.Append("Slash");
					if ((item.Values(LongValueKey.WandElemDmgType) & 2) == 2) output.Append("Pierce");
					if ((item.Values(LongValueKey.WandElemDmgType) & 4) == 4) output.Append("Bludge");
					if ((item.Values(LongValueKey.WandElemDmgType) & 8) == 8) output.Append("Cold");
					if ((item.Values(LongValueKey.WandElemDmgType) & 16) == 16) output.Append("Fire");
					if ((item.Values(LongValueKey.WandElemDmgType) & 32) == 32) output.Append("Acid");
					if ((item.Values(LongValueKey.WandElemDmgType) & 64) == 64) output.Append("Electrical");
				}
				output.Append(",");

				output.Append((item.Values(DoubleValueKey.Variance) > 0 ? item.Values(DoubleValueKey.Variance).ToString("N3") : String.Empty) + ",");
				output.Append((item.Values(LongValueKey.MaxDamage) > 0 ? item.Values(LongValueKey.MaxDamage).ToString() : String.Empty) + ",");
				output.Append((item.Values(LongValueKey.ElementalDmgBonus, 0) != 0 ? item.Values(LongValueKey.ElementalDmgBonus).ToString() : String.Empty) + ",");
				output.Append((item.Values(DoubleValueKey.DamageBonus, 1) != 1 ? Math.Round(((item.Values(DoubleValueKey.DamageBonus) - 1) * 100)).ToString() : String.Empty) + ",");
				output.Append((item.Values(DoubleValueKey.ElementalDamageVersusMonsters, 1) != 1 ? Math.Round(((item.Values(DoubleValueKey.ElementalDamageVersusMonsters) - 1) * 100)).ToString() : String.Empty) + ",");
				output.Append((item.Values(DoubleValueKey.AttackBonus, 1) != 1 ? Math.Round(((item.Values(DoubleValueKey.AttackBonus) - 1) * 100)).ToString() : String.Empty) + ",");
				output.Append((item.Values(DoubleValueKey.MeleeDefenseBonus, 1) != 1 ? Math.Round(((item.Values(DoubleValueKey.MeleeDefenseBonus) - 1) * 100)).ToString() : String.Empty) + ",");
				output.Append((item.Values(DoubleValueKey.MagicDBonus, 1) != 1 ? Math.Round(((item.Values(DoubleValueKey.MagicDBonus) - 1) * 100), 1).ToString() : String.Empty) + ",");
				output.Append((item.Values(DoubleValueKey.MissileDBonus, 1) != 1 ? Math.Round(((item.Values(DoubleValueKey.MissileDBonus) - 1) * 100), 1).ToString() : String.Empty) + ",");
				output.Append((item.Values(DoubleValueKey.ManaCBonus) != 0 ? Math.Round((item.Values(DoubleValueKey.ManaCBonus) * 100)).ToString() : String.Empty) + ",");

				{
					int maxDamage = item.Values(LongValueKey.MaxDamage);
					int elementalDmgBonus = item.Values(LongValueKey.ElementalDmgBonus, 0);
					double damageBonus = item.Values(DoubleValueKey.DamageBonus, 1);
					double elementalDamageVersusMonsters = item.Values(DoubleValueKey.ElementalDamageVersusMonsters, 1);
					double attackBonus = item.Values(DoubleValueKey.AttackBonus, 1);
					double meleeDefenseBonus = item.Values(DoubleValueKey.MeleeDefenseBonus, 1);
					double manaCBonus = item.Values(DoubleValueKey.ManaCBonus);

					for (int i = 0 ; i < item.Values(LongValueKey.SpellCount) ; i++)
					{
						int spellId = item.Spell(i);

						// LongValueKey.MaxDamage
						if (spellId == 4395) maxDamage += 2; // Incantation of Blood Drinker, this spell on the item adds 2 more points of damage over a user casted 8
						if (spellId == 2598 && !IsActiveSpell(2598, item)) maxDamage += 2; // Minor Blood Thirst
						if (spellId == 2586 && !IsActiveSpell(2586, item)) maxDamage += 4; // Major Blood Thirst
						if (spellId == 4661 && !IsActiveSpell(4661, item)) maxDamage += 7; // Epic Blood Thirst

						// DoubleValueKey.ElementalDamageVersusMonsters
						if (spellId == 4414) elementalDamageVersusMonsters += .01; // Incantation of Spirit Drinker, this spell on the item adds 1 more % of damage over a user casted 8
						if (spellId == 3251 && !IsActiveSpell(3251, item)) elementalDamageVersusMonsters += .01; // Minor Spirit Thirst
						if (spellId == 3250 && !IsActiveSpell(3250, item)) elementalDamageVersusMonsters += .03; // Major Spirit Thirst
						if (spellId == 4670 && !IsActiveSpell(4670, item)) elementalDamageVersusMonsters += .04; // Epic Spirit Thirst

						// DoubleValueKey.AttackBonus
						if (spellId == 2603 && !IsActiveSpell(2603, item)) attackBonus += .03; // Minor Heart Thirst
						if (spellId == 2591 && !IsActiveSpell(2591, item)) attackBonus += .05; // Major Heart Thirst
						if (spellId == 4666 && !IsActiveSpell(4666, item)) attackBonus += .07; // Epic Heart Thirst

						// DoubleValueKey.MeleeDefenseBonus
						if (spellId == 2600 && !IsActiveSpell(2600, item)) meleeDefenseBonus += .03; // Minor Defender
						if (spellId == 2588 && !IsActiveSpell(2588, item)) meleeDefenseBonus += .05; // Major Defender
						if (spellId == 4663 && !IsActiveSpell(4663, item)) meleeDefenseBonus += .07; // Epic Defender

						// DoubleValueKey.ManaCBonus
						if (spellId == 3201 && !IsActiveSpell(3201, item)) manaCBonus *= 1.05; // Feeble Hermetic Link
						if (spellId == 3199 && !IsActiveSpell(3199, item)) manaCBonus *= 1.10; // Minor Hermetic Link
						if (spellId == 3202 && !IsActiveSpell(3202, item)) manaCBonus *= 1.15; // Moderate Hermetic Link
						if (spellId == 3200 && !IsActiveSpell(3200, item)) manaCBonus *= 1.20; // Major Hermetic Link
					}

					// LongValueKey.MaxDamage
					if (IsActiveSpell(1616, item)) maxDamage -= 20; // Blood Drinker VI
					if (IsActiveSpell(2096, item)) maxDamage -= 22; // Infected Caress
					if (IsActiveSpell(5183, item)) maxDamage -= 22; // Incantation of Blood Drinker
					if (IsActiveSpell(4395, item)) maxDamage -= 22; // Incantation of Blood Drinker, this spell on the item adds 2 more points of damage over a user casted 8

					// DoubleValueKey.ElementalDamageVersusMonsters:
					if (IsActiveSpell(3258, item)) elementalDamageVersusMonsters -= .06; // Spirit Drinker VI
					if (IsActiveSpell(3259, item)) elementalDamageVersusMonsters -= .07; // Infected Spirit Caress
					if (IsActiveSpell(5182, item)) elementalDamageVersusMonsters -= .07; // Incantation of Spirit Drinker
					if (IsActiveSpell(4414, item)) elementalDamageVersusMonsters -= .08; // Incantation of Spirit Drinker

					// DoubleValueKey.AttackBonus
					if (IsActiveSpell(1592, item)) attackBonus -= .15; // Heart Seeker VI
					if (IsActiveSpell(2106, item)) attackBonus -= .17; // Elysa's Sight
					if (IsActiveSpell(4405, item)) attackBonus -= .20; // Incantation of Heart Seeker

					// DoubleValueKey.MeleeDefenseBonus
					if (IsActiveSpell(1605, item)) meleeDefenseBonus -= .15; // Defender VI
					if (IsActiveSpell(2101, item)) meleeDefenseBonus -= .17; // Cragstone's Will
					if (IsActiveSpell(4400, item)) meleeDefenseBonus -= .17; // Incantation of Defender

					// DoubleValueKey.ManaCBonus
					if (IsActiveSpell(1480, item)) manaCBonus /= 1.60; // Hermetic Link VI
					if (IsActiveSpell(2117, item)) manaCBonus /= 1.70; // Mystic's Blessing
					if (IsActiveSpell(4418, item)) manaCBonus /= 1.80; // Incantation of Hermetic Link

					output.Append((maxDamage > 0 ? maxDamage.ToString() : String.Empty) + ",");
					output.Append((elementalDmgBonus != 0 ? elementalDmgBonus.ToString() : String.Empty) + ",");
					output.Append((damageBonus != 1 ? Math.Round(((damageBonus - 1) * 100)).ToString() : String.Empty) + ",");
					output.Append((elementalDamageVersusMonsters != 1 ? Math.Round(((elementalDamageVersusMonsters - 1) * 100)).ToString() : String.Empty) + ",");
					output.Append((attackBonus != 1 ? Math.Round(((attackBonus - 1) * 100)).ToString() : String.Empty) + ",");
					output.Append((meleeDefenseBonus != 1 ? Math.Round(((meleeDefenseBonus - 1) * 100)).ToString() : String.Empty) + ",");
					output.Append((manaCBonus != 0 ? Math.Round(manaCBonus * 100).ToString() : String.Empty) + ",");
				}

				output.Append((item.Values(LongValueKey.WieldReqValue) > 0 ? item.Values(LongValueKey.WieldReqValue).ToString() : String.Empty) + ",");

				output.Append((item.Values(LongValueKey.Workmanship) > 0 ? item.Values(LongValueKey.Workmanship).ToString() : String.Empty) + ",");
				output.Append((item.Values(LongValueKey.Value) > 0 ? item.Values(LongValueKey.Value).ToString() : String.Empty) + ",");
				output.Append((item.Values(LongValueKey.Burden) > 0 ? item.Values(LongValueKey.Burden).ToString() : String.Empty) + ",");

				writer.WriteLine(output);

				writer.Close();
			}
		}

		bool IsActiveSpell(int activeSpellId, WorldObject item)
		{
			for (int i = 0 ; i < item.ActiveSpellCount ; i++)
			{
				if (item.ActiveSpell(i) == activeSpellId)
					return true;
			}

			return false;
		}
	}
}
