using System;
using System.Collections.ObjectModel;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

using MyClasses.MetaViewWrappers;

namespace MagTools.Trackers
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

		public void Dispose()
		{
			try
			{
				manaTracker.ItemAdded -= new Action<ManaTrackedItem>(manaTracker_ItemAdded);
				manaTracker.ItemRemoved -= new Action<ManaTrackedItem>(manaTracker_ItemRemoved);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void manaTracker_ItemAdded(ManaTrackedItem obj)
		{
			try
			{
				WorldObject wo = CoreManager.Current.WorldFilter[obj.Id];

				if (wo == null)
					return;

				IListRow newRow = mainView.ManaList.Add();
				newRow[0][1] = wo.Icon + 0x6000000;
				newRow[1][0] = wo.Name;
				newRow[5][0] = obj.Id.ToString();

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
					if (int.Parse(mainView.ManaList[row - 1][5][0].ToString()) == obj.Id)
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
					if (int.Parse(mainView.ManaList[row - 1][5][0].ToString()) == obj.Id)
					{
						if (obj.ItemState == ManaTrackedItemState.Active)
							mainView.ManaList[row - 1][2][1] = IconActive;
						else if (obj.ItemState == ManaTrackedItemState.NotActive)
							mainView.ManaList[row - 1][2][1] = IconNotActive;
						else if (obj.ItemState == ManaTrackedItemState.Unknown)
							mainView.ManaList[row - 1][2][1] = IconUnknown;
						else
							mainView.ManaList[row - 1][2][1] = IconNone;

						if (obj.ItemState != ManaTrackedItemState.Active && obj.ItemState != ManaTrackedItemState.NotActive)
						{
							mainView.ManaList[row - 1][3][0] = "-";
							mainView.ManaList[row - 1][4][0] = "-";
							mainView.ManaList[row - 1][6][0] = int.MaxValue.ToString();
						}
						else
						{
							mainView.ManaList[row - 1][3][0] = obj.CalculatedCurrentMana + " / " + obj.MaximumMana;
							mainView.ManaList[row - 1][4][0] = string.Format("{0:d}h{1:d2}m", (int)obj.ManaTimeRemaining.TotalHours, (int)obj.ManaTimeRemaining.Minutes);
							mainView.ManaList[row - 1][6][0] = obj.ManaTimeRemaining.TotalSeconds.ToString();
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
					if (double.Parse(mainView.ManaList[row][6][0].ToString()) > double.Parse(mainView.ManaList[compareRow][6][0].ToString()))
					{
						object obj;

						obj = mainView.ManaList[row][0][1];
						mainView.ManaList[row][0][1] = mainView.ManaList[compareRow][0][1];
						mainView.ManaList[compareRow][0][1] = obj;

						obj = mainView.ManaList[row][1][0];
						mainView.ManaList[row][1][0] = mainView.ManaList[compareRow][1][0];
						mainView.ManaList[compareRow][1][0] = obj;

						obj = mainView.ManaList[row][2][1];
						mainView.ManaList[row][2][1] = mainView.ManaList[compareRow][2][1];
						mainView.ManaList[compareRow][2][1] = obj;

						obj = mainView.ManaList[row][3][0];
						mainView.ManaList[row][3][0] = mainView.ManaList[compareRow][3][0];
						mainView.ManaList[compareRow][3][0] = obj;

						obj = mainView.ManaList[row][4][0];
						mainView.ManaList[row][4][0] = mainView.ManaList[compareRow][4][0];
						mainView.ManaList[compareRow][4][0] = obj;

						obj = mainView.ManaList[row][5][0];
						mainView.ManaList[row][5][0] = mainView.ManaList[compareRow][5][0];
						mainView.ManaList[compareRow][5][0] = obj;

						obj = mainView.ManaList[row][6][0];
						mainView.ManaList[row][6][0] = mainView.ManaList[compareRow][6][0];
						mainView.ManaList[compareRow][6][0] = obj;
					}
				}
			}
		}
	}
}
