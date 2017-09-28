using System;
using System.Collections.Generic;
using System.Text;

namespace GenericSettingsFile
{
    /// <summary>
    /// A dictionary of settings parsed out of a settings file
    /// which is a file of settings lines (see Setting class for setting line syntax)
    /// </summary>
    public class SettingsCollection
    {
        private readonly Dictionary<string, Setting> _settings = new Dictionary<string, Setting>();
        public void AddSetting(Setting setting)
        {
            _settings[setting.Name] = setting;
        }
        public bool ContainsKey(string key) { return _settings.ContainsKey(key); }
        public Setting GetValue(string key)
        {
            if (!_settings.ContainsKey(key)) { throw new Exception(string.Format("Missing key '{0}'", key)); }
            return _settings[key];
        }
        public int Count { get { return _settings.Count; } }
    }
}
