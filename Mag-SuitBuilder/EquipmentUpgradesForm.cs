using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Mag_SuitBuilder.Equipment;

using Mag.Shared.Constants;

namespace Mag_SuitBuilder
{
	partial class EquipmentUpgradesForm : Form
	{
		public EquipmentUpgradesForm()
		{
			InitializeComponent();
		}

		public void Update(Dictionary<SuitBuildableMyWorldObject, List<SuitBuildableMyWorldObject>> upgrades)
		{
			EquipmentGroup obsoleteEquipment = new EquipmentGroup();
			EquipmentGroup upgradeEquipment = new EquipmentGroup();

			foreach (var kvp in upgrades)
			{
				obsoleteEquipment.Add(kvp.Key);

				foreach (var v in kvp.Value)
					upgradeEquipment.Add(v);
			}

			Update(obsoleteEquipment, upgradeEquipment);
		}

		public void Update(EquipmentGroup obsoleteEquipment, EquipmentGroup upgradeEquipment)
		{
			currentEquipmentGrid.DataSource = obsoleteEquipment;
			currentEquipmentGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

			upgradeEquipmentGrid.DataSource = upgradeEquipment;
			upgradeEquipmentGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
		}

		private void currentEquipmentGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			// This just hides numeric fields that aren't supported, they return -1
			if ((e.Value is int && (int)e.Value == -1) ||
				(e.Value is double && Math.Abs((double)e.Value + 1) < Double.Epsilon) ||
				(e.Value is EquippableSlotFlags && (EquippableSlotFlags)e.Value == EquippableSlotFlags.None) ||
				(e.Value is CoverageFlags && (CoverageFlags)e.Value == CoverageFlags.None))
			{
				e.PaintBackground(e.ClipBounds, true);
				e.Handled = true;
			}
		}

		private void upgradeEquipmentGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			// This just hides numeric fields that aren't supported, they return -1
			if ((e.Value is int && (int)e.Value == -1) ||
				(e.Value is double && Math.Abs((double)e.Value + 1) < Double.Epsilon) ||
				(e.Value is EquippableSlotFlags && (EquippableSlotFlags)e.Value == EquippableSlotFlags.None) ||
				(e.Value is CoverageFlags && (CoverageFlags)e.Value == CoverageFlags.None))
			{
				e.PaintBackground(e.ClipBounds, true);
				e.Handled = true;
			}
		}
	}
}
