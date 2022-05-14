using AlephVault.Unity.Meetgard.Types;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Client;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.Entities.Objects;
using UnityEngine;


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
                    [RequireComponent(typeof(TriggerPlatform))]
                    public class GlobalTeleporterModelClientSide : NetRoseModelClientSide<Nothing, Nothing>
                    {
                        protected override void InflateFrom(Nothing fullData)
                        {
                        }

                        protected override void UpdateFrom(Nothing refreshData)
                        {
                        }
                    }
                }
            }
        }
    }
}