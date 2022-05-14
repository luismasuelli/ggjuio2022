using Newtonsoft.Json;


namespace GGJUIO2020.Server
{
    namespace Types
    {
        namespace RemoteStorage
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
}