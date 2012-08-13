using System;
using System.IO;
using System.Text.RegularExpressions;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

using Decal.Filters;

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

		public static void ExportSpells(string targetFileName)
		{
			using (StreamWriter writer = new StreamWriter(targetFileName, true))
			{
				writer.WriteLine("Id,Name,Description,Difficulty,Duration,Family,Flags,Generation,IconId,IsDebuff,IsFastWindup,IsFellowship,IsIrresistible,IsOffensive,IsUntargetted,Mana,School,SortKey,Speed,TargetEffect,TargetMask,Type,Unknown1,Unknown2,Unknown3,Unknown4,Unknown5,Unknown6,Unknown7,Unknown8,Unknown9,Unknown10");

				FileService service = CoreManager.Current.Filter<FileService>();

				for (int i = 0 ; i < service.SpellTable.Length ; i++)
				{
					Spell spell = service.SpellTable[i];

					string flags = "";
					flags += ((spell.Flags & 0x80000) == 0x80000) ? "1" : "0";
					flags += ((spell.Flags & 0x40000) == 0x40000) ? "1" : "0";
					flags += ((spell.Flags & 0x20000) == 0x20000) ? "1" : "0";
					flags += ((spell.Flags & 0x10000) == 0x10000) ? "1" : "0";
					flags += ((spell.Flags & 0x8000) == 0x8000) ? "1" : "0";
					flags += ((spell.Flags & 0x4000) == 0x4000) ? "1" : "0";
					flags += ((spell.Flags & 0x2000) == 0x2000) ? "1" : "0";
					flags += ((spell.Flags & 0x1000) == 0x1000) ? "1" : "0";
					flags += ((spell.Flags & 0x800) == 0x800) ? "1" : "0";
					flags += ((spell.Flags & 0x400) == 0x400) ? "1" : "0";
					flags += ((spell.Flags & 0x200) == 0x200) ? "1" : "0";
					flags += ((spell.Flags & 0x100) == 0x100) ? "1" : "0";
					flags += ((spell.Flags & 0x80) == 0x80) ? "1" : "0";
					flags += ((spell.Flags & 0x40) == 0x40) ? "1" : "0";
					flags += ((spell.Flags & 0x20) == 0x20) ? "1" : "0";
					flags += ((spell.Flags & 0x10) == 0x10) ? "1" : "0";
					flags += ((spell.Flags & 0x8) == 0x8) ? "1" : "0";
					flags += ((spell.Flags & 0x4) == 0x4) ? "1" : "0";
					flags += ((spell.Flags & 0x2) == 0x2) ? "1" : "0";
					flags += ((spell.Flags & 0x1) == 0x1) ? "1" : "0";

					writer.WriteLine(spell.Id + "," + spell.Name.Replace(",", ".") + "," + spell.Description.Replace(",", ".") + "," + spell.Difficulty + "," + spell.Duration + "," + spell.Family + "," + flags + "," + spell.Generation + "," + spell.IconId + "," + spell.IsDebuff + "," + spell.IsFastWindup + "," + spell.IsFellowship + "," + spell.IsIrresistible + "," + spell.IsOffensive + "," + spell.IsUntargetted + "," + spell.Mana + "," + spell.School + "," + spell.SortKey + "," + spell.Speed + "," + spell.TargetEffect + "," + spell.TargetMask + "," + spell.Type + "," + spell.Unknown1 + "," + spell.Unknown2 + "," + spell.Unknown3 + "," + spell.Unknown4 + "," + spell.Unknown5 + "," + spell.Unknown6 + "," + spell.Unknown7 + "," + spell.Unknown8 + "," + spell.Unknown9 + "," + spell.Unknown10);
				}

				writer.Close();
			}
		}
	}
}
