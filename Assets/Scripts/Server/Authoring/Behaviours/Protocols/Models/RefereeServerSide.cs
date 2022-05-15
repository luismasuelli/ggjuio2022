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
                    public class RefereeServerSide : NetRoseModelServerSide<Nothing, Nothing>
                    {
                        protected void Start()
                        {
                            base.Start();
                            GetComponent<TalkReceiver>().onTalkReceived.AddListener((obj) =>
                            {
                                LoginProtocolServerSide loginProtocol = Protocol.GetComponent<LoginProtocolServerSide>();
                                MainProtocolServerSide mainProtocol = Protocol.GetComponent<MainProtocolServerSide>();
                                PlayerCharacterServerSide objSS = obj.GetComponent<PlayerCharacterServerSide>();
                                if (objSS)
                                {
                                    ulong owner = objSS.Owner;
                                    UserAccount account = (UserAccount)loginProtocol.GetSessionData(owner, "account");
                                    if (account.Model.Progress == 9)
                                    {
                                        mainProtocol.SendAlreadyComplete(owner);
                                    }
                                    else
                                    {
                                        int progress = account.Model.Progress;
                                        QuestItem item = account.Model.Quest[progress];
                                        mainProtocol.SendCurrentMission(owner, item.CityIndex, item.questionType);
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