using System;
using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Spells;

namespace Mag_SuitBuilder.Search
{
	abstract class Searcher
	{
		protected readonly SearcherConfiguration Config = new SearcherConfiguration();
		protected readonly EquipmentGroup Equipment = new EquipmentGroup();
		protected readonly SuitBuilder SuitBuilder = new SuitBuilder();

		protected Searcher(SearcherConfiguration config, IEnumerable<SuitBuildableMyWorldObject> equipment, CompletedSuit startingSuit = null)
		{

			Config = config;

			foreach (var piece in equipment)
				Equipment.Add(piece);

			// Remove pieces that don't meet our minimum requirements
			for (int i = Equipment.Count - 1; i >= 0; i--)
			{
				if (!config.ItemPassesRules(Equipment[i]))
					Equipment.RemoveAt(i);
			}

			// Remove surpassed pieces
			for (int i = Equipment.Count - 1; i >= 0; i--)
			{
				if (Equipment.ItemIsSurpassed(Equipment[i]))
					Equipment.RemoveAt(i);
			}

			// If we were given a starting suit, lets start our SuitBuilder off with all those items
			if (startingSuit != null)
			{
				foreach (var o in startingSuit)
					SuitBuilder.Push(o.Value, o.Key);
			}

			// Remove pieces we can't add to our base suit, or pieces that can provide no beneficial spell
			for (int i = Equipment.Count - 1; i >= 0; i--)
			{
				// todo hack fix
				//if (!SuitBuilder.SlotIsOpen(Equipment[i].EquipableSlots) || !SuitBuilder.CanGetBeneficialSpellFrom(Equipment[i]))
				//	Equipment.RemoveAt(i);
			}
		}
				
		public event Action<CompletedSuit> SuitCreated;

		protected virtual void OnSuitCreated(CompletedSuit obj)
		{
			Action<CompletedSuit> handler = SuitCreated;

			if (handler != null)
				handler(obj);
		}

		public event Action SearchCompleted;

		protected virtual void OnSearchCompleted()
		{
			Action handler = SearchCompleted;

			if (handler != null)
				handler();
		}

		public bool Running { get; private set; }

		public void Start()
		{
			if (Running)
				return;

			Running = true;

			StartSearch();
		}

		public void Stop()
		{
			if (!Running)
				return;

			Running = false;
		}

		protected abstract void StartSearch();
	}
}
