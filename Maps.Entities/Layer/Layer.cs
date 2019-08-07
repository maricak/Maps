using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Maps.Entities
{
    [Table("Layer")]

    public class Layer
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual Map Map { get; set; }
    }
}
