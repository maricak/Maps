using System;

namespace Maps.Entities
{
    public class DetailsHeaderLayerViewModel
    {
        public Guid MapId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DetailsHeaderLayerViewModel() { }
        public DetailsHeaderLayerViewModel(Layer layer)
        {
            if (layer != null)
            {
                if (layer.Map != null)
                {
                    MapId = layer.Map.Id;
                }
                Id = layer.Id;
                Name = layer.Name;
            }
        }
    }
}
