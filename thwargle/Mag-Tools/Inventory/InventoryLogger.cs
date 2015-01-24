using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Inventory
{
	class InventoryLogger : IDisposable
	{
		private string inventoryFileName
		{
			get { return PluginCore.PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".Inventory.xml"; }
		}


		public InventoryLogger()
		{
			try
			{
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				CoreManager.Current.WorldFilter.MoveObject += new EventHandler<MoveObjectEventArgs>(WorldFilter_MoveObject);
				CoreManager.Current.CharacterFilter.Logoff += new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);
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
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
					CoreManager.Current.WorldFilter.MoveObject -= new EventHandler<MoveObjectEventArgs>(WorldFilter_MoveObject);
					CoreManager.Current.CharacterFilter.Logoff -= new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		bool loggedInAndWaitingForIdData;

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.InventoryManagement.InventoryLogger.Value)
					return;

				// Check to see if the inventory file doesn't exist. If it doesn't, lets id all our equipment and then dump again.
				if (!File.Exists(inventoryFileName))
				{
					Debug.WriteToChat("Requesting id information for all armor/weapon inventory. This will take a few minutes...");

					foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
					{
						if (!wo.HasIdData && ObjectClassNeedsIdent(wo.ObjectClass, wo.Name))
							CoreManager.Current.Actions.RequestId(wo.Id);
					}

					loggedInAndWaitingForIdData = true;
				}
				else
					DumpInventoryToFile(true);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			if (!Settings.SettingsManager.InventoryManagement.InventoryLogger.Value)
				return;

			if (loggedInAndWaitingForIdData)
			{
				bool allInventoryHasIdData = true;

				foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (!wo.HasIdData && ObjectClassNeedsIdent(wo.ObjectClass, wo.Name))
					{
						allInventoryHasIdData = false;
						break;
					}
				}

				if (allInventoryHasIdData)
				{
					loggedInAndWaitingForIdData = false;
					DumpInventoryToFile();
					Debug.WriteToChat("Requesting id information for all armor/weapon inventory completed. Log file written.");
				}
			}
		}

		readonly List<int> requestedIds = new List<int>();

		void WorldFilter_MoveObject(object sender, MoveObjectEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.InventoryManagement.InventoryLogger.Value)
					return;

				// Check if the player just received an item that it needs id data for
				if (!e.Moved.HasIdData && ObjectClassNeedsIdent(e.Moved.ObjectClass, e.Moved.Name) && !requestedIds.Contains(e.Moved.Id))
				{
					// Make sure its in our inventory
					foreach (var invo in CoreManager.Current.WorldFilter.GetInventory())
					{
						if (invo.Id == e.Moved.Id)
						{
							requestedIds.Add(e.Moved.Id);
							CoreManager.Current.Actions.RequestId(e.Moved.Id);
							break;
						}
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_Logoff(object sender, Decal.Adapter.Wrappers.LogoffEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.InventoryManagement.InventoryLogger.Value)
					return;

				DumpInventoryToFile();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void DumpInventoryToFile(bool requestIdsIfItemDoesntHave = false)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(List<MyWorldObject>));

			// Load the objects from our saved inventory.xml
			List<MyWorldObject> previouslySavedObjects = new List<MyWorldObject>();

			if (File.Exists(inventoryFileName))
			{
				try
				{
					using (XmlReader reader = XmlReader.Create(inventoryFileName))
						previouslySavedObjects = (List<MyWorldObject>) serializer.Deserialize(reader);
				}
				catch (InvalidOperationException) // File is corrupt
				{
					Debug.WriteToChat("Inventory file is corrupt.");
				}
			}

			// Go through our current inventory
			List<MyWorldObject> myWorldObjects = new List<MyWorldObject>();

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
			{
				// Check to see if we already have some information for this item
				foreach (MyWorldObject prevso in previouslySavedObjects)
				{
					if (prevso.Id == wo.Id && prevso.ObjectClass == (int)wo.ObjectClass)
					{
						// If neither our past nor our current item HadIdData, but it should, lets request it
						if (requestIdsIfItemDoesntHave && !prevso.HasIdData && !wo.HasIdData && ObjectClassNeedsIdent(wo.ObjectClass, wo.Name))
						{
							CoreManager.Current.Actions.RequestId(wo.Id);
							myWorldObjects.Add(MyWorldObjectCreator.Create(wo));
						}
						else
						{
							// Add the WorldObject to the MyWorldObject data so we have up to date information
							myWorldObjects.Add(MyWorldObjectCreator.Combine(prevso, wo));
						}

						goto end;
					}
				}

				if (requestIdsIfItemDoesntHave && !wo.HasIdData && ObjectClassNeedsIdent(wo.ObjectClass, wo.Name))
					CoreManager.Current.Actions.RequestId(wo.Id);

				myWorldObjects.Add(MyWorldObjectCreator.Create(wo));

				end: ;
			}

			// Write it out to the inventory.xml file
			FileInfo fileInfo = new FileInfo(inventoryFileName);

			if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
				fileInfo.Directory.Create();

			XmlDocument xmlDoc = new XmlDocument();
			XPathNavigator nav = xmlDoc.CreateNavigator();
			using (XmlWriter writer = nav.AppendChild())
				serializer.Serialize(writer, myWorldObjects);

			xmlDoc.Save(inventoryFileName);
		}

		private bool ObjectClassNeedsIdent(ObjectClass objectClass, string name)
		{
			if (objectClass == ObjectClass.Armor || objectClass == ObjectClass.Clothing ||
				objectClass == ObjectClass.MeleeWeapon || objectClass == ObjectClass.MissileWeapon || objectClass == ObjectClass.WandStaffOrb ||
				objectClass == ObjectClass.Jewelry ||
				(objectClass == ObjectClass.Gem && !String.IsNullOrEmpty(name) && name.Contains("Aetheria")) || // Aetheria are Gems
				(objectClass == ObjectClass.Misc && !String.IsNullOrEmpty(name) && name.Contains("Essence"))) // Essences (Summoning Gems) are Misc
				return true;

			return false;
		}
	}
}
