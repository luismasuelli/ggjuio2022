using System.Threading.Tasks;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
using GameMeanMachine.Unity.NetRose.Types.Models;
using GGJUIO2020.Types.Protocols.Messages;
using TMPro;


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
                    public class PlayerCharacterServerSide : OwnedNetRoseModelServerSide<CharacterDetails, CharacterDetails>
                    {
                        private TMP_Text label;

                        private TMP_Text GetLabel()
                        {
                            if (!label) label = GetComponentInChildren<TMP_Text>();
                            return label;
                        }

                        private string oldNickname;

                        public float ThrottleTime = 0;

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
                        
                        protected override OwnedModel<CharacterDetails> GetInnerFullData(ulong connectionId)
                        {
                            return new OwnedModel<CharacterDetails>()
                            {
                                Owned = connectionId == GetOwner(),
                                Data = new CharacterDetails()
                                {
                                    NickName = NickName
                                }
                            };
                        }

                        protected override CharacterDetails GetInnerRefreshData(ulong connectionId, string context)
                        {
                            return new CharacterDetails() { NickName = NickName };
                        }
                    }
                }
            }
        }
    }
}