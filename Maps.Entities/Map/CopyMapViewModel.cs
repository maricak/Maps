using System;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for copying of the given public map to 
    /// users list of maps.
    /// </summary>
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
