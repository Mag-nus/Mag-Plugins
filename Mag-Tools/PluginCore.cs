using System;

using Decal.Adapter;
using Decal.Adapter.Wrappers;
using MyClasses.MetaViewWrappers;


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

		internal static Decal.Adapter.Wrappers.PluginHost host = null;
		internal static Decal.Adapter.CoreManager core = null;

		internal static  Views.MainView mainView;

		private ManaTracker.Tracker manaTracker;
		private VirindiTools.ItemInfoOnIdent itemInfoOnIdent;

		/// <summary>
		/// This is called when the plugin is started up. This happens only once.
		/// </summary>
		protected override void Startup()
		{
			try
			{
				host = Host;
				core = Core;

				mainView = new Views.MainView();

				System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
				System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
				PluginCore.mainView.VersionLabel.Text = "Version: " + fvi.ProductVersion;

				manaTracker = new ManaTracker.Tracker();
				itemInfoOnIdent = new VirindiTools.ItemInfoOnIdent();
				
			}
			catch (Exception ex) { Util.LogError(ex); }
		}

		/// <summary>
		/// This is called when the plugin is shut down. This happens only once.
		/// </summary>
		protected override void Shutdown()
		{
			try
			{
				itemInfoOnIdent.Dispose();
				manaTracker.Dispose();

				mainView.Dispose();

				host = null;
				core = null;
			}
			catch (Exception ex) { Util.LogError(ex); }
		}

		[BaseEvent("LoginComplete", "CharacterFilter")]
		private void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				Util.WriteToChat("Plugin now online. Server population: " + Core.CharacterFilter.ServerPopulation);

			}
			catch (Exception ex) { Util.LogError(ex); }
		}
	}
}
