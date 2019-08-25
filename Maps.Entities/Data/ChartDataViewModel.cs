using System;
using System.Collections.Generic;

namespace Maps.Entities
{
    public class ChartDataViewModel
    {
        public string LayerName { get; set; }
        public bool HasData { get; set; }
        public IList<DetailsDataViewModel> Data { get; set; }

        public ChartDataViewModel()
        {
            Data = new List<DetailsDataViewModel>();
        }
        public ChartDataViewModel(Layer layer)
        {
            if (layer != null)
            {
                LayerName = layer.Name;
                HasData = layer.HasData;

                Data = new List<DetailsDataViewModel>();
                if (layer.Data != null)
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
