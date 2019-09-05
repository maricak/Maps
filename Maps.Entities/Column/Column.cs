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
        /// This column will have map filters and data charts if this field is set.
        /// </summary>
        public bool HasChart { get; set; }

        /// <summary>
        /// Layer the column belongs to.
        /// </summary>
        public virtual Layer Layer { get; set; }
    }
}
