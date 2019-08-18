using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maps.Entities
{
    [Table("Map")]
    public class Map
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Creation time")]
        [DataType(DataType.DateTime)]
        public DateTime CreationTime { get; set; }

        public ICollection<Layer> Layers { get; set; }

        public virtual User User { get; set; }
    }
}
