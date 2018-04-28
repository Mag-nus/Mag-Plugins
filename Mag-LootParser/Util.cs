
namespace Mag_LootParser
{
    static class Util
    {
        public static int IndexOfNth(string input, char value, int nth = 1)
        {
            int offset = input.IndexOf(value);

            for (int i = 1; i < nth; i++)
            {
                if (offset == -1) return -1;
                offset = input.IndexOf(value, offset + 1);
            }

            return offset;
        }
    }
}
