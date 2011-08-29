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

			int slots_filled = 0;

			foreach (WorldObject obj in CoreManager.Current.WorldFilter.GetByContainer(container))
			{
				if (obj.ObjectClass == ObjectClass.Container || obj.ObjectClass == ObjectClass.Foci || obj.Values(LongValueKey.EquippedSlots) != 0)
					continue;

				slots_filled++;
			}

			if (CoreManager.Current.WorldFilter[container] == null)
				throw new ArgumentOutOfRangeException("Invalid container passed, null reference");

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
					text.Contains("says, \"Drosta"))
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
					text.StartsWith("You say, \"Drosta"))
					return true;
			}

			return false;
		}
	}
}
