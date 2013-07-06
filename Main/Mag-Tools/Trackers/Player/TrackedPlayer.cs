using System;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Trackers.Player
{
	class TrackedPlayer : IDisposable
	{
		/// <summary>
		/// This is raised when an item the tracker is watching has been changed.
		/// </summary>
		public event Action<TrackedPlayer> Changed;

		public readonly string Name;

		public DateTime LastSeen;

		public int LandBlock;

		public double LocationX;
		public double LocationY;
		public double LocationZ;

		public int Id;

		public PlayerType PlayerType;

		public TrackedPlayer(string name)
		{
			Name = name;

			CoreManager.Current.WorldFilter.CreateObject += new EventHandler<Decal.Adapter.Wrappers.CreateObjectEventArgs>(WorldFilter_CreateObject);
			CoreManager.Current.WorldFilter.MoveObject += new EventHandler<Decal.Adapter.Wrappers.MoveObjectEventArgs>(WorldFilter_MoveObject);
		}

		public TrackedPlayer(string name, DateTime lastSeen, int landBlock, double locationX, double locaitonY, double locationZ, int id, PlayerType playerType = PlayerType.Unknown) : this(name)
		{
			LastSeen = lastSeen;

			LandBlock = landBlock;

			LocationX = locationX;
			LocationY = locaitonY;
			LocationZ = locationZ;

			Id = id;

			PlayerType = playerType;
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
					CoreManager.Current.WorldFilter.MoveObject -= new EventHandler<Decal.Adapter.Wrappers.MoveObjectEventArgs>(WorldFilter_MoveObject);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void WorldFilter_CreateObject(object sender, Decal.Adapter.Wrappers.CreateObjectEventArgs e)
		{
			try
			{
				ProcessObject(e.New);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_MoveObject(object sender, Decal.Adapter.Wrappers.MoveObjectEventArgs e)
		{
			try
			{
				ProcessObject(e.Moved);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private void ProcessObject(WorldObject obj)
		{
			if (obj.ObjectClass == ObjectClass.Player && obj.Name == Name)
			{
				LastSeen = DateTime.Now;

				LandBlock = obj.Values(LongValueKey.Landblock);

				LocationX = obj.RawCoordinates().X;
				LocationY = obj.RawCoordinates().Y;
				LocationZ = obj.RawCoordinates().Z;

				Id = obj.Id;

				if (Changed != null)
					Changed(this);
			}
		}
	}
}
