using System;
using System.Collections.Generic;

using Mag.Shared;

using Decal.Adapter;

namespace MagFilter
{
	class LoginMessageQueueManager
	{
		readonly Queue<string> loginMessageQueue = new Queue<string>();
		bool sendingLastEnter;

		public void FilterCore_ClientDispatch(object sender, NetworkMessageEventArgs e)
		{
			if (loginMessageQueue.Count > 0 && e.Message.Type == 0xF7B1 && Convert.ToInt32(e.Message["action"]) == 0xA1)
			{
				sendingLastEnter = false;
				CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);
			}
		}

		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (loginMessageQueue.Count == 0 && sendingLastEnter == false)
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
					PostMessageTools.SendMsg(loginMessageQueue.Dequeue());
					sendingLastEnter = true;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public void FilterCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			if (e.Text.StartsWith("/mf lmq add "))
			{
				loginMessageQueue.Enqueue(e.Text.Substring(12, e.Text.Length - 12));
				Debug.WriteToChat("Login Message Queue added: " + e.Text);
			}
			else if (e.Text == "/mf lmq clear")
			{
				loginMessageQueue.Clear();
				Debug.WriteToChat("Login Message Queue cleared");
			}
		}
	}
}
