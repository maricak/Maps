using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    public class DetailsMapViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Creation time")]
        public DateTime CreationTime { get; set; }

        public List<DetailsLayerViewModel> Layers { get; set; }

        public DetailsMapViewModel()
        {
            Layers = new List<DetailsLayerViewModel>();
        }

        public DetailsMapViewModel(Map map)
        {
            Id = map.Id;
            Name = map.Name;
            CreationTime = map.CreationTime;
            Layers = new List<DetailsLayerViewModel>();

            if (map.Layers != null)
            {
                foreach (var layer in map.Layers)
                {
                    Layers.Add(new DetailsLayerViewModel(layer));
                }
            }
        }
    }
}
