using System;
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

        public DetailsLayerViewModel() { }

        public DetailsLayerViewModel(Layer layer)
        {
            Id = layer.Id;
            Name = layer.Name;
            if (layer.Map != null)
            {
                MapId = layer.Map.Id;
            }
            HasData = layer.HasData;
        }

        public DetailsLayerViewModel(Guid mapId)
        {
            MapId = mapId;
        }
    }
}
