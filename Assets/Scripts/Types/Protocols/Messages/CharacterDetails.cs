using AlephVault.Unity.Binary;


namespace GGJUIO2020.Types
{
    namespace Protocols
    {
        namespace Messages
        {
            /// <summary>
            ///   A serializer for the character details.
            /// </summary>
            public class CharacterDetails : ISerializable
            {
                public string NickName;
                public bool Owned;

                public void Serialize(Serializer serializer)
                {
                    serializer.Serialize(ref NickName);
                    serializer.Serialize(ref Owned);
                }
            }
        }
    }
}