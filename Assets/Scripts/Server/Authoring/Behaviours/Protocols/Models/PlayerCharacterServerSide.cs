using System;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Client;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
using TMPro;
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
                        private TMP_Text label;

                        private TMP_Text GetLabel()
                        {
                            if (!label) label = GetComponentInChildren<TMP_Text>();
                            return label;
                        }

                        private string oldNickname;

                        public string NickName
                        {
                            set
                            {
                                // WARNING: THIS MUST ONLY BE CALLED ON INSTANTIATION.
                                GetLabel().text = value;
                            }
                            get
                            {
                                return GetLabel().text;
                            }
                        }
                        
                        protected override String GetInnerFullData(ulong connectionId)
                        {
                            return (String)NickName;
                        }

                        protected override String GetInnerRefreshData(ulong connectionId, string context)
                        {
                            return (String)NickName;
                        }
                    }
                }
            }
        }
    }
}