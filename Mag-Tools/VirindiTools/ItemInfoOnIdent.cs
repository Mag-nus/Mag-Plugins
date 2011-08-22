using System;
using System.Collections.Generic;

using Decal.Adapter.Wrappers;
using Decal.Filters;

namespace MagTools.VirindiTools
{
	class ItemInfoOnIdent : IDisposable
	{
		public ItemInfoOnIdent()
		{
			try
			{
				PluginCore.core.ContainerOpened += new EventHandler<Decal.Adapter.ContainerOpenedEventArgs>(Core_ContainerOpened);
				PluginCore.core.WorldFilter.ChangeObject += new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
			}
			catch (Exception ex) { Util.LogError(ex); }
		}

		public void Dispose()
		{
			try
			{
				PluginCore.core.ContainerOpened -= new EventHandler<Decal.Adapter.ContainerOpenedEventArgs>(Core_ContainerOpened);
				PluginCore.core.WorldFilter.ChangeObject -= new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
			}
			catch (Exception ex) { Util.LogError(ex); }
		}

		void Core_ContainerOpened(object sender, Decal.Adapter.ContainerOpenedEventArgs e)
		{
			try
			{
				foreach (WorldObject wo in PluginCore.core.WorldFilter.GetByContainer(e.ItemGuid))
					ProcessItem(wo);
			}
			catch (Exception ex) { Util.LogError(ex); }
		}

		void WorldFilter_ChangeObject(object sender, Decal.Adapter.Wrappers.ChangeObjectEventArgs e)
		{
			try
			{
				if (e.Change != WorldChangeType.IdentReceived)
					return;

				if (PluginCore.core.Actions.CurrentSelection != e.Changed.Id)
					return;

				ProcessItem(e.Changed);
			}
			catch (Exception ex) { Util.LogError(ex); }
		}

		private void ProcessItem(WorldObject wo)
		{
			if (wo.ObjectClass == ObjectClass.Corpse ||
				wo.ObjectClass == ObjectClass.Door ||
				wo.ObjectClass == ObjectClass.Foci ||
				wo.ObjectClass == ObjectClass.Housing ||
				wo.ObjectClass == ObjectClass.Lifestone ||
				wo.ObjectClass == ObjectClass.Monster ||
				wo.ObjectClass == ObjectClass.Npc ||
				wo.ObjectClass == ObjectClass.Player ||
				wo.ObjectClass == ObjectClass.Portal ||
				wo.ObjectClass == ObjectClass.Vendor)
				return;

			/*
			 * me: FLootPluginClassifyCallback and immediate, difference being anything other than hte obv?
				Jess: immediate should only be called after queryneedsid and then IDing if needed
				Jess: and with all those you need to wait 1 frame after the packet
			 * 
			 * (12:25:27 AM) Jess: you can create something like a vtankinterface class
				(12:25:38 AM) Jess: and then only create an instance of it when vtank is present
				(12:25:57 AM) me: ok, i'll do that when I'm ready to distribute, for now I'd like to work on getting my ideas working
				(12:26:03 AM) Jess: so everytime you want to call something you check to see if that instance is null and if not you call the interface class which calls vtank
			 * 
			 * */

			if (uTank2.PluginCore.PC.FLootPluginQueryNeedsID(wo.Id))
			{
				// public delegate void delFLootPluginClassifyCallback(int obj, LootAction result, bool getsuccess);
				uTank2.PluginCore.delFLootPluginClassifyCallback callback = new uTank2.PluginCore.delFLootPluginClassifyCallback(uTankCallBack);
				uTank2.PluginCore.PC.FLootPluginClassifyCallback(wo.Id, callback);
			}
			else
			{
				uTank2.LootPlugins.LootAction result = uTank2.PluginCore.PC.FLootPluginClassifyImmediate(wo.Id);

				processVTankIdentLootAction(wo.Id, result);
			}
		}

		void uTankCallBack(int obj, uTank2.LootPlugins.LootAction result, bool getsuccess)
		{
			try
			{
				if (!getsuccess)
					return;

				processVTankIdentLootAction(obj, result);

			}
			catch (Exception ex) { Util.LogError(ex); }
		}

