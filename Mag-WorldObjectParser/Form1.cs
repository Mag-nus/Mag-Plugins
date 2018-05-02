using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

using Mag.Shared;
using Mag.Shared.Constants;

using Mag_WorldObjectParser.Properties;

namespace Mag_WorldObjectParser
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			this.Text += " " + Application.ProductVersion;

		    txtSourcePath.Text = (string)Settings.Default["SourceFolder"];

		    if (String.IsNullOrEmpty(txtSourcePath.Text))
		    {
		        var pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-WorldObjectLogger");
		        txtSourcePath.Text = pluginPersonalFolder.FullName;
		    }

		    typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
			dataGridView1.RowHeadersVisible = false;
			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridViewLongValueKeys, new object[] { true });
			dataGridViewLongValueKeys.RowHeadersVisible = false;
			dataGridViewLongValueKeys.AllowUserToAddRows = false;
			dataGridViewLongValueKeys.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridViewDoubleValueKeys, new object[] { true });
			dataGridViewDoubleValueKeys.RowHeadersVisible = false;
			dataGridViewDoubleValueKeys.AllowUserToAddRows = false;
			dataGridViewDoubleValueKeys.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridViewStringValueKeys, new object[] { true });
			dataGridViewStringValueKeys.RowHeadersVisible = false;
			dataGridViewStringValueKeys.AllowUserToAddRows = false;
			dataGridViewStringValueKeys.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
		}

		private void cmdBrowseForDifferentSource_Click(object sender, EventArgs e)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.SelectedPath = txtSourcePath.Text;

			    if (dialog.ShowDialog() == DialogResult.OK)
			    {
			        txtSourcePath.Text = dialog.SelectedPath;
			        Settings.Default["SourceFolder"] = txtSourcePath.Text;
			        Settings.Default.Save();
                }
			}
		}




	    private readonly object totalLinesLockObject = new object();
		private int totalLines;
		private int corruptLines;

	    private CancellationTokenSource cts = new CancellationTokenSource();

        private void cmdProcessAllFiles_Click(object sender, EventArgs e)
		{
            cmdBrowseForDifferentSource.Enabled = false;
		    cmdProcessAllFiles.Enabled = false;
            cmdStop.Enabled = true;

		    cts = new CancellationTokenSource();

            var startTime = DateTime.Now;

			OnBeforeLoadFiles();

			lblResults.Text = null;

			var files = Directory.GetFiles(txtSourcePath.Text, "*.csv", SearchOption.AllDirectories);

			totalLines = 0;
			corruptLines = 0;

		    ThreadPool.QueueUserWorkItem(o =>
		    {
		        int filesProcessed = 0;

		        Parallel.ForEach(files, file =>
		        {
		            if (cts.IsCancellationRequested)
		                return;

                    ProcessFile(file, cts.Token);

		            if (cts.IsCancellationRequested)
		                return;

                    var processed = Interlocked.Increment(ref filesProcessed);

		            progressBar1.BeginInvoke((Action)(() => progressBar1.Value = (int)(((double)processed / files.Length) * 100)));
		        });

		        BeginInvoke((Action)(() =>
                {
                    lblResults.Text = totalLines.ToString("N0") + " lines read. " + corruptLines.ToString("N0") + " corrupt lines found.";

                    OnLoadFilesComplete();

                    cmdBrowseForDifferentSource.Enabled = true;
                    cmdProcessAllFiles.Enabled = true;

                    //MessageBox.Show((DateTime.Now - startTime).TotalSeconds.ToString("N1"));
                }));
            });
		}

	    private void cmdStop_Click(object sender, EventArgs e)
	    {
	        cmdStop.Enabled = false;

	        cts.Cancel();
        }

        readonly JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

		private void ProcessFile(string fileName, CancellationToken ct)
		{
			var fileLines = File.ReadAllLines(fileName);

		    lock (totalLinesLockObject)
			    totalLines += fileLines.Length;

			foreach (var line in fileLines)
			{
			    if (ct.IsCancellationRequested)
			        return;

                // "Timestamp,Landcell,RawCoordinates,JSON"
                // "2017-01-23 15:25:42Z,"00000000",49.0488996505737 -29.9918003082275 -7.45058059692383E-09","{"RawData":"45F70000334020771100000003980100180001000C00000000003D00020000000C00000000000000B40104724BC8704135EFEFC1000000B2F70435BF0000000000000000F70435BF86000009220000202B000034DA0500020100020000000000000002000000000090010000300000000400446F6F720000A131171380000000141000002000000000000040"}"
                // "2017-01-23 15:25:42Z,"00000000",49.0488996505737 -29.9918003082275 -7.45058059692383E-09","{"Id":"1998602291","ObjectClass":"Door","BoolValues":{},"DoubleValues":{"167772167":"269.999963323784","167772168":"2"},"LongValues":{"19":" - 1","218103811":"1912865204","218103831":"2","218103835":"4116","218103843":"32","218103847":"104451","218103808":"12705","218103830":"33555930","218103832":"48","218103834":"128","218103809":"4887"},"StringValues":{"1":"Door"},"ActiveSpells":"","Spells":""}"

                try
				{
					var thirdComma = Util.IndexOfNth(line, ',', 3);

					if (thirdComma == -1) // Corrupt line
					{
					    Interlocked.Increment(ref corruptLines);
						continue;
					}

					var firstPart = line.Substring(0, thirdComma);

					var firstPartSplit = firstPart.Split(',');
					if (firstPartSplit[0] == "\"Timestamp\"") // Header line
						continue;

					DateTime timestamp;
					if (!DateTime.TryParse(firstPartSplit[0].Substring(1, firstPartSplit[0].Length - 2), out timestamp)) // Corrupt line
					{
					    Interlocked.Increment(ref corruptLines);
                        continue;
					}

					int landcell;
					if (!int.TryParse(firstPartSplit[1].Substring(1, firstPartSplit[1].Length - 2), NumberStyles.HexNumber, null, out landcell)) // Corrupt line
					{
					    Interlocked.Increment(ref corruptLines);
                        continue;
					}

					double x;
					double y;
					double z;
					var rawCoordinatesSplit = firstPartSplit[2].Substring(1, firstPartSplit[2].Length - 2).Split(' ');
					if (rawCoordinatesSplit.Length != 3) // Corrupt line
					{
					    Interlocked.Increment(ref corruptLines);
                        continue;
					}
					if (!double.TryParse(rawCoordinatesSplit[0], out x)) // Corrupt line
					{
					    Interlocked.Increment(ref corruptLines);
                        continue;
					}
					if (!double.TryParse(rawCoordinatesSplit[1], out y)) // Corrupt line
					{
					    Interlocked.Increment(ref corruptLines);
                        continue;
					}
					if (!double.TryParse(rawCoordinatesSplit[2], out z)) // Corrupt line
					{
					    Interlocked.Increment(ref corruptLines);
                        continue;
					}
					var rawCoordinates = new Tuple<double, double, double>(x, y, z);

					var jsonPart = line.Substring(thirdComma + 1, line.Length - (thirdComma + 1));
					if (jsonPart[0] != '"' || jsonPart[jsonPart.Length - 1] != '"') // Corrupt line
					{
					    Interlocked.Increment(ref corruptLines);
                        continue;
					}

					jsonPart = jsonPart.Substring(1, jsonPart.Length - 2); // Trim the quotes... why did I add them.. :(

					if (jsonPart.StartsWith("{\"Ra"))
					{
						var createPacket = new CreatePacket();

						createPacket.Timestamp = timestamp;
						createPacket.Landcell = landcell;
						createPacket.RawCoordinates = rawCoordinates;

						Dictionary<string, object> result = (Dictionary<string, object>) jsonSerializer.DeserializeObject(jsonPart);

						if (result.Count != 1)
						{
						    Interlocked.Increment(ref corruptLines);
                            continue;
						}

						foreach (var kvp in result)
							createPacket.RawData = Util.HexStringToByteArray((string) kvp.Value);

					    if (ct.IsCancellationRequested)
					        return;

                        if (!ProcessCreatePacket(createPacket))
						{
						    Interlocked.Increment(ref corruptLines);
                            continue;
						}
					}
					else if (jsonPart.StartsWith("{\"Id"))
					{
						bool retried = false;
						retry:

						try
						{
							var identResponse = new IdentResponse();

							identResponse.Timestamp = timestamp;
							identResponse.Landcell = landcell;
							identResponse.RawCoordinates = rawCoordinates;

							Dictionary<string, object> result = (Dictionary<string, object>) jsonSerializer.DeserializeObject(jsonPart);

							foreach (var kvp in result)
							{
								switch (kvp.Key)
								{
									case "Id":
										identResponse.Id = int.Parse((string) kvp.Value);
										break;

									case "ObjectClass":
										identResponse.ObjectClass = (ObjectClass) Enum.Parse(typeof (ObjectClass), (string) kvp.Value);
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
										var values = (Dictionary<string, object>) kvp.Value;

										foreach (var kvp2 in values)
										{
											var key = (IntValueKey)int.Parse(kvp2.Key);
											var value = int.Parse(kvp2.Value.ToString());

											identResponse.LongValues[key] = value;
										}
									}

										break;

									case "StringValues":
									{
										var values = (Dictionary<string, object>) kvp.Value;

										foreach (var kvp2 in values)
										{
											var key = int.Parse(kvp2.Key);

											identResponse.StringValues[key] = kvp2.Value.ToString();
										}
									}

										break;

									case "ActiveSpells":
										if (!string.IsNullOrEmpty((string) kvp.Value))
										{
											var spellsSplit = ((string) kvp.Value).Split(',');

											foreach (var spell in spellsSplit)
												identResponse.ActiveSpells.Add(int.Parse(spell));
										}

										break;

									case "Spells":
										if (!string.IsNullOrEmpty((string) kvp.Value))
										{
											var spellsSplit = ((string) kvp.Value).Split(',');

											foreach (var spell in spellsSplit)
												identResponse.Spells.Add(int.Parse(spell));
										}

										break;

									case "Attributes":
									{
										identResponse.ExtendIDAttributeInfo = new ExtendIDAttributeInfo();

										var values = (Dictionary<string, object>) kvp.Value;

										foreach (var kvp2 in values)
										{
											switch (kvp2.Key)
											{
												case "healthMax":
													identResponse.ExtendIDAttributeInfo.healthMax = uint.Parse((string) kvp2.Value);
													break;

												case "manaMax":
													identResponse.ExtendIDAttributeInfo.manaMax = uint.Parse((string) kvp2.Value);
													break;

												case "staminaMax":
													identResponse.ExtendIDAttributeInfo.staminaMax = uint.Parse((string) kvp2.Value);
													break;

												case "strength":
													identResponse.ExtendIDAttributeInfo.strength = uint.Parse((string) kvp2.Value);
													break;

												case "endurance":
													identResponse.ExtendIDAttributeInfo.endurance = uint.Parse((string) kvp2.Value);
													break;

												case "quickness":
													identResponse.ExtendIDAttributeInfo.quickness = uint.Parse((string) kvp2.Value);
													break;

												case "coordination":
													identResponse.ExtendIDAttributeInfo.coordination = uint.Parse((string) kvp2.Value);
													break;

												case "focus":
													identResponse.ExtendIDAttributeInfo.focus = uint.Parse((string) kvp2.Value);
													break;

												case "self":
													identResponse.ExtendIDAttributeInfo.self = uint.Parse((string) kvp2.Value);
													break;

												default:
													throw new NotImplementedException();
											}
										}
									}

										break;

									case "Resources":
									{
										var values = (Dictionary<string, object>) kvp.Value;

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

						    if (ct.IsCancellationRequested)
						        return;

                            if (!ProcessIdentResponse(identResponse))
							{
							    Interlocked.Increment(ref corruptLines);
                                continue;
							}
						}
						catch
						{
							// This hacky retry method is about 25% faster
							if (!retried)
							{
								retried = true;

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

								goto retry;
							}

						    Interlocked.Increment(ref corruptLines);
                            continue;
						}
					}
					else
					{
					    Interlocked.Increment(ref corruptLines);
                        continue;
					}
				}
				catch
				{
				    Interlocked.Increment(ref corruptLines);
                    continue;
				}
			}
		}




	    private readonly Object processLockObject = new Object();

		private readonly Dictionary<IntValueKey, int> longValueKeysFound = new Dictionary<IntValueKey, int>();
		private readonly Dictionary<int, int> doubleValueKeysFound = new Dictionary<int, int>();
		private readonly Dictionary<int, int> stringValueKeysFound = new Dictionary<int, int>();

		/// <summary>
		/// Dictionary of creatures, and their identified attribute variants, and the count for each variant
		/// </summary>
		private readonly Dictionary<int, List<CreatureInfo>> creatureAttributes = new Dictionary<int, List<CreatureInfo>>();

		private void OnBeforeLoadFiles()
		{
			longValueKeysFound.Clear();
			doubleValueKeysFound.Clear();
			stringValueKeysFound.Clear();

			creatureAttributes.Clear();

			dataGridView1.DataSource = null;
			dataGridViewLongValueKeys.DataSource = null;
			dataGridViewDoubleValueKeys.DataSource = null;
			dataGridViewStringValueKeys.DataSource = null;
		}

		private bool ProcessCreatePacket(CreatePacket createPacket)
		{
		    lock (processLockObject)
		    {
                // todo

		        return true;
		    }
		}

		private bool ProcessIdentResponse(IdentResponse identResponse)
		{
		    lock (processLockObject)
		    {
		        foreach (var key in identResponse.LongValues)
		        {
		            if (!longValueKeysFound.ContainsKey(key.Key))
		                longValueKeysFound[key.Key] = 1;
		            else
		                longValueKeysFound[key.Key] = longValueKeysFound[key.Key] + 1;
		        }

		        foreach (var key in identResponse.DoubleValues)
		        {
		            if (!doubleValueKeysFound.ContainsKey(key.Key))
		                doubleValueKeysFound[key.Key] = 1;
		            else
		                doubleValueKeysFound[key.Key] = doubleValueKeysFound[key.Key] + 1;
		        }

		        foreach (var key in identResponse.StringValues)
		        {
		            if (!stringValueKeysFound.ContainsKey(key.Key))
		                stringValueKeysFound[key.Key] = 1;
		            else
		                stringValueKeysFound[key.Key] = stringValueKeysFound[key.Key] + 1;
		        }

		        if (identResponse.ExtendIDAttributeInfo != null)
		        {
		            if (!identResponse.LongValues.ContainsKey(IntValueKey.Type)) // Type. Everything should have this
		                return false;

		            if (!identResponse.StringValues.ContainsKey((int) StringValueKey.Name)
		            ) // Name. Everything should have this
		                return false;

		            List<CreatureInfo> creatureInfos;

		            if (!creatureAttributes.ContainsKey(identResponse.LongValues[IntValueKey.Type]))
		            {
		                creatureInfos = new List<CreatureInfo>();
		                creatureAttributes[identResponse.LongValues[IntValueKey.Type]] = creatureInfos;
		            }
		            else
		                creatureInfos = creatureAttributes[identResponse.LongValues[IntValueKey.Type]];

		            /*if (!creatureAttributes.ContainsKey(identResponse.StringValues[1]))
                    {
                        creatureInfos = new List<CreatureInfo>();
                        creatureAttributes[identResponse.StringValues[1]] = creatureInfos;
                    }
                    else
                        creatureInfos = creatureAttributes[identResponse.StringValues[1]];*/

		            CreatureInfo creatureInfo = new CreatureInfo();

		            creatureInfo.Name = identResponse.StringValues[1];

		            if (identResponse.LongValues.ContainsKey(IntValueKey.CreatureLevel))
		                creatureInfo.Level = identResponse.LongValues[IntValueKey.CreatureLevel];

		            creatureInfo.healthMax = identResponse.ExtendIDAttributeInfo.healthMax;
		            creatureInfo.staminaMax = identResponse.ExtendIDAttributeInfo.staminaMax;
		            creatureInfo.manaMax = identResponse.ExtendIDAttributeInfo.manaMax;
		            creatureInfo.strength = identResponse.ExtendIDAttributeInfo.strength;
		            creatureInfo.endurance = identResponse.ExtendIDAttributeInfo.endurance;
		            creatureInfo.quickness = identResponse.ExtendIDAttributeInfo.quickness;
		            creatureInfo.coordination = identResponse.ExtendIDAttributeInfo.coordination;
		            creatureInfo.focus = identResponse.ExtendIDAttributeInfo.focus;
		            creatureInfo.self = identResponse.ExtendIDAttributeInfo.self;

		            for (int i = 0; i <= creatureInfos.Count; i++)
		            {
		                if (i == creatureInfos.Count)
		                {
                            // todo: This is currently disabled because its broken by pets. The same pet might be named Bob's Pet Dragon and Sally's Pet Dragon.
		                    //if (i > 0 && creatureInfos[0].Name != creatureInfo.Name)
		                    //	MessageBox.Show("This shouldn't happen ");

		                    creatureInfo.Count = 1;
		                    creatureInfo.Landcell.Add(identResponse.Landcell);
		                    creatureInfos.Add(creatureInfo);
		                    break;
		                }

		                if (creatureInfos[i].Equals(creatureInfo))
		                {
		                    creatureInfos[i].Count++;

		                    if (!creatureInfos[i].Landcell.Contains(identResponse.Landcell))
		                        creatureInfos[i].Landcell.Add(identResponse.Landcell);

		                    break;
		                }
		            }
		        }

		        return true;
		    }
		}

		private void OnLoadFilesComplete()
		{
			var dt = new DataTable();

			dt.Columns.Add("Type", typeof(int));
			dt.Columns.Add("Count", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			dt.Columns.Add("Level", typeof(int));

			dt.Columns.Add("healthMax", typeof(int));
			dt.Columns.Add("staminaMax", typeof(int));
			dt.Columns.Add("manaMax", typeof(int));

			dt.Columns.Add("strength", typeof(int));
			dt.Columns.Add("endurance", typeof(int));
			dt.Columns.Add("quickness", typeof(int));
			dt.Columns.Add("coordination", typeof(int));
			dt.Columns.Add("focus", typeof(int));
			dt.Columns.Add("self", typeof(int));

			foreach (var kvp in creatureAttributes)
			{
				bool hasExtendedAttributeInfo = false;

				foreach (var creatureInfo in kvp.Value)
				{
					if (creatureInfo.strength != 0)
						hasExtendedAttributeInfo = true;
				}

				foreach (var creatureInfo in kvp.Value)
				{
					if (hasExtendedAttributeInfo && creatureInfo.strength == 0)
						continue;

					var dr = dt.NewRow();

					dr["Type"] = kvp.Key;
					dr["Count"] = creatureInfo.Count;
					dr["Name"] = creatureInfo.Name;
					dr["Level"] = creatureInfo.Level;

					dr["healthMax"] = creatureInfo.healthMax;
					dr["staminaMax"] = creatureInfo.staminaMax;
					dr["manaMax"] = creatureInfo.manaMax;

					dr["strength"] = creatureInfo.strength;
					dr["endurance"] = creatureInfo.endurance;
					dr["quickness"] = creatureInfo.quickness;
					dr["coordination"] = creatureInfo.coordination;
					dr["focus"] = creatureInfo.focus;
					dr["self"] = creatureInfo.self;

					dt.Rows.Add(dr);
				}
			}

			dataGridView1.DataSource = dt;

			for (int i = 0; i < dataGridView1.Columns.Count; i++)
			{
				if (i == 2)
					continue;

				dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			}

			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dataGridView1.AutoResizeColumns();


			dt = new DataTable();

			dt.Columns.Add("Key", typeof(int));
			dt.Columns.Add("Desc", typeof(string));
			dt.Columns.Add("Hits", typeof(int));

			foreach (var kvp in longValueKeysFound)
			{
				var dr = dt.NewRow();

				dr["Key"] = kvp.Key;
				dr["Desc"] = (IntValueKey)kvp.Key;
				dr["Hits"] = kvp.Value;

				dt.Rows.Add(dr);
			}

			dataGridViewLongValueKeys.DataSource = dt;

			for (int i = 0; i < dataGridViewLongValueKeys.Columns.Count; i++)
			{
				if (i == 1) continue;
				dataGridViewLongValueKeys.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			}

			dataGridViewLongValueKeys.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dataGridViewLongValueKeys.AutoResizeColumns();
			dataGridViewLongValueKeys.Sort(dataGridViewLongValueKeys.Columns[0], System.ComponentModel.ListSortDirection.Ascending);


			dt = new DataTable();

			dt.Columns.Add("Key", typeof(int));
			dt.Columns.Add("Desc", typeof(string));
			dt.Columns.Add("Hits", typeof(int));

			foreach (var kvp in doubleValueKeysFound)
			{
				var dr = dt.NewRow();

				dr["Key"] = kvp.Key;
				dr["Desc"] = (DoubleValueKey)kvp.Key;
				dr["Hits"] = kvp.Value;

				dt.Rows.Add(dr);
			}

			dataGridViewDoubleValueKeys.DataSource = dt;

			for (int i = 0; i < dataGridViewDoubleValueKeys.Columns.Count; i++)
			{
				if (i == 1) continue;
				dataGridViewDoubleValueKeys.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			}

			dataGridViewDoubleValueKeys.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dataGridViewDoubleValueKeys.AutoResizeColumns();
			dataGridViewDoubleValueKeys.Sort(dataGridViewDoubleValueKeys.Columns[0], System.ComponentModel.ListSortDirection.Ascending);


			dt = new DataTable();

			dt.Columns.Add("Key", typeof(int));
			dt.Columns.Add("Desc", typeof(string));
			dt.Columns.Add("Hits", typeof(int));

			foreach (var kvp in stringValueKeysFound)
			{
				var dr = dt.NewRow();

				dr["Key"] = kvp.Key;
				dr["Desc"] = (StringValueKey)kvp.Key;
				dr["Hits"] = kvp.Value;

				dt.Rows.Add(dr);
			}

			dataGridViewStringValueKeys.DataSource = dt;

			for (int i = 0; i < dataGridViewStringValueKeys.Columns.Count; i++)
			{
				if (i == 1) continue;
				dataGridViewStringValueKeys.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			}

			dataGridViewStringValueKeys.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dataGridViewStringValueKeys.AutoResizeColumns();
			dataGridViewStringValueKeys.Sort(dataGridViewStringValueKeys.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
		}
    }
}
