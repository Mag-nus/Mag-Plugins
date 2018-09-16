using System;
using System.Collections.Generic;

using Mag.Shared;

using Decal.Adapter;

namespace MagFilter
{
	class AfterLoginCompleteMessageQueueManager
	{
		bool freshLogin;

		readonly Queue<string> loginCompleteMessageQueue = new Queue<string>();
		bool sendingLastEnter;

		const int DefaultMillisecondsToWaitAfterLoginComplete = 3000;
		int millisecondsToWaitAfterLoginComplete = DefaultMillisecondsToWaitAfterLoginComplete;

		DateTime loginCompleteTime = DateTime.MaxValue;

		public void FilterCore_ClientDispatch(object sender, NetworkMessageEventArgs e)
		{
			if (e.Message.Type == 0xF7C8) // Enter Game
				freshLogin = true;

			if (freshLogin && e.Message.Type == 0xF7B1 && Convert.ToInt32(e.Message["action"]) == 0xA1) // Character Materialize (Any time is done portalling in, login or portal)
			{
				freshLogin = false;

				if (loginCompleteMessageQueue.Count > 0)
				{
					loginCompleteTime = DateTime.Now;

					sendingLastEnter = false;
					CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);
				}
			}
		}

		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (DateTime.Now.Subtract(TimeSpan.FromMilliseconds(millisecondsToWaitAfterLoginComplete)) < loginCompleteTime)
					return;

				if (loginCompleteMessageQueue.Count == 0 && sendingLastEnter == false)
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
					PostMessageTools.SendEnter();
					PostMessageTools.SendMsg(loginCompleteMessageQueue.Dequeue());
					sendingLastEnter = true;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public void FilterCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			if (e.Text.StartsWith("/mf alcmq add "))
			{
				loginCompleteMessageQueue.Enqueue(e.Text.Substring(14, e.Text.Length - 14));
				Debug.WriteToChat("After Login Complete Message Queue added: " + e.Text);

				e.Eat = true;
			}
			else if (e.Text.StartsWith("/mf olcmq add ")) // Backwards Compatability
			{
				loginCompleteMessageQueue.Enqueue(e.Text.Substring(14, e.Text.Length - 14));
				Debug.WriteToChat("After Login Complete Message Queue added: " + e.Text);

				e.Eat = true;
			}
			else if (e.Text == "/mf alcmq clear" || e.Text == "/mf olcmq clear")
			{
				loginCompleteMessageQueue.Clear();
				Debug.WriteToChat("After Login Complete Message Queue cleared");

				e.Eat = true;
			}
			else if (e.Text.StartsWith("/mf alcmq wait set "))
			{
				millisecondsToWaitAfterLoginComplete = int.Parse(e.Text.Substring(19, e.Text.Length - 19));
				Debug.WriteToChat("After Login Complete Message Queue Wait time set: " + e.Text + "ms");

				e.Eat = true;
			}
			else if (e.Text.StartsWith("/mf olcwait set ")) // Backwards Compatability
			{
				millisecondsToWaitAfterLoginComplete = int.Parse(e.Text.Substring(16, e.Text.Length - 16));
				Debug.WriteToChat("After Login Complete Message Queue Wait time set: " + e.Text + "ms");

				e.Eat = true;
			}
			else if (e.Text == "/mf alcmq wait clear" || e.Text == "/mf olcwait clear")
			{
				millisecondsToWaitAfterLoginComplete = DefaultMillisecondsToWaitAfterLoginComplete;
				Debug.WriteToChat("After Login Complete Wait time reset to 3000ms");

				e.Eat = true;
			}
		}
	}
}