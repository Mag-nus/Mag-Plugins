using System;
using System.Collections.Generic;
using System.IO;
using MagTools.Trackers.Combat.Standard;
using MagTools.Trackers.Combat.Aetheria;
using MagTools.Trackers.Combat.Cloaks;

using Decal.Adapter;

namespace MagTools.Trackers.Combat
{
	class CombatTracker : IDisposable
	{
		readonly StandardTracker standardTracker;
		readonly AetheriaTracker aetheriaTracker;
		readonly CloakTracker cloakTracker;

		public CombatTracker()
		{
			try
			{
				standardTracker = new StandardTracker();
				aetheriaTracker = new AetheriaTracker();
				cloakTracker = new CloakTracker();

				standardTracker.CombatEvent += new Action<CombatEventArgs>(standardTracker_CombatEvent);
				aetheriaTracker.SurgeEvent += new Action<Aetheria.SurgeEventArgs>(aetheriaTracker_SurgeEvent);
				cloakTracker.SurgeEvent += new Action<Cloaks.SurgeEventArgs>(cloakTracker_SurgeEvent);

				CoreManager.Current.CharacterFilter.Login += new EventHandler<Decal.Adapter.Wrappers.LoginEventArgs>(CharacterFilter_Login);
				CoreManager.Current.CharacterFilter.Logoff += new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private bool disposed;

		public void Dispose()
		{
			Dispose(true);

			// Use SupressFinalize in case a subclass
			// of this type implements a finalizer.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// If you need thread safety, use a lock around these 
			// operations, as well as in your methods that use the resource.
			if (!disposed)
			{
				if (disposing)
				{
					standardTracker.Dispose();
					aetheriaTracker.Dispose();
					cloakTracker.Dispose();

					CoreManager.Current.CharacterFilter.Login -= new EventHandler<Decal.Adapter.Wrappers.LoginEventArgs>(CharacterFilter_Login);
					CoreManager.Current.CharacterFilter.Logoff -= new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void CharacterFilter_Login(object sender, Decal.Adapter.Wrappers.LoginEventArgs e)
		{
			try
			{
				if (Settings.SettingsManager.CombatTracker.Persistent.Value)
				{
					CombatTrackerImporter importer = new CombatTrackerImporter(PluginCore.PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker.xml");

					importer.Import(combatInfos, aetheriaInfos, cloakInfos);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_Logoff(object sender, Decal.Adapter.Wrappers.LogoffEventArgs e)
		{
			try
			{
				if (Settings.SettingsManager.CombatTracker.Persistent.Value)
				{
					CombatTrackerExporter exporter = new CombatTrackerExporter(combatInfos, aetheriaInfos, cloakInfos);

					exporter.Export(PluginCore.PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker.xml");
				}

				if (Settings.SettingsManager.CombatTracker.ExportOnLogOff.Value)
				{
					if (combatInfos.Count != 0 || aetheriaInfos.Count != 0 || cloakInfos.Count != 0)
					{
						CombatTrackerExporter exporter = new CombatTrackerExporter(combatInfos, aetheriaInfos, cloakInfos);

						exporter.Export(PluginCore.PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker." + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".xml");
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		readonly List<CombatInfo> combatInfos = new List<CombatInfo>();
		readonly List<AetheriaInfo> aetheriaInfos = new List<AetheriaInfo>();
		readonly List<CloakInfo> cloakInfos = new List<CloakInfo>();

		void standardTracker_CombatEvent(CombatEventArgs obj)
		{
			//Debug.WriteToChat("Combat: SourceName: " + obj.SourceName + ", TargetName: " + obj.TargetName + ", AttackType: " + obj.AttackType + ", DamageElemenet: " + obj.DamageElemenet + ", IsKillingBlow: " + obj.IsKillingBlow + ", IsCriticalHit: " + obj.IsCriticalHit + ", IsFailedAttack: " + obj.IsFailedAttack + ", DamageAmount: " + obj.DamageAmount);

			// We only track events that interact with us
			if (obj.SourceName != CoreManager.Current.CharacterFilter.Name && obj.TargetName != CoreManager.Current.CharacterFilter.Name)
				return;

			CombatInfo combatInfo = combatInfos.Find(i => i.SourceName == obj.SourceName && i.TargetName == obj.TargetName);

			if (combatInfo == null)
			{
				combatInfo = new CombatInfo(obj.SourceName, obj.TargetName);

				combatInfos.Add(combatInfo);
			}

			combatInfo.AddFromCombatEventArgs(obj);

			if (CombatInfoUpdated != null)
				CombatInfoUpdated(combatInfo);
		}

		void aetheriaTracker_SurgeEvent(Aetheria.SurgeEventArgs obj)
		{
			//Debug.WriteToChat("Aetheria: SourceName: " + obj.SourceName + ", TargetName: " + obj.TargetName + ", SurgeType: " + obj.SurgeType);

			// We only track our own surge events
			if (obj.SourceName != CoreManager.Current.CharacterFilter.Name)
				return;

			AetheriaInfo aetheriaInfo = aetheriaInfos.Find(i => i.SourceName == obj.SourceName && i.TargetName == obj.TargetName);

			if (aetheriaInfo == null)
			{
				aetheriaInfo = new AetheriaInfo(obj.SourceName, obj.TargetName);

				aetheriaInfos.Add(aetheriaInfo);
			}

			aetheriaInfo.AddFromSurgeEventArgs(obj);

			if (AetheriaInfoUpdated != null)
				AetheriaInfoUpdated(aetheriaInfo);
		}

		void cloakTracker_SurgeEvent(Cloaks.SurgeEventArgs obj)
		{
			//Debug.WriteToChat("Cloak: SourceName: " + obj.SourceName + ", TargetName: " + obj.TargetName + ", SurgeType: " + obj.SurgeType);

			// We only track our own surge events
			if (obj.SourceName != CoreManager.Current.CharacterFilter.Name)
				return;

			CloakInfo cloakInfo = cloakInfos.Find(i => i.SourceName == obj.SourceName && i.TargetName == obj.TargetName);

			if (cloakInfo == null)
			{
				cloakInfo = new CloakInfo(obj.SourceName, obj.TargetName);

				cloakInfos.Add(cloakInfo);
			}

			cloakInfo.AddFromSurgeEventArgs(obj);

			if (CloakInfoUpdated != null)
				CloakInfoUpdated(cloakInfo);
		}

		public event Action<bool> InfoCleared;
		public event Action<CombatInfo> CombatInfoUpdated;
		public event Action<AetheriaInfo> AetheriaInfoUpdated;
		public event Action<CloakInfo> CloakInfoUpdated;

		public List<CombatInfo> GetCombatInfos(string name)
		{
			return combatInfos.FindAll(i => i.SourceName == name || i.TargetName == name);
		}

		public List<AetheriaInfo> GetAetheriaInfos(string name)
		{
			return aetheriaInfos.FindAll(i => i.SourceName == name || i.TargetName == name);
		}

		public List<CloakInfo> GetCloakInfos(string name)
		{
			return cloakInfos.FindAll(i => i.SourceName == name || i.TargetName == name);
		}

		public void ClearCurrentStats()
		{
			combatInfos.Clear();
			aetheriaInfos.Clear();
			cloakInfos.Clear();

			if (InfoCleared != null)
				InfoCleared(true);
		}

		public void ExportCurrentStats()
		{
			string fileName = PluginCore.PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker." + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".xml";

			CombatTrackerExporter exporter = new CombatTrackerExporter(combatInfos, aetheriaInfos, cloakInfos);

			exporter.Export(fileName);

			CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Stats exported to: " + fileName, 5);
		}

		public void ClearPersistantStats()
		{
			FileInfo fileInfo = new FileInfo(PluginCore.PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.Server + @"\" + CoreManager.Current.CharacterFilter.Name + ".CombatTracker.xml");

			if (fileInfo.Exists)
			{
				fileInfo.Delete();

				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "File deleted: " + fileInfo.FullName, 5);
			}
			else
			{
				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "No persistant stats found: " + fileInfo.FullName, 5);
			}
		}
	}
}
