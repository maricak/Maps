namespace Maps.Entities
{
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
