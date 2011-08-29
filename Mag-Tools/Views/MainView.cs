using System;
using System.Collections.Generic;

using VirindiViewService;
using VirindiViewService.Controls;

namespace MagTools.Views
{
	class MainView : IDisposable
	{
		VirindiViewService.ViewProperties properties;
		VirindiViewService.ControlGroup controls;
		VirindiViewService.HudView view;

		public MainView()
		{
			try
			{
				// Create the view
				VirindiViewService.XMLParsers.Decal3XMLParser parser = new VirindiViewService.XMLParsers.Decal3XMLParser();
				parser.ParseFromResource("MagTools.Views.mainView.xml", out properties, out controls);

				// Display the view
				view = new VirindiViewService.HudView(properties, controls);

				// Wire up the controls

				// Mana Tracker
				ManaList = (HudList)view["ManaList"] ?? new HudList();

				ManaTotal = (HudStaticText)view["ManaTotal"] ?? new HudStaticText();

				// Misc
				OptionList = (HudList)view["OptionList"] ?? new HudList();

				VersionLabel = (HudStaticText)view["VersionLabel"] ?? new HudStaticText();
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
					//Remove the view
					view.Dispose();
					controls.Dispose();
					properties.Dispose();
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		// Mana Tracker
		public HudList ManaList { get; private set; }

		public HudStaticText ManaTotal { get; private set; }

		// Misc
		private HudList OptionList { get; set; }

		private List<Option> options = new List<Option>();

		public void AddOption(Option option)
		{
			VirindiViewService.Controls.HudList.HudListRowAccessor newRow = OptionList.AddRow();

			options.Add(option);

			((VirindiViewService.Controls.HudCheckBox)newRow[0]).Change += new EventHandler(Option_Change);
			((VirindiViewService.Controls.HudStaticText)newRow[1]).Text = option.Name;
		}

		public event Action<Option> OptionEnabled;
		public event Action<Option> OptionDisabled;

		void Option_Change(object sender, EventArgs e)
		{
			try
			{
				VirindiViewService.Controls.HudCheckBox hudCheckBox = sender as VirindiViewService.Controls.HudCheckBox;

				for (int row = 1 ; row <= OptionList.RowCount ; row++)
				{
					if (OptionList[row - 1][0] == sender)
					{
						if (hudCheckBox.Checked && OptionEnabled != null)
							OptionEnabled(options[row - 1]);
						if (hudCheckBox.Checked == false && OptionDisabled != null)
							OptionDisabled(options[row - 1]);
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		/// <summary>
		/// Changing an option through this function will not raise OptionEnabled or OptionDisabled.
		/// </summary>
		/// <param name="option"></param>
		/// <param name="optionChecked"></param>
		public void SetOption(Option option, bool optionChecked)
		{
			if (!options.Contains(option))
				return;

			int index = options.IndexOf(option);

			((VirindiViewService.Controls.HudCheckBox)OptionList[index][0]).Change -= new EventHandler(Option_Change);
			((VirindiViewService.Controls.HudCheckBox)OptionList[index][0]).Checked = optionChecked;
			((VirindiViewService.Controls.HudCheckBox)OptionList[index][0]).Change += new EventHandler(Option_Change);
		}

		public HudStaticText VersionLabel { get; private set; }
	}
}
