using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maps.Entities
{
    public class HorizontalBarUniqueStringModelView
    {
        public JArray LabelArray { get; set; }

        public JArray CountArray { get; set; }

        public Guid ColumnId { get; set; }
        public string ColumnName { get; set; }

        public HorizontalBarUniqueStringModelView(ChartColumnViewModel column, ICollection<Data> data)
        {
            ColumnName = column.Name;
            ColumnId = column.Id;
            var res = data.GroupBy(d => d.Values[column.Name])
                    .Select(grp => new
                    {
                        label = grp.Key,
                        count = grp.Select(d => d.Values[column.Name]).Count()
                    });
            LabelArray = new JArray(res.Select(r => r.label).ToArray());
            CountArray = new JArray(res.Select(r => r.count).ToArray());
        }
    }
}
