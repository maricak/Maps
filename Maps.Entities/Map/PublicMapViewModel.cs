using System;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for changing the IsPublic property 
    /// of the given map.
    /// </summary>
    public class PublicMapViewModel
    {
        public Guid Id { get; set; }

        public bool IsPublic { get; set; }

        public PublicMapViewModel() { }

        public PublicMapViewModel(Map map)
        {
            if (map != null)
            {
                Id = map.Id;
                IsPublic = map.IsPublic;
            }
        }
    }
}