using System.Threading.Tasks;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Server;
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

                        protected void Awake()
                        {
                            base.Awake();
                            OnAfterSpawned += OwnableModelServerSide_OnSpawned;
                            OnBeforeDespawned += OwnableModelServerSide_OnDespawned;
                        }
                        
                        protected void OnDestroy()
                        {
                            base.OnDestroy();
                            OnSpawned -= OwnableModelServerSide_OnSpawned;
                            OnDespawned -= OwnableModelServerSide_OnDespawned;
                        }

                        protected override CharacterDetails GetInnerFullData(ulong connectionId)
                        {
                            return new CharacterDetails() {NickName = NickName, Owned = connectionId == Owner};
                        }

                        protected override CharacterDetails GetInnerRefreshData(ulong connectionId, string context)
                        {
                            return new CharacterDetails() {NickName = NickName, Owned = connectionId == Owner};
                        }

                        private async Task OwnableModelServerSide_OnDespawned()
                        {
                            var _ = Protocol.SendToLimbo(Owner);
                        }

                        private async Task OwnableModelServerSide_OnSpawned()
                        {
                            var _ = Protocol.SendTo(Owner, Scope.Id);
                        }
                    }
                }
            }
        }
    }
}