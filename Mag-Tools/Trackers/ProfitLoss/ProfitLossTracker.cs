using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

using Mag.Shared;

namespace MagTools.Trackers.ProfitLoss
{
	class ProfitLossTracker : IItemTracker<TrackedProfitLoss>, IDisposable
	{
		/// <summary>
		/// TThis is raised when one or more items have been added to the tracker.
		/// </summary>
		public event Action<ICollection<TrackedProfitLoss>> ItemsAdded;

		/// <summary>
		/// This is raised when an item we're tracking has been changed.
		/// </summary>
		public event Action<TrackedProfitLoss> ItemChanged;

		/// <summary>
		/// This is raised when we have stopped tracking an item. After this is raised the ManaTrackedItem is disposed.
		/// </summary>
		public event Action<TrackedProfitLoss> ItemRemoved;

		readonly List<TrackedProfitLoss> trackedItems = new List<TrackedProfitLoss>();
		readonly Dictionary<TrackedProfitLoss, DateTime> lastChangeRaised = new Dictionary<TrackedProfitLoss, DateTime>();

		readonly Timer timer = new Timer();

		public ProfitLossTracker()
		{
			try
			{
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
				CoreManager.Current.CharacterFilter.Logoff += new EventHandler<LogoffEventArgs>(CharacterFilter_Logoff);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				CoreManager.Current.WorldFilter.ReleaseObject += new EventHandler<Decal.Adapter.Wrappers.ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);

				var stopWatch = new System.Diagnostics.Stopwatch();

				if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
					stopWatch.Start();

				ProcessPeas();
				ProcessComps();
				ProcessSalvage();
				ProcessNetProfit();

				if (Settings.SettingsManager.Misc.VerboseDebuggingEnabled.Value)
					Debug.WriteToChat("Loaded Profit/Loss Tracker: " + stopWatch.Elapsed.TotalMilliseconds.ToString("N0") + "ms");

				timer.Tick += new EventHandler(timer_Tick);
				timer.Interval = 500;
				timer.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_Logoff(object sender, LogoffEventArgs e)
		{
			try
			{
				CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<Decal.Adapter.Wrappers.ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				CoreManager.Current.WorldFilter.ReleaseObject -= new EventHandler<Decal.Adapter.Wrappers.ReleaseObjectEventArgs>(WorldFilter_ReleaseObject);

				timer.Tick -= new EventHandler(timer_Tick);
				timer.Stop();
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
				ProcessObject(e.New);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, Decal.Adapter.Wrappers.ChangeObjectEventArgs e)
		{
			try
			{
				ProcessObject(e.Changed);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ReleaseObject(object sender, Decal.Adapter.Wrappers.ReleaseObjectEventArgs e)
		{
			try
			{
				ProcessObject(e.Released);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void ProcessObject(WorldObject obj)
		{
			if (obj == null)
				return;

			// Peas
			if (obj.ObjectClass == ObjectClass.SpellComponent && obj.Name.Contains("Pea"))
			{
				ProcessPeas();
				ProcessNetProfit();
			}

			// Comps
			if (obj.ObjectClass == ObjectClass.SpellComponent && !obj.Name.Contains("Pea"))
			{
				ProcessComps();
				ProcessNetProfit();
			}

			// Salvage
			if (obj.ObjectClass == ObjectClass.Salvage)
			{
				ProcessSalvage();
				ProcessNetProfit();
			}

			// Net Profit
			if (obj.ObjectClass == ObjectClass.TradeNote || obj.ObjectClass == ObjectClass.Money)
				ProcessNetProfit();
		}

		void ProcessPeas()
		{
			int value = 0;

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (wo.ObjectClass == ObjectClass.SpellComponent && wo.Name.Contains("Pea"))
					value += wo.Values(LongValueKey.Value);
			}

			ProcessItemToTrack("Peas", value);
		}

		void ProcessComps()
		{
			int value = 0;

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (wo.ObjectClass == ObjectClass.SpellComponent && !wo.Name.Contains("Pea"))
					value += wo.Values(LongValueKey.Value);
			}

			ProcessItemToTrack("Comps", value);
		}

		void ProcessSalvage()
		{
			int value = 0;

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (wo.ObjectClass == ObjectClass.Salvage)
					value += wo.Values(LongValueKey.Value);
			}

			ProcessItemToTrack("Salvage", value);
		}

		void ProcessNetProfit()
		{
			int value = 0;

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (wo.ObjectClass == ObjectClass.SpellComponent)
					value += wo.Values(LongValueKey.Value);

				if (wo.ObjectClass == ObjectClass.Salvage)
					value += wo.Values(LongValueKey.Value);

				if (wo.ObjectClass == ObjectClass.TradeNote)
					value += wo.Values(LongValueKey.Value);

				if (wo.ObjectClass == ObjectClass.Money)
					value += wo.Values(LongValueKey.Value);
			}

			ProcessItemToTrack("Net Profit", value);
		}

		void ProcessItemToTrack(string name, int value)
		{
			foreach (var trackedItem in trackedItems)
			{
				if (trackedItem.Name == name)
				{
					trackedItem.AddSnapShot(DateTime.Now, value, SnapShotGroup<int>.PruneMethod.DecreaseResolution);

					if (ItemChanged != null)
					{
						ItemChanged(trackedItem);

						if (lastChangeRaised.ContainsKey(trackedItem))
							lastChangeRaised[trackedItem] = DateTime.Now;
						else
							lastChangeRaised.Add(trackedItem, DateTime.Now);
					}

					return;
				}
			}

			var trackedProfitLoss = new TrackedProfitLoss(name);
			trackedProfitLoss.AddSnapShot(DateTime.Now, value, SnapShotGroup<int>.PruneMethod.DecreaseResolution);

			trackedItems.Add(trackedProfitLoss);

			if (ItemsAdded != null)
			{
				ItemsAdded(new List<TrackedProfitLoss> { trackedProfitLoss });

				if (lastChangeRaised.ContainsKey(trackedProfitLoss))
					lastChangeRaised[trackedProfitLoss] = DateTime.Now;
				else
					lastChangeRaised.Add(trackedProfitLoss, DateTime.Now);
			}
		}

		void timer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (ItemChanged == null)
					return;

				foreach (var trackedItem in trackedItems)
				{
					if (lastChangeRaised.ContainsKey(trackedItem) && DateTime.Now - lastChangeRaised[trackedItem] < TimeSpan.FromSeconds(1))
						return;

					ItemChanged(trackedItem);

					if (lastChangeRaised.ContainsKey(trackedItem))
						lastChangeRaised[trackedItem] = DateTime.Now;
					else
						lastChangeRaised.Add(trackedItem, DateTime.Now);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
