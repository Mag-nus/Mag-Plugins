using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;

using MagTools.Client;
using MagTools.Inventory;
using MagTools.Loggers;
using MagTools.Views;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

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

		// Macros
		Macros.OpenMainPackOnLogin openMainPackOnLogin;
		Macros.MaximizeChatOnLogin maximizeChatOnLogin;
		Macros.AutoRecharge autoRecharge;
		Macros.AutoTradeAccept autoTradeAccept;
		Macros.OneTouchHeal oneTouchHeal;
		Macros.LogOutOnDeath logOutOnDeath;
	
		// Trackers
		Trackers.Equipment.EquipmentTracker equipmentTracker;
		public Trackers.Equipment.IEquipmentTracker EquipmentTracker { get { return equipmentTracker; } }
		Trackers.Combat.CombatTracker combatTrackerCurrent;
		Trackers.Combat.CombatTracker combatTrackerPersistent;
		Trackers.Corpse.CorpseTracker corpseTracker;
		Trackers.Player.PlayerTracker playerTracker;
		Trackers.Consumable.ConsumableTracker consumableTracker;

		// Loggers
		Loggers.Chat.ChatLogger chatLogger;
		Loggers.Chat.BufferedChatLogFileWriter chatLogFileWriter;

		// Misc
		WindowFrameRemover windowFrameRemover;
		WindowMover windowMover;
		NoFocusFPSManager noFocusFPSManager;


		// Relies on other decal assemblies
		ChatFilter chatFilter;


		// Virindi Classic Looter Extensions, depends on VTClassic.dll
		Macros.InventoryPacker inventoryPacker;
		public Macros.IInventoryPacker InventoryPacker { get { return inventoryPacker; } }
		Macros.AutoTradeAdd autoTradeAdd;
		Macros.AutoBuySell autoBuySell;


		// Virindi Tank Extensions, depends on utank2-i.dll
		ItemInfo.ItemInfoPrinter itemInfoPrinter;
		Macros.Looter looter;
		public Macros.ILooter Looter { get { return looter; } }


		// Views, depends on VirindiViewService.dll
		MainView mainView;

		ManaTrackerGUI manaTrackerGUI;
		CombatTrackerGUI combatTrackerGUICurrent;
		CombatTrackerGUI combatTrackerGUIPersistent;
		CorpseTrackerGUI corpseTrackerGUI;
		PlayerTrackerGUI playerTrackerGUI;
		ConsumableTrackerGUI consumableTrackerGUI;

		ChatLoggerGUI chatLoggerGroup1GUI;
		ChatLoggerGUI chatLoggerGroup2GUI;

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
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
				CoreManager.Current.CharacterFilter.Logoff += new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);
				CoreManager.Current.CommandLineText += new EventHandler<ChatParserInterceptEventArgs>(Current_CommandLineText);


				// General
				inventoryExporter = new InventoryExporter();
				inventoryLogger = new InventoryLogger();

				// Macros
				openMainPackOnLogin = new Macros.OpenMainPackOnLogin();
				maximizeChatOnLogin = new Macros.MaximizeChatOnLogin();
				autoRecharge = new Macros.AutoRecharge();
				autoTradeAccept = new Macros.AutoTradeAccept();
				oneTouchHeal = new Macros.OneTouchHeal();
				logOutOnDeath = new Macros.LogOutOnDeath();

				// Trackers
				equipmentTracker = new Trackers.Equipment.EquipmentTracker();
				combatTrackerCurrent = new Trackers.Combat.CombatTracker();
				combatTrackerPersistent = new Trackers.Combat.CombatTracker();
				corpseTracker = new Trackers.Corpse.CorpseTracker();
				playerTracker = new Trackers.Player.PlayerTracker();
				consumableTracker = new Trackers.Consumable.ConsumableTracker();

				// Loggers
				chatLogger = new Loggers.Chat.ChatLogger();
				chatLogFileWriter = new Loggers.Chat.BufferedChatLogFileWriter(null, chatLogger, TimeSpan.FromMinutes(10));

				// Misc
				windowFrameRemover = new WindowFrameRemover();
				windowMover = new WindowMover();
				noFocusFPSManager = new NoFocusFPSManager();


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
					objectName = "inventoryPacker";		inventoryPacker = new Macros.InventoryPacker();
					objectName = "autoTradeAdd";		autoTradeAdd = new Macros.AutoTradeAdd();
					objectName = "autoBuySell";			autoBuySell = new Macros.AutoBuySell();
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
					looter = new Macros.Looter();
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
					consumableTrackerGUI = new ConsumableTrackerGUI(consumableTracker, mainView.ConsumableTrackerList);
					chatLoggerGroup1GUI = new ChatLoggerGUI(chatLogger, Settings.SettingsManager.ChatLogger.Groups[0], mainView.ChatLogger1List);
					chatLoggerGroup2GUI = new ChatLoggerGUI(chatLogger, Settings.SettingsManager.ChatLogger.Groups[1], mainView.ChatLogger2List);

					mainView.CombatTrackerClearCurrentStats.Hit += (s2, e2) => { try { combatTrackerCurrent.ClearStats(); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.CombatTrackerExportCurrentStats.Hit += (s2, e2) => { try { combatTrackerCurrent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker." + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".xml", true); } catch (Exception ex) { Debug.LogException(ex); } };
					mainView.CombatTrackerClearPersistentStats.Hit += new EventHandler(CombatTrackerClearPersistentStats_Hit);

					mainView.CorpseTrackerClearHistory.Hit += new EventHandler(CorpseTrackerClearHistory_Hit);

					mainView.PlayerTrackerClearHistory.Hit += new EventHandler(PlayerTrackerClearHistory_Hit);

					mainView.ChatLoggerClearHistory.Hit += new EventHandler(ChatLoggerClearHistory_Hit);

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
				CoreManager.Current.PluginInitComplete -= new EventHandler<EventArgs>(Current_PluginInitComplete_VTClassic);
				CoreManager.Current.PluginInitComplete -= new EventHandler<EventArgs>(Current_PluginInitComplete_VTank);
				CoreManager.Current.PluginInitComplete -= new EventHandler<EventArgs>(Current_PluginInitComplete_VVS);
				CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete_VHS);
				CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
				CoreManager.Current.CharacterFilter.Logoff -= new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);
				CoreManager.Current.CommandLineText -= new EventHandler<ChatParserInterceptEventArgs>(Current_CommandLineText);


				// Views, depends on VirindiViewService.dll
				// We dispose these before our other objects (Trackers/Macros) as these probably reference those other objects.
				if (hud != null) hud.Dispose();

				if (chatLoggerGroup1GUI != null) chatLoggerGroup1GUI.Dispose();
				if (chatLoggerGroup2GUI != null) chatLoggerGroup2GUI.Dispose();

				if (consumableTrackerGUI != null) consumableTrackerGUI.Dispose();
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
				if (noFocusFPSManager != null) noFocusFPSManager.Dispose();

				// Loggers
				if (chatLogger != null) chatLogger.Dispose();
				if (chatLogFileWriter != null) chatLogFileWriter.Dispose();

				// Trackers
				if (equipmentTracker != null) equipmentTracker.Dispose();
				if (combatTrackerCurrent != null) combatTrackerCurrent.Dispose();
				if (combatTrackerPersistent != null) combatTrackerPersistent.Dispose();
				if (corpseTracker != null) corpseTracker.Dispose();
				if (playerTracker != null) playerTracker.Dispose();
				if (consumableTracker != null) consumableTracker.Dispose();

				// Macros
				if (openMainPackOnLogin != null) openMainPackOnLogin.Dispose();
				if (maximizeChatOnLogin != null) maximizeChatOnLogin.Dispose();
				if (autoRecharge != null) autoRecharge.Dispose();
				if (autoTradeAccept != null) autoTradeAccept.Dispose();
				if (logOutOnDeath != null) logOutOnDeath.Dispose();

				// General
				if (inventoryLogger != null) inventoryLogger.Dispose();


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
						catch (FileNotFoundException) { CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: " + "Unable to start Inventory Packer. Is Virindi Tank running?", 5); }
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
						corpseTracker.ImportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CorpseTracker.xml");
				}
				catch (Exception ex) { Debug.LogException(ex); }

				try
				{
					if (Settings.SettingsManager.PlayerTracker.Persistent.Value)
						playerTracker.ImportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".PlayerTracker.xml");
				}
				catch (Exception ex) { Debug.LogException(ex); }


				// Load Persistent Logs
				try
				{
					if (Settings.SettingsManager.ChatLogger.Persistent.Value)
					{
						chatLogFileWriter.FileName = PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".ChatLogger.txt";

						List<ILoggerTarget<Loggers.Chat.LoggedChat>> chatLoggers = new List<ILoggerTarget<Loggers.Chat.LoggedChat>>();
						chatLoggers.Add(chatLoggerGroup1GUI);
						chatLoggers.Add(chatLoggerGroup2GUI);
						Loggers.Chat.ChatLogImporter.Import(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".ChatLogger.txt", chatLoggers);					
					}
				}
				catch (Exception ex) { Debug.LogException(ex); }


				foreach (string startupError in startupErrors)
					CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: Startup Error: " + startupError, 5);

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

				if (Settings.SettingsManager.ChatLogger.Persistent.Value)
					chatLogFileWriter.Flush();

				if (Settings.SettingsManager.CombatTracker.ExportOnLogOff.Value)
					combatTrackerCurrent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker." + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".xml");
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Current_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			try
			{
				if (e.Text == null)
					return;

				string lower = e.Text.ToLower();

				if (lower.StartsWith("/mt logoff") || lower.StartsWith("/mt logout"))
				{
					CoreManager.Current.Actions.Logout();
					return;
				}

				if (lower.StartsWith("/mt send "))
				{
					     if (lower.StartsWith("/mt send enter")) PostMessageTools.SendEnter();
					else if (lower.StartsWith("/mt send pause")) PostMessageTools.SendPause();
					else if (lower.StartsWith("/mt send space")) PostMessageTools.SendSpace();
					else if (lower.StartsWith("/mt send cntrl+") && lower.Length >= 16) PostMessageTools.SendCntrl(e.Text[15]);
					else if (lower.StartsWith("/mt send f4")) PostMessageTools.SendF4();
					else if (lower.StartsWith("/mt send f12")) PostMessageTools.SendF12();
					else if (lower.StartsWith("/mt send msg ") && lower.Length > 13) PostMessageTools.SendMsg(e.Text.Substring(13, e.Text.Length - 13));

					return;
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

							if (!int.TryParse(splits[2], out x)) return;
							if (!int.TryParse(splits[3], out y)) return;

							PostMessageTools.SendMouseClick(x, y);
						}
					}

					return;
				}

				if (lower.StartsWith("/mt fellow "))
				{
					if (lower.StartsWith("/mt fellow create ") && lower.Length > 18)
					{
						string fellowName = lower.Substring(18, lower.Length - 18);

						PostMessageTools.SendF12();
						PostMessageTools.SendF4();

						System.Drawing.Rectangle rect = Core.Actions.UIElementRegion(Decal.Adapter.Wrappers.UIElementType.Panels);

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

						if (closest != null)
							Core.Actions.FellowshipRecruit(closest.Id);
					}

					return;
				}

				if (lower.StartsWith("/mt use ") && lower.Length > 8)
				{
					int objectId;
					int useMethod = 0;

					if (!lower.Contains(" on "))
						objectId = FindIdForName(lower.Substring(8, lower.Length - 8));
					else
					{
						string command = lower.Substring(8, lower.Length - 8);
						string first = command.Substring(0, command.IndexOf(" on ", StringComparison.Ordinal));
						string second = command.Substring(first.Length + 4, command.Length - first.Length - 4);
						Debug.WriteToChat("command: " + command.Length + " '" + command + "', first: " + first.Length + " '" + first + "', second: " + second.Length + " '" + second + "'");
						objectId = FindIdForName(first);
						useMethod = FindIdForName(second);
					}

					if (objectId == -1 || useMethod == -1)
						return;

					if (useMethod == 0)
						CoreManager.Current.Actions.UseItem(objectId, 0);
					else
						CoreManager.Current.Actions.UseItem(objectId, 1, useMethod);

					return;
				}

				if (lower.StartsWith("/mt give ") && lower.Contains(" to "))
				{
					int objectId;
					int destinationId;

					string command = lower.Substring(9, lower.Length - 9);
					string first = command.Substring(0, command.IndexOf(" to ", StringComparison.Ordinal));
					string second = command.Substring(first.Length + 4, command.Length - first.Length - 4);

					objectId = FindIdForName(first, true, false);
					destinationId = FindIdForName(second, false);

					if (objectId == -1 || destinationId == -1)
						return;

					CoreManager.Current.Actions.GiveItem(objectId, destinationId);

					return;
				}

				if (lower.StartsWith("/mt combatstate ") && lower.Length > 16)
				{
					string state = lower.Substring(16, lower.Length - 16);

					if (state == "magic") CoreManager.Current.Actions.SetCombatMode(CombatState.Magic);
					if (state == "melee") CoreManager.Current.Actions.SetCombatMode(CombatState.Melee);
					if (state == "missile") CoreManager.Current.Actions.SetCombatMode(CombatState.Missile);
					if (state == "peace") CoreManager.Current.Actions.SetCombatMode(CombatState.Peace);
				}

				if (lower.StartsWith("/mt autopack") && inventoryPacker != null) inventoryPacker.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		int FindIdForName(string name, bool searchInInventory = true, bool searchEnvironment = true)
		{
			if (searchInInventory)
			{
				foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (wo.Name.ToLower().Contains(name.ToLower()))
						return wo.Id;
				}
			}

			if (searchEnvironment)
			{
				WorldObject closestObject = Util.GetClosestObject(name);

				if (closestObject != null)
					return closestObject.Id;
			}

			return -1;
		}

		void FellowCreate_Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				System.Drawing.Rectangle rect = Core.Actions.UIElementRegion(Decal.Adapter.Wrappers.UIElementType.Panels);

				CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(FellowCreate_Current_RenderFrame);
				PostMessageTools.SendMouseClick(rect.X + 145, rect.Y + 343);
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

		void CorpseTrackerClearHistory_Hit(object sender, EventArgs e)
		{
			try
			{
				corpseTracker.ClearStats();

				FileInfo fileInfo = new FileInfo(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CorpseTracker.xml");

				if (fileInfo.Exists)
				{
					fileInfo.Delete();

					CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: " + "File deleted: " + fileInfo.FullName, 5);
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

					CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: " + "File deleted: " + fileInfo.FullName, 5);
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
			if (Settings.SettingsManager.CombatTracker.Persistent.Value)
				combatTrackerPersistent.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker.xml");

			if (Settings.SettingsManager.CorpseTracker.Persistent.Value)
				corpseTracker.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CorpseTracker.xml");

			if (Settings.SettingsManager.PlayerTracker.Persistent.Value)
				playerTracker.ExportStats(PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".PlayerTracker.xml");
		}
	}
}
