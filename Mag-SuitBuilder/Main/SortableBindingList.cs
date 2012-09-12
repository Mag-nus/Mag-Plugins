using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mag_SuitBuilder
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
	}
}
