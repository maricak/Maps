using System;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel fol filtering based on the distance of user data from 
    /// the given location point.
    /// </summary>
    public class DistanceFilterViewModel : IFilterViewModel
    {
        FilterType IFilterViewModel.Type => FilterType.DISTANCE;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsVisible { get; set; }

        [Required(ErrorMessage = "Latitude is required")]
        [Range(-90, 90, ErrorMessage = "Latitude must be in range [-90, 90]")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        [Range(-90, 90, ErrorMessage = "Longitude must be in range [-180, 180]")]
        public double Longitude { get; set; }

        [Display(Name = "Radius in meters")]
        [Required(ErrorMessage = "Radius is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Radius cannot be negative")]
        public double Radius { get; set; }

        public DistanceFilterViewModel() { }

        public DistanceFilterViewModel(Layer layer)
        {
            if (layer != null)
            {
                Id = layer.Id;
                Name = layer.Name;
                IsVisible = layer.IsFilterVisible;
                Latitude = (double)layer.Center["lat"];
                Longitude = (double)layer.Center["lng"];
                Radius = (double)layer.Center["radius"];
            }
        }
    }
}