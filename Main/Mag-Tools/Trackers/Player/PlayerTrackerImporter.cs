using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MagTools.Trackers.Player
{
	class PlayerTrackerImporter
	{
		readonly string fileName;

		public PlayerTrackerImporter(string fileName)
		{
			this.fileName = fileName;
		}

		public bool Import(List<TrackedPlayer> trackedPlayers)
		{
			FileInfo fileInfo = new FileInfo(fileName);

			if (!fileInfo.Exists)
				return false;

			XmlDocument xmlDocument = new XmlDocument();

			xmlDocument.Load(fileName);

			XmlNode playersNode = xmlDocument.SelectSingleNode("Players");

			if (playersNode == null)
				return false;

			trackedPlayers.Clear();

			// Import the Players
			if (playersNode.HasChildNodes)
			{
				foreach (XmlNode aetheriaInfoNode in playersNode.ChildNodes)
				{
					if (aetheriaInfoNode.Attributes == null || aetheriaInfoNode.Attributes.Count == 0)
						continue;

					XmlAttribute name = aetheriaInfoNode.Attributes["Name"];

					TrackedPlayer item = new TrackedPlayer(name.Value);

					XmlAttribute attribute;

					if ((attribute = aetheriaInfoNode.Attributes["LastSeen"]) != null)
					{
						long value;
						if (long.TryParse(attribute.Value, out value))
							item.LastSeen = new DateTime(value);
					}

					if ((attribute = aetheriaInfoNode.Attributes["LandBlock"]) != null)
					{
						int value;
						if (int.TryParse(attribute.Value, out value))
							item.LandBlock = value;
					}

					if ((attribute = aetheriaInfoNode.Attributes["LocationX"]) != null)
					{
						double value;
						if (double.TryParse(attribute.Value, out value))
							item.LocationX = value;
					}

					if ((attribute = aetheriaInfoNode.Attributes["LocationY"]) != null)
					{
						double value;
						if (double.TryParse(attribute.Value, out value))
							item.LocationY = value;
					}

					if ((attribute = aetheriaInfoNode.Attributes["LocationZ"]) != null)
					{
						double value;
						if (double.TryParse(attribute.Value, out value))
							item.LocationZ = value;
					}

					trackedPlayers.Add(item);
				}
			}

			return true;
		}
	}
}
