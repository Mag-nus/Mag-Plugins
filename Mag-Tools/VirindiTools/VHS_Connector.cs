using System;
using System.Collections.Generic;
using System.Text;
using Decal.Adapter;
using Decal.Adapter.Wrappers;
using System.Reflection;

namespace MagTools.VirindiTools
{
	static class VHS_Connector
	{
		public static bool IsVHSPresent()
		{
			try
			{
				//See if VCS assembly is loaded
				System.Reflection.Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
				bool loaded = false;
				foreach (System.Reflection.Assembly a in asms)
				{
					AssemblyName nmm = a.GetName();
					if ((nmm.Name == "VirindiHotkeySystem") && (nmm.Version >= new System.Version("1.0.0.0")))
					{
						loaded = true;
						break;
					}
				}

				if (loaded)
					if (Curtain_VHSInstanceEnabled())
						return true;
			}
			catch
			{

			}

			return false;
		}

		static bool Curtain_VHSInstanceEnabled()
		{
			return VirindiHotkeySystem.VHotkeySystem.Running;
		}
	}
}
