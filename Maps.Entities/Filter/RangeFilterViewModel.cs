using System;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for filtering data by numeric columns. This filter
    /// checks whether the value of the data is inside the given range.
    /// </summary>
    public class RangeFilterViewModel : IFilterViewModel
    {
        FilterType IFilterViewModel.Type => FilterType.RANGE;

        public Guid Id { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Range start is required")]
        public double RangeStart { get; set; }

        public bool IsVisible { get; set; }

        [Required(ErrorMessage = "Range end is required")]
        public double RangeEnd { get; set; }

        public RangeFilterViewModel() { }

        public RangeFilterViewModel(Column column)
        {
            Id = column.Id;
            Name = column.Name;
            IsVisible = column.IsFilterVisible;
            RangeStart = (double)column.Filter["min"];
            RangeEnd = (double)column.Filter["max"];
        }
    }
}