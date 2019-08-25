﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    public class DetailsMapViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<DetailsLayerViewModel> Layers { get; set; }

        public List<DropdownItemLayerViewModel> DropdownLayerItems { get; set; }

        public DetailsMapViewModel()
        {
            Layers = new List<DetailsLayerViewModel>();
            DropdownLayerItems = new List<DropdownItemLayerViewModel>();
        }

        public DetailsMapViewModel(Map map)
        {
            Id = map.Id;
            Name = map.Name;
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
