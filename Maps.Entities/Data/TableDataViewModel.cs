using System.Collections.Generic;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for displaying layer data in a table.
    /// </summary>
    public class TableDataViewModel
    {
        public string LayerName { get; set; }
        public bool HasData { get; set; }
        public IList<DetailsDataViewModel> Data { get; set; }

        public TableDataViewModel()
        {
            Data = new List<DetailsDataViewModel>();
        }

        public TableDataViewModel(Layer layer)
        {
            if (layer != null)
            {
                LayerName = layer.Name;
                HasData = layer.HasData;
                Data = new List<DetailsDataViewModel>();
                if (layer.HasData && layer.Data != null)
                {
                    foreach (var data in layer.Data)
                    {
                        Data.Add(new DetailsDataViewModel(data));
                    }
                }
            }
        }
    }
}
