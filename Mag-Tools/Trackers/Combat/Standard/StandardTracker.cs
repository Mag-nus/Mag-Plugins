using System;
using System.Text.RegularExpressions;

using Mag.Shared;

using Decal.Adapter;

namespace MagTools.Trackers.Combat.Standard
{
	class StandardTracker
	{
		public event Action<CombatEventArgs> CombatEvent;

		public StandardTracker()
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

				AttackType attackType = AttackType.Unknown;
				DamageElement damageElemenet = DamageElement.Unknown;

				bool isFailedAttack = false;
				bool isCriticalHit = e.Text.Contains("Critical hit!");
				bool isOverpower = e.Text.Contains("Overpower!");
				bool isKillingBlow = false;

				int damageAmount = 0;

				// You evaded Remoran Corsair!
				// Ruschk Sadist evaded your attack.
				// You resist the spell cast by Remoran Corsair
				// Sentient Crystal Shard resists your spell
				if (CombatMessages.IsFailedAttack(e.Text))
				{
					isFailedAttack = true;

					string parsedName = string.Empty;

					foreach (Regex regex in CombatMessages.FailedAttacks)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							parsedName = match.Groups["targetname"].Value;
							break;
						}
					}

					if (e.Text.StartsWith("You evaded "))
					{
						sourceName = parsedName;
						targetName = CoreManager.Current.CharacterFilter.Name;
						attackType = AttackType.MeleeMissle;
					}
					else if (e.Text.Contains(" evaded your attack"))
					{
						sourceName = CoreManager.Current.CharacterFilter.Name;
						targetName = parsedName;
						attackType = AttackType.MeleeMissle;
					}
					else if (e.Text.StartsWith("You resist the spell cast by "))
					{
						sourceName = parsedName;
						targetName = CoreManager.Current.CharacterFilter.Name;
						attackType = AttackType.Magic;
					}
					else if (e.Text.Contains(" resists your spell"))
					{
						sourceName = CoreManager.Current.CharacterFilter.Name;
						targetName = parsedName;
						attackType = AttackType.Magic;
					}
				}
				// You flatten Noble Remains's body with the force of your assault!
				// Your killing blow nearly turns Shivering Crystalline Wisp inside-out!
				// The thunder of crushing Pyre Minion is followed by the deafening silence of death!
				// Old Bones is shattered by your assault!
				else if (CombatMessages.IsKilledByMeMessage(e.Text))
				{
					isKillingBlow = true;

					sourceName = CoreManager.Current.CharacterFilter.Name;

					foreach (Regex regex in CombatMessages.TargetKilledByMe)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							targetName = match.Groups["targetname"].Value;

							break;
						}
					}
				}
				else
				{
					foreach (Regex regex in CombatMessages.MeleeMissileReceivedAttacks)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							sourceName = match.Groups["targetname"].Value;
							targetName = CoreManager.Current.CharacterFilter.Name;
							attackType = AttackType.MeleeMissle;
							damageElemenet = GetElementFromText(e.Text);
							int.TryParse(match.Groups["points"].Value, out damageAmount);
							goto Found;
						}
					}

					foreach (Regex regex in CombatMessages.MeleeMissileGivenAttacks)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							sourceName = CoreManager.Current.CharacterFilter.Name;
							targetName = match.Groups["targetname"].Value;
							attackType = AttackType.MeleeMissle;
							damageElemenet = GetElementFromText(e.Text);
							int.TryParse(match.Groups["points"].Value, out damageAmount);
							goto Found;
						}
					}

					foreach (Regex regex in CombatMessages.MagicReceivedAttacks)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							sourceName = match.Groups["targetname"].Value;
							targetName = CoreManager.Current.CharacterFilter.Name;
							attackType = AttackType.Magic;
							damageElemenet = GetElementFromText(e.Text);
							int.TryParse(match.Groups["points"].Value, out damageAmount);
							goto Found;
						}
					}

					foreach (Regex regex in CombatMessages.MagicGivenAttacks)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							sourceName = CoreManager.Current.CharacterFilter.Name;
							targetName = match.Groups["targetname"].Value;
							attackType = AttackType.Magic;
							damageElemenet = GetElementFromText(e.Text);
							int.TryParse(match.Groups["points"].Value, out damageAmount);
							goto Found;
						}
					}

					foreach (Regex regex in CombatMessages.MagicCastAttacks)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							sourceName = CoreManager.Current.CharacterFilter.Name;
							targetName = match.Groups["targetname"].Value;
							attackType = AttackType.Magic;
							damageElemenet = DamageElement.None;
							goto Found;
						}
					}

					Found: ;
				}

				if (sourceName == String.Empty && targetName == String.Empty)
					return;

				if (!isKillingBlow && attackType == AttackType.Unknown)
					Debug.WriteToChat("Unable to parse attack type from: " + e.Text);

				if (!isKillingBlow && !isFailedAttack && damageElemenet == DamageElement.Unknown)
					Debug.WriteToChat("Unable to parse damage element from: " + e.Text);

				CombatEventArgs combatEventArgs = new CombatEventArgs(sourceName, targetName, attackType, damageElemenet, isFailedAttack, isCriticalHit, isOverpower, isKillingBlow, damageAmount);

				if (CombatEvent != null)
					CombatEvent(combatEventArgs);
			}
			catch (Exception ex) { Debug.LogException(ex, e.Text); }
		}

		static DamageElement GetElementFromText(string text)
		{
			// Slash
			if (text.Contains("slash") || text.Contains(" cut") || text.Contains(" scratch") || text.Contains(" mangle"))
				return DamageElement.Slash;
			
			// Pierce
			if (text.Contains("pierc") || text.Contains(" gore") || text.Contains(" impale") || text.Contains(" nick") || text.Contains(" stab"))
				return DamageElement.Pierce;
			
			// Bludge
			if (text.Contains("bludge") || text.Contains(" smash") || text.Contains(" bash") || text.Contains(" graze") || text.Contains(" crush"))
				return DamageElement.Bludge;
			
			// Fire
			if (text.Contains("fire") || text.Contains(" burn") || text.Contains(" singe") || text.Contains(" scorch") || text.Contains(" incinerate"))
				return DamageElement.Fire;
			
			// Cold
			if (text.Contains("cold") || text.Contains(" frost") || text.Contains(" chill") || text.Contains(" numb") || text.Contains(" freeze"))
				return DamageElement.Cold;
			
			// Acid
			if (text.Contains("acid") || text.Contains(" blister") || text.Contains(" sear") || text.Contains(" corrode") || text.Contains(" dissolve"))
				return DamageElement.Acid;
			
			// Electric
			if (text.Contains("electric") || text.Contains(" lightning") || text.Contains(" jolt") || text.Contains(" shock") || text.Contains(" spark") || text.Contains(" blast"))
				return DamageElement.Electric;

			// Nether
			if (text.Contains(" eradicate") || text.Contains(" wither") || text.Contains(" scar") || text.Contains(" twist"))
				return DamageElement.Typeless;

			// Typeless/Life
			if (text.Contains(" deplete") || text.Contains(" siphon") || text.Contains(" exhaust") || text.Contains(" drain") || text.Contains(" lose ") || text.Contains(" health "))
				return DamageElement.Typeless;

			return DamageElement.Unknown;
		}
	}
}
