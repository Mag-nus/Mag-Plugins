using System;
using System.Collections.Generic;
using System.Globalization;
using Decal.Adapter;

using Mag.Shared;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class AccountServerCharacterGUI : IDisposable
	{
		private readonly HudTextBox loginText;
		private readonly HudList loginList;

		private readonly HudTextBox loginCompleteText;
		private readonly HudList loginCompleteList;

		private readonly HudTextBox periodicCommandText;
		private readonly HudTextBox periodicCommandInterval;
		private readonly HudTextBox periodicCommandOffset;
		private readonly HudList periodicCommandList;

		public AccountServerCharacterGUI(MainView mainView)
		{
			try
			{
				loginText = mainView.LoginText;
				mainView.LoginAdd.Hit += new EventHandler(LoginAdd_Hit);
				loginList = mainView.LoginList;
				loginList.Click += new HudList.delClickedControl(loginList_Click);

				loginCompleteText = mainView.LoginCompleteText;
				mainView.LoginCompleteAdd.Hit += new EventHandler(LoginCompleteAdd_Hit);
				loginCompleteList = mainView.LoginCompleteList;
				loginCompleteList.Click += new HudList.delClickedControl(loginCompleteList_Click);

				periodicCommandText = mainView.PeriodicCommandText;
				periodicCommandInterval = mainView.PeriodicCommandInterval;
				periodicCommandOffset = mainView.PeriodicCommandOffset;
				mainView.PeriodicCommandAdd.Hit += new EventHandler(PeriodicCommandAdd_Hit);
				periodicCommandList = mainView.PeriodicCommandList;
				periodicCommandList.Click += new HudList.delClickedControl(periodicCommandList_Click);

				CoreManager.Current.CharacterFilter.Login += new EventHandler<Decal.Adapter.Wrappers.LoginEventArgs>(CharacterFilter_Login);
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
					CoreManager.Current.CharacterFilter.Login -= new EventHandler<Decal.Adapter.Wrappers.LoginEventArgs>(CharacterFilter_Login);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}

		}
		void CharacterFilter_Login(object sender, Decal.Adapter.Wrappers.LoginEventArgs e)
		{
			try
			{
				var loginCommands = Settings.SettingsManager.AccountServerCharacter.GetOnLoginCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name);

				foreach (var command in loginCommands)
				{
					HudList.HudListRowAccessor newRow = loginList.AddRow();

					((HudStaticText)newRow[0]).Text = command;
					((HudPictureBox)newRow[1]).Image = 0x60028FC;
					((HudPictureBox)newRow[2]).Image = 0x60028FD;
					((HudPictureBox)newRow[3]).Image = 0x60011F8;
				}

				var loginCompleteCommands = Settings.SettingsManager.AccountServerCharacter.GetOnLoginCompleteCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name);

				foreach (var command in loginCompleteCommands)
				{
					HudList.HudListRowAccessor newRow = loginCompleteList.AddRow();

					((HudStaticText)newRow[0]).Text = command;
					((HudPictureBox)newRow[1]).Image = 0x60028FC;
					((HudPictureBox)newRow[2]).Image = 0x60028FD;
					((HudPictureBox)newRow[3]).Image = 0x60011F8;
				}

				var periodicCommands = Settings.SettingsManager.AccountServerCharacter.GetPeriodicCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name);

				foreach (var command in periodicCommands)
				{
					HudList.HudListRowAccessor newRow = periodicCommandList.AddRow();

					((HudStaticText)newRow[0]).Text = command.Command;
					((HudStaticText)newRow[1]).Text = command.Interval.TotalMinutes.ToString(CultureInfo.InvariantCulture);
					((HudStaticText)newRow[2]).Text = command.OffsetFromMidnight.TotalMinutes.ToString(CultureInfo.InvariantCulture);
					((HudPictureBox)newRow[3]).Image = 0x60011F8;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void LoginAdd_Hit(object sender, EventArgs e)
		{
			try
			{
				var command = loginText.Text;

				if (String.IsNullOrEmpty(command))
					return;

				loginText.Text = String.Empty;

				var commands = Settings.SettingsManager.AccountServerCharacter.GetOnLoginCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name);

				commands.Add(command);

				Settings.SettingsManager.AccountServerCharacter.SetOnLoginCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name, commands);

				HudList.HudListRowAccessor newRow = loginList.AddRow();

				((HudStaticText)newRow[0]).Text = command;
				((HudPictureBox)newRow[1]).Image = 0x60028FC;
				((HudPictureBox)newRow[2]).Image = 0x60028FD;
				((HudPictureBox)newRow[3]).Image = 0x60011F8;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void loginList_Click(object sender, int row, int col)
		{
			try
			{
				if ((row == 0 && col == 1) || (row == loginList.RowCount - 1 && col == 2))
					return;

				if (col == 1)
				{
					string temp = ((HudStaticText)loginList[row][0]).Text;
					((HudStaticText)loginList[row][0]).Text = ((HudStaticText)loginList[row - 1][0]).Text;
					((HudStaticText)loginList[row - 1][0]).Text = temp;
				}
				else if (col == 2)
				{
					string temp = ((HudStaticText)loginList[row][0]).Text;
					((HudStaticText)loginList[row][0]).Text = ((HudStaticText)loginList[row + 1][0]).Text;
					((HudStaticText)loginList[row + 1][0]).Text = temp;
				}
				else if (col == 3)
					loginList.RemoveRow(row);
				else
					return;

				var commands = new List<string>();

				for (int i = 0 ; i < loginList.RowCount ; i++)
					commands.Add(((HudStaticText)loginList[i][0]).Text);

				Settings.SettingsManager.AccountServerCharacter.SetOnLoginCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name, commands);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void LoginCompleteAdd_Hit(object sender, EventArgs e)
		{
			try
			{
				var command = loginCompleteText.Text;

				if (String.IsNullOrEmpty(command))
					return;

				loginCompleteText.Text = String.Empty;

				var commands = Settings.SettingsManager.AccountServerCharacter.GetOnLoginCompleteCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name);

				commands.Add(command);

				Settings.SettingsManager.AccountServerCharacter.SetOnLoginCompleteCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name, commands);

				HudList.HudListRowAccessor newRow = loginCompleteList.AddRow();

				((HudStaticText)newRow[0]).Text = command;
				((HudPictureBox)newRow[1]).Image = 0x60028FC;
				((HudPictureBox)newRow[2]).Image = 0x60028FD;
				((HudPictureBox)newRow[3]).Image = 0x60011F8;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void loginCompleteList_Click(object sender, int row, int col)
		{
			try
			{
				if ((row == 0 && col == 1) || (row == loginCompleteList.RowCount - 1 && col == 2))
					return;

				if (col == 1)
				{
					string temp = ((HudStaticText)loginCompleteList[row][0]).Text;
					((HudStaticText)loginCompleteList[row][0]).Text = ((HudStaticText)loginCompleteList[row - 1][0]).Text;
					((HudStaticText)loginCompleteList[row - 1][0]).Text = temp;
				}
				else if (col == 2)
				{
					string temp = ((HudStaticText)loginCompleteList[row][0]).Text;
					((HudStaticText)loginCompleteList[row][0]).Text = ((HudStaticText)loginCompleteList[row + 1][0]).Text;
					((HudStaticText)loginCompleteList[row + 1][0]).Text = temp;
				}
				else if (col == 3)
					loginCompleteList.RemoveRow(row);
				else
					return;

				var commands = new List<string>();

				for (int i = 0; i < loginCompleteList.RowCount; i++)
					commands.Add(((HudStaticText)loginCompleteList[i][0]).Text);

				Settings.SettingsManager.AccountServerCharacter.SetOnLoginCompleteCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name, commands);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void PeriodicCommandAdd_Hit(object sender, EventArgs e)
		{
			try
			{
				var command = periodicCommandText.Text;
				var intervalText = periodicCommandInterval.Text;
				var offsetText = periodicCommandOffset.Text;

				if (String.IsNullOrEmpty(command) || String.IsNullOrEmpty(intervalText) || String.IsNullOrEmpty(offsetText))
					return;

				int interval;
				int offset;

				if (!int.TryParse(intervalText, out interval) || interval <= 0 || !int.TryParse(offsetText, out offset) || offset < 0)
					return;

				periodicCommandText.Text = String.Empty;

				var commands = Settings.SettingsManager.AccountServerCharacter.GetPeriodicCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name);

				commands.Add(new Settings.SettingsManager.PeriodicCommand(command, TimeSpan.FromMinutes(interval), TimeSpan.FromMinutes(offset)));

				Settings.SettingsManager.AccountServerCharacter.SetPeriodicCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name, commands);

				HudList.HudListRowAccessor newRow = periodicCommandList.AddRow();

				((HudStaticText)newRow[0]).Text = command;
				((HudStaticText)newRow[1]).Text = interval.ToString(CultureInfo.InvariantCulture);
				((HudStaticText)newRow[2]).Text = offset.ToString(CultureInfo.InvariantCulture);
				((HudPictureBox)newRow[3]).Image = 0x60011F8;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void periodicCommandList_Click(object sender, int row, int col)
		{
			try
			{
				if (col == 3)
					periodicCommandList.RemoveRow(row);
				else
					return;

				var commands = new List<Settings.SettingsManager.PeriodicCommand>();

				for (int i = 0; i < periodicCommandList.RowCount; i++)
				{
					int interval;
					int offset;

					int.TryParse(((HudStaticText)periodicCommandList[i][1]).Text, out interval);
					int.TryParse(((HudStaticText)periodicCommandList[i][2]).Text, out offset);

					commands.Add(new Settings.SettingsManager.PeriodicCommand(((HudStaticText)periodicCommandList[i][0]).Text, TimeSpan.FromMinutes(interval), TimeSpan.FromMinutes(offset)));
				}

				Settings.SettingsManager.AccountServerCharacter.SetPeriodicCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name, commands);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
