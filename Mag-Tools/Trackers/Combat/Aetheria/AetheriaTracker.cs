using System;

using Mag.Shared;

using Decal.Adapter;

namespace MagTools.Trackers.Combat.Aetheria
{
	class AetheriaTracker
	{
		public event Action<SurgeEventArgs> SurgeEvent;

		public AetheriaTracker()
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
				if (String.IsNullOrEmpty(e.Text))
					return;

				if (Util.IsChat(e.Text))
					return;

				string sourceName = String.Empty;
				string targetName = String.Empty;

				// For messages where your aetheria casts a spell on the target, we cannot track whose aetheria
				// actually cast the spell so we do not know the source.

				// Aetheria surges on PlayerName with the power of Surge of Destruction!
				// Aetheria surges on Pyreal Target Drudge with the power of Surge of Festering!
				//
				// You cast Surge of Destruction on yourself
				// Aetheria surges on MyCharactersName with the power of Surge of Destruction!
				// Surge of Destruction has expired.
				//
				// You cast Surge of Festering on yourself
				// Aetheria surges on MyCharactersName with the power of Surge of Festering!

				// Prevent duplicate event raises for the same aetheria surge
				if (e.Text.StartsWith("You cast") || e.Text.Contains("has expired."))
					return;

				if (e.Text.StartsWith("Aetheria surges on ") && e.Text.Contains(" with the "))
				{
					targetName = e.Text.Replace("Aetheria surges on ", "");
					targetName = targetName.Substring(0, targetName.IndexOf(" with the "));

					// These surges can only be cast on yourself
					if (e.Text.Contains("Surge of Destruction"))	sourceName = targetName;
					if (e.Text.Contains("Surge of Protection"))		sourceName = targetName;
					if (e.Text.Contains("Surge of Regeneration"))	sourceName = targetName;
				}

				SurgeType surgeType = SurgeType.Unknown;

				if (e.Text.Contains("Surge of Destruction"))	surgeType = SurgeType.SurgeOfDestruction;
				if (e.Text.Contains("Surge of Protection"))		surgeType = SurgeType.SurgeOfProtection;
				if (e.Text.Contains("Surge of Regeneration"))	surgeType = SurgeType.SurgeOfRegeneration;
				if (e.Text.Contains("Surge of Affliction"))		surgeType = SurgeType.SurgeOfAffliction;
				if (e.Text.Contains("Surge of Festering"))		surgeType = SurgeType.SurgeOfFestring;

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
