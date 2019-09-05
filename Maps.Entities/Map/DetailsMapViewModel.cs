using System;
using System.Collections.Generic;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for displaying all details of the map, icluding information about its layers.
    /// </summary>
    public class DetailsMapViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Details about map's layers.
        /// </summary>
        public List<DetailsLayerViewModel> Layers { get; set; }

        /// <summary>
        /// List of layers for the dropdown menu.
        /// </summary>
        public List<DropdownItemLayerViewModel> DropdownLayerItems { get; set; }

        public PublicMapViewModel SetPublic { get; set; }

        public DetailsMapViewModel()
        {
            Layers = new List<DetailsLayerViewModel>();
            DropdownLayerItems = new List<DropdownItemLayerViewModel>();
        }

        public DetailsMapViewModel(Map map)
        {
            if (map != null)
            {
                Id = map.Id;
                Name = map.Name;
                SetPublic = new PublicMapViewModel(map);
                Layers = new List<DetailsLayerViewModel>();
                DropdownLayerItems = new List<DropdownItemLayerViewModel>();

                if (map.Layers != null)
                {
                    foreach (var layer in map.Layers)
                    {
                        Layers.Add(new DetailsLayerViewModel(layer));
                        DropdownLayerItems.Add(new DropdownItemLayerViewModel(layer));
                    }
                }
            }
        }
    }
}