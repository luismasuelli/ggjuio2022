using System;
using AlephVault.Unity.Meetgard.Types;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Client;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
using GameMeanMachine.Unity.WindRose.Authoring.Behaviours.Entities.Objects.CommandExchange.Talk;
using GGJUIO2020.Client.Authoring.Behaviours.Protocols;
using GGJUIO2020.Types.Models;
using GGJUIO2020.Types.Protocols.Messages;
using TMPro;
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
                namespace Models
                {
                    /// <summary>
                    ///   Characters only have a nickname. They have a pretty same and
                    ///   bored design. But the nick is different.
                    /// </summary>
                    [RequireComponent(typeof(TalkReceiver))]
                    public class InformantServerSide : NetRoseModelServerSide<Nothing, Nothing>
                    {
                        internal int cityIndex;
                        
                        protected void Start()
                        {
                            base.Start();
                            GetComponent<TalkReceiver>().onTalkReceived.AddListener((obj) =>
                            {
                                Debug.Log("On Talk Received (informant)");
                                LoginProtocolServerSide loginProtocol = Protocol.GetComponent<LoginProtocolServerSide>();
                                MainProtocolServerSide mainProtocol = Protocol.GetComponent<MainProtocolServerSide>();
                                PlayerCharacterServerSide objSS = obj.GetComponent<PlayerCharacterServerSide>();
                                if (objSS)
                                {
                                    ulong owner = objSS.GetOwner();
                                    UserAccount account = (UserAccount)loginProtocol.GetSessionData(owner, "account");
                                    if (account.Model.Progress == 9)
                                    {
                                        Debug.Log($"Sending info (city={cityIndex})");
                                        mainProtocol.SendInfo(owner, cityIndex);
                                    }
                                    else
                                    {
                                        int progress = account.Model.Progress;
                                        QuestItem item = account.Model.Quest[progress];
                                        if (item.CityIndex != cityIndex)
                                        {
                                            Debug.Log($"Sending info (city={cityIndex})");
                                            mainProtocol.SendInfo(owner, cityIndex);
                                        }
                                        else
                                        {
                                            Debug.Log($"Sending step complete (city={cityIndex})");
                                            mainProtocol.SendStepComplete(owner, cityIndex);
                                            account.Model.Progress += 1;
                                            _ = Protocol.RunInMainThread(async () =>
                                            {
                                                await loginProtocol.Storage.UpdateUser(account.Model);
                                            });

                                            if (account.Model.Progress == 9)
                                            {
                                                mainProtocol.SendYouJustCompleted(owner);
                                                mainProtocol.BroadcastTheyJustCompleted((String)account.NickName);
                                            }
                                        }
                                    }
                                }
                            });
                        }
                        
                        protected override Nothing GetInnerFullData(ulong connectionId)
                        {
                            return new Nothing() {};
                        }

                        protected override Nothing GetInnerRefreshData(ulong connectionId, string context)
                        {
                            return new Nothing() {};
                        }
                    }
                }
            }
        }
    }
}