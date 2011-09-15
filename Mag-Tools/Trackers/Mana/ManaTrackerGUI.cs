using System;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Trackers.Mana
{
	class ManaTrackerGUI : IDisposable
	{
		private ManaTracker manaTracker;
		private Views.MainView mainView;

		const int IconUnknown	= 0x60020B5;	// Cicle (Supposed to represent a question mark, a backwards one I guess...)
		const int IconActive	= 0x60011F9;	// Green Circle
		const int IconNotActive	= 0x60011F8;	// Red Circle
		const int IconNone		= 0x600287A;	// Small Grayish Dot

		public ManaTrackerGUI(ManaTracker manaTracker, Views.MainView mainView)
		{
			try
			{
				this.manaTracker = manaTracker;
				this.mainView = mainView;

				manaTracker.ItemAdded += new Action<ManaTrackedItem>(manaTracker_ItemAdded);
				manaTracker.ItemRemoved += new Action<ManaTrackedItem>(manaTracker_ItemRemoved);
			}
			catch (Exception ex) { Debug.LogException(ex); }
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
					manaTracker.ItemAdded -= new Action<ManaTrackedItem>(manaTracker_ItemAdded);
					manaTracker.ItemRemoved -= new Action<ManaTrackedItem>(manaTracker_ItemRemoved);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		void manaTracker_ItemAdded(ManaTrackedItem obj)
		{
			try
			{
				WorldObject wo = CoreManager.Current.WorldFilter[obj.Id];

				if (wo == null)
					return;

				VirindiViewService.Controls.HudList.HudListRowAccessor newRow = mainView.ManaList.AddRow();

				((VirindiViewService.Controls.HudPictureBox)newRow[0]).Image = wo.Icon + 0x6000000;
				((VirindiViewService.Controls.HudStaticText)newRow[1]).Text = wo.Name;
				((VirindiViewService.Controls.HudStaticText)newRow[5]).Text = obj.Id.ToString();

				Item_Changed(obj);

				obj.Changed += new Action<ManaTrackedItem>(Item_Changed);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void manaTracker_ItemRemoved(ManaTrackedItem obj)
		{
			try
			{
				obj.Changed -= new Action<ManaTrackedItem>(Item_Changed);

				for (int row = 1 ; row <= mainView.ManaList.RowCount ; row++)
				{
					if (int.Parse(((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row - 1][5]).Text) == obj.Id)
					{
						mainView.ManaList.RemoveRow(row - 1);

						row--;
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Item_Changed(ManaTrackedItem obj)
		{
			try
			{
				for (int row = 1 ; row <= mainView.ManaList.RowCount ; row++)
				{
					if (int.Parse(((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row - 1][5]).Text) == obj.Id)
					{
						if (obj.ItemState == ManaTrackedItemState.Active)
							((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[row - 1][2]).Image = IconActive;
						else if (obj.ItemState == ManaTrackedItemState.NotActive)
							((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[row - 1][2]).Image = IconNotActive;
						else if (obj.ItemState == ManaTrackedItemState.Unknown)
							((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[row - 1][2]).Image = IconUnknown;
						else
							((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[row - 1][2]).Image = IconNone;

						if (obj.ItemState != ManaTrackedItemState.Active && obj.ItemState != ManaTrackedItemState.NotActive)
						{
							((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row - 1][3]).Text = "-";
							((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row - 1][4]).Text = "-";
							((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row - 1][6]).Text = int.MaxValue.ToString();
						}
						else
						{
							((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row - 1][3]).Text = obj.CalculatedCurrentMana + " / " + obj.MaximumMana;
							((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row - 1][4]).Text = string.Format("{0:d}h{1:d2}m", (int)obj.ManaTimeRemaining.TotalHours, (int)obj.ManaTimeRemaining.Minutes);
							((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row - 1][6]).Text = obj.ManaTimeRemaining.TotalSeconds.ToString();
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

			if (mainView.ManaList.RowCount == 0)
				return;

			for (int row = 0 ; row < mainView.ManaList.RowCount - 1 ; row++)
			{
				for (int compareRow = row + 1 ; compareRow < mainView.ManaList.RowCount ; compareRow++)
				{
					if (double.Parse(((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row][6]).Text) > double.Parse(((VirindiViewService.Controls.HudStaticText)mainView.ManaList[compareRow][6]).Text))
					{
						
						VirindiViewService.ACImage obj0 = ((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[row][0]).Image;
						((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[row][0]).Image = ((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[compareRow][0]).Image;
						((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[compareRow][0]).Image = obj0;

						string obj1 = ((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row][1]).Text;
						((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row][1]).Text = ((VirindiViewService.Controls.HudStaticText)mainView.ManaList[compareRow][1]).Text;
						((VirindiViewService.Controls.HudStaticText)mainView.ManaList[compareRow][1]).Text = obj1;

						VirindiViewService.ACImage obj2 = ((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[row][2]).Image;
						((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[row][2]).Image = ((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[compareRow][2]).Image;
						((VirindiViewService.Controls.HudPictureBox)mainView.ManaList[compareRow][2]).Image = obj2;

						string obj3 = ((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row][3]).Text;
						((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row][3]).Text = ((VirindiViewService.Controls.HudStaticText)mainView.ManaList[compareRow][3]).Text;
						((VirindiViewService.Controls.HudStaticText)mainView.ManaList[compareRow][3]).Text = obj3;

						string obj4 = ((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row][4]).Text;
						((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row][4]).Text = ((VirindiViewService.Controls.HudStaticText)mainView.ManaList[compareRow][4]).Text;
						((VirindiViewService.Controls.HudStaticText)mainView.ManaList[compareRow][4]).Text = obj4;

						string obj5 = ((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row][5]).Text;
						((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row][5]).Text = ((VirindiViewService.Controls.HudStaticText)mainView.ManaList[compareRow][5]).Text;
						((VirindiViewService.Controls.HudStaticText)mainView.ManaList[compareRow][5]).Text = obj5;

						string obj6 = ((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row][6]).Text;
						((VirindiViewService.Controls.HudStaticText)mainView.ManaList[row][6]).Text = ((VirindiViewService.Controls.HudStaticText)mainView.ManaList[compareRow][6]).Text;
						((VirindiViewService.Controls.HudStaticText)mainView.ManaList[compareRow][6]).Text = obj6;
					}
				}
			}
		}
	}
}
