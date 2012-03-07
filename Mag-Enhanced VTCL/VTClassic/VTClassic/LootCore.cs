///////////////////////////////////////////////////////////////////////////////
//File: LootCore.cs
//
//Description: The core of the VTClassic Virindi Tank Loot Plugin, implementing
//  old-style Virindi Tank looting.
//
//This file is Copyright (c) 2009 VirindiPlugins
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
using uTank2.LootPlugins;

namespace VTClassic
{
    public class LootCore: uTank2.LootPlugins.LootPluginBase, uTank2.LootPlugins.ILootPluginCapability_SalvageCombineDecision
    {
        internal static LootCore Instance;

        internal static void WriteToChat(string s)
        {
            if (Instance == null) return;
            Instance.Host.AddChatText(s);
        }

        internal static void WriteToChat(string s, int c, int w)
        {
            if (Instance == null) return;
            Instance.Host.AddChatText(s, c, w);
        }

        void ExceptionHandler(Exception ex)
        {
            WriteToChat("Exception: " + ex.ToString(), 6, 1);
        }

#if DEBUGMSG
        int neededid = 0;
        int noid = 0;
#endif

        public override bool DoesPotentialItemNeedID(uTank2.LootPlugins.GameItemInfo item)
        {
            try
            {
                if (item.HasIDData) return false;

                bool ret = LootRules.NeedsID(item);

#if DEBUGMSG
                if (ret)
                    Host.AddChatText("Item " + item.GetValueString(StringValueKey.Name, "") + " will get ID.", 0);
                else
                    Host.AddChatText("Item " + item.GetValueString(StringValueKey.Name, "") + " does not need ID.", 6);


                if (ret)
                    neededid++;
                else
                    noid++;

                if (((neededid + noid) % 20) == 0)
                    Host.AddChatText((neededid + noid).ToString() + " items seen, ID rate " + Math.Round(100d * (double)(neededid) / (double)(neededid + noid), 1).ToString() + "%");
#endif

                return ret;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return true;
        }

        public override uTank2.LootPlugins.LootAction GetLootDecision(uTank2.LootPlugins.GameItemInfo item)
        {
            try
            {
                if (LootRules == null) return uTank2.LootPlugins.LootAction.NoLoot;

                string matchedrulename;
                int data1;
                eLootAction act = LootRules.Classify(item, out matchedrulename, out data1);
                LootAction vtaction = LootAction.NoLoot;
                switch (act)
                {
                    case eLootAction.Keep:
                        vtaction = LootAction.Keep;
                        break;
                    case eLootAction.NoLoot:
                        vtaction = LootAction.NoLoot;
                        break;
                    case eLootAction.Salvage:
                        vtaction = LootAction.Salvage;
                        break;
                    case eLootAction.KeepUpTo:
                        vtaction = LootAction.GetKeepUpTo(data1);
                        break;
                    case eLootAction.Sell:
                        vtaction = LootAction.Sell;
                        break;
                }
                vtaction.RuleName = matchedrulename;
                return vtaction;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return uTank2.LootPlugins.LootAction.NoLoot;
        }

        public override void LoadProfile(string filename, bool newprofile)
        {
            try
            {
#if DEBUGMSG
                neededid = 0;
                noid = 0;
#endif

                if (newprofile)
                {
                    LootRules = new cLootRules();
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    {
                        using (CountedStreamWriter sr = new CountedStreamWriter(fs))
                        {
                            LootRules.Write(sr);
                        }
                    }

                    WriteToChat("Created blank profile " + filename + ".");
                }
                else
                {
                    if (!System.IO.File.Exists(filename)) return;

                    LootRules = new cLootRules();
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(fs))
                        {
                            if (LootRules.Read(sr, -1))
                                WriteToChat("Load profile " + filename + " successful (file version " + LootRules.UTLFileVersion.ToString() + ").");
                            else
                                WriteToChat("Load profile " + filename + " returned an error. Your entire profile may not have loaded properly.");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        public override void UnloadProfile()
        {
            try
            {
                LootRules = null;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        cLootRules LootRules = null;
        //MyClasses.MetaViewWrappers.IView view;

        public override void OpenEditorForProfile()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        public override void CloseEditorForProfile()
        {
            try
            {
                //view.Dispose();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        public override uTank2.LootPlugins.LootPluginInfo Startup()
        {
            try
            {
                Instance = this;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return new uTank2.LootPlugins.LootPluginInfo("utl");
        }

        public override void Shutdown()
        {
            try
            {
                Instance = null;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        #region ILootPluginCapability_SalvageCombineDecision Members

        public bool CanCombineBags(double bag1workmanship, double bag2workmanship, int material)
        {
            UTLBlockHandlers.UTLBlock_SalvageCombine CombineBlock = LootRules.ExtraBlockManager.GetFirstBlock("SalvageCombine") as UTLBlockHandlers.UTLBlock_SalvageCombine;

            return CombineBlock.CanCombineBags(bag1workmanship, bag2workmanship, material);
        }

        #endregion
    }
}
