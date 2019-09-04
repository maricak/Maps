using System;

namespace Maps.Entities
{
    public class CopyMapViewModel
    {
        public Guid Id { get; set; }

        public CopyMapViewModel() { }
        public CopyMapViewModel(Guid id)
        {
            Id = id;
        }
    }
}
