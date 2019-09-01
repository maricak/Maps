using Newtonsoft.Json.Linq;

namespace Maps.Entities
{
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
