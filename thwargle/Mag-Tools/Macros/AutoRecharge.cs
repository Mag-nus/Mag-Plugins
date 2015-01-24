using System;

using Decal.Adapter;

using Mag.Shared;

namespace MagTools.Macros
{
	class AutoRecharge : IDisposable
	{
		public AutoRecharge()
		{
			try
			{
				CoreManager.Current.ChatBoxMessage += new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
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
					CoreManager.Current.ChatBoxMessage -= new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.ManaManagement.AutoRecharge.Value)
					return;

				if (e.Text.StartsWith("You say, ") || e.Text.Contains("says, \""))
					return;

				// Your Gold Olthoi Koujia Sleeves is low on Mana.
				if (e.Text.Contains("Your") && e.Text.Contains(" is low on Mana."))
					ManaRecharger.Instance.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
