using System;
using System.Collections.Generic;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for changing icon of the given layer.
    /// </summary>
    public class SelectIconLayerViewModel
    {
        /// <summary>
        /// List of all awailable icons. Item in the list must match 
        /// name of the icon file.
        /// </summary>
        public static readonly List<string> ICON_NAMES = new List<string>()
            {
                "red", "red-dot",
                "green", "green-dot",
                "blue", "blue-dot",
                "orange", "orange-dot",
                "yellow", "yellow-dot",
                "purple","purple-dot",
                "lightblue", "lightblue-dot",
                "pink", "pink-dot"
            };
        public Guid Id { get; set; }

        public string Icon { get; set; }

        /// <summary>
        /// List of all posible icons used to dislay selection.
        /// </summary>
        public IList<string> Icons { get; set; }

        public SelectIconLayerViewModel()
        {
            Icons = ICON_NAMES;
        }

        public SelectIconLayerViewModel(Layer layer)
        {
            Icons = ICON_NAMES;
            if (layer != null)
            {
                Id = layer.Id;
                Icon = layer.Icon;
            }
        }
    }
}
