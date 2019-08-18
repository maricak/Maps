using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maps.Entities
{
    public enum UserDataType { ENUM, STRING, NUMBER, DATE, BOOL }

    [Table("Column")]
    public class Column
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public UserDataType DataType { get; set; }

        public string EnumValues_ { get; set; }

        [NotMapped]
        public string[] Tags
        {
            get { return EnumValues_ == null ? null : JsonConvert.DeserializeObject<string[]>(EnumValues_); }
            set { EnumValues_ = JsonConvert.SerializeObject(value); }
        }
        public virtual Layer Layer { get; set; }
    }
}
