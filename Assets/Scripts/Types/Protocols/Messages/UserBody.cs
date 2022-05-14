using AlephVault.Unity.Binary;


namespace GGJUIO2020.Types
{
    namespace Protocols
    {
        namespace Messages
        {
            /// <summary>
            ///   A serializer for the user body.
            /// </summary>
            public class UserBody : ISerializable
            {
                public string Login;
                public string NickName;
                public string Password;
                public string PasswordConfirm;

                public void Serialize(Serializer serializer)
                {
                    serializer.Serialize(ref Login);
                    serializer.Serialize(ref NickName);
                    serializer.Serialize(ref Password);
                    serializer.Serialize(ref PasswordConfirm);
                }

                public bool PasswordsMatch()
                {
                    return Password == PasswordConfirm;
                }
            }
        }
    }
}