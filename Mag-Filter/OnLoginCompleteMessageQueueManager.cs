using System;
using System.Collections.Generic;

using Mag.Shared;

using Decal.Adapter;

namespace MagFilter
{
	class OnLoginCompleteMessageQueueManager
	{
		readonly Queue<string> onLoginCompleteMessageQueue = new Queue<string>();
		readonly Queue<string> pendingCommands = new Queue<string>();
		bool sendingLastEnter;
		bool timer_ticked;
		private const int DEF_OLCWAIT = 3000;
		System.Windows.Forms.Timer olcmq_timer = new System.Windows.Forms.Timer();

		private bool disposed;

		public OnLoginCompleteMessageQueueManager()
		{
			try
			{
				olcmq_timer.Interval = DEF_OLCWAIT;
				olcmq_timer.Tick += new EventHandler(ticked);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					olcmq_timer.Tick -= new EventHandler(ticked);
				}

				disposed = true;
			}
		}

		public void FilterCore_ClientDispatch(object sender, NetworkMessageEventArgs e)
		{
			if (onLoginCompleteMessageQueue.Count > 0 && e.Message.Type == 0xF7B1 && Convert.ToInt32(e.Message["action"]) == 0xA1)
			{
				sendingLastEnter = false;
				CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);

				timer_ticked = false;
				olcmq_timer.Start();

				Debug.WriteToChat("On Login Complete Timer started.");

				pendingCommands.Enqueue(null);

				foreach (var action in onLoginCompleteMessageQueue)
					pendingCommands.Enqueue(action);
			}
		}

		public void FilterCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			if (e.Text.StartsWith("/mf olcmq add "))
			{
				onLoginCompleteMessageQueue.Enqueue(e.Text.Substring(14, e.Text.Length - 14));
				Debug.WriteToChat("On Login Complete Message Queue added: " + e.Text);

				e.Eat = true;
			}
			else if (e.Text == "/mf olcmq clear")
			{
				onLoginCompleteMessageQueue.Clear();
				Debug.WriteToChat("On Login Complete Message Queue cleared");

				e.Eat = true;
			}
			// HACK These two following conditionals are hacks
			// TODO Fix
			else if (e.Text.StartsWith("/mf olcwait set "))
			{
				olcmq_timer.Interval = int.Parse(e.Text.Substring(16, e.Text.Length - 16));
				Debug.WriteToChat("Wait time set: " + e.Text);

				e.Eat = true;
			}
			else if (e.Text == "/mf olcwait clear")
			{
				olcmq_timer.Interval = DEF_OLCWAIT;
				Debug.WriteToChat("On Login Complete Wait time reset");

				e.Eat = true;
			}
		}

		void ticked(object sender, EventArgs e)
		{
			olcmq_timer.Stop();
			timer_ticked = true;

			Debug.WriteToChat("On Login Complete Timer stopped");
		}

		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (!timer_ticked)
				{
					return;
				}

				if (onLoginCompleteMessageQueue.Count == 0 && sendingLastEnter == false)
				{
					CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);
					return;
				}

				if (sendingLastEnter)
				{
					PostMessageTools.SendEnter();
					sendingLastEnter = false;
				}
				else
				{
					string msg = onLoginCompleteMessageQueue.Dequeue();
					PostMessageTools.SendEnter();
					PostMessageTools.SendMsg(msg);
					Debug.WriteToChat("On Login Complete Sent: " + msg);
					sendingLastEnter = true;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}