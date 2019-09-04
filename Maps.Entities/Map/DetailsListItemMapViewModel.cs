using System;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel that represents one item in the list of user's maps.
    /// </summary>
    public class ListItemMapViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Creation time")]
        public DateTime CreationTime { get; set; }

        public ListItemMapViewModel() { }
        public ListItemMapViewModel(Map map)
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
