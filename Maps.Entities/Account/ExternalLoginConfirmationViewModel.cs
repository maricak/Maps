using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
