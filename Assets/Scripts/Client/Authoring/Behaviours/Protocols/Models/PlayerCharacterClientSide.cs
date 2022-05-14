using AlephVault.Unity.Binary.Wrappers;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Client;


namespace GGJUIO2020.Client
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace Protocols
            {
                namespace Models
                {
                    public class PlayerCharacterClientSide : NetRoseModelClientSide<String, String>
                    {
                        protected override void InflateFrom(String fullData)
                        {
                            // TODO implement later when designing the character prefab.
                            throw new System.NotImplementedException();
                        }

                        protected override void UpdateFrom(String refreshData)
                        {
                            // TODO implement later when designing the character prefab.
                            throw new System.NotImplementedException();
                        }
                    }
                }
            }
        }
    }
}