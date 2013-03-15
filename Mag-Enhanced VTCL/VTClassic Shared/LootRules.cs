///////////////////////////////////////////////////////////////////////////////
//File: LootRules.cs
//
//Description: The old-style Virindi Tank loot rule system.
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
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

#if VTC_PLUGIN
using uTank2.LootPlugins;
#endif

namespace VTClassic
{
    #region iLootRule abstract class
    internal abstract class iLootRule : iSettingsCollection, ICloneable
    {
        //iSettingsCollection methods
        public abstract void Read(System.IO.StreamReader inf, int fileversion);
        public abstract void Write(CountedStreamWriter inf);

        //LootRule abstract methods
        public abstract eLootRuleType GetRuleType();
        public abstract string DisplayString();
        public abstract bool MayRequireID();
        public abstract string FriendlyName();

#if VTC_PLUGIN
        //Members only when compiling the plugin, vtank is not referenced for editor
        public abstract bool Match(GameItemInfo id);
        public abstract void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch);
#endif

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

#if VTC_EDITOR
        public virtual bool UI_ActsOnCombo_Uses() { return false; }
        public virtual string UI_ActsOnCombo_Label() { return "[None]"; }
        public virtual void UI_ActsOnCombo_Set(int index) { }
        public virtual int UI_ActsOnCombo_Get() { return 0; }
        public virtual ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return new List<string>().AsReadOnly(); }
        public virtual System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return System.Drawing.Color.White; }

        public virtual bool UI_KeyCombo_Uses() { return false; }
        public virtual string UI_KeyCombo_Label() { return "[None]"; }
        public virtual void UI_KeyCombo_Set(int index) { }
        public virtual int UI_KeyCombo_Get() { return 0; }
        public virtual ReadOnlyCollection<string> UI_KeyCombo_Options() { return new List<string>().AsReadOnly(); }
        public virtual System.Drawing.Color UI_KeyCombo_OptionColors(int index) { return System.Drawing.Color.White; }

        public virtual bool UI_TextValue_Uses() { return false; }
        public virtual string UI_TextValue_Label() { return "[None]"; }
        public virtual void UI_TextValue_Set(string value) { }
        public virtual string UI_TextValue_Get() { return ""; }

        public virtual bool UI_TextValue2_Uses() { return false; }
        public virtual string UI_TextValue2_Label() { return "[None]"; }
        public virtual void UI_TextValue2_Set(string value) { }
        public virtual string UI_TextValue2_Get() { return ""; }

        public virtual bool UI_TextValue3_Uses() { return false; }
        public virtual string UI_TextValue3_Label() { return "[None]"; }
        public virtual void UI_TextValue3_Set(string value) { }
        public virtual string UI_TextValue3_Get() { return ""; }

        public virtual bool UI_TextValue4_Uses() { return false; }
        public virtual string UI_TextValue4_Label() { return "[None]"; }
        public virtual void UI_TextValue4_Set(string value) { }
        public virtual string UI_TextValue4_Get() { return ""; }

        public virtual bool UI_ColorSample_Uses() { return false; }
        public virtual System.Drawing.Color UI_ColorSample_Get() { return System.Drawing.Color.White; }
#endif

        cUniqueID iUniqueID;
        public void SetID(cUniqueID pUniqueID)
        {
            iUniqueID = pUniqueID;
        }
        public cUniqueID GetID()
        {
            return iUniqueID;
        }
    }
    #endregion iLootRule abstract class

    internal enum eLootRuleType
    {
        UnsupportedRequirement = -1,

        SpellNameMatch = 0,
        StringValueMatch = 1,
        LongValKeyLE = 2,
        LongValKeyGE = 3,
        DoubleValKeyLE = 4,
        DoubleValKeyGE = 5,
        DamagePercentGE = 6,
        ObjectClass = 7,
        SpellCountGE = 8,
        SpellMatch = 9,
        MinDamageGE = 10,
        LongValKeyFlagExists = 11,
        LongValKeyE = 12,
        LongValKeyNE = 13,
        AnySimilarColor = 14,
        SimilarColorArmorType = 15,
        SlotSimilarColor = 16,
        SlotExactPalette = 17,
        
        //Character reqs, not based on the item
        CharacterSkillGE = 1000,
        CharacterMainPackEmptySlotsGE = 1001,
        CharacterLevelGE = 1002,
        CharacterLevelLE = 1003,
        CharacterBaseSkill = 1004,

		// Mag-nus added, calculated requirements
		BuffedMedianDamageGE = 2000, // Melee Weapon
		BuffedMissileDamageGE = 2001,
		BuffedLongValKeyGE = 2003,
		BuffedDoubleValKeyGE = 2005,
		CalcdBuffedTinkedDamageGE = 2006,
		TotalRatingsGE = 2007,
		CalcedBuffedTinkedTargetMeleeGE = 2008,

        DisabledRule = 9999,
    }

    internal static class LootRuleCreator
    {
        public static iLootRule CreateLootRule(eLootRuleType t)
        {
            switch (t)
            {
                case eLootRuleType.SpellNameMatch: return new SpellNameMatch();
                case eLootRuleType.StringValueMatch: return new StringValueMatch();
                case eLootRuleType.LongValKeyLE: return new LongValKeyLE();
                case eLootRuleType.LongValKeyGE: return new LongValKeyGE();
                case eLootRuleType.DoubleValKeyLE: return new DoubleValKeyLE();
                case eLootRuleType.DoubleValKeyGE: return new DoubleValKeyGE();
                case eLootRuleType.DamagePercentGE: return new DamagePercentGE();
                case eLootRuleType.ObjectClass: return new ObjectClassE();
                case eLootRuleType.SpellCountGE: return new SpellCountGE();
                case eLootRuleType.SpellMatch: return new SpellMatch();
                case eLootRuleType.MinDamageGE: return new MinDamageGE();
                case eLootRuleType.LongValKeyFlagExists: return new LongValKeyFlagExists();
                case eLootRuleType.LongValKeyE: return new LongValKeyE();
                case eLootRuleType.LongValKeyNE: return new LongValKeyNE();
                case eLootRuleType.AnySimilarColor: return new AnySimilarColor();
                case eLootRuleType.SimilarColorArmorType: return new SimilarColorArmorType();
                case eLootRuleType.SlotSimilarColor: return new SlotSimilarColor();
                case eLootRuleType.SlotExactPalette: return new SlotExactPalette();

                //Character-based reqs
                case eLootRuleType.CharacterSkillGE: return new CharacterSkillGE();
                case eLootRuleType.CharacterMainPackEmptySlotsGE: return new CharacterMainPackEmptySlotsGE();
                case eLootRuleType.CharacterLevelGE: return new CharacterLevelGE();
                case eLootRuleType.CharacterLevelLE: return new CharacterLevelLE();
                case eLootRuleType.CharacterBaseSkill: return new CharacterBaseSkill();

				// Mag-nus added
				case eLootRuleType.BuffedMedianDamageGE: return new BuffedMedianDamageGE();
				case eLootRuleType.BuffedMissileDamageGE: return new BuffedMissileDamageGE();
				case eLootRuleType.BuffedLongValKeyGE: return new BuffedLongValKeyGE();
				case eLootRuleType.BuffedDoubleValKeyGE: return new BuffedDoubleValKeyGE();
				case eLootRuleType.CalcdBuffedTinkedDamageGE: return new CalcdBuffedTinkedDamageGE();
				case eLootRuleType.TotalRatingsGE: return new TotalRatingsGE();
				case eLootRuleType.CalcedBuffedTinkedTargetMeleeGE: return new CalcedBuffedTinkedTargetMeleeGE();

                case eLootRuleType.DisabledRule: return new DisabledRule(true);

                default: return null;
            }
        }
    }

    #region UnsupportedRequirement special rule type
    internal class cUnsupportedRequirement : iLootRule
    {
        public char[] data;

        public override eLootRuleType GetRuleType()
        {
            return eLootRuleType.UnsupportedRequirement;
        }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return false;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            ismatch = false;
            hasdecision = true;
        }
