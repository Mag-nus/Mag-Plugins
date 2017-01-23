using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace Mag_WorldObjectLogger
{
	[FriendlyName("Mag-WorldObjectLogger")]
	public class Class1 : PluginBase
	{
		private DirectoryInfo pluginPersonalFolder;

		protected override void Startup()
		{
			pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-WorldObjectLogger");

			if (!pluginPersonalFolder.Exists)
				pluginPersonalFolder.Create();

			Core.EchoFilter.ServerDispatch += EchoFilter_ServerDispatch;

			Core.WorldFilter.ReleaseObject += WorldFilter_ReleaseObject;

			Core.RenderFrame += Core_RenderFrame;
		}

		protected override void Shutdown()
		{
			Core.EchoFilter.ServerDispatch -= EchoFilter_ServerDispatch;

			Core.WorldFilter.ReleaseObject -= WorldFilter_ReleaseObject;

			Core.RenderFrame -= Core_RenderFrame;
		}


		private readonly Dictionary<int, string> itemsLogged = new Dictionary<int, string>();
		private readonly Dictionary<int, string> itemsLoggedWithIdent = new Dictionary<int, string>();
		private readonly List<string> itemsLoggedWithExtendedIdent = new List<string>();


		private struct ExtendIDAttributeInfo
		{
			public uint healthMax;
			public uint staminaMax;
			public uint manaMax;
			public uint strength;
			public uint endurance;
			public uint quickness;
			public uint coordination;
			public uint focus;
			public uint self;
		}

		private readonly Dictionary<int, ExtendIDAttributeInfo> identAttributes = new Dictionary<int, ExtendIDAttributeInfo>();

		private readonly Dictionary<int, Dictionary<int, int>> identResources = new Dictionary<int, Dictionary<int, int>>();


		private void EchoFilter_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				if (e.Message.Type == 0xF745) // Create Object
				{
					var id = Convert.ToInt32(e.Message.Value<int>("object"));

					if (id == 0)
						return;

					// ModelData
					// PhysicsData
					// GameData

					var wo = Core.WorldFilter[id];

					if (wo != null)
					{
						// Spells and projectiles are ObjectClass.Unknown
						if (wo.ObjectClass != ObjectClass.Player && wo.ObjectClass != ObjectClass.Corpse && wo.ObjectClass != ObjectClass.Unknown && wo.Container == 0)
						{
							if (!itemsLogged.ContainsKey(wo.Id) || itemsLogged[wo.Id] != wo.Name)
							{
								itemsLogged[wo.Id] = wo.Name;

								LogItemFromCreate(wo, e.Message.RawData);
								LogItem(wo);
							}
						}
					}
				}

				if (e.Message.Type == 0xF7B0) // Game Event
				{
					if (e.Message.Value<uint>("event") == 0x00C9) // Identify Object
					{
						var id = Convert.ToInt32(e.Message.Value<int>("object"));

						if (id == 0)
							return;

						if ((e.Message.Value<uint>("flags") & 0x00000100) != 0)
						{
							if (e.Message.Value<uint>("healthMax") > 0)
							{
								var extendIDAttributeInfo = new ExtendIDAttributeInfo();
								extendIDAttributeInfo.healthMax = e.Message.Value<uint>("healthMax");
								extendIDAttributeInfo.staminaMax = e.Message.Value<uint>("staminaMax");
								extendIDAttributeInfo.manaMax = e.Message.Value<uint>("manaMax");
								extendIDAttributeInfo.strength = e.Message.Value<uint>("strength");
								extendIDAttributeInfo.endurance = e.Message.Value<uint>("endurance");
								extendIDAttributeInfo.quickness = e.Message.Value<uint>("quickness");
								extendIDAttributeInfo.coordination = e.Message.Value<uint>("coordination");
								extendIDAttributeInfo.focus = e.Message.Value<uint>("focus");
								extendIDAttributeInfo.self = e.Message.Value<uint>("self");

								identAttributes[id] = extendIDAttributeInfo;
							}
						}

						if ((e.Message.Value<uint>("flags") & 0x00001000) != 0)
						{
							var resourceCount = e.Message.Value<uint>("resourceCount");

							if (resourceCount > 0)
							{
								byte[] resources = e.Message.RawValue("resources");

								identResources[id] = new Dictionary<int, int>();
								for (int i = 0; i < resources.Length; i += 8)
								{
									int resourcePropertyID = ((resources[i + 3]) << 24) | (resources[i + 2] << 16) | (resources[i + 1] << 8) | (resources[i + 0] << 0);
									int resourceID = ((resources[i + 7]) << 24) | (resources[i + 6] << 16) | (resources[i + 5] << 8) | (resources[i + 4] << 0);

									identResources[id][resourcePropertyID] = resourceID;
								}
							}
						}

						var wo = Core.WorldFilter[id];

						if (wo != null)
						{
							// Spells and projectiles are ObjectClass.Unknown
							if (wo.ObjectClass != ObjectClass.Player && wo.ObjectClass != ObjectClass.Corpse && wo.ObjectClass != ObjectClass.Unknown && wo.Container == 0)
							{
								if (!itemsLoggedWithExtendedIdent.Contains(wo.Name) && identAttributes.ContainsKey(wo.Id))
								{
									itemsLogged[wo.Id] = wo.Name;
									itemsLoggedWithIdent[wo.Id] = wo.Name;
									itemsLoggedWithExtendedIdent.Add(wo.Name);

									LogItem(wo);
								}
								else if ((!itemsLogged.ContainsKey(wo.Id) || itemsLogged[wo.Id] != wo.Name) || (!itemsLoggedWithIdent.ContainsKey(wo.Id) || itemsLoggedWithIdent[wo.Id] != wo.Name))
								{
									itemsLogged[wo.Id] = wo.Name;
									itemsLoggedWithIdent[wo.Id] = wo.Name;

									LogItem(wo);
								}
							}
						}
					}
				}
			}
			catch { }
		}


		private void WorldFilter_ReleaseObject(object sender, ReleaseObjectEventArgs e)
		{
			try
			{
				if (identAttributes.ContainsKey(e.Released.Id))
					identAttributes.Remove(e.Released.Id);

				if (identResources.ContainsKey(e.Released.Id))
					identResources.Remove(e.Released.Id);
			}
			catch { }
		}


		DateTime lastThought = DateTime.MinValue;
		private readonly Dictionary<string, int> requestCount = new Dictionary<string, int>();

		private void Core_RenderFrame(object sender, EventArgs e)
		{
			try
			{
				if (DateTime.Now - lastThought < TimeSpan.FromSeconds(2))
					return;

				lastThought = DateTime.Now;

				foreach (var wo in Core.WorldFilter.GetAll())
				{
					// Spells and projectiles are ObjectClass.Unknown
					if (wo.ObjectClass == ObjectClass.Player || wo.ObjectClass == ObjectClass.Corpse || wo.ObjectClass == ObjectClass.Unknown || wo.Container != 0)
						continue;

					if (GetDistanceFromPlayer(wo) < 40)
					{
						if (wo.ObjectClass == ObjectClass.Npc)
						{
							// Not all NPC's have attribute data
							if (!identAttributes.ContainsKey(wo.Id) && (!requestCount.ContainsKey(wo.Name) || requestCount[wo.Name] < 5))
							{
								//Core.Actions.AddChatText("Requesting id 1: " + wo.Name, 0);

								if (!requestCount.ContainsKey(wo.Name))
									requestCount[wo.Name] = 0;
								requestCount[wo.Name] = requestCount[wo.Name] + 1;

								CoreManager.Current.Actions.RequestId(wo.Id);
							}
						}
						else if (wo.ObjectClass == ObjectClass.Vendor || wo.ObjectClass == ObjectClass.Monster)
						{
							if (!identAttributes.ContainsKey(wo.Id))
							{
								//Core.Actions.AddChatText("Requesting id 2: " + wo.Name, 0);

								CoreManager.Current.Actions.RequestId(wo.Id);
							}
						}
						else if (!itemsLoggedWithIdent.ContainsKey(wo.Id) || itemsLoggedWithIdent[wo.Id] != wo.Name)
						{
							//Core.Actions.AddChatText("Requesting id 3: " + wo.Name, 0);

							CoreManager.Current.Actions.RequestId(wo.Id);
						}
					}
				}
			}
			catch { }
		}


		private void LogItemFromCreate(WorldObject item, byte[] rawData)
		{
			string logFileName = pluginPersonalFolder.FullName + @"\" + CoreManager.Current.Actions.Landcell.ToString("X8") + ".csv";

			FileInfo logFile = new FileInfo(logFileName);

			if (!logFile.Exists)
			{
				using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
				{
					writer.WriteLine("\"Timestamp\",\"LandCell\",\"RawCoordinates\",\"JSON\"");

					writer.Close();
				}
			}


			using (StreamWriter writer = new StreamWriter(logFileName, true))
			{
				StringBuilder output = new StringBuilder();

				// "Timestamp,Landcell,RawCoordinates,JSON"
				output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
				output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");
				output.Append('"' + item.RawCoordinates().X + " " + item.RawCoordinates().Y + " " + item.RawCoordinates().Z + '"' + ",");

				string rawDataOutput = "";

				foreach (var b in rawData)
					rawDataOutput += b.ToString("X2");

				output.Append("\"{\"RawData\":\"" + rawDataOutput + "\"}\"");

				writer.WriteLine(output);

				writer.Close();
			}
		}


		private void LogItem(WorldObject item)
		{
			string logFileName = pluginPersonalFolder.FullName + @"\" + CoreManager.Current.Actions.Landcell.ToString("X8") +".csv";

			FileInfo logFile = new FileInfo(logFileName);

			if (!logFile.Exists)
			{
				using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
				{
					writer.WriteLine("\"Timestamp\",\"LandCell\",\"RawCoordinates\",\"JSON\"");

					writer.Close();
				}
			}


			using (StreamWriter writer = new StreamWriter(logFileName, true))
			{
				bool needsComma = false;
				
				StringBuilder output = new StringBuilder();

				// "Timestamp,Landcell,RawCoordinates,JSON"
				output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
				output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");
				output.Append('"' + item.RawCoordinates().X + " " + item.RawCoordinates().Y + " " + item.RawCoordinates().Z + '"' + ",");

				output.Append("\"{");

				output.Append("\"Id\":\"" + item.Id + "\",");
				output.Append("\"ObjectClass\":\"" + item.ObjectClass + "\",");

				output.Append("\"BoolValues\":{");
				foreach (var value in item.BoolKeys)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append("\"" + value + "\":\"" + item.Values((BoolValueKey)value) + "\"");
				}
				output.Append("},");

				output.Append("\"DoubleValues\":{");
				needsComma = false;
				foreach (var value in item.DoubleKeys)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append("\"" + value + "\":\"" + item.Values((DoubleValueKey)value) + "\"");
				}
				output.Append("},");

				output.Append("\"LongValues\":{");
				needsComma = false;
				foreach (var value in item.LongKeys)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append("\"" + value + "\":\"" + item.Values((LongValueKey)value) + "\"");
				}
				output.Append("},");

				output.Append("\"StringValues\":{");
				needsComma = false;
				foreach (var value in item.StringKeys)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append("\"" + value + "\":\"" + item.Values((StringValueKey)value) + "\"");
				}
				output.Append("},");

				output.Append("\"ActiveSpells\":\"");
				needsComma = false;
				for (int i = 0 ; i < item.ActiveSpellCount ; i++)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append(item.ActiveSpell(i));
				}
				output.Append("\",");

				output.Append("\"Spells\":\"");
				needsComma = false;
				for (int i = 0; i < item.SpellCount; i++)
				{
					if (!needsComma)
						needsComma = true;
					else
						output.Append(",");

					output.Append(item.Spell(i));
				}
				output.Append("\"");

				if (identAttributes.ContainsKey(item.Id))
				{
					var extendedIDAttributeInfo = identAttributes[item.Id];

					output.Append(",\"Attributes\":{");
					output.Append("\"healthMax\":\"" + extendedIDAttributeInfo.healthMax + "\",");
					output.Append("\"staminaMax\":\"" + extendedIDAttributeInfo.staminaMax + "\",");
					output.Append("\"manaMax\":\"" + extendedIDAttributeInfo.manaMax + "\",");
					output.Append("\"strength\":\"" + extendedIDAttributeInfo.strength + "\",");
					output.Append("\"endurance\":\"" + extendedIDAttributeInfo.endurance + "\",");
					output.Append("\"quickness\":\"" + extendedIDAttributeInfo.quickness + "\",");
					output.Append("\"coordination\":\"" + extendedIDAttributeInfo.coordination + "\",");
					output.Append("\"focus\":\"" + extendedIDAttributeInfo.focus + "\",");
					output.Append("\"self\":\"" + extendedIDAttributeInfo.self + "\"");
					output.Append("}");
				}

				if (identResources.ContainsKey(item.Id))
				{
					var resources = identResources[item.Id];

					output.Append(",\"Resources\":{");
					needsComma = false;
					foreach (var value in resources)
					{
						if (!needsComma)
							needsComma = true;
						else
							output.Append(",");

						output.Append("\"" + value.Key + "\":\"" + value.Value + "\"");
					}
					output.Append("}");
				}

				output.Append("}\"");

				writer.WriteLine(output);

				writer.Close();
			}
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
