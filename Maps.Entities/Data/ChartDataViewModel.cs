using System;
using System.Collections.Generic;

namespace Maps.Entities
{
    public class ChartDataViewModel
    {
        public string LayerName { get; set; }
        public bool HasData { get; set; }
        public IList<DetailsDataViewModel> Data { get; set; }
        public IList<ChartColumnViewModel> Columns { get; set; }

        public IDictionary<string, HorizontalBarUniqueStringModelView> ChartModels { get; set; }

        public ChartDataViewModel()
        {
            Data = new List<DetailsDataViewModel>();
            Columns = new List<ChartColumnViewModel>();
        }
        public ChartDataViewModel(Layer layer)
        {
            if (layer != null)
            {
                LayerName = layer.Name;
                HasData = layer.HasData;

                Data = new List<DetailsDataViewModel>();
                Columns = new List<ChartColumnViewModel>();
                ChartModels = new Dictionary<string, HorizontalBarUniqueStringModelView>();
                if (layer.Data != null)
                {
                    foreach (var data in layer.Data)
                    {
                        Data.Add(new DetailsDataViewModel(data));
                    }
                    foreach (var column in layer.Columns)
                    {
                        var c = new ChartColumnViewModel(column);
                        Columns.Add(c);

                        if(column.DataType == UserDataType.STRING || column.DataType == UserDataType.NUMBER)
                        {
                            ChartModels.Add(column.Name, new HorizontalBarUniqueStringModelView(c, Data));
                        }
                    }
                }
            }
        }
    }
}
