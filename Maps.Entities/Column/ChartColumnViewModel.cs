using System;

namespace Maps.Entities
{
    public class ChartColumnViewModel
    {
        public string Name { get; set; }

        public UserDataType DataType { get; set; }

        public Guid Id { get; set; }

        public ChartColumnViewModel() { }
        public ChartColumnViewModel(Column column)
        {
            Name = column.Name;
            DataType = column.DataType;
            Id = column.Id;
        }
    }
}
