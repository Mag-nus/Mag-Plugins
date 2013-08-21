using System;

using Mag.Shared;

using Decal.Adapter;

namespace MagTools.Macros
{
	class OpenMainPackOnLogin : IDisposable
	{
		public OpenMainPackOnLogin()
		{
			try
			{
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
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
					CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.Misc.OpenMainPackOnLogin.Value)
					return;

				CoreManager.Current.Actions.UseItem(CoreManager.Current.CharacterFilter.Id, 0);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
