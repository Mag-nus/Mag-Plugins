using System;
using System.Collections.Generic;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.ItemInfo
{
	class DetectItemsIdentifiedByUser : IDisposable
	{
		public event EventHandler<ItemInfoIdentArgs> ItemIdentified;

		public DetectItemsIdentifiedByUser()
		{
			try
			{
				CoreManager.Current.WindowMessage += new EventHandler<WindowMessageEventArgs>(Current_WindowMessage);
				CoreManager.Current.ItemSelected += new EventHandler<ItemSelectedEventArgs>(Current_ItemSelected);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
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
					CoreManager.Current.WindowMessage -= new EventHandler<WindowMessageEventArgs>(Current_WindowMessage);
					CoreManager.Current.ItemSelected -= new EventHandler<ItemSelectedEventArgs>(Current_ItemSelected);
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		DateTime lastLeftClick = DateTime.MinValue;

		const int WM_LBUTTONDOWN = 0x201;

		void Current_WindowMessage(object sender, WindowMessageEventArgs e)
		{
			try
			{
				if (e.Msg == WM_LBUTTONDOWN)
					lastLeftClick = DateTime.Now;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		readonly Dictionary<int, DateTime> itemsSelected = new Dictionary<int, DateTime>();

		void Current_ItemSelected(object sender, ItemSelectedEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.ItemInfoOnIdent.Enabled.Value)
					return;

				if (e.ItemGuid == 0)
					return;

				if (itemsSelected.ContainsKey(e.ItemGuid))
					itemsSelected[e.ItemGuid] = DateTime.Now;
				else
					itemsSelected.Add(e.ItemGuid, DateTime.Now);

				if (Settings.SettingsManager.ItemInfoOnIdent.LeftClickIdent.Value && DateTime.Now - lastLeftClick < TimeSpan.FromSeconds(1))
				{
					CoreManager.Current.Actions.RequestId(e.ItemGuid);
					lastLeftClick = DateTime.MinValue;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.ItemInfoOnIdent.Enabled.Value)
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

				if (ItemIdentified != null)
				{
					ItemInfoIdentArgs itemInfoIdentArgs = new ItemInfoIdentArgs(e.Changed);

					ItemIdentified(this, itemInfoIdentArgs);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
