using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace Mag_CreatureLogger
{
	[FriendlyName("Mag-CreatureLogger")]
	public class Class1 : PluginBase
	{
		private DirectoryInfo pluginPersonalFolder;

		protected override void Startup()
		{
			pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-CreatureLogger");

			if (!pluginPersonalFolder.Exists)
				pluginPersonalFolder.Create();

			CoreManager.Current.CharacterFilter.LoginComplete += CharacterFilter_LoginComplete;
			CoreManager.Current.CharacterFilter.Logoff += CharacterFilter_Logoff;
		}

		protected override void Shutdown()
		{
			CoreManager.Current.CharacterFilter.LoginComplete -= CharacterFilter_LoginComplete;
			CoreManager.Current.CharacterFilter.Logoff -= CharacterFilter_Logoff;
		}

		private void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				CoreManager.Current.RenderFrame += Current_RenderFrame;
				Core.WorldFilter.ChangeObject += WorldFilter_ChangeObject;
			}
			catch { }
		}

		private void CharacterFilter_Logoff(object sender, LogoffEventArgs e)
		{
			try
			{
				Core.WorldFilter.ChangeObject -= WorldFilter_ChangeObject;
				CoreManager.Current.RenderFrame -= Current_RenderFrame;
			}
			catch { }
		}

		private readonly List<string> creaturesLogged = new List<string>();
		private readonly List<KeyValuePair<string, string>> itemsLogged = new List<KeyValuePair<string, string>>();

		DateTime lastThought = DateTime.MinValue;

		//private bool assessCreatureTrained;

		private void Current_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				// This function simply requests ID's for things... We only do that every 5 seconds
				if (DateTime.Now - lastThought < TimeSpan.FromMilliseconds(5000))
					return;

				lastThought = DateTime.Now;

				//assessCreatureTrained = CoreManager.Current.CharacterFilter.Skills[CharFilterSkillType.AssessCreature].Training >= TrainingType.Trained;

				foreach (var wo in CoreManager.Current.WorldFilter.GetAll())
				{
					if (wo.ObjectClass == ObjectClass.Monster)
					{
						if (creaturesLogged.Contains(wo.Name))
							continue;

						if (GetDistanceFromPlayer(wo) > 10)
							continue;

						CoreManager.Current.Actions.RequestId(wo.Id);
					}
					else if (wo.ObjectClass != ObjectClass.MeleeWeapon && wo.ObjectClass != ObjectClass.MissileWeapon && wo.ObjectClass == ObjectClass.WandStaffOrb)
					{
						if (GetDistanceFromPlayer(wo) > 10)
							continue;

						string parentCreature = null;

						// Make sure the item is held by a creature

						if (parentCreature == null)
							continue;

						foreach (var kvp in itemsLogged)
						{
							if (kvp.Key == parentCreature && kvp.Value == wo.Name)
								goto next;
						}

						CoreManager.Current.Actions.RequestId(wo.Id);
						
						next:;
					}
				}
			}
			catch { }
		}

		private void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				if (e.Change != WorldChangeType.IdentReceived)
					return;

				if (e.Changed.ObjectClass == ObjectClass.Monster)
				{
					if (creaturesLogged.Contains(e.Changed.Name))
						return;

					// Make sure we have extended id information. Str/Coord, etc..

					creaturesLogged.Add(e.Changed.Name);

					LogCreature(e.Changed);
				}
				else if (e.Changed.ObjectClass != ObjectClass.MeleeWeapon && e.Changed.ObjectClass != ObjectClass.MissileWeapon && e.Changed.ObjectClass == ObjectClass.WandStaffOrb)
				{
					string parentCreature = null;

					// Make sure the item is held by a creature

					if (parentCreature == null)
						return;

					// Make sure we have extended id information. Dmg, or some mod

					itemsLogged.Add(new KeyValuePair<string, string>(parentCreature, e.Changed.Name));

					LogCreatureItem(parentCreature, e.Changed);
				}
			}
			catch { }
		}

		private void LogCreature(WorldObject creature)
		{
			string logFileName = pluginPersonalFolder.FullName + @"\" + CoreManager.Current.Actions.Landcell.ToString("X8") + " Creatures.csv";

			FileInfo logFile = new FileInfo(logFileName);

			if (!logFile.Exists)
			{
				using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
				{
					// Fix this header
					//writer.WriteLine("\"Timestamp\",\"LandCell\",\"RawCoordinates\",\"JSON\"");

					writer.Close();
				}
			}


			// Append the log here
		}

		private void LogCreatureItem(string parnetCreature, WorldObject item)
		{
			string logFileName = pluginPersonalFolder.FullName + @"\" + CoreManager.Current.Actions.Landcell.ToString("X8") + " Creature Items.csv";

			FileInfo logFile = new FileInfo(logFileName);

			if (!logFile.Exists)
			{
				using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
				{
					// Fix this header
					//writer.WriteLine("\"Timestamp\",\"LandCell\",\"RawCoordinates\",\"JSON\"");

					writer.Close();
				}
			}


			// Append the log here
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
		private static double GetDistanceFromPlayer(WorldObject destObj)
		{
			if (CoreManager.Current.CharacterFilter.Id == 0)
				throw new ArgumentOutOfRangeException("destObj", "CharacterFilter.Id of 0");

			if (destObj.Id == 0)
				throw new ArgumentOutOfRangeException("destObj", "Object passed with an Id of 0");

			return CoreManager.Current.WorldFilter.Distance(CoreManager.Current.CharacterFilter.Id, destObj.Id) * 240;
		}
	}
}
