using System;
using System.Threading.Tasks;
using AlephVault.Unity.Meetgard.Authoring.Behaviours.Server;
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
                public class RegisterProtocolServerSide : ProtocolServerSide<RegisterProtocolDefinition>
                {
                    private Storage storage;
                    private Func<ulong, Task> Ok; 
                    private Func<ulong, Task> Duplicate; 
                    private Func<ulong, Task> PasswordMismatch; 
                    private Func<ulong, Task> Invalid; 
                    private Func<ulong, Task> UnexpectedError;
                    
                    protected override void Setup()
                    {
                        storage = GetComponent<Storage>();
                        Ok = MakeSender("Ok");
                        Duplicate = MakeSender("Duplicate");
                        PasswordMismatch = MakeSender("PasswordMismatch");
                        Invalid = MakeSender("Invalid");
                        UnexpectedError = MakeSender("UnexpectedError");
                    }

                    protected override void SetIncomingMessageHandlers()
                    {
                        AddIncomingMessageHandler<UserBody>("Register", async (protocol, clientId, body) =>
                        {
                            if (body.PasswordsMatch())
                            {
                                PasswordMismatch(clientId);
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
                                await storage.CreateUser(user);
                            }
                            catch (Storage.UserException e)
                            {
                                switch (e.ErrorCode)
                                {
                                    case ResultCode.ValidationError:
                                        Invalid(clientId);
                                        break;
                                    case ResultCode.DuplicateKey:
                                        Duplicate(clientId);
                                        break;
                                    default:
                                        Debug.LogError($"Exception for connection {clientId} from http: {e.ErrorCode}");
                                        UnexpectedError(clientId);
                                        break;
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogError($"Exception for connection {clientId}");
                                Debug.LogException(e);
                                UnexpectedError(clientId);
                            }
                        });
                    }

                    private QuestItem[] GenerateQuest()
                    {
                        // Generate and shuffle the cities.
                        int[] cities = {0, 1, 2, 3, 4, 5, 6, 7, 8};
                        for (int i = 0; i < 9; i++) {
                            int temp = cities[i];
                            int randomIndex = UnityEngine.Random.Range(i, 9);
                            cities[i] = cities[randomIndex];
                            cities[randomIndex] = temp;
                        }

                        // Make a random generator for the question types.
                        Random<string> questions = new Random<string>(new[] {"cuisine", "regional", "culture"});

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