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
                    ///   The end side of a <see cref="GlobalTeleporter"/>. See that
                    ///     class for more informaation.
                    /// </summary>
                    [RequireComponent(typeof(TriggerPlatform))]
                    public class GlobalTeleportTarget : MonoBehaviour
                    {
                        private static Dictionary<string, GlobalTeleportTarget> targets =
                            new Dictionary<string, GlobalTeleportTarget>();
                        
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
                        }

                        private void OnDestroy()
                        {
                            targets.Remove(teleportKey);
                        }
                        
                        public static bool TryGetTarget(string key, out GlobalTeleportTarget target)
                        {
                            return targets.TryGetValue(key, out target);
                        }
                    }
                }
            }
        }
    }
}
