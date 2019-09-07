using Newtonsoft.Json.Linq;
using System;

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

        public HorizontalBarViewModel()
        {
            LabelArray = new JArray();
            CountArray = new JArray();
        }

        public HorizontalBarViewModel(Column column)
        {
            ColumnId = column.Id;
            ColumnName = column.Name;
            LabelArray = column.Chart["labels"] as JArray;
            CountArray = column.Chart["counts"] as JArray;
        }
    }
}