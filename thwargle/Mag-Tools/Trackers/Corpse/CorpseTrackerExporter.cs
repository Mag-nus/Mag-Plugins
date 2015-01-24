using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace MagTools.Trackers.Corpse
{
	static class CorpseTrackerExporter
	{
		public static void Export(string fileName, IEnumerable<TrackedCorpse> items)
		{
			FileInfo fileInfo = new FileInfo(fileName);

			if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
				fileInfo.Directory.Create();


			XmlDocument xmlDocument = new XmlDocument();

			xmlDocument.LoadXml("<Corpses></Corpses>");

			XmlNode playersNode = xmlDocument.SelectSingleNode("Corpses");

			if (playersNode == null)
				return;


			// Export the Items
			foreach (TrackedCorpse item in items)
			{
				XmlNode playerNode = playersNode.AppendChild(xmlDocument.CreateElement("Corpse"));

				XmlAttribute attribute = xmlDocument.CreateAttribute("Id");
				attribute.Value = item.Id.ToString(CultureInfo.InvariantCulture);
				if (playerNode.Attributes != null)
					playerNode.Attributes.Append(attribute);

				attribute = xmlDocument.CreateAttribute("TimeStamp");
				attribute.Value = item.TimeStamp.Ticks.ToString(CultureInfo.InvariantCulture);
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

				attribute = xmlDocument.CreateAttribute("Description");
				attribute.Value = item.Description;
				if (playerNode.Attributes != null)
					playerNode.Attributes.Append(attribute);

				attribute = xmlDocument.CreateAttribute("Opened");
				attribute.Value = item.Opened.ToString();
				if (playerNode.Attributes != null)
					playerNode.Attributes.Append(attribute);
			}

			xmlDocument.Save(fileName);
		}
	}
}
