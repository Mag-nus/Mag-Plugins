using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

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
						if (!wo.HasIdData && ObjectClassNeedsIdent(wo.ObjectClass))
							CoreManager.Current.Actions.RequestId(wo.Id);
					}

					loggedInAndWaitingForIdData = true;
				}
				else
					DumpInventoryToFile();
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
					if (!wo.HasIdData && ObjectClassNeedsIdent(wo.ObjectClass))
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

		void DumpInventoryToFile()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(List<MyWorldObject>));

			List<MyWorldObject> previouslySavedObjects = new List<MyWorldObject>();

			if (File.Exists(inventoryFileName))
			{
				using (XmlReader reader = XmlReader.Create(inventoryFileName))
					previouslySavedObjects = (List<MyWorldObject>)serializer.Deserialize(reader);
			}

			List<MyWorldObject> myWorldObjects = new List<MyWorldObject>();

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetInventory())
			{
				foreach (MyWorldObject prevso in previouslySavedObjects)
				{
					if (prevso.Id == wo.Id && prevso.Name == wo.Name)
					{
						prevso.UpdateFromObject(wo);
						myWorldObjects.Add(prevso);
						goto end;
					}
				}
				myWorldObjects.Add(new MyWorldObject(wo));
				end: ;
			}

			XmlDocument xmlDoc = new XmlDocument();
			XPathNavigator nav = xmlDoc.CreateNavigator();
			using (XmlWriter writer = nav.AppendChild())
				serializer.Serialize(writer, myWorldObjects);

			xmlDoc.Save(inventoryFileName);
		}

		private bool ObjectClassNeedsIdent(ObjectClass objectClass)
		{
			if ((objectClass == ObjectClass.Armor || objectClass == ObjectClass.Clothing ||
				objectClass == ObjectClass.MeleeWeapon || objectClass == ObjectClass.MissileWeapon || objectClass == ObjectClass.WandStaffOrb ||
				objectClass == ObjectClass.Jewelry))
				return true;

			return false;
		}
	}
}
