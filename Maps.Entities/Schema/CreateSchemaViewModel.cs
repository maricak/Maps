using System;
using System.Collections.Generic;
using System.Linq;

namespace Maps.Entities
{
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
