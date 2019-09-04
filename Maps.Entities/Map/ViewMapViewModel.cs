using System;
using System.Collections.Generic;

namespace Maps.Entities
{
    public class ViewMapViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<DetailsLayerViewModel> Layers { get; set; }

        public ViewMapViewModel() { }

        public ViewMapViewModel(Map map)
        {
            Id = map.Id;
            Name = map.Name;
        }
    }
}
