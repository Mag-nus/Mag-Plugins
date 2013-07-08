using System;
using System.IO;
using System.Collections.ObjectModel;

using Decal.Adapter;

using MagTools.Client;
using MagTools.Inventory;
using MagTools.Trackers.Equipment;
using MagTools.Views;

using Mag.Shared;

/*
 * Created by Mag-nus. 8/19/2011
 * 
 * No license applied, feel free to use as you wish. H4CK TH3 PL4N3T? TR45H1NG 0UR R1GHT5? Y0U D3C1D3!
 * 
 * Notice how I use try/catch on every function that is called or raised by decal (by base events or user initiated events like buttons, etc...).
 * This is very important. Don't crash out your users!
 * 
 * In 2.9.6.4+ Host and Core both have Actions objects in them. They are essentially the same thing.
 * You sould use Host.Actions though so that your code compiles against 2.9.6.0 (even though I reference 2.9.6.5 in this project)
 * 
 * If you add this plugin to decal and then also create another plugin off of this sample, you will need to change the guid in
 * Properties/AssemblyInfo.cs to have both plugins in decal at the same time.
 * 
 * If you have issues compiling, remove the Decal.Adapater and VirindiViewService references and add the ones you have locally.
 * Decal.Adapter should be in C:\Games\Decal 3.0\
 * VirindiViewService should be in C:\Games\VirindiPlugins\VirindiViewService\
*/

/*
 * The design of this plugin is as follows.
 * 
 * Settings:
 * Settings and configuration options that the user can set, or the plugin sets, that may or may not go into a config file are located in Settings.SettingsManager.cs
 * 
 * GUI:
 * Currently, only VVS is supported.
 * The main GUI xml, and the VVS loader/parser of that xml is located in Views.mainView.xml and Views.MainView.cs
 * Any interaction with the VVS should go into the Views folder/namespace.
 * The goal here is to isolate views from the actual plugin functionality so we can run with views disabled.
 * 
 * Design based on Dependancy:
 * 
 * [CorePluginObjects] -> [SettingsManager] -> [SettingsFile]
 *         ^                     ^
 *         |                     |
 *          ------------------[Views]
 * 
 * CorePluginObjects depends on SettingsManager, which depends on SettingsFile.
 * Views depend on CorePluginObjects and SettingsManager.
*/

namespace MagTools
{
	// FriendlyName is the name that will show up in the plugins list of the decal agent (the one in windows, not in-game)
	[FriendlyName("Mag-Tools")]
	public sealed class PluginCore : PluginBase, IPluginCore
	{
		/// <summary>
		/// Returns the current instance of the plugin in Decal. If the plugin hasn't been loaded yet this will return null.
		/// </summary>
		public static IPluginCore Current { get; private set; }

		internal static string PluginName = "Mag-Tools";

		internal static DirectoryInfo PluginPersonalFolder
		{
			get
			{
				DirectoryInfo pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\" + PluginName);

				try
				{
					if (!pluginPersonalFolder.Exists)
						pluginPersonalFolder.Create();
				}
				catch (Exception ex) { Debug.LogException(ex); }

				return pluginPersonalFolder;
			}
		}

		// General
		ChatFilter chatFilter;
		InventoryExporter inventoryExporter;
		InventoryLogger inventoryLogger;

		// Macros
		Macros.OpenMainPackOnLogin openMainPackOnLogin;
		Macros.MaximizeChatOnLogin maximizeChatOnLogin;
		Macros.AutoRecharge autoRecharge;
		Macros.AutoTradeAccept autoTradeAccept;
		Macros.OneTouchHeal oneTouchHeal;
	
		// Trackers
		EquipmentTracker equipmentTracker;
		public IEquipmentTracker EquipmentTracker { get { return equipmentTracker; } }
		Trackers.Combat.CombatTracker combatTrackerCurrent;
		Trackers.Combat.CombatTracker combatTrackerPersistent;
		Trackers.Corpse.CorpseTracker corpseTrackerCurrent;
		Trackers.Corpse.CorpseTracker corpseTrackerPersistent;
		Trackers.Player.PlayerTracker playerTrackerCurrent;
		Trackers.Player.PlayerTracker playerTrackerPersistent;
		Trackers.Consumable.ConsumableTracker consumableTracker;

