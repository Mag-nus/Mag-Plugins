using System;
using System.Collections.Generic;
using System.IO;

namespace GenericSettingsFile
{
    /// <summary>
    /// A parser to read a text file into a SettingsCollection dictionary of named settings
    /// </summary>
    class SettingsFileLoader
    {
        public SettingsCollection ReadSettingsFile(string filepath)
        {
            if (string.IsNullOrEmpty(filepath)) { throw new Exception("ReadSettingsFile received empty filename"); }
            if (!File.Exists(filepath)) { throw new Exception("Missing file: " + filepath); }
            using (var file = new StreamReader(filepath))
            {
                var contents = file.ReadToEnd();
                return ReadSettingsFromText(contents);
            }
        }
        public SettingsCollection ReadSettingsFromText(string text)
        {
            var stringSeps = new string[] { "\r\n" };
            var lines = text.Split(stringSeps, StringSplitOptions.RemoveEmptyEntries);
            var settings = ParseSettings(lines);
            return settings;
        }
        public SettingsCollection ParseSettings(IEnumerable<string> lines)
        {
            var settings = new SettingsCollection();
            foreach (string line in lines)
            {
                var lineParser = new SettingsLineParser();
                Setting setting = lineParser.ExtractLine(line);
                settings.AddSetting(setting);
            }
            return settings;
        }
    }
}