		private void processVTankIdentLootAction(int itemId, uTank2.LootPlugins.LootAction result)
		{
			WorldObject wo = PluginCore.core.WorldFilter[itemId];

			if (wo == null)
				return;

			// If a rule does not exist for this item and the user doesn't have it selected, ignore it
			if (String.IsNullOrEmpty(result.RuleName) && itemId != PluginCore.core.Actions.CurrentSelection)
				return;

			//<Tell:IIDString:221112:-2024140046>(Epics)<\\Tell>

			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append(@"<Tell:IIDString:221112:-2024140046>");

			sb.Append(result.IsNoLoot ? "-" : "+");

			if (!String.IsNullOrEmpty(result.RuleName))
				sb.Append("(" + result.RuleName + ")");

			sb.Append(@"<\\Tell>");

			sb.Append(" " + wo.Name);

			int set = wo.Values((Decal.Adapter.Wrappers.LongValueKey)265, 0);
			if (set != 0)
			{
				// This list was taken from Virindi Tank Loot Editor
				if (set == 27) sb.Append(", Acid Proof Set");
				if (set == 14) sb.Append(", Adept's Set");
				if (set == 6) sb.Append(", Ancient Relic Set");
				if (set == 15) sb.Append(", Archer's Set");
				if (set == 10) sb.Append(", Arm, Mind, Heart Set");
				if (set == 11) sb.Append(", Coat of the Perfect Light Set");
				if (set == 28) sb.Append(", Cold Proof Set");
				if (set == 18) sb.Append(", Crafter's Set");
				if (set == 30) sb.Append(", Dedication Set");
				if (set == 16) sb.Append(", Defender's Set");
				if (set == 20) sb.Append(", Dexterous Set");
				if (set == 9) sb.Append(", Empyrean Rings Set");
				if (set == 26) sb.Append(", Flame Proof Set");
				if (set == 31) sb.Append(", Gladiatorial Clothing Set");
				if (set == 23) sb.Append(", Hardenend Set");
				if (set == 19) sb.Append(", Hearty Set");
				if (set == 25) sb.Append(", Interlocking Set");
				if (set == 12) sb.Append(", Leggings of Perfect Light Set");
				if (set == 29) sb.Append(", Lightning Proof Set");
				if (set == 5) sb.Append(", Noble Relic Set");
				if (set == 32) sb.Append(", Protective Clothing Set");
				if (set == 24) sb.Append(", Reinforced Set");
				if (set == 7) sb.Append(", Relic Alduressa Set");
				if (set == 8) sb.Append(", Shou-jen Set");
				if (set == 13) sb.Append(", Soldier's Set");
				if (set == 22) sb.Append(", Swift Set");
				if (set == 17) sb.Append(", Tinker's Set");
				if (set == 21) sb.Append(", Wise Set");
			}

			if (wo.Values(LongValueKey.ArmorLevel) > 0)
				sb.Append(", AL " + wo.Values(LongValueKey.ArmorLevel));

			if (wo.Values(LongValueKey.Imbued) > 0)
			{
				sb.Append(",");
				if ((wo.Values(LongValueKey.Imbued) & 1) == 1) sb.Append(" CS");
				if ((wo.Values(LongValueKey.Imbued) & 2) == 2) sb.Append(" CB");
				if ((wo.Values(LongValueKey.Imbued) & 4) == 4) sb.Append(" AR");
				if ((wo.Values(LongValueKey.Imbued) & 8) == 8) sb.Append(" SlashRend");
				if ((wo.Values(LongValueKey.Imbued) & 16) == 16) sb.Append(" PierceRend");
				if ((wo.Values(LongValueKey.Imbued) & 32) == 32) sb.Append(" BludgeRend");
				if ((wo.Values(LongValueKey.Imbued) & 64) == 64) sb.Append(" AcidRend");
				if ((wo.Values(LongValueKey.Imbued) & 128) == 128) sb.Append(" FrostRend");
				if ((wo.Values(LongValueKey.Imbued) & 256) == 256) sb.Append(" LightRend");
				if ((wo.Values(LongValueKey.Imbued) & 512) == 512) sb.Append(" FireRend");
			}

			if (wo.Values(LongValueKey.NumberTimesTinkered) > 0)
				sb.Append(", Tinks " + wo.Values(LongValueKey.NumberTimesTinkered));

			if (wo.Values(LongValueKey.MaxDamage) != 0 && wo.Values(DoubleValueKey.Variance) != 0)
				sb.Append(", " + (wo.Values(LongValueKey.MaxDamage) - (wo.Values(LongValueKey.MaxDamage) * wo.Values(DoubleValueKey.Variance))).ToString("N2") + "-" + wo.Values(LongValueKey.MaxDamage));

			if (wo.Values(DoubleValueKey.DamageBonus, 1) != 1)
				sb.Append(", +" + Math.Round(((wo.Values(DoubleValueKey.DamageBonus) - 1) * 100)) + "%");

			if (wo.Values(DoubleValueKey.ElementalDamageVersusMonsters, 1) != 1)
				sb.Append(", +" + Math.Round(((wo.Values(DoubleValueKey.ElementalDamageVersusMonsters) - 1) * 100)) + "%vs. Monsters");

			if (wo.Values(DoubleValueKey.AttackBonus, 1) != 1)
				sb.Append(", +" + Math.Round(((wo.Values(DoubleValueKey.AttackBonus) - 1) * 100)) + "%a");
			
			if (wo.Values(DoubleValueKey.MeleeDefenseBonus, 1) != 1)
				sb.Append(", " + Math.Round(((wo.Values(DoubleValueKey.MeleeDefenseBonus) - 1) * 100)) + "%md");

			if (wo.Values(DoubleValueKey.ManaCBonus) != 0)
				sb.Append(", " + Math.Round((wo.Values(DoubleValueKey.ManaCBonus) * 100)) + "%mc");

			if (wo.Values(LongValueKey.SpellCount) > 0)
			{
				FileService service = PluginCore.core.Filter<FileService>();

				List<int> itemActiveSpells = new List<int>();

				for (int i = 0 ; i < wo.Values(LongValueKey.SpellCount) ; i++)
					itemActiveSpells.Add(wo.Spell(i));

				itemActiveSpells.Sort();
				itemActiveSpells.Reverse();

				foreach (int spell in itemActiveSpells)
				{
					Spell spellById = service.SpellTable.GetById(spell);

					// Don't show banes on unenchantable armor
					if (wo.Values(LongValueKey.Unenchantable, 0) == 0 && (spellById.Name.Contains(" Bane") || spellById.Name.StartsWith("Impen") || spellById.Name.StartsWith("Brogard")))
						continue;

					// We should also add the ability to filter lvl 7, lvl 6, lvl 5 spells, etc..

					sb.Append(", " + spellById.Name);
				}
			}

			// Wield Lvl 180
			//if (wo.Values(LongValueKey.WieldReqValue) > 0)
			//	sb.Append(", Wield Lvl " + wo.Values(LongValueKey.WieldReqValue));

			// Melee Defense 300 to Activate

			if (wo.Values(LongValueKey.LoreRequirement) > 0)
				sb.Append(", Diff " + wo.Values(LongValueKey.LoreRequirement));

			if (wo.Values(LongValueKey.Workmanship) > 0 && wo.Values(LongValueKey.NumberTimesTinkered) != 10) // Don't show the work if its already 10 tinked.
				sb.Append(", Craft " + wo.Values(LongValueKey.Workmanship));

			if (wo.ObjectClass == ObjectClass.Armor && wo.Values(LongValueKey.Unenchantable, 0) != 0)
			{
				sb.Append(", [" +
					wo.Values(DoubleValueKey.SlashProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.PierceProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.BludgeonProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.ColdProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.FireProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.AcidProt).ToString("N1") + "/" +
					wo.Values(DoubleValueKey.LightningProt).ToString("N1") + "]");
			}

			PluginCore.host.Actions.AddChatText(sb.ToString(), 14, 1);
		}
	}
}
