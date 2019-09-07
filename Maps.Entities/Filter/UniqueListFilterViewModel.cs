using System;
using System.Collections.Generic;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for filtering data based on the all possible values 
    /// of the given column.
    /// </summary>
    public class UniqueListFilterViewModel : IFilterViewModel
    {
        FilterType IFilterViewModel.Type => FilterType.UNIQUE_LIST;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsVisible { get; set; }

        public IDictionary<string, bool> UniqueValues { get; set; }

        public UniqueListFilterViewModel()
        {
            UniqueValues = new Dictionary<string, bool>();
        }

        public UniqueListFilterViewModel(Column column)
        {
            Id = column.Id;
            Name = column.Name;
            IsVisible = column.IsFilterVisible;
            UniqueValues = new Dictionary<string, bool>();
            foreach (var property in column.Filter)
            {
                UniqueValues.Add(property.Key, (bool)property.Value);
            }
        }
    }
}