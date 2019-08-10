using System;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    public class DetailsMapViewModel
    {
        public Guid Id;
        public string Name { get; set; }

        [Display(Name = "Creation time")]
        public DateTime CreationTime { get; set; }

        public DetailsMapViewModel() { }

        public DetailsMapViewModel(Map map)
        {
            Id = map.Id;
            Name = map.Name;
            CreationTime = map.CreationTime;
        }
    }
}
