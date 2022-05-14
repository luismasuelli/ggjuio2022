using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


namespace GGJUIO2020.Server
{
    namespace Types
    {
        namespace RemoteStorage
        {
            public class User
            {
                [JsonProperty("_id")]
                public string Id;

                [JsonProperty("nickname")]
                public string NickName;

                [JsonProperty("login")]
                public string Login;

                // This is quite insecure in the real world!!!!!
                [JsonProperty("password")]
                public string Password;

                [JsonProperty("progress")]
                public int Progress;

                [JsonProperty("quest")]
                public QuestItem[] Quest;
            }
        }
    }
}
