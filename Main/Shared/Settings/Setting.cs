using System;

namespace Mag.Shared.Settings
{
	class Setting<T>
	{
		public readonly string Xpath;

		public readonly string Description;

		public readonly T DefaultValue;

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
				if (Object.Equals(this.value, value))
					return;

				// The value differs, set it.
				this.value = value;

				StoreValueInConfigFile();

				if (Changed != null)
					Changed(this);
			}
		}

		public event Action<Setting<T>> Changed;

		public Setting(string xpath, string description = null, T defaultValue = default(T))
		{
			Xpath = xpath;

			Description = description;

			DefaultValue = defaultValue;

			LoadValueFromConfig(defaultValue);
		}

		void LoadValueFromConfig(T defaultValue)
		{
			value = SettingsFile.GetSetting(Xpath, defaultValue);
		}

		void StoreValueInConfigFile()
		{
			SettingsFile.PutSetting(Xpath, value);
		}
	}
}