#endif

        public override void Read(System.IO.StreamReader inf, int fileversion)
        {

        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.Write(data, 0, data.Length);
        }

        public override string DisplayString()
        {
            return "UNSUPPORTED REQUIREMENT";
        }

        public override string FriendlyName()
        {
            return "Unsupported Requirement";
        }

        public override bool MayRequireID()
        {
            return false;
        }
    }
    #endregion UnsupportedRequirement special rule type

    #region LootRule classes

    #region SpellNameMatch
    internal class SpellNameMatch : iLootRule
    {
        public System.Text.RegularExpressions.Regex rx = new Regex("");

        public SpellNameMatch() { }
        public SpellNameMatch(System.Text.RegularExpressions.Regex k) { rx = k; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.SpellNameMatch; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<uTank2.MySpell> Spells = id.Spells;
            foreach (uTank2.MySpell sp in Spells)
            {
                if (rx.Match(sp.Name).Success)
                    return true;
            }
            return false;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            //Is the object magical?
            bool ismagical = ((id.GetValueInt(IntValueKey.IconOutline, 0) & 0x01) > 0);
            if (ismagical)
            {
                hasdecision = false;
                ismatch = false;        //Doesn't matter, just have to assign
            }
            else
            {
                hasdecision = true;
                ismatch = false;
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            rx = new System.Text.RegularExpressions.Regex(inf.ReadLine());
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(rx.ToString());
        }

        public override string DisplayString()
        {
            return "SpellNameMatch: " + rx.ToString();
        }

        public override string FriendlyName()
        {
            return "Spell Name Match";
        }

        public override bool MayRequireID()
        {
            return true;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Spell Name Pattern"; }
        public override void UI_TextValue_Set(string value) { rx = new Regex(value); }
        public override string UI_TextValue_Get() { return rx.ToString(); }
#endif
    }
    #endregion SpellNameMatch

    #region StringValueMatch
    internal class StringValueMatch : iLootRule
    {
        public System.Text.RegularExpressions.Regex rx = new Regex("");
        public StringValueKey vk = StringValueKey.FullDescription;

        public StringValueMatch() { }
        public StringValueMatch(System.Text.RegularExpressions.Regex k, StringValueKey v) { rx = k; vk = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.StringValueMatch; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return rx.Match(id.GetValueString(vk, "")).Success;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            rx = new System.Text.RegularExpressions.Regex(inf.ReadLine());
            vk = (StringValueKey)Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(rx.ToString());
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("{0} matches: {1}", vk, rx);
        }

        public override string FriendlyName()
        {
            return "String Value Match";
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "String Value Pattern"; }
        public override void UI_TextValue_Set(string value) { rx = new Regex(value); }
        public override string UI_TextValue_Get() { return rx.ToString(); }

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Acts on"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ComboKeys.GetSVKEntries(); }
        public override int UI_ActsOnCombo_Get() { return ComboKeys.IndexFromSVK(vk); }
        public override void UI_ActsOnCombo_Set(int index) { vk = ComboKeys.SVKFromIndex(index); }
        public override System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return ComboKeys.GetSVKColor(index); }
#endif
    }
    #endregion StringValueMatch

    #region LongValKeyLE
    internal class LongValKeyLE : iLootRule
    {
        public int keyval = 0;
        public IntValueKey vk = IntValueKey.ActivationReqSkillId;

        public LongValKeyLE() { }
        public LongValKeyLE(int k, IntValueKey v) { keyval = k; vk = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.LongValKeyLE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (id.GetValueInt(vk, 0) <= keyval);
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (IntValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            if (vk == IntValueKey.Material)
            {
                SortedDictionary<string, int> matIds = GameInfo.getMaterialInfo();
                if (matIds.ContainsValue((int)keyval))
                {
                    foreach (KeyValuePair<string, int> kv in matIds)
                    {
                        if (kv.Value == (int)keyval)
                        {
                            return string.Format("{0} <= {1} ({2})", vk, keyval, kv.Key);
                        }
                    }
                }
            }
            else if (vk == IntValueKey.WieldReqAttribute)
            {
                SortedDictionary<string, int> skillIds = GameInfo.getSkillInfo();
                if (skillIds.ContainsValue((int)keyval))
                {
                    foreach (KeyValuePair<string, int> kv in skillIds)
                    {
                        if (kv.Value == (int)keyval)
                        {
                            return string.Format("{0} <= {1} ({2})", vk, keyval, kv.Key);
                        }
                    }
                }
            }
            return string.Format("{0} <= {1}", vk, keyval);
        }

        public override string FriendlyName()
        {
            return "Long Value Key <=";
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Long Value"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Acts on"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ComboKeys.GetLVKEntries(); }
        public override int UI_ActsOnCombo_Get() { return ComboKeys.IndexFromLVK(vk); }
        public override void UI_ActsOnCombo_Set(int index) { vk = ComboKeys.LVKFromIndex(index); }
        public override System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return ComboKeys.GetLVKColor(index); }
#endif
    }
    #endregion LongValKeyLE

    #region LongValKeyGE
    internal class LongValKeyGE : iLootRule
    {
        public int keyval = 0;
        public IntValueKey vk = IntValueKey.ActivationReqSkillId;

        public LongValKeyGE() { }
        public LongValKeyGE(int k, IntValueKey v) { keyval = k; vk = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.LongValKeyGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (id.GetValueInt(vk, 0) >= keyval);
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (IntValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            if (vk == IntValueKey.Material)
            {
                SortedDictionary<string, int> matIds = GameInfo.getMaterialInfo();
                if (matIds.ContainsValue((int)keyval))
                {
                    foreach (KeyValuePair<string, int> kv in matIds)
                    {
                        if (kv.Value == (int)keyval)
                        {
                            return string.Format("{0} >= {1} ({2})", vk, keyval, kv.Key);
                        }
                    }
                }
            }
            else if (vk == IntValueKey.WieldReqAttribute)
            {
                SortedDictionary<string, int> skillIds = GameInfo.getSkillInfo();
                if (skillIds.ContainsValue((int)keyval))
                {
                    foreach (KeyValuePair<string, int> kv in skillIds)
                    {
                        if (kv.Value == (int)keyval)
                        {
                            return string.Format("{0} >= {1} ({2})", vk, keyval, kv.Key);
                        }
                    }
                }
            }
            return string.Format("{0} >= {1}", vk, keyval);
        }

        public override string FriendlyName()
        {
            return "Long Value Key >=";
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Long Value"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Acts on"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ComboKeys.GetLVKEntries(); }
        public override int UI_ActsOnCombo_Get() { return ComboKeys.IndexFromLVK(vk); }
        public override void UI_ActsOnCombo_Set(int index) { vk = ComboKeys.LVKFromIndex(index); }
        public override System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return ComboKeys.GetLVKColor(index); }
#endif
    }
    #endregion LongValKeyGE

    #region DoubleValKeyLE
    internal class DoubleValKeyLE : iLootRule
    {
        public double keyval = 0d;
        public DoubleValueKey vk = DoubleValueKey.AcidProt;

        public DoubleValKeyLE() { }
        public DoubleValKeyLE(double k, DoubleValueKey v) { keyval = k; vk = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.DoubleValKeyLE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (((float)id.GetValueDouble(vk, 0)) <= ((float)keyval));
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
            vk = (DoubleValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("{0} <= {1}", vk, keyval);
        }

        public override string FriendlyName()
        {
            return "Double Value Key <=";
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Double Value"; }
        public override void UI_TextValue_Set(string value) { double.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Acts on"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ComboKeys.GetDVKEntries(); }
        public override int UI_ActsOnCombo_Get() { return ComboKeys.IndexFromDVK(vk); }
        public override void UI_ActsOnCombo_Set(int index) { vk = ComboKeys.DVKFromIndex(index); }
        public override System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return ComboKeys.GetDVKColor(index); }
#endif
    }
    #endregion DoubleValKeyLE

    #region DoubleValKeyGE
    internal class DoubleValKeyGE : iLootRule
    {
        public double keyval = 0d;
        public DoubleValueKey vk = DoubleValueKey.AcidProt;

        public DoubleValKeyGE() { }
        public DoubleValKeyGE(double k, DoubleValueKey v) { keyval = k; vk = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.DoubleValKeyGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (((float)id.GetValueDouble(vk, 0)) >= ((float)keyval));
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
            vk = (DoubleValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("{0} >= {1}", vk, keyval);
        }

        public override string FriendlyName()
        {
            return "Double Value Key >=";
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Double Value"; }
        public override void UI_TextValue_Set(string value) { double.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Acts on"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ComboKeys.GetDVKEntries(); }
        public override int UI_ActsOnCombo_Get() { return ComboKeys.IndexFromDVK(vk); }
        public override void UI_ActsOnCombo_Set(int index) { vk = ComboKeys.DVKFromIndex(index); }
        public override System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return ComboKeys.GetDVKColor(index); }
#endif
    }
    #endregion DoubleValKeyGE

    #region DamagePercentGE
    internal class DamagePercentGE : iLootRule
    {
        public double keyval = 0d;

        public DamagePercentGE() { }
        public DamagePercentGE(double k) { keyval = k; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.DamagePercentGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return false;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = false;
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("DamagePercentGE >= {0}", keyval);
        }

        public override string FriendlyName()
        {
            return "Damage Percentage >=";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Damage Percent"; }
        public override void UI_TextValue_Set(string value) { double.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }
#endif
    }
    #endregion DamagePercentGE

    #region ObjectClassE
    internal class ObjectClassE : iLootRule
    {
        public ObjectClass vk = ObjectClass.Armor;

        public ObjectClassE() { }
        public ObjectClassE(ObjectClass v) { vk = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.ObjectClass; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (id.ObjectClass == vk);
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = Match(id);
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            vk = (ObjectClass)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("ObjectClass = {0}", vk); 
        }

        public override string FriendlyName()
        {
            return "ObjectClass";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR
        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "ObjectClass is"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ComboKeys.GetOCEntries(); }
        public override int UI_ActsOnCombo_Get() { return ComboKeys.IndexFromOC(vk); }
        public override void UI_ActsOnCombo_Set(int index) { vk = ComboKeys.OCFromIndex(index); }
#endif
    }
    #endregion ObjectClassE

    #region SpellCountGE
    internal class SpellCountGE : iLootRule
    {
        public int keyval = 0;

        public SpellCountGE() { }
        public SpellCountGE(int k) { keyval = k; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.SpellCountGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return id.Spells.Count >= keyval;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            //Is the object magical?
            bool ismagical = ((id.GetValueInt(IntValueKey.IconOutline, 0) & 0x01) > 0);
            if (ismagical)
            {
                hasdecision = false;
                ismatch = false;        //Doesn't matter, just have to assign
            }
            else
            {
                hasdecision = true;
                ismatch = false;
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("SpellCount >= {0}", keyval);
        }

        public override string FriendlyName()
        {
            return "Spell Count >=";
        }

        public override bool MayRequireID()
        {
            return true;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Spell Count"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }
#endif
    }
    #endregion SpellCountGE

    #region SpellMatch
    internal class SpellMatch : iLootRule
    {
        public Regex rxDoesMatch = new Regex("");
        public Regex rxDoesNotMatch = new Regex("");
        public int Count = 1;

        public SpellMatch() {  }
        public SpellMatch(Regex p, Regex n, int c) { rxDoesMatch = p; rxDoesNotMatch = n; Count = c; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.SpellMatch; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            int c = 0;
            System.Collections.ObjectModel.ReadOnlyCollection<uTank2.MySpell> Spells = id.Spells;
            bool rxnEmpty = string.Empty.Equals(rxDoesNotMatch.ToString().Trim());

            foreach (uTank2.MySpell sp in Spells)
            {
                if (rxDoesMatch.Match(sp.Name).Success && (rxnEmpty || !rxDoesNotMatch.Match(sp.Name).Success))
                {
                    c++; if (c >= Count) return true;
                }
            }
            return false;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            //Is the object magical?
            bool ismagical = ((id.GetValueInt(IntValueKey.IconOutline, 0) & 0x01) > 0);
            if (ismagical)
            {
                hasdecision = false;
                ismatch = false;        //Doesn't matter, just have to assign
            }
            else
            {
                hasdecision = true;
                ismatch = false;
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            rxDoesMatch = new Regex(inf.ReadLine());
            rxDoesNotMatch = new Regex(inf.ReadLine());
            Count = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(rxDoesMatch.ToString());
            inf.WriteLine(rxDoesNotMatch.ToString());
            inf.WriteLine(Count.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            if (string.Empty.Equals(rxDoesNotMatch.ToString().Trim()))
            {
                return string.Format("SpellMatch: {0} [{1} times]",
                rxDoesMatch, Count);
            }
            return string.Format("SpellMatch: {0}, but not {1} [{2} times]",
                rxDoesMatch, rxDoesNotMatch, Count);
        }

        public override string FriendlyName()
        {
            return "Spell Match and Count";
        }

        public override bool MayRequireID()
        {
            return true;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Does Match"; }
        public override void UI_TextValue_Set(string value) { rxDoesMatch = new Regex(value); }
        public override string UI_TextValue_Get() { return rxDoesMatch.ToString(); }

        public override bool UI_TextValue2_Uses() { return true; }
        public override string UI_TextValue2_Label() { return "Does NOT Match"; }
        public override void UI_TextValue2_Set(string value) { rxDoesNotMatch = new Regex(value); }
        public override string UI_TextValue2_Get() { return rxDoesNotMatch.ToString(); }

        public override bool UI_TextValue3_Uses() { return true; }
        public override string UI_TextValue3_Label() { return "Minimum spells that match"; }
        public override void UI_TextValue3_Set(string value) { int.TryParse(value, out Count); }
        public override string UI_TextValue3_Get() { return Count.ToString(); }
#endif
    }
    #endregion SpellMatch

    #region MinDamageGE
    internal class MinDamageGE : iLootRule
    {
        public double keyval = 0d;

        public MinDamageGE() { keyval = 0; }
        public MinDamageGE(double v) { keyval = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.MinDamageGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            int maxdamage = id.GetValueInt(IntValueKey.MaxDamage, 0);
            double variance = id.GetValueDouble(DoubleValueKey.Variance, 0.0);
            return maxdamage - (variance * maxdamage) >= keyval;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (id.ObjectClass == ObjectClass.MeleeWeapon)
            {
                hasdecision = false;
                ismatch = false;        //Doesn't matter, just have to assign
            }
            else
            {
                hasdecision = true;
                ismatch = false;
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("MinDamage >= {0}", keyval);
        }

        public override string FriendlyName()
        {
            return "Minimum Damage >=";
        }

        public override bool MayRequireID()
        {
            return true;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Minimum Damage"; }
        public override void UI_TextValue_Set(string value) { double.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }
#endif
    }
    #endregion MinDamageGE

    #region LongValKeyFlagExists
    internal class LongValKeyFlagExists : iLootRule
    {
        public int keyval = 0;
        public IntValueKey vk = IntValueKey.ActivationReqSkillId;

        public LongValKeyFlagExists() { }
        public LongValKeyFlagExists(int k, IntValueKey v) { keyval = k; vk = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.LongValKeyFlagExists; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (id.GetValueInt(vk, 0) & keyval) > 0;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (IntValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return string.Format("{0} has flags {1} (0x{1:X})", vk, keyval);
        }

        public override string FriendlyName()
        {
            return "Long Value Key Has Flags";
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Long Value"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Acts on"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ComboKeys.GetLVKEntries(); }
        public override int UI_ActsOnCombo_Get() { return ComboKeys.IndexFromLVK(vk); }
        public override void UI_ActsOnCombo_Set(int index) { vk = ComboKeys.LVKFromIndex(index); }
        public override System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return ComboKeys.GetLVKColor(index); }
#endif
    }
    #endregion LongValKeyFlagExists

    #region CharacterSkillGE
    internal class CharacterSkillGE : iLootRule
    {
        public int keyval = 0;
        public VTCSkillID vk = VTCSkillID.Alchemy;

        public CharacterSkillGE() { }
        public CharacterSkillGE(int k, VTCSkillID v) { keyval = k; vk = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.CharacterSkillGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            //Need to use interop CharStats to fetch skill effective value,
            //otherwise Gearcraft and Two Handed won't work
            Decal.Interop.Filters.SkillInfo skillinfo = null;
            try
            {
                skillinfo = Decal.Adapter.CoreManager.Current.CharacterFilter.Underlying.get_Skill((Decal.Interop.Filters.eSkillID)(int)vk);
                return (skillinfo.Buffed >= keyval);
            }
            finally
            {
                if (skillinfo != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(skillinfo);
            }
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = Match(id);
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (VTCSkillID)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return String.Format("Buffed {0} >= {1}", vk, keyval);
        }

        public override string FriendlyName()
        {
            return "Character Buffed Skill >=";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Buffed Skill Value"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Acts on"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ComboKeys.GetSkillEntries(); }
        public override int UI_ActsOnCombo_Get() { return ComboKeys.IndexFromSkill(vk); }
        public override void UI_ActsOnCombo_Set(int index) { vk = ComboKeys.SkillFromIndex(index); }
#endif
    }
    #endregion CharacterSkillGE

    #region CharacterMainPackEmptySlotsGE
    internal class CharacterMainPackEmptySlotsGE : iLootRule
    {
        public int keyval = 0;

        public CharacterMainPackEmptySlotsGE() { }
        public CharacterMainPackEmptySlotsGE(int v) { keyval = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.CharacterMainPackEmptySlotsGE; }

#if VTC_PLUGIN
        int CalculateFreeMainPackSlots()
        {
            int slots = 102;
            foreach (Decal.Adapter.Wrappers.WorldObject wo in Decal.Adapter.CoreManager.Current.WorldFilter.GetByContainer(Decal.Adapter.CoreManager.Current.CharacterFilter.Id))
            {
                if (wo.ObjectClass == Decal.Adapter.Wrappers.ObjectClass.Container) continue;
                if (wo.ObjectClass == Decal.Adapter.Wrappers.ObjectClass.Foci) continue;
                if (wo.Values(Decal.Adapter.Wrappers.LongValueKey.EquippedSlots, 0) > 0) continue;

                slots--;
            }

            return slots;
        }
        
        public override bool Match(GameItemInfo id)
        {
            return (CalculateFreeMainPackSlots() >= keyval);
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = Match(id);
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return String.Format("Main pack slots >= {0}", keyval);
        }

        public override string FriendlyName()
        {
            return "Free Main Pack Slots >=";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Empty Slots"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }
#endif
    }
    #endregion CharacterMainPackEmptySlotsGE

    #region LongValKeyE
    internal class LongValKeyE : iLootRule
    {
        public int keyval = 0;
        public IntValueKey vk = IntValueKey.ActivationReqSkillId;

        public LongValKeyE() { }
        public LongValKeyE(int k, IntValueKey v) { keyval = k; vk = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.LongValKeyE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (id.GetValueInt(vk, 0) == keyval);
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (IntValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            if (vk == IntValueKey.Material)
            {
                SortedDictionary<string, int> matIds = GameInfo.getMaterialInfo();
                if (matIds.ContainsValue((int)keyval))
                {
                    foreach (KeyValuePair<string, int> kv in matIds)
                    {
                        if (kv.Value == (int)keyval)
                        {
                            return string.Format("{0} == {1} ({2})", vk, keyval, kv.Key);
                        }
                    }
                }
            }
            else if (vk == IntValueKey.WieldReqAttribute)
            {
                SortedDictionary<string, int> skillIds = GameInfo.getSkillInfo();
                if (skillIds.ContainsValue((int)keyval))
                {
                    foreach (KeyValuePair<string, int> kv in skillIds)
                    {
                        if (kv.Value == (int)keyval)
                        {
                            return string.Format("{0} == {1} ({2})", vk, keyval, kv.Key);
                        }
                    }
                }
            }
            return string.Format("{0} == {1}", vk, keyval);
        }

        public override string FriendlyName()
        {
            return "Long Value Key ==";
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Long Value"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Acts on"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ComboKeys.GetLVKEntries(); }
        public override int UI_ActsOnCombo_Get() { return ComboKeys.IndexFromLVK(vk); }
        public override void UI_ActsOnCombo_Set(int index) { vk = ComboKeys.LVKFromIndex(index); }
        public override System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return ComboKeys.GetLVKColor(index); }
#endif
    }
    #endregion LongValKeyE

    #region LongValKeyNE
    internal class LongValKeyNE : iLootRule
    {
        public int keyval = 0;
        public IntValueKey vk = IntValueKey.ActivationReqSkillId;

        public LongValKeyNE() { }
        public LongValKeyNE(int k, IntValueKey v) { keyval = k; vk = v; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.LongValKeyNE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return (id.GetValueInt(vk, 0) != keyval);
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            if (GameInfo.IsIDProperty(vk))
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = Match(id);
            }
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            vk = (IntValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            if (vk == IntValueKey.Material)
            {
                SortedDictionary<string, int> matIds = GameInfo.getMaterialInfo();
                if (matIds.ContainsValue((int)keyval))
                {
                    foreach (KeyValuePair<string, int> kv in matIds)
                    {
                        if (kv.Value == (int)keyval)
                        {
                            return string.Format("{0} != {1} ({2})", vk, keyval, kv.Key);
                        }
                    }
                }
            }
            else if (vk == IntValueKey.WieldReqAttribute)
            {
                SortedDictionary<string, int> skillIds = GameInfo.getSkillInfo();
                if (skillIds.ContainsValue((int)keyval))
                {
                    foreach (KeyValuePair<string, int> kv in skillIds)
                    {
                        if (kv.Value == (int)keyval)
                        {
                            return string.Format("{0} != {1} ({2})", vk, keyval, kv.Key);
                        }
                    }
                }
            }
            return string.Format("{0} != {1}", vk, keyval);
        }

        public override string FriendlyName()
        {
            return "Long Value Key !=";
        }

        public override bool MayRequireID()
        {
            return GameInfo.IsIDProperty(vk);
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Long Value"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Acts on"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ComboKeys.GetLVKEntries(); }
        public override int UI_ActsOnCombo_Get() { return ComboKeys.IndexFromLVK(vk); }
        public override void UI_ActsOnCombo_Set(int index) { vk = ComboKeys.LVKFromIndex(index); }
        public override System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return ComboKeys.GetLVKColor(index); }
#endif
    }
    #endregion LongValKeyNE

    #region CharacterLevelGE
    internal class CharacterLevelGE : iLootRule
    {
        public int keyval = 0;

        public CharacterLevelGE() { }
        public CharacterLevelGE(int k) { keyval = k; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.CharacterLevelGE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return Decal.Adapter.CoreManager.Current.CharacterFilter.Level >= keyval;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = Match(id);
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return String.Format("Char Level >= {0}", keyval);
        }

        public override string FriendlyName()
        {
            return "Character Level >=";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Character Level"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }
#endif
    }
    #endregion CharacterLevelGE

    #region CharacterLevelLE
    internal class CharacterLevelLE : iLootRule
    {
        public int keyval = 0;

        public CharacterLevelLE() { }
        public CharacterLevelLE(int k) { keyval = k; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.CharacterLevelLE; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return Decal.Adapter.CoreManager.Current.CharacterFilter.Level <= keyval;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = Match(id);
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            keyval = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return String.Format("Char Level <= {0}", keyval);
        }

        public override string FriendlyName()
        {
            return "Character Level <=";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Character Level"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out keyval); }
        public override string UI_TextValue_Get() { return keyval.ToString(); }
#endif
    }
    #endregion CharacterLevelLE

    #region CharacterBaseSkill
    internal class CharacterBaseSkill : iLootRule
    {
        public VTCSkillID vk = VTCSkillID.Alchemy;
        public int minskill = 0;
        public int maxskill = 999;

        public CharacterBaseSkill() { }
        public CharacterBaseSkill(VTCSkillID v, int min, int max) {vk = v; minskill = min; maxskill = max; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.CharacterBaseSkill; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            //Need to use interop CharStats to fetch skill effective value,
            //otherwise Gearcraft and Two Handed won't work
            Decal.Interop.Filters.SkillInfo skillinfo = null;
            try
            {
                skillinfo = Decal.Adapter.CoreManager.Current.CharacterFilter.Underlying.get_Skill((Decal.Interop.Filters.eSkillID)(int)vk);
                return (skillinfo.Base >= minskill && skillinfo.Base <= maxskill);
            }
            finally
            {
                if (skillinfo != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(skillinfo);
            }
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = Match(id);
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            vk = (VTCSkillID)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            minskill = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            maxskill = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString(minskill, System.Globalization.CultureInfo.InvariantCulture));
            inf.WriteLine(Convert.ToString(maxskill, System.Globalization.CultureInfo.InvariantCulture));
        }

        public override string DisplayString()
        {
            return String.Format("Base {0} [{1}-{2}]", vk, minskill, maxskill);
        }

        public override string FriendlyName()
        {
            return "Base Skill Range";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Minimum Base Skill"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out minskill); }
        public override string UI_TextValue_Get() { return minskill.ToString(); }

        public override bool UI_TextValue2_Uses() { return true; }
        public override string UI_TextValue2_Label() { return "Maximum Base Skill"; }
        public override void UI_TextValue2_Set(string value) { int.TryParse(value, out maxskill); }
        public override string UI_TextValue2_Get() { return maxskill.ToString(); }

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Acts on"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ComboKeys.GetSkillEntries(); }
        public override int UI_ActsOnCombo_Get() { return ComboKeys.IndexFromSkill(vk); }
        public override void UI_ActsOnCombo_Set(int index) { vk = ComboKeys.SkillFromIndex(index); }
#endif
    }
    #endregion CharacterBaseSkill

	// Mag-nus added
	#region ' MagItemInfo '
	#if VTC_PLUGIN
	internal class MagItemInfo
	{
		private readonly GameItemInfo gameItemInfo;

		public MagItemInfo(GameItemInfo gameItemInfo)
		{
			this.gameItemInfo = gameItemInfo;
		}

		struct SpellInfo<T>
		{
			public readonly T Key;
			public readonly double Change;
			public readonly double Bonus;

			public SpellInfo(T key, double change, double bonus = 0)
			{
				Key = key;
				Change = change;
				Bonus = bonus;
			}
		}

		static readonly Dictionary<int, SpellInfo<IntValueKey>> LongValueKeySpellEffects = new Dictionary<int, SpellInfo<IntValueKey>>()
		{
			// In 2012 they removed these item spells and converted them to auras that are cast on the player, not on the item.
			{ 1616, new SpellInfo<IntValueKey>(IntValueKey.MaxDamage, 20)}, // Blood Drinker VI
			{ 2096, new SpellInfo<IntValueKey>(IntValueKey.MaxDamage, 22)}, // Infected Caress
			//{ 5183, new SpellInfo<LongValueKey>(LongValueKey.MaxDamage, 22)}, // Incantation of Blood Drinker Pre Feb-2013
			//{ 4395, new SpellInfo<LongValueKey>(LongValueKey.MaxDamage, 24, 2)}, // Incantation of Blood Drinker, this spell on the item adds 2 more points of damage over a user casted 8 Pre Feb-2013
			{ 5183, new SpellInfo<IntValueKey>(IntValueKey.MaxDamage, 24)}, // Incantation of Blood Drinker Post Feb-2013
			{ 4395, new SpellInfo<IntValueKey>(IntValueKey.MaxDamage, 24)}, // Incantation of Blood Drinker Post Feb-2013

			{ 2598, new SpellInfo<IntValueKey>(IntValueKey.MaxDamage, 2, 2)}, // Minor Blood Thirst
			{ 2586, new SpellInfo<IntValueKey>(IntValueKey.MaxDamage, 4, 4)}, // Major Blood Thirst
			{ 4661, new SpellInfo<IntValueKey>(IntValueKey.MaxDamage, 7, 7)}, // Epic Blood Thirst
			{ 6089, new SpellInfo<IntValueKey>(IntValueKey.MaxDamage, 10, 10)}, // Legendary Blood Thirst

			{ 3688, new SpellInfo<IntValueKey>(IntValueKey.MaxDamage, 300)}, // Prodigal Blood Drinker


			{ 2604, new SpellInfo<IntValueKey>(IntValueKey.ArmorLevel, 20, 20)}, // Minor Impenetrability
			{ 2592, new SpellInfo<IntValueKey>(IntValueKey.ArmorLevel, 40, 40)}, // Major Impenetrability
			{ 4667, new SpellInfo<IntValueKey>(IntValueKey.ArmorLevel, 60, 60)}, // Epic Impenetrability
			{ 6095, new SpellInfo<IntValueKey>(IntValueKey.ArmorLevel, 80, 80)}, // Legendary Impenetrability
		};

		static readonly Dictionary<int, SpellInfo<DoubleValueKey>> DoubleValueKeySpellEffects = new Dictionary<int, SpellInfo<DoubleValueKey>>()
		{
			// In 2012 they removed these item spells and converted them to auras that are cast on the player, not on the item.
			{ 3258, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .06)}, // Spirit Drinker VI
			{ 3259, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .07)}, // Infected Spirit Caress
			//{ 5182, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .07)}, // Incantation of Spirit Drinker Pre Feb-2013
			//{ 4414, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .08, .01)}, // Incantation of Spirit Drinker, this spell on the item adds 1 more % of damage over a user casted 8 Pre Feb-2013
			{ 5182, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .08)}, // Incantation of Spirit Drinker Post Feb-2013
			{ 4414, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .08)}, // Incantation of Spirit Drinker, this spell on the item adds 1 more % of damage over a user casted 8 Post Feb-2013

			{ 3251, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .01, .01)}, // Minor Spirit Thirst
			{ 3250, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .03, .03)}, // Major Spirit Thirst
			{ 4670, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .05, .05)}, // Epic Spirit Thirst
			{ 6098, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .07, .07)}, // Legendary Spirit Thirst

			{ 3735, new SpellInfo<DoubleValueKey>(DoubleValueKey.ElementalDamageVersusMonsters, .15)}, // Prodigal Spirit Drinker


			// In 2012 they removed these item spells and converted them to auras that are cast on the player, not on the item.
			{ 1592, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .15)}, // Heart Seeker VI
			{ 2106, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .17)}, // Elysa's Sight
			{ 4405, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .20)}, // Incantation of Heart Seeker

			{ 2603, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .03, .03)}, // Minor Heart Thirst
			{ 2591, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .05, .05)}, // Major Heart Thirst
			{ 4666, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .07, .07)}, // Epic Heart Thirst
			{ 6094, new SpellInfo<DoubleValueKey>(DoubleValueKey.AttackBonus, .09, .09)}, // Legendary Heart Thirst


			// In 2012 they removed these item spells and converted them to auras that are cast on the player, not on the item.
			{ 1605, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .15)}, // Defender VI
			{ 2101, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .17)}, // Cragstone's Will
			//{ 4400, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .17)}, // Incantation of Defender Pre Feb-2013
			{ 4400, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .20)}, // Incantation of Defender Post Feb-2013

			{ 2600, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .03, .03)}, // Minor Defender
			{ 3985, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .04, .04)}, // Mukkir Sense
			{ 2588, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .05, .05)}, // Major Defender
			{ 4663, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .07, .07)}, // Epic Defender
			{ 6091, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .09, .09)}, // Legendary Defender

			{ 3699, new SpellInfo<DoubleValueKey>(DoubleValueKey.MeleeDefenseBonus, .25)}, // Prodigal Defender


			// In 2012 they removed these item spells and converted them to auras that are cast on the player, not on the item.
			{ 1480, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.60)}, // Hermetic Link VI
			{ 2117, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.70)}, // Mystic's Blessing
			{ 4418, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.80)}, // Incantation of Hermetic Link

			{ 3201, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.05, 1.05)}, // Feeble Hermetic Link
			{ 3199, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.10, 1.10)}, // Minor Hermetic Link
			{ 3202, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.15, 1.15)}, // Moderate Hermetic Link
			{ 3200, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.20, 1.20)}, // Major Hermetic Link
			{ 6086, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.25, 1.25)}, // Epic Hermetic Link
			{ 6087, new SpellInfo<DoubleValueKey>(DoubleValueKey.ManaCBonus, 1.30, 1.30)}, // Legendary Hermetic Link
		};

		public double BuffedAverageDamage
		{
			get
			{
				double variance = gameItemInfo.GetValueDouble(DoubleValueKey.Variance, 0.0);
				int maxDamage = GetBuffedLogValueKey(IntValueKey.MaxDamage);
				double minDamage = maxDamage - (variance * maxDamage);

				return (minDamage + maxDamage) / 2;
			}
		}

		public double CalcedBuffedTinkedDamage
		{
			get
			{
				double variance = gameItemInfo.GetValueDouble(DoubleValueKey.Variance, 0.0);
				int maxDamage = GetBuffedLogValueKey(IntValueKey.MaxDamage);

				int numberOfTinksLeft = Math.Max(10 - gameItemInfo.GetValueInt(IntValueKey.NumberTimesTinkered, 0), 0);

				if (gameItemInfo.GetValueInt(IntValueKey.Imbued, 0) == 0)
					numberOfTinksLeft--; // Factor in an imbue tink

				// If this is not a loot generated item, it can't be tinked
				if (gameItemInfo.GetValueInt(IntValueKey.Material, 0) == 0)
					numberOfTinksLeft = 0;

				for (int i = 1 ; i <= numberOfTinksLeft ; i++)
				{
					double ironTinkDoT = CalculateDamageOverTime(maxDamage + 24 + 1, variance);
					double graniteTinkDoT = CalculateDamageOverTime(maxDamage + 24, variance * .8);

					if (ironTinkDoT >= graniteTinkDoT)
						maxDamage++;
					else
						variance *= .8;
				}

				return CalculateDamageOverTime(maxDamage + 24, variance);
			}
		}

		public bool CanReachTargetValues(double targetCalcedBuffedTinkedDoT, double targetBuffedMeleeDefenseBonus, double targetBuffedAttackBonus)
		{
			double buffedMeleeDefenseBonus = GetBuffedDoubleValueKey(DoubleValueKey.MeleeDefenseBonus);
			double buffedAttackBonus = GetBuffedDoubleValueKey(DoubleValueKey.AttackBonus);

			double variance = gameItemInfo.GetValueDouble(DoubleValueKey.Variance, 0.0);
			int maxDamage = GetBuffedLogValueKey(IntValueKey.MaxDamage);

			int numberOfTinksLeft = Math.Max(10 - gameItemInfo.GetValueInt(IntValueKey.NumberTimesTinkered, 0), 0);

			if (gameItemInfo.GetValueInt(IntValueKey.Imbued, 0) == 0)
				numberOfTinksLeft--; // Factor in an imbue tink

			// If this is not a loot generated item, it can't be tinked
			if (gameItemInfo.GetValueInt(IntValueKey.Material, 0) == 0)
				numberOfTinksLeft = 0;

			for (int i = 1; i <= numberOfTinksLeft; i++)
			{
				if (buffedMeleeDefenseBonus < targetBuffedMeleeDefenseBonus)
					buffedMeleeDefenseBonus += .01;
				else if (buffedAttackBonus < targetBuffedAttackBonus)
					buffedAttackBonus += .01;
				else
				{
					double ironTinkDoT = CalculateDamageOverTime(maxDamage + 24 + 1, variance);
					double graniteTinkDoT = CalculateDamageOverTime(maxDamage + 24, variance*.8);

					if (ironTinkDoT >= graniteTinkDoT)
						maxDamage++;
					else
						variance *= .8;
				}
			}

			return CalculateDamageOverTime(maxDamage + 24, variance) >= targetCalcedBuffedTinkedDoT && buffedMeleeDefenseBonus >= targetBuffedMeleeDefenseBonus && buffedAttackBonus >= targetBuffedAttackBonus;
		}

		public int TotalRatings
		{
			get
			{
				/*
				DamRating = 370,
				DamResRating = 371,
				CritRating = 372,
				CritResistRating = 373,
				CritDamRating = 374,
				CritDamResistRating = 375,
				HealBoostRating = 376,
				VitalityRating = 379,
				*/
				return gameItemInfo.GetValueInt(370, 0) + gameItemInfo.GetValueInt(371, 0) + gameItemInfo.GetValueInt(372, 0) + gameItemInfo.GetValueInt(373, 0) + gameItemInfo.GetValueInt(374, 0) + gameItemInfo.GetValueInt(375, 0) + gameItemInfo.GetValueInt(376, 0) + gameItemInfo.GetValueInt(379, 0);
			}
		}

		/// <summary>
		/// maxDamage * ((1 - critChance) * (2 - variance) / 2 + (critChance * critMultiplier));
		/// </summary>
		/// <param name="maxDamage"></param>
		/// <param name="variance"></param>
		/// <param name="critChance"></param>
		/// <param name="critMultiplier"></param>
		/// <returns></returns>
		public static double CalculateDamageOverTime(int maxDamage, double variance, double critChance = .1, double critMultiplier = 2)
		{
			return maxDamage * ((1 - critChance) * (2 - variance) / 2 + (critChance * critMultiplier));
		}

		public double BuffedMissileDamage
		{
			get
			{
				return GetBuffedLogValueKey(IntValueKey.MaxDamage) + (((GetBuffedDoubleValueKey(DoubleValueKey.DamageBonus) - 1) * 100) / 3) + GetBuffedLogValueKey(IntValueKey.ElementalDmgBonus);
			}
		}

		public int GetBuffedLogValueKey(IntValueKey key, int defaultValue = 0)
		{
			if (!gameItemInfo.KeyExistsInt((int)key))
				return defaultValue;

			int value = gameItemInfo.GetValueInt(key, defaultValue);

			for (int i = 0 ; i < gameItemInfo.Spells.Count ; i++)
			{
				int spellId = gameItemInfo.Spells[i].Id;

				if (LongValueKeySpellEffects.ContainsKey(spellId) && LongValueKeySpellEffects[spellId].Key == key && LongValueKeySpellEffects[spellId].Bonus != 0)
					value += (int)LongValueKeySpellEffects[spellId].Bonus;
			}

			return value;
		}

		public double GetBuffedDoubleValueKey(DoubleValueKey key, double defaultValue = 0)
		{
			if (!gameItemInfo.KeyExistsDouble((int)key))
				return defaultValue;

			double value = gameItemInfo.GetValueDouble(key, defaultValue);

			for (int i = 0 ; i < gameItemInfo.Spells.Count ; i++)
			{
				int spellId = gameItemInfo.Spells[i].Id;

				if (DoubleValueKeySpellEffects.ContainsKey(spellId) && DoubleValueKeySpellEffects[spellId].Key == key && DoubleValueKeySpellEffects[spellId].Bonus != 0)
				{
					if ((int)DoubleValueKeySpellEffects[spellId].Change == 1)
						value *= DoubleValueKeySpellEffects[spellId].Bonus;
					else
						value += DoubleValueKeySpellEffects[spellId].Bonus;
				}
			}

			return value;
		}
	}
	#endif
	#endregion

	#region BuffedMedianDamageGE
	internal class BuffedMedianDamageGE : iLootRule
	{
		public double keyval = 0d;

		public BuffedMedianDamageGE() { }
		public BuffedMedianDamageGE(int k) { keyval = k; }

		public override eLootRuleType GetRuleType() { return eLootRuleType.BuffedMedianDamageGE; }

#if VTC_PLUGIN
		public override bool Match(GameItemInfo id)
		{
			MagItemInfo magItemInfo = new MagItemInfo(id);

			int maxdamage = magItemInfo.GetBuffedLogValueKey(IntValueKey.MaxDamage);

			double variance = id.GetValueDouble(DoubleValueKey.Variance, 0.0);
			double mindamage = maxdamage - (variance * maxdamage);
			return (mindamage + maxdamage) / 2 >= keyval;
		}

		public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
		{
			if (id.ObjectClass == ObjectClass.MeleeWeapon || id.ObjectClass == ObjectClass.MissileWeapon)
			{
				hasdecision = false;
				ismatch = false;        //Doesn't matter, just have to assign
			}
			else
			{
				hasdecision = true;
				ismatch = false;
			}
		}
#endif

		public override void Read(System.IO.StreamReader inf, int profileversion)
		{
			keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
		}

		public override void Write(CountedStreamWriter inf)
		{
			inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
		}

		public override string DisplayString()
		{
			return String.Format("Buffed Median Damage >= {0}", keyval);
		}

		public override string FriendlyName()
		{
			return "Calced Buffed Median Damage >=";
		}

		public override bool MayRequireID()
		{
			return true;
		}

#if VTC_EDITOR
		public override bool UI_TextValue_Uses() { return true; }
		public override string UI_TextValue_Label() { return "Calced Buffed Median Damage"; }
		public override void UI_TextValue_Set(string value) { double.TryParse(value, out keyval); }
		public override string UI_TextValue_Get() { return keyval.ToString(); }
#endif
	}
	#endregion BuffedMedianDamageGE

	#region BuffedMissileDamageGE
	internal class BuffedMissileDamageGE : iLootRule
	{
		public double keyval = 0d;

		public BuffedMissileDamageGE() { }
		public BuffedMissileDamageGE(int k) { keyval = k; }

		public override eLootRuleType GetRuleType() { return eLootRuleType.BuffedMissileDamageGE; }

#if VTC_PLUGIN
		public override bool Match(GameItemInfo id)
		{
			MagItemInfo magItemInfo = new MagItemInfo(id);

			return magItemInfo.BuffedMissileDamage >= keyval;
		}

		public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
		{
			if (id.ObjectClass == ObjectClass.MissileWeapon)
			{
				hasdecision = false;
				ismatch = false;        //Doesn't matter, just have to assign
			}
			else
			{
				hasdecision = true;
				ismatch = false;
			}
		}
#endif

		public override void Read(System.IO.StreamReader inf, int profileversion)
		{
			keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
		}

		public override void Write(CountedStreamWriter inf)
		{
			inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
		}

		public override string DisplayString()
		{
			return String.Format("Buffed Missile Damage >= {0}", keyval);
		}

		public override string FriendlyName()
		{
			return "Calced Buffed Missile Damage >=";
		}

		public override bool MayRequireID()
		{
			return true;
		}

#if VTC_EDITOR
		public override bool UI_TextValue_Uses() { return true; }
		public override string UI_TextValue_Label() { return "Calced Buffed Missile Damage"; }
		public override void UI_TextValue_Set(string value) { double.TryParse(value, out keyval); }
		public override string UI_TextValue_Get() { return keyval.ToString(); }
#endif
	}
	#endregion BuffedMissileDamageGE

	#region BuffedLongValKeyGE
	internal class BuffedLongValKeyGE : iLootRule
	{
		public double keyval = 0d;
		public IntValueKey vk = IntValueKey.MaxDamage;

		public BuffedLongValKeyGE() { }
		public BuffedLongValKeyGE(double k, IntValueKey v) { keyval = k; vk = v; }

		public override eLootRuleType GetRuleType() { return eLootRuleType.BuffedLongValKeyGE; }

#if VTC_PLUGIN
		public override bool Match(GameItemInfo id)
		{
			MagItemInfo magItemInfo = new MagItemInfo(id);

			int val = magItemInfo.GetBuffedLogValueKey(vk);

			return val >= keyval;
		}

		public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
		{
			hasdecision = false;
			ismatch = false;
		}
#endif

		public override void Read(System.IO.StreamReader inf, int profileversion)
		{
			keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
			vk = (IntValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
		}

		public override void Write(CountedStreamWriter inf)
		{
			inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
			inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
		}

		public override string DisplayString()
		{
			return string.Format("Buffed {0} >= {1}", vk, keyval);
		}

		public override string FriendlyName()
		{
			return "Buffed Long Value Key >=";
		}

		public override bool MayRequireID()
		{
			return true;
		}

#if VTC_EDITOR
		public override bool UI_TextValue_Uses() { return true; }
		public override string UI_TextValue_Label() { return "Buffed Long Value"; }
		public override void UI_TextValue_Set(string value) { double.TryParse(value, out keyval); }
		public override string UI_TextValue_Get() { return keyval.ToString(); }

		List<IntValueKey> BuffedKeys = new List<IntValueKey> { IntValueKey.ArmorLevel, IntValueKey.MaxDamage };
		List<string> BuffedKeyNames = new List<string> { IntValueKey.ArmorLevel.ToString(), IntValueKey.MaxDamage.ToString() };

		public override bool UI_ActsOnCombo_Uses() { return true; }
		public override string UI_ActsOnCombo_Label() { return "Acts on"; }
		public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return BuffedKeyNames.AsReadOnly(); }
		public override int UI_ActsOnCombo_Get() { return BuffedKeys.IndexOf(vk); }
		public override void UI_ActsOnCombo_Set(int index) { vk = BuffedKeys[index]; }
		public override System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return System.Drawing.Color.DarkRed; }
