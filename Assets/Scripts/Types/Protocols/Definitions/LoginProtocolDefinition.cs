using AlephVault.Unity.Meetgard.Auth.Protocols.Simple;
// using AlephVault.Unity.Meetgard.Auth.Samples;
using AlephVault.Unity.Meetgard.Types;
using GGJUIO2020.Types.Protocols.Messages;


namespace GGJUIO2020.Types
{
    namespace Protocols
    {
        namespace Definitions
        {
            /// <summary>
            ///   A login protocol with unsecured User/Pass.
            /// </summary>
            public class SampleLoginProtocolDefinition : SimpleAuthProtocolDefinition<Nothing, LoginFailed, Kicked>
            {
                protected override void DefineLoginMessages()
                {
                    DefineLoginMessage<LoginBody>("Login:Community");
                }
            }
        }
    }
}