using System;
using System.Collections.Generic;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools
{
	class PrintItemInfoOnUserIdent : IDisposable
	{
		public bool Enabled { private get; set; }

		public PrintItemInfoOnUserIdent()
		{
			try
			{
				CoreManager.Current.ItemSelected += new EventHandler<ItemSelectedEventArgs>(Current_ItemSelected);
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
					CoreManager.Current.ItemSelected -= new EventHandler<ItemSelectedEventArgs>(Current_ItemSelected);
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		Dictionary<int, DateTime> itemsSelected = new Dictionary<int, DateTime>();

		void Current_ItemSelected(object sender, ItemSelectedEventArgs e)
		{
			try
			{
				if (!Enabled)
					return;

				if (e.ItemGuid == 0)
					return;

				if (itemsSelected.ContainsKey(e.ItemGuid))
					itemsSelected[e.ItemGuid] = DateTime.Now;
				else
					itemsSelected.Add(e.ItemGuid, DateTime.Now);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, Decal.Adapter.Wrappers.ChangeObjectEventArgs e)
		{
			try
			{
				if (!Enabled)
					return;

				if (e.Change != WorldChangeType.IdentReceived)
					return;

				// Remove id's that have been selected more than 10 seconds ago
				while (true)
				{
					int idToRemove = 0;

					foreach (KeyValuePair<int, DateTime> pair in itemsSelected)
					{
						if (pair.Value + TimeSpan.FromSeconds(10) < DateTime.Now)
						{
							idToRemove = pair.Key;
							break;
						}
					}

					if (idToRemove == 0)
						break;
					else
						itemsSelected.Remove(idToRemove);
				}

				if (!itemsSelected.ContainsKey(e.Changed.Id))
					return;

				itemsSelected.Remove(e.Changed.Id);

				if (e.Changed.ObjectClass == ObjectClass.Corpse ||
					e.Changed.ObjectClass == ObjectClass.Door ||
					e.Changed.ObjectClass == ObjectClass.Foci ||
					e.Changed.ObjectClass == ObjectClass.Housing ||
					e.Changed.ObjectClass == ObjectClass.Lifestone ||
					e.Changed.ObjectClass == ObjectClass.Npc ||
					e.Changed.ObjectClass == ObjectClass.Portal ||
					e.Changed.ObjectClass == ObjectClass.Vendor)
					return;

				PrintItemInfo printItemInfo = new PrintItemInfo(e.Changed);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
