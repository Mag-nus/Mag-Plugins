using System;
using System.Collections.Generic;
using System.Text;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Inventory
{
	class InventoryExporter
	{
		[Flags]
		public enum ExportGroups
		{
			WornEquipment	= 0x1,
			Inventory		= 0x2,

			All				= 0x7FFFFFFF
		}

		public void ExportToClipboard(ExportGroups groups)
		{
			Start(groups);
		}

		bool isRunning;
		ExportGroups exportGroups;
		bool idsRequested;

		void Start(ExportGroups groups)
		{
			if (isRunning)
				return;

			isRunning = true;
			exportGroups = groups;
			idsRequested = false;

			CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Copying all inventory item info to clipboard...", 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);

			CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);
		}

		void Stop()
		{
			if (!isRunning)
				return;

			isRunning = false;

			CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);

			CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "All inventory item info has been copied to the clipboard.", 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);
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
			List<WorldObject> worldObjects = new List<WorldObject>();

			if ((exportGroups & ExportGroups.Inventory) != 0)
			{
				foreach (WorldObject worldObject in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (ItemIsEquippedByMe(worldObject))
						continue;

					if (!worldObjects.Contains(worldObject))
						worldObjects.Add(worldObject);
				}
			}

			if ((exportGroups & ExportGroups.WornEquipment) != 0)
			{
				foreach (WorldObject worldObject in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (!ItemIsEquippedByMe(worldObject))
						continue;

					if (!worldObjects.Contains(worldObject))
						worldObjects.Add(worldObject);
				}
			}

			bool waitingForIdData = false;

			foreach (WorldObject obj in worldObjects)
			{
				if (!obj.HasIdData)
				{
					if (ObjectClassNeedsIdent(obj.ObjectClass))
					{
						if (!idsRequested)
							CoreManager.Current.Actions.RequestId(obj.Id);
						else
							waitingForIdData = true;
					}

				}
			}

			if (!idsRequested)
			{
				idsRequested = true;
				return;
			}

			if (!waitingForIdData)
			{
				ExportObjects(worldObjects);
				Stop();
			}
		}

		private bool ObjectClassNeedsIdent(ObjectClass objectClass)
		{
			if ((objectClass == ObjectClass.Armor || objectClass == ObjectClass.Clothing ||
				objectClass == ObjectClass.MeleeWeapon || objectClass == ObjectClass.MissileWeapon || objectClass == ObjectClass.WandStaffOrb ||
				objectClass == ObjectClass.Jewelry))
				return true;

			return false;
		}

		bool ItemIsEquippedByMe(WorldObject obj)
		{
			if (obj.Values(LongValueKey.EquippedSlots) <= 0)
				return false;

			// Weapons are in the -1 slot
			if (obj.Values(LongValueKey.Slot, -1) == -1)
				return (obj.Container == CoreManager.Current.CharacterFilter.Id);

			return true;
		}

		void ExportObjects(List<WorldObject> worldObjects)
		{
			worldObjects.Sort(new WorldObjectSorter());

			StringBuilder output = new StringBuilder();

			foreach (WorldObject obj in worldObjects)
			{
				ItemInfo.ItemInfo info = new ItemInfo.ItemInfo(obj);

				output.AppendLine(info.ToString());
			}

			System.Windows.Forms.Clipboard.SetText(output.ToString());
		}

		static bool ObjectIsInInventory(WorldObject worldObject)
		{
			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (worldObject == obj)
					return true;
			}

			return false;
		}

		private class WorldObjectSorter : IComparer<WorldObject>
		{
			public int Compare(WorldObject x, WorldObject y)
			{
				if (x == null)
				{
					if (y == null) // If x is null and y is null, they're equal. 
						return 0;

					// If x is null and y is not null, y is greater. 
					return -1;
				}

				// If x is not null...
				if (y == null) // ...and y is null, x is greater.
					return 1;

				// ...and y is not null, compare the the objects

				if (x.Container == CoreManager.Current.CharacterFilter.Id && x.Container != y.Container)
					return -1;
				
				if (y.Container == CoreManager.Current.CharacterFilter.Id && x.Container != y.Container)
					return 1;

				// Both items are in the main characters inventory
				if (x.Container == CoreManager.Current.CharacterFilter.Id && x.Container == y.Container)
				{
					if (x.Values(LongValueKey.EquippedSlots) > 0 && y.Values(LongValueKey.EquippedSlots) <= 0)
						return -1;

					if (x.Values(LongValueKey.EquippedSlots) <= 0 && y.Values(LongValueKey.EquippedSlots) > 0)
						return 1;

					// Both are equippped
					if (x.Values(LongValueKey.EquippedSlots) > 0 && y.Values(LongValueKey.EquippedSlots) > 0)
						return x.Values(LongValueKey.EquippedSlots).CompareTo(y.Values(LongValueKey.EquippedSlots));

					// Neither are equipped
					if ((x.ObjectClass != ObjectClass.Container && x.ObjectClass != ObjectClass.Foci) && (y.ObjectClass == ObjectClass.Container || y.ObjectClass == ObjectClass.Foci))
						return -1;

					if ((x.ObjectClass == ObjectClass.Container || x.ObjectClass == ObjectClass.Foci) && (y.ObjectClass != ObjectClass.Container && y.ObjectClass != ObjectClass.Foci))
						return 1;

					return x.Values(LongValueKey.Slot).CompareTo(y.Values(LongValueKey.Slot));
				}

				// Both items are not in the main characters inventory

				if (ObjectIsInInventory(x) && !ObjectIsInInventory(y))
					return -1;

				if (!ObjectIsInInventory(x) && ObjectIsInInventory(y))
					return 1;

				// Both are in side packed inventory
				if (ObjectIsInInventory(x) && ObjectIsInInventory(y))
				{
					if (x.Values(LongValueKey.Container) != y.Values(LongValueKey.Container))
						return x.Values(LongValueKey.Container).CompareTo(y.Values(LongValueKey.Container));

					return x.Values(LongValueKey.Slot).CompareTo(y.Values(LongValueKey.Slot));
				}

				// Neither are in inventory
				return x.Id.CompareTo(y.Id);
			}
		}
	}
}
