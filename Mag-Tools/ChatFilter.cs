using System;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

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

		public bool FilterVendorTells { private get; set; }

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
				if (e.Eat || string.IsNullOrEmpty(e.Text))
					return;

				if (e.Eat == false && FilterAttackEvades)
				{
					// Ruschk Sadist evaded your attack.
					if (e.Text.Contains(" evaded your attack.") && !e.Text.StartsWith("You say, ") && !e.Text.Contains("says, \""))
						e.Eat = true;
				}

				if (e.Eat == false && FilterDefenseEvades)
				{
					// You evaded Ruschk Sadist!
					if (e.Text.StartsWith("You evaded ") && !e.Text.StartsWith("You say, ") && !e.Text.Contains("says, \""))
						e.Eat = true;
				}

				if (e.Eat == false && FilterAttackResists)
				{
					// Sentient Crystal Shard resists your spell
					if (e.Text.EndsWith(" resists your spell.") && !e.Text.StartsWith("You say, ") && !e.Text.Contains("says, \""))
						e.Eat = true;
				}

				if (e.Eat == false && FilterDefenseResists)
				{
					// You resist the spell cast by Sentient Crystal Shard
					if (e.Text.StartsWith("You resist the spell cast by ") && !e.Text.StartsWith("You say, ") && !e.Text.Contains("says, \""))
						e.Eat = true;

					// You have no appropriate targets equipped for Ruschk Warlord's spell.
					if (e.Text.StartsWith("You have no appropriate target") && e.Text.Contains("spell") && !e.Text.StartsWith("You say, ") && !e.Text.Contains("says, \""))
						e.Eat = true;

					// You are an invalid target for the spell of Ruschk Warlord.
					if (e.Text.StartsWith("You are an invalid target for the spell") && !e.Text.StartsWith("You say, ") && !e.Text.Contains("says, \""))
						e.Eat = true;

					// Ruschk Warlord tried to cast a spell on you, but was too far away!
					if (e.Text.Contains("tried to cast a spell on you, but was too far away!") && !e.Text.StartsWith("You say, ") && !e.Text.Contains("says, \""))
						e.Eat = true;
				}

				if (e.Eat == false && FilterSpellCasting && Util.IsSpellCastMessage(e.Text))
					e.Eat = true;

				if (e.Eat == false && FilterCompUsage)
				{
					// The spell consumed the following components:
					if (e.Text.StartsWith("The spell consumed the following components") && !e.Text.StartsWith("You say, ") && !e.Text.Contains("says, \""))
						e.Eat = true;
				}

				if (e.Eat == false && FilterSpellExpires)
				{
					// The spell Defender VI on Brass Sceptre has expired.
					// Focus Self VI has expired.
					if (e.Text.Contains("has expired.") || e.Text.Contains("have expired.") && !e.Text.StartsWith("You say, ") && !e.Text.Contains("says, \""))
						e.Eat = true;
				}

				if (e.Eat == false && FilterNPKFails)
				{
					if (e.Text.StartsWith("You fail to affect ") && e.Text.Contains(" you are not a player killer!") && !e.Text.StartsWith("You say, ") && !e.Text.Contains("says, \""))
						e.Eat = true;
				}

				if (e.Eat == false && FilterVendorTells)
				{
					if (e.Text.Contains(" tells you"))
					{
						string vendorName = e.Text.Substring(0, e.Text.IndexOf(" tells you"));

						if (!string.IsNullOrEmpty(vendorName))
						{
							WorldObjectCollection collection = CoreManager.Current.WorldFilter.GetByName(vendorName);

							if (collection.Count == 1 && collection.First.ObjectClass == ObjectClass.Vendor)
								e.Eat = true;
						}
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
