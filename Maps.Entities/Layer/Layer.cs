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

        [StringLength(150)]
        public string Icon { get; set; }

        public virtual Map Map { get; set; }

        public bool HasData { get; set; }

        public bool HasColumns { get; set; }

        public bool IsVisible { get; set; }

        public ICollection<Data> Data { get; set; }

        public ICollection<Column> Columns { get; set; }

        public Layer()
        {
            Icon = "red-dot";
        }
    }
}
