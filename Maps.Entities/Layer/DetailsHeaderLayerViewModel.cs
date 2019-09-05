using System;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for the information about given layer 
    /// in the sidebar inside its header.
    /// </summary>
    public class DetailsHeaderLayerViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// When displaying view for creating a layer we have
        /// to know id of the map that will own it.
        /// </summary>
        public Guid MapId { get; set; }

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
