using System;

using Decal.Adapter;

namespace MagTools.Macros
{
	class AutoPack : IDisposable
	{
		public bool Enabled { private get; set; }

		public AutoPack()
		{
			try
			{
				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
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
					CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		void WorldFilter_CreateObject(object sender, Decal.Adapter.Wrappers.CreateObjectEventArgs e)
		{
			try
			{
				// Catch when an object is given to us by creating it in our inventory

				// Called when purchasing an item from a vendor.
				// Called when someone hands you an item.
				// Called when a character first logs in.

				if (!Enabled)
					return;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, Decal.Adapter.Wrappers.ChangeObjectEventArgs e)
		{
			try
			{
				// Catch when we pickup an object

				// StorageChange when picking up an item.
				// StorageChange when receiving an item via trade.
				// StorageChange when you move an item in your own inventory.
				// StorageChange when you dequip an item.

				if (!Enabled)
					return;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
