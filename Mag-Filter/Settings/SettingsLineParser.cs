using System;
using System.Collections.Generic;

namespace GenericSettingsFile
{
    /// <summary>
    /// A setting is a single line in a setting file
    /// and holds the setting name along with either 
    /// One value, e.g., character:JohnDoe
    /// or multiple (space-separated) values, e.g., characters=primary:JohnDoe secondary:JaneDoe
    /// Single or double quotes may be used around values,
    ///     e.g., names=primary:"John Doe" secondary:'Jane Doe'
    /// </summary>
    public class Setting
    {
        public Setting(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            Name = name;
            Parameters = new Dictionary<string, string>();
        }

        public string Name { get; private set; }
        public bool HasSingleValue { get { return SingleValue != null; } }
        public string SingleValue { get; set; }
        public bool HasParameters { get { return SingleValue == null; } }
        public int ParameterCount { get { return (Parameters == null ? 0 : Parameters.Count); } }
        public IDictionary<string, string> Parameters { get; set; }

        // Extracting parameters from within a multivalue
        public string GetStringParam(string key)
        {
            if (!this.Parameters.ContainsKey(key)) { throw new Exception(string.Format("Missing string param: '{0}'", key)); }
            return this.Parameters[key];
        }
        public DateTime GetDateParam(string key)
        {
            if (!this.Parameters.ContainsKey(key)) { throw new Exception(string.Format("Missing date param: '{0}'", key)); }
            var styles = System.Globalization.DateTimeStyles.RoundtripKind;
            return DateTime.Parse(this.Parameters[key], null, styles);
        }
        public DateTime GetUtcDateParam(string key)
        {
            DateTime timeRaw = GetDateParam(key);
            DateTime timeUtc = timeRaw.ToUniversalTime();
            return timeUtc;
        }
    }
    /// <summary>
    /// Class to parse a single line into a Setting (used by SettingsFileParser)
    /// </summary>
    public class SettingsLineParser
    {
        public Setting ExtractLine(string line)
        {
            Setting setting = null;
            var pos = line.IndexOfAny(new[] { '=', ':' });
            if (pos == -1)
            {
                // Entire line is setting name, no parameters at all
                setting = new Setting(line);
                setting.SingleValue = "";
            }
            else
            {
                setting = new Setting(line.Substring(0, pos));
                if (pos + 1 < line.Length)
                {
                    string value = line.Substring(pos + 1);
                    if (line[pos] == ':')
                    {
                        setting.SingleValue = value;
                    }
                    else
                    {
                        // extract space separated key/value parameters
                        setting.Parameters = ExtractParameters(value);
                    }
                }
            }
            return setting;
        }

        private IDictionary<string, string> ExtractParameters(string paramString)
        {
            var oldPos = 0;
            var items = new Dictionary<string, string>();
            while (true)
            {
                var pos = paramString.IndexOf(':', oldPos);
                if (pos == -1)
                    break;  // no more properties
                var name = paramString.Substring(oldPos, pos - oldPos);
                name = name.Trim();


                oldPos = pos + 1; //set that value starts after name and colon
                if (oldPos >= paramString.Length)
                {
                    items.Add(name, paramString.Substring(oldPos));
                    break;//last item and without value
                }
                if (paramString[oldPos] == '"')
                {
                    // jump to before quote
                    oldPos += 1;
                    pos = paramString.IndexOf('"', oldPos);
                    items.Add(name, paramString.Substring(oldPos, pos - oldPos));
                }
                else if (paramString[oldPos] == '\'')
                {
                    // jump to before quote
                    oldPos += 1;
                    pos = paramString.IndexOf('\'', oldPos);
                    items.Add(name, paramString.Substring(oldPos, pos - oldPos));
                }
                else
                {
                    pos = paramString.IndexOf(' ', oldPos);
                    if (pos == -1)
                    {
                        items.Add(name, paramString.Substring(oldPos));
                        break;//no more items
                    }

                    items.Add(name, paramString.Substring(oldPos, pos - oldPos));
                }


                oldPos = pos + 1;
            }

            return items;

        }

        public KeyValuePair<string, string> ExtractValue(string value, int pos1, int pos2)
        {
            var keyValue = value.Substring(pos1, pos2 - pos1 + 1);
            var colonPos = keyValue.IndexOf(':');
            if (colonPos == -1)
                throw new FormatException("Expected a colon for property " + keyValue);

            return new KeyValuePair<string, string>(keyValue.Substring(0, colonPos),
                keyValue.Substring(colonPos + 1));
        }
    }

    /*
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Should_be_able_to_extract_name_from_a_line()
        {
            var line = "G195=Out:LED0799,LED0814,Flags:L-N Desc:\"EAF-QCH-B1-01\" Invert:00 STO:35 SP:0 FStart: FStop: ";

            var sut = new SettingsParser();
            var actual = sut.ExtractLine(line);

            Assert.AreEqual("G195", actual.Name);
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void Setting_name_is_required()
        {
            var line = "G195 malformed";

            var sut = new SettingsParser();
            sut.ExtractLine(line);
        }


        [TestMethod, ExpectedException(typeof(FormatException))]
        public void equals_must_be_before_first_colon()
        {
            var line = "G195:malformed name=value";

            var sut = new SettingsParser();
            sut.ExtractLine(line);
        }

        [TestMethod]
        public void Should_be_able_to_extract_a_single_parameter()
        {
            var line = "G195=Out:LED0799";

            var sut = new SettingsParser();
            var actual = sut.ExtractLine(line);

            Assert.AreEqual("LED0799", actual.Parameters["Out"]);
        }

        [TestMethod]
        public void should_be_able_to_parse_multiple_properties()
        {
            var line = "G195=Out:LED0799 Invert:00";

            var sut = new SettingsParser();
            var actual = sut.ExtractLine(line);

            Assert.AreEqual("00", actual.Parameters["Invert"]);
        }

        [TestMethod]
        public void should_be_able_to_include_spaces_in_value_names_if_they_are_wrapped_by_quotes()
        {
            var line = "G195=Out:\"LED0799 Invert:00\"";

            var sut = new SettingsParser();
            var actual = sut.ExtractLine(line);

            Assert.AreEqual("LED0799 Invert:00", actual.Parameters["Out"]);
        }

        [TestMethod]
        public void second_parameter_value_should_also_be_able_To_be_quoted()
        {
            var line = "G195=In:Stream Out:\"LED0799 Invert:00\"";

            var sut = new SettingsParser();
            var actual = sut.ExtractLine(line);

            Assert.AreEqual("LED0799 Invert:00", actual.Parameters["Out"]);
        }

        [TestMethod]
        public void allow_empty_values()
        {
            var line = "G195=In:";

            var sut = new SettingsParser();
            var actual = sut.ExtractLine(line);

            Assert.AreEqual("", actual.Parameters["In"]);
        }

        [TestMethod]
        public void allow_empty_values_even_if_its_not_the_last()
        {
            var line = "G195=In: Out:Heavy";

            var sut = new SettingsParser();
            var actual = sut.ExtractLine(line);

            Assert.AreEqual("", actual.Parameters["In"]);
        }
    }
     * */
}
