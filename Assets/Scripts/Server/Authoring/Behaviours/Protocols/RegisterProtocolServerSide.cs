using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AlephVault.Unity.Meetgard.Authoring.Behaviours.Client;
using AlephVault.Unity.Meetgard.Authoring.Behaviours.Server;
using AlephVault.Unity.Meetgard.Protocols.Simple;
using AlephVault.Unity.RemoteStorage.Types.Results;
using AlephVault.Unity.Support.Generic.Types.Sampling;
using GGJUIO2020.Server.Authoring.Behaviours.External;
using GGJUIO2020.Types.Models;
using GGJUIO2020.Types.Protocols.Definitions;
using GGJUIO2020.Types.Protocols.Messages;
using UnityEngine;


namespace GGJUIO2020.Server
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace Protocols
            {
                [RequireComponent(typeof(Storage))]
                public class RegisterProtocolServerSide : MandatoryHandshakeProtocolServerSide<RegisterProtocolDefinition>
                {
                    private Storage storage;
                    private Func<ulong, Task> Ok; 
                    private Func<ulong, Task> Duplicate; 
                    private Func<ulong, Task> PasswordMismatch; 
                    private Func<ulong, Task> Invalid; 
                    private Func<ulong, Task> UnexpectedError;
                    private Func<ulong, Task> SendWelcome;
                    private Func<ulong, Task> SendTimeout;
                    
                    protected override void Setup()
                    {
                        storage = GetComponent<Storage>();
                    }
                    
                    protected override void Initialize()
                    {
                        base.Initialize();
                        Ok = MakeSender("Ok");
                        Duplicate = MakeSender("Duplicate");
                        PasswordMismatch = MakeSender("PasswordMismatch");
                        Invalid = MakeSender("Invalid");
                        UnexpectedError = MakeSender("UnexpectedError");
                        SendWelcome = MakeSender("Welcome");
                        SendTimeout = MakeSender("Timeout");
                    }
                    
                    protected override void SetIncomingMessageHandlers()
                    {
                        AddIncomingMessageHandler<UserBody>("Register", async (protocol, clientId, body) =>
                        {
                            RemoveHandshakePending(clientId);
                            if (!body.PasswordsMatch())
                            {
                                await PasswordMismatch(clientId);
                                // Then, close the client.
                                server.Close(clientId);
                                return;
                            }
                            
                            User user = new User();
                            user.Login = body.Login;
                            user.Password = body.Password;
                            user.Progress = 0;
                            user.NickName = body.NickName;
                            user.Quest = GenerateQuest();

                            try
                            {
                                await RunInMainThread(async () =>
                                {
                                    await storage.CreateUser(user);
                                });
                                await Ok(clientId);
                            }
                            catch (Storage.UserException e)
                            {
                                switch (e.ErrorCode)
                                {
                                    case ResultCode.ValidationError:
                                        await Invalid(clientId);
                                        break;
                                    case ResultCode.DuplicateKey:
                                        await Duplicate(clientId);
                                        break;
                                    default:
                                        Debug.LogError($"Exception for connection {clientId} from http: {e.ErrorCode}");
                                        await UnexpectedError(clientId);
                                        break;
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogError($"Exception for connection {clientId}");
                                Debug.LogException(e);
                                await UnexpectedError(clientId);
                            }
                            
                            // Then, close the client.
                            server.Close(clientId);
                        });
                    }

                    private QuestItem[] GenerateQuest()
                    {
                        // Make a random generator for the question types.
                        // Also a standard random for the ranges.
                        Random<string> questions = new Random<string>(new[] {"cuisine", "regional", "culture"});
                        System.Random r = new System.Random();
                        
                        // Generate and shuffle the cities.
                        int[] cities = {0, 1, 2, 3, 4, 5, 6, 7, 8};
                        for (int i = 0; i < 9; i++) {
                            int temp = cities[i];
                            int randomIndex = r.Next(i, 9);
                            cities[i] = cities[randomIndex];
                            cities[randomIndex] = temp;
                        }

                        QuestItem[] questItems = new QuestItem[9];
                        for (int i = 0; i < 9; i++)
                        {
                            questItems[i] = new QuestItem() {CityIndex = cities[i], questionType = questions.Get()};
                        }

                        return questItems;
                    }
                }
            }
        }
    }
}