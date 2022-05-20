using AlephVault.Unity.Meetgard.Authoring.Behaviours.Server;
using UnityEngine;


namespace GGJUIO2020.Server
{
    namespace Authoring
    {
        namespace Behaviours
        {
            [RequireComponent(typeof(NetworkServer))]
            public class GameServerLauncher : MonoBehaviour
            {
                // Start is called before the first frame update
                void Start()
                {
                    Application.runInBackground = true;
                    GetComponent<NetworkServer>().StartServer(16667);
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
