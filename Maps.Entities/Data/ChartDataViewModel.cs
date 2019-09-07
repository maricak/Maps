using System.Collections.Generic;

namespace Maps.Entities
{
    public class ChartDataViewModel
    {
        public string LayerName { get; set; }

        public bool HasData { get; set; }

        public IDictionary<string, HorizontalBarViewModel> ChartModels { get; set; }

        public ChartDataViewModel()
        {
            ChartModels = new Dictionary<string, HorizontalBarViewModel>();
        }

        public ChartDataViewModel(Layer layer)
        {
            if (layer != null)
            {
                LayerName = layer.Name;
                HasData = layer.HasData;
                ChartModels = new Dictionary<string, HorizontalBarViewModel>();
                if (layer.HasData)
                {
                    foreach (var column in layer.Columns)
                    {
                        if (column.HasChart)
                        {
                            ChartModels.Add(column.Name, new HorizontalBarViewModel(column));
                        }
                    }
                }
            }
        }
    }
}