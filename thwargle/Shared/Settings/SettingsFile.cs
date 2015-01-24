using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Xml;

namespace Mag.Shared.Settings
{
	static class SettingsFile
	{
		internal static readonly XmlDocument XmlDocument = new XmlDocument();

		static string _documentPath;

		static string _rootNodeName = "Settings";

		static SettingsFile()
		{
			ReloadXmlDocument();
		}

		public static void Init(string filePath, string rootNode = "Settings")
		{
			_documentPath = filePath;

			_rootNodeName = rootNode;

			ReloadXmlDocument();
		}

		public static void ReloadXmlDocument()
		{
			try
			{
				if (!String.IsNullOrEmpty(_documentPath) && File.Exists(_documentPath))
					XmlDocument.Load(_documentPath);
				else
					XmlDocument.LoadXml("<" + _rootNodeName + "></" + _rootNodeName + ">");
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);

				XmlDocument.LoadXml("<" + _rootNodeName + "></" + _rootNodeName + ">");
			}
		}

		public static void SaveXmlDocument()
		{
			XmlDocument.Save(_documentPath);
		}

		public static T GetSetting<T>(string xPath, T defaultValue = default(T))
		{
			XmlNode xmlNode = XmlDocument.SelectSingleNode(_rootNodeName + "/" + xPath);

			if (xmlNode != null)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

				if (converter.CanConvertFrom(typeof(string)))
					return (T)converter.ConvertFromString(xmlNode.InnerText);
			}

			return defaultValue;
		}

		public static void PutSetting<T>(string xPath, T value)
		{
			// Before we save a setting, we reload the document to make sure we don't overwrite settings saved from another session.
			ReloadXmlDocument();

			XmlNode xmlNode = XmlDocument.SelectSingleNode(_rootNodeName + "/" + xPath);

			if (xmlNode == null)
				xmlNode = createMissingNode(_rootNodeName + "/" + xPath);

			TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

			if (converter.CanConvertTo(typeof(string)))
			{
				string result = converter.ConvertToString(value);

				if (result != null)
				{
					xmlNode.InnerText = result;

					XmlDocument.Save(_documentPath);
				}
			}
		}

		static XmlNode createMissingNode(string xPath)
		{
			string[] xPathSections = xPath.Split('/');

			string currentXPath = "";

			XmlNode currentNode = XmlDocument.SelectSingleNode(_rootNodeName);

			foreach (string xPathSection in xPathSections)
			{
				currentXPath += xPathSection;

				XmlNode testNode = XmlDocument.SelectSingleNode(currentXPath);

				if (testNode == null)
				{
					if (currentNode != null)
						currentNode.InnerXml += "<" + xPathSection + "></" + xPathSection + ">";
				}

				currentNode = XmlDocument.SelectSingleNode(currentXPath);

				currentXPath += "/";
			}

			return currentNode;
		}

		public static IList<string> GetChilderenInnerTexts(string xPath)
		{
			XmlNode xmlNode = XmlDocument.SelectSingleNode(_rootNodeName + "/" + xPath);

			Collection<string> collection = new Collection<string>();

			if (xmlNode != null)
			{
				foreach (XmlNode childNode in xmlNode.ChildNodes)
					collection.Add(childNode.InnerText);
			}

			return collection;
		}

		public static void SetNodeChilderen(string xPath, string childNodeName, IList<string> innerTexts)
		{
			// Before we save a setting, we reload the document to make sure we don't overwrite settings saved from another session.
			ReloadXmlDocument();

			XmlNode parentNode = XmlDocument.SelectSingleNode(_rootNodeName + "/" + xPath);

			if (parentNode == null)
			{
				if (innerTexts.Count == 0)
					return;

				parentNode = createMissingNode(_rootNodeName + "/" + xPath);
			}

			parentNode.RemoveAll();

			if (innerTexts.Count == 0)
			{
				XmlDocument.Save(_documentPath);
				return;
			}

			foreach (string innerText in innerTexts)
			{
				XmlNode childNode = parentNode.AppendChild(XmlDocument.CreateElement(childNodeName));

				childNode.InnerText = innerText;
			}

			XmlDocument.Save(_documentPath);
		}

		public static XmlNode GetNode(string xPath, bool createIfNull = false)
		{
			var node = XmlDocument.SelectSingleNode(_rootNodeName + "/" + xPath);

			if (node == null && createIfNull)
				node = createMissingNode(_rootNodeName + "/" + xPath);

			return node;
		}

		public static void SetNodeChilderen(string xPath, string childNodeName, Collection<Dictionary<string, string>> childNodeAttributes)
		{
			// Before we save a setting, we reload the document to make sure we don't overwrite settings saved from another session.
			ReloadXmlDocument();

			XmlNode parentNode = XmlDocument.SelectSingleNode(_rootNodeName + "/" + xPath);

			if (parentNode == null)
				parentNode = createMissingNode(_rootNodeName + "/" + xPath);

			if (parentNode.HasChildNodes)
				parentNode.RemoveAll();

			foreach (Dictionary<string, string> dictionary in childNodeAttributes)
			{
				XmlNode childNode = parentNode.AppendChild(XmlDocument.CreateElement(childNodeName));

				foreach (KeyValuePair<string, string> pair in dictionary)
				{
					XmlAttribute attribute = XmlDocument.CreateAttribute(pair.Key);
					attribute.Value = pair.Value;

					if (childNode.Attributes != null) 
						childNode.Attributes.Append(attribute);
				}
			}

			XmlDocument.Save(_documentPath);
		}
	}
}
