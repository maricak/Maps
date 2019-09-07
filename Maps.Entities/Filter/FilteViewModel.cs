using System;

namespace Maps.Entities
{
    public enum FilterType { DISTANCE, RANGE, UNIQE_LIST }

    public class FilterViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsVisible { get; set; }

        public FilterType Type { get; set; }

        public FilterViewModel() { }

        public FilterViewModel(Column column)
        {
            if (column != null)
            {
                Id = column.Id;
                Name = column.Name;
                IsVisible = column.IsFilterVisible;
                Type = column.HasChart ? FilterType.UNIQE_LIST : FilterType.RANGE;
            }
        }

        public FilterViewModel(Layer layer)
        {
            if (layer != null)
            {
                Id = layer.Id;
                Name = "Distance";
                IsVisible = layer.IsFilterVisible;
                Type = FilterType.DISTANCE;
            }
        }
    }
}
