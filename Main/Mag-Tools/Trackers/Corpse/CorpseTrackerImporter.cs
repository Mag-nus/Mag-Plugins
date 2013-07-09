using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MagTools.Trackers.Corpse
{
	static class CorpseTrackerImporter
	{
		public static bool Import(string fileName, out List<TrackedCorpse> items)
		{
			items = new List<TrackedCorpse>();


			FileInfo fileInfo = new FileInfo(fileName);

			if (!fileInfo.Exists)
				return false;


			XmlDocument xmlDocument = new XmlDocument();

			xmlDocument.Load(fileName);

			XmlNode corpsesNode = xmlDocument.SelectSingleNode("Corpses");

			if (corpsesNode == null)
				return false;


			// Import the Items
			if (corpsesNode.HasChildNodes)
			{
				foreach (XmlNode node in corpsesNode.ChildNodes)
				{
					if (node.Attributes == null || node.Attributes.Count == 0)
						continue;

					XmlAttribute attribute;

					TrackedCorpse item;

					if ((attribute = node.Attributes["Id"]) != null)
					{
						int value;
						if (int.TryParse(attribute.Value, out value))
							item = new TrackedCorpse(value);
						else
							continue;
					}
					else
						continue;

					if ((attribute = node.Attributes["TimeStamp"]) != null)
					{
						long value;
						if (long.TryParse(attribute.Value, out value))
							item.TimeStamp = new DateTime(value);
					}

					if ((attribute = node.Attributes["LandBlock"]) != null)
					{
						int value;
						if (int.TryParse(attribute.Value, out value))
							item.LandBlock = value;
					}

					if ((attribute = node.Attributes["LocationX"]) != null)
					{
						double value;
						if (double.TryParse(attribute.Value, out value))
							item.LocationX = value;
					}

					if ((attribute = node.Attributes["LocationY"]) != null)
					{
						double value;
						if (double.TryParse(attribute.Value, out value))
							item.LocationY = value;
					}

					if ((attribute = node.Attributes["LocationZ"]) != null)
					{
						double value;
						if (double.TryParse(attribute.Value, out value))
							item.LocationZ = value;
					}

					if ((attribute = node.Attributes["Description"]) != null)
						item.Description = attribute.Value;

					if ((attribute = node.Attributes["Opened"]) != null)
					{
						bool value;
						if (bool.TryParse(attribute.Value, out value))
							item.Opened = value;
					}

					items.Add(item);
				}
			}

			return true;
		}
	}
}
