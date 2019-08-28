using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    public class CreateMapViewModel
    {
        [Required(ErrorMessage = "Map name is required.")]
        [StringLength(50, ErrorMessage = "Name must be less than {1} characters.")]
        public string Name { get; set; }
    }
}
