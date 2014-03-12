using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace MagTools.Trackers.Combat.Standard
{
	static class CombatMessages
	{
		public static Collection<Regex> FailedAttacks = new Collection<Regex>();

		public static Collection<Regex> MeleeMissileReceivedAttacks = new Collection<Regex>();
		public static Collection<Regex> MeleeMissileGivenAttacks = new Collection<Regex>();
		public static Collection<Regex> MagicReceivedAttacks = new Collection<Regex>();
		public static Collection<Regex> MagicGivenAttacks = new Collection<Regex>();

		public static Collection<Regex> MagicCastAttacks = new Collection<Regex>();

		public static Collection<Regex> TargetKilledByMe = new Collection<Regex>();

		static CombatMessages()
		{
			// You evaded Remoran Corsair!
			FailedAttacks.Add(new Regex("^You evaded (?<targetname>.+)!$"));
			// Ruschk Sadist evaded your attack.
			FailedAttacks.Add(new Regex("^(?<targetname>.+) evaded your attack.$"));
			// You resist the spell cast by Remoran Corsair
			FailedAttacks.Add(new Regex("^You resist the spell cast by (?<targetname>.+)$"));
			// Sentient Crystal Shard resists your spell
			FailedAttacks.Add(new Regex("^(?<targetname>.+) resists your spell$"));


			// Melee/Missile received
			// Critical hit! Overpower! Lugian Launcher smashes your lower leg for 108 points of bludgeoning damage!
			MeleeMissileReceivedAttacks.Add(new Regex("^Critical hit! Overpower! (?<targetname>.+) [\\w]+ your .+ for (?<points>.*) point.* of .+ damage.*$"));
			// Critical hit! Virindi Executor scratches your upper leg for 3 points of slashing damage!
			MeleeMissileReceivedAttacks.Add(new Regex("^Critical hit! (?<targetname>.+) [\\w]+ your .+ for (?<points>.*) point.* of .+ damage.*$"));
			// Overpower! Lugian Launcher bashes your foot for 43 points of bludgeoning damage!
			MeleeMissileReceivedAttacks.Add(new Regex("^Overpower! (?<targetname>.+) [\\w]+ your .+ for (?<points>.+) point.* of .+ damage.*$"));
			// Ruschk Sadist numbs your lower leg for 2 points of cold damage!
			// Annihilator grazes your upper arm for 3 points of bludgeoning damage!
			// Viamontian Mercenary scratches your foot for 3 points of slashing damage! Your Critical Protection augmentation allows you to avoid a critical hit!
			MeleeMissileReceivedAttacks.Add(new Regex("^(?<targetname>.+) [\\w]+ your .+ for (?<points>.+) point.* of .+ damage.*$"));
			
			// Melee/Missile given
			// Critical hit!  You scorch Annihilator for 685 points of fire damage!
			MeleeMissileGivenAttacks.Add(new Regex("^Critical hit!  You [\\w]+ (?<targetname>.*) for (?<points>.+) point.* of .+ damage.*$"));
			// You scorch Annihilator for 805 points of fire damage!
			MeleeMissileGivenAttacks.Add(new Regex("^You [\\w]+ (?<targetname>.*) for (?<points>.+) point.* of .+ damage.*$"));

			// Magic received
			// Critical hit! Overpower! Virindi Rival shocks you for 96 points with Incantation of Lightning Arc.
			MagicReceivedAttacks.Add(new Regex("^Critical hit! Overpower! (?<targetname>.+) [\\w]+ you for (?<points>.+) point.* with .+$"));
			// Critical hit! Mag-salvager smashes you for 147 points with Incantation of Shock Wave Streak.
			// Critical hit! Mag-salvager exhausts you for 39 points with Martyr's Hecatomb VII.
			MagicReceivedAttacks.Add(new Regex("^Critical hit! (?<targetname>.+) [\\w]+ you for (?<points>.+) point.* with .+$"));
			// Overpower! Virindi Rival shocks you for 96 points with Incantation of Lightning Arc.
			MagicReceivedAttacks.Add(new Regex("^Overpower! (?<targetname>.+) [\\w]+ you for (?<points>.+) point.* with .+$"));
			// Crystal Shard Sentinel scorches you for 47 points with Flame Arc VII.
			// Tendril of T'thuun depletes you for 188 points with Martyr's Hecatomb V.
			// Mag-salvager siphons you for 121 points with Martyr's Hecatomb VII.
			MagicReceivedAttacks.Add(new Regex("^(?<targetname>.+) [\\w]+ you for (?<points>.+) point.* with .+$"));
			// Magical energies lose 1 point of health due to Sentient Crystal Shard casting Vitality Siphon
			MagicReceivedAttacks.Add(new Regex("^Magical energies lose (?<points>.+) point.* of health due to (?<targetname>.+) casting .+$"));
			// You lose 39 points of health due to Infernal Zefir casting Drain Health Other V on you
			MagicReceivedAttacks.Add(new Regex("^You lose (?<points>.+) point.* of health due to (?<targetname>.+) casting .+$"));
			// Crystal Minion casts Harm Other VI and drains 39 points ...
			MagicReceivedAttacks.Add(new Regex("^(?<targetname>.+) casts .+ and drains (?<points>.+) point.* .+$"));

			// Magic given
			// Critical hit! You smash Mag-lite for 147 points with Incantation of Shock Wave Streak.
			// Critical hit! You exhaust Mag-lite for 39 points with Martyr's Hecatomb VII.
			// Critical hit! You eradicate Invading Bronze Gauntlet Knight for 743 points with Incantation of Nether Arc.
			MagicGivenAttacks.Add(new Regex("^Critical hit! You [\\w]+ (?<targetname>.+) for (?<points>.+) point.* with .+$"));
			// You nick Tremendous Monouga for 595 points with Incantation of Force Bolt.
			// You wither Invading Silver Scope Phalanx for 253 points with Incantation of Nether Arc.
			// You siphon Mag-lite for 121 points with Martyr's Hecatomb VII.
			// You exhaust Mag-lite for 42 points with Martyr's Hecatomb VII.
			// You drain Mag-lite for 34 points with Martyr's Hecatomb VII.
			MagicGivenAttacks.Add(new Regex("^You [\\w]+ (?<targetname>.+) for (?<points>.+) point.* with .+$"));


			// You cast Gossamer Flesh on Chicken
			MagicCastAttacks.Add(new Regex("^You cast Gossamer Flesh on (?<targetname>((?!, ).)+)$"));
			// You cast Gossamer Flesh on Chicken, refreshing Gossamer Flesh
			MagicCastAttacks.Add(new Regex("^You cast Gossamer Flesh on (?<targetname>.+), .+$"));


			// You flatten Noble Remains's body with the force of your assault!
			TargetKilledByMe.Add(new Regex("^You flatten (?<targetname>.+)'s body with the force of your assault!$"));
			// You bring Wight Blade Sorcerer to a fiery end! (Fire)
			TargetKilledByMe.Add(new Regex("^You bring (?<targetname>.+) to a fiery end!$"));
			// You beat Door to a lifeless pulp!
			TargetKilledByMe.Add(new Regex("^You beat (?<targetname>.+) to a lifeless pulp!$"));
			// You smite Famished Eater mightily!
			TargetKilledByMe.Add(new Regex("^You smite (?<targetname>.+) mightily!$"));
			// You obliterate Drudge Skulker!
			TargetKilledByMe.Add(new Regex("^You obliterate (?<targetname>.+)!$"));
			// You run Insatiable Eater through! (Pierce)
			TargetKilledByMe.Add(new Regex("^You run (?<targetname>.+) through!$"));
			// You reduce Insatiable Eater to a sizzling, oozing mass! (Acid)
			TargetKilledByMe.Add(new Regex("^You reduce (?<targetname>.+) to a sizzling, oozing mass!$"));
			// You knock Blockade Guard into next Morningthaw!
			TargetKilledByMe.Add(new Regex("^You knock (?<targetname>.+) into next Morningthaw!$"));
			// You split Blockade Guard apart! (Slash)
			TargetKilledByMe.Add(new Regex("^You split (?<targetname>.+) apart!$"));
			// You cleave Blockade Guard in twain! (Slash)
			TargetKilledByMe.Add(new Regex("^You cleave (?<targetname>.+) in twain!$"));
			// You slay Blockade Guard viciously enough to impart death several times over! (Slash)
			TargetKilledByMe.Add(new Regex("^You slay (?<targetname>.+) viciously enough to impart death several times over!$"));
			// You reduce ____ to a drained, twisted corpse! (Nether)
			TargetKilledByMe.Add(new Regex("^You reduce (?<targetname>.+) to a drained, twisted corpse!$"));

			// Your killing blow nearly turns Shivering Crystalline Wisp inside-out!
			TargetKilledByMe.Add(new Regex("^Your killing blow nearly turns (?<targetname>.+) inside-out!$"));
			// Your attack stops Ruschk Draktehn cold! (Frost)
			TargetKilledByMe.Add(new Regex("^Your attack stops (?<targetname>.+) cold!$"));
			// Your lightning coruscates over Insatiable Eater's mortal remains! (Lightning)
			TargetKilledByMe.Add(new Regex("^Your lightning coruscates over (?<targetname>.+)'s mortal remains!$"));
			// Your assault sends Ardent Moar to an icy death!
			TargetKilledByMe.Add(new Regex("^Your assault sends (?<targetname>.+) to an icy death!$"));
			// You killed SomeGuy!
			TargetKilledByMe.Add(new Regex("^You killed (?<targetname>.+)!$"));

			// The thunder of crushing Pyre Minion is followed by the deafening silence of death!
			TargetKilledByMe.Add(new Regex("^The thunder of crushing (?<targetname>.+) is followed by the deafening silence of death!$"));
			// The deadly force of your attack is so strong that Young Banderling's ancestors feel it!
			TargetKilledByMe.Add(new Regex("^The deadly force of your attack is so strong that (?<targetname>.+)'s ancestors feel it!$"));
			// The force of your killing blow violently dislocated Insatiable Eaters jaw!

			// Wight Blade Sorcerer's seared corpse smolders before you! (Fire)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+)'s seared corpse smolders before you!$"));
			// Wight is reduced to cinders! (Fire)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+) is reduced to cinders!$"));
			// Old Bones is shattered by your assault!
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+) is shattered by your assault!$"));
			// Gnawer Shreth catches your attack, with dire consequences!
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+) catches your attack, with dire consequences!$"));
			// Gnawer Shreth is utterly destroyed by your attack!
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+) is utterly destroyed by your attack!$"));
			// Ruschk Laktar suffers a frozen fate! (Frost)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+) suffers a frozen fate!$"));
			// Insatiable Eater's perforated corpse falls before you! (Pierce)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+)'s perforated corpse falls before you!$"));
			// Insatiable Eater is fatally punctured! (Pierce)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+) is fatally punctured!$"));
			// Insatiable Eater's death is preceded by a sharp, stabbing pain! (Pierce)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+)'s death is preceded by a sharp, stabbing pain!$"));
			// Insatiable Eater is torn to ribbons by your assault! (Slash)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+) is torn to ribbons by your assault!$"));
			// Insatiable Eater is liquified by your attack! (Acid)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+) is liquified by your attack!$"));
			// Insatiable Eater's last strength dissolves before you! (Acid)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+)'s last strength dissolves before you!$"));
			// Electricity tears Insatiable Eater apart! (Lightning)
			TargetKilledByMe.Add(new Regex("^Electricity tears (?<targetname>.+) apart!$"));
			// Blistered by lightning, Insatiable Eater falls! (Lightning)
			TargetKilledByMe.Add(new Regex("^Blistered by lightning, (?<targetname>.+) falls!$"));
			// ____'s last strength withers before you! (Nether)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+)'s last strength withers before you!$"));
			// ____ is dessicated by your attack! (Nether)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+) is dessicated by your attack!$"));
			// ____ is incinerated by your assault! (Fire)
			TargetKilledByMe.Add(new Regex("^(?<targetname>.+) is incinerated by your assault!$"));
		}

		public static bool IsFailedAttack(string text)
		{
			foreach (Regex regex in FailedAttacks)
			{
				if (regex.IsMatch(text))
					return true;
			}

			return false;
		}

		public static bool IsKilledByMeMessage(string text)
		{
			foreach (Regex regex in TargetKilledByMe)
			{
				if (regex.IsMatch(text))
					return true;
			}

			return false;
		}
	}
}
