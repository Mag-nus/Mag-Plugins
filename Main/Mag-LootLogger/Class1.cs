using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Text;

using Mag.Shared;

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
					Start();
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
					writer.WriteLine("Timestamp,Container,Id,Name,ObjectClass,EquipSkill,MasteryBonus,DamageType,Variance,MaxDamage,ElementalDmgBonus,DamageBonus,ElementalDamageVersusMonsters,AttackBonus,MeleeDefenseBonus,MagicDBonus,MissileDBonus,ManaCBonus,BuffedMaxDamage,BuffedElementalDmgBonus,BuffedDamageBonus,BuffedElementalDamageVersusMonsters,BuffedAttackBonus,BuffedMeleeDefenseBonus,BuffedManaCBonus,WieldReqValue,Work,Value,Burden");

					writer.Close();
				}
			}

			using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
			{
				MyWorldObject mwo = MyWorldObjectCreator.Create(item);

				StringBuilder output = new StringBuilder();

				output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");

				string containerName = CoreManager.Current.WorldFilter[item.Container] != null ? CoreManager.Current.WorldFilter[item.Container].Name : null;
				output.Append('"' + containerName + '"' + ",");

				output.Append(item.Id + ",");
				output.Append('"' + item.Name + '"' + ",");
				output.Append(item.ObjectClass.ToString() + ",");

				string skillName = Constants.SkillInfo.ContainsKey(item.Values(LongValueKey.EquipSkill)) ? Constants.SkillInfo[item.Values(LongValueKey.EquipSkill)] : null;
				output.Append((item.Values(LongValueKey.EquipSkill) > 0 ? skillName : String.Empty) + ",");

				string masteryName = Constants.MasteryInfo.ContainsKey(item.Values((LongValueKey)353)) ? Constants.MasteryInfo[item.Values((LongValueKey)353)] : null;
				output.Append((item.Values((LongValueKey)353) > 0 ? masteryName : String.Empty) + ",");

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

				output.Append((mwo.GetBuffedIntValueKey((int)LongValueKey.MaxDamage) > 0 ? mwo.GetBuffedIntValueKey((int)LongValueKey.MaxDamage).ToString() : String.Empty) + ",");
				output.Append((mwo.GetBuffedIntValueKey((int)LongValueKey.ElementalDmgBonus) > 0 ? mwo.GetBuffedIntValueKey((int)LongValueKey.ElementalDmgBonus).ToString() : String.Empty) + ",");
				output.Append((mwo.GetBuffedDoubleValueKey((int)DoubleValueKey.DamageBonus, 1) != 1 ? Math.Round(((mwo.GetBuffedDoubleValueKey((int)DoubleValueKey.DamageBonus) - 1) * 100)).ToString() : String.Empty) + ",");
				output.Append((mwo.BuffedElementalDamageVersusMonsters != -1 ? Math.Round(((mwo.BuffedElementalDamageVersusMonsters - 1) * 100)).ToString() : String.Empty) + ",");
				output.Append((mwo.BuffedAttackBonus != -1 ? Math.Round(((mwo.BuffedAttackBonus - 1) * 100)).ToString() : String.Empty) + ",");
				output.Append((mwo.BuffedMeleeDefenseBonus != -1 ? Math.Round(((mwo.BuffedMeleeDefenseBonus - 1) * 100)).ToString() : String.Empty) + ",");
				output.Append((mwo.BuffedManaCBonus != -1 ? Math.Round(mwo.BuffedManaCBonus * 100).ToString() : String.Empty) + ",");

				output.Append((item.Values(LongValueKey.WieldReqValue) > 0 ? item.Values(LongValueKey.WieldReqValue).ToString() : String.Empty) + ",");

				output.Append((item.Values(LongValueKey.Workmanship) > 0 ? item.Values(LongValueKey.Workmanship).ToString() : String.Empty) + ",");
				output.Append((item.Values(LongValueKey.Value) > 0 ? item.Values(LongValueKey.Value).ToString() : String.Empty) + ",");
				output.Append((item.Values(LongValueKey.Burden) > 0 ? item.Values(LongValueKey.Burden).ToString() : String.Empty) + ",");

				writer.WriteLine(output);

				writer.Close();
			}
		}
	}
}
