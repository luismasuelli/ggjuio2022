using System;
using System.Collections;
using System.Collections.Generic;
using AlephVault.Unity.Meetgard.Scopes.Authoring.Behaviours.Server;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.Entities.Objects;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.World;
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
                    namespace ScopeAddOns
                    {
                        [RequireComponent(typeof(NetRoseScopeServerSide))]
                        public class RefereeSetup : MonoBehaviour
                        {
                            private NetRoseScopeServerSide netRoseScopeServerSide;

                            private void Awake()
                            {
                                netRoseScopeServerSide = GetComponent<NetRoseScopeServerSide>();
                            }

                            private void Start()
                            {
                                netRoseScopeServerSide.ScopeServerSide.OnLoad += async () =>
                                {
                                    Map map = GetComponentInChildren<Map>();
                                    ushort REFEREE_X = 7;
                                    ushort REFEREE_Y = 3;

                                    try
                                    {
                                        var obj = netRoseScopeServerSide.NetRoseProtocolServerSide.InstantiateHere(
                                            2, async (obj) =>
                                            {
                                                MapObject mapObj = obj.GetComponent<MapObject>();
                                                mapObj.Initialize();
                                                mapObj.Attach(map, REFEREE_X, REFEREE_Y, true);
                                            }
                                        );
                                    }
                                    catch (KeyNotFoundException e)
                                    {
                                        Debug.Log("Wrong prefab index! Invalid for referee");
                                        throw;
                                    }
                                };
                            }
                        }
                    }
                }
            }
        }
    }
}
