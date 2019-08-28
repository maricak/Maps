using System;

namespace Maps.Entities
{
    public class DetailsCollapseLayerViewModel
    {
        public Guid Id { get; set; }
        public bool HasData { get; set; }
        public bool HasColumns { get; set; }

        public DetailsCollapseLayerViewModel() { }
        public DetailsCollapseLayerViewModel(Layer layer)
        {
            if (layer != null)
            {
                Id = layer.Id;
                HasData = layer.HasData;
                HasColumns = layer.HasColumns;
            }
        }
    }
}
