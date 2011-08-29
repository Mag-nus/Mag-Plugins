using System;

using Decal.Adapter;

namespace MagTools
{
	public class ChatFilter : IDisposable
	{
		public bool FilterAttackEvades { private get; set; }

		public bool FilterDefenseEvades { private get; set; }

		public bool FilterAttackResists { private get; set; }

		public bool FilterDefenseResists { private get; set; }

		public bool FilterSpellCasting { private get; set; }

		public bool FilterCompUsage { private get; set; }

		public bool FilterSpellExpires { private get; set; }

		public bool FilterNPKFails { private get; set; }

		public ChatFilter()
		{
			try
			{
				CoreManager.Current.ChatBoxMessage += new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private bool _disposed = false;

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
			if (!_disposed)
			{
				if (disposing)
				{
					CoreManager.Current.ChatBoxMessage -= new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}

		}

		void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
		{
			try
			{
				if (e.Eat == false && FilterAttackEvades)
				{
					// Ruschk Sadist evaded your attack.
					if (e.Text.EndsWith(" evaded your attack."))
						e.Eat = true;
				}

				if (e.Eat == false && FilterDefenseEvades)
				{
					// You evaded Ruschk Sadist!
					if (e.Text.StartsWith("You evaded "))
						e.Eat = true;
				}

				if (e.Eat == false && FilterAttackResists)
				{
					// Sentient Crystal Shard resists your spell
					if (e.Text.EndsWith(" resists your spell."))
						e.Eat = true;
				}

				if (e.Eat == false && FilterDefenseResists)
				{
					// You resist the spell cast by Sentient Crystal Shard
					if (e.Text.StartsWith("You resist the spell cast by "))
						e.Eat = true;
				}

				if (e.Eat == false && FilterSpellCasting && Util.IsSpellCastMessage(e.Text))
					e.Eat = true;

				if (e.Eat == false && FilterCompUsage)
				{
					// The spell consumed the following components:
					if (e.Text.StartsWith("The spell consumed the following components"))
						e.Eat = true;
				}

				if (e.Eat == false && FilterSpellExpires)
				{
					// The spell Defender VI on Brass Sceptre has expired.
					// Focus Self VI has expired.
					if (e.Text.EndsWith("has expired.") || e.Text.EndsWith("have expired."))
						e.Eat = true;
				}

				if (e.Eat == false && FilterNPKFails)
				{
					if (e.Text.StartsWith("You fail to affect ") && e.Text.Contains(" you are not a player killer!"))
						e.Eat = true;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
