using System;
using System.IO;
using System.Collections.ObjectModel;

using Decal.Adapter;

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

		// Macros
		Macros.OpenMainPackOnLogin openMainPackOnLogin;
		Macros.AutoRecharge autoRecharge;
		Macros.AutoTradeAccept autoTradeAccept;
	
		// Trackers
		Trackers.Mana.ManaTracker manaTracker;
		public Trackers.Mana.IManaTracker ManaTracker { get { return manaTracker; } }
		Trackers.Combat.CombatTracker combatTracker;

		// Misc
		Client.WindowFrameRemover windowFrameRemover;
		Client.WindowMover windowMover;

		// These objects may reference other plugins
		ItemInfo.ItemInfoPrinter itemInfoPrinter;

		// Virindi Classic Looter Extensions, depends on VTClassic.dll
		Macros.InventoryPacker inventoryPacker;
		public Macros.IInventoryPacker InventoryPacker { get { return inventoryPacker; } }
		Macros.AutoTradeAdd autoTradeAdd;
		Macros.AutoBuySell autoBuySell;

		// Virindi Tank Extensions, depends on utank2-i.dll
		Macros.ChestLooter chestLooter;
		public Macros.IChestLooter ChestLooter { get { return chestLooter; } }

		// Views, depends on VirindiViewService.dll
		Views.MainView mainView;
		Views.ManaTrackerGUI manaTrackerGUI;
		Views.CombatTrackerGUI combatTrackerGUI;

		readonly Collection<string> startupErrors = new Collection<string>();

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

				// General
				chatFilter = new ChatFilter();

				// Macros
				openMainPackOnLogin = new Macros.OpenMainPackOnLogin();
				autoRecharge = new Macros.AutoRecharge();
				autoTradeAccept = new Macros.AutoTradeAccept();

				// Trackers
				manaTracker = new Trackers.Mana.ManaTracker();
				combatTracker = new Trackers.Combat.CombatTracker();

				// Misc
				windowFrameRemover = new Client.WindowFrameRemover();
				windowMover = new Client.WindowMover();
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
				// These objects may reference other plugins
				try
				{
					itemInfoPrinter = new ItemInfo.ItemInfoPrinter();
				}
				catch (FileNotFoundException ex) { startupErrors.Add("itemInfoPrinter failed to load: " + ex.Message); }
				catch (Exception ex) { Debug.LogException(ex); }

				// Virindi Classic Looter Extensions, depends on VTClassic.dll
				try
				{
					inventoryPacker = new Macros.InventoryPacker();
					autoTradeAdd = new Macros.AutoTradeAdd();
					autoBuySell = new Macros.AutoBuySell();
				}
				catch (FileNotFoundException ex) { startupErrors.Add("Object failed to load: " + ex.Message + Environment.NewLine + "Did you copy VTClassic.dll to the same folder as MagTools.dll?" + Environment.NewLine + "Is Virindi Tank running?"); }
				catch (Exception ex) { Debug.LogException(ex); }

				// Virindi Tank Extensions, depends on utank2-i.dll
				try
				{
					chestLooter = new Macros.ChestLooter();
				}
				catch (FileNotFoundException ex) { startupErrors.Add("chestLooter failed to load: " + ex.Message + Environment.NewLine + "Is Virindi Tank running?"); }
				catch (Exception ex) { Debug.LogException(ex); }

				// Views, depends on VirindiViewService.dll
				try
				{
					mainView = new Views.MainView(combatTracker);
					manaTrackerGUI = new Views.ManaTrackerGUI(manaTracker, mainView);
					combatTrackerGUI = new Views.CombatTrackerGUI(combatTracker, mainView);

					System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
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
				CoreManager.Current.PluginInitComplete -= new EventHandler<EventArgs>(Current_PluginInitComplete);
				CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);

				// Views, depends on VirindiViewService.dll
				// We dispose these before our other objects (Trackers/Macros) as these probably reference those other objects.
				if (combatTrackerGUI != null) combatTrackerGUI.Dispose();
				if (manaTrackerGUI != null) manaTrackerGUI.Dispose();
				if (mainView != null) mainView.Dispose(); // We dispose this last in the Views as the other Views reference it.

				// These objects may reference other plugins
				if (itemInfoPrinter != null) itemInfoPrinter.Dispose();

				// Virindi Classic Looter Extensions, depends on VTClassic.dll
				if (inventoryPacker != null) inventoryPacker.Dispose();
				if (autoTradeAdd != null) autoTradeAdd.Dispose();
				if (autoBuySell != null) autoBuySell.Dispose();

				// Virindi Tank Extensions, depends on utank2-i.dll
				if (chestLooter != null) chestLooter.Dispose();

				// Misc
				if (windowFrameRemover != null) windowFrameRemover.Dispose();
				if (windowMover != null) windowMover.Dispose();

				// Trackers
				if (manaTracker != null) manaTracker.Dispose();
				if (combatTracker != null) combatTracker.Dispose();

				// Macros
				if (openMainPackOnLogin != null) openMainPackOnLogin.Dispose();
				if (autoRecharge != null) autoRecharge.Dispose();
				if (autoTradeAccept != null) autoTradeAccept.Dispose();

				// General
				if (chatFilter != null) chatFilter.Dispose();

				Current = null;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: " + "Plugin now online. Server population: " + Core.CharacterFilter.ServerPopulation, 5);

				try
				{
					if (InventoryPacker != null)
					{
						// http://delphi.about.com/od/objectpascalide/l/blvkc.htm
						VirindiHotkeySystem.VHotkeyInfo packInventoryHotkey = new VirindiHotkeySystem.VHotkeyInfo("Mag-Tools", true, "Pack Inventory", "Triggers the Inventory Packer Macro", 0x50, false, true, false);

						VirindiHotkeySystem.VHotkeySystem.InstanceReal.AddHotkey(packInventoryHotkey);

						packInventoryHotkey.Fired2 += new EventHandler<VirindiHotkeySystem.VHotkeyInfo.cEatableFiredEventArgs>(PackInventoryHotkey_Fired2);
					}
				}
				catch (FileNotFoundException ex) { startupErrors.Add("Pack Inventory hot key failed to bind: " + ex.Message + ". Is Virindi Hotkey System running?"); }
				catch (Exception ex) { Debug.LogException(ex); }

				foreach (string startupError in startupErrors)
				{
					CoreManager.Current.Actions.AddChatText("<{" + PluginName + "}>: Startup Error: " + startupError, 5);
				}

				startupErrors.Clear();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void PackInventoryHotkey_Fired2(object sender, VirindiHotkeySystem.VHotkeyInfo.cEatableFiredEventArgs e)
		{
			try
			{
				InventoryPacker.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
