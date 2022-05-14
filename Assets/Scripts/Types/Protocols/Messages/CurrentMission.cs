using AlephVault.Unity.Binary;


namespace GGJUIO2020.Types
{
    namespace Protocols
    {
        namespace Messages
        {
            /// <summary>
            ///   A serializer for the current mission.
            /// </summary>
            public class CurrentMission : ISerializable
            {
                public int CityIndex;
                public string QuestionType;
        
                public void Serialize(Serializer serializer)
                {
                    serializer.Serialize(ref CityIndex);
                    serializer.Serialize(ref QuestionType);
                }
            }
        }
    }
}