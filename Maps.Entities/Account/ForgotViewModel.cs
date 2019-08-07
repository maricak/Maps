using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
