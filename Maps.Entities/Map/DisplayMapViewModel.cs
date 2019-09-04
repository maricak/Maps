using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for displaying a map with Google Maps API.
    /// </summary>
    public class DisplayMapViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Information about the map that is necessary in 
        /// JavaScript so it can be displayed on the map.
        /// </summary>
        public JArray Layers { get; set; }

        public DisplayMapViewModel() { }

        public DisplayMapViewModel(Map map)
        {
            if (map != null)
            {
                Name = map.Name;
                Id = map.Id;
                Layers = new JArray();
                foreach (var layer in map.Layers)
                {
                    if (layer != null && layer.HasData && layer.IsVisible)
                    {
                        var jsonLayer = new JObject
                        {
                            ["name"] = layer.Name ?? "",
                            ["lat"] = layer.Columns.Where(c => c.DataType == UserDataType.LATITUDE).SingleOrDefault().Name ?? "",
                            ["lng"] = layer.Columns.Where(c => c.DataType == UserDataType.LONGITUDE).SingleOrDefault().Name ?? "",
                            ["icon"] = layer.Icon ?? "",
                        };
                        var jsonData = new JArray();
                        if (layer.Data != null)
                        {
                            foreach (var data in layer.Data)
                            {
                                jsonData.Add(data.Values);
                            }
                        }
                        jsonLayer["data"] = jsonData;
                        Layers.Add(jsonLayer);
                    }
                }
            }
        }
    }
}
