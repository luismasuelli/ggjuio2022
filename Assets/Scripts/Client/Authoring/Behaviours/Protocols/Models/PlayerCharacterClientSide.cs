using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Client;
using GGJUIO2020.Types.Protocols.Messages;
using TMPro;


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
                        
                        private TMP_Text GetLabel()
                        {
                            if (!label) label = GetComponentInChildren<TMP_Text>();
                            return label;
                        }
                        
                        protected override void InflateOwnedFrom(CharacterDetails fullData)
                        {
                            GetLabel().text = fullData.NickName;
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