using System.Collections;
using System.Collections.Generic;
using AlephVault.Unity.Meetgard.Types;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.Entities.Objects;
using GameMeanMachine.Unity.WindRose.Types;
using UnityEngine;


namespace GGJUIO2020.Server
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace Protocols
            {
                namespace Models
                {
                    /// <summary>
                    ///   The end side of a <see cref="LocalTeleporter"/>. See that
                    ///     class for more informaation.
                    /// </summary>
                    [RequireComponent(typeof(TriggerPlatform))]
                    public class GlobalTeleportTargetModelServerSide : NetRoseModelServerSide<Nothing, Nothing>
                    {
                        private static Dictionary<string, GlobalTeleportTargetModelServerSide> targets =
                            new Dictionary<string, GlobalTeleportTargetModelServerSide>();
                        
                        [SerializeField]
                        private string teleportKey;
                        
                        /// <summary>
                        ///   Tells whether the just teleported object will look
                        ///     (by setting its <see cref="MapObject.orientation"/>
                        ///     property to a new value) to the orientation specified
                        ///     in <see cref="NewOrientation"/>.
                        /// </summary>
                        public bool ForceOrientation = true;

                        /// <summary>
                        ///   This property is meaningful only if <see cref="ForceOrientation"/>
                        ///     is <c>true</c>.
                        /// </summary>
                        public Direction NewOrientation = Direction.DOWN;

                        private void Awake()
                        {
                            if (targets.ContainsKey(teleportKey))
                            {
                                Destroy(gameObject);
                                return;
                            }

                            targets[teleportKey] = this;
                            base.Awake();
                        }

                        private void OnDestroy()
                        {
                            base.OnDestroy();
                            targets.Remove(teleportKey);
                        }

                        protected override Nothing GetInnerFullData(ulong connectionId)
                        {
                            throw new System.NotImplementedException();
                        }

                        protected override Nothing GetInnerRefreshData(ulong connectionId, string context)
                        {
                            throw new System.NotImplementedException();
                        }

                        public static bool TryGetTarget(string key, out GlobalTeleportTargetModelServerSide target)
                        {
                            return targets.TryGetValue(key, out target);
                        }
                    }
                }
            }
        }
    }
}
