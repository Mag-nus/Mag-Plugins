using System;
using System.Globalization;

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
		public HudList CorpseTrackerListCurrent { get; private set; }
		public HudList CorpseTrackerListPersistent { get; private set; }

		// Corpse Tracker - Options
		public HudButton CorpseTrackerClearCurrentStats { get; private set; }
		public HudButton CorpseTrackerClearPersistentStats { get; private set; }

		HudCheckBox CorpseTrackerEnabled { get; set; }
		HudCheckBox CorpseTrackerPersistent { get; set; }
		HudCheckBox CorpseTrackerTrackAllCorpses { get; set; }
		HudCheckBox CorpseTrackerTrackFellowCorpses { get; set; }
		HudCheckBox CorpseTrackerTrackPermittedCorpses { get; set; }

		// Player Tracker
		public HudList PlayerTrackerListCurrent { get; private set; }
		public HudList PlayerTrackerListPersistent { get; private set; }

		// Player Tracker - Options
		public HudButton PlayerTrackerClearCurrentStats { get; private set; }
		public HudButton PlayerTrackerClearPersistentStats { get; private set; }

		HudCheckBox PlayerTrackerEnabled { get; set; }
		HudCheckBox PlayerTrackerPersistent { get; set; }

		// Consumable Tracker
		public HudList ConsumableTrackerList { get; private set; }


		// Tells Logger
		public HudList TellsLoggerListCurrent { get; private set; }
		public HudList TellsLoggerListPersistent { get; private set; }

		// Tells Logger - Options
		public HudButton TellsLoggerClearCurrentLogs { get; private set; }
		public HudButton TellsLoggerClearPersistentLogs { get; private set; }

		HudCheckBox TellsLoggerEnabled { get; set; }
		HudCheckBox TellsLoggerPersistent { get; set; }

		// Local Logger
		public HudList LocalLoggerListCurrent { get; private set; }
		public HudList LocalLoggerListPersistent { get; private set; }

		// Local Logger - Options
		public HudButton LocalLoggerClearCurrentLogs { get; private set; }
		public HudButton LocalLoggerClearPersistentLogs { get; private set; }

		HudCheckBox LocalLoggerEnabled { get; set; }
		HudCheckBox LocalLoggerPersistent { get; set; }

		// Fellow Logger
		public HudList FellowLoggerListCurrent { get; private set; }
		public HudList FellowLoggerListPersistent { get; private set; }

		// Fellow Logger - Options
		public HudButton FellowLoggerClearCurrentLogs { get; private set; }
		public HudButton FellowLoggerClearPersistentLogs { get; private set; }

		HudCheckBox FellowLoggerEnabled { get; set; }
		HudCheckBox FellowLoggerPersistent { get; set; }

		// Channels Logger
		public HudList ChannelsLoggerListCurrent { get; private set; }
		public HudList ChannelsLoggerListPersistent { get; private set; }

		// Channels Logger - Options
		public HudButton ChannelsLoggerClearCurrentLogs { get; private set; }
		public HudButton ChannelsLoggerClearPersistentLogs { get; private set; }

		HudCheckBox ChannelsLoggerEnabled { get; set; }
		HudCheckBox ChannelsLoggerPersistent { get; set; }
		HudCheckBox ChannelsLoggerGeneral { get; set; }
		HudCheckBox ChannelsLoggerTrade { get; set; }
		HudCheckBox ChannelsLoggerAllegiance { get; set; }


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

		// Misc - Tools
		public HudButton ClipboardWornEquipment { get; private set; }
		public HudButton ClipboardInventoryInfo { get; private set; }

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
				CorpseTrackerListCurrent = view != null ? (HudList)view["CorpseTrackerListCurrent"] : new HudList();
				CorpseTrackerListPersistent = view != null ? (HudList)view["CorpseTrackerListPersistent"] : new HudList();

				// Corpse Tracker - Options
				CorpseTrackerClearCurrentStats = view != null ? (HudButton)view["CorpseTrackerClearCurrentStats"] : new HudButton();
				CorpseTrackerClearPersistentStats = view != null ? (HudButton)view["CorpseTrackerClearPersistentStats"] : new HudButton();

				CorpseTrackerEnabled = view != null ? (HudCheckBox)view["CorpseTrackerEnabled"] : new HudCheckBox();
				CorpseTrackerPersistent = view != null ? (HudCheckBox)view["CorpseTrackerPersistent"] : new HudCheckBox();
				CorpseTrackerTrackAllCorpses = view != null ? (HudCheckBox)view["CorpseTrackerTrackAllCorpses"] : new HudCheckBox();
				CorpseTrackerTrackFellowCorpses = view != null ? (HudCheckBox)view["CorpseTrackerTrackFellowCorpses"] : new HudCheckBox();
				CorpseTrackerTrackPermittedCorpses = view != null ? (HudCheckBox)view["CorpseTrackerTrackPermittedCorpses"] : new HudCheckBox();

				// Player Tracker
				PlayerTrackerListCurrent = view != null ? (HudList)view["PlayerTrackerListCurrent"] : new HudList();
				PlayerTrackerListPersistent = view != null ? (HudList)view["PlayerTrackerListPersistent"] : new HudList();

				// Player Tracker - Options
				PlayerTrackerClearCurrentStats = view != null ? (HudButton)view["PlayerTrackerClearCurrentStats"] : new HudButton();
				PlayerTrackerClearPersistentStats = view != null ? (HudButton)view["PlayerTrackerClearPersistentStats"] : new HudButton();

				PlayerTrackerEnabled = view != null ? (HudCheckBox)view["PlayerTrackerEnabled"] : new HudCheckBox();
				PlayerTrackerPersistent = view != null ? (HudCheckBox)view["PlayerTrackerPersistent"] : new HudCheckBox();

				// Consumable Tracker
				ConsumableTrackerList = view != null ? (HudList)view["ConsumableTrackerList"] : new HudList();


				// Tells Logger
				TellsLoggerListCurrent = view != null ? (HudList)view["TellsLoggerListCurrent"] : new HudList();
				TellsLoggerListPersistent = view != null ? (HudList)view["TellsLoggerListPersistent"] : new HudList();

				// Tells Logger - Options
				TellsLoggerClearCurrentLogs = view != null ? (HudButton)view["TellsLoggerClearCurrentLogs"] : new HudButton();
				TellsLoggerClearPersistentLogs = view != null ? (HudButton)view["TellsLoggerClearPersistentLogs"] : new HudButton();

				TellsLoggerEnabled = view != null ? (HudCheckBox)view["TellsLoggerEnabled"] : new HudCheckBox();
				TellsLoggerPersistent = view != null ? (HudCheckBox)view["TellsLoggerPersistent"] : new HudCheckBox();

				// Local Logger
				LocalLoggerListCurrent = view != null ? (HudList)view["LocalLoggerListCurrent"] : new HudList();
				LocalLoggerListPersistent = view != null ? (HudList)view["LocalLoggerListPersistent"] : new HudList();

				// Local Logger - Options
				LocalLoggerClearCurrentLogs = view != null ? (HudButton)view["LocalLoggerClearCurrentLogs"] : new HudButton();
				LocalLoggerClearPersistentLogs = view != null ? (HudButton)view["LocalLoggerClearPersistentLogs"] : new HudButton();

				LocalLoggerEnabled = view != null ? (HudCheckBox)view["LocalLoggerEnabled"] : new HudCheckBox();
				LocalLoggerPersistent = view != null ? (HudCheckBox)view["LocalLoggerPersistent"] : new HudCheckBox();

				// Fellow Logger
				FellowLoggerListCurrent = view != null ? (HudList)view["FellowLoggerListCurrent"] : new HudList();
				FellowLoggerListPersistent = view != null ? (HudList)view["FellowLoggerListPersistent"] : new HudList();

				// Fellow Logger - Options
				FellowLoggerClearCurrentLogs = view != null ? (HudButton)view["FellowLoggerClearCurrentLogs"] : new HudButton();
				FellowLoggerClearPersistentLogs = view != null ? (HudButton)view["FellowLoggerClearPersistentLogs"] : new HudButton();

				FellowLoggerEnabled = view != null ? (HudCheckBox)view["FellowLoggerEnabled"] : new HudCheckBox();
				FellowLoggerPersistent = view != null ? (HudCheckBox)view["FellowLoggerPersistent"] : new HudCheckBox();

				// Channels Logger
				ChannelsLoggerListCurrent = view != null ? (HudList)view["ChannelsLoggerListCurrent"] : new HudList();
				ChannelsLoggerListPersistent = view != null ? (HudList)view["ChannelsLoggerListPersistent"] : new HudList();

				// Channels Logger - Options
				ChannelsLoggerClearCurrentLogs = view != null ? (HudButton)view["ChannelsLoggerClearCurrentLogs"] : new HudButton();
				ChannelsLoggerClearPersistentLogs = view != null ? (HudButton)view["ChannelsLoggerClearPersistentLogs"] : new HudButton();

				ChannelsLoggerEnabled = view != null ? (HudCheckBox)view["ChannelsLoggerEnabled"] : new HudCheckBox();
				ChannelsLoggerPersistent = view != null ? (HudCheckBox)view["ChannelsLoggerPersistent"] : new HudCheckBox();
				ChannelsLoggerGeneral = view != null ? (HudCheckBox)view["ChannelsLoggerGeneral"] : new HudCheckBox();
				ChannelsLoggerTrade = view != null ? (HudCheckBox)view["ChannelsLoggerTrade"] : new HudCheckBox();
				ChannelsLoggerAllegiance = view != null ? (HudCheckBox)view["ChannelsLoggerAllegiance"] : new HudCheckBox();


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

				// Misc - Tools
				ClipboardWornEquipment = view != null ? (HudButton)view["ClipboardWornEquipment"] : new HudButton();
				ClipboardInventoryInfo = view != null ? (HudButton)view["ClipboardInventoryInfo"] : new HudButton();

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


				// Tells Logger
				TellsLoggerEnabled.Checked = Settings.SettingsManager.TellsLogger.Enabled.Value;
				Settings.SettingsManager.TellsLogger.Enabled.Changed += obj => { TellsLoggerEnabled.Checked = obj.Value; };
				TellsLoggerEnabled.Change += (s, e) => { try { Settings.SettingsManager.TellsLogger.Enabled.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				TellsLoggerPersistent.Checked = Settings.SettingsManager.TellsLogger.Persistent.Value;
				Settings.SettingsManager.TellsLogger.Persistent.Changed += obj => { TellsLoggerPersistent.Checked = obj.Value; };
				TellsLoggerPersistent.Change += (s, e) => { try { Settings.SettingsManager.TellsLogger.Persistent.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				// Local Logger
				LocalLoggerEnabled.Checked = Settings.SettingsManager.LocalLogger.Enabled.Value;
				Settings.SettingsManager.LocalLogger.Enabled.Changed += obj => { LocalLoggerEnabled.Checked = obj.Value; };
				LocalLoggerEnabled.Change += (s, e) => { try { Settings.SettingsManager.LocalLogger.Enabled.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				LocalLoggerPersistent.Checked = Settings.SettingsManager.LocalLogger.Persistent.Value;
				Settings.SettingsManager.LocalLogger.Persistent.Changed += obj => { LocalLoggerPersistent.Checked = obj.Value; };
				LocalLoggerPersistent.Change += (s, e) => { try { Settings.SettingsManager.LocalLogger.Persistent.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				// Fellow Logger
				FellowLoggerEnabled.Checked = Settings.SettingsManager.FellowLogger.Enabled.Value;
				Settings.SettingsManager.FellowLogger.Enabled.Changed += obj => { FellowLoggerEnabled.Checked = obj.Value; };
				FellowLoggerEnabled.Change += (s, e) => { try { Settings.SettingsManager.FellowLogger.Enabled.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				FellowLoggerPersistent.Checked = Settings.SettingsManager.FellowLogger.Persistent.Value;
				Settings.SettingsManager.FellowLogger.Persistent.Changed += obj => { FellowLoggerPersistent.Checked = obj.Value; };
				FellowLoggerPersistent.Change += (s, e) => { try { Settings.SettingsManager.FellowLogger.Persistent.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				// Channels Logger
				ChannelsLoggerEnabled.Checked = Settings.SettingsManager.ChannelsLogger.Enabled.Value;
				Settings.SettingsManager.ChannelsLogger.Enabled.Changed += obj => { ChannelsLoggerEnabled.Checked = obj.Value; };
				ChannelsLoggerEnabled.Change += (s, e) => { try { Settings.SettingsManager.ChannelsLogger.Enabled.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				ChannelsLoggerPersistent.Checked = Settings.SettingsManager.ChannelsLogger.Persistent.Value;
				Settings.SettingsManager.ChannelsLogger.Persistent.Changed += obj => { ChannelsLoggerPersistent.Checked = obj.Value; };
				ChannelsLoggerPersistent.Change += (s, e) => { try { Settings.SettingsManager.ChannelsLogger.Persistent.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				ChannelsLoggerGeneral.Checked = Settings.SettingsManager.ChannelsLogger.General.Value;
				Settings.SettingsManager.ChannelsLogger.General.Changed += obj => { ChannelsLoggerGeneral.Checked = obj.Value; };
				ChannelsLoggerGeneral.Change += (s, e) => { try { Settings.SettingsManager.ChannelsLogger.General.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				ChannelsLoggerTrade.Checked = Settings.SettingsManager.ChannelsLogger.Trade.Value;
				Settings.SettingsManager.ChannelsLogger.Trade.Changed += obj => { ChannelsLoggerTrade.Checked = obj.Value; };
				ChannelsLoggerTrade.Change += (s, e) => { try { Settings.SettingsManager.ChannelsLogger.Trade.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };

				ChannelsLoggerAllegiance.Checked = Settings.SettingsManager.ChannelsLogger.Allegiance.Value;
				Settings.SettingsManager.ChannelsLogger.Allegiance.Changed += obj => { ChannelsLoggerAllegiance.Checked = obj.Value; };
				ChannelsLoggerAllegiance.Change += (s, e) => { try { Settings.SettingsManager.ChannelsLogger.Allegiance.Value = ((HudCheckBox)s).Checked; } catch (Exception ex) { Debug.LogException(ex); } };


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

				AddOption(OptionList, Settings.SettingsManager.InventoryManagement.InventoryLogger);

				AddOption(OptionList, Settings.SettingsManager.Misc.OpenMainPackOnLogin);
				AddOption(OptionList, Settings.SettingsManager.Misc.MaximizeChatOnLogin);
				AddOption(OptionList, Settings.SettingsManager.Misc.DebuggingEnabled);

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
						Settings.SettingsManager.Misc.NoFocusFPS.Value = value;
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

		void AddOption(HudList hudList, Settings.Setting<bool> setting)
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
