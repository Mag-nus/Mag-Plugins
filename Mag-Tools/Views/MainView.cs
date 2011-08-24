using System;

using MyClasses.MetaViewWrappers;

namespace MagTools.Views
{
	// View is the path to the xml file that contains info on how to draw our in-game plugin. The xml contains the name and icon our plugin shows in-game.
	// The view here is MagTools.mainView.xml because our projects default namespace is SamplePlugin, and the file name is mainView.xml.
	// The other key here is that mainView.xml must be included as an embeded resource. If its not, your plugin will not show up in-game.
	[MVView("MagTools.Views.mainView.xml")]
	class MainView : IDisposable
	{
		public MainView()
		{
			try
			{
				//Display the view
				MVWireupHelper.WireupStart(this, PluginCore.host);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public void Dispose()
		{
			try
			{
				//Remove the view
				MVWireupHelper.WireupEnd(this);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}


		[MVControlReference("ManaList")]
		private IList manaList = null;
		public IList ManaList { get { return manaList; } }

		[MVControlReference("ManaTotal")]
		private IStaticText manaTotal = null;
		public IStaticText ManaTotal { get { return manaTotal; } }

		//         <control progid="DecalControls.CheckBox" name="RefillMana" left="170" top="385" width="130" height="20" text="Auto Recharge On/Off" checked="true" />
		//[MVControlReference("RefillMana")]
		//private ICheckBox refillMana = null;
		//public ICheckBox RefillMana { get { return refillMana; } }


		[MVControlReference("VersionLabel")]
		private IStaticText versionLabel = null;
		public IStaticText VersionLabel { get { return versionLabel; } }

		[MVControlReference("DebugEnabled")]
		private ICheckBox debugEnabled = null;
		public ICheckBox DebugEnabled { get { return debugEnabled; } }
	}
}
