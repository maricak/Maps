using System;

namespace Maps.Entities
{
    public class DetailsLayerViewModel
    {
        public Guid MapId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DetailsLayerViewModel() { }

        public DetailsLayerViewModel(Layer layer)
        {
            Id = layer.Id;
            Name = layer.Name;
            if (layer.Map != null)
            {
                MapId = layer.Map.Id;
            }
        }

        public DetailsLayerViewModel(Guid mapId)
        {
            MapId = mapId;
        }
    }
}
