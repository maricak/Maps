using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maps.Entities
{
    /// <summary>
    /// Entity that represents one row in Map table.
    /// </summary>
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

        public bool IsPublic { get; set; }

        /// <summary>
        /// Map has a collection of its layers.
        /// </summary>
        public ICollection<Layer> Layers { get; set; }

        /// <summary>
        /// Map has a reference to the user it belongs to.
        /// </summary>
        public virtual User User { get; set; }
    }
}
