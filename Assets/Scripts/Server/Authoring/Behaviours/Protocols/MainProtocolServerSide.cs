using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AlephVault.Unity.Binary.Wrappers;
using AlephVault.Unity.Meetgard.Authoring.Behaviours.Server;
using AlephVault.Unity.Meetgard.Types;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.Entities.Objects;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.Entities.Objects.CommandExchange.Talk;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.World;
using GameMeanMachine.Unity.WindRose.Types;
using GGJUIO2020.Server.Authoring.Behaviours.Protocols.Models;
using GGJUIO2020.Types.Protocols.Definitions;
using GGJUIO2020.Types.Protocols.Messages;
using UnityEngine;
using String = AlephVault.Unity.Binary.Wrappers.String;


namespace GGJUIO2020.Server
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace Protocols
            {
                [RequireComponent(typeof(PingProtocolServerSide))]
                [RequireComponent(typeof(NetRoseProtocolServerSide))]
                [RequireComponent(typeof(LoginProtocolServerSide))]
                public class MainProtocolServerSide : ProtocolServerSide<MainProtocolDefinition>
                {
                    private LoginProtocolServerSide loginProtocolServerSide;
                    private NetRoseProtocolServerSide netRoseProtocolServerSide;
                    private Dictionary<ulong, PlayerCharacterServerSide> playerCharacters;

                    private Dictionary<ulong, PlayerCharacterServerSide> objects = new Dictionary<ulong, PlayerCharacterServerSide>();
                    private Dictionary<ulong, float> times = new Dictionary<ulong, float>();

                    private Func<ulong, CurrentMission, Task> CurrentMissionSender;
                    private Func<ulong, Task> AlreadyCompleteSender;
                    private Func<ulong, Int, Task> InfoSender;
                    private Func<ulong, Int, Task> StepCompleteSender;
                    private Func<ulong, Task> YouJustCompletedSender;
                    private Func<IEnumerable<ulong>, String, Dictionary<ulong, Task>> TheyJustCompletedBroadcaster;

                    protected override void Setup()
                    {
                        base.Setup();
                        loginProtocolServerSide = GetComponent<LoginProtocolServerSide>();
                        netRoseProtocolServerSide = GetComponent<NetRoseProtocolServerSide>();
                    }

                    protected override void Initialize()
                    {
                        ushort PLAYER_X = 13;
                        ushort PLAYER_Y = 3;

                        CurrentMissionSender = MakeSender<CurrentMission>("CurrentMission");
                        AlreadyCompleteSender = MakeSender("AlreadyComplete"); // Referee response.
                        // Informant.
                        InfoSender = MakeSender<Int>("Info"); // Informant response.
                        StepCompleteSender = MakeSender<Int>("StepComplete"); // Informant response.
                        // Notifications.
                        YouJustCompletedSender = MakeSender("YouJustCompleted"); // Server notification.
                        TheyJustCompletedBroadcaster = MakeBroadcaster<String>("SomeoneJustCompleted"); // Server notification.

                        base.Initialize();
                        loginProtocolServerSide.OnSessionStarting += async (clientId, account) =>
                        {
                            _ = RunInMainThread(() =>
                            {
                                NetRoseScopeServerSide centralScope = netRoseProtocolServerSide.ScopesProtocolServerSide
                                    .LoadedScopes[1].GetComponent<NetRoseScopeServerSide>();
                                Map centralMap = centralScope.GetComponent<Scope>()[0];
                                // Store the account data.
                                loginProtocolServerSide.SetSessionData(clientId, "account", account);
                                // Instantiate the prefab for the player.
                                var obj = netRoseProtocolServerSide.InstantiateHere(
                                    0, async (obj) =>
                                    {
                                        PlayerCharacterServerSide objSS = obj.GetComponent<PlayerCharacterServerSide>();
                                        // Give it the required connection id.
                                        objSS.Owner = clientId;
                                        // Add it to the dictionary.
                                        objects[clientId] = objSS;
                                        times[clientId] = 0;
                                        // And attach it. This will causa a spawn.
                                        MapObject mapObj = obj.GetComponent<MapObject>();
                                        mapObj.Initialize();
                                        mapObj.Attach(centralMap, PLAYER_X, PLAYER_Y, true);
                                    }
                                );
                                // Put it in the appropriate scope, and synchronize.
                            });
                        };
                        loginProtocolServerSide.OnSessionTerminating += async (clientId, account) =>
                        {
                            _ = RunInMainThread(() =>
                            {
                                if (objects.TryGetValue(clientId, out PlayerCharacterServerSide objSS))
                                {
                                    // It will de-spawn and destroy the object.
                                    Destroy(objSS.MapObject.gameObject);
                                    // Then, unregistering it.
                                    objects.Remove(clientId);
                                    times.Remove(clientId);
                                }
                            });
                        };
                    }

                    private void DoThrottled(ulong connectionId, Action<MapObject> callback)
                    {
                        PlayerCharacterServerSide objSS = objects[connectionId];
                        MapObject obj = objSS.MapObject;
                        float time = Time.time;
                        if (times[objSS.Owner] + 0.75 / obj.Speed <= time)
                        {
                            times[objSS.Owner] = time;
                            callback(obj);
                        }
                    }
                    protected override void SetIncomingMessageHandlers()
                    {
                        base.SetIncomingMessageHandlers();
                        AddIncomingMessageHandler("MoveLeft", async (protocol, clientId) =>
                        {
                            var _ = RunInMainThread(() =>
                            {
                                DoThrottled(clientId, (obj) =>
                                {
                                    obj.Orientation = Direction.LEFT;
                                    obj.StartMovement(Direction.LEFT);
                                });
                            });
                        });
                        AddIncomingMessageHandler("MoveRight", async (protocol, clientId) =>
                        {
                            var _ = RunInMainThread(() =>
                            {
                                DoThrottled(clientId, (obj) =>
                                {
                                    obj.Orientation = Direction.RIGHT;
                                    obj.StartMovement(Direction.RIGHT);
                                });
                            });
                        });
                        AddIncomingMessageHandler("MoveUp", async (protocol, clientId) =>
                        {
                            var _ = RunInMainThread(() =>
                            {
                                DoThrottled(clientId, (obj) =>
                                {
                                    obj.Orientation = Direction.UP;
                                    obj.StartMovement(Direction.UP);
                                });
                            });
                        });
                        AddIncomingMessageHandler("MoveDown", async (protocol, clientId) =>
                        {
                            var _ = RunInMainThread(() =>
                            {
                                DoThrottled(clientId, (obj) =>
                                {
                                    obj.Orientation = Direction.DOWN;
                                    obj.StartMovement(Direction.DOWN);
                                });
                            });
                        });
                        AddIncomingMessageHandler("Talk", async (protocol, clientId) =>
                        {
                            DoThrottled(clientId, (obj) =>
                            {
                                obj.GetComponent<TalkSender>().Talk();
                            });
                        });
                    }

                    public void SendCurrentMission(ulong clientId, int cityIndex, string questionType)
                    {
                        _ = CurrentMissionSender(clientId, new CurrentMission()
                        {
                            CityIndex = cityIndex, QuestionType = questionType
                        });
                    }

                    public void SendAlreadyComplete(ulong clientId)
                    {
                        _ = AlreadyCompleteSender(clientId);
                    }

                    public void SendInfo(ulong clientId, int cityIndex)
                    {
                        _ = InfoSender(clientId, (Int) cityIndex);
                    }

                    public void SendStepComplete(ulong clientId, int cityIndex)
                    {
                        _ = StepCompleteSender(clientId, (Int) cityIndex);
                    }

                    public void SendYouJustCompleted(ulong clientId)
                    {
                        _ = YouJustCompletedSender(clientId);
                    }

                    public void BroadcastTheyJustCompleted(String nickname)
                    {
                        _ = TheyJustCompletedBroadcaster(null, nickname);
                    }
                }
            }
        }
    }
}