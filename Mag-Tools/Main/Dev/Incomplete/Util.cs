/*
using System;
using System.Reflection;

namespace MagTools.VirindiTools
{
	/// <summary>
	/// This class is a work in progress.
	/// Right now it doesn't work reliably.
	/// </summary>
	static class Util
	{
		public static bool uTank2IsLoaded(Version minver)
		{
			try
			{
				//uTank2.eCombatSpellType temp = uTank2.eCombatSpellType.Arc;

				Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();

				foreach (Assembly a in asms)
				{
					AssemblyName nmm = a.GetName();

					if (nmm.Name == "uTank2" && nmm.Version >= minver)
						return true;
				}

				return false;
			}
			catch
			{
				return false;
			}
		}

		public static bool VTClassicIsLoaded(Version minver)
		{
			try
			{
				//VTClassic.LootCore core = null;

				Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();

				foreach (Assembly a in asms)
				{
					AssemblyName nmm = a.GetName();

					if (nmm.Name == "VTClassic" && nmm.Version >= minver)
						return true;
				}

				return false;
			}
			catch
			{
				return false;
			}
		}

		public static bool VirindiItemToolIsLoaded(Version minver)
		{
			try
			{
				//VirindiItemTool.PluginCore.ePluginActivityState temp = VirindiItemTool.PluginCore.ePluginActivityState.Idle;

				Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();

				foreach (Assembly a in asms)
				{
					AssemblyName nmm = a.GetName();

					if (nmm.Name == "VirindiItemTool" && nmm.Version >= minver)
						return true;
				}

				return false;
			}
			catch
			{
				return false;
			}
		}
	}
}
*/