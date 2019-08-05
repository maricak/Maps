using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Maps.Models
{
    [Table("Map")]
    public class Map
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime CreationTime { get; set; }

        public ICollection<Layer> Layers { get; set; }

        public virtual ApplicationUser User { get; set; }
    }

    public class CreateMapViewModel
    {
        [Required(ErrorMessage = "The map name is required.")]
        [StringLength(50, ErrorMessage = "Name must be less than {1} characters.")]
        public string Name { get; set; }
    }
}
