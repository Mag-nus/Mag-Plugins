using System;

namespace MagTools
{
	class DebugGUI : IDisposable
	{
		private Views.MainView mainView;

		public DebugGUI(Views.MainView mainView)
		{
			try
			{
				this.mainView = mainView;

				Debug.DebugEnabled = mainView.DebugEnabled.Checked;

				mainView.DebugEnabled.Change += new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(DebugEnabled_Change);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public void Dispose()
		{
			try
			{
				mainView.DebugEnabled.Change -= new EventHandler<MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs>(DebugEnabled_Change);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void DebugEnabled_Change(object sender, MyClasses.MetaViewWrappers.MVCheckBoxChangeEventArgs e)
		{
			try
			{
				Debug.DebugEnabled = e.Checked;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
