using System;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class MainView : IDisposable
	{
		readonly VirindiViewService.ViewProperties properties;
		readonly VirindiViewService.ControlGroup controls;
		readonly VirindiViewService.HudView view;

		// Mana Tracker
		public HudList ManaList { get; private set; }

		public HudStaticText ManaTotal { get; private set; }
		HudCheckBox ManaRecharge { get; set; }
		public HudStaticText UnretainedTotal { get; private set; }

		// Combat Tracker - Tracker
		public HudList CombatTrackerMonsterListCurrent { get; private set; }
		public HudList CombatTrackerDamageListCurrent { get; private set; }
		public HudList CombatTrackerMonsterListPersistent { get; private set; }
		public HudList CombatTrackerDamageListPersistent { get; private set; }

		// Combat Tracker - Options
		public HudButton CombatTrackerClearCurrentStats { get; private set; }
		public HudButton CombatTrackerExportCurrentStats { get; private set; }
		public HudButton CombatTrackerClearPersistentStats { get; private set; }

		HudCheckBox CombatTrackerExportOnLogOff { get; set; }
		HudCheckBox CombatTrackerPersistent { get; set; }
		HudCheckBox CombatTrackerSortAlphabetically { get; set; }
		
		// Misc - Options
		HudList OptionList { get; set; }

		// Misc - Filters
		HudList FiltersList { get; set; }

		// Misc - Client
		HudCheckBox ClientRemoveFrame { get; set; }

		HudButton ClientSetWindowPosition { get; set; }
		HudButton ClientDelWindowPosition { get; set; }
		HudStaticText ClientSetPosition { get; set; }

		// Misc - Tools
		public HudButton ClipboardWornEquipment { get; private set; }
		public HudButton ClipboardInventoryInfo { get; private set; }

		// Misc - About
		public HudStaticText VersionLabel { get; private set; }

		public MainView()
		{
			try
			{
				// Create the view
				VirindiViewService.XMLParsers.Decal3XMLParser parser = new VirindiViewService.XMLParsers.Decal3XMLParser();
				parser.ParseFromResource("MagTools.Views.mainView.xml", out properties, out controls);

				// Display the view
				view = new VirindiViewService.HudView(properties, controls);


				// Assign the views objects to our local variables

				// Mana Tracker
				ManaList = view != null ? (HudList)view["ManaList"] : new HudList();

				ManaTotal = view != null ? (HudStaticText)view["ManaTotal"] : new HudStaticText();
				ManaRecharge = view != null ? (HudCheckBox)view["ManaRecharge"] : new HudCheckBox();
				UnretainedTotal = view != null ? (HudStaticText)view["UnretainedTotal"] : new HudStaticText();

				// Combat Tracker - Tracker
				CombatTrackerMonsterListCurrent = view != null ? (HudList)view["CombatTrackerMonsterListCurrent"] : new HudList();
				CombatTrackerDamageListCurrent = view != null ? (HudList)view["CombatTrackerDamageListCurrent"] : new HudList();
				CombatTrackerMonsterListPersistent = view != null ? (HudList)view["CombatTrackerMonsterListPersistent"] : new HudList();
				CombatTrackerDamageListPersistent = view != null ? (HudList)view["CombatTrackerDamageListPersistent"] : new HudList();

				// Combat Tracker - Options
				CombatTrackerClearCurrentStats = view != null ? (HudButton)view["CombatTrackerClearCurrentStats"] : new HudButton();
				CombatTrackerExportCurrentStats = view != null ? (HudButton)view["CombatTrackerExportCurrentStats"] : new HudButton();
				CombatTrackerClearPersistentStats = view != null ? (HudButton)view["CombatTrackerClearPersistentStats"] : new HudButton();

				CombatTrackerExportOnLogOff = view != null ? (HudCheckBox)view["CombatTrackerExportOnLogOff"] : new HudCheckBox();
				CombatTrackerPersistent = view != null ? (HudCheckBox)view["CombatTrackerPersistent"] : new HudCheckBox();
				CombatTrackerSortAlphabetically = view != null ? (HudCheckBox)view["CombatTrackerSortAlphabetically"] : new HudCheckBox();

				// Misc - Options
				OptionList = view != null ? (HudList)view["OptionList"] : new HudList();

				// Misc - Filters
				FiltersList = view != null ? (HudList)view["FiltersList"] : new HudList();

				// Misc - Client
				ClientRemoveFrame = view != null ? (HudCheckBox)view["ClientRemoveFrame"] : new HudCheckBox();

				ClientSetWindowPosition = view != null ? (HudButton)view["ClientSetWindowPosition"] : new HudButton();
				ClientDelWindowPosition = view != null ? (HudButton)view["ClientDelWindowPosition"] : new HudButton();
				ClientSetPosition = view != null ? (HudStaticText)view["ClientSetPosition"] : new HudStaticText();

				// Misc - Tools
				ClipboardWornEquipment = view != null ? (HudButton)view["ClipboardWornEquipment"] : new HudButton();
				ClipboardInventoryInfo = view != null ? (HudButton)view["ClipboardInventoryInfo"] : new HudButton();

				// Misc - About
				VersionLabel = view != null ? (HudStaticText)view["VersionLabel"] : new HudStaticText();


				// ******************************************************
				// Link some of our controls to our configuration manager
				// ******************************************************

				// Mana Tracker
				ManaRecharge.Checked = Settings.SettingsManager.ManaManagement.AutoRecharge.Value;
				Settings.SettingsManager.ManaManagement.AutoRecharge.Changed += (obj) => { ManaRecharge.Checked = obj.Value; };
				ManaRecharge.Change += (s, e) => 
				{
					try
					{
						Settings.SettingsManager.ManaManagement.AutoRecharge.Value = ((HudCheckBox)s).Checked;
					}
					catch (Exception ex) { Debug.LogException(ex); }
				};

				// Combat Tracker
				CombatTrackerExportOnLogOff.Checked = Settings.SettingsManager.CombatTracker.ExportOnLogOff.Value;
				Settings.SettingsManager.CombatTracker.ExportOnLogOff.Changed += (obj) => { CombatTrackerExportOnLogOff.Checked = obj.Value; };
				CombatTrackerExportOnLogOff.Change += (s, e) =>
				{
					try
					{
						Settings.SettingsManager.CombatTracker.ExportOnLogOff.Value = ((HudCheckBox)s).Checked;
					}
					catch (Exception ex) { Debug.LogException(ex); }
				};

				CombatTrackerPersistent.Checked = Settings.SettingsManager.CombatTracker.Persistent.Value;
				Settings.SettingsManager.CombatTracker.Persistent.Changed += (obj) => { CombatTrackerPersistent.Checked = obj.Value; };
				CombatTrackerPersistent.Change += (s, e) =>
				{
					try
					{
						Settings.SettingsManager.CombatTracker.Persistent.Value = ((HudCheckBox)s).Checked;
					}
					catch (Exception ex) { Debug.LogException(ex); }
				};

				CombatTrackerSortAlphabetically.Checked = Settings.SettingsManager.CombatTracker.SortAlphabetically.Value;
				Settings.SettingsManager.CombatTracker.SortAlphabetically.Changed += (obj) => { CombatTrackerSortAlphabetically.Checked = obj.Value; };
				CombatTrackerSortAlphabetically.Change += (s, e) =>
				{
					try
					{
						Settings.SettingsManager.CombatTracker.SortAlphabetically.Value = ((HudCheckBox)s).Checked;
					}
					catch (Exception ex) { Debug.LogException(ex); }
				};

				// Misc.Options
				AddOption(OptionList, Settings.SettingsManager.ItemInfoOnIdent.Enabled);
				AddOption(OptionList, Settings.SettingsManager.ItemInfoOnIdent.ShowBuffedValues);
				AddOption(OptionList, Settings.SettingsManager.ItemInfoOnIdent.ShowValueAndBurden);
				AddOption(OptionList, Settings.SettingsManager.ItemInfoOnIdent.LeftClickIdent);
				AddOption(OptionList, Settings.SettingsManager.ItemInfoOnIdent.AutoClipboard);

				AddOption(OptionList, Settings.SettingsManager.AutoBuySell.Enabled);
				AddOption(OptionList, Settings.SettingsManager.AutoBuySell.TestMode);

				AddOption(OptionList, Settings.SettingsManager.AutoTradeAdd.Enabled);

				AddOption(OptionList, Settings.SettingsManager.AutoTradeAccept.Enabled);

				AddOption(OptionList, Settings.SettingsManager.Looting.AutoLootChests);
				AddOption(OptionList, Settings.SettingsManager.Looting.AutoLootCorpses);
				AddOption(OptionList, Settings.SettingsManager.Looting.AutoLootMyCorpses);
				AddOption(OptionList, Settings.SettingsManager.Looting.LootSalvage);

				AddOption(OptionList, Settings.SettingsManager.Misc.OpenMainPackOnLogin);
				AddOption(OptionList, Settings.SettingsManager.Misc.MaximizeChatOnLogin);
				AddOption(OptionList, Settings.SettingsManager.Misc.DebuggingEnabled);

				// Misc.Filters
				AddOption(FiltersList, Settings.SettingsManager.Filters.AttackEvades);
				AddOption(FiltersList, Settings.SettingsManager.Filters.DefenseEvades);
				AddOption(FiltersList, Settings.SettingsManager.Filters.AttackResists);
				AddOption(FiltersList, Settings.SettingsManager.Filters.DefenseResists);
				AddOption(FiltersList, Settings.SettingsManager.Filters.NPKFails);
				AddOption(FiltersList, Settings.SettingsManager.Filters.DirtyFighting);
				AddOption(FiltersList, Settings.SettingsManager.Filters.MonsterDeaths);

				AddOption(FiltersList, Settings.SettingsManager.Filters.SpellCastingMine);
				AddOption(FiltersList, Settings.SettingsManager.Filters.SpellCastingOthers);
				AddOption(FiltersList, Settings.SettingsManager.Filters.SpellCastFizzles);
				AddOption(FiltersList, Settings.SettingsManager.Filters.CompUsage);
				AddOption(FiltersList, Settings.SettingsManager.Filters.SpellExpires);

				AddOption(FiltersList, Settings.SettingsManager.Filters.HealingKitSuccess);
				AddOption(FiltersList, Settings.SettingsManager.Filters.HealingKitFail);
				AddOption(FiltersList, Settings.SettingsManager.Filters.Salvaging);
				AddOption(FiltersList, Settings.SettingsManager.Filters.SalvagingFails);
				AddOption(FiltersList, Settings.SettingsManager.Filters.ManaStoneUsage);

				AddOption(FiltersList, Settings.SettingsManager.Filters.TradeBuffBotSpam);
				AddOption(FiltersList, Settings.SettingsManager.Filters.FailedAssess);

				AddOption(FiltersList, Settings.SettingsManager.Filters.KillTaskComplete);
				AddOption(FiltersList, Settings.SettingsManager.Filters.VendorTells);
				AddOption(FiltersList, Settings.SettingsManager.Filters.MonsterTell);
				AddOption(FiltersList, Settings.SettingsManager.Filters.NpcChatter);
				AddOption(FiltersList, Settings.SettingsManager.Filters.MasterArbitratorSpam);
				AddOption(FiltersList, Settings.SettingsManager.Filters.AllMasterArbitratorChat);

				AddOption(FiltersList, Settings.SettingsManager.Filters.StatusTextYoureTooBusy);
				AddOption(FiltersList, Settings.SettingsManager.Filters.StatusTextCasting);
				AddOption(FiltersList, Settings.SettingsManager.Filters.StatusTextAll);

				// Misc.Client
				ClientRemoveFrame.Checked = Settings.SettingsManager.Misc.RemoveWindowFrame.Value;
				Settings.SettingsManager.Misc.RemoveWindowFrame.Changed += (obj) => { ClientRemoveFrame.Checked = obj.Value; };
				ClientRemoveFrame.Change += (s, e) =>
				{
					try
					{
						Settings.SettingsManager.Misc.RemoveWindowFrame.Value = ((HudCheckBox)s).Checked;
					}
					catch (Exception ex) { Debug.LogException(ex); }
				};

				ClientSetWindowPosition.Hit += (s, e) =>
				{
					try
					{
						Client.WindowMover.SetWindowPosition();
						UpdateClientWindowPositionLabel();
					}
					catch (Exception ex) { Debug.LogException(ex); }
				};
				ClientDelWindowPosition.Hit += (s, e) => 
				{
					try
					{
						Client.WindowMover.DeleteWindowPosition();
						UpdateClientWindowPositionLabel();
					} catch (Exception ex) { Debug.LogException(ex); }
				};
				UpdateClientWindowPositionLabel();
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
					//Remove the view
					if (view != null)
						view.Dispose();
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void UpdateClientWindowPositionLabel()
		{
			Client.WindowPosition windowPosition;

			if (Client.WindowMover.GetWindowPositionForThisClient(out windowPosition))
				ClientSetPosition.Text = "Current Set Position: X: " + windowPosition.X + ", Y: " + windowPosition.Y;
			else
				ClientSetPosition.Text = "Current Set Position: not set.";
		}

		void AddOption(HudList hudList, Settings.Setting<bool> setting)
		{
			HudList.HudListRowAccessor newRow = hudList.AddRow();

			((HudCheckBox)newRow[0]).Checked = setting.Value;
			setting.Changed += (obj) => { ((HudCheckBox)newRow[0]).Checked = obj.Value; };
			((HudCheckBox)newRow[0]).Change += (s, e) =>
			{
				try
				{
					setting.Value = ((HudCheckBox)s).Checked;
				}
				catch (Exception ex) { Debug.LogException(ex); }
			};
			((HudStaticText)newRow[1]).Text = setting.Description;

		}

		/*
		<page label="Comp Tracker">
		<control progid="DecalControls.FixedLayout" clipped="">
		<control progid="DecalControls.List" name="lstStats" left="0" top="0" width="320" height="380">
		  <column progid="DecalControls.TextColumn" fixedwidth="60" />
		  <column progid="DecalControls.TextColumn" fixedwidth="34" justify="right" />
		  <column progid="DecalControls.TextColumn" fixedwidth="82" justify="right" />
		  <column progid="DecalControls.TextColumn" fixedwidth="82" justify="right" />
		  <column progid="DecalControls.TextColumn" fixedwidth="42" justify="right" />
		</control>
		<control progid="DecalControls.PushButton" name="pbResetStats" left="5" top="380" width="60" height="20" text="Reset" />
		</control>
		</page>
		*/
	}
}
