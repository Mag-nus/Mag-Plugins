using System;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools
{
	public static class Util
	{
		/// <summary>
		/// This function will return the distance in meters.
		/// The manual distance units are in map compass units, while the distance units used in the UI are meters.
		/// In AC there are 240 meters in a kilometer; thus if you set your attack range to 1 in the UI it
		/// will showas 0.00416666666666667in the manual options (0.00416666666666667 being 1/240). 
		/// </summary>
		/// <param name="id1"></param>
		/// <param name="ind2"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Object passed with an Id of 0</exception>
		public static double GetDistance(WorldObject obj1, WorldObject obj2)
		{
			if (obj1.Id == 0 || obj2.Id == 0)
				throw new ArgumentOutOfRangeException("Object passed with an Id of 0");

			return CoreManager.Current.WorldFilter.Distance(obj1.Id, obj2.Id) * 240;
		}

		/// <summary>
		/// This function will return the distance in meters.
		/// The manual distance units are in map compass units, while the distance units used in the UI are meters.
		/// In AC there are 240 meters in a kilometer; thus if you set your attack range to 1 in the UI it
		/// will showas 0.00416666666666667in the manual options (0.00416666666666667 being 1/240). 
		/// </summary>
		/// <param name="id1"></param>
		/// <param name="ind2"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">CharacterFilder.Id or Object passed with an Id of 0</exception>
		public static double GetDistanceFromPlayer(WorldObject destObj)
		{
			if (CoreManager.Current.CharacterFilter.Id == 0)
				throw new ArgumentOutOfRangeException("CharacterFilter.Id of 0");

			if (destObj.Id == 0)
				throw new ArgumentOutOfRangeException("Object passed with an Id of 0");

			return CoreManager.Current.WorldFilter.Distance(CoreManager.Current.CharacterFilter.Id, destObj.Id) * 240;
		}

		/// <summary>
		/// Gets the closest object found of the specified object class. If no object is found, null is returned.
		/// </summary>
		/// <returns></returns>
		public static WorldObject GetClosestObject(ObjectClass objectClass)
		{
			WorldObject Closest = null;

			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetLandscape())
			{
				if (obj.ObjectClass != objectClass)
					continue;

				if (Closest == null || GetDistanceFromPlayer(obj) < GetDistanceFromPlayer(Closest))
					Closest = obj;
			}

			return Closest;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static int GetFreePackSlots(int container)
		{
			if (container == 0)
				throw new ArgumentOutOfRangeException("Invalid container passed, id of 0.");

			WorldObject target = CoreManager.Current.WorldFilter[container];

			if (target == null || (target.ObjectClass != ObjectClass.Player && target.ObjectClass != ObjectClass.Container))
				throw new ArgumentOutOfRangeException("Invalid container passed, null reference");

			int slots_filled = 0;

			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetByContainer(container))
			{
				if (obj.ObjectClass == ObjectClass.Container || obj.ObjectClass == ObjectClass.Foci || obj.Values(LongValueKey.EquippedSlots) != 0)
					continue;

				slots_filled++;
			}

			return CoreManager.Current.WorldFilter[container].Values(LongValueKey.ItemSlots) - slots_filled;
		}

		public static bool IsSpellCastMessage(string text)
		{
			if (text == null)
				return false;

			// Fat Guy In A Little Coat says, "Zojak
			if (text.Contains("says, \""))
			{
				if (text.Contains("says, \"Zojak") ||
					text.Contains("says, \"Malar") ||
					text.Contains("says, \"Puish") ||
					text.Contains("says, \"Cruath") ||
					text.Contains("says, \"Volae") ||
					text.Contains("says, \"Quavosh") ||
					text.Contains("says, \"Shurov") ||
					text.Contains("says, \"Boquar") ||
					text.Contains("says, \"Helkas") ||
					text.Contains("says, \"Equin") ||
					text.Contains("says, \"Roiga") ||
					text.Contains("says, \"Malar") ||
					text.Contains("says, \"Jevak") ||
					text.Contains("says, \"Tugak") ||
					text.Contains("says, \"Slavu") ||
					text.Contains("says, \"Drostu") ||
					text.Contains("says, \"Traku") ||
					text.Contains("says, \"Yanoi") ||
					text.Contains("says, \"Drosta") ||
					text.Contains("says, \"Feazh"))
					return true;
			}

			// You say, "Zojak 
			else if (text.StartsWith("You say, "))
			{
				if (text.StartsWith("You say, \"Zojak") ||
					text.StartsWith("You say, \"Malar") ||
					text.StartsWith("You say, \"Puish") ||
					text.StartsWith("You say, \"Cruath") ||
					text.StartsWith("You say, \"Volae") ||
					text.StartsWith("You say, \"Quavosh") ||
					text.StartsWith("You say, \"Shurov") ||
					text.StartsWith("You say, \"Boquar") ||
					text.StartsWith("You say, \"Helkas") ||
					text.StartsWith("You say, \"Equin") ||
					text.StartsWith("You say, \"Roiga") ||
					text.StartsWith("You say, \"Malar") ||
					text.StartsWith("You say, \"Jevak") ||
					text.StartsWith("You say, \"Tugak") ||
					text.StartsWith("You say, \"Slavu") ||
					text.StartsWith("You say, \"Drostu") ||
					text.StartsWith("You say, \"Traku") ||
					text.StartsWith("You say, \"Yanoi") ||
					text.StartsWith("You say, \"Drosta") ||
					text.StartsWith("You say, \"Feazh"))
					return true;
			}

			return false;
		}

		/// <summary>
		/// This will return a skills name by its id.
		/// For example, 1 returns "Axe".
		/// If the skill is unknown the following is returned: "Unknown skill id: " + id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string GetSkillNameById(int id)
		{
			// This list was taken from the Alinco source
			if (id == 0x1) return "Axe";
			if (id == 0x2) return "Bow";
			if (id == 0x3) return "Crossbow";
			if (id == 0x4) return "Dagger";
			if (id == 0x5) return "Mace";
			if (id == 0x6) return "Melee Defense";
			if (id == 0x7) return "Missile Defense";
			if (id == 0x9) return "Spear";
			if (id == 0xA) return "Staff";
			if (id == 0xB) return "Sword";
			if (id == 0xC) return "Thrown Weapons";
			if (id == 0xD) return "Unarmed Combat";
			if (id == 0xE) return "Arcane Lore";
			if (id == 0xF) return "Magic Defense";
			if (id == 0x10) return "Mana Conversion";
			if (id == 0x12) return "Item Tinkering";
			if (id == 0x13) return "Assess Person";
			if (id == 0x14) return "Deception";
			if (id == 0x15) return "Healing";
			if (id == 0x16) return "Jump";
			if (id == 0x17) return "Lockpick";
			if (id == 0x18) return "Run";
			if (id == 0x1B) return "Assess Creature";
			if (id == 0x1C) return "Weapon Tinkering";
			if (id == 0x1D) return "Armor Tinkering";
			if (id == 0x1E) return "Magic Item Tinkering";
			if (id == 0x1F) return "Creature Enchantment";
			if (id == 0x20) return "Item Enchantment";
			if (id == 0x21) return "Life Magic";
			if (id == 0x22) return "War Magic";
			if (id == 0x23) return "Leadership";
			if (id == 0x24) return "Loyalty";
			if (id == 0x25) return "Fletching";
			if (id == 0x26) return "Alchemy";
			if (id == 0x27) return "Cooking";
			if (id == 0x28) return "Salvaging";
			if (id == 0x29) return "Two Handed Combat";

			return "Unknown skill id: " + id;
		}

		/// <summary>
		/// Returns true if the text is a monster killed by you death message. ex:
		/// You flatten Noble Remains's body with the force of your assault!
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsMonsterKilledByYouDeathMessage(string text)
		{
			if (text.StartsWith("You say, ") || text.Contains("says, \""))
				return false;

			// You flatten Noble Remains's body with the force of your assault!
			// You bring Wight Blade Sorcerer to a fiery end! (Fire)
			// You beat Door to a lifeless pulp!
			// You smite Famished Eater mightily!
			// You obliterate Drudge Skulker!
			// You run Insatiable Eater through! (Pierce)
			// You reduce Insatiable Eater to a sizzling, oozing mass! (Acid)
			// You knock Blockade Guard into next Morningthaw!
			// You split Blockade Guard apart! (Slash)
			// You cleave Blockade Guard in twain! (Slash)
			// You slay Blockade Guard viciously enough to impart death several times over! (Slash)
			// You reduce ____ to a drained, twisted corpse! (Nether)
			if (text.StartsWith("You "))
			{
				if (text.Contains("with the force of your assault!"))
					return true;
				if (text.Contains("to a fiery end!"))
					return true;
				if (text.Contains("to a lifeless pulp!"))
					return true;
				if (text.Contains("smite") && text.Contains("mightily!"))
					return true;
				if (text.StartsWith("You obliterate ") && text.Contains("!"))
					return true;
				if (text.StartsWith("You run ") && text.Contains("through!"))
					return true;
				if (text.StartsWith("You reduce ") && text.Contains("to a sizzling, oozing mass!"))
					return true;
				if (text.StartsWith("You knock ") && text.Contains("into next Morningthaw!"))
					return true;
				if (text.StartsWith("You split ") && text.Contains("apart!"))
					return true;
				if (text.StartsWith("You cleave ") && text.Contains("in twain!"))
					return true;
				if (text.StartsWith("You slay ") && text.Contains("several times over!"))
					return true;
				if (text.StartsWith("You reduce ") && text.Contains("twisted corpse!"))
					return true;
			}
			// Your killing blow nearly turns Shivering Crystalline Wisp inside-out!
			// Your attack stops Ruschk Draktehn cold! (Frost)
			// Your lightning coruscates over Insatiable Eater's mortal remains! (Lightning)
			// Your assault sends Ardent Moar to an icy death!
			else if (text.StartsWith("Your "))
			{
				if (text.Contains("killing blow nearly turns") && text.Contains("inside-out!"))
					return true;
				if (text.Contains("attack stops") && text.Contains("cold!"))
					return true;
				if (text.Contains("lightning coruscates") && text.Contains("mortal remains!"))
					return true;
				if (text.Contains("assault sends") && text.Contains("to an icy death!"))
					return true;
			}
			// The thunder of crushing Pyre Minion is followed by the deafening silence of death!
			// The deadly force of your attack is so strong that Young Banderling's ancestors feel it!
			// The force of your killing blow violently dislocated Insatiable Eaters jaw!
			else if (text.StartsWith("The "))
			{
				if (text.Contains("is followed by the deafening silence of death!"))
					return true;
				if (text.Contains("ancestors feel it!"))
					return true;
				if (text.Contains("force of your killing blow"))
					return true;
			}
			// Wight Blade Sorcerer's seared corpse smolders before you! (Fire)
			// Wight is reduced to cinders! (Fire)
			// Old Bones is shattered by your assault!
			// Gnawer Shreth catches your attack, with dire consequences!
			// Gnawer Shreth is utterly destroyed by your attack!
			// Ruschk Laktar suffers a frozen fate! (Frost)
			// Insatiable Eater's perforated corpse falls before you! (Pierce)
			// Insatiable Eater is fatally punctured! (Pierce)
			// Insatiable Eater's death is preceded by a sharp, stabbing pain! (Pierce)
			// Insatiable Eater is torn to ribbons by your assault! (Slash)
			// Insatiable Eater is liquified by your attack! (Acid)
			// Insatiable Eater's last strength dissolves before you! (Acid)
			// Electricity tears Insatiable Eater apart! (Lightning)
			// Blistered by lightning, Insatiable Eater falls! (Lightning)
			// Uber Penguin is inscinerated by your assault! (Fire)
			// ____'s last strength withers before you! (Nether)
			// ____ is dessicated by your attack! (Nether)
			// ____ is incinerated by your assault!
			else
			{
				if (text.Contains("seared corpse smolders before you!"))
					return true;
				if (text.Contains("is reduced to cinders!"))
					return true;
				if (text.Contains("is shattered by your assault!"))
					return true;
				if (text.Contains("catches your attack, with dire consequences!"))
					return true;
				if (text.Contains("is utterly destroyed by your attack!"))
					return true;
				if (text.Contains("suffers a frozen fate!"))
					return true;
				if (text.Contains("perforated corpse falls before you!"))
					return true;
				if (text.Contains("is fatally punctured!"))
					return true;
				if (text.Contains("death is preceded by a sharp, stabbing pain!"))
					return true;
				if (text.Contains("is torn to ribbons by your assault!"))
					return true;
				if (text.Contains("is liquified by your attack!"))
					return true;
				if (text.Contains("last strength dissolves before you!"))
					return true;
				if (text.Contains("Electricity tears") && text.Contains("apart!"))
					return true;
				if (text.Contains("Blistered by lightning") && text.Contains("falls!"))
					return true;
				if (text.Contains("inscinerated by your assault!") )
					return true;
				if (text.Contains("last strength withers before you!"))
					return true;
				if (text.Contains("dessicated by your attack!"))
					return true;
				if (text.Contains("is incinerated by your assault!"))
					return true;
			}

			return false;
		}
	}
}
