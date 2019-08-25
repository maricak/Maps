using System;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    public class DetailsListItemMapViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Creation time")]
        public DateTime CreationTime { get; set; }

        public DetailsListItemMapViewModel() { }
        public DetailsListItemMapViewModel(Map map)
        {
            Id = map.Id;
            Name = map.Name;
            CreationTime = map.CreationTime;
        }
    }
}
