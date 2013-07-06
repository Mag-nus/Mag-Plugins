using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace MagTools.Trackers.Player
{
	class PlayerTrackerExporter
	{
		readonly List<TrackedPlayer> trackedPlayers;

		public PlayerTrackerExporter(List<TrackedPlayer> trackedPlayers)
		{
			this.trackedPlayers = trackedPlayers;
		}

		public void Export(string fileName)
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
			if (trackedPlayers.Count > 0)
			{
				foreach (TrackedPlayer item in trackedPlayers)
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
			}

			xmlDocument.Save(fileName);
		}
	}
}
