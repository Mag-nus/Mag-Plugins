using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

namespace MagTools
{
    //Attaches events from core
	[WireUpBaseEvents]

	// FriendlyName is the name that will show up in the plugins list of the decal agent (the one in windows, not in-game)
	[FriendlyName("Mag-Tools")]
	public class PluginCore : PluginBase
	{
		internal static string PluginName = "Mag-Tools";

		private static DirectoryInfo pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\" + PluginCore.PluginName);
		internal static DirectoryInfo PluginPersonalFolder
		{
			get
			{
				if (!pluginPersonalFolder.Exists)
					pluginPersonalFolder.Create();
				
				return pluginPersonalFolder;
			}
		}

		internal static Decal.Adapter.Wrappers.PluginHost host = null;

		// View
		private Views.MainView mainView;

		// General
		private ChatFilter chatFilter;
		private OpenMainPackOnLogin openMainPackOnLogin;

		// Macros
		private Macros.AutoBuySell autoBuySell;
		private Macros.AutoPack autoPack;
		private Macros.AutoGive autoGive;
		private Macros.AutoTradeAdd autoTradeAdd;
		private Macros.AutoTradeAccept autoTradeAccept;

		// Trackers
		private Trackers.ManaTracker manaTracker;
		private Trackers.ManaTrackerGUI manaTrackerGUI;

		//
		private VirindiTools.ItemInfoOnIdent itemInfoOnIdent;

		// Settings
		private Settings.XmlFile pluginConfigFile;

		Collection<string> startupErrors = new Collection<string>();

