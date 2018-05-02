using System;
using System.Globalization;
using System.IO;
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

            //OnBeforeLoadFiles();

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

                    //OnLoadFilesComplete();

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

                // "Timestamp","ContainerName","ContainerID","LandCell","Location","JSON"
                // "2017-01-20 08:18:48Z,"Monty's Golden Chest",2056986702,"A9B20117","40.8N, 33.6E","{"Id":"-1419165865","ObjectClass":"Misc","BoolValues":{"69":"True"},"DoubleValues":{"167":"45","167772170":"0"},"LongValues":{"367":"430","375":"7","218103835":"67108882","218103843":"8","280":"213","33":"0","105":"7","369":"115","114":"0","366":"54","374":"13","218103810":"2056986634","218103826":"16","218103830":"33554817","218103834":"128","218103850":"29728","19":"7000","91":"50","218103831":"48","218103847":"137345","92":"50","218103808":"49548","218103824":"64","218103832":"1076382872","5":"50","218103809":"4154","218103833":"7","218103849":"29733"},"StringValues":{"1":"Lightning Phyntos Wasp Essence (125)","14":"Use this essence to summon or dismiss your Lightning Phyntos Wasp."},"ActiveSpells":"","Spells":""}"
                // "2017-01-20 08:18:48Z,"Monty's Golden Chest",2056986702,"A9B20117","40.8N, 33.6E","{"Id":"-1419511451","ObjectClass":"Scroll","BoolValues":{},"DoubleValues":{"167772170":"0"},"LongValues":{"19":"2000","218103831":"16","218103835":"18","218103843":"8","218103847":"135297","218103808":"45258","218103816":"5785","218103832":"6307864","218103848":"0","5":"30","218103809":"28959","218103810":"2056986634","218103830":"33554826","218103834":"8192","218103838":"1"},"StringValues":{"1":"Scroll of Dirty Fighting Mastery Self VII","14":"Use this item to attempt to learn its spell.","16":"Inscribed spell: Dirty Fighting Mastery Self VII Increases the caster's Dirty Fighting skill by 40 points."},"ActiveSpells":"","Spells":"5785"}"

                try
                {
                    var fifthComma = Util.IndexOfNth(line, ',', 5);

                    if (fifthComma == -1) // Corrupt line
                    {
                        Interlocked.Increment(ref corruptLines);
                        continue;
                    }

                    var firstPart = line.Substring(0, fifthComma);

                    var firstPartSplit = firstPart.Split(',');
                    if (firstPartSplit[0] == "\"Timestamp\"") // Header line
                        continue;

                    DateTime timestamp;
                    if (!DateTime.TryParse(firstPartSplit[0].Substring(1, firstPartSplit[0].Length - 2), out timestamp)) // Corrupt line
                    {
                        Interlocked.Increment(ref corruptLines);
                        continue;
                    }

                    // ContainerName

                    // ContainerID

                    int landcell;
                    if (!int.TryParse(firstPartSplit[3].Substring(1, firstPartSplit[3].Length - 2), NumberStyles.HexNumber, null, out landcell)) // Corrupt line
                    {
                        Interlocked.Increment(ref corruptLines);
                        continue;
                    }

                    /*double x;
                    double y;
                    double z;
                    var rawCoordinatesSplit = firstPartSplit[4].Substring(1, firstPartSplit[4].Length - 2).Split(' ');
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
                    var rawCoordinates = new Tuple<double, double, double>(x, y, z);*/

                    var jsonPart = line.Substring(fifthComma + 1, line.Length - (fifthComma + 1));
                    if (jsonPart[0] != '"' || jsonPart[jsonPart.Length - 1] != '"') // Corrupt line
                    {
                        Interlocked.Increment(ref corruptLines);
                        continue;
                    }

                    jsonPart = jsonPart.Substring(1, jsonPart.Length - 2); // Trim the quotes... why did I add them.. :(

                    if (jsonPart.StartsWith("{\"Id"))
                    {
                        // todo

                        if (ct.IsCancellationRequested)
                            return;
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
    }
}
