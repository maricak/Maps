using System;

namespace Maps.Entities
{
    public class DropdownItemLayerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DropdownItemLayerViewModel() { }

        public DropdownItemLayerViewModel(Layer layer)
        {
            if(layer != null)
            {
                Id = layer.Id;
                Name = layer.Name;
            }
        }
    }
}
