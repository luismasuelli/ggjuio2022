using AlephVault.Unity.Binary;


namespace GGJUIO2020.Types
{
    namespace Protocols
    {
        namespace Messages
        {
            /// <summary>
            ///   A serializer for the login body.
            /// </summary>
            public class LoginBody : ISerializable
            {
                public string Login;
                public string Password;

                public void Serialize(Serializer serializer)
                {
                    serializer.Serialize(ref Login);
                    serializer.Serialize(ref Password);
                }
            }
        }
    }
}