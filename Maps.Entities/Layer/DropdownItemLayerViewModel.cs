using System;

namespace Maps.Entities
{
    /// <summary>
    /// List of layers in the dropdown lists for displaying Data 
    /// table and charts.
    /// </summary>
    public class DropdownItemLayerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DropdownItemLayerViewModel() { }

        public DropdownItemLayerViewModel(Layer layer)
        {
            if (layer != null)
            {
                Id = layer.Id;
                Name = layer.Name;
            }
        }
    }
}
