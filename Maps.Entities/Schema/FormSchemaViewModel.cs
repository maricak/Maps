using System;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    public class FormSchemaViewModel
    {
        [Required(ErrorMessage = "Number of columns is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of columns must be positive")]
        [Display(Name = "Number of columns")]
        public int NumColumns { get; set; }

        public Guid LayerId { get; set; }

        public FormSchemaViewModel() { }
        public FormSchemaViewModel(Guid layerId)
        {
            LayerId = layerId;
        }
    }
}
