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
                public class Storage : MonoBehaviour
                {
                    public class UserException : Exception
                    {
                        /// <summary>
                        ///   The error code.
                        /// </summary>
                        public readonly ResultCode ErrorCode;

                        public UserException(ResultCode code) : base("Resource error")
                        {
                            ErrorCode = code;
                        }
                    }
                    
                    /// <summary>
                    ///   The API key.
                    /// </summary>
                    [SerializeField]
                    private string ApiKey;

                    // The root handler.
                    private Root httpRoot;

                    // The users handler.
                    private ListResource<User, User> users;

                    private void Awake()
                    {
                        httpRoot = new Root("http://localhost:6666", new Authorization("Bearer", "sample-abcdef"));
                        users = (ListResource<User, User>)httpRoot.GetList<User, User>("players");
                    }

                    /// <summary>
                    ///   Gets a user by its login account.
                    /// </summary>
                    /// <param name="login">The login</param>
                    /// <exception cref="Exception">When something is not ok, or missing</exception>
                    public async Task<User> GetUserByLogin(string login)
                    {
                        Result<JObject, string> result = await users.View(
                            "by-login", new Dictionary<string, string>() {{"login", login}}
                        );
                        if (result.Code == ResultCode.Ok)
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            return serializer.Deserialize<User>(new JTokenReader(result.Element));
                        }
                        else if (result.Code == ResultCode.DoesNotExist)
                        {
                            throw new UserException(ResultCode.DoesNotExist);
                        }
                        else
                        {
                            Debug.LogError($"HTTP Error code is: {result.Code}");
                            throw new UserException(ResultCode.ServerError);
                        }
                    }

                    /// <summary>
                    ///   Registers a user by its full data (_id is ignored).
                    /// </summary>
                    /// <param name="user">The whole user data</param>
                    public async Task CreateUser(User user)
                    {
                        Result<User, string> result = await users.Create(user);
                        if (result.Code == ResultCode.Created)
                        {
                            // Everything ok.
                            return;
                        }
                        else if (result.Code == ResultCode.DuplicateKey)
                        {
                            throw new UserException(ResultCode.DuplicateKey);
                        }
                        else if (result.Code == ResultCode.ValidationError)
                        {
                            throw new UserException(ResultCode.ValidationError);
                        }
                        else
                        {
                            Debug.LogError($"HTTP Error code is: {result.Code}");
                            throw new UserException(ResultCode.ServerError);
                        }
                    }

                    /// <summary>
                    ///   Updates (i.e. fully replaces) a user record.
                    /// </summary>
                    /// <param name="user">The user to update. The _id and its data are taken into account</param>
                    public async Task UpdateUser(User user)
                    {
                        Result<User, string> result = await users.Replace(user.Id, user);
                        if (result.Code == ResultCode.Created)
                        {
                            // Everything ok.
                            return;
                        }
                        else if (result.Code == ResultCode.DuplicateKey)
                        {
                            throw new UserException(ResultCode.DuplicateKey);
                        }
                        else if (result.Code == ResultCode.ValidationError)
                        {
                            throw new UserException(ResultCode.ValidationError);
                        }
                        else
                        {
                            Debug.LogError($"HTTP Error code is: {result.Code}");
                            throw new UserException(ResultCode.ServerError);
                        }
                    }
                }
            }
        }
    }
}