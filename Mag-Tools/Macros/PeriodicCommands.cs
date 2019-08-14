using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Decal.Adapter;

using Mag.Shared;

namespace MagTools.Macros
{
	class PeriodicCommands : IDisposable
	{
		readonly Timer timer = new Timer();

		public PeriodicCommands()
		{
			try
			{
				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);

				timer.Interval = 20000; // An interval of 20 seconds means we'll attempt to run commands 3 times a minute.
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
					CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);

					timer.Tick -= new EventHandler(timer_Tick);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				timer.Start();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		DateTime lastTick;

		readonly Queue<Settings.SettingsManager.PeriodicCommand> pendingCommands = new Queue<Settings.SettingsManager.PeriodicCommand>();

		void timer_Tick(object sender, EventArgs e)
		{
			try
			{
				// Make sure we don't run this minutes commands twice
				if (lastTick.Minute == DateTime.UtcNow.Minute)
					return;

				lastTick = DateTime.UtcNow;

				var periodicCommands = Settings.SettingsManager.AccountServerCharacter.GetPeriodicCommands(CoreManager.Current.CharacterFilter.AccountName, CoreManager.Current.CharacterFilter.Server, CoreManager.Current.CharacterFilter.Name);

				var serverPeriodicCommands = Settings.SettingsManager.Server.GetPeriodicCommands(CoreManager.Current.CharacterFilter.Server);

				if (periodicCommands.Count == 0 && serverPeriodicCommands.Count == 0)
					return;

				int previousPendingCommandCount = pendingCommands.Count;

				int minutesAfterMidnight = (int)(DateTime.Now - DateTime.Today).TotalMinutes;

				foreach (var periodicCommand in periodicCommands)
				{
					if ((minutesAfterMidnight + (int)periodicCommand.OffsetFromMidnight.TotalMinutes) % (int)periodicCommand.Interval.TotalMinutes == 0)
						pendingCommands.Enqueue(periodicCommand);
				}

				foreach (var periodicCommand in serverPeriodicCommands)
				{
					if ((minutesAfterMidnight + (int)periodicCommand.OffsetFromMidnight.TotalMinutes) % (int)periodicCommand.Interval.TotalMinutes == 0)
						pendingCommands.Enqueue(periodicCommand);
				}

				if (previousPendingCommandCount == 0 && pendingCommands.Count > 0)
					CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				var pendingCommand = pendingCommands.Dequeue();

				if (pendingCommand.Command.ToLower().StartsWith("/mt"))
					PluginCore.Current.ProcessMTCommand(pendingCommand.Command);
				else
					DecalProxy.DispatchChatToBoxWithPluginIntercept(pendingCommand.Command);
			}
			catch (Exception ex) { Debug.LogException(ex); }
			finally
			{
				if (pendingCommands.Count == 0)
					CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);
			}
		}
	}
}
