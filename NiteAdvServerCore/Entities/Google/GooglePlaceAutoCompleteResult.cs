using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Entities
{
    public class GooglePlaceAutoCompletePrediction
    {
      
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

    }

    public class StructuredFormatting
    {
        [JsonProperty("main_text")]
        public string MainText { get; set; }

        [JsonProperty("secondary_text")]
        public string SecondaryText { get; set; }
    }

    public class GooglePlaceAutoCompleteResult
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("candidates")]
        public List<GooglePlaceAutoCompletePrediction> Candidates { get; set; }
    }
}
