using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

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
                    cts.Dispose();

                    lblResults.Text = totalLines.ToString("N0") + " lines read. " + corruptLines.ToString("N0") + " corrupt lines found.";
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

        readonly JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

        private void ProcessFile(string fileName, CancellationToken ct)
        {
            var fileLines = File.ReadAllLines(fileName);

            // "Timestamp","ContainerName","ContainerID","LandCell","Location","JSON"
            if (fileLines.Length < 2 || fileLines[0] != "\"Timestamp\",\"ContainerName\",\"ContainerID\",\"LandCell\",\"Location\",\"JSON\"")
                return;

            lock (totalLinesLockObject)
                totalLines += fileLines.Length;

            foreach (var line in fileLines)
            {
                if (ct.IsCancellationRequested)
                    return;

                // "2017-01-20 08:18:48Z,"Monty's Golden Chest",2056986702,"A9B20117","40.8N, 33.6E","{"Id":"-1419165865","ObjectClass":"Misc","BoolValues":{"69":"True"},"DoubleValues":{"167":"45","167772170":"0"},"LongValues":{"367":"430","375":"7","218103835":"67108882","218103843":"8","280":"213","33":"0","105":"7","369":"115","114":"0","366":"54","374":"13","218103810":"2056986634","218103826":"16","218103830":"33554817","218103834":"128","218103850":"29728","19":"7000","91":"50","218103831":"48","218103847":"137345","92":"50","218103808":"49548","218103824":"64","218103832":"1076382872","5":"50","218103809":"4154","218103833":"7","218103849":"29733"},"StringValues":{"1":"Lightning Phyntos Wasp Essence (125)","14":"Use this essence to summon or dismiss your Lightning Phyntos Wasp."},"ActiveSpells":"","Spells":""}"
                // "2017-01-20 08:18:48Z,"Monty's Golden Chest",2056986702,"A9B20117","40.8N, 33.6E","{"Id":"-1419511451","ObjectClass":"Scroll","BoolValues":{},"DoubleValues":{"167772170":"0"},"LongValues":{"19":"2000","218103831":"16","218103835":"18","218103843":"8","218103847":"135297","218103808":"45258","218103816":"5785","218103832":"6307864","218103848":"0","5":"30","218103809":"28959","218103810":"2056986634","218103830":"33554826","218103834":"8192","218103838":"1"},"StringValues":{"1":"Scroll of Dirty Fighting Mastery Self VII","14":"Use this item to attempt to learn its spell.","16":"Inscribed spell: Dirty Fighting Mastery Self VII Increases the caster's Dirty Fighting skill by 40 points."},"ActiveSpells":"","Spells":"5785"}"
                // "2017-01-15 04:02:33Z,"Corpse of Mu-miyah Sentinel",-1851166334,"8764003C","21.6S, 6.8E","{"Id":"-1851166529","ObjectClass":"Money","BoolValues":{},"DoubleValues":{},"LongValues":{"19":"4775","218103815":"25000","218103831":"1","218103835":"16","218103843":"1","218103847":"131073","218103808":"273","218103832":"28696","5":"0","218103809":"8863","218103810":"-1851166402","218103814":"4775","218103830":"33557367","218103834":"64"},"StringValues":{"1":"Pyreal"},"ActiveSpells":"","Spells":""}"

                try
                {
                    var sixthComma = Util.IndexOfNth(line, ',', 6);

                    if (sixthComma == -1) // Corrupt line
                    {
                        Interlocked.Increment(ref corruptLines);
                        continue;
                    }

                    var firstPart = line.Substring(0, sixthComma);

                    var firstPartSplit = firstPart.Split(',');
                    if (firstPartSplit[0] == "\"Timestamp\"") // Header line
                        continue;

                    DateTime timestamp;
                    if (!DateTime.TryParse(firstPartSplit[0].Substring(1, firstPartSplit[0].Length - 2), out timestamp)) // Corrupt line
                    {
                        Interlocked.Increment(ref corruptLines);
                        continue;
                    }

                    var containerName = firstPartSplit[1].Substring(1, firstPartSplit[1].Length - 2);

                    var containerID = int.Parse(firstPartSplit[2].Substring(2, firstPartSplit[2].Length - 2));

                    int landcell;
                    if (!int.TryParse(firstPartSplit[3].Substring(1, firstPartSplit[3].Length - 2), NumberStyles.HexNumber, null, out landcell)) // Corrupt line
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

                        Dictionary<string, object> result = (Dictionary<string, object>)jsonSerializer.DeserializeObject(jsonPart);

                        identResponse.ParseFromDictionary(result);

                        if (ct.IsCancellationRequested)
                            return;

                        if (!ProcessLootItem(containerName, containerID, identResponse))
                        {
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

        private readonly Dictionary<string, Dictionary<int, List<IdentResponse>>> containersLoot = new Dictionary<string, Dictionary<int, List<IdentResponse>>>();

        private void OnBeforeLoadFiles()
        {
            containersLoot.Clear();
        }

        private bool ProcessLootItem(string containerName, int containerID, IdentResponse identResponse)
        {
            lock (processLockObject)
            {
                Dictionary<int, List<IdentResponse>> containers;

                if (containersLoot.ContainsKey(containerName))
                    containers = containersLoot[containerName];
                else
                {
                    containers = new Dictionary<int, List<IdentResponse>>();
                    containersLoot.Add(containerName, containers);
                }

                List<IdentResponse> items;

                if (containers.ContainsKey(containerID))
                    items = containers[containerID];
                else
                {
                    items = new List<IdentResponse>();
                    containers.Add(containerID, items);
                }

                // Does this item already exist?
                foreach (var item in items)
                {
                    if (item.Id == identResponse.Id)
                        return true;
                }

                items.Add(identResponse);

                return true;
            }
        }

        private void OnLoadFilesComplete()
        {
            var sb = new StringBuilder();


            // Calculate the loot tiers
            TierCalculator.Calculate(containersLoot);


            // Populate the Containers tab
            var dt = new DataTable();

            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Tier", typeof(int));
            dt.Columns.Add("Hits", typeof(int));
            dt.Columns.Add("Average Items", typeof(float));
            dt.Columns.Add("Total Items", typeof(int));

            foreach (var kvp in containersLoot)
            {
                var dr = dt.NewRow();

                dr["Name"] = kvp.Key;
                dr["Tier"] = TierCalculator.GetTierByContainerName(kvp.Key);
                dr["Hits"] = kvp.Value.Count;

                var totalItems = 0;

                foreach (var containers in kvp.Value)
                    totalItems += containers.Value.Count;

                dr["Average Items"] = totalItems / (float)kvp.Value.Count;
                dr["Total Items"] = totalItems;

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


            // Calculate the stats
            StatsCalculator.Calculate(containersLoot);


            // Output stats by tier
            foreach (var kvp in StatsCalculator.StatsByLootTier)
                File.WriteAllText(Path.Combine(txtOutputPath.Text, "Tier " + kvp.Key + ".txt"), kvp.Value.ToString());

            // Output stats by container name
            foreach (var kvp in StatsCalculator.StatsByContainerName)
                File.WriteAllText(Path.Combine(txtOutputPath.Text, "Container " + kvp.Key + ".txt"), kvp.Value.ToString());
        }
    }
}
