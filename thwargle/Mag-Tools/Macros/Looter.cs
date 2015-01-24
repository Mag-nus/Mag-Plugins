using System;
using System.Collections.ObjectModel;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class Looter : IDisposable, ILooter
	{
		public Looter()
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
				WorldObject container = CoreManager.Current.WorldFilter[e.ItemGuid];

				if (container == null)
					return;

				// Do not loot housing chests
				if (container.Name == "Storage")
					return;

				if (container.ObjectClass == ObjectClass.Corpse)
				{
					if (Settings.SettingsManager.Looting.AutoLootCorpses.Value)
					{
						Start();
						return;
					}

					if (Settings.SettingsManager.Looting.AutoLootMyCorpses.Value && container.Name == "Corpse of " + CoreManager.Current.CharacterFilter.Name)
					{
						Start();
						return;
					}
				}

				// Only loot chests and vaults, etc...
				if (Settings.SettingsManager.Looting.AutoLootChests.Value && (container.Name.Contains("Chest") || container.Name.Contains("Vault") || container.Name.Contains("Reliquary")))
				{
					Start();
					return;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public bool IsRunning { get; private set; }

		bool idsRequested;

		readonly Collection<string> blackLitedItems = new Collection<string>();

		void Start()
		{
			if (IsRunning)
				return;

			idsRequested = false;

			blackLitedItems.Clear();

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

		int currentWorkingId;
		DateTime currentWorkingIdFirstAttempt;

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

			if (CoreManager.Current.Actions.BusyState != 0)
				return;

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.Actions.OpenedContainer))
			{
				if (!uTank2.PluginCore.PC.FLootPluginQueryNeedsID(wo.Id))
				{
					// If this is an item on our corpse, loot it
					if (Settings.SettingsManager.Looting.AutoLootMyCorpses.Value && CoreManager.Current.WorldFilter[CoreManager.Current.Actions.OpenedContainer].Name == "Corpse of " + CoreManager.Current.CharacterFilter.Name)
					{
					}
					else
					{
						uTank2.LootPlugins.LootAction result = uTank2.PluginCore.PC.FLootPluginClassifyImmediate(wo.Id);

						if (result.IsNoLoot)
							continue;

						if (result.IsSalvage)
						{
							if (!Settings.SettingsManager.Looting.LootSalvage.Value)
								continue;
						}
						else if (!result.IsKeep && !result.IsKeepUpTo)
								continue;

						if (blackLitedItems.Contains(wo.Name))
							continue;

						// Check the keep #
						if (result.IsKeepUpTo)
						{
							int totalInInventory = 0;

							foreach (WorldObject inventoryItem in CoreManager.Current.WorldFilter.GetInventory())
							{
								uTank2.LootPlugins.LootAction inventoryItemResult = uTank2.PluginCore.PC.FLootPluginClassifyImmediate(inventoryItem.Id);

								if (inventoryItemResult.IsKeepUpTo && result.RuleName == inventoryItemResult.RuleName && result.Data1 == inventoryItemResult.Data1 && wo.Name == inventoryItem.Name)
								{
									if (inventoryItem.Values(LongValueKey.StackMax, 0) > 0)
										totalInInventory += inventoryItem.Values(LongValueKey.StackCount, 1);
									else
										totalInInventory++;
								}
							}

							if (totalInInventory >= result.Data1)
								continue;
						}
					}

					if (currentWorkingId != wo.Id)
					{
						currentWorkingId = wo.Id;
						currentWorkingIdFirstAttempt = DateTime.Now;
					}
					else
					{
						if (DateTime.Now - currentWorkingIdFirstAttempt > TimeSpan.FromSeconds(10))
						{
							Debug.WriteToChat("Blacklisting item: " + wo.Id + ", " + wo.Name);
							blackLitedItems.Add(wo.Name);
							continue;
						}
					}

					CoreManager.Current.Actions.MoveItem(wo.Id, CoreManager.Current.CharacterFilter.Id);

					return;
				}

				waitingForIds = true;
			}

			if (!waitingForIds)
			{
				if (CoreManager.Current.WorldFilter[CoreManager.Current.Actions.OpenedContainer].ObjectClass != ObjectClass.Corpse)
					Debug.WriteToChat("No more lootable items found.");

				Stop();
			}
		}
	}
}
