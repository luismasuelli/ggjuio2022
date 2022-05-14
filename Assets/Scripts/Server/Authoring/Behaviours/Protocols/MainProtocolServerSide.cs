using System;
using System.Collections.Generic;
using System.Diagnostics;
using AlephVault.Unity.Meetgard.Authoring.Behaviours.Server;
using AlephVault.Unity.Meetgard.Types;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
using GGJUIO2020.Server.Authoring.Behaviours.Protocols.Models;
using GGJUIO2020.Types.Protocols.Definitions;
using UnityEngine;


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

                    protected override void Setup()
                    {
                        base.Setup();
                        loginProtocolServerSide = GetComponent<LoginProtocolServerSide>();
                        netRoseProtocolServerSide = GetComponent<NetRoseProtocolServerSide>();
                    }

                    protected override void Initialize()
                    {
                        base.Initialize();
                        loginProtocolServerSide.OnSessionStarting += async (clientId, account) =>
                        {
                            // Store the account data.
                            loginProtocolServerSide.SetSessionData(clientId, "account", account);
                            // Instantiate the prefab for the player.
                            // Put it in the appropriate scope, and synchronize.
                        };
                        loginProtocolServerSide.OnSessionTerminating += async (clientId, account) =>
                        {
                            // Release everything.
                        };
                    }

                    protected override void SetIncomingMessageHandlers()
                    {
                        base.SetIncomingMessageHandlers();
                        AddIncomingMessageHandler("MoveLeft", async (protocol, clientId) =>
                        {
                            
                        });
                        AddIncomingMessageHandler("MoveRight", async (protocol, clientId) =>
                        {
                            
                        });
                        AddIncomingMessageHandler("MoveUp", async (protocol, clientId) =>
                        {
                            
                        });
                        AddIncomingMessageHandler("MoveDown", async (protocol, clientId) =>
                        {
                            
                        });
                        AddIncomingMessageHandler("Talk", async (protocol, clientId) =>
                        {
                            
                        });
                    }
                }
            }
        }
    }
}