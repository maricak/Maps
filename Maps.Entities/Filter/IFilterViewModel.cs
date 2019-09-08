using System;

namespace Maps.Entities
{
    /// <summary>
    /// Now exsits three types of filters:
    ///     1. UNIQUE_LIST: filter by possible values of the column.
    ///     2. DISTANCE: filter by distance of the row in user data from given point.
    ///     3. RANGE: filter by numeric values and whether they belong to the given range.
    /// </summary>
    public enum FilterType { UNIQUE_LIST, DISTANCE, RANGE }

    /// <summary>
    /// Interface that reporesents basic ViewModel for filtering. All filter
    /// ViewModel should implement this interrface.
    /// </summary>
    public interface IFilterViewModel
    {
        /// <summary>
        /// Type of the filter ViewModel
        /// </summary>
        FilterType Type { get; }

        /// <summary>
        /// Id of the column or layer(for distance filter) the filter depends on.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Name of the filter's column.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Is this filter taken in consideration when the map is 
        /// being rendered.
        /// </summary>
        bool IsVisible { get; set; }
    }
}