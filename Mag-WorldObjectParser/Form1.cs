using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;

using Mag.Shared;

namespace Mag_WorldObjectParser
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.Text += " " + Application.ProductVersion;

			var pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-WorldObjectLogger");
			txtSourcePath.Text = pluginPersonalFolder.FullName;
		}

		private void cmdBrowseForDifferentSource_Click(object sender, EventArgs e)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.SelectedPath = txtSourcePath.Text;

				if (dialog.ShowDialog() == DialogResult.OK)
					txtSourcePath.Text = dialog.SelectedPath;
			}
		}

		private int totalLines;
		private int corruptLines;

		private void cmdReadAllFiles_Click(object sender, EventArgs e)
		{
			lblResults.Text = null;

			var files = Directory.GetFiles(txtSourcePath.Text, "*.csv", SearchOption.AllDirectories);

			totalLines = 0;
			corruptLines = 0;

			for (int i = 0; i < files.Length; i++)
				ProcessFile(files[i], i, files.Length);

			lblResults.Text = totalLines.ToString("N0") + " lines read. " + corruptLines.ToString("N0") + " corrupt lines found.";
		}

		readonly JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

		private void ProcessFile(string fileName, int fileIndex, int totalFiles)
		{
			lblProgress.Text = "Processing file " + (fileIndex + 1).ToString("N0") + " of " + totalFiles.ToString("N0");
			lblProgress.Refresh();
			lblWorkingFile.Text = fileName;
			lblWorkingFile.Refresh();
			progressBar1.Value = (int)(((double)(fileIndex + 1) / totalFiles) * 100);
			progressBar1.Refresh();

			var fileLines = File.ReadAllLines(fileName);

			totalLines += fileLines.Length;

			foreach (var line in fileLines)
			{
				// "Timestamp,Landcell,RawCoordinates,JSON"
				// "2017-01-23 15:25:42Z,"00000000",49.0488996505737 -29.9918003082275 -7.45058059692383E-09","{"RawData":"45F70000334020771100000003980100180001000C00000000003D00020000000C00000000000000B40104724BC8704135EFEFC1000000B2F70435BF0000000000000000F70435BF86000009220000202B000034DA0500020100020000000000000002000000000090010000300000000400446F6F720000A131171380000000141000002000000000000040"}"
				// "2017-01-23 15:25:42Z,"00000000",49.0488996505737 -29.9918003082275 -7.45058059692383E-09","{"Id":"1998602291","ObjectClass":"Door","BoolValues":{},"DoubleValues":{"167772167":"269.999963323784","167772168":"2"},"LongValues":{"19":" - 1","218103811":"1912865204","218103831":"2","218103835":"4116","218103843":"32","218103847":"104451","218103808":"12705","218103830":"33555930","218103832":"48","218103834":"128","218103809":"4887"},"StringValues":{"1":"Door"},"ActiveSpells":"","Spells":""}"

				var thirdComma = Util.IndexOfNth(line, ',', 3);

				if (thirdComma == -1) // Corrupt line
				{
					corruptLines++;
					continue;
				}

				var firstPart = line.Substring(0, thirdComma);

				var firstPartSplit = firstPart.Split(',');
				if (firstPartSplit[0] == "\"Timestamp\"") // Header line
					continue;

				DateTime timestamp;
				if (!DateTime.TryParse(firstPartSplit[0].Substring(1, firstPartSplit[0].Length - 2), out timestamp)) // Corrupt line
				{
					corruptLines++;
					continue;
				}

				int landcell;
				if (!int.TryParse(firstPartSplit[1].Substring(1, firstPartSplit[1].Length - 2), NumberStyles.HexNumber, null, out landcell)) // Corrupt line
				{
					corruptLines++;
					continue;
				}

				Tuple<double, double, double> rawCoordinates;
				double x;
				double y;
				double z;
				var rawCoordinatesSplit = firstPartSplit[2].Substring(1, firstPartSplit[2].Length - 2).Split(' ');
				if (rawCoordinatesSplit.Length != 3) // Corrupt line
				{
					corruptLines++;
					continue;
				}
				if (!double.TryParse(rawCoordinatesSplit[0], out x)) // Corrupt line
				{
					corruptLines++;
					continue;
				}
				if (!double.TryParse(rawCoordinatesSplit[1], out y)) // Corrupt line
				{
					corruptLines++;
					continue;
				}
				if (!double.TryParse(rawCoordinatesSplit[2], out z)) // Corrupt line
				{
					corruptLines++;
					continue;
				}
				rawCoordinates = new Tuple<double, double, double>(x, y, z);

				var jsonPart = line.Substring(thirdComma + 1, line.Length - (thirdComma + 1));
				if (jsonPart[0] != '"' || jsonPart[jsonPart.Length - 1] != '"') // Corrupt line
				{
					corruptLines++;
					continue;
				}

				jsonPart = jsonPart.Substring(1, jsonPart.Length - 2); // Trim the quotes... why did I add them.. :(

				if (jsonPart.StartsWith("{\"Ra"))
				{
					var createPacket = new CreatePacket();

					createPacket.Timestamp = timestamp;
					createPacket.Landcell = landcell;
					createPacket.RawCoordinates = rawCoordinates;

					Dictionary<string, object> result = (Dictionary<string, object>)jsonSerializer.DeserializeObject(jsonPart);

					if (result.Count != 1)
					{
						corruptLines++;
						continue;
					}

					foreach (var kvp in result)
						createPacket.RawData = Util.HexStringToByteArray((string)kvp.Value);
				}
				else if (jsonPart.StartsWith("{\"Id"))
				{
					// This is because I forgot to encode strings... 
					jsonPart = jsonPart.Replace(" \"Ruschk\" ", " \\\"Ruschk\\\" ");
					jsonPart = jsonPart.Replace(" \"giants\" ", " \\\"giants\\\" ");
					jsonPart = jsonPart.Replace(" \"guard dogs.\"", " \\\"guard dogs.\\\"");
					jsonPart = jsonPart.Replace(" \"Slayer of Hope\" ", " \\\"Slayer of Hope\\\" ");
					jsonPart = jsonPart.Replace(" \"The Haven\"", " \\\"The Haven\\\"");
					jsonPart = jsonPart.Replace(" \"intelligent portals\" ", " \\\"intelligent portals\\\" ");
					jsonPart = jsonPart.Replace(" \"anti-magic\" ", " \\\"anti-magic\\\" ");
					jsonPart = jsonPart.Replace(" \"slippage\"", " \\\"slippage\\\"");
					jsonPart = jsonPart.Replace(" \"spilled\"", " \\\"spilled\\\"");
					jsonPart = jsonPart.Replace(" \"small place\" ", " \\\"small place\\\" ");
					jsonPart = jsonPart.Replace(" \"Hea Arantah's\" ", " \\\"Hea Arantah's\\\" ");
					jsonPart = jsonPart.Replace(" \"Wharu\"", " \\\"Wharu\\\"");
					jsonPart = jsonPart.Replace(" \"infiltrators,\"", " \\\"infiltrators,\\\"");
					jsonPart = jsonPart.Replace(" \". . . the (Undead) believe the tentacled creatures are the spawn of the Great Ones.\" ", " \\\". . . the (Undead) believe the tentacled creatures are the spawn of the Great Ones.\\\" ");
					jsonPart = jsonPart.Replace(" \"Great Ones\" ", " \\\"Great Ones\\\" ");
					jsonPart = jsonPart.Replace(" \"fire that fell from the sky.\"", " \\\"fire that fell from the sky.\\\"");
					jsonPart = jsonPart.Replace(" \"Bloodless,\"", " \\\"Bloodless,\\\"");
					jsonPart = jsonPart.Replace(" \"it is best to let a sleeping Ursuin lie.\"", " \\\"it is best to let a sleeping Ursuin lie.\\\"");
					jsonPart = jsonPart.Replace(" \"pure\" ", " \\\"pure\\\" ");
					jsonPart = jsonPart.Replace(" \"The Story of Ben Ten and Yanshi.\" ", " \\\"The Story of Ben Ten and Yanshi.\\\" ");
					jsonPart = jsonPart.Replace(" \"Gria'venir,\" ", " \\\"Gria'venir,\\\" ");
					jsonPart = jsonPart.Replace(" \"Atual Arutoa\" ", " \\\"Atual Arutoa\\\" ");
					jsonPart = jsonPart.Replace(" \"wings.\" ", " \\\"wings.\\\" ");
					jsonPart = jsonPart.Replace(" \"Great Tukal\" ", " \\\"Great Tukal\\\" ");
					jsonPart = jsonPart.Replace(" \"snow sharks.\" ", " \\\"snow sharks.\\\" ");
					jsonPart = jsonPart.Replace("\"Lords of the World\" ", "\\\"Lords of the World\\\" ");
					jsonPart = jsonPart.Replace("\"Winds From Darkness\"", "\\\"Winds From Darkness\\\"");
					jsonPart = jsonPart.Replace(" \"Property of Celcynd\" ", " \\\"Property of Celcynd\\\" ");

					try
					{
						var identResponse = new IdentResponse();

						identResponse.Timestamp = timestamp;
						identResponse.Landcell = landcell;
						identResponse.RawCoordinates = rawCoordinates;

						Dictionary<string, object> result = (Dictionary<string, object>)jsonSerializer.DeserializeObject(jsonPart);

						foreach (var kvp in result)
						{
							switch (kvp.Key)
							{
								case "Id":
									identResponse.Id = int.Parse((string)kvp.Value);
									break;

								case "ObjectClass":
									identResponse.ObjectClass = (ObjectClass)Enum.Parse(typeof(ObjectClass), (string)kvp.Value);
									break;

								case "BoolValues":
									{
										var values = (Dictionary<string, object>) kvp.Value;

										foreach (var kvp2 in values)
										{
											var key = int.Parse(kvp2.Key);
											var value = bool.Parse(kvp2.Value.ToString());

											identResponse.BoolValues[key] = value;
										}
									}

									break;

								case "DoubleValues":
									{
										var values = (Dictionary<string, object>) kvp.Value;

										foreach (var kvp2 in values)
										{
											var key = int.Parse(kvp2.Key);
											var value = double.Parse(kvp2.Value.ToString());

											identResponse.DoubleValues[key] = value;
										}
									}

									break;

								case "LongValues":
									{
										var values = (Dictionary<string, object>)kvp.Value;

										foreach (var kvp2 in values)
										{
											var key = int.Parse(kvp2.Key);
											var value = int.Parse(kvp2.Value.ToString());

											identResponse.LongValues[key] = value;
										}
									}

									break;

								case "StringValues":
									{
										var values = (Dictionary<string, object>)kvp.Value;

										foreach (var kvp2 in values)
										{
											var key = int.Parse(kvp2.Key);

											identResponse.StringValues[key] = kvp2.Value.ToString();
										}
									}

									break;

								case "ActiveSpells":
									if (!string.IsNullOrEmpty((string)kvp.Value))
									{
										var spellsSplit = ((string)kvp.Value).Split(',');

										foreach (var spell in spellsSplit)
											identResponse.ActiveSpells.Add(int.Parse(spell));
									}

									break;

								case "Spells":
									if (!string.IsNullOrEmpty((string)kvp.Value))
									{
										var spellsSplit = ((string)kvp.Value).Split(',');

										foreach (var spell in spellsSplit)
											identResponse.Spells.Add(int.Parse(spell));
									}

									break;

								case "Attributes":
									{
										identResponse.ExtendIDAttributeInfo = new ExtendIDAttributeInfo();

										var values = (Dictionary<string, object>)kvp.Value;

										foreach (var kvp2 in values)
										{
											switch (kvp2.Key)
											{
												case "healthMax":
													identResponse.ExtendIDAttributeInfo.healthMax = uint.Parse((string)kvp2.Value);
													break;

												case "manaMax":
													identResponse.ExtendIDAttributeInfo.manaMax = uint.Parse((string)kvp2.Value);
													break;

												case "staminaMax":
													identResponse.ExtendIDAttributeInfo.staminaMax = uint.Parse((string)kvp2.Value);
													break;

												case "strength":
													identResponse.ExtendIDAttributeInfo.strength = uint.Parse((string)kvp2.Value);
													break;

												case "endurance":
													identResponse.ExtendIDAttributeInfo.endurance = uint.Parse((string)kvp2.Value);
													break;

												case "quickness":
													identResponse.ExtendIDAttributeInfo.quickness = uint.Parse((string)kvp2.Value);
													break;

												case "coordination":
													identResponse.ExtendIDAttributeInfo.coordination = uint.Parse((string)kvp2.Value);
													break;

												case "focus":
													identResponse.ExtendIDAttributeInfo.focus = uint.Parse((string)kvp2.Value);
													break;

												case "self":
													identResponse.ExtendIDAttributeInfo.self = uint.Parse((string)kvp2.Value);
													break;

												default:
													throw new NotImplementedException();
											}
										}
									}

									break;

								case "Resources":
									{
										var values = (Dictionary<string, object>)kvp.Value;

										foreach (var kvp2 in values)
										{
											var key = int.Parse(kvp2.Key);
											var value = int.Parse(kvp2.Value.ToString());

											identResponse.Resources[key] = value;
										}
									}

									break;

								default:
									throw new NotImplementedException();
							}
						}
					}
					catch
					{
						corruptLines++;
						continue;
					}
				}
				else
				{
					corruptLines++;
					continue;
				}
			}
		}

		class BaseLine
		{
			public DateTime Timestamp;

			public int Landcell;

			public Tuple<double, double, double> RawCoordinates;
		}

		class CreatePacket : BaseLine
		{
			public byte[] RawData;
		}

		class ExtendIDAttributeInfo
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

		class IdentResponse : BaseLine
		{
			public int Id;

			public ObjectClass ObjectClass;

			public Dictionary<int, bool> BoolValues = new Dictionary<int, bool>();
			public Dictionary<int, double> DoubleValues = new Dictionary<int, double>();
			public Dictionary<int, int> LongValues = new Dictionary<int, int>();
			public Dictionary<int, string> StringValues = new Dictionary<int, string>();

			public List<int> ActiveSpells = new List<int>();
			public List<int> Spells = new List<int>();

			/// <summary>
			/// Null if not present
			/// </summary>
			public ExtendIDAttributeInfo ExtendIDAttributeInfo;

			public Dictionary<int, int> Resources = new Dictionary<int, int>();
		}
	}
}
