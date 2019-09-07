using System.Collections.Generic;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for displaying all data regarding the filtering 
    /// inside a given layer: icon change, visibility change...
    /// </summary>
    public class FilterLayerViewModel
    {
        public SelectIconLayerViewModel SelectIcon { get; set; }

        public VisibilityLayerViewModel Visibility { get; set; }

        public IList<FilterViewModel> ColumnFilters { get; set; }

        public FilterLayerViewModel()
        {
            SelectIcon = new SelectIconLayerViewModel();
            Visibility = new VisibilityLayerViewModel();
            ColumnFilters = new List<FilterViewModel>();
        }

        public FilterLayerViewModel(Layer layer)
        {
            SelectIcon = new SelectIconLayerViewModel(layer);
            Visibility = new VisibilityLayerViewModel(layer);
            ColumnFilters = new List<FilterViewModel>();
            if (layer != null)
            {
                foreach (var column in layer.Columns)
                {
                    if (column.HasChart || column.DataType == UserDataType.NUMBER)
                    {
                        ColumnFilters.Add(new FilterViewModel(column));
                    }
                }
                ColumnFilters.Add(new FilterViewModel(layer));
            }
        }
    }
}