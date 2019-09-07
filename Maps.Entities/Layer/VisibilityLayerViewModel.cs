using System;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel used for changing visibility of the given layer on a map.
    /// </summary>
    public class VisibilityLayerViewModel
    {
        public Guid Id { get; set; }

        public bool IsVisible { get; set; }

        public VisibilityLayerViewModel() { }

        public VisibilityLayerViewModel(Layer layer)
        {
            if (layer != null)
            {
                Id = layer.Id;
                IsVisible = layer.IsVisible;
            }
        }
    }
}