#endif
	}
	#endregion BuffedLongValKeyGE

	#region BuffedDoubleValKeyGE
	internal class BuffedDoubleValKeyGE : iLootRule
	{
		public double keyval = 0d;
		public DoubleValueKey vk = DoubleValueKey.AttackBonus;

		public BuffedDoubleValKeyGE() { }
		public BuffedDoubleValKeyGE(double k, DoubleValueKey v) { keyval = k; vk = v; }

		public override eLootRuleType GetRuleType() { return eLootRuleType.BuffedDoubleValKeyGE; }

#if VTC_PLUGIN
		public override bool Match(GameItemInfo id)
		{
			MagItemInfo magItemInfo = new MagItemInfo(id);

			double val = magItemInfo.GetBuffedDoubleValueKey(vk);

			return (float)val >= (float)keyval;
		}

		public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
		{
			hasdecision = false;
			ismatch = false;
		}
#endif

		public override void Read(System.IO.StreamReader inf, int profileversion)
		{
			keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
			vk = (DoubleValueKey)Convert.ToUInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
		}

		public override void Write(CountedStreamWriter inf)
		{
			inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
			inf.WriteLine(Convert.ToString((int)vk, System.Globalization.CultureInfo.InvariantCulture));
		}

		public override string DisplayString()
		{
			return string.Format("Buffed {0} >= {1}", vk, keyval);
		}

		public override string FriendlyName()
		{
			return "Buffed Double Value Key >=";
		}

		public override bool MayRequireID()
		{
			return true;
		}

#if VTC_EDITOR
		public override bool UI_TextValue_Uses() { return true; }
		public override string UI_TextValue_Label() { return "Buffed Double Value"; }
		public override void UI_TextValue_Set(string value) { double.TryParse(value, out keyval); }
		public override string UI_TextValue_Get() { return keyval.ToString(); }

		List<DoubleValueKey> BuffedKeys = new List<DoubleValueKey> { DoubleValueKey.AttackBonus, DoubleValueKey.ElementalDamageVersusMonsters, DoubleValueKey.ManaCBonus, DoubleValueKey.MeleeDefenseBonus };
		List<string> BuffedKeyNames = new List<string> { DoubleValueKey.AttackBonus.ToString(), DoubleValueKey.ElementalDamageVersusMonsters.ToString(), DoubleValueKey.ManaCBonus.ToString(), DoubleValueKey.MeleeDefenseBonus.ToString() };

		public override bool UI_ActsOnCombo_Uses() { return true; }
		public override string UI_ActsOnCombo_Label() { return "Acts on"; }
		public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return BuffedKeyNames.AsReadOnly(); }
		public override int UI_ActsOnCombo_Get() { return BuffedKeys.IndexOf(vk); }
		public override void UI_ActsOnCombo_Set(int index) { vk = BuffedKeys[index]; }
		public override System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return System.Drawing.Color.DarkRed; }
