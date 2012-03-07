///////////////////////////////////////////////////////////////////////////////
//File: UTLFileExtraBlockManager.cs
//
//Description: A class for VTClassic files to allow arbitrary length- and type- delimited
//  data blocks at the end of a profile file.
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

namespace VTClassic
{
    internal interface IUTLFileBlockHandler
    {
        string BlockTypeID { get; }
        void Read(System.IO.StreamReader inf, int len);
        void Write(CountedStreamWriter inf);
    }

    internal class UTLFileExtraBlockManager
    {
        static Dictionary<string, Type> BlockHandlerTypes = new Dictionary<string, Type>();
        static void AddHandlerType(Type t)
        {
            if (!typeof(IUTLFileBlockHandler).IsAssignableFrom(t)) throw new Exception("UTLFileExtraBlockManager: Cannot add handler type.");

            //Create one to determine its typeid
            IUTLFileBlockHandler h = (IUTLFileBlockHandler)t.GetConstructor(new Type[] { }).Invoke(null);
            string tid = h.BlockTypeID;

            if (BlockHandlerTypes.ContainsKey(tid)) throw new Exception("UTLFileExtraBlockManager: duplicate handler id (" + tid + ")");

            BlockHandlerTypes[tid] = t;
        }
        static UTLFileExtraBlockManager()
        {
            try
            {
                //Add the current handler types
                AddHandlerType(typeof(UTLBlockHandlers.UTLBlock_SalvageCombine));
            }
            catch (Exception exx)
            {
                System.Windows.Forms.MessageBox.Show("Exception in static constructor(" + System.Reflection.Assembly.GetExecutingAssembly().FullName + "): " + exx.ToString());
            }
        }

        List<IUTLFileBlockHandler> FileBlocks = new List<IUTLFileBlockHandler>();

        void TryAddDefaultBlock(IUTLFileBlockHandler h)
        {
            if (GetFirstBlock(h.BlockTypeID) == null)
                FileBlocks.Add(h);
        }
        public void CreateDefaultBlocks()
        {
            //Create the blocks that aren't here
            TryAddDefaultBlock(new UTLBlockHandlers.UTLBlock_SalvageCombine());
        }

        public IUTLFileBlockHandler GetFirstBlock(string HandlerName)
        {
            foreach (IUTLFileBlockHandler h in FileBlocks)
            {
                if (h.BlockTypeID == HandlerName) return h;
            }
            return null;
        }

        public void Read(System.IO.StreamReader inf)
        {
            FileBlocks.Clear();

            while (!inf.EndOfStream)
            {
                //Read blocks
                string blocktype = inf.ReadLine();
                int blocklen = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);

                if (BlockHandlerTypes.ContainsKey(blocktype))
                {
                    //Known blocktype, create and add
                    IUTLFileBlockHandler h = (IUTLFileBlockHandler)BlockHandlerTypes[blocktype].GetConstructor(new Type[] { }).Invoke(null);
                    h.Read(inf, blocklen);
                    FileBlocks.Add(h);
                }
                else
                {
                    //Unknown blocktype, skip ahead
                    char[] qq = new char[blocklen];
                    inf.Read(qq, 0, blocklen);
                }
            }

            CreateDefaultBlocks();
        }

        public void Write(CountedStreamWriter inf)
        {
            foreach (IUTLFileBlockHandler h in FileBlocks)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CountedStreamWriter sw = new CountedStreamWriter(ms);
                h.Write(sw);
                sw.Flush();

                inf.WriteLine(h.BlockTypeID);
                inf.WriteLine(((int)sw.Count).ToString(System.Globalization.CultureInfo.InvariantCulture));

                ms.Position = 0;
                System.IO.StreamReader sr = new System.IO.StreamReader(ms);
                inf.Write(sr.ReadToEnd());
            }
        }
    }
}
