using System;
using System.Web;

namespace Maps.Entities
{
    public class DetailsCollapseLayerViewModel
    {
        public Guid Id { get; set; }
        public bool HasData { get; set; }

        public HttpPostedFileBase DataFile { get; set; }

        public DetailsCollapseLayerViewModel() { }
        public DetailsCollapseLayerViewModel(Layer layer)
        {
            if (layer != null)
            {
                Id = layer.Id;
                HasData = layer.HasData;
            }
        }
    }
}
