using System;

namespace Mag_WorldObjectParser
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

		public static string ByteArrayToHex(byte[] array, bool splitWithSpaces = true)
		{
			if (splitWithSpaces)
			{
				char[] c = new char[array.Length * 2 + (Math.Max(0, array.Length - 1))];

				for (int i = 0; i < array.Length; ++i)
				{
					byte b = ((byte)(array[i] >> 4));

					c[i * 2 + i] = (char)(b > 9 ? b + 0x37 : b + 0x30);

					b = ((byte)(array[i] & 0xF));

					c[i * 2 + 1 + i] = (char)(b > 9 ? b + 0x37 : b + 0x30);

					if (i < array.Length - 1)
						c[i * 2 + 2 + i] = (char)0x20;
				}

				return new string(c);
			}
			else
			{
				char[] c = new char[array.Length * 2];

				for (int i = 0; i < array.Length; ++i)
				{
					byte b = ((byte)(array[i] >> 4));

					c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);

					b = ((byte)(array[i] & 0xF));

					c[i * 2 + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
				}

				return new string(c);
			}
		}

		public static byte[] HexStringToByteArray(string hex)
		{
			byte[] bytes = new byte[hex.Length / 2];

			for (int i = 0; i < hex.Length; i += 2)
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

			return bytes;
		}
	}
}
