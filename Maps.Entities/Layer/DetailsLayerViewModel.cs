using System;
using System.Collections.Generic;
using System.Web;

namespace Maps.Entities
{
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

        public DetailsLayerViewModel(Guid mapId)
        {
            HeaderModel = new DetailsHeaderLayerViewModel
            {
                MapId = mapId
            };
        }
    }
}
