using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maps.Entities
{
    [Table("Layer")]

    public class Layer
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public bool HasData { get; set; }

        public virtual Map Map { get; set; }

        public ICollection<Data> Data { get; set; }
        public ICollection<Column> Columns { get; set; }

    }
}
