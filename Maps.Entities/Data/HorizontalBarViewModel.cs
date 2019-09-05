using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maps.Entities
{
    /// <summary>
    /// ModelView that contains data that will be
    /// displayed in a Horizontal bar chart.
    /// </summary>
    public class HorizontalBarViewModel
    {
        /// <summary>
        /// Array of labels.
        /// </summary>
        public JArray LabelArray { get; set; }

        /// <summary>
        /// Array of number of entities with corresponding label.
        /// </summary>
        public JArray CountArray { get; set; }

        public Guid ColumnId { get; set; }
        public string ColumnName { get; set; }

        public HorizontalBarViewModel(ChartColumnViewModel column, ICollection<Data> data)
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