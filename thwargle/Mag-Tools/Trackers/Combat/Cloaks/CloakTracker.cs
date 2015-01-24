using System;

using Mag.Shared;

using Decal.Adapter;

namespace MagTools.Trackers.Combat.Cloaks
{
	class CloakTracker
	{
		public event Action<SurgeEventArgs> SurgeEvent;

		public CloakTracker()
		{
			try
			{
				CoreManager.Current.ChatBoxMessage += new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
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
					CoreManager.Current.ChatBoxMessage -= new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(e.Text))
					return;

				if (Util.IsChat(e.Text))
					return;

				string sourceName = String.Empty;
				string targetName = String.Empty;

				// First person intercepts
				// You cast Cloaked in Skill on yourself
				// The cloak of MyPlayerName weaves the magic of Cloaked in Skill!
				//
				// You cast Shroud of Darkness (Melee) on Invading Iron Blade Knight
				// You cast Shroud of Darkness (Magic) on Infernal Zefir
				// Your cloak reduced the damage from 162 down to 0!

				// Third person intercepts
				// The cloak of PlayerName weaves the magic of Shroud of Darkness (Melee)!
				// The cloak of PlayerName weaves the magic of Cloaked in Skill!

				if (e.Text.StartsWith("You cast ") || e.Text.StartsWith("Your cloak "))
				{
					if (e.Text.Contains("Shroud of Darkness") && !e.Text.Contains("yourself"))
					{
						sourceName = CoreManager.Current.CharacterFilter.Name;

						if (e.Text.Contains(" on "))
							targetName = e.Text.Remove(0, e.Text.IndexOf(" on ") + 4);
					}

					if (e.Text.Contains("Cloaked in Skill"))
					{
						sourceName = CoreManager.Current.CharacterFilter.Name;
						targetName = CoreManager.Current.CharacterFilter.Name;
					}

					if (e.Text.Contains("Your cloak reduced the damage"))
					{
						sourceName = CoreManager.Current.CharacterFilter.Name;
						targetName = CoreManager.Current.CharacterFilter.Name;
					}
				}

				SurgeType surgeType = SurgeType.Unknown;

				if (e.Text.Contains("Shroud of Darkness (Melee)"))		surgeType = SurgeType.ShroudOfDarknessMelee;
				if (e.Text.Contains("Shroud of Darkness (Missile)"))	surgeType = SurgeType.ShroudOfDarknessMissile;
				if (e.Text.Contains("Shroud of Darkness (Magic)"))		surgeType = SurgeType.ShroudOfDarknessMagic;

				if (e.Text.Contains("Cloaked in Skill"))				surgeType = SurgeType.CloakedInSkill;
				if (e.Text.Contains("Your cloak reduced the damage"))	surgeType = SurgeType.DamageReduction;

				if (surgeType == SurgeType.Unknown)
					return;

				SurgeEventArgs surgeEventArgs = new SurgeEventArgs(sourceName, targetName, surgeType);

				if (SurgeEvent != null)
					SurgeEvent(surgeEventArgs);
			}
			catch (Exception ex) { Debug.LogException(ex, e.Text); }
		}
	}
}
