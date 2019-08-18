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

        public string Values_ { get; set; }

        [NotMapped]
        public JObject Values
        {
            get { return Values_ == null ? null : JsonConvert.DeserializeObject<JObject>(Values_); }
            set { Values_ = JsonConvert.SerializeObject(value); }
        }
    }
}
