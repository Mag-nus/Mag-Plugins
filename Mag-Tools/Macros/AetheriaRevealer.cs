using System;
using System.Windows.Forms;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class AetheriaRevealer : IDisposable
	{
		readonly Timer timer = new Timer();

		public AetheriaRevealer()
		{
			try
			{
				timer.Tick += new EventHandler(timer_Tick);
				timer.Interval = 4000;

				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				CoreManager.Current.CharacterFilter.Logoff += new EventHandler<LogoffEventArgs>(CharacterFilter_Logoff);
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
					CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<CreateObjectEventArgs>(WorldFilter_CreateObject);
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
					CoreManager.Current.CharacterFilter.Logoff -= new EventHandler<LogoffEventArgs>(CharacterFilter_Logoff);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void WorldFilter_CreateObject(object sender, CreateObjectEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.InventoryManagement.AetheriaRevealer.Value)
					return;

				if (e.New.Name == "Coalesced Aetheria")
					timer.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.InventoryManagement.AetheriaRevealer.Value)
					return;

				if (e.Changed.Name == "Coalesced Aetheria")
					timer.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_Logoff(object sender, LogoffEventArgs e)
		{
			try
			{
				timer.Stop();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void timer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.InventoryManagement.AetheriaRevealer.Value)
				{
					timer.Stop();
					return;
				}

				if (CoreManager.Current.Actions.CombatMode != CombatState.Peace)
					return;

				if (CoreManager.Current.Actions.OpenedContainer != 0)
					return;

				if (CoreManager.Current.Actions.BusyState != 0)
					return;

				int coalescedAetheriaId = 0;

				foreach (var wo in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (wo.Name == "Coalesced Aetheria")
					{
						coalescedAetheriaId = wo.Id;
						break;
					}
				}

				if (coalescedAetheriaId == 0)
				{
					timer.Stop();
					return;
				}

				int aetheriaManaStoneId = 0;

				foreach (var wo in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (wo.Name == "Aetheria Mana Stone")
					{
						aetheriaManaStoneId = wo.Id;
						break;
					}
				}

				if (aetheriaManaStoneId != 0)
					CoreManager.Current.Actions.UseItem(aetheriaManaStoneId, 1, coalescedAetheriaId);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
