using System;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools
{
	class ChatFilter : IDisposable
	{
		readonly PluginHost host;

		public ChatFilter(PluginHost host)
		{
			try
			{
				this.host = host;

				CoreManager.Current.ChatBoxMessage += new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);

				host.Underlying.Hooks.StatusTextIntercept += new Decal.Interop.Core.IACHooksEvents_StatusTextInterceptEventHandler(Hooks_StatusTextIntercept);
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

					host.Underlying.Hooks.StatusTextIntercept -= new Decal.Interop.Core.IACHooksEvents_StatusTextInterceptEventHandler(Hooks_StatusTextIntercept);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}

		}

		void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
		{
			try
			{
				if (e.Eat || string.IsNullOrEmpty(e.Text))
					return;

				bool isChat = Util.IsChat(e.Text);

				if (e.Eat == false && Settings.SettingsManager.Filters.AttackEvades.Value)
				{
					// Ruschk Sadist evaded your attack.
					if (!isChat && e.Text.Contains(" evaded your attack."))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.DefenseEvades.Value)
				{
					// You evaded Ruschk Sadist!
					if (!isChat && e.Text.StartsWith("You evaded "))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.AttackResists.Value)
				{
					// Sentient Crystal Shard resists your spell
					// Invading Silver Scope Knight resists your spell
					if (!isChat && e.Text.Contains(" resists your spell"))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.DefenseResists.Value)
				{
					// You resist the spell cast by Sentient Crystal Shard
					if (!isChat && e.Text.StartsWith("You resist the spell cast by "))
						e.Eat = true;

					// You have no appropriate targets equipped for Ruschk Warlord's spell.
					if (!isChat && e.Text.StartsWith("You have no appropriate target") && e.Text.Contains("spell"))
						e.Eat = true;

					// You are an invalid target for the spell of Ruschk Warlord.
					if (!isChat && e.Text.StartsWith("You are an invalid target for the spell"))
						e.Eat = true;

					// Ruschk Warlord tried to cast a spell on you, but was too far away!
					if (!isChat && e.Text.Contains("tried to cast a spell on you, but was too far away!"))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.NPKFails.Value)
				{
					if (!isChat && e.Text.StartsWith("You fail to affect ") && e.Text.Contains(" you are not a player killer!"))
						e.Eat = true;

					if (!isChat && e.Text.Contains("fails to affect you") && e.Text.Contains(" is not a player killer!"))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.DirtyFighting.Value)
				{
					// Dirty Fighting! [player] delivers a Traumatic Assault to [mob]!
					// Dirty Fighting! [player] delivers a Bleeding Assault to [mob]!
					// Dirty Fighting! [player] delivers a Unbalancing Assault to [mob]!
					// Dirty Fighting! [player] delivers a Blinding Assault to [mob]!
					if (!isChat && e.Text.StartsWith("Dirty Fighting! ") && e.Text.Contains(" delivers a "))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.MonsterDeaths.Value)
				{
					if (Trackers.Combat.Standard.CombatMessages.IsKilledByMeMessage(e.Text))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.SpellCastingMine.Value)
				{
					// You say, "Zojak 
					if (Util.IsSpellCastingMessage(e.Text, true, false))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.SpellCastingOthers.Value)
				{
					// Fat Guy In A Little Coat says, "Zojak
					if (Util.IsSpellCastingMessage(e.Text, false))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.SpellCastFizzles.Value)
				{
					// Your spell fizzled.
					if (!isChat && e.Text.StartsWith("Your spell fizzled."))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.CompUsage.Value)
				{
					// The spell consumed the following components:
					if (!isChat && e.Text.StartsWith("The spell consumed the following components"))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.SpellExpires.Value)
				{
					// Don't filter rare expires
					if (!isChat && !e.Text.Contains("Brilliance") && !e.Text.Contains("Prodigal") && !e.Text.Contains("Spectral"))
					{
						// The spell Defender VI on Brass Sceptre has expired.
						// Focus Self VI has expired.
						if (e.Text.Contains("has expired.") || e.Text.Contains("have expired."))
							e.Eat = true;
					}
				}


				if (e.Eat == false && Settings.SettingsManager.Filters.HealingKitSuccess.Value)
				{
					// You heal yourself for 88 Health points. Your treated Healing Kit has 16  uses left.
					// You expertly heal yourself for 123 Health points. Your Treated Healing Kit has 41 uses left.
					if (!isChat && e.Text.StartsWith("You ") && e.Text.Contains(" heal yourself for "))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.HealingKitFail.Value)
				{
					// You fail to heal yourself. Your Treated Healing Kit has 18 uses left.
					if (!isChat && e.Text.StartsWith("You fail to heal yourself. "))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.Salvaging.Value)
				{
					// You obtain 9 granite (ws 8.00) using your knowledge of Salvaging
					if (!isChat && e.Text.StartsWith("You obtain ") && e.Text.Contains(" using your knowledge of "))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.SalvagingFails.Value)
				{
					// Salvaging Failed!
					if (!isChat && e.Text.StartsWith("Salvaging Failed!"))
						e.Eat = true;

					//  The following were not suitable for salvaging: Salvaged Sunstone (79), Salvaged Sunstone (7).
					if (!isChat && e.Text.Contains("The following were not suitable for salvaging: "))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.AuraOfCraftman.Value)
				{
					// Your Aura of the Craftman augmentation increased your skill by 5!
					if (!isChat && e.Text.StartsWith("Your Aura of the Craftman augmentation increased your skill by 5!"))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.ManaStoneUsage.Value)
				{
					// The Mana Stone gives 6,127 points of mana to the following items: 
					if (!isChat && e.Text.StartsWith("The Mana Stone gives "))
						e.Eat = true;
					// You need 6,833 more mana to fully charge your items.
					if (!isChat && e.Text.StartsWith("You need ") && e.Text.Trim().EndsWith(" more mana to fully charge your items."))
						e.Eat = true;
					// The Mana Stone drains 3,153 points of mana from the Fez.
					if (!isChat && e.Text.StartsWith("The Mana Stone drains "))
						e.Eat = true;
					// The Fez is destroyed.
					if (!isChat && e.Text.StartsWith("The ") && e.Text.Trim().EndsWith(" is destroyed."))
						e.Eat = true;
				}


				if (e.Eat == false && Settings.SettingsManager.Filters.TradeBuffBotSpam.Value)
				{
					if (Util.IsChat(e.Text, Util.ChatFlags.PlayerSaysLocal) && (e.Text.Trim().EndsWith("-t-\"") || e.Text.Trim().EndsWith("-b-\"")))
						e.Eat = true;

					// Trade bot emotes
					if (!isChat && (e.Text.Trim().EndsWith("-t-") || e.Text.Trim().EndsWith("-b-")))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.FailedAssess.Value)
				{
					// Someone tried and failed to assess you!
					if (!isChat && e.Text.Trim().EndsWith("tried and failed to assess you!"))
						e.Eat = true;
				}


				if (e.Eat == false && Settings.SettingsManager.Filters.KillTaskComplete.Value)
				{
					// You have killed 50 Drudge Raveners! Your task is complete!
					if (!isChat && e.Text.StartsWith("You have killed ") && e.Text.Trim().EndsWith("Your task is complete!"))
						e.Eat = true;
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.VendorTells.Value)
				{
					if (Util.IsChat(e.Text, Util.ChatFlags.NpcTellsYou))
					{
						string sourceName = Util.GetSourceOfChat(e.Text);

						if (!string.IsNullOrEmpty(sourceName))
						{
							WorldObjectCollection collection = CoreManager.Current.WorldFilter.GetByName(sourceName);

							if (collection.Count >= 1 && collection.First.ObjectClass == ObjectClass.Vendor)
								e.Eat = true;
						}
					}
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.MonsterTell.Value)
				{
					if (Util.IsChat(e.Text, Util.ChatFlags.NpcTellsYou))
					{
						string sourceName = Util.GetSourceOfChat(e.Text);

						if (!string.IsNullOrEmpty(sourceName))
						{
							WorldObjectCollection collection = CoreManager.Current.WorldFilter.GetByName(sourceName);

							if (collection.Count >= 1 && collection.First.ObjectClass == ObjectClass.Monster)
								e.Eat = true;
						}
					}
				}

				if (e.Eat == false && Settings.SettingsManager.Filters.NpcChatter.Value)
				{
					if (Util.IsChat(e.Text, Util.ChatFlags.NpcSays))
					{
						string sourceName = Util.GetSourceOfChat(e.Text);

						if (!string.IsNullOrEmpty(sourceName))
						{
							WorldObjectCollection collection = CoreManager.Current.WorldFilter.GetByName(sourceName);

							if (collection.Count >= 1 && collection.First.ObjectClass == ObjectClass.Npc)
								e.Eat = true;
						}
					}
				}

				if (e.Eat == false && (Settings.SettingsManager.Filters.MasterArbitratorSpam.Value || Settings.SettingsManager.Filters.AllMasterArbitratorChat.Value))
				{
					if (isChat)
					{
						if (Util.GetSourceOfChat(e.Text) == "Master Arbitrator")
						{
							/*
							(We don't filter these)
							Master Arbitrator tells you, "If you wish to fight as a gladiator in the Arena I will require you to purchase a ticket from the Ticket Vendors over there. We do need to keep the place running don't we?"
							Master Arbitrator tells you, "Also, I warn you now. Prepare your fellowship ahead of time. Once you pay me you cannot change your registered group and only that group will be allowed into the Arena I assign you. After you enter the Arena you must wait one hour before recieving your reward. Our gladiators need time to rest between fights."

							(We don't filter these)
							20:57:15 Master Arbitrator tells you, "Your fellowship's Arena battles still continue. I cannot reward anyone in your fellowship while they still have time left in the Colosseum. (4s)"
							20:57:17 Master Arbitrator tells you, "Your fellowship's Arena battles still continue. I cannot reward anyone in your fellowship while they still have time left in the Colosseum. (2s)"
							20:57:20 Master Arbitrator tells you, "You fought in the Colosseum's Arenas too recently. I cannot reward you for 4s."

							(In this set of messages, we don't filter the Well done!)
							20:57:24 Master Arbitrator tells you, "Well done! I was greatly impressed with your performance in the arenas."
							20:57:24 Master Arbitrator tells you, "You shall be known to all as a "Colosseum Champion"!"
							20:57:24 Master Arbitrator tells you, "Take this knowledge and this Colosseum Vault Key as a reward for your accomplishments Champion."
							20:57:24 Master Arbitrator tells you, "Use the the key to open the Colosseum Vault and claim some of our treasury for yourself."

							(In this set of messages, we don't filter the Good Luck!)
							21:01:14 Your fellowship is now locked.  You may not recruit new members.  If you leave the fellowship, you have 15 minutes to be recruited back into the fellowship.
							21:01:16 [Fellowship] Master Arbitrator says, "Your fellowship will be battling in Arena One."
							21:01:17 [Fellowship] Master Arbitrator says, "Use one of the two portals to enter your Arena. If every member of your group is powerful enough you may skip the lower battles by using the Advanced Colosseum Arena, but any one member of your fellow may be restricted from using that portal so be careful or you may be split up."
							21:01:17 [Fellowship] Master Arbitrator says, "Don't forget that you must wait one full hour after the time you enter the colosseum before I will reward you for your achievements in the Arenas."
							21:01:17 [Fellowship] Master Arbitrator says, "Good Luck!"
							*/

							if (Settings.SettingsManager.Filters.MasterArbitratorSpam.Value)
							{
								if (Util.IsChat(e.Text, Util.ChatFlags.NpcTellsYou))
								{
									if (e.Text.Contains("\"You shall be known") || e.Text.Contains("\"Take this knowledge") || e.Text.Contains("\"Use the the key"))
										e.Eat = true;
								}
								else if (Util.IsChat(e.Text, Util.ChatFlags.PlayerSaysChannel))
								{
									if (e.Text.Contains("\"Your fellowship") || e.Text.Contains("\"Use one of the") || e.Text.Contains("\"Don't forget"))
										e.Eat = true;
								}
							}

							if (Settings.SettingsManager.Filters.AllMasterArbitratorChat.Value)
								e.Eat = true;
						}
					}

					if (Settings.SettingsManager.Filters.AllMasterArbitratorChat.Value && e.Text.StartsWith("Your fellowship is now locked.") && e.Text.Contains("you have 15 minutes to be recruited"))
						e.Eat = true;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Hooks_StatusTextIntercept(string bstrText, ref bool bEat)
		{
			try
			{
				// You're too busy!
				if (Settings.SettingsManager.Filters.StatusTextYoureTooBusy.Value && !bEat && bstrText == "You're too busy!")
					bEat = true;

				// Casting .....
				if (Settings.SettingsManager.Filters.StatusTextCasting.Value && !bEat && bstrText.StartsWith("Casting "))
					bEat = true;

				if (Settings.SettingsManager.Filters.StatusTextAll.Value)
					bEat = true;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
