using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MagTools.Settings
{
	class XmlFile : IDisposable
	{
		private string fileName;

		private XmlDocument xmlDoc = new XmlDocument();

		public XmlFile(string fileName, string rootNode)
		{
			this.fileName = fileName;

			FileInfo fileInfo = new FileInfo(fileName);

			if (fileInfo.Exists)
			{
				try
				{
					xmlDoc.Load(fileName);

					// Do we have a different root node than what we want?
					if (xmlDoc.DocumentElement.Name != rootNode)
					{
						xmlDoc.RemoveAll();

						xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", string.Empty));
						XmlElement root = xmlDoc.CreateElement(rootNode);
						xmlDoc.AppendChild(root);
					}
				}
				catch
				{
					// If we had an error, just delete the bitch.. its probably corrupted
					System.IO.File.Delete(fileName);

					xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", string.Empty));
					XmlElement root = xmlDoc.CreateElement(rootNode);
					xmlDoc.AppendChild(root);

					xmlDoc.Save(fileName);
				}
			}
			else
			{
				xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", string.Empty));
				XmlElement root = xmlDoc.CreateElement(rootNode);
				xmlDoc.AppendChild(root);

				xmlDoc.Save(fileName);
			}

			xmlDoc.NodeChanged += new XmlNodeChangedEventHandler(xmlDoc_Changed);
			xmlDoc.NodeInserted += new XmlNodeChangedEventHandler(xmlDoc_Changed);
			xmlDoc.NodeRemoved += new XmlNodeChangedEventHandler(xmlDoc_Changed);
		}

		private bool _disposed = false;

		public void Dispose()
		{
			Dispose(true);

			// Use SupressFinalize in case a subclass
			// of this type implements a finalizer.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// If you need thread safety, use a lock around these 
			// operations, as well as in your methods that use the resource.
			if (!_disposed)
			{
				if (disposing)
				{
					xmlDoc.NodeChanged -= new XmlNodeChangedEventHandler(xmlDoc_Changed);
					xmlDoc.NodeInserted -= new XmlNodeChangedEventHandler(xmlDoc_Changed);
					xmlDoc.NodeRemoved -= new XmlNodeChangedEventHandler(xmlDoc_Changed);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}

		}

		private void xmlDoc_Changed(object sender, XmlNodeChangedEventArgs e)
		{
			try
			{
				xmlDoc.Save(fileName);

			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public void ReloadXmlFile()
		{
			xmlDoc.NodeChanged -= new XmlNodeChangedEventHandler(xmlDoc_Changed);
			xmlDoc.NodeInserted -= new XmlNodeChangedEventHandler(xmlDoc_Changed);
			xmlDoc.NodeRemoved -= new XmlNodeChangedEventHandler(xmlDoc_Changed);

			xmlDoc.Load(fileName);

			xmlDoc.NodeChanged += new XmlNodeChangedEventHandler(xmlDoc_Changed);
			xmlDoc.NodeInserted += new XmlNodeChangedEventHandler(xmlDoc_Changed);
			xmlDoc.NodeRemoved += new XmlNodeChangedEventHandler(xmlDoc_Changed);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xpath">The path of the node not including the root node.</param>
		/// <returns></returns>
		protected bool NodeExists(string xpath)
		{
			if (String.IsNullOrEmpty(xpath))
				return false;

			return (xmlDoc.DocumentElement.SelectSingleNode("/" + xmlDoc.DocumentElement.Name + "/" + xpath) != null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="xpath">The path in relation to the parent.</param>
		/// <returns></returns>
		public XmlNode CreateNode(XmlNode parent, string xpath, bool forceCreate = false)
		{
			string[] partsOfXPath = xpath.Trim('/').Split('/');

			if (partsOfXPath.Length == 0)
				return parent;

			string nextNodeInXPath = partsOfXPath[0];

			if (string.IsNullOrEmpty(nextNodeInXPath))
				return parent;

			// get or create the node from the name
			XmlNode node = parent.SelectSingleNode(nextNodeInXPath);

			if (node == null || forceCreate)
				node = parent.AppendChild(xmlDoc.CreateElement(nextNodeInXPath));

			// rejoin the remainder of the array as an xpath expression and recurse
			string rest = "/";

			for (int i = 1 ; i < partsOfXPath.Length ; i++)
			{
				rest += partsOfXPath[i] + "/";
			}

			return CreateNode(node, rest);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xpath">The path of the node not including the root node.</param>
		/// <returns></returns>
		public XmlNode CreateNode(string xpath)
		{
			if (NodeExists(xpath))
				return GetNode(xpath);

			return CreateNode(xmlDoc.DocumentElement, xpath);
		}

		/// <summary> 
		/// 
		/// </summary>
		/// <param name="xpath">The path of the node not including the root node.</param>
		/// <returns></returns>
		public XmlNode GetNode(string xpath)
		{
			return xmlDoc.DocumentElement.SelectSingleNode("/" + xmlDoc.DocumentElement.Name + "/" + xpath);
		}

		/// <summary> 
		/// 
		/// </summary>
		/// <param name="xpath">The path of the node not including the root node.</param>
		/// <returns></returns>
		public void DeleteNode(string xpath)
		{
			XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/" + xmlDoc.DocumentElement.Name + "/" + xpath);

			if (node != null)
				xmlDoc.RemoveChild(node);
		}

		public void AddAttribute(XmlNode node, string attributeName)
		{
			XmlAttribute attribute = xmlDoc.CreateAttribute(attributeName);

			node.Attributes.SetNamedItem(attribute);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xpath">The path of the node not including the root node.</param>
		/// <returns></returns>
		public bool GetBoolean(string xpath)
		{
			xpath = xpath.Replace(" ", "");

			XmlNode node = GetNode(xpath);

			if (node == null)
				return false;

			return Convert.ToBoolean(node.InnerText);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xpath">The path of the node not including the root node.</param>
		/// <param name="value"></param>
		public void SetBoolean(string xpath, bool value)
		{
			if (GetBoolean(xpath) == value)
				return;

			ReloadXmlFile();

			xpath = xpath.Replace(" ", "");

			XmlNode node = CreateNode(xpath);

			node.InnerText = Convert.ToString(value);
		}

		public IEnumerable<string> GetCollection(string xpath)
		{
			xpath = xpath.Replace(" ", "");

			XmlNode node = GetNode(xpath);

			Collection<string> collection = new Collection<string>();

			if (node == null)
				return collection;

			foreach (XmlNode childNode in node.ChildNodes)
			{
				collection.Add(childNode.InnerText);
			}

			return collection;
		}
	}
}