#endif
	}
	#endregion BuffedDoubleValKeyGE

	#region CalcdBuffedTinkedDamageGE
	internal class CalcdBuffedTinkedDamageGE : iLootRule
	{
		public double keyval = 0d;

		public CalcdBuffedTinkedDamageGE() { }
		public CalcdBuffedTinkedDamageGE(int k) { keyval = k; }

		public override eLootRuleType GetRuleType() { return eLootRuleType.CalcdBuffedTinkedDamageGE; }

#if VTC_PLUGIN
		public override bool Match(GameItemInfo id)
		{
			MagItemInfo magItemInfo = new MagItemInfo(id);

			return magItemInfo.CalcedBuffedTinkedDamage >= keyval;
		}

		public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
		{
			if (id.ObjectClass == ObjectClass.MeleeWeapon || id.ObjectClass == ObjectClass.MissileWeapon)
			{
				hasdecision = false;
				ismatch = false;        //Doesn't matter, just have to assign
			}
			else
			{
				hasdecision = true;
				ismatch = false;
			}
		}
#endif

		public override void Read(System.IO.StreamReader inf, int profileversion)
		{
			keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
		}

		public override void Write(CountedStreamWriter inf)
		{
			inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
		}

		public override string DisplayString()
		{
			return String.Format("Calced Buffed Tinked Dmg >= {0}", keyval);
		}

		public override string FriendlyName()
		{
			return "Calced Buffed Tinked Dmg >=";
		}

		public override bool MayRequireID()
		{
			return true;
		}

#if VTC_EDITOR
		public override bool UI_TextValue_Uses() { return true; }
		public override string UI_TextValue_Label() { return "Calced Buffed Tinked Damage"; }
		public override void UI_TextValue_Set(string value) { double.TryParse(value, out keyval); }
		public override string UI_TextValue_Get() { return keyval.ToString(); }
#endif
	}
	#endregion CalcdBuffedTinkedDamageGE

	#region TotalRatingsGE
	internal class TotalRatingsGE : iLootRule
	{
		public double keyval = 0d;

		public TotalRatingsGE() { }
		public TotalRatingsGE(int k) { keyval = k; }

		public override eLootRuleType GetRuleType() { return eLootRuleType.TotalRatingsGE; }

#if VTC_PLUGIN
		public override bool Match(GameItemInfo id)
		{
			MagItemInfo magItemInfo = new MagItemInfo(id);

			return magItemInfo.TotalRatings >= keyval;
		}

		public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
		{
			//if (id.ObjectClass == ObjectClass.Misc | id.ObjectClass == ObjectClass.Armor | id.ObjectClass == ObjectClass.Clothing | id.ObjectClass == ObjectClass.Jewelry)
			{
				hasdecision = false;
				ismatch = false;        //Doesn't matter, just have to assign
			}/*
			else
			{
				hasdecision = true;
				ismatch = false;
			}*/
		}
#endif

		public override void Read(System.IO.StreamReader inf, int profileversion)
		{
			keyval = GameInfo.HaxConvertDouble(inf.ReadLine());
		}

		public override void Write(CountedStreamWriter inf)
		{
			inf.WriteLine(Convert.ToString(keyval, System.Globalization.CultureInfo.InvariantCulture));
		}

		public override string DisplayString()
		{
			return String.Format("Calced Total Ratings >= {0}", keyval);
		}

		public override string FriendlyName()
		{
			return "Calced Total Ratings >=";
		}

		public override bool MayRequireID()
		{
			return true;
		}

#if VTC_EDITOR
		public override bool UI_TextValue_Uses() { return true; }
		public override string UI_TextValue_Label() { return "Calced Total Ratings"; }
		public override void UI_TextValue_Set(string value) { double.TryParse(value, out keyval); }
		public override string UI_TextValue_Get() { return keyval.ToString(); }
#endif
	}
	#endregion TotalRatingsGE

	#region CalcedBuffedTinkedTargetMeleeGE
	internal class CalcedBuffedTinkedTargetMeleeGE : iLootRule
	{
		double targetCalcedBuffedTinkedDoT;
		double targetBuffedMeleeDefenseBonus;
		double targetBuffedAttackBonus;

		public override eLootRuleType GetRuleType() { return eLootRuleType.CalcedBuffedTinkedTargetMeleeGE; }

#if VTC_PLUGIN
		public override bool Match(GameItemInfo id)
		{
			MagItemInfo magItemInfo = new MagItemInfo(id);

			return magItemInfo.CanReachTargetValues(targetCalcedBuffedTinkedDoT, targetBuffedMeleeDefenseBonus, targetBuffedAttackBonus);
		}

		public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
		{
			if (id.ObjectClass == ObjectClass.MeleeWeapon)
			{
				hasdecision = false;
				ismatch = false;        //Doesn't matter, just have to assign
			}
			else
			{
				hasdecision = true;
				ismatch = false;
			}
		}
#endif

		public override void Read(System.IO.StreamReader inf, int profileversion)
		{
			targetCalcedBuffedTinkedDoT = double.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
			targetBuffedMeleeDefenseBonus = double.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
			targetBuffedAttackBonus = double.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
		}

		public override void Write(CountedStreamWriter inf)
		{
			inf.WriteLine(targetCalcedBuffedTinkedDoT);
			inf.WriteLine(targetBuffedMeleeDefenseBonus);
			inf.WriteLine(targetBuffedAttackBonus);
		}

		public override string DisplayString()
		{
			return String.Format("Melee Target: {0} DoT: {1} md, {2} a", targetCalcedBuffedTinkedDoT, targetBuffedMeleeDefenseBonus, targetBuffedAttackBonus);
		}

		public override string FriendlyName()
		{
			return "Calced Buffed Tinked Target Melee";
		}

		public override bool MayRequireID()
		{
			return true;
		}

#if VTC_EDITOR
		public override bool UI_TextValue_Uses() { return true; }
		public override string UI_TextValue_Label() { return "Target Calced Buffed Tinked DoT"; }
		public override void UI_TextValue_Set(string value) { double.TryParse(value, out targetCalcedBuffedTinkedDoT); }
		public override string UI_TextValue_Get() { return targetCalcedBuffedTinkedDoT.ToString(); }

		public override bool UI_TextValue2_Uses() { return true; }
		public override string UI_TextValue2_Label() { return "Target Buffed Melee Defense Bonus"; }
		public override void UI_TextValue2_Set(string value) { double.TryParse(value, out targetBuffedMeleeDefenseBonus); }
		public override string UI_TextValue2_Get() { return targetBuffedMeleeDefenseBonus.ToString(); }

		public override bool UI_TextValue3_Uses() { return true; }
		public override string UI_TextValue3_Label() { return "Target Buffed Attack Bonus"; }
		public override void UI_TextValue3_Set(string value) { double.TryParse(value, out targetBuffedAttackBonus); }
		public override string UI_TextValue3_Get() { return targetBuffedAttackBonus.ToString(); }
#endif
	}
	#endregion CalcedBuffedTinkedTargetMeleeGE
	// Mag-nus added, end.

	#region DisabledRule
	internal class DisabledRule : iLootRule
    {
        public Boolean b = true;

        public DisabledRule() { }
        public DisabledRule(Boolean b) { this.b = b; }

        public override eLootRuleType GetRuleType() { return eLootRuleType.DisabledRule; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            return !b;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = false;
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            b = "true".Equals(inf.ReadLine());
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(b ? "true" : "false");
        }

        public override string DisplayString()
        {
            return b ? "This rule is currently disabled" : "This rule is active";
        }

        public override string FriendlyName()
        {
            return "Enable/Disable";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return false; }
        public override string UI_TextValue_Label() { return String.Empty; }
        public override void UI_TextValue_Set(string value) { }
        public override string UI_TextValue_Get() { return String.Empty; }

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Status"; }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return new ReadOnlyCollection<string>(new String[] {"Disabled", "Enabled"}); }
        public override int UI_ActsOnCombo_Get() { return b ? 0 : 1; }
        public override void UI_ActsOnCombo_Set(int index) { b = index < 1; }
        public override System.Drawing.Color UI_ActsOnCombo_OptionColors(int index) { return System.Drawing.Color.White; }
#endif
    }
    #endregion DisabledRule

    #region AnySimilarColor
    internal class AnySimilarColor : iLootRule
    {
        public System.Drawing.Color EColor = System.Drawing.Color.White;
        public double MaxDifferenceSV = 0.1d;
        public double MaxDifferenceH = 10d;

        public AnySimilarColor() { }

        public override eLootRuleType GetRuleType() { return eLootRuleType.AnySimilarColor; }

#if VTC_PLUGIN
        public static void ColorToHSV(System.Drawing.Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public override bool Match(GameItemInfo id)
        {
            double ht, st, vt;
            ColorToHSV(EColor, out ht, out st, out vt);

            foreach (uTank2.LootPlugins.GameItemInfo.PaletteData pal in id.Palettes)
            {
                System.Drawing.Color exc = pal.ExampleColor;

                double h, s, v;
                ColorToHSV(exc, out h, out s, out v);

                //Distance between h
                if (Math.Abs(h - ht) > MaxDifferenceH) continue;

                //Distance between sv
                double ss = s - st;
                double vv = v - vt;
                double svdist = Math.Sqrt(ss * ss + vv * vv);
                if (svdist > MaxDifferenceSV) continue;

                //Success
                return true;

                /*
                //Euclidean distance for colors
                int r = exc.R - EColor.R;
                int g = exc.G - EColor.G;
                int b = exc.B - EColor.B;
                int dist = (int)Math.Floor(Math.Sqrt(r * r + g * g + b * b));

                if (dist > MaxDifference) continue;

                //Match!
                return true;
                */
            }

            return false;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = Match(id);
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            int r = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            int g = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            int b = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            MaxDifferenceH = double.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            MaxDifferenceSV = double.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);

            EColor = System.Drawing.Color.FromArgb(r, g, b);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(EColor.R);
            inf.WriteLine(EColor.G);
            inf.WriteLine(EColor.B);
            inf.WriteLine(MaxDifferenceH);
            inf.WriteLine(MaxDifferenceSV);
        }

        public override string DisplayString()
        {
            return String.Format("Any Color {0}: {1}, {2}", EColor, MaxDifferenceH, MaxDifferenceSV);
        }

        public override string FriendlyName()
        {
            return "One Similar Color";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "RGB color value (hex)"; }
        public override void UI_TextValue_Set(string value) { int p; int.TryParse(value, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out p); EColor = System.Drawing.Color.FromArgb(unchecked(p | (int)0xFF000000)); }
        public override string UI_TextValue_Get() { return (EColor.ToArgb() & 0xFFFFFF).ToString("X"); }

        public override bool UI_TextValue2_Uses() { return true; }
        public override string UI_TextValue2_Label() { return "Max diff (Hue 0-255)"; }
        public override void UI_TextValue2_Set(string value) { double.TryParse(value, out MaxDifferenceH); }
        public override string UI_TextValue2_Get() { return MaxDifferenceH.ToString(); }

        public override bool UI_TextValue3_Uses() { return true; }
        public override string UI_TextValue3_Label() { return "Max diff (S/V 0-1)"; }
        public override void UI_TextValue3_Set(string value) { double.TryParse(value, out MaxDifferenceSV); }
        public override string UI_TextValue3_Get() { return MaxDifferenceSV.ToString(); }

        public override bool UI_ColorSample_Uses() { return true; }
        public override System.Drawing.Color UI_ColorSample_Get() { return EColor; }
#endif
    }
    #endregion AnySimilarColor

    #region SimilarColorArmorType
    internal class SimilarColorArmorType : iLootRule
    {
        public System.Drawing.Color EColor = System.Drawing.Color.White;
        public String ArmorGroup;
        public double MaxDifferenceSV = 0.1d;
        public double MaxDifferenceH = 10d;

        private static SortedDictionary<String, int[]> armorGroupDefinitions = null;
        private static ReadOnlyCollection<String> armorGroups = null;

        public SimilarColorArmorType()
        {
            ArmorGroup = ArmorGroups()[0];
        }

        public override eLootRuleType GetRuleType() { return eLootRuleType.SimilarColorArmorType; }

        public static ReadOnlyCollection<String> ArmorGroups()
        {
            if (armorGroups == null)
            {
                List<string> keys = new List<string>();
                foreach (string k in ColorXML.SlotDefinitions.Keys)
                    keys.Add(k);
                armorGroups = keys.AsReadOnly();
            }
            return armorGroups;
        }

        public static int[] PaletteIndices(String armorGroup)
        {
            if (ColorXML.SlotDefinitions.ContainsKey(armorGroup))
            {
                return ColorXML.SlotDefinitions[armorGroup];
            }
            return new int[] { };
        }

#if VTC_PLUGIN
        public static void ColorToHSV(System.Drawing.Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public override bool Match(GameItemInfo id)
        {
            double ht, st, vt;
            ColorToHSV(EColor, out ht, out st, out vt);

            List<int> indices = new List<int>(PaletteIndices(ArmorGroup));

            for (int i = 0; i < id.Palettes.Count; i++)
            {
                if (indices.Contains(i))
                {

                    uTank2.LootPlugins.GameItemInfo.PaletteData pal = id.Palettes[i];

                    System.Drawing.Color exc = pal.ExampleColor;

                    double h, s, v;
                    ColorToHSV(exc, out h, out s, out v);

                    //Distance between h
                    if (Math.Abs(h - ht) > MaxDifferenceH) continue;

                    //Distance between sv
                    double ss = s - st;
                    double vv = v - vt;
                    double svdist = Math.Sqrt(ss * ss + vv * vv);
                    if (svdist > MaxDifferenceSV) continue;

                    //Success
                    return true;

                    /*
                    //Euclidean distance for colors
                    int r = exc.R - EColor.R;
                    int g = exc.G - EColor.G;
                    int b = exc.B - EColor.B;
                    int dist = (int)Math.Floor(Math.Sqrt(r * r + g * g + b * b));

                    if (dist > MaxDifference) continue;

                    //Match!
                    return true;
                    */
                }
            }

            return false;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = Match(id);
        }
#endif
        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            int r = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            int g = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            int b = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            MaxDifferenceH = double.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            MaxDifferenceSV = double.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);

            EColor = System.Drawing.Color.FromArgb(r, g, b);

            ArmorGroup = inf.ReadLine();
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(EColor.R);
            inf.WriteLine(EColor.G);
            inf.WriteLine(EColor.B);
            inf.WriteLine(MaxDifferenceH);
            inf.WriteLine(MaxDifferenceSV);
            inf.WriteLine(ArmorGroup);
        }

        public override string DisplayString()
        {
            return String.Format("{0}: {1}, {2}, {3}", ArmorGroup, EColor, MaxDifferenceH, MaxDifferenceSV);
        }

        public override string FriendlyName()
        {
            return "Armor Type Similar Color";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR

        public override bool UI_ActsOnCombo_Uses() { return true; }
        public override string UI_ActsOnCombo_Label() { return "Armor Type/Color Region"; }
        public override void UI_ActsOnCombo_Set(int index) { ArmorGroup = ArmorGroups()[index]; }
        public override int UI_ActsOnCombo_Get() { return ArmorGroups().IndexOf(ArmorGroup); }
        public override ReadOnlyCollection<string> UI_ActsOnCombo_Options() { return ArmorGroups(); }

        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "RGB color value (hex)"; }
        public override void UI_TextValue_Set(string value) { int p; int.TryParse(value, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out p); EColor = System.Drawing.Color.FromArgb(unchecked(p | (int)0xFF000000)); }
        public override string UI_TextValue_Get() { return (EColor.ToArgb() & 0xFFFFFF).ToString("X"); }

        public override bool UI_TextValue2_Uses() { return true; }
        public override string UI_TextValue2_Label() { return "Max diff (Hue 0-255)"; }
        public override void UI_TextValue2_Set(string value) { double.TryParse(value, out MaxDifferenceH); }
        public override string UI_TextValue2_Get() { return MaxDifferenceH.ToString(); }

        public override bool UI_TextValue3_Uses() { return true; }
        public override string UI_TextValue3_Label() { return "Max diff (S/V 0-1)"; }
        public override void UI_TextValue3_Set(string value) { double.TryParse(value, out MaxDifferenceSV); }
        public override string UI_TextValue3_Get() { return MaxDifferenceSV.ToString(); }

        public override bool UI_ColorSample_Uses() { return true; }
        public override System.Drawing.Color UI_ColorSample_Get() { return EColor; }
