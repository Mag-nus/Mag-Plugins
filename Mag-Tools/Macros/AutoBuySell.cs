using System;

using Decal.Adapter;

namespace MagTools.Macros
{
	class AutoBuySell : IDisposable
	{
		public AutoBuySell()
		{
			try
			{
				CoreManager.Current.WorldFilter.ApproachVendor += new EventHandler<Decal.Adapter.Wrappers.ApproachVendorEventArgs>(WorldFilter_ApproachVendor);
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
					CoreManager.Current.WorldFilter.ApproachVendor -= new EventHandler<Decal.Adapter.Wrappers.ApproachVendorEventArgs>(WorldFilter_ApproachVendor);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		void WorldFilter_ApproachVendor(object sender, Decal.Adapter.Wrappers.ApproachVendorEventArgs e)
		{
			try
			{

			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
