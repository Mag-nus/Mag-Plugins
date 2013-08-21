using System;

using Decal.Adapter;

using Mag.Shared;

namespace MagTools.Macros
{
	class LogOutOnDeath : IDisposable
	{
		public LogOutOnDeath()
		{
			try
			{
				CoreManager.Current.CharacterFilter.Death += new EventHandler<Decal.Adapter.Wrappers.DeathEventArgs>(CharacterFilter_Death);
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
					CoreManager.Current.CharacterFilter.Death -= new EventHandler<Decal.Adapter.Wrappers.DeathEventArgs>(CharacterFilter_Death);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void CharacterFilter_Death(object sender, Decal.Adapter.Wrappers.DeathEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.Misc.LogOutOnDeath.Value)
					return;

				CoreManager.Current.Actions.Logout();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
