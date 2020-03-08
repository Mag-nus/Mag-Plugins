using System;
using System.IO;
using System.Text;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace Mag_VendorLogger
{
	[FriendlyName("Mag-VendorLogger")]
	public class Class1 : PluginBase
	{
		private DirectoryInfo pluginPersonalFolder;

		protected override void Startup()
		{
			pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-VendorLogger");

			if (!pluginPersonalFolder.Exists)
				pluginPersonalFolder.Create();

			Core.WorldFilter.ApproachVendor += WorldFilter_ApproachVendor;
		}

		protected override void Shutdown()
		{
			Core.WorldFilter.ApproachVendor -= WorldFilter_ApproachVendor;
		}

		private string logFileName;

		private void WorldFilter_ApproachVendor(object sender, ApproachVendorEventArgs e)
		{
			try
			{
				logFileName = pluginPersonalFolder.FullName + @"\" + CoreManager.Current.Actions.Landcell.ToString("X8") + " " + CoreManager.Current.WorldFilter[e.MerchantId].Name + ".csv";

				FileInfo fileInfo = new FileInfo(logFileName);

				if (fileInfo.Exists)
					fileInfo.Delete();


				using (StreamWriter writer = new StreamWriter(fileInfo.FullName, true))
				{
					// "Timestamp,ContainerName,ContainerID,Landcell,Location,JSON"
					writer.WriteLine("\"BuyRate\",\"Categories\",\"Count\",\"MaxValue\",\"MerchantId\",,\"Quantity\",\"SellRate\"");

					writer.WriteLine("\"" + e.Vendor.BuyRate + "\",\"" + e.Vendor.Categories + "\",\"" + e.Vendor.Count + "\",\"" + e.Vendor.MaxValue + "\",\"" + e.Vendor.MerchantId + "\",\"" + e.Vendor.Quantity + "\",\"" + e.Vendor.SellRate + "\"");

					writer.Close();
				}


				foreach (WorldObject wo in e.Vendor)
					LogItem(wo);

				CoreManager.Current.Actions.AddChatTextRaw("<{Mag-VendorLogger}>: " + CoreManager.Current.WorldFilter[e.MerchantId].Name + " log completed.", 5);
			}
			catch { }
		}

		private void LogItem(WorldObject item)
		{
			FileInfo logFile = new FileInfo(logFileName);

			if (!logFile.Exists)
			{
				CoreManager.Current.Actions.AddChatTextRaw("<{Mag-VendorLogger}>: Log file appears deleted...", 5);
				return;
			}


			using (StreamWriter writer = new StreamWriter(logFileName, true))
			{
				bool needsComma = false;

				StringBuilder output = new StringBuilder();

				output.Append("{");

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

				output.Append("}");

				writer.WriteLine(output);

				writer.Close();
			}
		}
	}
}
