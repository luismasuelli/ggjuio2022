using AlephVault.Unity.Binary.Wrappers;
using AlephVault.Unity.Meetgard.Protocols;
using GGJUIO2020.Types.Protocols.Messages;


namespace GGJUIO2020.Types
{
    namespace Protocols
    {
        namespace Definitions
        {
            /// <summary>
            ///   The main protocol definition.
            /// </summary>
            public class MainProtocolDefinition : ProtocolDefinition
            {
                protected override void DefineMessages()
                {
                    DefineClientMessage("MoveLeft");
                    DefineClientMessage("MoveRight");
                    DefineClientMessage("MoveUp");
                    DefineClientMessage("MoveDown");
                    DefineClientMessage("Talk");
                    // Referee.
                    DefineServerMessage<CurrentMission>("CurrentMission"); // Referee response.
                    DefineServerMessage("AlreadyComplete"); // Referee response.
                    // Informant.
                    DefineServerMessage<Int>("Info"); // Informant response.
                    DefineServerMessage<Int>("StepComplete"); // Informant response.
                    // Notifications.
                    DefineServerMessage("YouJustCompleted"); // Server notification.
                    DefineServerMessage<String>("SomeoneJustCompleted"); // Server notification.
                }
            }
        }
    }
}