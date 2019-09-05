using System;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for editing map's name.
    /// </summary>
    public class EditMapViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The map name is required.")]
        [StringLength(50, ErrorMessage = "Name must be less than {1} characters.")]
        public string Name { get; set; }

        public DateTime CreationTime { get; set; }

        public EditMapViewModel() { }

        public EditMapViewModel(Map map)
        {
            if (map != null)
            {
                Id = map.Id;
                Name = map.Name;
                CreationTime = map.CreationTime;
            }
        }
    }
}
