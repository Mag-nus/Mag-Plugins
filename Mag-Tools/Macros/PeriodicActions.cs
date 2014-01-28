using System;
using System.Windows.Forms;

using Decal.Adapter;

using Mag.Shared;

namespace MagTools.Macros
{
	class PeriodicActions : IDisposable
	{
		Timer timer = new Timer();

		public PeriodicActions()
		{
			try
			{
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);

				timer.Tick += new EventHandler(timer_Tick);
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

					timer.Tick -= new EventHandler(timer_Tick);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{

			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void timer_Tick(object sender, EventArgs e)
		{
			try
			{

			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
