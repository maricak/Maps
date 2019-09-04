using System;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for dispaying all details of the given layer: 
    /// the ones inside the sidebar header and collapse.
    /// </summary>
    public class DetailsLayerViewModel
    {
        public DetailsCollapseLayerViewModel CollapseModel { get; set; }
        public DetailsHeaderLayerViewModel HeaderModel { get; set; }

        public DetailsLayerViewModel() { }

        public DetailsLayerViewModel(Layer layer)
        {
            HeaderModel = new DetailsHeaderLayerViewModel(layer);
            CollapseModel = new DetailsCollapseLayerViewModel(layer);
        }
    }
}
