using System;
using System.Collections.Generic;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for read-only view of the maps that are 
    /// public.
    /// </summary>
    public class ViewMapViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ViewMapViewModel() { }

        public ViewMapViewModel(Map map)
        {
            if (map != null)
            {
                Id = map.Id;
                Name = map.Name;
            }
        }
    }
}