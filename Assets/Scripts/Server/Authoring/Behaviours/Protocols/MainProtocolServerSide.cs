using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlephVault.Unity.Binary.Wrappers;
using AlephVault.Unity.Meetgard.Authoring.Behaviours.Server;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.Entities.Objects;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.Entities.Objects.CommandExchange.Talk;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.World;
using GameMeanMachine.Unity.WindRose.Types;
using GGJUIO2020.Server.Authoring.Behaviours.Protocols.Models;
using GGJUIO2020.Types.Protocols.Definitions;
using GGJUIO2020.Types.Protocols.Messages;
using UnityEngine;
using Debug = UnityEngine.Debug;
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
                [RequireComponent(typeof(PlayerCharacterPrincipalProtocolServerSide))]
                [RequireComponent(typeof(LoginProtocolServerSide))]
                public class MainProtocolServerSide : ProtocolServerSide<MainProtocolDefinition>
                {
                    private LoginProtocolServerSide loginProtocolServerSide;
                    private PlayerCharacterPrincipalProtocolServerSide playerCharacterPrincipalProtocolServerSide;
                    private Dictionary<ulong, PlayerCharacterServerSide> playerCharacters;
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
                        playerCharacterPrincipalProtocolServerSide = GetComponent<PlayerCharacterPrincipalProtocolServerSide>();
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
                            loginProtocolServerSide.SetSessionData(clientId, "account", account);
                            playerCharacterPrincipalProtocolServerSide.InstantiateCharacter(
                                clientId, () => {
                                    NetRoseScopeServerSide centralScope = playerCharacterPrincipalProtocolServerSide.ScopesProtocolServerSide
                                        .LoadedScopes[1].GetComponent<NetRoseScopeServerSide>();
                                    return centralScope.GetComponent<Scope>()[0];
                                }, PLAYER_X, PLAYER_Y, (ch) => {
                                    ch.NickName = account.NickName;
                                }
                            );
                        };
                        loginProtocolServerSide.OnSessionTerminating += async (clientId, account) =>
                        {
                            playerCharacterPrincipalProtocolServerSide.RemoveCharacter(clientId);
                        };
                    }

                    private void DoThrottled(ulong connectionId, Action<MapObject> callback)
                    {
                        PlayerCharacterServerSide objSS = playerCharacterPrincipalProtocolServerSide.GetPrincipal(connectionId);
                        MapObject obj = objSS.MapObject;
                        float time = Time.time;
                        if (objSS.ThrottleTime + 0.75 / obj.Speed <= time)
                        {
                            objSS.ThrottleTime = time;
                            callback(obj);
                        }
                    }
                    protected override void SetIncomingMessageHandlers()
                    {
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
                            var _ = RunInMainThread(() =>
                            {
                                DoThrottled(clientId, (obj) =>
                                {
                                    Debug.Log("Talking...");
                                    obj.GetComponent<TalkSender>().Talk();
                                });
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