using System;
using System.Globalization;
using System.Text.RegularExpressions;

using Mag.Shared;

using MagTools.Inventory;

using Decal.Adapter;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class InventoryToolsView
	{
		readonly HudTextBox inventorySearch;
		readonly HudList inventoryList;
		readonly HudStaticText inventoryItemText;

		public InventoryToolsView(MainView mainView, InventoryExporter inventoryExporter)
		{
			try
			{
				inventorySearch = mainView.InventorySearch;
				inventoryList = mainView.InventoryList;
				inventoryItemText = mainView.InventoryItemText;

				mainView.ClipboardWornEquipment.Hit += (s2, e2) => { try { inventoryExporter.ExportToClipboard(InventoryExporter.ExportGroups.WornEquipment); } catch (Exception ex) { Debug.LogException(ex); } };
				mainView.ClipboardInventoryInfo.Hit += (s2, e2) => { try { inventoryExporter.ExportToClipboard(InventoryExporter.ExportGroups.Inventory); } catch (Exception ex) { Debug.LogException(ex); } };

				inventorySearch.Change += new EventHandler(InventorySearch_Change);
				inventoryList.Click += new VirindiViewService.Controls.HudList.delClickedControl(InventoryList_Click);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void InventorySearch_Change(object sender, EventArgs e)
		{
			try
			{
				inventoryList.ClearRows();

				var regex = new Regex(inventorySearch.Text, RegexOptions.IgnoreCase);

				foreach (var wo in CoreManager.Current.WorldFilter.GetInventory())
				{
					var itemInfo = new ItemInfo.ItemInfo(wo);

					if (regex.IsMatch(itemInfo.ToString()))
					{
						HudList.HudListRowAccessor newRow = inventoryList.AddRow();

						((HudPictureBox)newRow[0]).Image = wo.Icon + 0x6000000;
						((HudStaticText)newRow[1]).Text = wo.Name;
						((HudStaticText)newRow[2]).Text = wo.Id.ToString(CultureInfo.InvariantCulture);
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void InventoryList_Click(object sender, int row, int col)
		{
			try
			{
				int id;

				if (int.TryParse(((HudStaticText)inventoryList[row][2]).Text, out id))
				{
					CoreManager.Current.Actions.SelectItem(id);

					var wo = CoreManager.Current.WorldFilter[id];

					if (wo != null)
					{
						var itemInfo = new ItemInfo.ItemInfo(wo);
						inventoryItemText.Text = itemInfo.ToString();
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