		/// <summary>
		/// This is called when the plugin is started up. This happens only once.
		/// </summary>
		protected override void Startup()
		{
			try
			{
				host = Host;

				// View
				mainView = new Views.MainView();
				mainView.OptionEnabled += new Action<Option>(mainView_OptionEnabled);
				mainView.OptionDisabled += new Action<Option>(mainView_OptionDisabled);

				// Settings
				FileInfo pluginConfigFileInfo = new FileInfo(PluginPersonalFolder.FullName + @"\" + PluginName + ".xml");
				pluginConfigFile = new Settings.XmlFile(pluginConfigFileInfo.FullName, PluginName);

				// General
				chatFilter = new ChatFilter();
				openMainPackOnLogin = new OpenMainPackOnLogin();

				// Macros
				autoTradeAccept = new Macros.AutoTradeAccept();

				// Trackers
				manaTracker = new Trackers.ManaTracker(host);
				manaTrackerGUI = new Trackers.ManaTrackerGUI(manaTracker, mainView);
			}
			catch (Exception ex) { Debug.LogException(ex); }

			// We init objects that have dependancies here. These might crash out.
			try
			{
				itemInfoOnIdent = new VirindiTools.ItemInfoOnIdent(host);
			}
			catch (System.IO.FileNotFoundException ex) { startupErrors.Add("itemInfoOnIdent failed to load: " + ex.Message); }
			catch (Exception ex) { Debug.LogException(ex); }

			try
			{
				autoPack = new Macros.AutoPack();
			}
			catch (System.IO.FileNotFoundException ex) { startupErrors.Add("autoPack failed to load: " + ex.Message); }
			catch (Exception ex) { Debug.LogException(ex); }

			try
			{
				autoGive = new Macros.AutoGive();
			}
			catch (System.IO.FileNotFoundException ex) { startupErrors.Add("autoGive failed to load: " + ex.Message); }
			catch (Exception ex) { Debug.LogException(ex); }

			try
			{
				autoTradeAdd = new Macros.AutoTradeAdd();
			}
			catch (System.IO.FileNotFoundException ex) { startupErrors.Add("autoTradeAdd failed to load: " + ex.Message); }
			catch (Exception ex) { Debug.LogException(ex); }

			try
			{
				autoBuySell = new Macros.AutoBuySell();
			}
			catch (System.IO.FileNotFoundException ex) { startupErrors.Add("autoBuySell failed to load: " + ex.Message); }
			catch (Exception ex) { Debug.LogException(ex); }

			try
			{
				AddOptionsToGUI();
				LoadOptionsFromConfig();


				System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
				System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
				if (mainView != null) mainView.VersionLabel.Text = "Version: " + fvi.ProductVersion;
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
				//
				if (itemInfoOnIdent != null) itemInfoOnIdent.Dispose();

				// Trackers
				if (manaTrackerGUI != null) manaTrackerGUI.Dispose();
				if (manaTracker != null) manaTracker.Dispose();

				// Macros
				if (autoBuySell != null) autoBuySell.Dispose();
				if (autoTradeAdd != null) autoTradeAdd.Dispose();
				if (autoGive != null) autoGive.Dispose();
				if (autoPack != null) autoPack.Dispose();
				if (autoTradeAccept != null) autoTradeAccept.Dispose();

				// General
				if (chatFilter != null) chatFilter.Dispose();
				if (openMainPackOnLogin != null) openMainPackOnLogin.Dispose();

				// Settings
				if (pluginConfigFile != null) pluginConfigFile.Dispose();

				// View
				if (mainView != null)
				{
					mainView.OptionEnabled -= new Action<Option>(mainView_OptionEnabled);
					mainView.OptionDisabled -= new Action<Option>(mainView_OptionDisabled);
					mainView.Dispose();
				}

				host = null;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		[BaseEvent("LoginComplete", "CharacterFilter")]
		private void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				Host.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Plugin now online. Server population: " + Core.CharacterFilter.ServerPopulation, 5);

				foreach (string startupError in startupErrors)
				{
					Host.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: Startup Error: " + startupError, 5);
				}

				startupErrors.Clear();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private void AddOptionsToGUI()
		{
			if (mainView == null)
				return;

			mainView.AddOption(Option.FilterAttackEvades);
			mainView.AddOption(Option.FilterDefenseEvades);
			mainView.AddOption(Option.FilterAttackResists);
			mainView.AddOption(Option.FilterDefenseResists);
			mainView.AddOption(Option.FilterSpellCasting);
			mainView.AddOption(Option.FilterCompUsage);
			mainView.AddOption(Option.FilterSpellExpires);
			mainView.AddOption(Option.FilterNPKFails);
			mainView.AddOption(Option.FilterVendorTells);

			mainView.AddOption(Option.ItemInfoOnIdent);

			mainView.AddOption(Option.AutoBuySellEnabled);

			mainView.AddOption(Option.AutoTradeAdd);

			mainView.AddOption(Option.AutoTradeAcceptEnabled);

			mainView.AddOption(Option.OpenMainPackOnLogin);
			mainView.AddOption(Option.DebuggingEnabled);
		}

		void mainView_OptionEnabled(Option obj)
		{
			if (pluginConfigFile == null)
				return;

			pluginConfigFile.SetBoolean(obj.Xpath, true);

			LoadOptionsFromConfig();
		}

		void mainView_OptionDisabled(Option obj)
		{
			if (pluginConfigFile == null)
				return;

			pluginConfigFile.SetBoolean(obj.Xpath, false);

			LoadOptionsFromConfig();
		}

		private void LoadOptionsFromConfig()
		{
			if (pluginConfigFile == null)
				return;

			if (mainView != null && chatFilter != null)
			{
				mainView.SetOption(Option.FilterAttackEvades, pluginConfigFile.GetBoolean(Option.FilterAttackEvades.Xpath));
				chatFilter.FilterAttackEvades = pluginConfigFile.GetBoolean(Option.FilterAttackEvades.Xpath);

				mainView.SetOption(Option.FilterDefenseEvades, pluginConfigFile.GetBoolean(Option.FilterDefenseEvades.Xpath));
				chatFilter.FilterDefenseEvades = pluginConfigFile.GetBoolean(Option.FilterDefenseEvades.Xpath);

				mainView.SetOption(Option.FilterAttackResists, pluginConfigFile.GetBoolean(Option.FilterAttackResists.Xpath));
				chatFilter.FilterAttackResists = pluginConfigFile.GetBoolean(Option.FilterAttackResists.Xpath);

				mainView.SetOption(Option.FilterDefenseResists, pluginConfigFile.GetBoolean(Option.FilterDefenseResists.Xpath));
				chatFilter.FilterDefenseResists = pluginConfigFile.GetBoolean(Option.FilterDefenseResists.Xpath);

				mainView.SetOption(Option.FilterSpellCasting, pluginConfigFile.GetBoolean(Option.FilterSpellCasting.Xpath));
				chatFilter.FilterSpellCasting = pluginConfigFile.GetBoolean(Option.FilterSpellCasting.Xpath);

				mainView.SetOption(Option.FilterCompUsage, pluginConfigFile.GetBoolean(Option.FilterCompUsage.Xpath));
				chatFilter.FilterCompUsage = pluginConfigFile.GetBoolean(Option.FilterCompUsage.Xpath);

				mainView.SetOption(Option.FilterSpellExpires, pluginConfigFile.GetBoolean(Option.FilterSpellExpires.Xpath));
				chatFilter.FilterSpellExpires = pluginConfigFile.GetBoolean(Option.FilterSpellExpires.Xpath);

				mainView.SetOption(Option.FilterNPKFails, pluginConfigFile.GetBoolean(Option.FilterNPKFails.Xpath));
				chatFilter.FilterNPKFails = pluginConfigFile.GetBoolean(Option.FilterNPKFails.Xpath);

				mainView.SetOption(Option.FilterVendorTells, pluginConfigFile.GetBoolean(Option.FilterVendorTells.Xpath));
				chatFilter.FilterVendorTells = pluginConfigFile.GetBoolean(Option.FilterVendorTells.Xpath);
			}

			if (mainView != null && itemInfoOnIdent != null)
			{
				mainView.SetOption(Option.ItemInfoOnIdent, pluginConfigFile.GetBoolean(Option.ItemInfoOnIdent.Xpath));
				itemInfoOnIdent.Enabled = pluginConfigFile.GetBoolean(Option.ItemInfoOnIdent.Xpath);
			}

			if (mainView != null && autoBuySell != null)
			{
				mainView.SetOption(Option.AutoBuySellEnabled, pluginConfigFile.GetBoolean(Option.AutoBuySellEnabled.Xpath));
				autoBuySell.Enabled = pluginConfigFile.GetBoolean(Option.AutoBuySellEnabled.Xpath);
			}

			if (mainView != null && autoTradeAdd != null)
			{
				mainView.SetOption(Option.AutoTradeAdd, pluginConfigFile.GetBoolean(Option.AutoTradeAdd.Xpath));
				autoTradeAdd.Enabled = pluginConfigFile.GetBoolean(Option.AutoTradeAdd.Xpath);
			}

			if (mainView != null && autoTradeAccept != null)
			{
				mainView.SetOption(Option.AutoTradeAcceptEnabled, pluginConfigFile.GetBoolean(Option.AutoTradeAcceptEnabled.Xpath));
				autoTradeAccept.Enabled = pluginConfigFile.GetBoolean(Option.AutoTradeAcceptEnabled.Xpath);

				IEnumerable<string> whitelist = pluginConfigFile.GetCollection(OptionGroup.AutoTradeAccept.Xpath + "/Whitelist");

				foreach (string obj in whitelist)
				{
					autoTradeAccept.AddToWhitelist(new System.Text.RegularExpressions.Regex(obj));
				}
			}

			if (mainView != null && openMainPackOnLogin != null)
			{
				mainView.SetOption(Option.OpenMainPackOnLogin, pluginConfigFile.GetBoolean(Option.OpenMainPackOnLogin.Xpath));
				openMainPackOnLogin.Enabled = pluginConfigFile.GetBoolean(Option.OpenMainPackOnLogin.Xpath);
			}

			if (mainView != null)
			{
				mainView.SetOption(Option.DebuggingEnabled, pluginConfigFile.GetBoolean(Option.DebuggingEnabled.Xpath));
				Debug.Enabled = pluginConfigFile.GetBoolean(Option.DebuggingEnabled.Xpath);
			}
		}
	}
}
