using Newtonsoft.Json;


namespace GGJUIO2020.Types
{
    namespace Models
    {
        public class QuestItem
        {
            [JsonProperty("cityIndex")]
            public int CityIndex;

            [JsonProperty("questionType")]
            public string questionType;
        }
    }
}