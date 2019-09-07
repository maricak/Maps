using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maps.Entities
{
    /// <summary>
    /// Entity that represents one row in the Data table.
    /// </summary>
    [Table("Data")]
    public class Data
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Pairs name->value are kept in the databse in the JSON format.
        /// </summary>
        public string Values_
        {
            get { return Values != null ? JsonConvert.SerializeObject(Values) : null; }
            set { Values = string.IsNullOrEmpty(value) ? new JObject() : JsonConvert.DeserializeObject<JObject>(value); }
        }

        [NotMapped]
        public JObject Values { get; set; }

        /// <summary>
        /// Layer this piece of data belongs to.
        /// </summary>
        public virtual Layer Layer { get; set; }
    }
}