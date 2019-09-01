using System.Collections.Generic;

namespace Maps.Entities
{
    public class ChartDataViewModel
    {
        public string LayerName { get; set; }
        public bool HasData { get; set; }
        public IList<ChartColumnViewModel> Columns { get; set; }

        public IDictionary<string, HorizontalBarUniqueStringModelView> ChartModels { get; set; }

        public ChartDataViewModel()
        {
            Columns = new List<ChartColumnViewModel>();
        }
        public ChartDataViewModel(Layer layer)
        {
            if (layer != null)
            {
                LayerName = layer.Name;
                HasData = layer.HasData;
                Columns = new List<ChartColumnViewModel>();
                ChartModels = new Dictionary<string, HorizontalBarUniqueStringModelView>();
                if (layer.HasData && layer.Data != null)
                {
                    foreach (var column in layer.Columns)
                    {
                        var c = new ChartColumnViewModel(column);
                        Columns.Add(c);
                        if (column.HasChart)
                        {
                            ChartModels.Add(column.Name, new HorizontalBarUniqueStringModelView(c, layer.Data));
                        }
                    }
                }
            }
        }
    }
}
