namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for displaying all data regarding the filtering 
    /// inside a given layer: icon change, visibility change...
    /// </summary>
    public class FilterLayerViewModel
    {
        public SelectIconLayerViewModel SelectIcon { get; set; }

        public VisibilityLayerViewModel Visibility { get; set; }

        public FilterLayerViewModel() { }

        public FilterLayerViewModel(Layer layer)
        {
            SelectIcon = new SelectIconLayerViewModel(layer);
            Visibility = new VisibilityLayerViewModel(layer);
        }
    }
}
