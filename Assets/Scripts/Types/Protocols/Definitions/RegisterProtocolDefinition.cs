using AlephVault.Unity.Meetgard.Protocols;
using GGJUIO2020.Types.Protocols.Messages;


namespace GGJUIO2020.Types
{
    namespace Protocols
    {
        namespace Definitions
        {
            /// <summary>
            ///   A simple register protocol.
            /// </summary>
            public class RegisterProtocolDefinition : MandatoryHandshakeProtocolDefinition
            {
                protected override void DefineMessages()
                {
                    base.DefineMessages();
                    DefineClientMessage<UserBody>("Register");
                    DefineServerMessage("Ok");
                    DefineServerMessage("Duplicate");
                    DefineServerMessage("PasswordMismatch");
                    DefineServerMessage("Invalid");
                    DefineServerMessage("UnexpectedError");
                }
            }
        }
    }
}