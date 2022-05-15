using AlephVault.Unity.Meetgard.Authoring.Behaviours.Server;
using UnityEngine;


namespace GGJUIO2020.Server
{
    namespace Authoring
    {
        namespace Behaviours
        {
            [RequireComponent(typeof(NetworkServer))]
            public class RegisterServerLauncher : MonoBehaviour
            {
                // Start is called before the first frame update
                void Start()
                {
                    GetComponent<NetworkServer>().StartServer(16666);
                }

                private void OnDestroy()
                {
                    NetworkServer server = GetComponent<NetworkServer>();
                    if (server.IsListening)
                    {
                        server.StopServer();
                    }
                }
            }
        }
    }
}
