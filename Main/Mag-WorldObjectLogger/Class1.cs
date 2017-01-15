using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace Mag_WorldObjectLogger
{
    public class Class1 : PluginBase
	{
		private DirectoryInfo pluginPersonalFolder;

		protected override void Startup()
		{
			pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-WorldObjectLogger");

			if (!pluginPersonalFolder.Exists)
				pluginPersonalFolder.Create();

			Core.WorldFilter.CreateObject += WorldFilter_CreateObject;
			Core.WorldFilter.ChangeObject += WorldFilter_ChangeObject;
		}

		protected override void Shutdown()
		{
			Core.WorldFilter.CreateObject -= WorldFilter_CreateObject;
			Core.WorldFilter.ChangeObject -= WorldFilter_ChangeObject;
		}

		private void WorldFilter_CreateObject(object sender, CreateObjectEventArgs e)
		{
			try
			{
				// Spells and projectiles are ObjectClass.Unknown
				if (e.New.ObjectClass == ObjectClass.Player || e.New.ObjectClass == ObjectClass.Corpse || e.New.ObjectClass == ObjectClass.Unknown || e.New.Container != 0)
					return;

				CoreManager.Current.Actions.RequestId(e.New.Id);
			}
			catch { }
		}

		private void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				// Spells and projectiles are ObjectClass.Unknown
				if (e.Changed.ObjectClass == ObjectClass.Player || e.Changed.ObjectClass == ObjectClass.Corpse || e.Changed.ObjectClass == ObjectClass.Unknown || e.Changed.Container != 0)
					return;

				if (e.Change == WorldChangeType.IdentReceived)
					LogItem(e.Changed);
			}
			catch { }
		}

		private readonly Dictionary<int, string> itemsLogged = new Dictionary<int, string>();

		private void LogItem(WorldObject item)
		{
			if (itemsLogged.ContainsKey(item.Id) && itemsLogged[item.Id] == item.Name)
				return;

			itemsLogged[item.Id] = item.Name;


			string logFileName = pluginPersonalFolder.FullName + @"\" + CoreManager.Current.Actions.Landcell.ToString("X8") +".csv";

			FileInfo logFile = new FileInfo(logFileName);

			if (!logFile.Exists)
			{
				using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
				{
					writer.WriteLine("\"Timestamp\",\"LandCell\",\"RawCoordinates\",\"JSON\"");

					writer.Close();
				}
			}


			using (StreamWriter writer = new StreamWriter(logFileName, true))
			{
				bool needsComma = false;
				
				StringBuilder output = new StringBuilder();

				// "Timestamp,Landcell,RawCoordinates,JSON"
				output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
				output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");
				output.Append('"' + item.RawCoordinates().X + " " + item.RawCoordinates().Y + " " + item.RawCoordinates().Z + '"' + ",");

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
				for (int i = 0 ; i < item.ActiveSpellCount ; i++)
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
