using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Text;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace Mag_LootLogger
{
	[FriendlyName("Mag-LootLogger")]
	public class Class1 : PluginBase
	{
		private DirectoryInfo pluginPersonalFolder;

		protected override void Startup()
		{
			pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-LootLogger");

			if (!pluginPersonalFolder.Exists)
				pluginPersonalFolder.Create();

			Core.ContainerOpened += new EventHandler<ContainerOpenedEventArgs>(Core_ContainerOpened);
		}

		protected override void Shutdown()
		{
			Core.ContainerOpened -= new EventHandler<ContainerOpenedEventArgs>(Core_ContainerOpened);
		}


		private WorldObject currentOpenContainer;
		private bool currentOpenContainerIsSingleUseChest;

		/// <summary>
		/// Item.Id, Container.Id
		/// </summary>
		readonly Dictionary<int, int> corpseItemsLogged = new Dictionary<int, int>();
		readonly List<int> chestItemsLogged = new List<int>();


		void Core_ContainerOpened(object sender, ContainerOpenedEventArgs e)
		{
			try
			{
				currentOpenContainer = CoreManager.Current.WorldFilter[e.ItemGuid];

				if (currentOpenContainer == null)
					return;

				// Do not log housing chests
				if (currentOpenContainer.Name == "Storage")
					return;

				if (currentOpenContainer.ObjectClass == ObjectClass.Corpse)
				{
					currentOpenContainerIsSingleUseChest = false;
					Start();
				}
				else if (currentOpenContainer.Name.Contains("Chest") || currentOpenContainer.Name.Contains("Vault") || currentOpenContainer.Name.Contains("Reliquary"))
				{
					currentOpenContainerIsSingleUseChest = true;
					chestItemsLogged.Clear();
					Start();
				}
			}
			catch { }
		}


		public bool IsRunning { get; private set; }

		readonly Collection<int> idsRequested = new Collection<int>();


		void Start()
		{
			if (IsRunning)
				return;

			idsRequested.Clear();

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
			catch { }
		}


		void Think()
		{
			if (CoreManager.Current.Actions.OpenedContainer == 0)
			{
				CoreManager.Current.Actions.AddChatText("<{Mag-LootLogger}>: Container closed before all items were logged.", 5, 1);

				Stop();
				return;
			}

			// Sometimes it takes a bit before we actually get the item information for the items inside the container
			if (CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.Actions.OpenedContainer).Count == 0)
				return;

			bool stillWaiting = false;

			foreach (WorldObject wo in CoreManager.Current.WorldFilter.GetByContainer(CoreManager.Current.Actions.OpenedContainer))
			{
				if (!wo.HasIdData)
				{
					if (!idsRequested.Contains(wo.Id))
					{
						idsRequested.Add(wo.Id);
						CoreManager.Current.Actions.RequestId(wo.Id);
					}

					stillWaiting = true;
				}
				else
					LogItem(wo);
			}

			if (!stillWaiting)
			{
				CoreManager.Current.Actions.AddChatText("<{Mag-LootLogger}>: All items logged.", 5, 1);
				Stop();
			}
		}


		private void LogItem(WorldObject item)
		{
			if (currentOpenContainerIsSingleUseChest)
			{
				if (chestItemsLogged.Contains(item.Id))
					return;

				chestItemsLogged.Add(item.Id);
			}
			else
			{
				if (corpseItemsLogged.ContainsKey(item.Id) && corpseItemsLogged[item.Id] == item.Container)
					return;

				corpseItemsLogged[item.Id] = item.Container;
			}


			string logFileName = pluginPersonalFolder.FullName + @"\" + CoreManager.Current.Actions.Landcell.ToString("X8") + ".csv";

			FileInfo logFile = new FileInfo(logFileName);

			if (!logFile.Exists)
			{
				using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
				{
					writer.WriteLine("\"Timestamp\",\"ContainerName\",\"ContainerID\",\"LandCell\",\"Location\",\"JSON\"");

					writer.Close();
				}
			}


			using (StreamWriter writer = new StreamWriter(logFileName, true))
			{
				bool needsComma = false;

				StringBuilder output = new StringBuilder();

				// "Timestamp,ContainerName,ContainerID,Landcell,Location,JSON"
				output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
				output.Append('"' + currentOpenContainer.Name + '"' + ",");
				output.Append('"' + currentOpenContainer.Id + '"' + ",");
				output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");
				output.Append('"' + currentOpenContainer.Coordinates().ToString() + '"' + ",");

				output.Append("\"{");

				output.Append("\"Id\":\"" + item.Id + "\",");
				output.Append("\"ObjectClass\":\"" + item.ObjectClass + "\",");

				output.Append("\"BoolValues\":{");
				foreach (var value in item.BoolKeys)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append("\"" + value + "\":\"" + item.Values((BoolValueKey)value) + "\"");
				}
				output.Append("},");

				output.Append("\"DoubleValues\":{");
				needsComma = false;
				foreach (var value in item.DoubleKeys)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append("\"" + value + "\":\"" + item.Values((DoubleValueKey)value) + "\"");
				}
				output.Append("},");

				output.Append("\"LongValues\":{");
				needsComma = false;
				foreach (var value in item.LongKeys)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append("\"" + value + "\":\"" + item.Values((LongValueKey)value) + "\"");
				}
				output.Append("},");

				output.Append("\"StringValues\":{");
				needsComma = false;
				foreach (var value in item.StringKeys)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append("\"" + value + "\":\"" + item.Values((StringValueKey)value) + "\"");
				}
				output.Append("},");

				output.Append("\"ActiveSpells\":\"");
				needsComma = false;
				for (int i = 0; i < item.ActiveSpellCount; i++)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append(item.ActiveSpell(i));
				}
				output.Append("\",");

				output.Append("\"Spells\":\"");
				needsComma = false;
				for (int i = 0; i < item.SpellCount; i++)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append(item.Spell(i));
				}
				output.Append("\"");

				output.Append("}\"");

				writer.WriteLine(output);

				writer.Close();
			}
		}
	}
}
