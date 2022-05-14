using System;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Client;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
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
                    public class PlayerCharacterServerSide : NetRoseModelServerSide<String, String>
                    {
                        private string oldNickname;
                        public string NickName;
                        
                        protected override String GetInnerFullData(ulong connectionId)
                        {
                            return (String)NickName;
                        }

                        protected override String GetInnerRefreshData(ulong connectionId, string context)
                        {
                            return (String)NickName;
                        }

                        private void Update()
                        {
                            if (oldNickname != NickName)
                            {
                                // TODO refresh the nickname in the label.
                            }
                        }
                    }
                }
            }
        }
    }
}