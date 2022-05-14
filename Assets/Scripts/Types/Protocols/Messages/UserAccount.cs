using AlephVault.Unity.Binary;
using AlephVault.Unity.Binary.Wrappers;
using AlephVault.Unity.Meetgard.Auth.Types;
using GGJUIO2020.Types.Models;


namespace GGJUIO2020.Types
{
    namespace Protocols
    {
        namespace Messages
        {
            /// <summary>
            ///   A serializer for the full user account.
            /// </summary>
            public class UserAccount : ISerializable, IRecordWithPreview<string, String>
            {
                public string Login;
                public string NickName;
                // This will NOT be synchronized to the client.
                public User Model;

                // Bare constructor.
                public UserAccount() {}

                // Pre-populated constructor.
                public UserAccount(User user)
                {
                    Login = user.Login;
                    NickName = user.NickName;
                    Model = user;
                }

                public void Serialize(Serializer serializer)
                {
                    serializer.Serialize(ref Login);
                    serializer.Serialize(ref NickName);
                }

                public string GetID()
                {
                    return new String(Login);
                }

                public String GetProfileDisplayData()
                {
                    return (String)NickName;
                }
            }
        }
    }
}