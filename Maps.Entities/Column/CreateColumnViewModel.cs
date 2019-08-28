using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    public class CreateColumnViewModel
    {
        [Required(ErrorMessage = "Column name is required.")]
        [StringLength(50, ErrorMessage = "Name must be less than {1} characters.")]
        public string Name { get; set; }

        [Display(Name = "Data type")]
        public UserDataType DataType { get; set; }

        [Display(Name = "Chart?")]
        public bool Chart { get; set; }

        public CreateColumnViewModel() { }
        public CreateColumnViewModel(Column column)
        {
            Name = column.Name;
            DataType = column.DataType;
        }
    }
}
