///////////////////////////////////////////////////////////////////////////////
//File: UTLBlock_SalvageCombine.cs
//
//Description: A UTL file block for storing salvage combination ranges.
//  This file is shared between the VTClassic Plugin and the VTClassic Editor.
//
//This file is Copyright (c) 2010 VirindiPlugins
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
using System.Text;

namespace VTClassic.UTLBlockHandlers
{
    internal class UTLBlock_SalvageCombine : IUTLFileBlockHandler
    {
        const int SALVAGEBLOCK_FILE_FORMAT_VERSION = 1;

        public string DefaultCombineString = "";
        public Dictionary<int, string> MaterialCombineStrings = new Dictionary<int, string>();

        public UTLBlock_SalvageCombine()
        {
            //Default rules
            DefaultCombineString = "1-6, 7-8, 9, 10";

            string imbs = "1-10";
            //Magic item tinkering
            MaterialCombineStrings[GameInfo.GetMaterialID("Agate")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Azurite")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Black Opal")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Bloodstone")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Carnelian")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Citrine")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Fire Opal")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Hematite")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Lavender Jade")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Malachite")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Red Jade")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Rose Quartz")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Sunstone")] = imbs;

            //Weapon tinkering
            MaterialCombineStrings[GameInfo.GetMaterialID("White Sapphire")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Red Garnet")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Jet")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Imperial Topaz")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Emerald")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Black Garnet")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Aquamarine")] = imbs;

            //Armor tinkering
            MaterialCombineStrings[GameInfo.GetMaterialID("Zircon")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Yellow Topaz")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Peridot")] = imbs;

            //Other
            MaterialCombineStrings[GameInfo.GetMaterialID("Leather")] = imbs;
            MaterialCombineStrings[GameInfo.GetMaterialID("Ivory")] = imbs;
        }

        #region CombineString Parsing
        struct sDoublePair
        {
            public double a;
            public double b;
        }
        static int GetRangeIndex(List<sDoublePair> Ranges, double val)
        {
            for (int i = 0; i < Ranges.Count; ++i)
            {
                //Gaps in ranges go to the previous range
                if (Ranges[i].a > val)
                    return i - 1;

                //If we fall into this range, choose it
                if ((Ranges[i].a <= val) && (Ranges[i].b >= val))
                    return i;
            }
            return Ranges.Count;
        }
        static bool TestCombineString(double w1, double w2, string pcombinestring)
        {
            List<sDoublePair> Ranges = new List<sDoublePair>();

            //Look through the string and delete all characters we don't understand
            string combinestring = pcombinestring;
            for (int i = combinestring.Length - 1; i >= 0; --i)
            {
                if (Char.IsDigit(combinestring[i])) continue;
                if (combinestring[i] == ',') continue;
                if (combinestring[i] == ';') continue;
                if (combinestring[i] == '-') continue;
                if (combinestring[i] == '.') continue;

                combinestring.Remove(i, 1);
            }

            //Split and parse string into ranges
            string[] toks = combinestring.Split(';', ',');
            foreach (string tok in toks)
            {
                if (tok.Length == 0) continue;
                string[] numbers = tok.Split('-');
                if (numbers.Length == 0) continue;

                sDoublePair addpair = new sDoublePair();
                if (numbers.Length == 1)
                {
                    addpair.a = double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture);
                    addpair.b = addpair.a;
                }
                else
                {
                    addpair.a = double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture);
                    addpair.b = double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture);
                }
                Ranges.Add(addpair);
            }

            //Find out which range we fall into
            return (GetRangeIndex(Ranges, w1) == GetRangeIndex(Ranges, w2));
        }
        #endregion CombineString Parsing

        public bool CanCombineBags(double bag1workmanship, double bag2workmanship, int material)
        {
            if (MaterialCombineStrings.ContainsKey(material))
                return TestCombineString(bag1workmanship, bag2workmanship, MaterialCombineStrings[material]);
            else
                return TestCombineString(bag1workmanship, bag2workmanship, DefaultCombineString);
        }

        public string BlockTypeID
        {
            get { return "SalvageCombine"; }
        }

        public void Read(System.IO.StreamReader inf, int len)
        {
            string formatversion = inf.ReadLine();

            DefaultCombineString = inf.ReadLine();

            MaterialCombineStrings.Clear();
            int nummatstrings = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            for (int i = 0; i < nummatstrings; ++i)
            {
                int mat = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                string cmb = inf.ReadLine();
                MaterialCombineStrings[mat] = cmb;
            }
        }

        public void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(SALVAGEBLOCK_FILE_FORMAT_VERSION);

            inf.WriteLine(DefaultCombineString);

            inf.WriteLine(MaterialCombineStrings.Count);
            foreach (KeyValuePair<int, string> kp in MaterialCombineStrings)
            {
                inf.WriteLine(kp.Key);
                inf.WriteLine(kp.Value);
            }
        }
    }
}