using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maps.Entities
{
    [Table("Data")]
    public class Data
    {
        [Key]
        public Guid Id { get; set; }

        public virtual Layer Layer { get; set; }

        public string Values_
        {
            get { return Values != null ? JsonConvert.SerializeObject(Values) : null; }
            set { Values = JsonConvert.DeserializeObject<JObject>(value); }
        }

        [NotMapped]
        public JObject Values { get; set; }
    }
}
