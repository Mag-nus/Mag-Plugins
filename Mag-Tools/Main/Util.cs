using System;
using System.Text.RegularExpressions;

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
		/// <param name="obj1"></param>
		/// <param name="obj2"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">Object passed with an Id of 0</exception>
		public static double GetDistance(WorldObject obj1, WorldObject obj2)
		{
			if (obj1.Id == 0)
				throw new ArgumentOutOfRangeException("obj1", "Object passed with an Id of 0");

			if (obj2.Id == 0)
				throw new ArgumentOutOfRangeException("obj2", "Object passed with an Id of 0");

			return CoreManager.Current.WorldFilter.Distance(obj1.Id, obj2.Id) * 240;
		}

		/// <summary>
		/// This function will return the distance in meters.
		/// The manual distance units are in map compass units, while the distance units used in the UI are meters.
		/// In AC there are 240 meters in a kilometer; thus if you set your attack range to 1 in the UI it
		/// will showas 0.00416666666666667in the manual options (0.00416666666666667 being 1/240). 
		/// </summary>
		/// <param name="destObj"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">CharacterFilder.Id or Object passed with an Id of 0</exception>
		public static double GetDistanceFromPlayer(WorldObject destObj)
		{
			if (CoreManager.Current.CharacterFilter.Id == 0)
				throw new ArgumentOutOfRangeException("destObj", "CharacterFilter.Id of 0");

			if (destObj.Id == 0)
				throw new ArgumentOutOfRangeException("destObj", "Object passed with an Id of 0");

			return CoreManager.Current.WorldFilter.Distance(CoreManager.Current.CharacterFilter.Id, destObj.Id) * 240;
		}

		/// <summary>
		/// Gets the closest object found of the specified object class. If no object is found, null is returned.
		/// </summary>
		/// <returns></returns>
		public static WorldObject GetClosestObject(ObjectClass objectClass)
		{
			WorldObject closest = null;

			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetLandscape())
			{
				if (obj.ObjectClass != objectClass)
					continue;

				if (closest == null || GetDistanceFromPlayer(obj) < GetDistanceFromPlayer(closest))
					closest = obj;
			}

			return closest;
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
				throw new ArgumentOutOfRangeException("container", "Invalid container passed, id of 0.");

			WorldObject target = CoreManager.Current.WorldFilter[container];

			if (target == null || (target.ObjectClass != ObjectClass.Player && target.ObjectClass != ObjectClass.Container))
				throw new ArgumentOutOfRangeException("container", "Invalid container passed, null reference");

			int slotsFilled = 0;

			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetByContainer(container))
			{
				if (obj.ObjectClass == ObjectClass.Container || obj.ObjectClass == ObjectClass.Foci || obj.Values(LongValueKey.EquippedSlots) != 0)
					continue;

				slotsFilled++;
			}

			return CoreManager.Current.WorldFilter[container].Values(LongValueKey.ItemSlots) - slotsFilled;
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
			if (id == 0x2B) return "Nether";

			return "Unknown skill id: " + id;
		}

		/// <summary>
		/// This will return an armor attribute set name based on its id.
		/// For example, 27 returns "Acid Proof Set".
		/// If the set is unknown the following is returned: "Unknown set id: " + id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string GetAttributeSetNameById(int id)
		{
			// This list was taken from Virindi Tank Loot Editor
			// 01-04
			if (id == 05) return "Noble Relic Set";
			if (id == 06) return "Ancient Relic Set";
			if (id == 07) return "Relic Alduressa Set";
			if (id == 08) return "Shou-jen Set";
			if (id == 09) return "Empyrean Rings Set";
			if (id == 10) return "Arm, Mind, Heart Set";
			if (id == 11) return "Coat of the Perfect Light Set";
			if (id == 12) return "Leggings of Perfect Light Set";
			if (id == 13) return "Soldier's Set";
			if (id == 14) return "Adept's Set";
			if (id == 15) return "Archer's Set";
			if (id == 16) return "Defender's Set";
			if (id == 17) return "Tinker's Set";
			if (id == 18) return "Crafter's Set";
			if (id == 19) return "Hearty Set";
			if (id == 20) return "Dexterous Set";
			if (id == 21) return "Wise Set";
			if (id == 22) return "Swift Set";
			if (id == 23) return "Hardenend Set";
			if (id == 24) return "Reinforced Set";
			if (id == 25) return "Interlocking Set";
			if (id == 26) return "Flame Proof Set";
			if (id == 27) return "Acid Proof Set";
			if (id == 28) return "Cold Proof Set";
			if (id == 29) return "Lightning Proof Set";
			if (id == 30) return "Dedication Set";
			if (id == 31) return "Gladiatorial Clothing Set";
			if (id == 32) return "Protective Clothing Set";
			// 33-34
			if (id == 35) return "Sigil of Defense";
			if (id == 36) return "Sigil of Destruction";
			if (id == 37) return "Sigil of Fury";
			if (id == 38) return "Sigil of Growth";
			if (id == 39) return "Sigil of Vigor";
			// 40-48
			if (id == 49) return "Alchemy Set";
			if (id == 50) return "Arcane Lore Set";
			if (id == 51) return "Armor Tinkering Set";
			if (id == 52) return "Assess Person Set";
			if (id == 53) return "Axe Set";
			if (id == 54) return "Bow Set";
			if (id == 55) return "Cooking Set";
			if (id == 56) return "Creature Enchantment Set";
			if (id == 57) return "Crossbow Set";
			if (id == 58) return "Dagger Set";
			if (id == 59) return "Deception Set";
			if (id == 60) return "Fletching Set";
			if (id == 61) return "Healing Set";
			if (id == 62) return "Item Enchantment Set";
			if (id == 63) return "Item Tinkering Set";
			if (id == 64) return "Leadership Set";
			if (id == 65) return "Life Magic Set";
			if (id == 66) return "Loyalty Set";
			if (id == 67) return "Mace Set";
			if (id == 68) return "Magic Defense Set";
			if (id == 69) return "Magic Item Tinkering Set";
			if (id == 70) return "Mana Conversion Set";
			if (id == 71) return "Melee Defense Set";
			if (id == 72) return "Missile Defense Set";
			if (id == 73) return "Salvaging Set";
			if (id == 74) return "Spear Set";
			if (id == 75) return "Staff Set";
			if (id == 76) return "Sword Set";
			if (id == 77) return "Thrown Weapons Set";
			if (id == 78) return "Two Handed Combat Set";
			if (id == 79) return "Unarmed Combat Set";
			if (id == 80) return "Void Magic Set";
			if (id == 81) return "War Magic Set";
			if (id == 82) return "Weapon Tinkering Set";
			if (id == 83) return "Assess Creature  Set";

			return "Unknown set id: " + id;
		}

		// http://www.regular-expressions.info/reference.html

		// Local Chat
		// You say, "test"
		private static readonly Regex YouSay = new Regex("^You say, \"(?<msg>.*)\"$");
		// <Tell:IIDString:1343111160:PlayerName>PlayerName<\Tell> says, "asdf"
		private static readonly Regex PlayerSaysLocal = new Regex("^<Tell:IIDString:[0-9]+:(?<name>[\\w\\s'-]+)>[\\w\\s'-]+<\\\\Tell> says, \"(?<msg>.*)\"$");
		//
		// Master Arbitrator says, "Arena Three is now available for new warriors!"
		private static readonly Regex NpcSays = new Regex("^(?<name>[\\w\\s'-]+) says, \"(?<msg>.*)\"$");

		// Channel Chat
		// [Allegiance] <Tell:IIDString:0:PlayerName>PlayerName<\Tell> says, "kk"
		// [General] <Tell:IIDString:0:PlayerName>PlayerName<\Tell> says, "asdfasdfasdf"
		// [Fellowship] <Tell:IIDString:0:PlayerName>PlayerName<\Tell> says, "test"
		private static readonly Regex PlayerSaysChannel = new Regex("^\\[(?<channel>.+)]+ <Tell:IIDString:[0-9]+:(?<name>[\\w\\s'-]+)>[\\w\\s'-]+<\\\\Tell> says, \"(?<msg>.*)\"$");
		//
		// [Fellowship] <Tell:IIDString:0:Master Arbitrator>Master Arbitrator<\Tell> says, "Good Luck!"

		// Tells
		// You tell PlayerName, "test"
		private static readonly Regex YouTell = new Regex("^You tell .+, \"(?<msg>.*)\"$");
		// <Tell:IIDString:1343111160:PlayerName>PlayerName<\Tell> tells you, "test"
		private static readonly Regex PlayerTellsYou = new Regex("^<Tell:IIDString:[0-9]+:(?<name>[\\w\\s'-]+)>[\\w\\s'-]+<\\\\Tell> tells you, \"(?<msg>.*)\"$");
		//
		// Master Arbitrator tells you, "You fought in the Colosseum's Arenas too recently. I cannot reward you for 4s."
		private static readonly Regex NpcTellsYou = new Regex("^(?<name>[\\w\\s'-]+) tells you, \"(?<msg>.*)\"$");

		[Flags]
		public enum ChatFlags : byte
		{
			PlayerSaysLocal		= 0x01,
			PlayerSaysChannel	= 0x02,
			YouSay				= 0x04,

			PlayerTellsYou		= 0x08,
			YouTell				= 0x10,

			NpcSays				= 0x20,
			NpcTellsYou			= 0x40,

			All					= 0xFF,
		}

		/// <summary>
		/// Returns true if the text was said by a person, envoy, npc, monster, etc..
		/// </summary>
		/// <param name="text"></param>
		/// <param name="chatFlags"></param>
		/// <returns></returns>
		public static bool IsChat(string text, ChatFlags chatFlags = ChatFlags.All)
		{
			if ((chatFlags & ChatFlags.PlayerSaysLocal) == ChatFlags.PlayerSaysLocal && PlayerSaysLocal.IsMatch(text))
				return true;

			if ((chatFlags & ChatFlags.PlayerSaysChannel) == ChatFlags.PlayerSaysChannel && PlayerSaysChannel.IsMatch(text))
				return true;

			if ((chatFlags & ChatFlags.YouSay) == ChatFlags.YouSay && YouSay.IsMatch(text))
				return true;


			if ((chatFlags & ChatFlags.PlayerTellsYou) == ChatFlags.PlayerTellsYou && PlayerTellsYou.IsMatch(text))
				return true;

			if ((chatFlags & ChatFlags.YouTell) == ChatFlags.YouTell && YouTell.IsMatch(text))
				return true;


			if ((chatFlags & ChatFlags.NpcSays) == ChatFlags.NpcSays && NpcSays.IsMatch(text))
				return true;

			if ((chatFlags & ChatFlags.NpcTellsYou) == ChatFlags.NpcTellsYou && NpcTellsYou.IsMatch(text))
				return true;

			return false;
		}

		/// <summary>
		/// This will return the name of the person/monster/npc of a chat message or tell.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string GetSourceOfChat(string text)
		{
			bool isSays = IsChat(text, ChatFlags.NpcSays | ChatFlags.PlayerSaysChannel | ChatFlags.PlayerSaysLocal);
			bool isTell = IsChat(text, ChatFlags.NpcTellsYou | ChatFlags.PlayerTellsYou);

			if (isSays && isTell)
			{
				int indexOfSays = text.IndexOf(" says, \"");
				int indexOfTell = text.IndexOf(" tells you");

				if (indexOfSays <= indexOfTell)
					isTell = false;
				else
					isSays = false;
			}

			string source = string.Empty;

			if (isSays)
				source = text.Substring(0, text.IndexOf(" says, \""));
			else if (isTell)
				source = text.Substring(0, text.IndexOf(" tells you"));
			else
				return source;

			source = source.Trim();

			if (source.Contains(">") && source.Contains("<"))
			{
				source = source.Remove(0, source.IndexOf('>') + 1);
				if (source.Contains("<"))
					source = source.Substring(0, source.IndexOf('<'));
			}

			return source;
		}
	}
}
