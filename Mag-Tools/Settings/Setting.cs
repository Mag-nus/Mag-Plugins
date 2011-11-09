using System;

namespace MagTools.Settings
{
	class Setting<T>
	{
		public readonly string Xpath;

		public readonly string Description;

		private T value;
		public T Value
		{
			get
			{
				return value;
			}
			set
			{
				// If we're setting it to the value its already at, don't continue with the set.
				object currentValue = this.value;
				object newValue = value;

				if (typeof(T) == typeof(byte))
				{
					if ((byte)currentValue == (byte)newValue) return;
				}
				else if (typeof(T) == typeof(sbyte))
				{
					if ((sbyte)currentValue == (sbyte)newValue) return;
				}
				else if (typeof(T) == typeof(int))
				{
					if ((int)currentValue == (int)newValue) return;
				}
				else if (typeof(T) == typeof(uint))
				{
					if ((uint)currentValue == (uint)newValue) return;
				}
				else if (typeof(T) == typeof(short))
				{
					if ((short)currentValue == (short)newValue) return;
				}
				else if (typeof(T) == typeof(ushort))
				{
					if ((ushort)currentValue == (ushort)newValue) return;
				}
				else if (typeof(T) == typeof(long))
				{
					if ((long)currentValue == (long)newValue) return;
				}
				else if (typeof(T) == typeof(ulong))
				{
					if ((ulong)currentValue == (ulong)newValue) return;
				}
				else if (typeof(T) == typeof(float))
				{
					if (Math.Abs((float)currentValue - (float)newValue) < float.Epsilon) return;
				}
				else if (typeof(T) == typeof(double))
				{
					if (Math.Abs((double)currentValue - (double)newValue) < double.Epsilon) return;
				}
				else if (typeof(T) == typeof(char))
				{
					if ((char)currentValue == (char)newValue) return;
				}
				else if (typeof(T) == typeof(bool))
				{
					if ((bool)currentValue == (bool)newValue) return;
				}
				else if (typeof(T) == typeof(object))
				{
					if ((object)currentValue == (object)newValue) return;
				}
				else if (typeof(T) == typeof(string))
				{
					if ((string)currentValue == (string)newValue) return;
				}
				else if (typeof(T) == typeof(decimal))
				{
					if ((decimal)currentValue == (decimal)newValue) return;
				}

				// The value differs, set it.
				this.value = value;

				if (Changed != null)
					Changed(this);
			}
		}

		public event Action<Setting<T>> Changed;

		public Setting(string xpath, string description)
		{
			Xpath = xpath;

			Description = description;
		}
	}
}
