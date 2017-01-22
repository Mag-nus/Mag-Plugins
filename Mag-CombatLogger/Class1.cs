using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Text;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace Mag_CombatLogger
{
	[FriendlyName("Mag-CombatLogger")]
	public class Class1 : PluginBase
	{
		private DirectoryInfo pluginPersonalFolder;

		protected override void Startup()
		{
			pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-CombatLogger");

			if (!pluginPersonalFolder.Exists)
				pluginPersonalFolder.Create();

			Core.EchoFilter.ServerDispatch += EchoFilter_ServerDispatch;
		}

		protected override void Shutdown()
		{
			Core.EchoFilter.ServerDispatch += EchoFilter_ServerDispatch;
		}

		private void EchoFilter_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				if (e.Message.Type == 0xF745) // Create Object
				{
					
				}

				if (e.Message.Type == 0xF7B0) // Game Event
				{
					if (e.Message.Value<uint>("event") == 0x00C9) // Identify Object
					{
						if ((e.Message.Value<uint>("flags") & 0x00000100) != 0)
						{
							var id = Convert.ToInt32(e.Message.Value<int>("object"));

							if (id == 0)
								return;

							var obj = Core.WorldFilter[id];

							if (obj == null)
								return;

							if (obj.ObjectClass == ObjectClass.Npc || obj.ObjectClass == ObjectClass.Vendor || obj.ObjectClass == ObjectClass.Monster)
							{
								// Any distance greater than 52-53 seems to not return extended ID information
								Core.Actions.AddChatText(obj.Name + ", Distance: " + GetDistanceFromPlayer(obj), 0);
								Core.Actions.AddChatText(e.Message.Value<uint>("healthMax") + " " + e.Message.Value<uint>("staminaMax") + " " + e.Message.Value<uint>("manaMax") + " " + e.Message.Value<uint>("strength") + " " + e.Message.Value<uint>("endurance") + " " + e.Message.Value<uint>("quickness") + " " + e.Message.Value<uint>("coordination") + " " + e.Message.Value<uint>("focus") + " " + e.Message.Value<uint>("self"), 0);

								for (int i = 370; i < 375; i++)
								{
									if (obj.LongKeys.Contains(i))
										Core.Actions.AddChatText(i + ": " + obj.LongKeys[i], 0);
								}
								string output = null;
								foreach (var b in e.Message.RawData)
									output += b.ToString("X2");
								Core.Actions.AddChatText(output, 0);

								Core.Actions.AddChatText(e.Message.Value<uint>("flags").ToString("X8"), 0);

								// Need to add in the ratings as well
							}

							// Dmg/CritDmg	Rating
							// Dmg/CritDmg	Resist

							/*
								8:59:12 Old Bones, Distance: 55.3490826083906
								8:59:12 43 0 0 0 0 0 0 0 0
								8:59:12 B0F70000 AB711150 C1000000 C9000000 D8255FDC 01010000 00000000 02001000 02000000 1E000000 19000000 08000000 00000000 2B000000 2B000000
								8:59:12 flags 00000101

								8:59:14 Old Bones, Distance: 51.2590187317853
								8:59:14 43 50 65 25 35 80 75 55 65
								        dmg/critdmg Rating 5/0
								8:59:14 B0F70000 AB711150 C4000000 C9000000 D8255FDC 01410000 01000000
								03001000
								02000000 1E000000
								33010000 05000000
								19000000 08000000
								08000000 2B000000 2B000000 19000000 23000000 50000000 4B000000 37000000 41000000 32000000 41000000 32000000 41000000 19000000 14000000 14000000 14000000 0A000000 14000000 0F000000 0F000000 0F000000
								8:59:14 flags 00004101

								8:21:31 Mu - miyah Soldier, Distance: 1.792366595809
								8:21:31 4600 5620 1880 210 220 230 230 320 330
										dmg/critdmg Rating 9/0
										dmg/critdmg Resist 0/20
										B0F70000 6B440350 89030000 C9000000 0F9BD391 01410000 01000000
								05001000
								02000000 0E000000
								33010000 09000000
								19000000 F0000000
								3B010000 0A000000
								3C010000 14000000
								08000000 F8110000 F8110000 D2000000 DC000000 E6000000 E6000000 40010000 4A010000 F1150000 7E060000 F4150000 58070000 5E010000 77010000 86010000 68010000 68010000 68010000 68010000 7C010000 7C010000
							*/
						}
					}
				}
			}
			catch { }
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
