using System;

using Decal.Adapter;

namespace MagTools
{
	class OpenMainPackOnLogin : IDisposable
	{
		public bool Enabled { private get; set; }

		public OpenMainPackOnLogin()
		{
			try
			{
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private bool _disposed = false;

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
			if (!_disposed)
			{
				if (disposing)
				{
					CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				if (!Enabled)
					return;

				CoreManager.Current.Actions.UseItem(CoreManager.Current.CharacterFilter.Id, 0);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
