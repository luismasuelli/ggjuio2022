using System;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Client;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
using GGJUIO2020.Types.Protocols.Messages;
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
                    public class PlayerCharacterServerSide : NetRoseModelServerSide<CharacterDetails, CharacterDetails>
                    {
                        private TMP_Text label;
                        internal ulong Owner;

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
                        
                        protected override CharacterDetails GetInnerFullData(ulong connectionId)
                        {
                            return new CharacterDetails() {NickName = NickName, Owned = connectionId == Owner};
                        }

                        protected override CharacterDetails GetInnerRefreshData(ulong connectionId, string context)
                        {
                            return new CharacterDetails() {NickName = NickName, Owned = connectionId == Owner};
                        }
                    }
                }
            }
        }
    }
}