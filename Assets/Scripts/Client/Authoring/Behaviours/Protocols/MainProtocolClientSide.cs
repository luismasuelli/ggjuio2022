using System;
using System.Threading.Tasks;
using AlephVault.Unity.Binary.Wrappers;
using AlephVault.Unity.Meetgard.Auth.Protocols.Simple;
using AlephVault.Unity.Meetgard.Auth.Types;
using AlephVault.Unity.Meetgard.Authoring.Behaviours.Client;
using AlephVault.Unity.Meetgard.Protocols;
using AlephVault.Unity.Meetgard.Samples.Chat;
using AlephVault.Unity.Meetgard.Types;
using AlephVault.Unity.RemoteStorage.Types.Results;
using AlephVault.Unity.Support.Authoring.Behaviours;
using AlephVault.Unity.Support.Utils;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Client;
using GameMeanMachine.Unity.WindRose.Types;
using GGJUIO2020.Client.Authoring.Behaviours.Protocols.Models;
using GGJUIO2020.Server.Authoring.Behaviours.External;
using GGJUIO2020.Types.Models;
using GGJUIO2020.Types.Protocols.Definitions;
using GGJUIO2020.Types.Protocols.Messages;
using UnityEngine;
using Exception = System.Exception;
using String = AlephVault.Unity.Binary.Wrappers.String;


namespace GGJUIO2020.Client
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace Protocols
            {
                [RequireComponent(typeof(Throttler))]
                [RequireComponent(typeof(PingProtocolClientSide))]
                [RequireComponent(typeof(PrincipalObjectsNetRoseProtocolClientSide))]
                [RequireComponent(typeof(LoginProtocolClientSide))]
                public class MainProtocolClientSide : ProtocolClientSide<MainProtocolDefinition>
                {
                    private Func<Task> MoveLeft;
                    private Func<Task> MoveRight;
                    private Func<Task> MoveUp;
                    private Func<Task> MoveDown;
                    private Func<Task> Talk;

                    private LoginProtocolClientSide loginProtocol;
                    private Throttler throttler;

                    public event Func<Task> AlreadyComplete;
                    public event Func<int, string, Task> CurrentMission;
                    public event Func<int, Task> Info;
                    public event Func<int, Task> StepComplete;
                    public event Func<Task> YouJustCompleted;
                    public event Func<string, Task> TheyJustCompleted; 

                    protected override void Setup()
                    {
                        throttler = GetComponent<Throttler>();
                        loginProtocol = GetComponent<LoginProtocolClientSide>();
                    }

                    protected override void Initialize()
                    {
                        MoveLeft = MakeSender("MoveLeft");
                        MoveRight = MakeSender("MoveRight");
                        MoveUp = MakeSender("MoveUp");
                        MoveDown = MakeSender("MoveDown");
                        Talk = MakeSender("Talk");
                    }

                    protected override void SetIncomingMessageHandlers()
                    {
                        // Referee.
                        AddIncomingMessageHandler<CurrentMission>("CurrentMission", async (protocol, mission) =>
                        {
                            Debug.Log($"Current mission: {protocol}/{mission}");
                            await (CurrentMission?.InvokeAsync(mission.CityIndex, mission.QuestionType) ?? Task.CompletedTask);

                        });
                        AddIncomingMessageHandler("AlreadyComplete", async protocol =>
                        {
                            Debug.Log($"Already complete");
                            await (AlreadyComplete?.InvokeAsync() ?? Task.CompletedTask);
                        });
                        // Informant.
                        AddIncomingMessageHandler<Int>("Info", async (protocol, city) =>
                        {
                            Debug.Log($"Info: {city}");
                            await (Info?.InvokeAsync(city) ?? Task.CompletedTask);
                        });
                        AddIncomingMessageHandler<Int>("StepComplete", async (protocol, city) =>
                        {
                            Debug.Log($"Step complete: {city}");
                            await (StepComplete?.InvokeAsync(city) ?? Task.CompletedTask);
                        });
                        // Notifications.
                        AddIncomingMessageHandler("YouJustCompleted", async protocol =>
                        {
                            Debug.Log("You just completed all the missions");
                            await (YouJustCompleted?.InvokeAsync() ?? Task.CompletedTask);
                        });
                        AddIncomingMessageHandler<String>("SomeoneJustCompleted", async (protocol, nickname) =>
                        {
                            Debug.Log($"{nickname} : they just completed all the missions");
                            await (TheyJustCompleted?.InvokeAsync(nickname) ?? Task.CompletedTask);
                        });
                    }
                    
                    public void TalkNPC()
                    {
                        if (loginProtocol.LoggedIn) throttler.Throttled(() => { RunInMainThread(Talk); });
                    }
                    
                    public void WalkDown()
                    {
                        if (loginProtocol.LoggedIn) throttler.Throttled(() => {
                            RunInMainThread(() => {
                                if (PlayerCharacterClientSide.Instance)
                                {
                                    PlayerCharacterClientSide.Instance.MapObject.StartMovement(Direction.DOWN);
                                }
                                MoveDown();
                            });
                        });
                    }

                    public void WalkLeft()
                    {
                        if (loginProtocol.LoggedIn) throttler.Throttled(() => {
                            RunInMainThread(() => {
                                if (PlayerCharacterClientSide.Instance)
                                {
                                    PlayerCharacterClientSide.Instance.MapObject.StartMovement(Direction.LEFT);
                                }
                                MoveLeft();
                            });
                        });
                    }

                    public void WalkRight()
                    {
                        if (loginProtocol.LoggedIn) throttler.Throttled(() => {
                            RunInMainThread(() => {
                                if (PlayerCharacterClientSide.Instance)
                                {
                                    PlayerCharacterClientSide.Instance.MapObject.StartMovement(Direction.RIGHT);
                                }
                                MoveRight();
                            });
                        });
                    }

                    public void WalkUp()
                    {
                        if (loginProtocol.LoggedIn) throttler.Throttled(() => {
                            RunInMainThread(() => {
                                if (PlayerCharacterClientSide.Instance)
                                {
                                    PlayerCharacterClientSide.Instance.MapObject.StartMovement(
                                        Direction.UP, PlayerCharacterClientSide.Instance.MapObject.IsMoving
                                    );
                                }
                                MoveUp();
                            });
                        });
                    }
                }
            }
        }
    }
}