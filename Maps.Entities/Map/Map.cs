using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maps.Entities
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

        public virtual User User { get; set; }
    }
}
