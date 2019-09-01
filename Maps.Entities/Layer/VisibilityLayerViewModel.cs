using System;

namespace Maps.Entities
{
    public class VisibilityLayerViewModel
    {
        public Guid Id { get; set; }

        public bool IsVisible { get; set; }

        public VisibilityLayerViewModel() { }
        public VisibilityLayerViewModel(Layer layer)
        {
            Id = layer.Id;
            IsVisible = layer.IsVisible;
        }
    }
}