#endif
    }
    #endregion SimilarColorArmorType

    #region SlotSimilarColor
    internal class SlotSimilarColor : iLootRule
    {
        public System.Drawing.Color EColor = System.Drawing.Color.White;
        public double MaxDifferenceSV = 0.1d;
        public double MaxDifferenceH = 10d;
        public int Slot;

        public SlotSimilarColor() { }

        public override eLootRuleType GetRuleType() { return eLootRuleType.SlotSimilarColor; }

#if VTC_PLUGIN
        public static void ColorToHSV(System.Drawing.Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public override bool Match(GameItemInfo id)
        {
            double ht, st, vt;
            ColorToHSV(EColor, out ht, out st, out vt);

            if (id.Palettes.Count <= Slot) return false;
            uTank2.LootPlugins.GameItemInfo.PaletteData pal = id.Palettes[Slot];

            System.Drawing.Color exc = pal.ExampleColor;

            double h, s, v;
            ColorToHSV(exc, out h, out s, out v);

            //Distance between h
            if (Math.Abs(h - ht) > MaxDifferenceH) return false;

            //Distance between sv
            double ss = s - st;
            double vv = v - vt;
            double svdist = Math.Sqrt(ss * ss + vv * vv);
            if (svdist > MaxDifferenceSV) return false;

            //Success
            return true;
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = Match(id);
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            int r = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            int g = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            int b = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            MaxDifferenceH = double.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            MaxDifferenceSV = double.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            Slot = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);

            EColor = System.Drawing.Color.FromArgb(r, g, b);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(EColor.R);
            inf.WriteLine(EColor.G);
            inf.WriteLine(EColor.B);
            inf.WriteLine(MaxDifferenceH);
            inf.WriteLine(MaxDifferenceSV);
            inf.WriteLine(Slot);
        }

        public override string DisplayString()
        {
            return String.Format("Slot {0} Color {1}: {2}, {3}", Slot, EColor, MaxDifferenceH, MaxDifferenceSV);
        }

        public override string FriendlyName()
        {
            return "Slot Similar Color";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR
        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "RGB color value (hex)"; }
        public override void UI_TextValue_Set(string value) { int p; int.TryParse(value, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out p); EColor = System.Drawing.Color.FromArgb(unchecked(p | (int)0xFF000000)); }
        public override string UI_TextValue_Get() { return (EColor.ToArgb() & 0xFFFFFF).ToString("X"); }

        public override bool UI_TextValue2_Uses() { return true; }
        public override string UI_TextValue2_Label() { return "Max diff (Hue 0-255)"; }
        public override void UI_TextValue2_Set(string value) { double.TryParse(value, out MaxDifferenceH); }
        public override string UI_TextValue2_Get() { return MaxDifferenceH.ToString(); }

        public override bool UI_TextValue3_Uses() { return true; }
        public override string UI_TextValue3_Label() { return "Max diff (S/V 0-1)"; }
        public override void UI_TextValue3_Set(string value) { double.TryParse(value, out MaxDifferenceSV); }
        public override string UI_TextValue3_Get() { return MaxDifferenceSV.ToString(); }

        public override bool UI_TextValue4_Uses() { return true; }
        public override string UI_TextValue4_Label() { return "Palette Entry #"; }
        public override void UI_TextValue4_Set(string value) { int.TryParse(value, out Slot); }
        public override string UI_TextValue4_Get() { return Slot.ToString(); }

        public override bool UI_ColorSample_Uses() { return true; }
        public override System.Drawing.Color UI_ColorSample_Get() { return EColor; }
#endif
    }
    #endregion SlotSimilarColor

    #region SlotExactPalette
    internal class SlotExactPalette : iLootRule
    {
        public int Palette;
        public int Slot;

        public SlotExactPalette() { }

        public override eLootRuleType GetRuleType() { return eLootRuleType.SlotExactPalette; }

#if VTC_PLUGIN
        public override bool Match(GameItemInfo id)
        {
            if (id.Palettes.Count <= Slot) return false;
            uTank2.LootPlugins.GameItemInfo.PaletteData pal = id.Palettes[Slot];

            int ppal = pal.Palette & 0xFFFFFF;
            int mpal = Palette & 0xFFFFFF;

            return (ppal == mpal);
        }

        public override void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            hasdecision = true;
            ismatch = Match(id);
        }
