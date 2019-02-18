using System;
using System.Globalization;

using Mag.Shared;
using Mag.Shared.Settings;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class MainView : IDisposable
	{
		readonly VirindiViewService.ViewProperties properties;
		readonly VirindiViewService.ControlGroup controls;
		readonly VirindiViewService.HudView view;


		// Mana Tracker
		public HudList ManaList { get; private set; }

		public HudStaticText ManaTotal { get; private set; }
		HudCheckBox ManaRecharge { get; set; }
		public HudStaticText UnretainedTotal { get; private set; }

		// Combat Tracker
		public HudList CombatTrackerMonsterListCurrent { get; private set; }
		public HudList CombatTrackerDamageListCurrent { get; private set; }
		public HudList CombatTrackerMonsterListPersistent { get; private set; }
		public HudList CombatTrackerDamageListPersistent { get; private set; }

		// Combat Tracker - Options
		public HudButton CombatTrackerClearCurrentStats { get; private set; }
		public HudButton CombatTrackerExportCurrentStats { get; private set; }
		public HudButton CombatTrackerClearPersistentStats { get; private set; }

		HudCheckBox CombatTrackerExportOnLogOff { get; set; }
		HudCheckBox CombatTrackerPersistent { get; set; }
		HudCheckBox CombatTrackerSortAlphabetically { get; set; }

		// Corpse Tracker
		public HudList CorpseTrackerList { get; private set; }

		// Corpse Tracker - Options
		public HudButton CorpseTrackerClearHistory { get; private set; }

		HudCheckBox CorpseTrackerEnabled { get; set; }
		HudCheckBox CorpseTrackerPersistent { get; set; }
		HudCheckBox CorpseTrackerTrackAllCorpses { get; set; }
		HudCheckBox CorpseTrackerTrackFellowCorpses { get; set; }
		HudCheckBox CorpseTrackerTrackPermittedCorpses { get; set; }

		// Player Tracker
		public HudList PlayerTrackerList { get; private set; }

		// Player Tracker - Options
		public HudButton PlayerTrackerClearHistory { get; private set; }

		HudCheckBox PlayerTrackerEnabled { get; set; }
		HudCheckBox PlayerTrackerPersistent { get; set; }

		// Item Count Tracker
		public HudList InventoryTrackerList { get; private set; }


		// Chat Logger
		public HudList ChatLogger1List { get; private set; }
		public HudList ChatLogger2List { get; private set; }

		// Chat - Options
		public HudButton ChatLoggerClearHistory { get; private set; }
		HudCheckBox ChatLoggerPersistent { get; set; }

		public HudList ChatGroup1OptionsList { get; private set; }
		public HudList ChatGroup2OptionsList { get; private set; }


		// Tools - Inventory
		public HudButton ClipboardWornEquipment { get; private set; }
		public HudButton ClipboardInventoryInfo { get; private set; }

		public HudTextBox InventorySearch { get; private set; }
		public HudList InventoryList { get; private set; }
		public HudStaticText InventoryItemText { get; private set; }

		// Tools - Tinkering
		public HudButton TinkeringAddSelectedItem { get; private set; }

		public HudCombo TinkeringMaterial { get; private set; }
		public HudTextBox TinkeringMinimumPercent { get; private set; }
		public HudTextBox TinkeringTargetTotalTinks { get; private set; }

		public HudButton TinkeringStart { get; private set; }
		public HudButton TinkeringStop { get; private set; }

		public HudList TinkeringList { get; private set; }

		// Tools - Character
		public HudTextBox LoginText { get; private set; }
		public HudButton LoginAdd { get; private set; }
		public HudList LoginList { get; private set; }

		public HudTextBox LoginCompleteText { get; private set; }
		public HudButton LoginCompleteAdd { get; private set; }
		public HudList LoginCompleteList { get; private set; }

		public HudTextBox PeriodicCommandText { get; private set; }
		public HudTextBox PeriodicCommandInterval { get; private set; }
		public HudTextBox PeriodicCommandOffset { get; private set; }
		public HudButton PeriodicCommandAdd { get; private set; }
		public HudList PeriodicCommandList { get; private set; }

		// Tools - Server
		public HudTextBox ServerLoginText { get; private set; }
		public HudButton ServerLoginAdd { get; private set; }
		public HudList ServerLoginList { get; private set; }

		public HudTextBox ServerLoginCompleteText { get; private set; }
		public HudButton ServerLoginCompleteAdd { get; private set; }
		public HudList ServerLoginCompleteList { get; private set; }

		public HudTextBox ServerPeriodicCommandText { get; private set; }
		public HudTextBox ServerPeriodicCommandInterval { get; private set; }
		public HudTextBox ServerPeriodicCommandOffset { get; private set; }
		public HudButton ServerPeriodicCommandAdd { get; private set; }
		public HudList ServerPeriodicCommandList { get; private set; }


		// Misc - Options
		HudList OptionList { get; set; }

		public HudTextBox OutputWindow { get; private set; }

		// Misc - Filters
		HudList FiltersList { get; set; }

		// Misc - Client
		HudCheckBox ClientRemoveFrame { get; set; }

		HudButton ClientSetWindowPosition { get; set; }
		HudButton ClientDelWindowPosition { get; set; }
		HudStaticText ClientSetPosition { get; set; }

		HudTextBox NoFocusFPS { get; set; }
		HudTextBox MaxFPS { get; set; }

		// Misc - About
		public HudStaticText VersionLabel { get; private set; }

		public MainView()
		{
			try
			{
				// Create the view
				VirindiViewService.XMLParsers.Decal3XMLParser parser = new VirindiViewService.XMLParsers.Decal3XMLParser();
				parser.ParseFromResource("MagTools.Views.mainView.xml", out properties, out controls);

				// Display the view
				view = new VirindiViewService.HudView(properties, controls);


				// Assign the views objects to our local variables

				// Mana Tracker
				ManaList = view != null ? (HudList)view["ManaList"] : new HudList();

				ManaTotal = view != null ? (HudStaticText)view["ManaTotal"] : new HudStaticText();
				ManaRecharge = view != null ? (HudCheckBox)view["ManaRecharge"] : new HudCheckBox();
				UnretainedTotal = view != null ? (HudStaticText)view["UnretainedTotal"] : new HudStaticText();

				// Combat Tracker
				CombatTrackerMonsterListCurrent = view != null ? (HudList)view["CombatTrackerMonsterListCurrent"] : new HudList();
				CombatTrackerDamageListCurrent = view != null ? (HudList)view["CombatTrackerDamageListCurrent"] : new HudList();
				CombatTrackerMonsterListPersistent = view != null ? (HudList)view["CombatTrackerMonsterListPersistent"] : new HudList();
				CombatTrackerDamageListPersistent = view != null ? (HudList)view["CombatTrackerDamageListPersistent"] : new HudList();

				// Combat Tracker - Options
				CombatTrackerClearCurrentStats = view != null ? (HudButton)view["CombatTrackerClearCurrentStats"] : new HudButton();
				CombatTrackerExportCurrentStats = view != null ? (HudButton)view["CombatTrackerExportCurrentStats"] : new HudButton();
				CombatTrackerClearPersistentStats = view != null ? (HudButton)view["CombatTrackerClearPersistentStats"] : new HudButton();

				CombatTrackerExportOnLogOff = view != null ? (HudCheckBox)view["CombatTrackerExportOnLogOff"] : new HudCheckBox();
				CombatTrackerPersistent = view != null ? (HudCheckBox)view["CombatTrackerPersistent"] : new HudCheckBox();
				CombatTrackerSortAlphabetically = view != null ? (HudCheckBox)view["CombatTrackerSortAlphabetically"] : new HudCheckBox();

				// Corpse Tracker
				CorpseTrackerList = view != null ? (HudList)view["CorpseTrackerList"] : new HudList();

				// Corpse Tracker - Options
				CorpseTrackerClearHistory = view != null ? (HudButton)view["CorpseTrackerClearHistory"] : new HudButton();

				CorpseTrackerEnabled = view != null ? (HudCheckBox)view["CorpseTrackerEnabled"] : new HudCheckBox();
				CorpseTrackerPersistent = view != null ? (HudCheckBox)view["CorpseTrackerPersistent"] : new HudCheckBox();
				CorpseTrackerTrackAllCorpses = view != null ? (HudCheckBox)view["CorpseTrackerTrackAllCorpses"] : new HudCheckBox();
				CorpseTrackerTrackFellowCorpses = view != null ? (HudCheckBox)view["CorpseTrackerTrackFellowCorpses"] : new HudCheckBox();
				CorpseTrackerTrackPermittedCorpses = view != null ? (HudCheckBox)view["CorpseTrackerTrackPermittedCorpses"] : new HudCheckBox();

				// Player Tracker
				PlayerTrackerList = view != null ? (HudList)view["PlayerTrackerList"] : new HudList();

				// Player Tracker - Options
				PlayerTrackerClearHistory = view != null ? (HudButton)view["PlayerTrackerClearHistory"] : new HudButton();

				PlayerTrackerEnabled = view != null ? (HudCheckBox)view["PlayerTrackerEnabled"] : new HudCheckBox();
				PlayerTrackerPersistent = view != null ? (HudCheckBox)view["PlayerTrackerPersistent"] : new HudCheckBox();

				// Item Count Tracker
				InventoryTrackerList = view != null ? (HudList)view["InventoryTrackerList"] : new HudList();


				// Chat Logger
				ChatLogger1List = view != null ? (HudList)view["ChatLogger1List"] : new HudList();
				ChatLogger2List = view != null ? (HudList)view["ChatLogger2List"] : new HudList();

				// Chat Logger - Options
				ChatLoggerClearHistory = view != null ? (HudButton)view["ChatLoggerClearHistory"] : new HudButton();
				ChatLoggerPersistent = view != null ? (HudCheckBox)view["ChatLoggerPersistent"] : new HudCheckBox();

				ChatGroup1OptionsList = view != null ? (HudList)view["ChatGroup1OptionsList"] : new HudList();
				ChatGroup2OptionsList = view != null ? (HudList)view["ChatGroup2OptionsList"] : new HudList();


				// Tools - Inventory
				ClipboardWornEquipment = view != null ? (HudButton)view["ClipboardWornEquipment"] : new HudButton();
				ClipboardInventoryInfo = view != null ? (HudButton)view["ClipboardInventoryInfo"] : new HudButton();

				InventorySearch = view != null ? (HudTextBox)view["InventorySearch"] : new HudTextBox();
				InventorySearch.Hit += (s, e) => { if (InventorySearch.Text == "regex search string") InventorySearch.Text = String.Empty; };
				InventoryList = view != null ? (HudList)view["InventoryList"] : new HudList();
				InventoryItemText = view != null ? (HudStaticText)view["InventoryItemText"] : new HudStaticText();

				// Tools - Tinkering
				TinkeringAddSelectedItem = view != null ? (HudButton)view["TinkeringAddSelectedItem"] : new HudButton();

				TinkeringMaterial = view != null ? (HudCombo)view["TinkeringMaterial"] : new HudCombo(view.Controls);
				TinkeringMinimumPercent = view != null ? (HudTextBox)view["TinkeringMinimumPercent"] : new HudTextBox();
				TinkeringTargetTotalTinks = view != null ? (HudTextBox)view["TinkeringTargetTotalTinks"] : new HudTextBox();

				TinkeringStart = view != null ? (HudButton)view["TinkeringStart"] : new HudButton();
				TinkeringStop = view != null ? (HudButton)view["TinkeringStop"] : new HudButton();

				TinkeringList = view != null ? (HudList)view["TinkeringList"] : new HudList();

				// Tools - Character
				LoginText = view != null ? (HudTextBox)view["LoginText"] : new HudTextBox();
				LoginAdd = view != null ? (HudButton)view["LoginAdd"] : new HudButton();
				LoginList = view != null ? (HudList)view["LoginList"] : new HudList();

				LoginCompleteText = view != null ? (HudTextBox)view["LoginCompleteText"] : new HudTextBox();
				LoginCompleteAdd = view != null ? (HudButton)view["LoginCompleteAdd"] : new HudButton();
				LoginCompleteList = view != null ? (HudList)view["LoginCompleteList"] : new HudList();

				PeriodicCommandText = view != null ? (HudTextBox)view["PeriodicCommandText"] : new HudTextBox();
				PeriodicCommandInterval = view != null ? (HudTextBox)view["PeriodicCommandInterval"] : new HudTextBox();
				PeriodicCommandOffset = view != null ? (HudTextBox)view["PeriodicCommandOffset"] : new HudTextBox();
				PeriodicCommandAdd = view != null ? (HudButton)view["PeriodicCommandAdd"] : new HudButton();
				PeriodicCommandList = view != null ? (HudList)view["PeriodicCommandList"] : new HudList();

				// Tools - Server
				ServerLoginText = view != null ? (HudTextBox)view["ServerLoginText"] : new HudTextBox();
				ServerLoginAdd = view != null ? (HudButton)view["ServerLoginAdd"] : new HudButton();
				ServerLoginList = view != null ? (HudList)view["ServerLoginList"] : new HudList();

				ServerLoginCompleteText = view != null ? (HudTextBox)view["ServerLoginCompleteText"] : new HudTextBox();
				ServerLoginCompleteAdd = view != null ? (HudButton)view["ServerLoginCompleteAdd"] : new HudButton();
				ServerLoginCompleteList = view != null ? (HudList)view["ServerLoginCompleteList"] : new HudList();

				ServerPeriodicCommandText = view != null ? (HudTextBox)view["ServerPeriodicCommandText"] : new HudTextBox();
				ServerPeriodicCommandInterval = view != null ? (HudTextBox)view["ServerPeriodicCommandInterval"] : new HudTextBox();
				ServerPeriodicCommandOffset = view != null ? (HudTextBox)view["ServerPeriodicCommandOffset"] : new HudTextBox();
				ServerPeriodicCommandAdd = view != null ? (HudButton)view["ServerPeriodicCommandAdd"] : new HudButton();
				ServerPeriodicCommandList = view != null ? (HudList)view["ServerPeriodicCommandList"] : new HudList();


				// Misc - Options
				OptionList = view != null ? (HudList)view["OptionList"] : new HudList();

				OutputWindow = view != null ? (HudTextBox)view["OutputWindow"] : new HudTextBox();

				// Misc - Filters
				FiltersList = view != null ? (HudList)view["FiltersList"] : new HudList();

				// Misc - Client
				ClientRemoveFrame = view != null ? (HudCheckBox)view["ClientRemoveFrame"] : new HudCheckBox();

				ClientSetWindowPosition = view != null ? (HudButton)view["ClientSetWindowPosition"] : new HudButton();
				ClientDelWindowPosition = view != null ? (HudButton)view["ClientDelWindowPosition"] : new HudButton();
				ClientSetPosition = view != null ? (HudStaticText)view["ClientSetPosition"] : new HudStaticText();

				NoFocusFPS = view != null ? (HudTextBox)view["NoFocusFPS"] : new HudTextBox();
				MaxFPS = view != null ? (HudTextBox)view["MaxFPS"] : new HudTextBox();

				// Misc - About
				VersionLabel = view != null ? (HudStaticText)view["VersionLabel"] : new HudStaticText();


				// ******************************************************
				// Link some of our controls to our configuration manager
				// ******************************************************

				// Mana Tracker
				ManaRecharge.Checked = Settings.SettingsManager.ManaManagement.AutoRecharge.Value;
				Settings.SettingsManager.ManaManagement.AutoRecharge.Changed += obj => { ManaRecharge.Checked = obj.Value; };
				ManaRecharge.Change += (s, e) => { try { Settings.SettingsManager.ManaManagement.AutoRecharge.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				// Combat Tracker
				CombatTrackerExportOnLogOff.Checked = Settings.SettingsManager.CombatTracker.ExportOnLogOff.Value;
				Settings.SettingsManager.CombatTracker.ExportOnLogOff.Changed += obj => { CombatTrackerExportOnLogOff.Checked = obj.Value; };
				CombatTrackerExportOnLogOff.Change += (s, e) => { try { Settings.SettingsManager.CombatTracker.ExportOnLogOff.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				CombatTrackerPersistent.Checked = Settings.SettingsManager.CombatTracker.Persistent.Value;
				Settings.SettingsManager.CombatTracker.Persistent.Changed += obj => { CombatTrackerPersistent.Checked = obj.Value; };
				CombatTrackerPersistent.Change += (s, e) => { try { Settings.SettingsManager.CombatTracker.Persistent.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				CombatTrackerSortAlphabetically.Checked = Settings.SettingsManager.CombatTracker.SortAlphabetically.Value;
				Settings.SettingsManager.CombatTracker.SortAlphabetically.Changed += obj => { CombatTrackerSortAlphabetically.Checked = obj.Value; };
				CombatTrackerSortAlphabetically.Change += (s, e) => { try { Settings.SettingsManager.CombatTracker.SortAlphabetically.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				// Corpse Tracker
				CorpseTrackerEnabled.Checked = Settings.SettingsManager.CorpseTracker.Enabled.Value;
				Settings.SettingsManager.CorpseTracker.Enabled.Changed += obj => { CorpseTrackerEnabled.Checked = obj.Value; };
				CorpseTrackerEnabled.Change += (s, e) => { try { Settings.SettingsManager.CorpseTracker.Enabled.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				CorpseTrackerPersistent.Checked = Settings.SettingsManager.CorpseTracker.Persistent.Value;
				Settings.SettingsManager.CorpseTracker.Persistent.Changed += obj => { CorpseTrackerPersistent.Checked = obj.Value; };
				CorpseTrackerPersistent.Change += (s, e) => { try { Settings.SettingsManager.CorpseTracker.Persistent.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				CorpseTrackerTrackAllCorpses.Checked = Settings.SettingsManager.CorpseTracker.TrackAllCorpses.Value;
				Settings.SettingsManager.CorpseTracker.TrackAllCorpses.Changed += obj => { CorpseTrackerTrackAllCorpses.Checked = obj.Value; };
				CorpseTrackerTrackAllCorpses.Change += (s, e) => { try { Settings.SettingsManager.CorpseTracker.TrackAllCorpses.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				CorpseTrackerTrackFellowCorpses.Checked = Settings.SettingsManager.CorpseTracker.TrackFellowCorpses.Value;
				Settings.SettingsManager.CorpseTracker.TrackFellowCorpses.Changed += obj => { CorpseTrackerTrackFellowCorpses.Checked = obj.Value; };
				CorpseTrackerTrackFellowCorpses.Change += (s, e) => { try { Settings.SettingsManager.CorpseTracker.TrackFellowCorpses.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				CorpseTrackerTrackPermittedCorpses.Checked = Settings.SettingsManager.CorpseTracker.TrackPermittedCorpses.Value;
				Settings.SettingsManager.CorpseTracker.TrackPermittedCorpses.Changed += obj => { CorpseTrackerTrackPermittedCorpses.Checked = obj.Value; };
				CorpseTrackerTrackPermittedCorpses.Change += (s, e) => { try { Settings.SettingsManager.CorpseTracker.TrackPermittedCorpses.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				// Player Tracker
				PlayerTrackerEnabled.Checked = Settings.SettingsManager.PlayerTracker.Enabled.Value;
				Settings.SettingsManager.PlayerTracker.Enabled.Changed += obj => { PlayerTrackerEnabled.Checked = obj.Value; };
				PlayerTrackerEnabled.Change += (s, e) => { try { Settings.SettingsManager.PlayerTracker.Enabled.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				PlayerTrackerPersistent.Checked = Settings.SettingsManager.PlayerTracker.Persistent.Value;
				Settings.SettingsManager.PlayerTracker.Persistent.Changed += obj => { PlayerTrackerPersistent.Checked = obj.Value; };
				PlayerTrackerPersistent.Change += (s, e) => { try { Settings.SettingsManager.PlayerTracker.Persistent.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };


				// Chat Logger
				ChatLoggerPersistent.Checked = Settings.SettingsManager.ChatLogger.Persistent.Value;
				Settings.SettingsManager.ChatLogger.Persistent.Changed += obj => { ChatLoggerPersistent.Checked = obj.Value; };
				ChatLoggerPersistent.Change += (s, e) => { try { Settings.SettingsManager.ChatLogger.Persistent.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				AddOption(ChatGroup1OptionsList, Settings.SettingsManager.ChatLogger.Groups[0].Area);
				AddOption(ChatGroup1OptionsList, Settings.SettingsManager.ChatLogger.Groups[0].Tells);
				AddOption(ChatGroup1OptionsList, Settings.SettingsManager.ChatLogger.Groups[0].Fellowship);
				AddOption(ChatGroup1OptionsList, Settings.SettingsManager.ChatLogger.Groups[0].General);
				AddOption(ChatGroup1OptionsList, Settings.SettingsManager.ChatLogger.Groups[0].Trade);
				AddOption(ChatGroup1OptionsList, Settings.SettingsManager.ChatLogger.Groups[0].Allegiance);

				AddOption(ChatGroup2OptionsList, Settings.SettingsManager.ChatLogger.Groups[1].Area);
				AddOption(ChatGroup2OptionsList, Settings.SettingsManager.ChatLogger.Groups[1].Tells);
				AddOption(ChatGroup2OptionsList, Settings.SettingsManager.ChatLogger.Groups[1].Fellowship);
				AddOption(ChatGroup2OptionsList, Settings.SettingsManager.ChatLogger.Groups[1].General);
				AddOption(ChatGroup2OptionsList, Settings.SettingsManager.ChatLogger.Groups[1].Trade);
				AddOption(ChatGroup2OptionsList, Settings.SettingsManager.ChatLogger.Groups[1].Allegiance);


				// Misc.Options
				AddOption(OptionList, Settings.SettingsManager.ItemInfoOnIdent.Enabled);
				AddOption(OptionList, Settings.SettingsManager.ItemInfoOnIdent.ShowBuffedValues);
				AddOption(OptionList, Settings.SettingsManager.ItemInfoOnIdent.ShowValueAndBurden);
				AddOption(OptionList, Settings.SettingsManager.ItemInfoOnIdent.LeftClickIdent);
				AddOption(OptionList, Settings.SettingsManager.ItemInfoOnIdent.AutoClipboard);

				AddOption(OptionList, Settings.SettingsManager.AutoBuySell.Enabled);
				AddOption(OptionList, Settings.SettingsManager.AutoBuySell.TestMode);

				AddOption(OptionList, Settings.SettingsManager.AutoTradeAdd.Enabled);

				AddOption(OptionList, Settings.SettingsManager.AutoTradeAccept.Enabled);

				AddOption(OptionList, Settings.SettingsManager.Looting.AutoLootChests);
				AddOption(OptionList, Settings.SettingsManager.Looting.AutoLootCorpses);
				AddOption(OptionList, Settings.SettingsManager.Looting.AutoLootMyCorpses);
				AddOption(OptionList, Settings.SettingsManager.Looting.LootSalvage);

				AddOption(OptionList, Settings.SettingsManager.Tinkering.AutoClickYes);

				AddOption(OptionList, Settings.SettingsManager.InventoryManagement.InventoryLogger);
				AddOption(OptionList, Settings.SettingsManager.InventoryManagement.AetheriaRevealer);
				AddOption(OptionList, Settings.SettingsManager.InventoryManagement.HeartCarver);
				AddOption(OptionList, Settings.SettingsManager.InventoryManagement.ShatteredKeyFixer);
				AddOption(OptionList, Settings.SettingsManager.InventoryManagement.KeyRinger);
				AddOption(OptionList, Settings.SettingsManager.InventoryManagement.KeyDeringer);

				AddOption(OptionList, Settings.SettingsManager.Misc.OpenMainPackOnLogin);
				AddOption(OptionList, Settings.SettingsManager.Misc.MaximizeChatOnLogin);
				AddOption(OptionList, Settings.SettingsManager.Misc.LogOutOnDeath);
				AddOption(OptionList, Settings.SettingsManager.Misc.DebuggingEnabled);
				AddOption(OptionList, Settings.SettingsManager.Misc.VerboseDebuggingEnabled);

				OutputWindow.Text = Settings.SettingsManager.Misc.OutputTargetWindow.Value.ToString(CultureInfo.InvariantCulture);
				OutputWindow.Change += (s, e) =>
				{
					try
					{
						int value;
						if (!int.TryParse(OutputWindow.Text, out value))
							value = Settings.SettingsManager.Misc.OutputTargetWindow.DefaultValue;
						Settings.SettingsManager.Misc.OutputTargetWindow.Value = value;
					}
					catch (Exception ex) { Debug.LogException(ex); }
				};

				// Misc.Filters
				AddOption(FiltersList, Settings.SettingsManager.Filters.AttackEvades);
				AddOption(FiltersList, Settings.SettingsManager.Filters.DefenseEvades);
				AddOption(FiltersList, Settings.SettingsManager.Filters.AttackResists);
				AddOption(FiltersList, Settings.SettingsManager.Filters.DefenseResists);
				AddOption(FiltersList, Settings.SettingsManager.Filters.NPKFails);
				AddOption(FiltersList, Settings.SettingsManager.Filters.DirtyFighting);
				AddOption(FiltersList, Settings.SettingsManager.Filters.MonsterDeaths);

				AddOption(FiltersList, Settings.SettingsManager.Filters.SpellCastingMine);
				AddOption(FiltersList, Settings.SettingsManager.Filters.SpellCastingOthers);
				AddOption(FiltersList, Settings.SettingsManager.Filters.SpellCastFizzles);
				AddOption(FiltersList, Settings.SettingsManager.Filters.CompUsage);
				AddOption(FiltersList, Settings.SettingsManager.Filters.SpellExpires);

				AddOption(FiltersList, Settings.SettingsManager.Filters.HealingKitSuccess);
				AddOption(FiltersList, Settings.SettingsManager.Filters.HealingKitFail);
				AddOption(FiltersList, Settings.SettingsManager.Filters.Salvaging);
				AddOption(FiltersList, Settings.SettingsManager.Filters.SalvagingFails);
				AddOption(FiltersList, Settings.SettingsManager.Filters.AuraOfCraftman);
				AddOption(FiltersList, Settings.SettingsManager.Filters.ManaStoneUsage);

				AddOption(FiltersList, Settings.SettingsManager.Filters.TradeBuffBotSpam);
				AddOption(FiltersList, Settings.SettingsManager.Filters.FailedAssess);

				AddOption(FiltersList, Settings.SettingsManager.Filters.KillTaskComplete);
				AddOption(FiltersList, Settings.SettingsManager.Filters.VendorTells);
				AddOption(FiltersList, Settings.SettingsManager.Filters.MonsterTell);
				AddOption(FiltersList, Settings.SettingsManager.Filters.NpcChatter);
				AddOption(FiltersList, Settings.SettingsManager.Filters.MasterArbitratorSpam);
				AddOption(FiltersList, Settings.SettingsManager.Filters.AllMasterArbitratorChat);

				AddOption(FiltersList, Settings.SettingsManager.Filters.StatusTextYoureTooBusy);
				AddOption(FiltersList, Settings.SettingsManager.Filters.StatusTextCasting);
				AddOption(FiltersList, Settings.SettingsManager.Filters.StatusTextAll);

				// Misc.Client
				ClientRemoveFrame.Checked = Settings.SettingsManager.Misc.RemoveWindowFrame.Value;
				Settings.SettingsManager.Misc.RemoveWindowFrame.Changed += obj => { ClientRemoveFrame.Checked = obj.Value; };
				ClientRemoveFrame.Change += (s, e) => { try { Settings.SettingsManager.Misc.RemoveWindowFrame.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				ClientSetWindowPosition.Hit += (s, e) => { try { Client.WindowMover.SetWindowPosition(); UpdateClientWindowPositionLabel(); } catch (Exception ex) { Debug.LogException(ex); } };
				ClientDelWindowPosition.Hit += (s, e) =>  { try { Client.WindowMover.DeleteWindowPosition(); UpdateClientWindowPositionLabel(); } catch (Exception ex) { Debug.LogException(ex); } };
				UpdateClientWindowPositionLabel();

				NoFocusFPS.Text = Settings.SettingsManager.Misc.NoFocusFPS.Value.ToString(CultureInfo.InvariantCulture);
				NoFocusFPS.Change += (s, e) =>
				{
					try
					{
						int value;
						if (!int.TryParse(NoFocusFPS.Text, out value))
							value = Settings.SettingsManager.Misc.NoFocusFPS.DefaultValue;
						else if (value <= Settings.SettingsManager.Misc.NoFocusFPS.DefaultValue)
						{
							Debug.WriteToChat("No Focus FPS cannot be less than " + Settings.SettingsManager.Misc.NoFocusFPS.DefaultValue + ". Set to " + Settings.SettingsManager.Misc.NoFocusFPS.DefaultValue + " to disable.");
							value = 10;
						}
						Settings.SettingsManager.Misc.NoFocusFPS.Value = value;
					}
					catch (Exception ex) { Debug.LogException(ex); }
				};

				MaxFPS.Text = Settings.SettingsManager.Misc.MaxFPS.Value.ToString(CultureInfo.InvariantCulture);
				MaxFPS.Change += (s, e) =>
				{
					try
					{
						int value;
						if (!int.TryParse(MaxFPS.Text, out value))
							value = Settings.SettingsManager.Misc.MaxFPS.DefaultValue;
						else if (value != 0 && value < 20)
						{ 
							Debug.WriteToChat("Maximum FPS cannot be less than 20. Set to zero to disable.");
							value = 20;
						}
						Settings.SettingsManager.Misc.MaxFPS.Value = value;
					}
					catch (Exception ex) { Debug.LogException(ex); }
				};
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private bool disposed;

		public void Dispose()
		{
			Dispose(true);

			// Use SupressFinalize in case a subclass
			// of this type implements a finalizer.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// If you need thread safety, use a lock around these 
			// operations, as well as in your methods that use the resource.
			if (!disposed)
			{
				if (disposing)
				{
					//Remove the view
					if (view != null)
						view.Dispose();
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void UpdateClientWindowPositionLabel()
		{
			Client.WindowPosition windowPosition;

			if (Client.WindowMover.GetWindowPositionForThisClient(out windowPosition))
				ClientSetPosition.Text = "Current Set Position: X: " + windowPosition.X + ", Y: " + windowPosition.Y;
			else
				ClientSetPosition.Text = "Current Set Position: not set.";
		}

		void AddOption(HudList hudList, Setting<bool> setting)
		{
			HudList.HudListRowAccessor newRow = hudList.AddRow();

			((HudCheckBox)newRow[0]).Checked = setting.Value;
			setting.Changed += obj => { ((HudCheckBox)newRow[0]).Checked = obj.Value; };
			((HudCheckBox)newRow[0]).Change += (s, e) =>
			{
				try
				{
					setting.Value = ((HudCheckBox)s).Checked;
				}
				catch (Exception ex) { Debug.LogException(ex); }
			};
			((HudStaticText)newRow[1]).Text = setting.Description;

		}
	}
}
