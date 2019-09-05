using System;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for createing new layer for the given map.
    /// </summary>
    public class CreateLayerViewModel
    {
        [Required(ErrorMessage = "Map ID is required.")]
        public Guid MapId { get; set; }

        [Required(ErrorMessage = "Layer name is required.")]
        [StringLength(50, ErrorMessage = "Name must be less than {1} characters.")]
        public string Name { get; set; }

        public CreateLayerViewModel() { }

        public CreateLayerViewModel(Guid mapId)
        {
            MapId = mapId;
        }
    }
}