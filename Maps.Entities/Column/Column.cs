using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maps.Entities
{
    /// <summary>
    /// Posible columns types.
    /// </summary>
    public enum UserDataType { STRING, NUMBER, LONGITUDE, LATITUDE }

    /// <summary>
    /// Entity that represents one row in the Column table.
    /// </summary>
    [Table("Column")]
    public class Column
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public UserDataType DataType { get; set; }

        /// <summary>
        /// This column will have checkbox filters and data charts if this field is set.
        /// </summary>
        public bool HasChart { get; set; }

        /// <summary>
        /// For columns that have HasChart set here will be pair columnValue->count of data with that value.
        /// </summary>
        public string Chart_
        {
            get { return Chart != null ? JsonConvert.SerializeObject(Chart) : null; }
            set { Chart = string.IsNullOrEmpty(value) ? new JObject() : JsonConvert.DeserializeObject<JObject>(value); }
        }

        [NotMapped]
        public JObject Chart { get; set; }

        /// <summary>
        /// Filter data in the form key->value
        /// --  For HasChart columns keys will be unique values of this column and  values will be booleans 
        ///     that determine whether markers with this particluar value will be displayed on the map.
        /// --  For number columns which don't have HasChart set keys will be 'min' and 'max' and
        ///     values will be range limits. Only markers with values for this column inside the given
        ///     range will be displayed.
        /// --  for lat keys will be 'lat, 'lng' and 'radius' and values will determine and values will
        ///     be coordinates of the point and radius. Only markers within the radious from the given 
        ///     point will be displayed.
        /// </summary>
        public string Filter_
        {
            get { return Filter != null ? JsonConvert.SerializeObject(Filter) : null; }
            set { Filter = string.IsNullOrEmpty(value) ? new JObject() : JsonConvert.DeserializeObject<JObject>(value); }
        }

        [NotMapped]
        public JObject Filter { get; set; }

        /// <summary>
        /// If this filed is set the filter for this column will be taken into consideration 
        /// in the process of determining which markers should be displayed on the map.
        /// </summary>
        public bool IsFilterVisible { get; set; }

        /// <summary>
        /// Layer the column belongs to.
        /// </summary>
        public virtual Layer Layer { get; set; }
    }
}