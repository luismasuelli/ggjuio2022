using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Client;
using GameMeanMachine.Unity.WindRose.Types;
using GGJUIO2020.Types.Protocols.Messages;
using TMPro;
using UnityEngine;


namespace GGJUIO2020.Client
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace Protocols
            {
                namespace Models
                {
                    public class PlayerCharacterClientSide : OwnedNetRoseModelClientSide<CharacterDetails, CharacterDetails>
                    {
                        private TMP_Text label;

                        [SerializeField]
                        private bool optimistic = false;

                        public static PlayerCharacterClientSide Instance { get; private set; }

                        public override bool IsOptimistic()
                        {
                            return optimistic;
                        }
                        
                        private TMP_Text GetLabel()
                        {
                            if (!label) label = GetComponentInChildren<TMP_Text>();
                            return label;
                        }
                        
                        protected override void InflateOwnedFrom(CharacterDetails fullData)
                        {
                            GetLabel().text = fullData.NickName;
                            if (IsOwned() && IsOptimistic())
                            {
                                Instance = this;
                            }
                        }
                        
                        protected override void UpdateOwnedFrom(CharacterDetails refreshData)
                        {
                            GetLabel().text = refreshData.NickName;
                        }
                    }
                }
            }
        }
    }
}