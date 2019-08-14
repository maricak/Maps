﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Maps.Entities
{
    public class EditLayerViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The map name is required.")]
        [StringLength(50, ErrorMessage = "Name must be less than {1} characters.")]
        public string Name { get; set; }


        public EditLayerViewModel() { }
        public EditLayerViewModel(Layer layer)
        {
            Id = layer.Id;
            Name = layer.Name;
        }
    }
}
