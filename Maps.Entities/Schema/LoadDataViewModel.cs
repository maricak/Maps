using System;
using System.Web;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for loading data to the layer from the input file.
    /// </summary>
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
