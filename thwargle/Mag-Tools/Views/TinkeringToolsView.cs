using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class TinkeringToolsView : IDisposable
	{
		readonly HudCombo tinkeringMaterial;
		readonly HudTextBox tinkeringMinimumPercent;
		readonly HudTextBox tinkeringTargetTotalTinks;
		readonly HudList tinkeringList;

		Timer timer = new Timer();

		public TinkeringToolsView(MainView mainView)
		{
			try
			{
				tinkeringMaterial = mainView.TinkeringMaterial;
				tinkeringMinimumPercent = mainView.TinkeringMinimumPercent;
				tinkeringTargetTotalTinks = mainView.TinkeringTargetTotalTinks;
				tinkeringList = mainView.TinkeringList;

				mainView.TinkeringAddSelectedItem.Hit += new EventHandler(TinkeringAddSelectedItem_Hit);

				mainView.TinkeringMaterial.AddItem("Brass", null);
				mainView.TinkeringMaterial.AddItem("Granite", null);
				mainView.TinkeringMaterial.AddItem("Green Garnet", null);
				mainView.TinkeringMaterial.AddItem("Iron", null);
				mainView.TinkeringMaterial.AddItem("Mahogany", null);
				mainView.TinkeringMaterial.AddItem("Steel", null);
				mainView.TinkeringMaterial.AddItem("Velvet", null);

				mainView.TinkeringMinimumPercent.Text = "100";

				mainView.TinkeringStart.Hit += new EventHandler(TinkeringStart_Hit);
				mainView.TinkeringStop.Hit += new EventHandler(TinkeringStop_Hit);

				mainView.TinkeringList.Click += new VirindiViewService.Controls.HudList.delClickedControl(TinkeringList_Click);

				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				CoreManager.Current.EchoFilter.ServerDispatch += new EventHandler<NetworkMessageEventArgs>(EchoFilter_ServerDispatch);

				timer.Interval = 1000;
				timer.Tick += new EventHandler(timer_Tick);
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
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
					CoreManager.Current.EchoFilter.ServerDispatch -= new EventHandler<NetworkMessageEventArgs>(EchoFilter_ServerDispatch);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void TinkeringAddSelectedItem_Hit(object sender, EventArgs e)
		{
			try
			{
				if (CoreManager.Current.Actions.CurrentSelection == 0)
					return;

				WorldObject wo = CoreManager.Current.WorldFilter[CoreManager.Current.Actions.CurrentSelection];

				if (wo == null)
					return;

				if (wo.ObjectClass != ObjectClass.Armor && wo.ObjectClass != ObjectClass.Clothing &&
					wo.ObjectClass != ObjectClass.MeleeWeapon && wo.ObjectClass != ObjectClass.MissileWeapon && wo.ObjectClass != ObjectClass.WandStaffOrb &&
					wo.ObjectClass != ObjectClass.Jewelry)
					return;

				for (int row = 0 ; row < tinkeringList.RowCount ; row++)
				{
					int id;

					if (int.TryParse(((HudStaticText)tinkeringList[row][6]).Text, out id) && id == wo.Id)
						return;
				}

				HudList.HudListRowAccessor newRow = tinkeringList.AddRow();

				//((HudPictureBox)newRow[0]).Image
				((HudPictureBox)newRow[1]).Image = wo.Icon + 0x6000000;
				((HudStaticText)newRow[2]).Text = wo.Name;
				//((HudStaticText)newRow[3]).Text
				//((HudStaticText)newRow[4]).Text
				((HudPictureBox)newRow[5]).Image = 0x60011F8;
				((HudStaticText)newRow[6]).Text = wo.Id.ToString(CultureInfo.InvariantCulture);

				CoreManager.Current.Actions.RequestId(wo.Id);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		bool running;

		void TinkeringStart_Hit(object sender, EventArgs e)
		{
			try
			{
				if (running)
					return;

				running = true;

				foreach (var wo in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (wo.ObjectClass == ObjectClass.Salvage && !wo.HasIdData)
					{
						MyWorldObject mwo = MyWorldObjectCreator.Create(wo);

						if (mwo.Material == ((HudStaticText)tinkeringMaterial[tinkeringMaterial.Current]).Text)
							CoreManager.Current.Actions.RequestId(wo.Id);
					}
				}

				timer.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void TinkeringStop_Hit(object sender, EventArgs e)
		{
			try
			{
				if (!running)
					return;

				running = false;

				timer.Stop();

				for (int row = 0; row < tinkeringList.RowCount; row++)
					((HudStaticText)tinkeringList[row][0]).Text = "";
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void timer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (!running)
					return;

				if (CoreManager.Current.Actions.CombatMode != CombatState.Peace)
				{
					TinkeringStop_Hit(null, null);
					return;
				}

				if (CoreManager.Current.Actions.BusyState != 0)
					return;

				// Do we have bags of salvage without id data?
				foreach (var wo in CoreManager.Current.WorldFilter.GetInventory())
				{
					if (wo.ObjectClass == ObjectClass.Salvage && !wo.HasIdData)
					{
						MyWorldObject mwo = MyWorldObjectCreator.Create(wo);

						if (mwo.Material == ((HudStaticText)tinkeringMaterial[tinkeringMaterial.Current]).Text)
						{
							CoreManager.Current.Actions.RequestId(wo.Id);
							return;
						}
					}
				}

				TinkeringStop_Hit(null, null);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void TinkeringList_Click(object sender, int row, int col)
		{
			try
			{
				if (col == 5)
					tinkeringList.RemoveRow(row);
				else
				{
					int id;

					if (int.TryParse(((HudStaticText)tinkeringList[row][6]).Text, out id))
						CoreManager.Current.Actions.SelectItem(id);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				if (e.Change != WorldChangeType.IdentReceived)
					return;

				for (int row = 0; row < tinkeringList.RowCount; row++)
				{
					int id;

					if (int.TryParse(((HudStaticText)tinkeringList[row][6]).Text, out id) && id == e.Changed.Id)
					{
						MyWorldObject mwo = MyWorldObjectCreator.Create(e.Changed);

						((HudStaticText)tinkeringList[row][3]).Text = mwo.Workmanship.ToString(CultureInfo.InvariantCulture);
						((HudStaticText)tinkeringList[row][4]).Text = mwo.Tinks.ToString(CultureInfo.InvariantCulture);

						return;
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		// You determine that you have a 100 percent chance to succeed.
		private static readonly Regex PercentConfirmation = new Regex("^You determine that you have a (?<percent>.+) percent chance to succeed.$");

		void EchoFilter_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				if (!Settings.SettingsManager.Tinkering.AutoClickYes.Value)
					return;

				if (e.Message.Type == 0xF7B0 && (int)e.Message["event"] == 0x0274 && e.Message.Value<int>("type") == 5)
				{
					Match match = PercentConfirmation.Match(e.Message.Value<string>("text"));

					if (match.Success)
					{
						int minimumPercent;
						int percent;

						if (int.TryParse(match.Groups["percent"].Value, out percent) && int.TryParse(tinkeringMinimumPercent.Text, out minimumPercent) && percent >= minimumPercent)
							PostMessageTools.ClickYes();
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
