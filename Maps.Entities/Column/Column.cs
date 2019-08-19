using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maps.Entities
{
    public enum UserDataType { ENUM, STRING, NUMBER, BOOL, LONGITUDE, LATITUDE }

    [Table("Column")]
    public class Column
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public UserDataType DataType { get; set; }

        public string EnumValues_
        {
            get { return EnumValues != null ? JsonConvert.SerializeObject(EnumValues) : null; }
            set { EnumValues = JsonConvert.DeserializeObject<List<string>>(value); }
        }

        [NotMapped]
        public List<string> EnumValues { get; set; }
        public virtual Layer Layer { get; set; }
    }
}
