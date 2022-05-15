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
                        public class InformantSetup : MonoBehaviour
                        {
                            private NetRoseScopeServerSide netRoseScopeServerSide;

                            [SerializeField]
                            private int cityIndex;
                            
                            private void Awake()
                            {
                                netRoseScopeServerSide = GetComponent<NetRoseScopeServerSide>();
                                netRoseScopeServerSide.ScopeServerSide.OnLoad += async () =>
                                {
                                    Map map = GetComponentInChildren<Map>();
                                    ushort INFORMANT_X = 3;
                                    ushort INFORMANT_Y = 3;

                                    try
                                    {
                                        var obj = netRoseScopeServerSide.NetRoseProtocolServerSide.InstantiateHere(
                                            "informant", async (obj) =>
                                            {
                                                MapObject mapObj = obj.GetComponent<MapObject>();
                                                mapObj.Attach(map, INFORMANT_X, INFORMANT_Y, true);
                                                // TODO: Look for the informant behaviour and link the provinceIndex
                                            }
                                        );
                                    }
                                    catch (KeyNotFoundException e)
                                    {
                                        Console.WriteLine("Wrong prefab index! Invalid for informant");
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
