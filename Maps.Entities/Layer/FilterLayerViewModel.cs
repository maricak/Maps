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

        public IList<IFilterViewModel> Filters { get; set; }

        public FilterLayerViewModel()
        {
            SelectIcon = new SelectIconLayerViewModel();
            Visibility = new VisibilityLayerViewModel();
            Filters = new List<IFilterViewModel>();
        }

        public FilterLayerViewModel(IList<string> icons)
        {
            SelectIcon = new SelectIconLayerViewModel(icons);
            Visibility = new VisibilityLayerViewModel();
            Filters = new List<IFilterViewModel>();
        }

        public FilterLayerViewModel(Layer layer, IList<string> icons)
        {
            SelectIcon = new SelectIconLayerViewModel(layer, icons);
            Visibility = new VisibilityLayerViewModel(layer);
            Filters = new List<IFilterViewModel>();
            if (layer != null)
            {
                foreach (var column in layer.Columns)
                {
                    if (column.HasChart)
                    {
                        Filters.Add(new UniqueListFilterViewModel(column));
                    }
                    else if (column.DataType != UserDataType.STRING)
                    {
                        Filters.Add(new RangeFilterViewModel(column));
                    }
                }

                Filters.Add(new DistanceFilterViewModel(layer));
            }
        }
    }
}