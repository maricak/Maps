using Newtonsoft.Json.Linq;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for displaying one data entity.
    /// </summary>
    public class DetailsDataViewModel
    {
        public JObject Values { get; set; }

        public DetailsDataViewModel()
        {
            Values = new JObject();
        }

        public DetailsDataViewModel(Data data)
        {
            Values = data.Values;
        }
    }
}