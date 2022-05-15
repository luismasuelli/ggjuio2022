using AlephVault.Unity.Binary.Wrappers;
using GameMeanMachine.Unity.NetRose.Authoring.Behaviours.Client;
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
                    public class PlayerCharacterClientSide : NetRoseModelClientSide<String, String>
                    {
                        private TMP_Text label;

                        private TMP_Text GetLabel()
                        {
                            if (!label) label = GetComponentInChildren<TMP_Text>();
                            return label;
                        }
                        
                        protected override void InflateFrom(String fullData)
                        {
                            GetLabel().text = fullData;
                        }

                        protected override void UpdateFrom(String refreshData)
                        {
                            GetLabel().text = refreshData;
                        }
                    }
                }
            }
        }
    }
}