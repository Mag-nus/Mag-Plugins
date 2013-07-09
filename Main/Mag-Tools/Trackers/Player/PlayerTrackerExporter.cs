using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace MagTools.Trackers.Player
{
	static class PlayerTrackerExporter
	{
		public static void Export(string fileName, IEnumerable<TrackedPlayer> items)
		{
			FileInfo fileInfo = new FileInfo(fileName);

			if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
				fileInfo.Directory.Create();


			XmlDocument xmlDocument = new XmlDocument();

			xmlDocument.LoadXml("<Players></Players>");

			XmlNode playersNode = xmlDocument.SelectSingleNode("Players");

			if (playersNode == null)
				return;


			// Export the Players
			foreach (TrackedPlayer item in items)
			{
				XmlNode playerNode = playersNode.AppendChild(xmlDocument.CreateElement("Player"));

				XmlAttribute attribute = xmlDocument.CreateAttribute("Name");
				attribute.Value = item.Name;
				if (playerNode.Attributes != null)
					playerNode.Attributes.Append(attribute);

				attribute = xmlDocument.CreateAttribute("LastSeen");
				attribute.Value = item.LastSeen.Ticks.ToString(CultureInfo.InvariantCulture);
				if (playerNode.Attributes != null)
					playerNode.Attributes.Append(attribute);

				attribute = xmlDocument.CreateAttribute("LandBlock");
				attribute.Value = item.LandBlock.ToString(CultureInfo.InvariantCulture);
				if (playerNode.Attributes != null)
					playerNode.Attributes.Append(attribute);

				attribute = xmlDocument.CreateAttribute("LocationX");
				attribute.Value = item.LocationX.ToString(CultureInfo.InvariantCulture);
				if (playerNode.Attributes != null)
					playerNode.Attributes.Append(attribute);

				attribute = xmlDocument.CreateAttribute("LocationY");
				attribute.Value = item.LocationY.ToString(CultureInfo.InvariantCulture);
				if (playerNode.Attributes != null)
					playerNode.Attributes.Append(attribute);

				attribute = xmlDocument.CreateAttribute("LocationZ");
				attribute.Value = item.LocationZ.ToString(CultureInfo.InvariantCulture);
				if (playerNode.Attributes != null)
					playerNode.Attributes.Append(attribute);
			}

			xmlDocument.Save(fileName);
		}
	}
}
