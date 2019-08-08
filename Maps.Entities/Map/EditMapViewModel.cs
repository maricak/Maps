using System;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    public class EditMapViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The map name is required.")]
        [StringLength(50, ErrorMessage = "Name must be less than {1} characters.")]
        public string Name { get; set; }

        public EditMapViewModel() { }
        public EditMapViewModel(Map map)
        {
            Id = map.Id;
            Name = map.Name;
        }

        public void UpdateMap(ref Map map)
        {
            map.Name = Name;
        }
    }
}
