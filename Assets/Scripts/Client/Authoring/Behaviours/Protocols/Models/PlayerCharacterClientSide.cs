using AlephVault.Unity.Binary.Wrappers;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Client;
using GGJUIO2020.Types.Protocols.Messages;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;


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
                    public class PlayerCharacterClientSide : NetRoseModelClientSide<CharacterDetails, CharacterDetails>
                    {
                        private TMP_Text label;
                        public static PlayerCharacterClientSide OwnedInstance { get; private set; }

                        private TMP_Text GetLabel()
                        {
                            if (!label) label = GetComponentInChildren<TMP_Text>();
                            return label;
                        }
                        
                        protected override void InflateFrom(CharacterDetails fullData)
                        {
                            GetLabel().text = fullData.NickName;
                            if (OwnedInstance == this && !fullData.Owned)
                            {
                                OwnedInstance = null;
                            }
                            else if (OwnedInstance != this && fullData.Owned)
                            {
                                OwnedInstance = this;
                            }
                        }

                        protected override void UpdateFrom(CharacterDetails refreshData)
                        {
                            GetLabel().text = refreshData.NickName;
                            if (OwnedInstance == this && !refreshData.Owned)
                            {
                                OwnedInstance = null;
                            }
                            else if (OwnedInstance != this && refreshData.Owned)
                            {
                                OwnedInstance = this;
                            }
                        }
                    }
                }
            }
        }
    }
}