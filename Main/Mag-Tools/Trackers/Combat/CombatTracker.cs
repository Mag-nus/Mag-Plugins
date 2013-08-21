using System;
using System.Collections.Generic;

using MagTools.Trackers.Combat.Standard;
using MagTools.Trackers.Combat.Aetheria;
using MagTools.Trackers.Combat.Cloaks;

using Mag.Shared;

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
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
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
		public event Action<List<CombatInfo>> StatsLoaded;
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

		public void ClearStats()
		{
			combatInfos.Clear();
			aetheriaInfos.Clear();
			cloakInfos.Clear();

			if (InfoCleared != null)
				InfoCleared(true);
		}

		public void ImportStats(string xmlFileName)
		{
			CombatTrackerImporter importer = new CombatTrackerImporter(xmlFileName);

			importer.Import(combatInfos, aetheriaInfos, cloakInfos);

			if (StatsLoaded != null)
				StatsLoaded(combatInfos);
		}

		public void ExportStats(string xmlFileName, bool showMessage = false)
		{
			if (combatInfos.Count == 0 && aetheriaInfos.Count == 0 && cloakInfos.Count == 0)
				return;

			CombatTrackerExporter exporter = new CombatTrackerExporter(combatInfos, aetheriaInfos, cloakInfos);

			exporter.Export(xmlFileName);

			if (showMessage)
				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Stats exported to: " + xmlFileName, 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);
		}
	}
}
