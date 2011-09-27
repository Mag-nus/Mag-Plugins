using System;
using System.Collections.ObjectModel;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools
{
	class PrintItemInfoOnContainerOpen : IDisposable
	{
		public bool Enabled { private get; set; }

		public PrintItemInfoOnContainerOpen()
		{
			try
			{
				CoreManager.Current.ContainerOpened += new EventHandler<ContainerOpenedEventArgs>(Current_ContainerOpened);
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
					Stop();

					CoreManager.Current.ContainerOpened -= new EventHandler<ContainerOpenedEventArgs>(Current_ContainerOpened);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		void Current_ContainerOpened(object sender, ContainerOpenedEventArgs e)
		{
			try
			{
				if (!Enabled)
					return;

				Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		bool started = false;
		
		DateTime startTime;

		Collection<int> itemsProcessed = new Collection<int>();

		void Start()
		{
			if (started)
				return;

			startTime = DateTime.Now;

			CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);

			started = true;
		}

		void Stop()
		{
			if (!started)
				return;

			CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);

			started = false;
		}

		DateTime lastThought = DateTime.MinValue;

		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (DateTime.Now - lastThought < TimeSpan.FromMilliseconds(100))
					return;

				lastThought = DateTime.Now;

				Think();

			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Think()
		{
			if (CoreManager.Current.Actions.OpenedContainer == 0)
			{
				Stop();
				return;
			}

			// Sometimes it takes a bit before we actually get the item information for the items inside the container
			if (CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.Actions.OpenedContainer).Count == 0)
				return;

			// Do not show salvage rules for chests and vaults
			bool hideSalvageRules = false;

			WorldObject container = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.OpenedContainer];

			if (container != null && (container.Name.Contains("Chest") || container.Name.Contains("Vault")))
				hideSalvageRules = true;

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.Actions.OpenedContainer))
			{
				if (itemsProcessed.Contains(wo.Id))
					continue;

				itemsProcessed.Add(wo.Id);

				PrintItemInfo printItemInfo = new PrintItemInfo(wo, true, hideSalvageRules);
			}

			// Some chests take a few seconds to print out ALL item info
			if (DateTime.Now - startTime < TimeSpan.FromSeconds(5))
				return;

			Stop();
		}
	}
}
