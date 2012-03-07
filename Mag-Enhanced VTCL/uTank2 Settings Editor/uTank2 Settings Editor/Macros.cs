///////////////////////////////////////////////////////////////////////////////
//File: Macros.cs
//
//Description: Helper methods for altering rules and requirements.
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

using System.Collections.Generic;
using System;

namespace VTClassic
{
    public partial class Form1
    {
        void ruleMoveDown(int index, bool setCurrent)
        {
            if (index + 1 >= lstRules.Items.Count) return;
            string swap;
            cLootItemRule swapl;

            swap = (string)lstRules.Items[index + 1];
            swapl = LootRules.Rules[index + 1];
            lstRules.Items[index + 1] = lstRules.Items[index];
            LootRules.Rules[index + 1] = LootRules.Rules[index];
            lstRules.Items[index] = swap;
            LootRules.Rules[index] = swapl;

            if (setCurrent)
            {
                SetCurrentReq(null, 0);
                SetCurrentRule(LootRules.Rules[index + 1], index + 1);
                lstRules.SelectedIndex = index + 1;
            }
            lstRules.TopIndex = Math.Max(0, lstRules.SelectedIndex - ((lstRules.Height / lstRules.ItemHeight) / 2));
            FileChanged = true;
        }

        void ruleMoveUp(int index, bool setCurrent)
        {
            if (index < 1) return;
            ruleMoveDown(index - 1, setCurrent);
            lstRules.SelectedIndex = index - 1;
        }

        void addMaterialRules(int[] matIds, int work, bool update)
        {
            bool create;
            string name;
            foreach (int m in matIds)
            {
                create = true;
                name = string.Format("S: {0}", GameInfo.getMaterialName(m));

                foreach (cLootItemRule r in this.LootRules.Rules)
                {
                    if (r.act == eLootAction.Salvage && r.name.Equals(name))
                    {
                        create = false;

                        if (update)
                        {
                            updateMaterialRule(r, m, work);
                            FileChanged = true;
                        }
                        break;
                    }
                }
                if (create)
                {
                    addMaterialRule(m, work, name);
                }
            }
            SetCurrentRule(null, -1);
        }

        private void addMaterialRule(int mat, int work, string name)
        {
            cLootItemRule r = new cLootItemRule();
            r.name = name;
            r.act = eLootAction.Salvage;

            updateMaterialRule(r, mat, work);

            LootRules.Rules.Add(r);
            lstRules.Items.Add(r.name);
            FileChanged = true;
        }

        private void updateMaterialRule(cLootItemRule r, int mat, int w)
        {
            LongValKeyLE r1 = new LongValKeyLE(mat, IntValueKey.Material);
            LongValKeyGE r2 = new LongValKeyGE(mat, IntValueKey.Material);
            DoubleValKeyGE r3 = new DoubleValKeyGE(Convert.ToDouble(w), DoubleValueKey.SalvageWorkmanship);

            r.IntRules = new List<iLootRule>(new iLootRule[] { r1, r2, r3 });
        }

        private void alterWorkmanshipReqs(eLootAction e, int by)
        {
            if (e == eLootAction.Keep) by = -1 * by;

            foreach (cLootItemRule r in this.LootRules.Rules)
            {
                if (r.act == e)
                {
                    foreach (iLootRule req in r.IntRules)
                    {
                        if (req.GetRuleType() == eLootRuleType.DoubleValKeyGE)
                        {
                            ((DoubleValKeyGE)req).keyval = Math.Min(10,((DoubleValKeyGE)req).keyval + by);
                        }
                        else if (req.GetRuleType() == eLootRuleType.DoubleValKeyLE)
                        {
                            ((DoubleValKeyLE)req).keyval = Math.Max(1, ((DoubleValKeyLE)req).keyval - by);
                        }

                    }
                }
            }
        }

    }
}