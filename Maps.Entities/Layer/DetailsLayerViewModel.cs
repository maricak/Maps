using System;
using System.Collections.Generic;
using System.Web;

namespace Maps.Entities
{
    public class DetailsLayerViewModel
    {
        public Guid MapId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool HasData { get; set; }

        public HttpPostedFileBase DataFile { get; set; }

        public List<DetailsDataViewModel> Data { get; set; }

        public DetailsLayerViewModel()
        {
            Data = new List<DetailsDataViewModel>();
        }

        public DetailsLayerViewModel(Layer layer)
        {
            Id = layer.Id;
            Name = layer.Name;
            if (layer.Map != null)
            {
                MapId = layer.Map.Id;
            }
            HasData = layer.HasData;
            Data = new List<DetailsDataViewModel>();
            if (layer.Data!= null)
            {
                foreach (var data in layer.Data)
                {
                    Data.Add(new DetailsDataViewModel(data));
                }
            }
        }

        public DetailsLayerViewModel(Guid mapId)
        {
            MapId = mapId;
        }
    }
}
