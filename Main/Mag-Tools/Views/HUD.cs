using System;
using System.Drawing;

using MagTools.Trackers.Equipment;

using Mag.Shared;

using VirindiViewService;
using VirindiViewService.Controls;

using Decal.Adapter;

namespace MagTools.Views
{
	class HUD : IDisposable
	{
		System.Windows.Forms.Timer hudUpdateTimer = new System.Windows.Forms.Timer();

		readonly EquipmentTracker equipmentTracker;

		HudView hudView;
		HudList hudListHead;

		public HUD(EquipmentTracker equipmentTracker)
		{
			try
			{
				return;
				hudUpdateTimer.Tick += new EventHandler(hudUpdateTimer_Tick);

				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
				CoreManager.Current.CharacterFilter.Logoff += new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);

				this.equipmentTracker = equipmentTracker;
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
					CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
					CoreManager.Current.CharacterFilter.Logoff -= new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}

		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				hudView = new HudView("Mag-HUD", 200, 80, new ACImage(Color.Black), false);

				hudView.Visible = true;
				hudView.UserMinimizable = false;
				hudView.ShowIcon = false;
				hudView.UserResizeable = true;
				//View.ClickThrough = true;
				hudView.Theme = HudViewDrawStyle.GetThemeByName("Minimalist Transparent");

				hudView.LoadUserSettings();

				hudListHead = new HudList();
				hudView.Controls.HeadControl = hudListHead;

				hudListHead.Padding = 0; // Default: 1
				hudListHead.WPadding = 0; // Default: 7
				hudListHead.WPaddingOuter = 0; // Default: 3

				hudListHead.AddColumn(typeof(HudPictureBox), 16, null);
				hudListHead.AddColumn(typeof(HudStaticText), 999, null);

				HudList.HudListRowAccessor newRow = hudListHead.AddRow();
				((HudPictureBox)newRow[0]).Image = new ACImage(13107); // Major Mana Stone
				//((HudStaticText)newRow[1]).Text = "Cool Stuff Here 0 1 2 3 45 6 7 8sdf 8asdf 8asdf8asdf8asdf";

				hudUpdateTimer.Interval = 1000;
				hudUpdateTimer.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_Logoff(object sender, Decal.Adapter.Wrappers.LogoffEventArgs e)
		{
			try
			{
				hudUpdateTimer.Stop();

				if (hudView != null)
				{
					hudView.Dispose();
					hudView = null;
				}

				if (hudListHead != null)
				{
					hudListHead.Dispose();
					hudListHead = null;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void hudUpdateTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				/*((HudStaticText)hudListHead[0][1]).Text = new DateTime(equipmentTracker.RemainingTimeBeforeNextEmptyItem.Ticks).ToString("H:mm.ss") + " remaining, " + equipmentTracker.ManaNeededToRefillItems + " to refill";

				if (equipmentTracker.NumberOfInactiveItems > 0 || equipmentTracker.NumberOfUnretainedItems > 0)
				{
					
				}*/
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
