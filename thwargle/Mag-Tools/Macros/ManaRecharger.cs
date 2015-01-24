using System;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	sealed class ManaRecharger
	{
		private static readonly ManaRecharger instance = new ManaRecharger();

		private ManaRecharger() { }

		public static ManaRecharger Instance
		{
			get
			{
				return instance;
			}
		}

		bool started;

		DateTime utcTimeStarted = DateTime.MinValue;

		public void Start()
		{
			if (started)
				return;

			CoreManager.Current.CharacterFilter.Logoff += new EventHandler<LogoffEventArgs>(CharacterFilter_Logoff);
			CoreManager.Current.ChatBoxMessage += new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
			CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);

			utcTimeStarted = DateTime.UtcNow;

			started = true;
		}

		public void Stop()
		{
			if (!started)
				return;

			CoreManager.Current.CharacterFilter.Logoff -= new EventHandler<LogoffEventArgs>(CharacterFilter_Logoff);
			CoreManager.Current.ChatBoxMessage -= new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
			CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);

			started = false;
		}

		void CharacterFilter_Logoff(object sender, LogoffEventArgs e)
		{
			try
			{
				Stop();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
		{
			try
			{
				if (e.Text.Contains("The Mana Stone gives"))
					Stop();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		DateTime lastThought = DateTime.MinValue;

		void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (DateTime.Now - lastThought < TimeSpan.FromMilliseconds(100))
					return;

				lastThought = DateTime.Now;

				Think();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		int lastChargeId;
		DateTime lastChargeAttempTime = DateTime.MinValue;

		void Think()
		{
			// See if we can find a mana stone that has been identified AFTER startTime and has mana in it.
			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (obj.ObjectClass != ObjectClass.ManaStone || !obj.Name.Contains("Mana Stone") || !obj.HasIdData)
					continue;

				DateTime utcLastIdTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(obj.LastIdTime);

				if (utcLastIdTime < utcTimeStarted)
					continue;

				if (obj.Values(LongValueKey.CurrentMana) > 0)
				{
					// If we're trying to use a different stone than our last attempt, make sure we've waited 5 seconds for the stone to actually work and trigger Stop()
					if (lastChargeId != obj.Id && DateTime.Now - lastChargeAttempTime < TimeSpan.FromSeconds(5))
						return;

					lastChargeId = obj.Id;
					lastChargeAttempTime = DateTime.Now;

					CoreManager.Current.Actions.ApplyItem(obj.Id, CoreManager.Current.CharacterFilter.Id);

					return;
				}
			}

			// We were unable to find an id'd stone with mana, lets see if we can find a mana stone that needs a new id
			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (obj.ObjectClass != ObjectClass.ManaStone || !obj.Name.Contains("Mana Stone"))
					continue;

				if (obj.HasIdData)
				{
					DateTime utcLastIdTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(obj.LastIdTime);

					if (utcLastIdTime >= utcTimeStarted)
						continue;
				}

				CoreManager.Current.Actions.RequestId(obj.Id);

				return;
			}

			// At this point we haven't found any mana stones with mana in them, lets use a mana charge
			WorldObject smallestCharge = null;

			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetInventory())
			{
				if (obj.ObjectClass != ObjectClass.ManaStone || !obj.Name.Contains("Mana Charge"))
					continue;

				if (smallestCharge == null || obj.Values(LongValueKey.CurrentMana) < smallestCharge.Values(LongValueKey.CurrentMana))
					smallestCharge = obj;
			}

			if (smallestCharge != null)
			{
				// If we're trying to use a different stone than our last attempt, make sure we've waited 5 seconds for the stone to actually work and trigger Stop()
				if (lastChargeId != smallestCharge.Id && DateTime.Now - lastChargeAttempTime < TimeSpan.FromSeconds(5))
					return;

				lastChargeId = smallestCharge.Id;
				lastChargeAttempTime = DateTime.Now;

				CoreManager.Current.Actions.ApplyItem(smallestCharge.Id, CoreManager.Current.CharacterFilter.Id);
			}
		}
	}
}
