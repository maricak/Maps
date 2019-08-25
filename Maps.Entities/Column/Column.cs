using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maps.Entities
{
    public enum UserDataType { STRING, NUMBER, LONGITUDE, LATITUDE }

    [Table("Column")]
    public class Column
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public UserDataType DataType { get; set; }

        public virtual Layer Layer { get; set; }
    }
}
