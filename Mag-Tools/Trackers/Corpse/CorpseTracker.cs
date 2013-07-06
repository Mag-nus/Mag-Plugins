using System;
using System.Collections.Generic;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Trackers.Corpse
{
	class CorpseTracker : IDisposable
	{
		/// <summary>
		/// This is raised when an item has been added to the tracker.
		/// </summary>
		public event Action<TrackedCorpse> ItemAdded;

		/// <summary>
		/// This is raised when we have stopped tracking an item. After this is raised the ManaTrackedItem is disposed.
		/// </summary>
		public event Action<TrackedCorpse> ItemRemoved;

		readonly Dictionary<int, TrackedCorpse> trackedCorpses = new Dictionary<int, TrackedCorpse>();

		public CorpseTracker()
		{
			try
			{
				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
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
					CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
					CoreManager.Current.ContainerOpened -= new EventHandler<ContainerOpenedEventArgs>(Current_ContainerOpened);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void WorldFilter_CreateObject(object sender, Decal.Adapter.Wrappers.CreateObjectEventArgs e)
		{
			try
			{
				ProcessWorldObject(e.New, true);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				if (e.Change == WorldChangeType.IdentReceived)
					ProcessWorldObject(e.Changed);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void ProcessWorldObject(WorldObject wo, bool allowRequestId = false)
		{
			if (wo.ObjectClass != ObjectClass.Corpse)
				return;

			if (trackedCorpses.ContainsKey(wo.Id))
				return;

			bool trackCorpse = false;

			if (wo.Name.Contains(CoreManager.Current.CharacterFilter.Name))
				trackCorpse = true;

			// If track all corpses
			//

			// Track permitted corpses
			//

			// Track my killed corpses
			if (!trackCorpse && wo.Values(LongValueKey.Burden) > 6000)
			{
				if (!wo.HasIdData)
				{
					if (allowRequestId)
						CoreManager.Current.Actions.RequestId(wo.Id);
				}
				else if (wo.Values(StringValueKey.FullDescription).Contains(CoreManager.Current.CharacterFilter.Name))
					trackCorpse = true;
			}

			if (trackCorpse)
			{
				TrackedCorpse trackedCorpse = new TrackedCorpse(wo.Id, DateTime.Now, wo.Values(LongValueKey.Landblock), wo.RawCoordinates().X, wo.RawCoordinates().Y, wo.RawCoordinates().Z, wo.Name);

				trackedCorpses.Add(trackedCorpse.Id, trackedCorpse);

				if (ItemAdded != null)
					ItemAdded(trackedCorpse);
			}
		}

		void Current_ContainerOpened(object sender, ContainerOpenedEventArgs e)
		{
			try
			{
				if (trackedCorpses.ContainsKey(e.ItemGuid))
				{
					TrackedCorpse trackedCorpse = trackedCorpses[e.ItemGuid];

					trackedCorpses.Remove(e.ItemGuid);

					if (ItemRemoved != null)
						ItemRemoved(trackedCorpse);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
