using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Device.Location;
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

        public DisplayMapViewModel()
        {
            Layers = new JArray();
        }

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
                            ["center"] = false
                        };

                        if (layer.IsFilterVisible)
                        {
                            jsonLayer["center"] = true;
                            jsonLayer["center_lat"] = (double)layer.Center["lat"];
                            jsonLayer["center_lng"] = (double)layer.Center["lng"];
                            jsonLayer["radius"] = (double)layer.Center["radius"];
                        }

                        var jsonData = new JArray();
                        if (layer.Data != null)
                        {
                            foreach (var data in layer.Data)
                            {
                                if (!CheckDistance(data, (string)jsonLayer["lat"], (string)jsonLayer["lng"], layer))
                                {
                                    continue;
                                }

                                if (!CheckUniquList(data, layer.Columns.ToList()))
                                {
                                    continue;
                                }

                                if (!CheckRange(data, layer.Columns.ToList()))
                                {
                                    continue;
                                }

                                jsonData.Add(data.Values);
                            }
                        }

                        jsonLayer["data"] = jsonData;
                        Layers.Add(jsonLayer);
                    }
                }
            }
        }

        private bool CheckDistance(Data data, string latName, string lngName, Layer layer)
        {
            if (!layer.IsFilterVisible)
            {
                return true;
            }

            var first = new GeoCoordinate((double)layer.Center["lat"], (double)layer.Center["lng"]);
            var second = new GeoCoordinate((double)data.Values[latName], (double)data.Values[lngName]);

            return first.GetDistanceTo(second) < (double)layer.Center["radius"];
        }

        private bool CheckUniquList(Data data, IList<Column> columns)
        {
            foreach (var column in columns)
            {
                if (column.HasChart)
                {
                    var value = (string)data.Values[column.Name];
                    if (column.IsFilterVisible && !(bool)column.Filter[value])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CheckRange(Data data, IList<Column> columns)
        {
            foreach (var column in columns)
            {
                if (!column.HasChart && column.DataType != UserDataType.STRING)
                {
                    var value = (double)data.Values[column.Name];
                    var min = (double)column.Filter["min"];
                    var max = (double)column.Filter["max"];
                    if (column.IsFilterVisible && (value < min || value > max))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}