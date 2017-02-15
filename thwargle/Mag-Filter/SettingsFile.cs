using System;
using System.Collections.Generic;
using System.IO;

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
    }
    /// <summary>
    /// A parser to read a text file into a SettingsCollection dictionary of named settings
    /// </summary>
    class SettingsFileParser
    {
        public SettingsCollection ReadSettingsFile(string filepath)
        {
            var settings = new SettingsCollection();
            if (string.IsNullOrEmpty(filepath)) { throw new Exception("ReadSettingsFile received empty filename"); }
            if (!File.Exists(filepath)) { throw new Exception("Missing file: " + filepath); }
            // TODO - if file is locked by writer, then fail somehow
            using (var file = new StreamReader(filepath))
            {
                string contents = file.ReadToEnd();
                string[] stringSeps = new string[] { "\r\n" };
                string[] lines = contents.Split(stringSeps, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    var lineParser = new SettingsLineParser();
                    Setting setting = lineParser.ExtractLine(line);
                    settings.AddSetting(setting);
                }
            }
            return settings;
        }
    }
}
