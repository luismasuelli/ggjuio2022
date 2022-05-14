using System;
using System.Threading.Tasks;
using UnityEngine;


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