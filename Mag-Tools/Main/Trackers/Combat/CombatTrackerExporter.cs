using System.Collections.Generic;
using System.IO;
using System.Xml;

using MagTools.Trackers.Combat.Standard;

namespace MagTools.Trackers.Combat
{
	class CombatTrackerExporter
	{
		readonly List<CombatInfo> combatInfos;
		readonly List<AetheriaInfo> aetheriaInfos;
		readonly List<CloakInfo> cloakInfos;

		public CombatTrackerExporter(List<CombatInfo> combatInfos, List<AetheriaInfo> aetheriaInfos, List<CloakInfo> cloakInfos)
		{
			this.combatInfos = combatInfos;
			this.aetheriaInfos = aetheriaInfos;
			this.cloakInfos = cloakInfos;
		}

		public void Export(string fileName)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			
			if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
				fileInfo.Directory.Create();

			XmlDocument xmlDocument = new XmlDocument();

			xmlDocument.LoadXml("<CombatTracker></CombatTracker>");

			XmlNode parentNode = xmlDocument.SelectSingleNode("CombatTracker");

			if (parentNode == null)
				return;


			// Export the Combat Info
			if (combatInfos.Count > 0)
			{
				parentNode.InnerXml += "<CombatInfos></CombatInfos>";

				XmlNode combatInfosNode = xmlDocument.SelectSingleNode("CombatTracker/CombatInfos");

				foreach (CombatInfo info in combatInfos)
				{
					XmlNode combatInfoNode = combatInfosNode.AppendChild(xmlDocument.CreateElement("CombatInfo"));

					XmlAttribute attribute = xmlDocument.CreateAttribute("SourceName");
					attribute.Value = info.SourceName;
					if (combatInfoNode.Attributes != null)
						combatInfoNode.Attributes.Append(attribute);

					attribute = xmlDocument.CreateAttribute("TargetName");
					attribute.Value = info.TargetName;
					if (combatInfoNode.Attributes != null)
						combatInfoNode.Attributes.Append(attribute);

					if (info.KillingBlows != 0)
					{
						attribute = xmlDocument.CreateAttribute("KillingBlows");
						attribute.Value = info.KillingBlows.ToString();
						if (combatInfoNode.Attributes != null)
							combatInfoNode.Attributes.Append(attribute);
					}

					foreach (KeyValuePair<AttackType, CombatInfo.DamageByAttackType> attackTypePair in info.DamageByAttackTypes)
					{
						XmlNode attackTypeNode = combatInfoNode.AppendChild(xmlDocument.CreateElement(attackTypePair.Key.ToString()));

						foreach (KeyValuePair<DamageElement, CombatInfo.DamageByAttackType.DamageByElement> elementPair in attackTypePair.Value.DamageByElements)
						{
							XmlNode elementNode = attackTypeNode.AppendChild(xmlDocument.CreateElement(elementPair.Key.ToString()));

							attribute = xmlDocument.CreateAttribute("TotalAttacks");
							attribute.Value = elementPair.Value.TotalAttacks.ToString();
							if (elementNode.Attributes != null)
								elementNode.Attributes.Append(attribute);

							if (elementPair.Value.FailedAttacks != 0)
							{
								attribute = xmlDocument.CreateAttribute("FailedAttacks");
								attribute.Value = elementPair.Value.FailedAttacks.ToString();
								if (elementNode.Attributes != null)
									elementNode.Attributes.Append(attribute);
							}

							if (elementPair.Value.Crits != 0)
							{
								attribute = xmlDocument.CreateAttribute("Crits");
								attribute.Value = elementPair.Value.Crits.ToString();
								if (elementNode.Attributes != null)
									elementNode.Attributes.Append(attribute);
							}

							if (elementPair.Value.TotalNormalDamage != 0)
							{
								attribute = xmlDocument.CreateAttribute("TotalNormalDamage");
								attribute.Value = elementPair.Value.TotalNormalDamage.ToString();
								if (elementNode.Attributes != null)
									elementNode.Attributes.Append(attribute);
							}

							if (elementPair.Value.MaxNormalDamage != 0)
							{
								attribute = xmlDocument.CreateAttribute("MaxNormalDamage");
								attribute.Value = elementPair.Value.MaxNormalDamage.ToString();
								if (elementNode.Attributes != null)
									elementNode.Attributes.Append(attribute);
							}

							if (elementPair.Value.TotalCritDamage != 0)
							{
								attribute = xmlDocument.CreateAttribute("TotalCritDamage");
								attribute.Value = elementPair.Value.TotalCritDamage.ToString();
								if (elementNode.Attributes != null)
									elementNode.Attributes.Append(attribute);
							}

							if (elementPair.Value.MaxCritDamage != 0)
							{
								attribute = xmlDocument.CreateAttribute("MaxCritDamage");
								attribute.Value = elementPair.Value.MaxCritDamage.ToString();
								if (elementNode.Attributes != null)
									elementNode.Attributes.Append(attribute);
							}
						}
					}
				}
			}


			// Export the Aetheria Info
			if (aetheriaInfos.Count > 0)
			{
				parentNode.InnerXml += "<AetheriaInfos></AetheriaInfos>";

				XmlNode aetheriaInfosNode = xmlDocument.SelectSingleNode("CombatTracker/AetheriaInfos");

				foreach (AetheriaInfo info in aetheriaInfos)
				{
					XmlNode aetheriaInfoNode = aetheriaInfosNode.AppendChild(xmlDocument.CreateElement("AetheriaInfo"));

					XmlAttribute attribute = xmlDocument.CreateAttribute("SourceName");
					attribute.Value = info.SourceName;
					if (aetheriaInfoNode.Attributes != null)
						aetheriaInfoNode.Attributes.Append(attribute);

					attribute = xmlDocument.CreateAttribute("TargetName");
					attribute.Value = info.TargetName;
					if (aetheriaInfoNode.Attributes != null)
						aetheriaInfoNode.Attributes.Append(attribute);

					if (info.TotalSurges != 0)
					{
						attribute = xmlDocument.CreateAttribute("TotalSurges");
						attribute.Value = info.TotalSurges.ToString();
						if (aetheriaInfoNode.Attributes != null)
							aetheriaInfoNode.Attributes.Append(attribute);
					}
				}
			}


			// Export the Cloak Info
			if (cloakInfos.Count > 0)
			{
				parentNode.InnerXml += "<CloakInfos></CloakInfos>";

				XmlNode cloakInfosNode = xmlDocument.SelectSingleNode("CombatTracker/CloakInfos");

				foreach (CloakInfo info in cloakInfos)
				{
					XmlNode cloakInfoNode = cloakInfosNode.AppendChild(xmlDocument.CreateElement("CloakInfo"));

					XmlAttribute attribute = xmlDocument.CreateAttribute("SourceName");
					attribute.Value = info.SourceName;
					if (cloakInfoNode.Attributes != null)
						cloakInfoNode.Attributes.Append(attribute);

					attribute = xmlDocument.CreateAttribute("TargetName");
					attribute.Value = info.TargetName;
					if (cloakInfoNode.Attributes != null)
						cloakInfoNode.Attributes.Append(attribute);

					if (info.TotalSurges != 0)
					{
						attribute = xmlDocument.CreateAttribute("TotalSurges");
						attribute.Value = info.TotalSurges.ToString();
						if (cloakInfoNode.Attributes != null)
							cloakInfoNode.Attributes.Append(attribute);
					}
				}
			}


			xmlDocument.Save(fileName);
		}
	}
}
