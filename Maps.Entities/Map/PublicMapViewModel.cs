using System;

namespace Maps.Entities
{
    public class PublicMapViewModel
    {
        public Guid Id { get; set; }

        public bool IsPublic { get; set; }

        public PublicMapViewModel() { }
        public PublicMapViewModel(Map map)
        {
            Id = map.Id;
            IsPublic = map.IsPublic;
        }
    }
}