		// Loggers
		Loggers.Chat.ChatLogger chatLogger;

		// Misc
		WindowFrameRemover windowFrameRemover;
		WindowMover windowMover;
		NoFocusFPSManager noFocusFPSManager;

		// These objects may reference other plugins
		ItemInfo.ItemInfoPrinter itemInfoPrinter;

		// Virindi Classic Looter Extensions, depends on VTClassic.dll
		Macros.InventoryPacker inventoryPacker;
		public Macros.IInventoryPacker InventoryPacker { get { return inventoryPacker; } }
		Macros.AutoTradeAdd autoTradeAdd;
		Macros.AutoBuySell autoBuySell;

		// Virindi Tank Extensions, depends on utank2-i.dll
		Macros.Looter looter;
		public Macros.ILooter Looter { get { return looter; } }

		// Views, depends on VirindiViewService.dll
		MainView mainView;

		ManaTrackerGUI manaTrackerGUI;
		CombatTrackerGUI combatTrackerGUICurrent;
		CombatTrackerGUI combatTrackerGUIPersistent;
		CorpseTrackerGUI corpseTrackerGUICurrent;
		CorpseTrackerGUI corpseTrackerGUIPersistent;
		PlayerTrackerGUI playerTrackerGUICurrent;
		PlayerTrackerGUI playerTrackerGUIPersistent;
		ConsumableTrackerGUI consumableTrackerGUI;

		ChatLoggerGUI chatLoggerTellsGUICurrent;
		ChatLoggerGUI chatLoggerTellsGUIPersistent;
		ChatLoggerGUI chatLoggerLocalGUICurrent;
		ChatLoggerGUI chatLoggerLocalGUIPersistent;
		ChatLoggerGUI chatLoggerFellowGUICurrent;
		ChatLoggerGUI chatLoggerFellowGUIPersistent;
		ChatLoggerGUI chatLoggerChannelsGUICurrent;
		ChatLoggerGUI chatLoggerChannelsGUIPersistent;

		HUD hud;

		readonly Collection<string> startupErrors = new Collection<string>();
		readonly System.Windows.Forms.Timer savePersistentStatsTimer = new System.Windows.Forms.Timer();

