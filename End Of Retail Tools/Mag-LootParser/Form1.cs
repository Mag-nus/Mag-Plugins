using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mag.Shared.Constants;

using Mag_LootParser.Properties;

namespace Mag_LootParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

			this.Text += " " + Application.ProductVersion;

            txtSourcePath.Text = (string)Settings.Default["SourceFolder"];
            txtOutputPath.Text = (string)Settings.Default["OutputFolder"];

            if (String.IsNullOrEmpty(txtSourcePath.Text))
            {
                var pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-LootLogger");
                txtSourcePath.Text = pluginPersonalFolder.FullName;
            }
        }

        private void cmdBrowseForDifferentSource_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = txtSourcePath.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtSourcePath.Text = dialog.SelectedPath;
                    Settings.Default["SourceFolder"] = dialog.SelectedPath;
                    Settings.Default.Save();
                }
            }
        }

        private void cmdBrowseForDifferentOutput_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = txtOutputPath.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = dialog.SelectedPath;
                    Settings.Default["OutputFolder"] = dialog.SelectedPath;
                    Settings.Default.Save();
                }
            }
        }




        private readonly object totalLinesLockObject = new object();
        private int totalLines;
        private int skippedLines;
        private int corruptLines;

        private CancellationTokenSource cts;

        private void cmdProcessAllFiles_Click(object sender, EventArgs e)
        {
            txtSourcePath.Enabled = false;
            cmdBrowseForDifferentSource.Enabled = false;
            txtOutputPath.Enabled = false;
            cmdBrowseForDifferentOutput.Enabled = false;
            cmdProcessAllFiles.Enabled = false;
            cmdStop.Enabled = true;

			cts = new CancellationTokenSource();

            var startTime = DateTime.Now;

            OnBeforeLoadFiles();

            lblResults.Text = null;
            lblTime.Text = null;

            // Clear our outputs
            dataGridView1.DataSource = null;

            var files = Directory.GetFiles(txtSourcePath.Text, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".csv") || s.EndsWith(".json")).ToList();

			totalLines = 0;
            corruptLines = 0;
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

                    progressBar1.BeginInvoke((Action)(() => progressBar1.Value = (int)(((double)processed / files.Count) * 100)));
                });

                BeginInvoke((Action)(() =>
                {
                    cts.Dispose();

                    lblResults.Text = totalLines.ToString("N0") + " lines read. " + skippedLines.ToString("N0") + " lines skipped. " + corruptLines.ToString("N0") + " corrupt lines found.";
                    lblTime.Text = "Total Seconds: " + (DateTime.Now - startTime).TotalSeconds.ToString("N1");

                    OnLoadFilesComplete();

                    txtSourcePath.Enabled = true;
                    cmdBrowseForDifferentSource.Enabled = true;
                    txtOutputPath.Enabled = true;
                    cmdBrowseForDifferentOutput.Enabled = true;
                    cmdProcessAllFiles.Enabled = true;
                    cmdStop.Enabled = false;
                }));
            });
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            cmdStop.Enabled = false;

            cts.Cancel();
        }

        private enum FileType
        {
			Unknown,
			Mag_LootLogger,
			ACE,
        }

        private void ProcessFile(string fileName, CancellationToken ct)
        {
	        if (fileName.EndsWith(".json"))
	        {
		        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
				var fileText = File.ReadAllText(fileName);

		        var containers = JsonSerializer.Deserialize<List<ContainerInfo>>(fileText);

		        lock (processLockObject)
					containersLoot[fileNameWithoutExtension] = containers;

		        return;
	        }


            var fileLines = File.ReadAllLines(fileName);

            FileType fileType = FileType.Unknown;

			// "Timestamp","ContainerName","ContainerID","LandCell","Location","JSON"
			if (fileLines.Length < 2 || fileLines[0] != "\"Timestamp\",\"ContainerName\",\"ContainerID\",\"LandCell\",\"Location\",\"JSON\"")
			{
				if (fileLines[0].StartsWith("DID ") && fileLines[0].EndsWith("}"))
					fileType = FileType.ACE;
			}
			else
				fileType = FileType.Mag_LootLogger;

			if (fileType == FileType.Unknown)
				return;

            lock (totalLinesLockObject)
                totalLines += fileLines.Length;

            for (int i = 0; i < fileLines.Length; i++)
            {
                if (ct.IsCancellationRequested)
                    return;

                try
                {
                    var line = fileLines[i];

                    if (fileType == FileType.Mag_LootLogger)
                    {
	                    // "2017-01-20 08:18:48Z,"Monty's Golden Chest",2056986702,"A9B20117","40.8N, 33.6E","{"Id":"-1419165865","ObjectClass":"Misc","BoolValues":{"69":"True"},"DoubleValues":{"167":"45","167772170":"0"},"LongValues":{"367":"430","375":"7","218103835":"67108882","218103843":"8","280":"213","33":"0","105":"7","369":"115","114":"0","366":"54","374":"13","218103810":"2056986634","218103826":"16","218103830":"33554817","218103834":"128","218103850":"29728","19":"7000","91":"50","218103831":"48","218103847":"137345","92":"50","218103808":"49548","218103824":"64","218103832":"1076382872","5":"50","218103809":"4154","218103833":"7","218103849":"29733"},"StringValues":{"1":"Lightning Phyntos Wasp Essence (125)","14":"Use this essence to summon or dismiss your Lightning Phyntos Wasp."},"ActiveSpells":"","Spells":""}"
	                    // "2017-01-20 08:18:48Z,"Monty's Golden Chest",2056986702,"A9B20117","40.8N, 33.6E","{"Id":"-1419511451","ObjectClass":"Scroll","BoolValues":{},"DoubleValues":{"167772170":"0"},"LongValues":{"19":"2000","218103831":"16","218103835":"18","218103843":"8","218103847":"135297","218103808":"45258","218103816":"5785","218103832":"6307864","218103848":"0","5":"30","218103809":"28959","218103810":"2056986634","218103830":"33554826","218103834":"8192","218103838":"1"},"StringValues":{"1":"Scroll of Dirty Fighting Mastery Self VII","14":"Use this item to attempt to learn its spell.","16":"Inscribed spell: Dirty Fighting Mastery Self VII Increases the caster's Dirty Fighting skill by 40 points."},"ActiveSpells":"","Spells":"5785"}"
	                    // "2017-01-15 04:02:33Z,"Corpse of Mu-miyah Sentinel",-1851166334,"8764003C","21.6S, 6.8E","{"Id":"-1851166529","ObjectClass":"Money","BoolValues":{},"DoubleValues":{},"LongValues":{"19":"4775","218103815":"25000","218103831":"1","218103835":"16","218103843":"1","218103847":"131073","218103808":"273","218103832":"28696","5":"0","218103809":"8863","218103810":"-1851166402","218103814":"4775","218103830":"33557367","218103834":"64"},"StringValues":{"1":"Pyreal"},"ActiveSpells":"","Spells":""}"

	                    // "2017-01-19 09:03:44Z,"Corpse of Gigas Raider",-1419920476,"02E70109","81.3N, 99.2W","{"Id":"-1419521241","ObjectClass":"Scroll","BoolValues":{},"DoubleValues":{"167772170":"0"},"LongValues":{"19":"100","218103831":"16","218103835":"18","218103843":"8","218103847":"135297","218103808":"3060","218103816":"1087","218103832":"6307864","218103848":"0","5":"30","218103809":"13652","218103810":"-1419920544","218103830":"33554826","218103834":"8192","218103838":"1"},"StringValues":{"1":"Scroll of Lightning Vulnerability Other IV","14":"Use this item to attempt to learn its spell.","16":"Inscribed spell: Lightning Vulnerability Other IV
	                    // Increases damage the target takes from Lightning by 75%."},"ActiveSpells":"","Spells":"1087"}"
	                    if (i < fileLines.Length - 1)
	                    {
		                    var nextLine = fileLines[i + 1];

		                    var thisLineIsTerminated = line.EndsWith("}\"");
		                    var nextLineHasProperStart = nextLine.StartsWith("\"") && !nextLine.StartsWith("\",");

		                    if (!thisLineIsTerminated && !nextLineHasProperStart)
		                    {
			                    line += '\n' + nextLine;
			                    i++;

			                    // "2017-01-31 11:39:31Z,"Corpse of Cyberkiller",-583852276,"C4A80102","32.6N, 55.0E","{"Id":"-583994245","ObjectClass":"Armor","BoolValues":{"100":"True"},"DoubleValues":{"167772160":"1.29999995231628","167772164":"0.341241389513016","167772161":"1","167772163":"0.600000023841858","167772165":"0.400000005960464","167772169":"3","5":"-0.0333333350718021","167772162":"1","167772166":"0.400000005960464"},"LongValues":{"218103835":"18","218103843":"1","105":"3","218103821":"15360","106":"194","110":"0","218103810":"-583852344","218103822":"7680","218103830":"33554644","218103834":"2","218103838":"3","19":"13665","107":"123","115":"0","131":"60","218103831":"16","218103847":"137217","28":"146","108":"441","218103808":"72","218103824":"1","218103832":"-2128265064","218103848":"0","5":"4126","109":"194","218103809":"6300"},"StringValues":{"1":"Platemail Hauberk","7":"AL 146
			                    // Imp III, Acid Bane II, Frost Bane IV
			                    // 1/29, Diff 194","8":"Shai'tan","16":"Finely crafted Gold Platemail Hauberk , set with 4 Peridots"},"ActiveSpells":"","Spells":"1483,1494,1526"}"
			                    if (i < fileLines.Length - 1)
			                    {
				                    nextLine = fileLines[i + 1];

				                    thisLineIsTerminated = line.EndsWith("}\"");
				                    nextLineHasProperStart = nextLine.StartsWith("\"");

				                    if (!thisLineIsTerminated && !nextLineHasProperStart)
				                    {
					                    line += '\n' + nextLine;
					                    i++;
				                    }
			                    }
		                    }
	                    }

	                    var sixthComma = Util.IndexOfNth(line, ',', 6);

	                    if (sixthComma == -1) // Corrupt line
	                    {
		                    if (line != "\"Timestamp\",\"ContainerName\",\"ContainerID\",\"LandCell\",\"Location\",\"JSON\"")
			                    Interlocked.Increment(ref corruptLines);
		                    continue;
	                    }

	                    var firstPart = line.Substring(0, sixthComma);

	                    var firstPartSplit = firstPart.Split(',');
	                    if (firstPartSplit[0] == "\"Timestamp\"") // Header line
		                    continue;

	                    if (!DateTime.TryParse(firstPartSplit[0].Substring(1, firstPartSplit[0].Length - 2), out var timestamp)) // Corrupt line
	                    {
		                    Interlocked.Increment(ref corruptLines);
		                    continue;
	                    }

	                    var containerName = firstPartSplit[1].Substring(1, firstPartSplit[1].Length - 2);

	                    var containerID = (uint) int.Parse(firstPartSplit[2].Substring(2, firstPartSplit[2].Length - 2));

	                    if (!int.TryParse(firstPartSplit[3].Substring(1, firstPartSplit[3].Length - 2), NumberStyles.HexNumber, null, out var landcell)) // Corrupt line
	                    {
		                    Interlocked.Increment(ref corruptLines);
		                    continue;
	                    }

	                    var location = firstPartSplit[4].Substring(1) + ", " + firstPartSplit[5].Substring(0, firstPartSplit[5].Length - 1);

	                    var jsonPart = line.Substring(sixthComma + 1, line.Length - (sixthComma + 1));
	                    if (jsonPart[0] != '"' || jsonPart[jsonPart.Length - 1] != '"') // Corrupt line
	                    {
		                    Interlocked.Increment(ref corruptLines);
		                    continue;
	                    }

	                    jsonPart = jsonPart.Substring(1, jsonPart.Length - 2); // Trim the quotes... why did I add them.. :(

						if (jsonPart.StartsWith("{\"Id"))
						{
							var identResponse = new IdentResponse();

							identResponse.Timestamp = timestamp;
							identResponse.Landcell = landcell;
							//identResponse.RawCoordinates = rawCoordinates;

							// This is because I forgot to encode strings...
							jsonPart = jsonPart.Replace("\r\n", "\\r\\n");

							jsonPart = jsonPart.Replace("\n", "\\n");

							jsonPart = jsonPart.Replace("\"Enchanted\"", "\\\"Enchanted\\\"");
							jsonPart = jsonPart.Replace("\"Bunny Master\"", "\\\"Bunny Master\\\"");
							jsonPart = jsonPart.Replace("\"Samuel\"", "\\\"Samuel\\\"");

							jsonPart = jsonPart.Replace("\"Is that what I think it is?\"", "\\\"Is that what I think it is?\\\"");

							jsonPart = jsonPart.Replace("\"Procedures By", "\\\"Procedures By");
							jsonPart = jsonPart.Replace("The Creeping Blight.\"", "The Creeping Blight.\\\"");

							var result = JsonSerializer.Deserialize<ExpandoObject>(jsonPart);

							identResponse.ParseFromDictionary(result);

							if (ct.IsCancellationRequested)
								return;

							ProcessLootItemWithChecks(containerID, containerName, landcell, location, identResponse);
						}
						else
						{
		                    Interlocked.Increment(ref corruptLines);
		                    continue;
	                    }
                    }
					else if (fileType == FileType.ACE)
                    {
	                    var identResponse = new IdentResponse();

	                    //identResponse.Timestamp = timestamp;
	                    //identResponse.Landcell = landcell;
	                    //identResponse.RawCoordinates = rawCoordinates;

	                    var indexOf = line.IndexOf('{');

						var firstPart = line.Substring(0, line.IndexOf('{'));
						var secondPart = line.Substring(indexOf, line.Length - indexOf);

						var firstPartSplit = firstPart.Split(',');
						string containerName = firstPartSplit[0];
						uint containerID = uint.Parse(firstPartSplit[1]);

						var biota = JsonSerializer.Deserialize<ACE.Entity.Models.Biota>(secondPart);

	                    identResponse.ParseFromBiota(biota);

	                    if (ct.IsCancellationRequested)
		                    return;

						int landcell = 0;
						string location = null;

						ProcessLootItemNoChecks(containerID, containerName, landcell, location, identResponse);
					}
                }
                catch // Item is likely inscribed
                {
                    Interlocked.Increment(ref corruptLines);
                    continue;
                }
            }
        }




        private readonly Object processLockObject = new Object();

        private readonly Dictionary<string, List<ContainerInfo>> containersLoot = new Dictionary<string, List<ContainerInfo>>();

        private void OnBeforeLoadFiles()
        {
            containersLoot.Clear();
        }

        private void ProcessLootItemWithChecks(uint containerID, string containerName, int landcell, string location, IdentResponse identResponse)
        {
            // Player Corpses
            if (containerName == "Corpse of Father Of Sin" ||
                containerName == "Corpse of Copastetic" ||
                containerName == "Corpse of Blumenkind" ||
                containerName == "Corpse of Cyberkiller" ||
                containerName == "Corpse of Sholdslastridelc" ||
                containerName == "Corpse of Thisistheendmyonlyfriendtheend" ||
				containerName == "Corpse of Dieter the Black" ||
                containerName == "Corpse of Twangs" ||
                containerName == "Corpse of Big D Boy" ||
	            containerName == "Corpse of Blood Quench" ||
				containerName == "Corpse of Ripley of Frostfell" ||
                containerName == "Corpse of Al Bow" ||
                containerName == "Corpse of Ashly g" ||
                containerName == "Corpse of Zojak Quazith" ||
                containerName == "Corpse of Crimson Mage of Mt" ||
                containerName == "Corpse of Mr Goodman's Goodies" ||
                containerName == "Corpse of Mag-nus" ||
                containerName == "Corpse of Dirty Mike" ||
                containerName == "Corpse of Shootey McWarwand" ||
                containerName == "Corpse of Sinner of Dt" ||
                containerName == "Corpse of Rimok" ||
                containerName == "Corpse of Ujiio")

			{
                Interlocked.Increment(ref skippedLines);
                return;
            }

            // Housing containers can contain anything
            if (containerName == "Chest")
            {
                Interlocked.Increment(ref skippedLines);
                return;
            }

            // These landscape containers seem to be multi-tier
            if (containerName == "Runed Chest" ||
                containerName == "Coral Encrusted Chest")
            {
                Interlocked.Increment(ref skippedLines);
                return;
            }

            // These items are odd-balls that were found in corpses. Possibly code error, or maybe a player put the item in the corpse?
            // Corpse of Grave Rat
            if (identResponse.Id == 0xDD2B79A9 && identResponse.StringValues[StringValueKey.Name] == "Electric Spine Glaive")
            {
                Interlocked.Increment(ref skippedLines);
                return;
            }
            // Corpse of Drudge Slave
            if (identResponse.Id == 0xDC94F88A && identResponse.StringValues[StringValueKey.Name] == "Heavy Crossbow")
            {
                Interlocked.Increment(ref skippedLines);
                return;
            }
            if (identResponse.Id == 0xDC9AE6E2 && identResponse.StringValues[StringValueKey.Name] == "Katar")
            {
                Interlocked.Increment(ref skippedLines);
                return;
            }
            if (identResponse.Id == 0xD80AB491 && identResponse.StringValues[StringValueKey.Name] == "Flaming Great Star Mace")
            {
	            Interlocked.Increment(ref skippedLines);
	            return;
            }
			// Corpse of Mercenary
			if (identResponse.Id == 0xABCC0A35 && identResponse.StringValues[StringValueKey.Name] == "Studded Leather Breastplate")
            {
                Interlocked.Increment(ref skippedLines);
                return;
            }
			// Corpse of Mahharu
			if (identResponse.Id == 0xAAFFFDEB && identResponse.StringValues[StringValueKey.Name] == "War Axe")
			{
				Interlocked.Increment(ref skippedLines);
				return;
			}

            // Not sure why corpses are being detected as inside a container, probably a data bug
            if (identResponse.ObjectClass == Mag.Shared.ObjectClass.Corpse)
            {
                Interlocked.Increment(ref skippedLines);
                return;
            }

            ProcessLootItemNoChecks(containerID, containerName, landcell, location, identResponse);
        }

		private void ProcessLootItemNoChecks(uint containerID, string containerName, int landcell, string location, IdentResponse identResponse)
		{
			lock (processLockObject)
			{
				List<ContainerInfo> containers;

				if (containersLoot.ContainsKey(containerName))
					containers = containersLoot[containerName];
				else
				{
					containers = new List<ContainerInfo>();
					containersLoot.Add(containerName, containers);
				}

				ContainerInfo containerInfo = null;

				foreach (var container in containers)
				{
					if (container.Id == containerID && container.Name == containerName && container.Landcell == landcell && container.Location == location)
					{
						containerInfo = container;
						break;
					}
				}

				if (containerInfo == null)
				{
					containerInfo = new ContainerInfo { Id = containerID, Name = containerName, Landcell = landcell, Location = location };
					containers.Add(containerInfo);
				}

				// Does this item already exist?
				foreach (var item in containerInfo.Items)
				{
					if (item.Id == identResponse.Id)
						return;
				}

				containerInfo.Items.Add(identResponse);
			}
		}

		private void OnLoadFilesComplete()
		{
			if (chkGeneratecontainersLootJson.Checked)
			{
				var workignOutputJsonFolder = Path.Combine(txtOutputPath.Text, DateTime.Now.ToString("yyyy-MM-dd HH-mm") + " containers json");

				if (!Directory.Exists(workignOutputJsonFolder))
					Directory.CreateDirectory(workignOutputJsonFolder);

				foreach (var kvp in containersLoot)
				{
					var containersLootJson = JsonSerializer.Serialize(kvp.Value);
					File.WriteAllText(Path.Combine(workignOutputJsonFolder, $"{kvp.Key}.json"), containersLootJson);
				}

				return;
			}


			var workignOutputFolder = Path.Combine(txtOutputPath.Text, DateTime.Now.ToString("yyyy-MM-dd HH-mm"));

			if (!Directory.Exists(workignOutputFolder))
				Directory.CreateDirectory(workignOutputFolder);


			// Calculate the stats
			StatsCalculator.Calculate(containersLoot);


            // Populate the Containers tab
            var dt = new DataTable();

            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Tier", typeof(int));
            dt.Columns.Add("Hits", typeof(int));
            dt.Columns.Add("Average Items", typeof(float));
            dt.Columns.Add("Total Items", typeof(int));

            foreach (var stats in StatsCalculator.StatsByContainerNameAndTier)
            {
                var dr = dt.NewRow();

                dr["Name"] = stats.ContainerName;
                dr["Tier"] = stats.Tier;
                dr["Hits"] = stats.TotalContainers;

                dr["Average Items"] = stats.TotalItems / (float)stats.TotalContainers;
                dr["Total Items"] = stats.TotalItems;

                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["Hits"].DefaultCellStyle.Format = "N0";
            dataGridView1.Columns["Average Items"].DefaultCellStyle.Format = "0.0";
            dataGridView1.Columns["Total Items"].DefaultCellStyle.Format = "N0";

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (i == 0)
                    continue;

                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoResizeColumns();


            // Output stats by tier
            foreach (var kvp in StatsCalculator.StatsByLootTier)
                File.WriteAllText(Path.Combine(workignOutputFolder, "Tier " + kvp.Key + " (No Chests).txt"), kvp.Value.ToString());

            // Output stats by container name
            foreach (var stats in StatsCalculator.StatsByContainerNameAndTier)
                File.WriteAllText(Path.Combine(workignOutputFolder, "Container " + stats.ContainerName + $" (T{stats.Tier}).txt"), stats.ToString());


            // Audit all the containers for anomolies
            File.Delete(Path.Combine(workignOutputFolder, "Tier Container Audit.txt"));
            foreach (var stats in StatsCalculator.StatsByContainerNameAndTier)
            {
                // Chests of the same name can contain multiple tiers
				if (ContainerInfo.IsChest(stats.ContainerName))
					continue;

	            outputAuditLine = false;

                ContainerTierAuditSpells(stats, 1, Mag.Shared.Spells.Spell.BuffLevels.I, Mag.Shared.Spells.Spell.BuffLevels.III);
                ContainerTierAuditSpells(stats, 2, Mag.Shared.Spells.Spell.BuffLevels.III, Mag.Shared.Spells.Spell.BuffLevels.V);
                ContainerTierAuditSpells(stats, 3, Mag.Shared.Spells.Spell.BuffLevels.IV, Mag.Shared.Spells.Spell.BuffLevels.VI);
                ContainerTierAuditSpells(stats, 4, Mag.Shared.Spells.Spell.BuffLevels.IV, Mag.Shared.Spells.Spell.BuffLevels.VI);
                ContainerTierAuditSpells(stats, 5, Mag.Shared.Spells.Spell.BuffLevels.V, Mag.Shared.Spells.Spell.BuffLevels.VII);
                ContainerTierAuditSpells(stats, 6, Mag.Shared.Spells.Spell.BuffLevels.VI, Mag.Shared.Spells.Spell.BuffLevels.VII);
                ContainerTierAuditSpells(stats, 7, Mag.Shared.Spells.Spell.BuffLevels.VI, Mag.Shared.Spells.Spell.BuffLevels.VIII);
                ContainerTierAuditSpells(stats, 8, Mag.Shared.Spells.Spell.BuffLevels.VI, Mag.Shared.Spells.Spell.BuffLevels.VIII);

                ContainerTierAuditWieldReqs(stats, 1, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.TwoHandedCombat, (int)Skill.HeavyWeapons, (int)Skill.LightWeapons, (int)Skill.FinesseWeapons }, new HashSet<int> { 0 });
				ContainerTierAuditWieldReqs(stats, 2, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.TwoHandedCombat, (int)Skill.HeavyWeapons, (int)Skill.LightWeapons, (int)Skill.FinesseWeapons }, new HashSet<int> { 0, 250 });
                ContainerTierAuditWieldReqs(stats, 3, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.TwoHandedCombat, (int)Skill.HeavyWeapons, (int)Skill.LightWeapons, (int)Skill.FinesseWeapons }, new HashSet<int> { 0, 250, 300 });
                ContainerTierAuditWieldReqs(stats, 4, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.TwoHandedCombat, (int)Skill.HeavyWeapons, (int)Skill.LightWeapons, (int)Skill.FinesseWeapons }, new HashSet<int> { 0, 250, 300, 325 });
                ContainerTierAuditWieldReqs(stats, 5, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.TwoHandedCombat, (int)Skill.HeavyWeapons, (int)Skill.LightWeapons, (int)Skill.FinesseWeapons }, new HashSet<int> { 300, 325, 350 });
                ContainerTierAuditWieldReqs(stats, 6, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.TwoHandedCombat, (int)Skill.HeavyWeapons, (int)Skill.LightWeapons, (int)Skill.FinesseWeapons }, new HashSet<int> { 350, 370, 400 });
                ContainerTierAuditWieldReqs(stats, 7, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.TwoHandedCombat, (int)Skill.HeavyWeapons, (int)Skill.LightWeapons, (int)Skill.FinesseWeapons }, new HashSet<int> { 370, 400, 420 });
                ContainerTierAuditWieldReqs(stats, 8, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.TwoHandedCombat, (int)Skill.HeavyWeapons, (int)Skill.LightWeapons, (int)Skill.FinesseWeapons }, new HashSet<int> { 400, 420, 430 });

                ContainerTierAuditWieldReqs(stats, 1, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.MissileWeapons }, new HashSet<int> { 0 });
				ContainerTierAuditWieldReqs(stats, 2, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.MissileWeapons }, new HashSet<int> { 0, 250 });
                ContainerTierAuditWieldReqs(stats, 3, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.MissileWeapons }, new HashSet<int> { 0, 250, 270 });
                ContainerTierAuditWieldReqs(stats, 4, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.MissileWeapons }, new HashSet<int> { 250, 270, 290 });
                ContainerTierAuditWieldReqs(stats, 5, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.MissileWeapons }, new HashSet<int> { 270, 290, 315 });
                ContainerTierAuditWieldReqs(stats, 6, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.MissileWeapons }, new HashSet<int> { 315, 335, 360 });
                ContainerTierAuditWieldReqs(stats, 7, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.MissileWeapons }, new HashSet<int> { 335, 360, 375 });
                ContainerTierAuditWieldReqs(stats, 8, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.MissileWeapons }, new HashSet<int> { 360, 375, 385 });

                ContainerTierAuditWieldReqs(stats, 1, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.WarMagic, (int)Skill.VoidMagic }, new HashSet<int> { 0 });
                ContainerTierAuditWieldReqs(stats, 2, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.WarMagic, (int)Skill.VoidMagic }, new HashSet<int> { 0 });
                ContainerTierAuditWieldReqs(stats, 3, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.WarMagic, (int)Skill.VoidMagic }, new HashSet<int> { 0 });
                ContainerTierAuditWieldReqs(stats, 4, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.WarMagic, (int)Skill.VoidMagic }, new HashSet<int> { 0, 290 });
				ContainerTierAuditWieldReqs(stats, 5, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.WarMagic, (int)Skill.VoidMagic }, new HashSet<int> { 0, 290, 310 });
                ContainerTierAuditWieldReqs(stats, 6, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.WarMagic, (int)Skill.VoidMagic }, new HashSet<int> { 0, 290, 310, 330, 355 });
                ContainerTierAuditWieldReqs(stats, 7, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.WarMagic, (int)Skill.VoidMagic }, new HashSet<int> { 330, 355, 375 });
                ContainerTierAuditWieldReqs(stats, 8, (int)WieldRequirement.RawSkill, new HashSet<int> { (int)Skill.WarMagic, (int)Skill.VoidMagic }, new HashSet<int> { 355, 375, 385 }); 

				if (outputAuditLine)
                    File.AppendAllText(Path.Combine(workignOutputFolder, "Tier Container Audit.txt"), Environment.NewLine);
            }
        }

        private bool outputAuditLine;

		private void ContainerTierAuditSpells(Stats stats, int tier, Mag.Shared.Spells.Spell.BuffLevels minBuffLevel, Mag.Shared.Spells.Spell.BuffLevels maxBuffLevel)
        {
            if (stats.Tier != tier)
                return;

            foreach (var itemGroupStats in stats.ObjectClasses.Values)
                ContainerTierAuditSpellsInner(stats.ContainerName, tier, itemGroupStats, minBuffLevel, maxBuffLevel);
        }

        private void ContainerTierAuditSpellsInner(string containerName, int tier, ItemGroups.ItemGroupStats itemGroupStats, Mag.Shared.Spells.Spell.BuffLevels minBuffLevel, Mag.Shared.Spells.Spell.BuffLevels maxBuffLevel)
        {
            foreach (var item in itemGroupStats.Items)
            {
                if (!item.LongValues.ContainsKey(IntValueKey.ItemWorkmanship))
                    continue;

                foreach (var spellId in item.Spells)
                {
                    var spell = Mag.Shared.Spells.SpellTools.GetSpell(spellId);

                    if (spell.BuffLevel == Mag.Shared.Spells.Spell.BuffLevels.None)
                        continue;

                    if (spell.BuffLevel < minBuffLevel || spell.BuffLevel > maxBuffLevel)
                    {
                        File.AppendAllText(Path.Combine(txtOutputPath.Text, "Tier Container Audit.txt"), $"containerName: {containerName.PadRight(30)}, tier: {tier}, item: 0x{item.Id:X8}:{item.StringValues[StringValueKey.Name].PadRight(30)}, has spell: {spell}, Calculated Item Tier: {TierCalculator.Calculate(new List<IdentResponse> { item })}" + Environment.NewLine);
                        outputAuditLine = true;
                    }
                }
            }
        }

        private void ContainerTierAuditWieldReqs(Stats stats, int tier, int wieldRequirements, HashSet<int> wieldSkillType, HashSet<int> validWieldDifficulties)
        {
	        if (stats.Tier != tier)
		        return;

	        foreach (var itemGroupStats in stats.ObjectClasses.Values)
		        ContainerTierAuditWieldReqsInner(stats.ContainerName, tier, itemGroupStats, wieldRequirements, wieldSkillType, validWieldDifficulties);
		}

		private void ContainerTierAuditWieldReqsInner(string containerName, int tier, ItemGroups.ItemGroupStats itemGroupStats, int wieldRequirements, HashSet<int> wieldSkillType, HashSet<int> validWieldDifficulties)
        {
	        foreach (var item in itemGroupStats.Items)
	        {
		        if (!item.LongValues.ContainsKey(IntValueKey.ItemWorkmanship))
			        continue;

				if (!item.LongValues.ContainsKey(IntValueKey.WieldRequirements))
			        continue;

				if (item.LongValues[IntValueKey.WieldRequirements] != wieldRequirements)
					continue;

				if (!item.LongValues.ContainsKey(IntValueKey.WieldSkillType))
					continue;

				if (!wieldSkillType.Contains(item.LongValues[IntValueKey.WieldSkillType]))
					continue;

				if (!item.LongValues.ContainsKey(IntValueKey.WieldDifficulty))
					continue;

				if (!validWieldDifficulties.Contains(item.LongValues[IntValueKey.WieldDifficulty]))
				{
					File.AppendAllText(Path.Combine(txtOutputPath.Text, "Tier Container Audit.txt"), $"containerName: {containerName.PadRight(30)}, tier: {tier}, item: 0x{item.Id:X8}:{item.StringValues[StringValueKey.Name].PadRight(30)}, has WieldRequirements: {item.LongValues[IntValueKey.WieldRequirements]}, WieldSkillType: {item.LongValues[IntValueKey.WieldSkillType]}, WieldDifficulty: {item.LongValues[IntValueKey.WieldDifficulty]}, Calculated Item Tier: {TierCalculator.Calculate(new List<IdentResponse> { item })}" + Environment.NewLine);
					outputAuditLine = true;
				}
			}
		}
	}
}
