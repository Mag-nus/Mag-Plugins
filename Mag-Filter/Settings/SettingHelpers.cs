using System;
using System.Collections.Generic;
using System.Text;

namespace GenericSettingsFile
{
    public static class SettingHelpers
    {
        public static string GetSingleStringValue(SettingsCollection coll, string key)
        {
            var setting = coll.GetValue(key);
            if (setting.ParameterCount > 0)
            {
                throw new Exception(string.Format("Setting '{0}' has multiple parameters", key));
            }
            if (!setting.HasSingleValue)
            {
                return null;
            }
            return setting.SingleValue;
        }
        public static int GetSingleIntValue(SettingsCollection coll, string key)
        {
            string text = GetSingleStringValue(coll, key);
            int value;
            if (!int.TryParse(text, out value))
            {
                throw new Exception(string.Format("Setting '{0}' failed to parse as int: '{1}'", key, value));
            }
            return value;
        }

        public static bool GetSingleBoolValue(SettingsCollection coll, string key, bool defVal)
        {
            string text = GetSingleStringValue(coll, key);
            return ObjToBool(text, defVal);
        }
        private static bool EqStr(string text1, string text2)
        {
            return (string.Compare(text1, text2, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        private static bool ObjToBool(object obj, bool defval)
        {
            if (obj == null) { return defval; }
            string text = obj.ToString();
            if (EqStr(text, "True") || EqStr(text, "Yes")) { return true; }
            if (EqStr(text, "False") || EqStr(text, "No")) { return false; }
            bool bval = defval;
            bool.TryParse(text, out bval);
            return bval;
        }

        public static DateTime GetSingleDateTimeValue(SettingsCollection coll, string key)
        {
            string text = GetSingleStringValue(coll, key);
            DateTime value;
            if (!DateTime.TryParse(text, out value))
            {
                throw new Exception(string.Format("Setting '{0}' failed to parse as DateTime: '{1}'", key, value));
            }
            return value;
        }
    }
}
