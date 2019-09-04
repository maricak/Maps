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

        public DetailsHeaderLayerViewModel() { }
        public DetailsHeaderLayerViewModel(Layer layer)
        {
            if (layer != null)
            {
                Id = layer.Id;
                Name = layer.Name;
            }
        }
    }
}
