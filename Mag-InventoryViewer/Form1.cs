using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Mag_InventoryViewer
{
	public partial class Form1 : Form
	{
		readonly SortableBindingList<MyWorldObject> boundList = new SortableBindingList<MyWorldObject>();

		public Form1()
		{
			InitializeComponent();

			Text += " " + Application.ProductVersion;

			txtSearchDirectory.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-Tools\";

			typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
			dataGridView1.DataSource = boundList;
		}

		private void cmdLoadInventory_Click(object sender, System.EventArgs e)
		{
			this.Enabled = false;

			XmlSerializer serializer = new XmlSerializer(typeof(List<MyWorldObject>));

			treeView1.Nodes.Clear();
			boundList.Clear();

			string[] serverFolderPaths = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-Tools\");

			foreach (string serverFolderPath in serverFolderPaths)
			{
				string serverName = serverFolderPath.Substring(serverFolderPath.LastIndexOf(Path.DirectorySeparatorChar) + 1, serverFolderPath.Length - serverFolderPath.LastIndexOf(Path.DirectorySeparatorChar) - 1);

				TreeNode serverNode = treeView1.Nodes.Add(serverName);

				string[] characterFilePaths = Directory.GetFiles(serverFolderPath, "*.Inventory.xml", SearchOption.AllDirectories);

				foreach (string characterFilePath in characterFilePaths)
				{
					string characterName = characterFilePath.Substring(characterFilePath.LastIndexOf(Path.DirectorySeparatorChar) + 1, characterFilePath.Length - characterFilePath.LastIndexOf(Path.DirectorySeparatorChar) - 1);
					characterName = characterName.Substring(0, characterName.IndexOf("."));

					TreeNode characterNode = serverNode.Nodes.Add(characterName);

					List<MyWorldObject> myWorldObjects = new List<MyWorldObject>();

					using (XmlReader reader = XmlReader.Create(characterFilePath))
						myWorldObjects = (List<MyWorldObject>)serializer.Deserialize(reader);

					foreach (var mwo in myWorldObjects)
						mwo.Character = characterName;

					characterNode.Tag = myWorldObjects;
				}
			}

			treeView1.ExpandAll();

			if (treeView1.Nodes.Count > 0)
				treeView1.Nodes[0].Checked = true;

			dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

			this.Enabled = true;
		}

		int count = 0;
		private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
		{
			count++;
			boundList.RaiseListChangedEvents = false;

			foreach (TreeNode node in e.Node.Nodes)
				node.Checked = e.Node.Checked;

			if (e.Node.Tag is List<MyWorldObject>)
			{
				foreach (MyWorldObject piece in (e.Node.Tag as List<MyWorldObject>))
				{
					if (!e.Node.Checked && boundList.Contains(piece))
						boundList.Remove(piece);
					else if (e.Node.Checked && !boundList.Contains(piece))
						boundList.Add(piece);
				}
			}

			if ((--count) == 0)
			{
				boundList.RaiseListChangedEvents = true;
				boundList.ResetBindings();
			}
		}
	}
}
