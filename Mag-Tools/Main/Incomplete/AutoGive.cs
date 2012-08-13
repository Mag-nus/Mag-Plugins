/*
using System;

using Decal.Adapter;

namespace MagTools.Macros
{
	class AutoGive : IDisposable
	{
		public bool Enabled { private get; set; }

		public AutoGive()
		{
			try
			{
				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.MoveObject += new EventHandler<Decal.Adapter.Wrappers.MoveObjectEventArgs>(WorldFilter_MoveObject);
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
					CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
					CoreManager.Current.WorldFilter.MoveObject -= new EventHandler<Decal.Adapter.Wrappers.MoveObjectEventArgs>(WorldFilter_MoveObject);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void WorldFilter_CreateObject(object sender, Decal.Adapter.Wrappers.CreateObjectEventArgs e)
		{
			try
			{
				// Catch when mules log in
				if (!Enabled)
					return;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_MoveObject(object sender, Decal.Adapter.Wrappers.MoveObjectEventArgs e)
		{
			try
			{
				// Catch when you become in range of a mule
				if (!Enabled)
					return;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
*/