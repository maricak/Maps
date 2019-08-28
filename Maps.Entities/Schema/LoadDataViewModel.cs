using System;
using System.Web;

namespace Maps.Entities
{
    public class LoadDataViewModel
    {
        public Guid LayerId { get; set; }
        public HttpPostedFileBase DataFile { get; set; }

        public LoadDataViewModel() { }
        public LoadDataViewModel(Guid layerId)
        {
            LayerId = layerId;
        }
    }
}
