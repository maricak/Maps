using System;
using System.Collections.Generic;
using System.Configuration;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for changing icon of the given layer.
    /// </summary>
    public class SelectIconLayerViewModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// List of all awailable icons. Item in the list must match 
        /// name of the icon file.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// List of all posible icons used to dislay selection.
        /// </summary>
        public IList<string> Icons { get; set; }

        public SelectIconLayerViewModel()
        {
            Icons = new List<string>();
        }

        public SelectIconLayerViewModel(IList<string> icons)
        {
            Icons = icons;
        }

        public SelectIconLayerViewModel(Layer layer, IList<string> icons)
        {
            Icons = icons;
            if (layer != null)
            {
                Id = layer.Id;
                Icon = layer.Icon;
            }
        }
    }
}