using System;
using System.Globalization;
using MagTools.Trackers.Equipment;

using Mag.Shared;

using VirindiViewService.Controls;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Views
{
	class ManaTrackerGUI : IDisposable
	{
		private readonly EquipmentTracker manaTracker;
		private readonly MainView mainView;

		const int IconUnknown	= 0x60020B5;	// Cicle (Supposed to represent a question mark, a backwards one I guess...)
		const int IconActive	= 0x60011F9;	// Green Circle
		const int IconNotActive	= 0x60011F8;	// Red Circle
		const int IconNone		= 0x600287A;	// Small Grayish Dot

		public ManaTrackerGUI(EquipmentTracker manaTracker, MainView mainView)
		{
			try
			{
				this.manaTracker = manaTracker;
				this.mainView = mainView;

				manaTracker.ItemAdded += new Action<IEquipmentTrackedItem>(manaTracker_ItemAdded);
				manaTracker.ItemRemoved += new Action<IEquipmentTrackedItem>(manaTracker_ItemRemoved);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private bool disposed;

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
			if (!disposed)
			{
				if (disposing)
				{
					manaTracker.ItemAdded -= new Action<IEquipmentTrackedItem>(manaTracker_ItemAdded);
					manaTracker.ItemRemoved -= new Action<IEquipmentTrackedItem>(manaTracker_ItemRemoved);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void manaTracker_ItemAdded(IEquipmentTrackedItem obj)
		{
			try
			{
				WorldObject wo = CoreManager.Current.WorldFilter[obj.Id];

				if (wo == null)
					return;

				if (wo.Name != null && wo.Name.Contains("Aetheria") ||			// // We don't display aetheria
					wo.Values(LongValueKey.EquipableSlots) == 134217728 ||		// // We don't display cloaks (EquipableSlots: 134217728)
					wo.Values(LongValueKey.EquippedSlots) == 8388608)			// // We don't display archer/missile ammo (arrows)
				{
				}
				else
				{
					HudList.HudListRowAccessor newRow = mainView.ManaList.AddRow();

					((HudPictureBox)newRow[0]).Image = wo.Icon + 0x6000000;
					((HudStaticText)newRow[1]).Text = wo.Name;
					((HudStaticText)newRow[5]).Text = obj.Id.ToString(CultureInfo.InvariantCulture);
				}

				Item_Changed(obj);

				obj.Changed += new Action<IEquipmentTrackedItem>(Item_Changed);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void manaTracker_ItemRemoved(IEquipmentTrackedItem obj)
		{
			try
			{
				obj.Changed -= new Action<IEquipmentTrackedItem>(Item_Changed);

				for (int row = 1 ; row <= mainView.ManaList.RowCount ; row++)
				{
					if (int.Parse(((HudStaticText)mainView.ManaList[row - 1][5]).Text) == obj.Id)
					{
						mainView.ManaList.RemoveRow(row - 1);

						row--;
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Item_Changed(IEquipmentTrackedItem obj)
		{
			try
			{
				for (int row = 1 ; row <= mainView.ManaList.RowCount ; row++)
				{
					if (int.Parse(((HudStaticText)mainView.ManaList[row - 1][5]).Text) == obj.Id)
					{
						if (obj.ItemState == EquipmentTrackedItemState.Active)
							((HudPictureBox)mainView.ManaList[row - 1][2]).Image = IconActive;
						else if (obj.ItemState == EquipmentTrackedItemState.NotActive)
							((HudPictureBox)mainView.ManaList[row - 1][2]).Image = IconNotActive;
						else if (obj.ItemState == EquipmentTrackedItemState.Unknown)
							((HudPictureBox)mainView.ManaList[row - 1][2]).Image = IconUnknown;
						else
							((HudPictureBox)mainView.ManaList[row - 1][2]).Image = IconNone;

						if (obj.ItemState != EquipmentTrackedItemState.Active && obj.ItemState != EquipmentTrackedItemState.NotActive)
						{
							((HudStaticText)mainView.ManaList[row - 1][3]).Text = "-";
							((HudStaticText)mainView.ManaList[row - 1][4]).Text = "-";
							((HudStaticText)mainView.ManaList[row - 1][6]).Text = int.MaxValue.ToString(CultureInfo.InvariantCulture);
						}
						else
						{
							((HudStaticText)mainView.ManaList[row - 1][3]).Text = obj.CalculatedCurrentMana + " / " + obj.MaximumMana;
							((HudStaticText)mainView.ManaList[row - 1][4]).Text = string.Format("{0:d}h{1:d2}m", (int)obj.ManaTimeRemaining.TotalHours, obj.ManaTimeRemaining.Minutes);
							((HudStaticText)mainView.ManaList[row - 1][6]).Text = obj.ManaTimeRemaining.TotalSeconds.ToString(CultureInfo.InvariantCulture);
						}

						SortList();
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private void SortList()
		{
			mainView.ManaTotal.Text = "Mana needed: " + manaTracker.ManaNeededToRefillItems;

			mainView.UnretainedTotal.Text = "Unretained Items: " + manaTracker.NumberOfUnretainedItems;

			if (mainView.ManaList.RowCount == 0)
				return;

			for (int row = 0 ; row < mainView.ManaList.RowCount - 1 ; row++)
			{
				for (int compareRow = row + 1 ; compareRow < mainView.ManaList.RowCount ; compareRow++)
				{
					if (double.Parse(((HudStaticText)mainView.ManaList[row][6]).Text) > double.Parse(((HudStaticText)mainView.ManaList[compareRow][6]).Text))
					{
						
						VirindiViewService.ACImage obj0 = ((HudPictureBox)mainView.ManaList[row][0]).Image;
						((HudPictureBox)mainView.ManaList[row][0]).Image = ((HudPictureBox)mainView.ManaList[compareRow][0]).Image;
						((HudPictureBox)mainView.ManaList[compareRow][0]).Image = obj0;

						string obj1 = ((HudStaticText)mainView.ManaList[row][1]).Text;
						((HudStaticText)mainView.ManaList[row][1]).Text = ((HudStaticText)mainView.ManaList[compareRow][1]).Text;
						((HudStaticText)mainView.ManaList[compareRow][1]).Text = obj1;

						VirindiViewService.ACImage obj2 = ((HudPictureBox)mainView.ManaList[row][2]).Image;
						((HudPictureBox)mainView.ManaList[row][2]).Image = ((HudPictureBox)mainView.ManaList[compareRow][2]).Image;
						((HudPictureBox)mainView.ManaList[compareRow][2]).Image = obj2;

						string obj3 = ((HudStaticText)mainView.ManaList[row][3]).Text;
						((HudStaticText)mainView.ManaList[row][3]).Text = ((HudStaticText)mainView.ManaList[compareRow][3]).Text;
						((HudStaticText)mainView.ManaList[compareRow][3]).Text = obj3;

						string obj4 = ((HudStaticText)mainView.ManaList[row][4]).Text;
						((HudStaticText)mainView.ManaList[row][4]).Text = ((HudStaticText)mainView.ManaList[compareRow][4]).Text;
						((HudStaticText)mainView.ManaList[compareRow][4]).Text = obj4;

						string obj5 = ((HudStaticText)mainView.ManaList[row][5]).Text;
						((HudStaticText)mainView.ManaList[row][5]).Text = ((HudStaticText)mainView.ManaList[compareRow][5]).Text;
						((HudStaticText)mainView.ManaList[compareRow][5]).Text = obj5;

						string obj6 = ((HudStaticText)mainView.ManaList[row][6]).Text;
						((HudStaticText)mainView.ManaList[row][6]).Text = ((HudStaticText)mainView.ManaList[compareRow][6]).Text;
						((HudStaticText)mainView.ManaList[compareRow][6]).Text = obj6;
					}
				}
			}
		}
	}
}
