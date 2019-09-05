using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for creating single Column entity.
    /// </summary>
    public class CreateColumnViewModel
    {
        [Required(ErrorMessage = "Column name is required.")]
        [StringLength(50, ErrorMessage = "Name must be less than {1} characters.")]
        public string Name { get; set; }

        [Display(Name = "Data type")]
        public UserDataType DataType { get; set; }

        /// <summary>
        /// Represents whether this column will have charts and map filter.
        /// </summary>
        [Display(Name = "Filter?")]
        public bool HasChart { get; set; }

        public CreateColumnViewModel() { }
        public CreateColumnViewModel(Column column)
        {
            Name = column.Name;
            DataType = column.DataType;
        }
    }
}