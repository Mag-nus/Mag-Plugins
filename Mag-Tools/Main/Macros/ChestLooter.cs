using System;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class ChestLooter : IDisposable, IChestLooter
	{
		public ChestLooter()
		{
			try
			{
				CoreManager.Current.ContainerOpened += new EventHandler<ContainerOpenedEventArgs>(Current_ContainerOpened);
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
					Stop();

					CoreManager.Current.ContainerOpened -= new EventHandler<ContainerOpenedEventArgs>(Current_ContainerOpened);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void Current_ContainerOpened(object sender, ContainerOpenedEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.Looting.Enabled.Value)
					return;

				WorldObject container = CoreManager.Current.WorldFilter[e.ItemGuid];

				if (container == null)
					return;

				// Do not loot monster corpses
				if (container.ObjectClass == ObjectClass.Corpse)
					return;

				// Do not loot housing chests
				if (container.Name == "Storage")
					return;

				// Only loot chests and vaults, etc...
				if (!container.Name.Contains("Chest") && !container.Name.Contains("Vault") && !container.Name.Contains("Reliquary"))
					return;

				Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public bool IsRunning { get; private set; }

		bool idsRequested;

		void Start()
		{
			if (IsRunning)
				return;

			idsRequested = false;

			CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);

			IsRunning = true;
		}

		void Stop()
		{
			if (!IsRunning)
				return;

			CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);

			IsRunning = false;
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

			bool waitingForIds = false;

			// First we request id's for items that need them
			if (!idsRequested)
			{
				foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.Actions.OpenedContainer))
				{
					if (uTank2.PluginCore.PC.FLootPluginQueryNeedsID(wo.Id))
					{
						CoreManager.Current.Actions.RequestId(wo.Id);

						waitingForIds = true;
					}
				}

				idsRequested = true;
			}

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.Actions.OpenedContainer))
			{
				if (!uTank2.PluginCore.PC.FLootPluginQueryNeedsID(wo.Id))
				{
					uTank2.LootPlugins.LootAction result = uTank2.PluginCore.PC.FLootPluginClassifyImmediate(wo.Id);

					if (result.IsNoLoot || (!result.IsKeep && !result.IsKeepUpTo))
						continue;

					CoreManager.Current.Actions.MoveItem(wo.Id, CoreManager.Current.CharacterFilter.Id);

					return;
				}

				waitingForIds = true;
			}

			if (!waitingForIds)
			{
				Debug.WriteToChat("No more lootable items found.");

				Stop();
			}
		}
	}
}
