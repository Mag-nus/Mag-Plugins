using System;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Trackers.Combat
{
	public class CombatTracker : IDisposable
	{
		public delegate void CombatEventHandler(object sender, CombatEventArgs e);
		public event CombatEventHandler CombatEvent;  

		public CombatTracker()
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
				if (CombatEvent == null)
					return;

				if (e.Text.Contains(" says, ") || e.Text.Contains(" tells you"))
					return;

				string monsterName = null;

				AttackDirection attackDirection = AttackDirection.Unknown;
				AttackType attackType = AttackType.Unknown;
				DamageElement damageElemenet = DamageElement.Unknown;

				bool isFailedAttack = false;
				bool isKillingBlow = false;
				bool isCriticalHit = e.Text.Contains("Critical hit!");

				int damageAmount = 0;

				// A bow attack that doesn't hit the target
				// Your missile attack hit the environment.

				// An attack on a player that would have been a crit without the protection aug.
				// Viamontian Mercenary scratches your foot for 3 points of slashing damage! Your Critical Protection augmentation allows you to avoid a critical hit!

				// Parsing the monster name from the string can be a bit tricky.
				// First we remove common phrases that we know are not the monster name.
				// The first chunk of capitalized words is the monster name.
				string parsedString = e.Text.Replace("Critical hit!", "").Replace("Your ", "").Replace("You ", "").Replace("The ", "").Replace("Critical Protection", "").Replace("Magical energies", "").Replace("Electricity tears", "").Replace("Blistered by", "").Replace("to a", "");
				string[] words = parsedString.Trim().Split(' ');
				foreach (string word in words)
				{
					// This is broken for monsters that names start with The
					if (char.IsUpper(word[0]) || (monsterName != null && (word == "of" || word == "the" || word == "a" || word == "to" || word == "in")))
						monsterName += monsterName == null ? word : " " + word;
					else if (monsterName != null)
						break;
				}
				if (monsterName != null)
					monsterName = monsterName.Replace("!", "").Replace("'s", "");

				// Ruschk Sadist evaded your attack.
				if (e.Text.Contains(" evaded your attack"))
				{
					attackDirection = AttackDirection.PlayerInitiated;
					// This is the message for both melee and missile combat failures.
					attackType = AttackType.MeleeMissle;

					isFailedAttack = true;
				}
				// Sentient Crystal Shard resists your spell
				else if (e.Text.Contains(" resists your spell"))
				{
					attackDirection = AttackDirection.PlayerInitiated;
					attackType = AttackType.Magic;

					isFailedAttack = true;
				}
				// You resist the spell cast by Remoran Corsair
				else if (e.Text.StartsWith("You resist the spell cast by "))
				{
					attackDirection = AttackDirection.PlayerReceived;
					attackType = AttackType.Magic;

					isFailedAttack = true;
				}
				// You evaded Remoran Corsair!
				else if (e.Text.StartsWith("You evaded "))
				{
					attackDirection = AttackDirection.PlayerReceived;
					// This is the message for both melee and missile combat failures.
					attackType = AttackType.MeleeMissle;

					isFailedAttack = true;
				}
				else if (MagTools.Util.IsMonsterKilledByYouDeathMessage(e.Text))
				{
					attackDirection = AttackDirection.PlayerInitiated;

					isKillingBlow = true;
				}
				else
				{
					if (!e.Text.Contains(" point"))
						return;

					if (e.Text.StartsWith("You cast") ||
						e.Text.StartsWith("You suffer") ||
						e.Text.Contains(" restore") ||
						e.Text.Contains(" yourself ") ||
						e.Text.Contains(" Vassals ") ||
						e.Text.Contains("Mana Stone") ||
						e.Text.Contains(" your stamina") ||
						e.Text.Contains(" of stamina") ||
						e.Text.Contains(" your mana") ||
						e.Text.Contains(" of mana") ||
						e.Text.Contains(" and dispels") ||
						e.Text.Contains("periodic healing") ||
						e.Text.Contains("Spells points") ||
						e.Text.Contains("points and "))
						return;

					// Determine attackDirection
					if ((e.Text.StartsWith("You ") || e.Text.StartsWith("Critical hit! You ") || e.Text.StartsWith("Critical hit!  You ")) &&
							!e.Text.StartsWith("You lose") && !e.Text.StartsWith("Critical hit! You lose") && !e.Text.StartsWith("Critical hit!  You lose"))
					{
						// Damage Given
						attackDirection = AttackDirection.PlayerInitiated;
					}
					else
					{
						// Damage Received
						attackDirection = AttackDirection.PlayerReceived;
					}

					// Determine attackType and damageElemenet

					// Magical energies lose 1 point of health due to Sentient Crystal Shard casting Vitality Siphon
					// You lose 39 points of health due to Infernal Zefir casting Drain Health Other V on you
					// Crystal Minion casts Harm Other VI and drains 39 points ...
					if (((e.Text.Contains(" lose ") || e.Text.Contains(" and drains ")) && e.Text.Contains("health")) || e.Text.Contains(" exhausts "))
					{
						attackType = AttackType.Magic;
						damageElemenet = DamageElement.Typeless;
					}
					// Critical hit! Virindi Executor scratches your upper leg for 3 points of slashing damage!
					// Crystal Shard Sentinel scorches you for 47 points with Flame Arc VII.
					// Ruschk Sadist numbs your lower leg for 2 points of cold damage!
					else
					{
						if (e.Text.Contains(" with "))
							attackType = AttackType.Magic;
						else
							attackType = AttackType.MeleeMissle;

						// Slash
						if (e.Text.Contains("slash") || e.Text.Contains(" cut") || e.Text.Contains(" scratch") || e.Text.Contains(" mangle"))
							damageElemenet = DamageElement.Slash;
						// Pierce
						else if (e.Text.Contains("pierc") || e.Text.Contains(" gore") || e.Text.Contains(" impale") || e.Text.Contains(" nick") || e.Text.Contains(" stab"))
							damageElemenet = DamageElement.Pierce;
						// Bludge
						else if (e.Text.Contains("bludge") || e.Text.Contains(" smash") || e.Text.Contains(" bash") || e.Text.Contains(" graze") || e.Text.Contains(" crush"))
							damageElemenet = DamageElement.Bludge;
						// Fire
						else if (e.Text.Contains("fire") || e.Text.Contains(" burn") || e.Text.Contains(" singe") || e.Text.Contains(" scorch") || e.Text.Contains(" incinerate"))
							damageElemenet = DamageElement.Fire;
						// Cold
						else if (e.Text.Contains("cold") || e.Text.Contains(" frost") || e.Text.Contains(" chill") || e.Text.Contains(" numb"))
							damageElemenet = DamageElement.Cold;
						// Acid
						else if (e.Text.Contains("acid") || e.Text.Contains(" blister") || e.Text.Contains(" sear") || e.Text.Contains(" corrode") || e.Text.Contains(" dissolve"))
							damageElemenet = DamageElement.Acid;
						// Electric
						else if (e.Text.Contains("electric") || e.Text.Contains(" lightning") || e.Text.Contains(" jolt") || e.Text.Contains(" shock") || e.Text.Contains(" spark"))
							damageElemenet = DamageElement.Electric;
					}

					if (damageElemenet == DamageElement.Unknown)
						Debug.WriteToChat("Unable to parse element from damage message: " + e.Text);

					// Calculate the DamageAmount
					string damageString = e.Text.Substring(0, e.Text.IndexOf(" point"));
					damageString = damageString.Substring(damageString.LastIndexOf(' ') + 1, damageString.Length - damageString.LastIndexOf(' ') - 1);

					int.TryParse(damageString, out damageAmount);
				}

				CombatEventArgs combatEventArgs = new CombatEventArgs(monsterName, attackDirection, attackType, damageElemenet, isFailedAttack, isKillingBlow, isCriticalHit, damageAmount);

				if (CombatEvent != null)
					CombatEvent(this, combatEventArgs);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
