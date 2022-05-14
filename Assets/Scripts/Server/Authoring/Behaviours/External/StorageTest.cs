using System;
using System.Threading.Tasks;
using UnityEngine;
using AlephVault.Unity.RemoteStorage.StandardHttp.Types;
using AlephVault.Unity.Support.Generic.Authoring.Types;
using AlephVault.Unity.RemoteStorage.Types.Results;
using GGJUIO2020.Server.Types.RemoteStorage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace GGJUIO2020.Server
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace External
            {
                [RequireComponent(typeof(Storage))]
                public class StorageTest : MonoBehaviour
                {
                    private Storage storage;
                    
                    private void Awake()
                    {
                        storage = GetComponent<Storage>();
                    }

                    private void Start()
                    {
                        RunTest();
                    }

                    private async void RunTest()
                    {
                        Debug.Log($"User: {await storage.GetUserByLogin("sample-user")}");                        
                    }
                }
            }
        }
    }
}