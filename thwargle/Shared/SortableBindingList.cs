using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mag.Shared
{
	class SortableBindingList<T> : BindingList<T> where T : class
	{
		protected override bool SupportsSortingCore { get { return true; } }

		private bool isSorted;
		private ListSortDirection sortDirection;
		private PropertyDescriptor sortProperty;

		protected override bool IsSortedCore { get { return isSorted; } }

		protected override ListSortDirection SortDirectionCore { get { return sortDirection; } }

		protected override PropertyDescriptor SortPropertyCore { get { return sortProperty; } }

		protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			sortProperty = prop;
			sortDirection = direction;

			List<T> items = (List<T>)Items;

			items.Sort(delegate(T lhs, T rhs)
			{
				isSorted = true;
				object lhsValue = lhs == null ? null : prop.GetValue(lhs);
				object rhsValue = rhs == null ? null : prop.GetValue(rhs);
				int result = Comparer.Default.Compare(lhsValue, rhsValue);

				if (direction == ListSortDirection.Descending)
					result = -result;

				return result;
			});
		}

		protected override void RemoveSortCore()
		{
			sortDirection = ListSortDirection.Ascending;
			sortProperty = null;
		}


		/// <summary>
		/// Sorts using the default IComparer of T
		/// </summary>
		public void Sort()
		{
			sort(null, null);
		}

		public void Sort(IComparer<T> comparer)
		{
			sort(comparer, null);
		}

		public void Sort(Comparison<T> comparison)
		{
			sort(null, comparison);
		}

		private void sort(IComparer<T> comparer, Comparison<T> comparison)
		{
			sortProperty = null;
			sortDirection = ListSortDirection.Ascending;

			//Extract items and sort separately
			List<T> sortList = new List<T>();
			foreach (var item in this)
				sortList.Add(item);

			if (comparison == null)
				sortList.Sort(comparer);
			else
				sortList.Sort(comparison);

			//Disable notifications, rebuild, and re-enable notifications
			bool oldRaise = RaiseListChangedEvents;
			RaiseListChangedEvents = false;

			try
			{
				ClearItems();
				sortList.ForEach(item => Add(item));
			}
			finally
			{
				RaiseListChangedEvents = oldRaise;
				ResetBindings();
			}

		}
	}
}