		/// <summary>
		/// This is called when the plugin is started up. This happens only once.
		/// We init most of our objects here, EXCEPT ones that depend on other assemblies (not counting decal assemblies).
		/// </summary>
		protected override void Startup()
		{
			try
			{
				Current = this;

				CoreManager.Current.PluginInitComplete += new EventHandler<EventArgs>(Current_PluginInitComplete);
				CoreManager.Current.CharacterFilter.LoginComplete +=new EventHandler(CharacterFilter_LoginComplete);
				CoreManager.Current.CharacterFilter.Logoff += new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);

				// General
				inventoryExporter = new InventoryExporter();
				inventoryLogger = new InventoryLogger();

				// Macros
				openMainPackOnLogin = new Macros.OpenMainPackOnLogin();
				maximizeChatOnLogin = new Macros.MaximizeChatOnLogin();
				autoRecharge = new Macros.AutoRecharge();
				autoTradeAccept = new Macros.AutoTradeAccept();
				oneTouchHeal = new Macros.OneTouchHeal();

				// Trackers
				equipmentTracker = new EquipmentTracker();
				combatTrackerCurrent = new Trackers.Combat.CombatTracker();
				combatTrackerPersistent = new Trackers.Combat.CombatTracker();
				corpseTrackerCurrent = new Trackers.Corpse.CorpseTracker();
				corpseTrackerPersistent = new Trackers.Corpse.CorpseTracker(true);
				playerTrackerCurrent = new Trackers.Player.PlayerTracker();
				playerTrackerPersistent = new Trackers.Player.PlayerTracker();
				consumableTracker = new Trackers.Consumable.ConsumableTracker();

				// Loggers
				chatLogger = new Loggers.Chat.ChatLogger();

				// Misc
				windowFrameRemover = new WindowFrameRemover();
				windowMover = new WindowMover();
				noFocusFPSManager = new NoFocusFPSManager();

				savePersistentStatsTimer.Interval = 60000; // Set the timer to run once every hour
				savePersistentStatsTimer.Tick += new EventHandler(SavePersistentStatsTimer_Tick);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		/// <summary>
		/// We init objects that depend on other assemblies here.
		/// We don't do this in Startup() because our plugin may have loaded before theirs.
		/// It is also possible that the assembly these objects refer to isn't loaded at all, or may not even exist.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Current_PluginInitComplete(object sender, EventArgs e)
		{
			try
			{
				// Relies on other decal assemblies
				try
				{
					chatFilter = new ChatFilter(Host); // Decal.Interop.Core
				}
				catch (FileNotFoundException ex) { startupErrors.Add("chatFilter failed to load: " + ex.Message); }
				catch (Exception ex) { Debug.LogException(ex); }

				// These objects may reference other plugins
				try
				{
					itemInfoPrinter = new ItemInfo.ItemInfoPrinter();
				}
				catch (FileNotFoundException ex) { startupErrors.Add("itemInfoPrinter failed to load: " + ex.Message); }
				catch (Exception ex) { Debug.LogException(ex); }

				// Virindi Classic Looter Extensions, depends on VTClassic.dll
				string objectName = null;
				try
				{
					objectName = "inventoryPacker";
					inventoryPacker = new Macros.InventoryPacker();
					objectName = "autoTradeAdd";
					autoTradeAdd = new Macros.AutoTradeAdd();
					objectName = "autoBuySell";
					autoBuySell = new Macros.AutoBuySell();
				}
				catch (FileNotFoundException ex) { startupErrors.Add(objectName + " failed to load: " + ex.Message + Environment.NewLine + "Is Virindi Tank running?"); }
				catch (Exception ex) { Debug.LogException(ex); }

				// Virindi Tank Extensions, depends on utank2-i.dll
				try
				{
					looter = new Macros.Looter();
				}
				catch (FileNotFoundException ex) { startupErrors.Add("looter failed to load: " + ex.Message + Environment.NewLine + "Is Virindi Tank running?"); }
				catch (Exception ex) { Debug.LogException(ex); }

				// Views, depends on VirindiViewService.dll
				try
				{
					mainView = new MainView();

					manaTrackerGUI = new ManaTrackerGUI(equipmentTracker, mainView);
					combatTrackerGUICurrent = new CombatTrackerGUI(combatTrackerCurrent, mainView.CombatTrackerMonsterListCurrent, mainView.CombatTrackerDamageListCurrent);
					combatTrackerGUIPersistent = new CombatTrackerGUI(combatTrackerPersistent, mainView.CombatTrackerMonsterListPersistent, mainView.CombatTrackerDamageListPersistent);
					corpseTrackerGUICurrent = new CorpseTrackerGUI(corpseTrackerCurrent, mainView.CorpseTrackerListCurrent);
					corpseTrackerGUIPersistent = new CorpseTrackerGUI(corpseTrackerPersistent, mainView.CorpseTrackerListPersistent);
					playerTrackerGUICurrent = new PlayerTrackerGUI(playerTrackerCurrent, mainView.PlayerTrackerListCurrent);
					playerTrackerGUIPersistent = new PlayerTrackerGUI(playerTrackerPersistent, mainView.PlayerTrackerListPersistent);
					consumableTrackerGUI = new ConsumableTrackerGUI(consumableTracker, mainView.ConsumableTrackerList);

					chatLoggerTellsGUICurrent = new ChatLoggerGUI(chatLogger, Util.ChatChannels.Tells, mainView.TellsLoggerListCurrent);
					chatLoggerTellsGUIPersistent = new ChatLoggerGUI(chatLogger, Util.ChatChannels.Tells, mainView.TellsLoggerListPersistent);
					chatLoggerLocalGUICurrent = new ChatLoggerGUI(chatLogger, Util.ChatChannels.Area, mainView.LocalLoggerListCurrent);
					chatLoggerLocalGUIPersistent = new ChatLoggerGUI(chatLogger, Util.ChatChannels.Area, mainView.LocalLoggerListPersistent);
					chatLoggerFellowGUICurrent = new ChatLoggerGUI(chatLogger, Util.ChatChannels.Fellowship, mainView.FellowLoggerListCurrent);
					chatLoggerFellowGUIPersistent = new ChatLoggerGUI(chatLogger, Util.ChatChannels.Fellowship, mainView.FellowLoggerListPersistent);
					chatLoggerChannelsGUICurrent = new ChatLoggerGUI(chatLogger, Util.ChatChannels.General | Util.ChatChannels.Trade | Util.ChatChannels.Allegiance, mainView.ChannelsLoggerListCurrent);
					chatLoggerChannelsGUIPersistent = new ChatLoggerGUI(chatLogger, Util.ChatChannels.General | Util.ChatChannels.Trade | Util.ChatChannels.Allegiance, mainView.ChannelsLoggerListPersistent);

					mainView.CombatTrackerClearCurrentStats.Hit += (s2, e2) => { try { combatTrackerCurrent.ClearStats(); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.CombatTrackerExportCurrentStats.Hit += (s2, e2) => { try { combatTrackerCurrent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker." + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".xml", true); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.CombatTrackerClearPersistentStats.Hit += new EventHandler(CombatTrackerClearPersistentStats_Hit);

					mainView.CorpseTrackerClearCurrentStats.Hit += (s2, e2) => { try { corpseTrackerCurrent.ClearStats(); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.CorpseTrackerClearPersistentStats.Hit += new EventHandler(CorpseTrackerClearPersistentStats_Hit);

					mainView.PlayerTrackerClearCurrentStats.Hit += (s2, e2) => { try { playerTrackerCurrent.ClearStats(); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.PlayerTrackerClearPersistentStats.Hit += new EventHandler(PlayerTrackerClearPersistentStats_Hit);

					//mainView.TellsLoggerClearCurrentLogs.Hit += (s2, e2) => { try { chatLoggerTellsCurrent.clea(); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.TellsLoggerClearPersistentLogs.Hit += new EventHandler(TellsLoggerClearPersistentLogs_Hit);

					//mainView.TellsLoggerClearCurrentLogs.Hit += (s2, e2) => { try { chatLoggerTellsCurrent.clea(); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.LocalLoggerClearPersistentLogs.Hit += new EventHandler(LocalLoggerClearPersistentLogs_Hit);

					//mainView.TellsLoggerClearCurrentLogs.Hit += (s2, e2) => { try { chatLoggerTellsCurrent.clea(); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.FellowLoggerClearPersistentLogs.Hit += new EventHandler(FellowLoggerClearPersistentLogs_Hit);

					//mainView.TellsLoggerClearCurrentLogs.Hit += (s2, e2) => { try { chatLoggerTellsCurrent.clea(); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.ChannelsLoggerClearPersistentLogs.Hit += new EventHandler(ChannelsLoggerClearPersistentLogs_Hit);

					mainView.ClipboardWornEquipment.Hit += (s2, e2) => { try { inventoryExporter.ExportToClipboard(InventoryExporter.ExportGroups.WornEquipment); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.ClipboardInventoryInfo.Hit += (s2, e2) => { try { inventoryExporter.ExportToClipboard(InventoryExporter.ExportGroups.Inventory); } catch (Exception ex) { Debug.LogException(ex); } };
					
					System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
					System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
					mainView.VersionLabel.Text = "Version: " + fvi.ProductVersion;

					hud = new HUD(equipmentTracker);
				}
				catch (FileNotFoundException ex) { startupErrors.Add("Views failed to load: " + ex.Message + Environment.NewLine + "Is Virindi View Service Running?"); }
				catch (Exception ex) { Debug.LogException(ex); }
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		/// <summary>
		/// This is called when the plugin is shut down. This happens only once.
		/// </summary>
		protected override void Shutdown()
		{
			try
			{
				savePersistentStatsTimer.Tick -= new EventHandler(SavePersistentStatsTimer_Tick);

				CoreManager.Current.PluginInitComplete -= new EventHandler<EventArgs>(Current_PluginInitComplete);
				CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
				CoreManager.Current.CharacterFilter.Logoff -= new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);

				// Views, depends on VirindiViewService.dll
				// We dispose these before our other objects (Trackers/Macros) as these probably reference those other objects.
				if (hud != null) hud.Dispose();

				if (chatLoggerTellsGUICurrent != null) chatLoggerTellsGUICurrent.Dispose();
				if (chatLoggerTellsGUIPersistent != null) chatLoggerTellsGUIPersistent.Dispose();
				if (chatLoggerLocalGUICurrent != null) chatLoggerLocalGUICurrent.Dispose();
				if (chatLoggerLocalGUIPersistent != null) chatLoggerLocalGUIPersistent.Dispose();
				if (chatLoggerFellowGUICurrent != null) chatLoggerFellowGUICurrent.Dispose();
				if (chatLoggerFellowGUIPersistent != null) chatLoggerFellowGUIPersistent.Dispose();
				if (chatLoggerChannelsGUICurrent != null) chatLoggerChannelsGUICurrent.Dispose();
				if (chatLoggerChannelsGUIPersistent != null) chatLoggerChannelsGUIPersistent.Dispose();

				if (consumableTrackerGUI != null) consumableTrackerGUI.Dispose();
				if (playerTrackerGUIPersistent != null) playerTrackerGUIPersistent.Dispose();
				if (playerTrackerGUICurrent != null) playerTrackerGUICurrent.Dispose();
				if (corpseTrackerGUICurrent != null) corpseTrackerGUICurrent.Dispose();
				if (corpseTrackerGUIPersistent != null) corpseTrackerGUIPersistent.Dispose();
				if (combatTrackerGUIPersistent != null) combatTrackerGUIPersistent.Dispose();
				if (combatTrackerGUICurrent != null) combatTrackerGUICurrent.Dispose();
				if (manaTrackerGUI != null) manaTrackerGUI.Dispose();
				if (mainView != null) mainView.Dispose(); // We dispose this last in the Views as the other Views reference it.

				// These objects may reference other plugins
				if (itemInfoPrinter != null) itemInfoPrinter.Dispose();

				// Virindi Classic Looter Extensions, depends on VTClassic.dll
				if (inventoryPacker != null) inventoryPacker.Dispose();
				if (autoTradeAdd != null) autoTradeAdd.Dispose();
				if (autoBuySell != null) autoBuySell.Dispose();

				// Virindi Tank Extensions, depends on utank2-i.dll
				if (looter != null) looter.Dispose();

				// Misc
				if (windowFrameRemover != null) windowFrameRemover.Dispose();
				if (windowMover != null) windowMover.Dispose();
				if (noFocusFPSManager != null) noFocusFPSManager.Dispose();

				// Loggers
				if (chatLogger != null) chatLogger.Dispose();

				// Trackers
				if (equipmentTracker != null) equipmentTracker.Dispose();
				if (combatTrackerCurrent != null) combatTrackerCurrent.Dispose();
				if (combatTrackerPersistent != null) combatTrackerPersistent.Dispose();
				if (corpseTrackerCurrent != null) corpseTrackerCurrent.Dispose();
				if (corpseTrackerPersistent != null) corpseTrackerPersistent.Dispose();
				if (playerTrackerCurrent != null) playerTrackerCurrent.Dispose();
				if (playerTrackerPersistent != null) playerTrackerPersistent.Dispose();
				if (consumableTracker != null) consumableTracker.Dispose();

				// Macros
				if (openMainPackOnLogin != null) openMainPackOnLogin.Dispose();
				if (maximizeChatOnLogin != null) maximizeChatOnLogin.Dispose();
				if (autoRecharge != null) autoRecharge.Dispose();
				if (autoTradeAccept != null) autoTradeAccept.Dispose();

				// General
				if (chatFilter != null) chatFilter.Dispose();
				if (inventoryLogger != null) inventoryLogger.Dispose();

				Current = null;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				// Load Persistent Stats
				try
				{
					if (Settings.SettingsManager.CombatTracker.Persistent.Value)
						combatTrackerPersistent.ImportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker.xml");
				}
				catch (Exception ex) { Debug.LogException(ex); }

				try
				{
					if (Settings.SettingsManager.CorpseTracker.Persistent.Value)
						corpseTrackerPersistent.ImportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CorpseTracker.xml");
				}
				catch (Exception ex) { Debug.LogException(ex); }

				try
				{
					if (Settings.SettingsManager.PlayerTracker.Persistent.Value)
						playerTrackerPersistent.ImportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".PlayerTracker.xml");
				}
				catch (Exception ex) { Debug.LogException(ex); }


				// Load Persistent Logs
				/*try
				{
					if (Settings.SettingsManager.TellsLogger.Persistent.Value)
						chatLoggerTellsPersistent.ImportLogs(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".TellsLogger.xml");
				}
				catch (Exception ex) { Debug.LogException(ex); }

				try
				{
					if (Settings.SettingsManager.LocalLogger.Persistent.Value)
						chatLoggerLocalPersistent.ImportLogs(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".LocalLogger.xml");
				}
				catch (Exception ex) { Debug.LogException(ex); }

				try
				{
					if (Settings.SettingsManager.FellowLogger.Persistent.Value)
						chatLoggerFellowPersistent.ImportLogs(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".FellowLogger.xml");
				}
				catch (Exception ex) { Debug.LogException(ex); }

				try
				{
					if (Settings.SettingsManager.ChannelsLogger.Persistent.Value)
						chatLoggerChannelsPersistent.ImportLogs(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".ChannelsLogger.xml");
				}
				catch (Exception ex) { Debug.LogException(ex); }*/


				// Wire up Inventory Packer Hotkey
				try
				{
					if (InventoryPacker != null)
					{
						// http://delphi.about.com/od/objectpascalide/l/blvkc.htm
						VirindiHotkeySystem.VHotkeyInfo packInventoryHotkey = new VirindiHotkeySystem.VHotkeyInfo("Mag-Tools", true, "Pack Inventory", "Triggers the Inventory Packer Macro", 0x50, false, true, false);

						VirindiHotkeySystem.VHotkeySystem.InstanceReal.AddHotkey(packInventoryHotkey);

						packInventoryHotkey.Fired2 += (s, e2) =>
							{
								try
								{
									VirindiHotkeySystem.VHotkeyInfo keyInfo = (VirindiHotkeySystem.VHotkeyInfo)s;

									if (!CoreManager.Current.Actions.ChatState || keyInfo.AltState || keyInfo.ControlState)
										InventoryPacker.Start();
								}
								catch (FileNotFoundException) { CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: " + "Unable to start Inventory Packer. Is Virindi Tank running?", 5); }
								catch (Exception ex) { Debug.LogException(ex); }
							};
					} 
				}
				catch (FileNotFoundException ex) { startupErrors.Add("Pack Inventory hot key failed to bind: " + ex.Message + ". Is Virindi Hotkey System running?"); }
				catch (Exception ex) { Debug.LogException(ex); }

				// Wire up One Touch Heal Hotkey
				try
				{
					if (oneTouchHeal != null)
					{
						// http://delphi.about.com/od/objectpascalide/l/blvkc.htm
						VirindiHotkeySystem.VHotkeyInfo oneTouchHealHotkey = new VirindiHotkeySystem.VHotkeyInfo("Mag-Tools", true, "One Touch Heal", "Triggers the One Touch Healing Macro", 0, false, false, false);

						VirindiHotkeySystem.VHotkeySystem.InstanceReal.AddHotkey(oneTouchHealHotkey);

						oneTouchHealHotkey.Fired2 += (s, e2) =>
						{
							try
							{
								VirindiHotkeySystem.VHotkeyInfo keyInfo = (VirindiHotkeySystem.VHotkeyInfo)s;

								if (!CoreManager.Current.Actions.ChatState || keyInfo.AltState || keyInfo.ControlState)
									oneTouchHeal.Start();
							}
							catch (Exception ex) { Debug.LogException(ex); }
						};
					}
				}
				catch (FileNotFoundException ex) { startupErrors.Add("One Touch Heal hot key failed to bind: " + ex.Message + ". Is Virindi Hotkey System running?"); }
				catch (Exception ex) { Debug.LogException(ex); }

				// Wire up Maximize/Minimize Chat Hotkey
				try
				{
						// http://delphi.about.com/od/objectpascalide/l/blvkc.htm
						VirindiHotkeySystem.VHotkeyInfo maximizeChat = new VirindiHotkeySystem.VHotkeyInfo("Mag-Tools", true, "Maximize Chat", "Maximizes Main Chat", 0, false, false, false);

						VirindiHotkeySystem.VHotkeySystem.InstanceReal.AddHotkey(maximizeChat);

						maximizeChat.Fired2 += (s, e2) =>
						{
							try
							{
								ChatSizeManager.Maximize();
							}
							catch (Exception ex) { Debug.LogException(ex); }
						};

						// http://delphi.about.com/od/objectpascalide/l/blvkc.htm
						VirindiHotkeySystem.VHotkeyInfo minimizeChat = new VirindiHotkeySystem.VHotkeyInfo("Mag-Tools", true, "Minimize Chat", "Minimizes Main Chat", 0, false, false, false);

						VirindiHotkeySystem.VHotkeySystem.InstanceReal.AddHotkey(minimizeChat);

						minimizeChat.Fired2 += (s, e2) =>
						{
							try
							{
								ChatSizeManager.Minimize();
							}
							catch (Exception ex) { Debug.LogException(ex); }
						};
				}
				catch (FileNotFoundException ex) { startupErrors.Add("AC Chat minimize/maximize hot key failed to bind: " + ex.Message + ". Is Virindi Hotkey System running?"); }
				catch (Exception ex) { Debug.LogException(ex); }

				foreach (string startupError in startupErrors)
				{
					CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: Startup Error: " + startupError, 5);
				}

				startupErrors.Clear();

				CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: " + "Plugin now online. Server population: " + Core.CharacterFilter.ServerPopulation, 5);

				savePersistentStatsTimer.Start();

				//Util.ExportSpells(PluginPersonalFolder.FullName + @"\spells.csv");
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_Logoff(object sender, Decal.Adapter.Wrappers.LogoffEventArgs e)
		{
			try
			{
				savePersistentStatsTimer.Stop();

				ExportPersistentStats();

				ExportPersistentLogs();

				if (Settings.SettingsManager.CombatTracker.ExportOnLogOff.Value)
					combatTrackerCurrent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker." + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".xml");
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		#region ' Clear Persistent Stats Buttons '

		void CombatTrackerClearPersistentStats_Hit(object sender, EventArgs e)
		{
			try
			{
				combatTrackerPersistent.ClearStats();

				FileInfo fileInfo = new FileInfo(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker.xml");

				if (fileInfo.Exists)
				{
					fileInfo.Delete();

					CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: " + "File deleted: " + fileInfo.FullName, 5);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CorpseTrackerClearPersistentStats_Hit(object sender, EventArgs e)
		{
			try
			{
				corpseTrackerPersistent.ClearStats();

				FileInfo fileInfo = new FileInfo(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CorpseTracker.xml");

				if (fileInfo.Exists)
				{
					fileInfo.Delete();

					CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: " + "File deleted: " + fileInfo.FullName, 5);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void PlayerTrackerClearPersistentStats_Hit(object sender, EventArgs e)
		{
			try
			{
				playerTrackerPersistent.ClearStats();

				FileInfo fileInfo = new FileInfo(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".PlayerTracker.xml");

				if (fileInfo.Exists)
				{
					fileInfo.Delete();

					CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: " + "File deleted: " + fileInfo.FullName, 5);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		#endregion

		#region ' Clear Persistent Logs Buttons '

		void TellsLoggerClearPersistentLogs_Hit(object sender, EventArgs e)
		{

		}

		void LocalLoggerClearPersistentLogs_Hit(object sender, EventArgs e)
		{

		}

		void FellowLoggerClearPersistentLogs_Hit(object sender, EventArgs e)
		{

		}

		void ChannelsLoggerClearPersistentLogs_Hit(object sender, EventArgs e)
		{

		}

		#endregion

		void SavePersistentStatsTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				ExportPersistentStats();

				ExportPersistentLogs();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void ExportPersistentStats()
		{
			if (Settings.SettingsManager.CombatTracker.Persistent.Value)
				combatTrackerPersistent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker.xml");

			if (Settings.SettingsManager.CorpseTracker.Persistent.Value)
				corpseTrackerPersistent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CorpseTracker.xml");

			if (Settings.SettingsManager.PlayerTracker.Persistent.Value)
				playerTrackerPersistent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".PlayerTracker.xml");
		}

		void ExportPersistentLogs()
		{
			/*if (Settings.SettingsManager.TellsLogger.Persistent.Value)
				chatLoggerTellsPersistent.ExportLogs(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".TellsLogger.xml");

			if (Settings.SettingsManager.LocalLogger.Persistent.Value)
				chatLoggerLocalPersistent.ExportLogs(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".LocalLogger.xml");

			if (Settings.SettingsManager.FellowLogger.Persistent.Value)
				chatLoggerFellowPersistent.ExportLogs(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".FellowLogger.xml");

			if (Settings.SettingsManager.ChannelsLogger.Persistent.Value)
				chatLoggerChannelsPersistent.ExportLogs(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".ChannelsLogger.xml");*/
		}
	}
}
