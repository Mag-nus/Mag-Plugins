using System;
using System.Collections.Generic;

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
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void LoginAdd_Hit(object sender, EventArgs e)
		{
			try
			{
				var action = loginText.Text;

				if (String.IsNullOrEmpty(action))
					return;

				loginText.Text = String.Empty;

				var actions = Settings.SettingsManager.AccountServerCharacter.GetOnLoginCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name);

				actions.Add(action);

				Settings.SettingsManager.AccountServerCharacter.SetOnLoginCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name, actions);

				HudList.HudListRowAccessor newRow = loginList.AddRow();

				((HudStaticText)newRow[0]).Text = action;
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

				if (col == 2)
				{
					string temp = ((HudStaticText)loginList[row][0]).Text;
					((HudStaticText)loginList[row][0]).Text = ((HudStaticText)loginList[row + 1][0]).Text;
					((HudStaticText)loginList[row + 1][0]).Text = temp;
				}

				if (col == 3)
					loginList.RemoveRow(row);

				var actions = new List<string>();

				for (int i = 0 ; i < loginList.RowCount ; i++)
					actions.Add(((HudStaticText)loginList[i][0]).Text);

				Settings.SettingsManager.AccountServerCharacter.SetOnLoginCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name, actions);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void LoginCompleteAdd_Hit(object sender, EventArgs e)
		{
			try
			{
				var action = loginCompleteText.Text;

				if (String.IsNullOrEmpty(action))
					return;

				loginCompleteText.Text = String.Empty;

				var actions = Settings.SettingsManager.AccountServerCharacter.GetOnLoginCompleteCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name);

				actions.Add(action);

				Settings.SettingsManager.AccountServerCharacter.SetOnLoginCompleteCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name, actions);

				HudList.HudListRowAccessor newRow = loginCompleteList.AddRow();

				((HudStaticText)newRow[0]).Text = action;
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

				if (col == 2)
				{
					string temp = ((HudStaticText)loginCompleteList[row][0]).Text;
					((HudStaticText)loginCompleteList[row][0]).Text = ((HudStaticText)loginCompleteList[row + 1][0]).Text;
					((HudStaticText)loginCompleteList[row + 1][0]).Text = temp;
				}

				if (col == 3)
					loginCompleteList.RemoveRow(row);

				var actions = new List<string>();

				for (int i = 0; i < loginCompleteList.RowCount; i++)
					actions.Add(((HudStaticText)loginCompleteList[i][0]).Text);

				Settings.SettingsManager.AccountServerCharacter.SetOnLoginCompleteCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name, actions);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
