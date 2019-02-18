using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Collections.ObjectModel;
using System.Reflection;

using MagTools.Client;
using MagTools.Inventory;
using MagTools.Loggers;
using MagTools.Macros;
using MagTools.Views;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;
using Decal.Filters;

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
		InventoryExporter inventoryExporter;
		InventoryLogger inventoryLogger;
		IdleActionManager idleActionManager;

		// Macros
		LoginActions loginActions;
		PeriodicCommands periodicCommands;
		OpenMainPackOnLogin openMainPackOnLogin;
		MaximizeChatOnLogin maximizeChatOnLogin;
		AutoPercentConfirmation autoPercentConfirmation;
		AutoRecharge autoRecharge;
		AutoTradeAccept autoTradeAccept;
		OneTouchHeal oneTouchHeal;
		LogOutOnDeath logOutOnDeath;
	
		// Trackers
		Trackers.Equipment.EquipmentTracker equipmentTracker;
		public Trackers.Equipment.IEquipmentTracker EquipmentTracker { get { return equipmentTracker; } }
		Trackers.Combat.CombatTracker combatTrackerCurrent;
		Trackers.Combat.CombatTracker combatTrackerPersistent;
		Trackers.Corpse.CorpseTracker corpseTracker;
		Trackers.Player.PlayerTracker playerTracker;
		Trackers.Inventory.InventoryTracker inventoryTracker;
		Trackers.ProfitLoss.ProfitLossTracker profitLossTracker;

		// Loggers
		Loggers.Chat.ChatLogger chatLogger;
		Loggers.Chat.BufferedChatLogFileWriter chatLogFileWriter;

		// Misc
		WindowFrameRemover windowFrameRemover;
		WindowMover windowMover;
		FPSManager fpsManager;


		// Relies on other decal assemblies
		ChatFilter chatFilter;


		// Virindi Classic Looter Extensions, depends on VTClassic.dll
		InventoryPacker inventoryPacker;
		public IInventoryPacker InventoryPacker { get { return inventoryPacker; } }
		AutoTradeAdd autoTradeAdd;
		AutoBuySell autoBuySell;


		// Virindi Tank Extensions, depends on utank2-i.dll
		ItemInfo.ItemInfoPrinter itemInfoPrinter;
		Looter looter;
		public ILooter Looter { get { return looter; } }


		// Views, depends on VirindiViewService.dll
		MainView mainView;

		ManaTrackerGUI manaTrackerGUI;
		CombatTrackerGUI combatTrackerGUICurrent;
		CombatTrackerGUI combatTrackerGUIPersistent;
		CorpseTrackerGUI corpseTrackerGUI;
		PlayerTrackerGUI playerTrackerGUI;
		InventoryTrackerGUI inventoryTrackerGUI;

		ChatLoggerGUI chatLoggerGroup1GUI;
		ChatLoggerGUI chatLoggerGroup2GUI;

		InventoryToolsView inventoryToolsView;
		TinkeringToolsView tinkeringToolsView;

		AccountServerCharacterGUI accountServerCharacterGUI;
		ServerGUI serverGUI;

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

				Debug.Init(PluginPersonalFolder.FullName + @"\Exceptions.txt", PluginName);
				Mag.Shared.Settings.SettingsFile.Init(PluginPersonalFolder.FullName + @"\" + PluginName + ".xml", PluginName);

				CoreManager.Current.PluginInitComplete += new EventHandler<EventArgs>(Current_PluginInitComplete);
				CoreManager.Current.PluginInitComplete += new EventHandler<EventArgs>(Current_PluginInitComplete_VTClassic);
				CoreManager.Current.PluginInitComplete += new EventHandler<EventArgs>(Current_PluginInitComplete_VTank);
				CoreManager.Current.PluginInitComplete += new EventHandler<EventArgs>(Current_PluginInitComplete_VVS);
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete_VHS);
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete_VHUD);
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
				CoreManager.Current.CharacterFilter.Logoff += new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);
				CoreManager.Current.CommandLineText += new EventHandler<ChatParserInterceptEventArgs>(Current_CommandLineText);


				// General
				inventoryExporter = new InventoryExporter();
				inventoryLogger = new InventoryLogger();
				idleActionManager = new IdleActionManager();

				// Macros
				loginActions = new LoginActions();
				periodicCommands = new PeriodicCommands();
				openMainPackOnLogin = new OpenMainPackOnLogin();
				maximizeChatOnLogin = new MaximizeChatOnLogin();
				autoPercentConfirmation = new AutoPercentConfirmation();
				autoRecharge = new AutoRecharge();
				autoTradeAccept = new AutoTradeAccept();
				oneTouchHeal = new OneTouchHeal();
				logOutOnDeath = new LogOutOnDeath();

				// Trackers
				equipmentTracker = new Trackers.Equipment.EquipmentTracker();
				combatTrackerCurrent = new Trackers.Combat.CombatTracker();
				combatTrackerPersistent = new Trackers.Combat.CombatTracker();
				corpseTracker = new Trackers.Corpse.CorpseTracker();
				playerTracker = new Trackers.Player.PlayerTracker();
				inventoryTracker = new Trackers.Inventory.InventoryTracker();
				profitLossTracker = new Trackers.ProfitLoss.ProfitLossTracker();

				// Loggers
				chatLogger = new Loggers.Chat.ChatLogger();
				chatLogFileWriter = new Loggers.Chat.BufferedChatLogFileWriter(null, chatLogger, TimeSpan.FromMinutes(10));

				// Misc
				windowFrameRemover = new WindowFrameRemover();
				windowMover = new WindowMover();
				fpsManager = new FPSManager();


				savePersistentStatsTimer.Interval = 600000; // Set the timer to run once every 10 minutes
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

				//These are already wrapped and shouldn't throw.
				MyClasses.VCS_Connector.Initialize(Host, "MagTools");
				MyClasses.VCS_Connector.InitializeCategory("CommandLine", "Generic plugin text");
				MyClasses.VCS_Connector.InitializeCategory("Errors", "Error messages");
				MyClasses.VCS_Connector.InitializeCategory("IDs", "ID messages");
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Current_PluginInitComplete_VTClassic(object sender, EventArgs e)
		{
			try
			{
				// Virindi Classic Looter Extensions, depends on VTClassic.dll
				string objectName = null;
				try
				{
					objectName = "inventoryPacker";		inventoryPacker = new InventoryPacker();
					objectName = "autoTradeAdd";		autoTradeAdd = new AutoTradeAdd();
					objectName = "autoBuySell";			autoBuySell = new AutoBuySell();
				}
				catch (FileNotFoundException ex) { startupErrors.Add(objectName + " failed to load: " + ex.Message + Environment.NewLine + "Is Virindi Tank running?"); }
				catch (Exception ex) { Debug.LogException(ex); }
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Current_PluginInitComplete_VTank(object sender, EventArgs e)
		{
			try
			{
				// Virindi Tank Extensions, depends on utank2-i.dll
				try
				{
					itemInfoPrinter = new ItemInfo.ItemInfoPrinter();
				}
				catch (FileNotFoundException ex) { startupErrors.Add("itemInfoPrinter failed to load: " + ex.Message); }
				catch (Exception ex) { Debug.LogException(ex); }

				try
				{
					looter = new Looter();
				}
				catch (FileNotFoundException ex) { startupErrors.Add("looter failed to load: " + ex.Message + Environment.NewLine + "Is Virindi Tank running?"); }
				catch (Exception ex) { Debug.LogException(ex); }
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Current_PluginInitComplete_VVS(object sender, EventArgs e)
		{
			try
			{
				// Views, depends on VirindiViewService.dll
				try
				{
					mainView = new MainView();

					manaTrackerGUI = new ManaTrackerGUI(equipmentTracker, mainView);
					combatTrackerGUICurrent = new CombatTrackerGUI(combatTrackerCurrent, mainView.CombatTrackerMonsterListCurrent, mainView.CombatTrackerDamageListCurrent);
					combatTrackerGUIPersistent = new CombatTrackerGUI(combatTrackerPersistent, mainView.CombatTrackerMonsterListPersistent, mainView.CombatTrackerDamageListPersistent);
					corpseTrackerGUI = new CorpseTrackerGUI(corpseTracker, mainView.CorpseTrackerList);
					playerTrackerGUI = new PlayerTrackerGUI(playerTracker, mainView.PlayerTrackerList);
					inventoryTrackerGUI = new InventoryTrackerGUI(profitLossTracker, inventoryTracker, mainView.InventoryTrackerList);

					chatLoggerGroup1GUI = new ChatLoggerGUI(chatLogger, Settings.SettingsManager.ChatLogger.Groups[0], mainView.ChatLogger1List);
					chatLoggerGroup2GUI = new ChatLoggerGUI(chatLogger, Settings.SettingsManager.ChatLogger.Groups[1], mainView.ChatLogger2List);

					inventoryToolsView = new InventoryToolsView(mainView, inventoryExporter);
					tinkeringToolsView = new TinkeringToolsView(mainView);

					accountServerCharacterGUI = new AccountServerCharacterGUI(mainView);
					serverGUI = new ServerGUI(mainView);

					mainView.CombatTrackerClearCurrentStats.Hit += (s2, e2) => { try { combatTrackerCurrent.ClearStats(); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.CombatTrackerExportCurrentStats.Hit += (s2, e2) => { try { combatTrackerCurrent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker." + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".xml", true); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.CombatTrackerClearPersistentStats.Hit += new EventHandler(CombatTrackerClearPersistentStats_Hit);

					mainView.CorpseTrackerClearHistory.Hit += new EventHandler(CorpseTrackerClearHistory_Hit);

					mainView.PlayerTrackerClearHistory.Hit += new EventHandler(PlayerTrackerClearHistory_Hit);

					mainView.ChatLoggerClearHistory.Hit += new EventHandler(ChatLoggerClearHistory_Hit);

					Assembly assembly = Assembly.GetExecutingAssembly();
					System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
					mainView.VersionLabel.Text = "Version: " + fvi.ProductVersion;
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
				CoreManager.Current.PluginInitComplete -= new EventHandler<EventArgs>(Current_PluginInitComplete_VTClassic);
				CoreManager.Current.PluginInitComplete -= new EventHandler<EventArgs>(Current_PluginInitComplete_VTank);
				CoreManager.Current.PluginInitComplete -= new EventHandler<EventArgs>(Current_PluginInitComplete_VVS);
				CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete_VHS);
				CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete_VHUD);
				CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
				CoreManager.Current.CharacterFilter.Logoff -= new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);
				CoreManager.Current.CommandLineText -= new EventHandler<ChatParserInterceptEventArgs>(Current_CommandLineText);


				// Views, depends on VirindiViewService.dll
				// We dispose these before our other objects (Trackers/Macros) as these probably reference those other objects.
				if (hud != null) hud.Dispose();

				if (accountServerCharacterGUI != null) accountServerCharacterGUI.Dispose();
				if (serverGUI != null) serverGUI.Dispose();

				if (tinkeringToolsView != null) tinkeringToolsView.Dispose();
				//if (inventoryToolsView != null) inventoryToolsView.Dispose();

				if (chatLoggerGroup1GUI != null) chatLoggerGroup1GUI.Dispose();
				if (chatLoggerGroup2GUI != null) chatLoggerGroup2GUI.Dispose();

				if (inventoryTrackerGUI != null) inventoryTrackerGUI.Dispose();
				if (playerTrackerGUI != null) playerTrackerGUI.Dispose();
				if (corpseTrackerGUI != null) corpseTrackerGUI.Dispose();
				if (combatTrackerGUIPersistent != null) combatTrackerGUIPersistent.Dispose();
				if (combatTrackerGUICurrent != null) combatTrackerGUICurrent.Dispose();
				if (manaTrackerGUI != null) manaTrackerGUI.Dispose();
				if (mainView != null) mainView.Dispose(); // We dispose this last in the Views as the other Views reference it.


				// Virindi Tank Extensions, depends on utank2-i.dll
				if (itemInfoPrinter != null) itemInfoPrinter.Dispose();
				if (looter != null) looter.Dispose();


				// Virindi Classic Looter Extensions, depends on VTClassic.dll
				if (inventoryPacker != null) inventoryPacker.Dispose();
				if (autoTradeAdd != null) autoTradeAdd.Dispose();
				if (autoBuySell != null) autoBuySell.Dispose();


				// Relies on other decal assemblies
				if (chatFilter != null) chatFilter.Dispose();


				// Misc
				if (windowFrameRemover != null) windowFrameRemover.Dispose();
				if (windowMover != null) windowMover.Dispose();
				if (fpsManager != null) fpsManager.Dispose();

				// Loggers
				if (chatLogger != null) chatLogger.Dispose();
				if (chatLogFileWriter != null) chatLogFileWriter.Dispose();

				// Trackers
				if (equipmentTracker != null) equipmentTracker.Dispose();
				if (combatTrackerCurrent != null) combatTrackerCurrent.Dispose();
				if (combatTrackerPersistent != null) combatTrackerPersistent.Dispose();
				if (corpseTracker != null) corpseTracker.Dispose();
				if (playerTracker != null) playerTracker.Dispose();
				if (inventoryTracker != null) inventoryTracker.Dispose();
				if (profitLossTracker != null) profitLossTracker.Dispose();

				// Macros
				if (loginActions != null) loginActions.Dispose();
				if (periodicCommands != null) periodicCommands.Dispose();
				if (openMainPackOnLogin != null) openMainPackOnLogin.Dispose();
				if (maximizeChatOnLogin != null) maximizeChatOnLogin.Dispose();
				if (autoPercentConfirmation != null) autoPercentConfirmation.Dispose();
				if (autoRecharge != null) autoRecharge.Dispose();
				if (autoTradeAccept != null) autoTradeAccept.Dispose();
				if (logOutOnDeath != null) logOutOnDeath.Dispose();

				// General
				if (inventoryLogger != null) inventoryLogger.Dispose();
				if (idleActionManager != null) idleActionManager.Dispose();


				Current = null;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_LoginComplete_VHS(object sender, EventArgs e)
		{
			try
			{
				// Wire up Inventory Packer Hotkey
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
						catch (FileNotFoundException) { MyClasses.VCS_Connector.SendChatTextCategorized("Errors", "<{" + PluginName + "}>: " + "Unable to start Inventory Packer. Is Virindi Tank running?", 5); }
						catch (Exception ex) { Debug.LogException(ex); }
					};
				}


				// Wire up One Touch Heal Hotkey
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


				// Wire up Maximize/Minimize Chat Hotkey
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
			catch (FileNotFoundException ex) { startupErrors.Add("Hotkey failed to bind: " + ex.Message + ". Is Virindi Hotkey System running?"); }
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_LoginComplete_VHUD(object sender, EventArgs e)
		{
			try
			{
				hud = new HUD(equipmentTracker, inventoryTracker, profitLossTracker, combatTrackerCurrent);
			}
			catch (FileNotFoundException ex) { startupErrors.Add("HUD failed to bind: " + ex.Message + ". Is Virindi HUDs running?"); }
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				var stopWatch = new System.Diagnostics.Stopwatch();

				// Load Persistent Stats
				try
				{
					if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
					{
						stopWatch.Reset();
						stopWatch.Start();
					}

					if (Settings.SettingsManager.CombatTracker.Persistent.Value)
						combatTrackerPersistent.ImportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker.xml");

					if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
					{
						stopWatch.Stop();
						Debug.WriteToChat("Loaded Persistent Combat Tracker: " + stopWatch.Elapsed.TotalMilliseconds.ToString("N0") + "ms");
					}
				}
				catch (Exception ex) { Debug.LogException(ex); }

				try
				{
					if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
					{
						stopWatch.Reset();
						stopWatch.Start();
					}

					if (Settings.SettingsManager.CorpseTracker.Persistent.Value)
						corpseTracker.ImportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CorpseTracker.xml");

					if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
					{
						stopWatch.Stop();
						Debug.WriteToChat("Loaded Persistent Corpse Trackers: " + stopWatch.Elapsed.TotalMilliseconds.ToString("N0") + "ms");
					}
				}
				catch (Exception ex) { Debug.LogException(ex); }

				try
				{
					if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
					{
						stopWatch.Reset();
						stopWatch.Start();
					}

					if (Settings.SettingsManager.PlayerTracker.Persistent.Value)
						playerTracker.ImportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".PlayerTracker.xml");

					if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
					{
						stopWatch.Stop();
						Debug.WriteToChat("Loaded Persistent Player Tracker: " + stopWatch.Elapsed.TotalMilliseconds.ToString("N0") + "ms");
					}
				}
				catch (Exception ex) { Debug.LogException(ex); }


				// Load Persistent Logs
				try
				{
					if (Settings.SettingsManager.ChatLogger.Persistent.Value)
					{
						if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
						{
							stopWatch.Reset();
							stopWatch.Start();
						}

						chatLogFileWriter.FileName = PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".ChatLogger.txt";

						List<ILoggerTarget<Loggers.Chat.LoggedChat>> chatLoggers = new List<ILoggerTarget<Loggers.Chat.LoggedChat>>();
						chatLoggers.Add(chatLoggerGroup1GUI);
						chatLoggers.Add(chatLoggerGroup2GUI);
						Loggers.Chat.ChatLogImporter.Import(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".ChatLogger.txt", chatLoggers);

						if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
						{
							stopWatch.Stop();
							Debug.WriteToChat("Loaded Persistent Chat: " + stopWatch.Elapsed.TotalMilliseconds.ToString("N0") + "ms");
						}
					}
				}
				catch (Exception ex) { Debug.LogException(ex); }


				foreach (string startupError in startupErrors)
					MyClasses.VCS_Connector.SendChatTextCategorized("Errors", "<{" + PluginName + "}>: Startup Error: " + startupError, 5);

				startupErrors.Clear();


				MyClasses.VCS_Connector.SendChatTextCategorized("CommandLine", "<{" + PluginName + "}>: " + "Plugin now online. Server population: " + Core.CharacterFilter.ServerPopulation, 5);

				savePersistentStatsTimer.Start();

				//Util.ExportSpells(PluginPersonalFolder.FullName + @"\spells.csv");
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_Logoff(object sender, LogoffEventArgs e)
		{
			try
			{
				savePersistentStatsTimer.Stop();

				ExportPersistentStats();

				if (Settings.SettingsManager.ChatLogger.Persistent.Value)
					chatLogFileWriter.Flush();

				if (Settings.SettingsManager.CombatTracker.ExportOnLogOff.Value)
					combatTrackerCurrent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker." + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".xml");
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		#region ' /mt commands '
		void Current_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			try
			{
				if (e.Text == null)
					return;

				if (ProcessMTCommand(e.Text))
					e.Eat = true;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		readonly Dictionary<string, object> rememberedSettings = new Dictionary<string, object>();

		public bool ProcessMTCommand(string mtCommand)
		{
			string lower = mtCommand.ToLower().Trim();

			if (lower.StartsWith("/mt test"))
			{
				//DecalProxy.DispatchChatToBoxWithPluginIntercept("/vt start");
				//void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
				/*CoreManager.Current.ChatBoxMessage += new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
				if (CoreManager.Current.ChatBoxMessage != null)
				{ // or the event-name for field-like events
					// or your own event-type in place of EventHandler
					foreach (EventHandler subscriber in field.GetInvocationList())
					{
						// etc
					}
				}*/

				return true;
			}

			if (lower.StartsWith("/mt logoff") || lower.StartsWith("/mt logout"))
			{
				CoreManager.Current.Actions.Logout();
				return true;
			}

			if (lower.StartsWith("/mt send "))
			{
				if (lower.StartsWith("/mt send enter")) PostMessageTools.SendEnter();
				else if (lower.StartsWith("/mt send pause")) PostMessageTools.SendPause();
				else if (lower.StartsWith("/mt send space")) PostMessageTools.SendSpace();
				else if (lower.StartsWith("/mt send cntrl+") && lower.Length >= 16) PostMessageTools.SendCntrl(mtCommand[15]);
				else if (lower.StartsWith("/mt send f4")) PostMessageTools.SendF4();
				else if (lower.StartsWith("/mt send f12")) PostMessageTools.SendF12();
				else if (lower.StartsWith("/mt send msg ") && lower.Length > 13) PostMessageTools.SendMsg(mtCommand.Substring(13, mtCommand.Length - 13));
				else return false;

				return true;
			}

				if (lower.StartsWith("/mt quit") || lower.StartsWith("/mt exit"))
				{
				PostMessageTools.SendAltF4();
				return true;
				}

			if (lower.StartsWith("/mt click "))
			{
				if (lower.StartsWith("/mt click ok")) PostMessageTools.ClickOK();
				else if (lower.StartsWith("/mt click yes")) PostMessageTools.ClickYes();
				else if (lower.StartsWith("/mt click no")) PostMessageTools.ClickNo();
				else if (lower.StartsWith("/mt click "))
				{
					string[] splits = lower.Split(' ');

					if (splits.Length >= 4)
					{
						int x;
						int y;

						if (!int.TryParse(splits[2], out x)) return false;
						if (!int.TryParse(splits[3], out y)) return false;

						PostMessageTools.SendMouseClick(x, y);
					}
				}

				return true;
			}

			if (lower.StartsWith("/mt get xy"))
			{
				Point p;
				if (User32.GetCursorPos(out p))
				{
					User32.RECT rct = new User32.RECT();

					if (User32.GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rct))
						Debug.WriteToChat("Current cursor position: " + (p.X - rct.Left) + "," + (p.Y - rct.Top));
				}

				return true;
			}

			if (lower.StartsWith("/mt face "))
			{
				if (lower.Length > 9)
				{
					int heading;
					if (!int.TryParse(lower.Substring(9, lower.Length - 9), out heading))
						return false;

					CoreManager.Current.Actions.FaceHeading(heading, true);
				}

				return true;
			}

			if (lower.StartsWith("/mt jump") || lower.StartsWith("/mt sjump"))
			{
				int msToHoldDown = 0;
				bool addShift = lower.Contains("sjump");
				bool addW = lower.Contains("jumpw");
				bool addZ = lower.Contains("jumpz");
				bool addX = lower.Contains("jumpx");
				bool addC = lower.Contains("jumpc");

				string[] split = lower.Split(' ');
				if (split.Length == 3)
					int.TryParse(split[2], out msToHoldDown);

				PostMessageTools.SendSpace(msToHoldDown, addShift, addW, addZ, addX, addC);

				return true;
			}

			if (lower.StartsWith("/mt movement "))
			{
				int msToHoldDown = 0;
				char key;

				string[] split = lower.Split(' ');
				if (split.Length != 4)
					return false;

				char.TryParse(split[2], out key);
				int.TryParse(split[3], out msToHoldDown);

				PostMessageTools.SendMovement(key, msToHoldDown);

				return true;
			}

			if (lower.StartsWith("/mt fellow "))
			{
				if (lower.StartsWith("/mt fellow create ") && lower.Length > 18)
				{
					string fellowName = lower.Substring(18, lower.Length - 18);

					PostMessageTools.SendF12();
					PostMessageTools.SendF4();

					Rectangle rect = Core.Actions.UIElementRegion(UIElementType.Panels);

					PostMessageTools.SendMouseClick(rect.X + 200, rect.Y + 240);
					PostMessageTools.SendMsg(fellowName);

					CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(FellowCreate_Current_RenderFrame);
				}
				else if (lower.StartsWith("/mt fellow open")) Core.Actions.FellowshipSetOpen(true);
				else if (lower.StartsWith("/mt fellow close")) Core.Actions.FellowshipSetOpen(false);
				else if (lower.StartsWith("/mt fellow disband")) Core.Actions.FellowshipDisband();
				else if (lower.StartsWith("/mt fellow quit")) Core.Actions.FellowshipQuit();
				else if (lower.StartsWith("/mt fellow recruit ") && lower.Length > 19)
				{
					string player = lower.Substring(19, lower.Length - 19);

					WorldObject closest = Util.GetClosestObject(player);

					try
					{
						if (closest != null)
							Core.Actions.FellowshipRecruit(closest.Id);
					}
					catch (AccessViolationException) { } // Eat the decal error
				}
				else return false;

				return true;
			}

			if ((lower.StartsWith("/mt cast ") && lower.Length > 9) || (lower.StartsWith("/mt castp ") && lower.Length > 10))
			{
				bool partialMatch = lower.StartsWith("/mt castp ");
				int offset = partialMatch ? 10 : 9;

				int spellId;
				int objectId = 0;

				string[] splits = lower.Split(' ');

				if (splits.Length < 3)
					return false;

				int.TryParse(splits[2], out spellId);

				string spellName;
				string targetName = null;

				if (!lower.Contains(" on "))
					spellName = lower.Substring(offset, lower.Length - offset);
				else
				{
					spellName = lower.Substring(offset, lower.IndexOf(" on ", StringComparison.Ordinal) - offset);
					targetName = lower.Substring(lower.IndexOf(" on ", StringComparison.Ordinal) + 4, lower.Length - (lower.IndexOf(" on ", StringComparison.Ordinal) + 4));
				}

				if (spellId == 0)
				{
					FileService service = CoreManager.Current.Filter<FileService>();

					for (int i = 0; i < service.SpellTable.Length; i++)
					{
						Spell spell = service.SpellTable[i];

						if (String.Equals(spellName, spell.Name, StringComparison.OrdinalIgnoreCase) || (partialMatch && spell.Name.ToLower().Contains(spellName.ToLower())))
						{
							spellId = spell.Id;
							break;
						}
					}

					if (spellId == 0) return false;
				}

				if (targetName != null)
				{
					objectId = FindIdForName(targetName, false, false, true, partialMatch);
					if (objectId == -1) return false;
				}

				CoreManager.Current.Actions.CastSpell(spellId, objectId);
				return true;
			}

			if ((lower.StartsWith("/mt select ") && lower.Length > 11) || (lower.StartsWith("/mt selectp ") && lower.Length > 12))
			{
				bool partialMatch = lower.StartsWith("/mt selectp ");
				int offset = partialMatch ? 12 : 11;

				int objectId = FindIdForName(lower.Substring(offset, lower.Length - offset), true, true, true, partialMatch);

				if (objectId == -1) return false;
				CoreManager.Current.Actions.SelectItem(objectId);
				return true;
			}

			if ((lower.StartsWith("/mt use ") && lower.Length > 8) || (lower.StartsWith("/mt usep ") && lower.Length > 9) ||
				(lower.StartsWith("/mt usei ") && lower.Length > 9) || (lower.StartsWith("/mt useip ") && lower.Length > 10) ||
				(lower.StartsWith("/mt usel ") && lower.Length > 9) || (lower.StartsWith("/mt uselp ") && lower.Length > 10))
			{
				bool partialMatch = lower.StartsWith("/mt usep ") || lower.StartsWith("/mt useip ") || lower.StartsWith("/mt uselp ");
				int offset = lower.StartsWith("/mt use ") || lower.StartsWith("/mt usep ") ? (partialMatch ? 9 : 8) : (partialMatch ? 10 : 9);

				bool searchInventory = !lower.StartsWith("/mt usel");
				bool searchLandscape = !lower.StartsWith("/mt usei");

				int objectId;
				int useMethod = 0;

				if (!lower.Contains(" on "))
				{
					if (lower.Contains("closestnpc"))
					{
						WorldObject wo = Util.GetClosestObject(ObjectClass.Npc);
						if (wo == null) return false;
						objectId = wo.Id;
					}
					else if (lower.Contains("closestvendor"))
					{
						WorldObject wo = Util.GetClosestObject(ObjectClass.Vendor);
						if (wo == null) return false;
						objectId = wo.Id;
					}
					else if (lower.Contains("closestportal"))
					{
						WorldObject wo = Util.GetClosestObject(ObjectClass.Portal);
						if (wo == null) return false;
						objectId = wo.Id;
					}
					else
						objectId = FindIdForName(lower.Substring(offset, lower.Length - offset), searchInventory, false, searchLandscape, partialMatch);
				}
				else
				{
					string command = lower.Substring(offset, lower.Length - offset);
					string first = command.Substring(0, command.IndexOf(" on ", StringComparison.Ordinal));
					string second = command.Substring(first.Length + 4, command.Length - first.Length - 4);

					objectId = FindIdForName(first, searchInventory, false, false, partialMatch);
					useMethod = FindIdForName(second, searchInventory, false, searchLandscape, partialMatch, objectId);
				}

				if (objectId == -1 || useMethod == -1)
					return false;

				if (useMethod == 0)
					CoreManager.Current.Actions.UseItem(objectId, 0);
				else
				{
					CoreManager.Current.Actions.SelectItem(useMethod);
					CoreManager.Current.Actions.UseItem(objectId, 1, useMethod);
				}

				return true;
			}

			if ((lower.StartsWith("/mt give ") && lower.Contains(" to ")) || (lower.StartsWith("/mt givep ") && lower.Contains(" to ")))
			{
				bool partialMatch = lower.StartsWith("/mt givep ");
				int offset = partialMatch ? 10 : 9;

				string command = lower.Substring(offset, lower.Length - offset);
				string first = command.Substring(0, command.IndexOf(" to ", StringComparison.Ordinal));
				string second = command.Substring(first.Length + 4, command.Length - first.Length - 4);

				int objectId = FindIdForName(first, true, false, false, partialMatch);
				int destinationId = FindIdForName(second, false, false, true, partialMatch);

				if (objectId == -1 || destinationId == -1) return false;
				CoreManager.Current.Actions.GiveItem(objectId, destinationId);
				return true;
			}

			if ((lower.StartsWith("/mt loot ") && lower.Length > 9) || (lower.StartsWith("/mt lootp ") && lower.Length > 10))
			{
				bool partialMatch = lower.StartsWith("/mt lootp ");
				int offset = partialMatch ? 10 : 9;

				int objectId = FindIdForName(lower.Substring(offset, lower.Length - offset), false, true, false, partialMatch);

				if (objectId == -1) return false;
				CoreManager.Current.Actions.UseItem(objectId, 0);
				return true;
			}

			if ((lower.StartsWith("/mt drop ") && lower.Length > 9) || (lower.StartsWith("/mt dropp ") && lower.Length > 10))
			{
				bool partialMatch = lower.StartsWith("/mt dropp ");
				int offset = partialMatch ? 10 : 9;

				int objectId = FindIdForName(lower.Substring(offset, lower.Length - offset), true, false, false, partialMatch);

				if (objectId == -1) return false;
				CoreManager.Current.Actions.DropItem(objectId);
				return true;
			}

			if (lower.StartsWith("/mt combatstate ") && lower.Length > 16)
			{
				string state = lower.Substring(16, lower.Length - 16);

				if (state == "magic") CoreManager.Current.Actions.SetCombatMode(CombatState.Magic);
				else if (state == "melee") CoreManager.Current.Actions.SetCombatMode(CombatState.Melee);
				else if (state == "missile") CoreManager.Current.Actions.SetCombatMode(CombatState.Missile);
				else if (state == "peace") CoreManager.Current.Actions.SetCombatMode(CombatState.Peace);
				else return false;
				return true;
			}

			if ((lower.StartsWith("/mt trade add ") && lower.Length > 14) || (lower.StartsWith("/mt trade addp ") && lower.Length > 15))
			{
				bool partialMatch = lower.StartsWith("/mt trade addp ");
				int offset = partialMatch ? 15 : 14;

				int objectId = FindIdForName(lower.Substring(offset, lower.Length - offset), true, false, false, partialMatch);

				if (objectId == -1) return false;
				CoreManager.Current.Actions.TradeAdd(objectId);
				return true;
			}
			if (lower.StartsWith("/mt trade accept")) { CoreManager.Current.Actions.TradeAccept(); return true; }
			if (lower.StartsWith("/mt trade decline")) { CoreManager.Current.Actions.TradeDecline(); return true; }
			if (lower.StartsWith("/mt trade reset")) { CoreManager.Current.Actions.TradeReset(); return true; }
			if (lower.StartsWith("/mt trade end")) { CoreManager.Current.Actions.TradeEnd(); return true; }

			if (CoreManager.Current.Actions.VendorId != 0)
			{
				if ((lower.StartsWith("/mt vendor addbuy ") && lower.Length > 18) || (lower.StartsWith("/mt vendor addbuyp ") && lower.Length > 19))
				{
					bool partialMatch = lower.StartsWith("/mt vendor addbuyp ");
					int offset = partialMatch ? 19 : 18;

					int count;
					string itemName = lower.Substring(offset, lower.Length - offset);
					var splits = itemName.Split(' ');
					if (splits.Length > 1 && int.TryParse(splits[splits.Length - 1], out count))
						itemName = itemName.Substring(0, itemName.LastIndexOf(' '));
					else
						count = 1;

					int objectId = FindIdForName(itemName, true, false, false, partialMatch);

					if (objectId == -1) return false;
					CoreManager.Current.Actions.VendorAddBuyList(objectId, count);
					return true;
				}
				if ((lower.StartsWith("/mt vendor addsell ") && lower.Length > 19) || (lower.StartsWith("/mt vendor addsellp ") && lower.Length > 20))
				{
					bool partialMatch = lower.StartsWith("/mt vendor addsellp ");
					int offset = partialMatch ? 20 : 19;

					int objectId = FindIdForName(lower.Substring(offset, lower.Length - offset), true, false, false, partialMatch);

					if (objectId == -1) return false;
					CoreManager.Current.Actions.VendorAddSellList(objectId);
					return true;
				}
				if (lower.StartsWith("/mt vendor buy")) { CoreManager.Current.Actions.VendorBuyAll(); return true; }
				if (lower.StartsWith("/mt vendor clearbuy")) { CoreManager.Current.Actions.VendorClearBuyList(); return true; }
				if (lower.StartsWith("/mt vendor sell")) { CoreManager.Current.Actions.VendorSellAll(); return true; }
				if (lower.StartsWith("/mt vendor clearsell")) { CoreManager.Current.Actions.VendorClearSellList(); return true; }
			}

			if (lower.StartsWith("/mt autopack") && inventoryPacker != null) { inventoryPacker.Start(); return true; }

			if (lower.StartsWith("/mt dumpspells")) { Util.ExportSpells(@"c:\mt spelldump.txt"); return true; }

			if (lower.StartsWith("/mt opt "))
			{
				var settingsManagerType = Assembly.GetExecutingAssembly().GetType("MagTools.Settings.SettingsManager", false);

				if (settingsManagerType == null)
				{
					Debug.WriteToChat("Failed to find type: MagTools.Settings.SettingsManager");
					return false;
				}

				if (lower.StartsWith("/mt opt list"))
				{
					foreach (var nestedType in settingsManagerType.GetNestedTypes())
					{
						foreach (var nestedField in nestedType.GetFields())
						{
							if (nestedField.FieldType == typeof(Mag.Shared.Settings.Setting<bool>))
								Debug.WriteToChat(nestedType.Name + "." + nestedField.Name + " <bool>");
							else if (nestedField.FieldType == typeof(Mag.Shared.Settings.Setting<int>))
								Debug.WriteToChat(nestedType.Name + "." + nestedField.Name + " <int>");
							else if (nestedField.FieldType == typeof(Mag.Shared.Settings.Setting<double>))
								Debug.WriteToChat(nestedType.Name + "." + nestedField.Name + " <double>");
							else if (nestedField.FieldType == typeof(Mag.Shared.Settings.Setting<string>))
								Debug.WriteToChat(nestedType.Name + "." + nestedField.Name + " <string>");
						}
					}

					return true;
				}
				
				if (lower.StartsWith("/mt opt get "))
				{
					string optionName = lower.Substring(12, lower.Length - 12);
					var optionField = GetOptionField(optionName);

					if (optionField == null)
						return false;

					if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<bool>))
						Debug.WriteToChat(optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<bool>)optionField.GetValue(null)).Value);
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<int>))
						Debug.WriteToChat(optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<int>)optionField.GetValue(null)).Value);
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<double>))
						Debug.WriteToChat(optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<double>)optionField.GetValue(null)).Value);
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<string>))
						Debug.WriteToChat(optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<string>)optionField.GetValue(null)).Value);
					else
					{
						Debug.WriteToChat("Failed to Get " + optionName);
						return false;
					}

					return true;
				}
				
				if (lower.StartsWith("/mt opt remember "))
				{
					string optionName = lower.Substring(17, lower.Length - 17);

					var optionField = GetOptionField(optionName);

					if (optionField == null)
						return false;

					if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<bool>))
					{
						if (!rememberedSettings.ContainsKey(optionField.Name + "." + optionField.Name))
							rememberedSettings.Add(optionField.Name + "." + optionField.Name, null);
						rememberedSettings[optionField.Name + "." + optionField.Name] = ((Mag.Shared.Settings.Setting<bool>)optionField.GetValue(null)).Value;
						Debug.WriteToChat("Remembered " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<bool>)optionField.GetValue(null)).Value);
					}
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<int>))
					{
						if (!rememberedSettings.ContainsKey(optionField.Name + "." + optionField.Name))
							rememberedSettings.Add(optionField.Name + "." + optionField.Name, null);
						rememberedSettings[optionField.Name + "." + optionField.Name] = ((Mag.Shared.Settings.Setting<int>)optionField.GetValue(null)).Value;
						Debug.WriteToChat("Remembered " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<int>)optionField.GetValue(null)).Value);
					}
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<double>))
					{
						if (!rememberedSettings.ContainsKey(optionField.Name + "." + optionField.Name))
							rememberedSettings.Add(optionField.Name + "." + optionField.Name, null);
						rememberedSettings[optionField.Name + "." + optionField.Name] = ((Mag.Shared.Settings.Setting<double>)optionField.GetValue(null)).Value;
						Debug.WriteToChat("Remembered " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<double>)optionField.GetValue(null)).Value);
					}
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<string>))
					{
						if (!rememberedSettings.ContainsKey(optionField.Name + "." + optionField.Name))
							rememberedSettings.Add(optionField.Name + "." + optionField.Name, null);
						rememberedSettings[optionField.Name + "." + optionField.Name] = ((Mag.Shared.Settings.Setting<string>)optionField.GetValue(null)).Value;
						Debug.WriteToChat("Remembered " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<string>)optionField.GetValue(null)).Value);
					}
					else
					{
						Debug.WriteToChat("Failed to Remember " + optionName);
						return false;
					}

					return true;
				}
				
				if (lower.StartsWith("/mt opt restore "))
				{
					string optionName = lower.Substring(16, lower.Length - 16);

					var optionField = GetOptionField(optionName);

					if (optionField == null)
						return false;

					if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<bool>))
					{
						if (rememberedSettings.ContainsKey(optionField.Name + "." + optionField.Name))
						{
							((Mag.Shared.Settings.Setting<bool>)optionField.GetValue(null)).Value = (bool)rememberedSettings[optionField.Name + "." + optionField.Name];
							Debug.WriteToChat("Restored " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<bool>)optionField.GetValue(null)).Value);
							return true;
						}
					}
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<int>))
					{
						if (rememberedSettings.ContainsKey(optionField.Name + "." + optionField.Name))
						{
							((Mag.Shared.Settings.Setting<int>)optionField.GetValue(null)).Value = (int)rememberedSettings[optionField.Name + "." + optionField.Name];
							Debug.WriteToChat("Restored " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<int>)optionField.GetValue(null)).Value);
							return true;
						}
					}
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<double>))
					{
						if (rememberedSettings.ContainsKey(optionField.Name + "." + optionField.Name))
						{
							((Mag.Shared.Settings.Setting<double>)optionField.GetValue(null)).Value = (double)rememberedSettings[optionField.Name + "." + optionField.Name];
							Debug.WriteToChat("Restored " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<double>)optionField.GetValue(null)).Value);
							return true;
						}
					}
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<string>))
					{
						if (rememberedSettings.ContainsKey(optionField.Name + "." + optionField.Name))
						{
							((Mag.Shared.Settings.Setting<string>)optionField.GetValue(null)).Value = (string)rememberedSettings[optionField.Name + "." + optionField.Name];
							Debug.WriteToChat("Restored " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<string>)optionField.GetValue(null)).Value);
							return true;
						}
					}

					Debug.WriteToChat("Failed to Restore " + optionName);
					return false;
				}
				
				if (lower.StartsWith("/mt opt set "))
				{
					string optionName = lower.Substring(12, lower.Length - 12 - (lower.Length - lower.LastIndexOf(' '))).Trim();
					string optionValue = lower.Substring(lower.LastIndexOf(' ') + 1, lower.Length - lower.LastIndexOf(' ') - 1).Trim();

					var optionField = GetOptionField(optionName);

					if (optionField == null)
						return false;

					if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<bool>))
					{
						bool result;
						if (bool.TryParse(optionValue, out result))
						{
							((Mag.Shared.Settings.Setting<bool>)optionField.GetValue(null)).Value = result;
							Debug.WriteToChat("Set " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<bool>)optionField.GetValue(null)).Value);
							return true;
						}
					}
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<int>))
					{
						int result;
						if (int.TryParse(optionValue, out result))
						{
							((Mag.Shared.Settings.Setting<int>)optionField.GetValue(null)).Value = result;
							Debug.WriteToChat("Set " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<int>)optionField.GetValue(null)).Value);
							return true;
						}
					}
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<double>))
					{
						double result;
						if (double.TryParse(optionValue, out result))
						{
							((Mag.Shared.Settings.Setting<double>)optionField.GetValue(null)).Value = result;
							Debug.WriteToChat("Set " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<double>)optionField.GetValue(null)).Value);
							return true;
						}
					}
					else if (optionField.FieldType == typeof(Mag.Shared.Settings.Setting<string>))
					{
						((Mag.Shared.Settings.Setting<string>)optionField.GetValue(null)).Value = optionValue;
						Debug.WriteToChat("Set " + optionField.Name + "." + optionField.Name + " = " + ((Mag.Shared.Settings.Setting<string>)optionField.GetValue(null)).Value);
						return true;
					}

					Debug.WriteToChat("Failed to Set " + optionName + " to " + optionValue);
					return false;
				}

				return false;
			}

			return false;
		}

		FieldInfo GetOptionField(string path)
		{
			var settingsManagerType = Assembly.GetExecutingAssembly().GetType("MagTools.Settings.SettingsManager", false);

			if (settingsManagerType == null)
				return null;

			foreach (var nestedType in settingsManagerType.GetNestedTypes())
			{
				foreach (var nestedField in nestedType.GetFields())
				{
					if (String.Equals(nestedType.Name + "." + nestedField.Name, path, StringComparison.OrdinalIgnoreCase))
						return nestedField;
				}
			}

			return null;
		}

		int FindIdForName(string name, bool searchInInventory, bool searchOpenContainer, bool searchEnvironment, bool partialMatch, int idToSkip = 0)
		{
			// Exact match attempt first
			if (searchInInventory)
			{
				foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (String.Compare(wo.Name, name, StringComparison.OrdinalIgnoreCase) == 0 && wo.Id != idToSkip)
						return wo.Id;
				}
			}

			if (searchOpenContainer && CoreManager.Current.Actions.OpenedContainer != 0)
			{
				foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.Actions.OpenedContainer))
				{
					if (String.Compare(wo.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
						return wo.Id;
				}
			}

			if (searchEnvironment)
			{
				WorldObject closestObject = Util.GetClosestObject(name);

				if (closestObject != null)
					return closestObject.Id;
			}

			// Partial match attempt second
			if (partialMatch)
			{
				if (searchInInventory)
				{
					foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
					{
						if (wo.Name.ToLower().Contains(name.ToLower()) && wo.Id != idToSkip)
							return wo.Id;
					}
				}

				if (searchOpenContainer && CoreManager.Current.Actions.OpenedContainer != 0)
				{
					foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.Actions.OpenedContainer))
					{
						if (wo.Name.ToLower().Contains(name.ToLower()))
							return wo.Id;
					}
				}

				if (searchEnvironment)
				{
					WorldObject closestObject = Util.GetClosestObject(name, true);

					if (closestObject != null)
						return closestObject.Id;
				}	
			}

			return -1;
		}

		void FellowCreate_Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(FellowCreate_Current_RenderFrame);

				Rectangle rect = Core.Actions.UIElementRegion(UIElementType.Panels);
				PostMessageTools.SendMouseClick(rect.X + 145, rect.Y + 343);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
		#endregion

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

					MyClasses.VCS_Connector.SendChatTextCategorized("CommandLine", "<{" + PluginName + "}>: " + "File deleted: " + fileInfo.FullName, 5);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CorpseTrackerClearHistory_Hit(object sender, EventArgs e)
		{
			try
			{
				corpseTracker.ClearStats();

				FileInfo fileInfo = new FileInfo(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CorpseTracker.xml");

				if (fileInfo.Exists)
				{
					fileInfo.Delete();

					MyClasses.VCS_Connector.SendChatTextCategorized("CommandLine", "<{" + PluginName + "}>: " + "File deleted: " + fileInfo.FullName, 5);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void PlayerTrackerClearHistory_Hit(object sender, EventArgs e)
		{
			try
			{
				playerTracker.ClearStats();

				FileInfo fileInfo = new FileInfo(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".PlayerTracker.xml");

				if (fileInfo.Exists)
				{
					fileInfo.Delete();

					MyClasses.VCS_Connector.SendChatTextCategorized("CommandLine", "<{" + PluginName + "}>: " + "File deleted: " + fileInfo.FullName, 5);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		#endregion

		#region ' Clear Persistent Logs Buttons '

		void ChatLoggerClearHistory_Hit(object sender, EventArgs e)
		{
			try
			{
				chatLoggerGroup1GUI.Clear();

				chatLoggerGroup2GUI.Clear();

				if (Settings.SettingsManager.CombatTracker.Persistent.Value)
					combatTrackerPersistent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".ChatLogger.txt");
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		#endregion

		void SavePersistentStatsTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				ExportPersistentStats();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void ExportPersistentStats()
		{
			var stopWatch = new System.Diagnostics.Stopwatch();

			if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
				stopWatch.Start();

			if (Settings.SettingsManager.CombatTracker.Persistent.Value)
				combatTrackerPersistent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker.xml");

			if (Settings.SettingsManager.CorpseTracker.Persistent.Value)
				corpseTracker.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CorpseTracker.xml");

			if (Settings.SettingsManager.PlayerTracker.Persistent.Value)
				playerTracker.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".PlayerTracker.xml");

			if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
			{
				stopWatch.Stop();
				Debug.WriteToChat("Export Persistent Stats: " + stopWatch.Elapsed.TotalMilliseconds.ToString("N0") + "ms");
			}
		}
	}
}
