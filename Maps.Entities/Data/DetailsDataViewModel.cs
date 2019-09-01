using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

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
