using System;
using System.Globalization;

namespace MagFilter.Channels
{
    /// <summary>
    /// A Command is the data required to pass a command from the launcher to the game or vice-versa
    /// It consists of the command string and the timestamp associated with it
    /// The timestamp is in a format that serializes predictably in a round-trip format
    /// </summary>
    public class Command
    {
        public readonly DateTime TimeStampUtc;
        public readonly string CommandString;
        public Command(DateTime timeStampUtc, string commandString)
        {
            this.TimeStampUtc = RoundTrippableTime(timeStampUtc);
            this.CommandString = commandString;
        }
        public static DateTime RoundTrippableTime(DateTime time)
        {
            string text = time.ToString("o");
            const DateTimeStyles styles = DateTimeStyles.RoundtripKind;
            DateTime result = DateTime.Parse(text, null, styles);
            result = result.ToUniversalTime();
            return result;
        }
        public override string ToString()
        {
            return string.Format("{0:S}: {1}", TimeStampUtc, CommandString);
        }
    }
}
