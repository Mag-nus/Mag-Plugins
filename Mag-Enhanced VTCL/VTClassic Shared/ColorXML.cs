///////////////////////////////////////////////////////////////////////////////
//File: ColorXML.cs
//
//Description: The ColorXML class, which handles slot definitions for
//  the Armor Type Similar Color requirement type.
//  This file is shared between the VTClassic Plugin and the VTClassic Editor.
//
//This file is Copyright (c) 2009-2010 VirindiPlugins
//
//The original copy of this code can be obtained from http://www.virindi.net/repos/virindi_public
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace VTClassic
{
    internal static class ColorXML
    {
        static Dictionary<string, int[]> iSlotDefinitions = null;

        public static Dictionary<string, int[]> SlotDefinitions
        {
            get
            {
                if (iSlotDefinitions == null) LoadColorXMLDefinitions();
                return iSlotDefinitions;
            }
        }

        static void LoadColorXMLDefinitions()
        {
            List<string> paths = new List<string>();
            List<string> files = new List<string>();

            files.Add("ColorSlots.Default.xml");
            files.Add("ColorSlots.User.xml");

            try
            {
                //This assembly's dir
                paths.Add(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            }
            catch { }

            try
            {
                //VTank profile directory
                paths.Add((string)Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Decal\Plugins\{642F1F48-16BE-48BF-B1D4-286652C4533E}").GetValue("ProfilePath"));
            }
            catch { }

            iSlotDefinitions = new Dictionary<string, int[]>();
            iSlotDefinitions["None"] = new int[0];
            foreach (string curpath in paths)
            {
                foreach (string curfile in files)
                {
                    string pathedfile = System.IO.Path.Combine(curpath, curfile);
                    if (!System.IO.File.Exists(pathedfile)) continue;

                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.Load(pathedfile);
                    }
                    catch { continue; }

                    XmlNode node_top = doc.DocumentElement;
                    foreach (XmlNode node_def in node_top.ChildNodes)
                    {
                        if (node_def.NodeType != XmlNodeType.Element) continue;
                        switch (node_def.Name.ToLowerInvariant())
                        {
                            case "slotdef":
                                {
                                    try
                                    {
                                        string defname = node_def.Attributes["name"].Value;

                                        List<int> palentries = new List<int>();

                                        foreach (XmlNode node_palentry in node_def.ChildNodes)
                                        {
                                            if (node_palentry.NodeType != XmlNodeType.Element) continue;
                                            if (node_palentry.Name != "entry") continue;

                                            palentries.Add(int.Parse(node_palentry.Attributes["value"].Value, System.Globalization.CultureInfo.InvariantCulture));
                                        }

                                        iSlotDefinitions[defname] = palentries.ToArray();
                                    }
                                    catch { continue; }
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}