#endif

        public override void Read(System.IO.StreamReader inf, int profileversion)
        {
            Slot = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            Palette = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void Write(CountedStreamWriter inf)
        {
            inf.WriteLine(Slot);
            inf.WriteLine(Palette);
        }

        public override string DisplayString()
        {
            return String.Format("Slot {0} Palette 0x{1:X}", Slot, Palette);
        }

        public override string FriendlyName()
        {
            return "Slot Exact Palette";
        }

        public override bool MayRequireID()
        {
            return false;
        }

#if VTC_EDITOR
        public override bool UI_TextValue2_Uses() { return true; }
        public override string UI_TextValue2_Label() { return "Palette ID"; }
        public override void UI_TextValue2_Set(string value) { int p; int.TryParse(value, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out Palette); }
        public override string UI_TextValue2_Get() { return (Palette & 0xFFFFFF).ToString("X"); }

        public override bool UI_TextValue_Uses() { return true; }
        public override string UI_TextValue_Label() { return "Palette Entry #"; }
        public override void UI_TextValue_Set(string value) { int.TryParse(value, out Slot); }
        public override string UI_TextValue_Get() { return Slot.ToString(); }
#endif
    }
    #endregion SlotExactPalette

    #endregion LootRule classes

    #region cLootItemRule (requirement set) and cLootRules (rule set)
    //A set of rules with an action attached
    internal class cLootItemRule : iSettingsCollection
    {
        public List<iLootRule> IntRules = new List<iLootRule>();
        public int pri = 0;
        public eLootAction act = eLootAction.Keep;
        public int LootActionData = 0;
        public string name = "";
        public string CustomExpression = "";

        public cLootItemRule()
        {
        }

        public int Priority() { return pri; }
        public eLootAction Action() { return act; }

        public bool AnyReqRequiresID()
        {
            foreach (iLootRule i in IntRules)
            {
                if (i.MayRequireID())
                {
                    return true;
                }
            }
            return false;
        }

#if VTC_PLUGIN
        public bool Match(GameItemInfo id)
        {
            foreach (iLootRule R in IntRules)
                if (!R.Match(id)) return false;
            return true;
        }

        public void EarlyMatch(GameItemInfo id, out bool hasdecision, out bool ismatch)
        {
            bool needid = false;
            foreach (iLootRule R in IntRules)
            {
                bool hd;
                bool im;
                R.EarlyMatch(id, out hd, out im);

                if (hd && (!im))
                {
                    hasdecision = true;
                    ismatch = false;
                    return;
                }

                if (!hd)
                    needid = true;
            }

            if (needid)
            {
                hasdecision = false;
                ismatch = false;
            }
            else
            {
                hasdecision = true;
                ismatch = true;
            }
        }
#endif

        #region iSettingsCollection Members

        public void Read(System.IO.StreamReader inf, int profileversion)
        {
            //Name
            name = inf.ReadLine();

            //Custom Expression
            if (UTLVersionInfo.VersionHasFeature(eUTLFileFeature.RuleExpression, profileversion))
                CustomExpression = inf.ReadLine();
            else
                CustomExpression = "";

            //The 'big line'
            string[] clines = inf.ReadLine().Split(new char[] { ';' });

            //Priority, Action are encoded in the 'big line'
            pri = Convert.ToInt32(clines[0], System.Globalization.CultureInfo.InvariantCulture);
            act = (eLootAction)Convert.ToInt32(clines[1], System.Globalization.CultureInfo.InvariantCulture);

            //Read extra info on the lootaction
            if (act == eLootAction.KeepUpTo)
            {
                LootActionData = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
            }

            //Rules...also encoded in the 'big line'
            IntRules.Clear();
            for (int i = 2; i < clines.Length; ++i)
            {
                int ruletype = Convert.ToInt32(clines[i], System.Globalization.CultureInfo.InvariantCulture);
                iLootRule newrule;
                newrule = LootRuleCreator.CreateLootRule((eLootRuleType)ruletype);

                if (UTLVersionInfo.VersionHasFeature(eUTLFileFeature.RequirementLengthCode, profileversion))
                {
                    int lengthcode = int.Parse(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                    if (newrule != null)
                    {
                        newrule.Read(inf, profileversion);
                        IntRules.Add(newrule);
                    }
                    else
                    {
                        //Skip
                        char[] b = new char[lengthcode];
                        inf.Read(b, 0, lengthcode);
                        cUnsupportedRequirement Unsup = new cUnsupportedRequirement();
                        Unsup.data = b;
                        IntRules.Add(Unsup);
                    }
                }
                else
                {
                    //Uncoded length, hope it is not null because if it is we are just going to crash
                    newrule.Read(inf, profileversion);
                    IntRules.Add(newrule);
                }
            }
        }

        public void Write(CountedStreamWriter inf)
        {
            //Name
            inf.WriteLine(name);

            //Custom Expression
            inf.WriteLine(CustomExpression);

            //Compose the 'big line'
            StringBuilder s = new StringBuilder();
            s.Append(Convert.ToString(pri, System.Globalization.CultureInfo.InvariantCulture));
            s.Append(";");
            s.Append(Convert.ToString((int)act, System.Globalization.CultureInfo.InvariantCulture));
            foreach (iLootRule lr in IntRules)
            {
                s.Append(";");
                s.Append(Convert.ToString((int)lr.GetRuleType(), System.Globalization.CultureInfo.InvariantCulture));
            }
            inf.WriteLine(s.ToString());

            if (act == eLootAction.KeepUpTo)
            {
                inf.WriteLine(LootActionData.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }

            foreach (iLootRule lr in IntRules)
            {
                //Write to a temp buffer so we can generate a length prefix
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CountedStreamWriter sw = new CountedStreamWriter(ms);

                lr.Write(sw);
                sw.Flush();

                inf.WriteLine(sw.Count);
                ms.Position = 0;
                System.IO.StreamReader sr = new System.IO.StreamReader(ms);
                inf.Write(sr.ReadToEnd());
            }
        }

        #endregion
    }

    internal class cLootRules
    {
        public List<cLootItemRule> Rules = new List<cLootItemRule>();
        public UTLFileExtraBlockManager ExtraBlockManager = new UTLFileExtraBlockManager();
        public int UTLFileVersion = UTLVersionInfo.MAX_PROFILE_VERSION;

        public cLootRules()
        {
            ExtraBlockManager.CreateDefaultBlocks();
        }

#if VTC_PLUGIN
        public eLootAction Classify(GameItemInfo id, out string matchedrulename, out int data1)
        {
            foreach (cLootItemRule R in Rules)
                if (R.Match(id))
                {
                    matchedrulename = R.name;
                    data1 = R.LootActionData;
                    return R.Action();
                }

            matchedrulename = "";
            data1 = 0;
            return eLootAction.NoLoot;
        }

        public bool NeedsID(GameItemInfo id)
        {
            bool iswalking = false;
            eLootAction walkaction = eLootAction.NoLoot;

            foreach (cLootItemRule R in Rules)
            {
                if (iswalking && (R.act != walkaction))
                    return true;

                bool hd;
                bool im;
                R.EarlyMatch(id, out hd, out im);

                //All higher priority rules don't need ID, and don't match
                //thus if this rule matches and doesn't need ID, we know it
                //will also be the final match and we don't need to ID
                if (hd && im)
                {
#if DEBUGMSG
                    if (iswalking)
                        LootCore.WriteToChat("Walking to next match saved an ID on " + id.GetValueString(StringValueKey.Name, "") + "!");
#endif
                    return false;
                }

                if (!hd)
                {
                    //As long as the action is the same, priority is irrelevant,
                    //so continue to match as long as action is the same
                    iswalking = true;
                    walkaction = R.act;
                    //return true;
                }
            }

            if (iswalking)
            {
                return true;
            }
            else
            {
                //Nothing needs an ID, and nothing matches
                return false;
            }
        }
#endif

        public void Clear()
        {
            Rules.Clear();
        }

        #region iSettingsCollection Members

        public bool Read(System.IO.StreamReader inf, int none)
        {
            try
            {
                Rules.Clear();

                //Version 0 files start with a rulecount at the top,
                //later versions start with
                //UTL
                //file version
                //rulecount
                string firstline = inf.ReadLine();
                int count;
                if (firstline == "UTL")
                {
                    UTLFileVersion = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);

                    if (UTLFileVersion > UTLVersionInfo.MAX_PROFILE_VERSION)
                        throw new Exception("Profile file is version " + UTLFileVersion.ToString() + ", only version " + UTLVersionInfo.MAX_PROFILE_VERSION.ToString() + " is supported by this version of VTClassic.");

                    count = Convert.ToInt32(inf.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    UTLFileVersion = 0;
                    count = Convert.ToInt32(firstline, System.Globalization.CultureInfo.InvariantCulture);
                }

                //Read rules
                for (int i = 0; i < count; ++i)
                {
                    cLootItemRule R = new cLootItemRule();
                    R.Read(inf, UTLFileVersion);
                    Rules.Add(R);
                }

                //Read extra info
                ExtraBlockManager.Read(inf);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Write(CountedStreamWriter inf)
        {
            try
            {
                inf.WriteLine("UTL");
                inf.WriteLine(UTLVersionInfo.MAX_PROFILE_VERSION);

                inf.WriteLine(Convert.ToString(Rules.Count, System.Globalization.CultureInfo.InvariantCulture));
                foreach (cLootItemRule R in Rules)
                    R.Write(inf);

                ExtraBlockManager.Write(inf);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
    #endregion cLootItemRule (requirement set) and cLootRules (rule set)
}
