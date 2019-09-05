using Newtonsoft.Json.Linq;

namespace Maps.Entities
{
    /// <summary>
    /// ViewModel for displaying one data entity.
    /// </summary>
    public class DetailsDataViewModel
    {
        public JObject Values { get; set; }

        public string Values_
        {
            get; set;
        }

        public DetailsDataViewModel(Data data)
        {
            Values = data.Values;
        }
    }
}
