using System;
using System.Collections.Generic;

namespace Maps.Entities
{
    public class SelectIconLayerViewModel
    {
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
        public IList<string> Icons { get; set; }

        public SelectIconLayerViewModel()
        {
            Icons = ICON_NAMES;
        }

        public SelectIconLayerViewModel(Layer layer)
        {
            Icons = ICON_NAMES;

            Id = layer.Id;
            Icon = layer.Icon;                
        }
    }
}
