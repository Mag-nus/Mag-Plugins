using System;
using System.Collections.Generic;

using Decal.Adapter;

using Mag.Shared;

namespace MagTools.Macros
{
	class LoginActions : IDisposable
	{
		public LoginActions()
		{
			try
			{
				CoreManager.Current.CharacterFilter.Login += new EventHandler<Decal.Adapter.Wrappers.LoginEventArgs>(CharacterFilter_Login);
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
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
					CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		readonly Queue<string> pendingCommands = new Queue<string>();

		void CharacterFilter_Login(object sender, Decal.Adapter.Wrappers.LoginEventArgs e)
		{
			try
			{
				var commands = Settings.SettingsManager.AccountServerCharacter.GetOnLoginCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name);

				var serverCommands = Settings.SettingsManager.Server.GetOnLoginCommands(CoreManager.Current.CharacterFilter.Server);

				if (commands.Count == 0 && serverCommands.Count == 0)
					return;

				if (pendingCommands.Count == 0)
					CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);

				// This queues up a dummy command so our actions happen one frame after all the other plugins have finished Login
				pendingCommands.Enqueue(null);

				foreach (var action in commands)
					pendingCommands.Enqueue(action);

				foreach (var action in serverCommands)
					pendingCommands.Enqueue(action);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				var commands = Settings.SettingsManager.AccountServerCharacter.GetOnLoginCompleteCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name);

				var serverCommands = Settings.SettingsManager.Server.GetOnLoginCompleteCommands(CoreManager.Current.CharacterFilter.Server);

				if (commands.Count == 0 && serverCommands.Count == 0)
					return;

				if (pendingCommands.Count == 0)
					CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);

				// This queues up a dummy command so our actions happen one frame after all the other plugins have finished LoginComplete
				pendingCommands.Enqueue(null);

				foreach (var action in commands)
					pendingCommands.Enqueue(action);

				foreach (var action in serverCommands)
					pendingCommands.Enqueue(action);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				var pendingCommand = pendingCommands.Dequeue();

				if (!String.IsNullOrEmpty(pendingCommand))
					processCommand(pendingCommand);
			}
			catch (Exception ex) { Debug.LogException(ex); }
			finally
			{
				if (pendingCommands.Count == 0)
					CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);
			}
		}

		void processCommand(string command)
		{
			if (command.ToLower().StartsWith("/mt"))
				PluginCore.Current.ProcessMTCommand(command);
			else
				DecalProxy.DispatchChatToBoxWithPluginIntercept(command);
		}
	}
}
