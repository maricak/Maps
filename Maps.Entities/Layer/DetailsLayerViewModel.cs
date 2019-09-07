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

        public DetailsLayerViewModel()
        {
            CollapseModel = new DetailsCollapseLayerViewModel();
            HeaderModel = new DetailsHeaderLayerViewModel();
        }

        public DetailsLayerViewModel(Layer layer)
        {
            HeaderModel = new DetailsHeaderLayerViewModel(layer);
            CollapseModel = new DetailsCollapseLayerViewModel(layer);
        }

        public DetailsLayerViewModel(Guid mapId)
        {
            HeaderModel = new DetailsHeaderLayerViewModel
            {
                MapId = mapId
            };
            CollapseModel = new DetailsCollapseLayerViewModel();
        }
    }
}