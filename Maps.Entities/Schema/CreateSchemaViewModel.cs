using System;
using System.Collections.Generic;
using System.Linq;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for input of schema(information about columns) for a given 
    /// layer and number of columns.
    /// </summary>
    public class CreateSchemaViewModel
    {
        public Guid LayerId { get; set; }

        public IList<CreateColumnViewModel> Columns { get; set; }

        public CreateSchemaViewModel()
        {
            Columns = new List<CreateColumnViewModel>();
        }

        public CreateSchemaViewModel(Guid layerId, int numColumns)
        {
            LayerId = layerId;
            Columns = Enumerable.Repeat(new CreateColumnViewModel(), numColumns).ToList();
        }
    }
}
