using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using MagTools.Trackers.Combat.Standard;

namespace MagTools.Trackers.Combat
{
	class CombatTrackerImporter
	{
		readonly string fileName;

		public CombatTrackerImporter(string fileName)
		{
			this.fileName = fileName;
		}

		public bool Import(List<CombatInfo> combatInfos, List<AetheriaInfo> aetheriaInfos, List<CloakInfo> cloakInfos)
		{
			FileInfo fileInfo = new FileInfo(fileName);

			if (!fileInfo.Exists)
				return false;

			XmlDocument xmlDocument = new XmlDocument();

			// Sometimes the following is thrown if Import() is called from Login(). Moving it to LoginComplete() seems to fix the issue.
			/*
			============================================================================
			6/24/2012 11:13:23 AM
			System.IO.IOException: The operation completed successfully.
			
			at System.IO.__Error.WinIOError(Int32 errorCode, String maybeFullPath)
			   at System.IO.FileStream.ReadCore(Byte[] buffer, Int32 offset, Int32 count)
			   at System.IO.FileStream.Read(Byte[] array, Int32 offset, Int32 count)
			   at System.Xml.XmlTextReaderImpl.InitStreamInput(Uri baseUri, String baseUriStr, Stream stream, Byte[] bytes, Int32 byteCount, Encoding encoding)
			   at System.Xml.XmlTextReaderImpl.InitStreamInput(Uri baseUri, String baseUriStr, Stream stream, Encoding encoding)
			   at System.Xml.XmlTextReaderImpl.OpenUrl()
			   at System.Xml.XmlTextReaderImpl.Read()
			   at System.Xml.XmlLoader.Load(XmlDocument doc, XmlReader reader, Boolean preserveWhitespace)
			   at System.Xml.XmlDocument.Load(XmlReader reader)
			   at System.Xml.XmlDocument.Load(String filename)
			   at MagTools.Trackers.Combat.CombatTrackerImporter.Import(List`1 combatInfos, List`1 aetheriaInfos, List`1 cloakInfos)
			   at MagTools.Trackers.Combat.CombatTracker.ImportStats(String xmlFileName)
			   at MagTools.PluginCore.CharacterFilter_Login(Object sender, LoginEventArgs e)
			============================================================================
			*/
			xmlDocument.Load(fileName);

			XmlNode parentNode = xmlDocument.SelectSingleNode("CombatTracker");

			if (parentNode == null)
				return false;


			combatInfos.Clear();

			aetheriaInfos.Clear();

			cloakInfos.Clear();


			// Import the CombatInfos
			XmlNode combatInfosNode = xmlDocument.SelectSingleNode("CombatTracker/CombatInfos");

			if (combatInfosNode != null && combatInfosNode.HasChildNodes)
			{
				foreach (XmlNode combatInfoNode in combatInfosNode.ChildNodes)
				{
					if (combatInfoNode.Attributes == null || combatInfoNode.Attributes.Count == 0)
						continue;

					XmlAttribute sourceName = combatInfoNode.Attributes["SourceName"];
					XmlAttribute targetName = combatInfoNode.Attributes["TargetName"];

					CombatInfo info = new CombatInfo(sourceName.Value, targetName.Value);

					XmlAttribute attribute = combatInfoNode.Attributes["KillingBlows"];

					if (attribute != null)
						int.TryParse(attribute.Value, out info.KillingBlows);

					if (combatInfoNode.HasChildNodes)
					{
						foreach (XmlNode attackTypeNode in combatInfoNode.ChildNodes)
						{
							CombatInfo.DamageByAttackType byAttackType = new CombatInfo.DamageByAttackType();

							if (attackTypeNode.HasChildNodes)
							{
								foreach (XmlNode elementNode in attackTypeNode.ChildNodes)
								{
									if (elementNode.Attributes != null && elementNode.Attributes.Count > 0)
									{
										CombatInfo.DamageByAttackType.DamageByElement byElement = new CombatInfo.DamageByAttackType.DamageByElement();

										attribute = elementNode.Attributes["TotalAttacks"];

										if (attribute != null)
											int.TryParse(attribute.Value, out byElement.TotalAttacks);

										attribute = elementNode.Attributes["FailedAttacks"];

										if (attribute != null)
											int.TryParse(attribute.Value, out byElement.FailedAttacks);

										attribute = elementNode.Attributes["Crits"];

										if (attribute != null)
											int.TryParse(attribute.Value, out byElement.Crits);

										attribute = elementNode.Attributes["TotalNormalDamage"];

										if (attribute != null)
											int.TryParse(attribute.Value, out byElement.TotalNormalDamage);

										attribute = elementNode.Attributes["MaxNormalDamage"];

										if (attribute != null)
											int.TryParse(attribute.Value, out byElement.MaxNormalDamage);

										attribute = elementNode.Attributes["TotalCritDamage"];

										if (attribute != null)
											int.TryParse(attribute.Value, out byElement.TotalCritDamage);

										attribute = elementNode.Attributes["MaxCritDamage"];

										if (attribute != null)
											int.TryParse(attribute.Value, out byElement.MaxCritDamage);

										byAttackType.DamageByElements.Add((DamageElement)Enum.Parse(typeof(DamageElement), elementNode.Name), byElement);
									}
								}
							}

							info.DamageByAttackTypes.Add((AttackType)Enum.Parse(typeof(AttackType), attackTypeNode.Name), byAttackType);
						}
					}

					combatInfos.Add(info);
				}
			}


			// Import the AetheriaInfos
			XmlNode aetheriaInfosNode = xmlDocument.SelectSingleNode("CombatTracker/AetheriaInfos");

			if (aetheriaInfosNode != null && aetheriaInfosNode.HasChildNodes)
			{
				foreach (XmlNode aetheriaInfoNode in aetheriaInfosNode.ChildNodes)
				{
					if (aetheriaInfoNode.Attributes == null || aetheriaInfoNode.Attributes.Count == 0)
						continue;

					XmlAttribute sourceName = aetheriaInfoNode.Attributes["SourceName"];
					XmlAttribute targetName = aetheriaInfoNode.Attributes["TargetName"];

					AetheriaInfo info = new AetheriaInfo(sourceName.Value, targetName.Value);

					XmlAttribute attribute = aetheriaInfoNode.Attributes["TotalSurges"];

					if (attribute != null)
						int.TryParse(attribute.Value, out info.TotalSurges);

					aetheriaInfos.Add(info);
				}
			}


			// Import the CloakInfos
			XmlNode cloakInfosNode = xmlDocument.SelectSingleNode("CombatTracker/CloakInfos");

			if (cloakInfosNode != null && cloakInfosNode.HasChildNodes)
			{
				foreach (XmlNode cloakInfoNode in cloakInfosNode.ChildNodes)
				{
					if (cloakInfoNode.Attributes == null || cloakInfoNode.Attributes.Count == 0)
						continue;

					XmlAttribute sourceName = cloakInfoNode.Attributes["SourceName"];
					XmlAttribute targetName = cloakInfoNode.Attributes["TargetName"];

					CloakInfo info = new CloakInfo(sourceName.Value, targetName.Value);

					XmlAttribute attribute = cloakInfoNode.Attributes["TotalSurges"];

					if (attribute != null)
						int.TryParse(attribute.Value, out info.TotalSurges);

					cloakInfos.Add(info);
				}
			}


			return true;
		}
	}
}
