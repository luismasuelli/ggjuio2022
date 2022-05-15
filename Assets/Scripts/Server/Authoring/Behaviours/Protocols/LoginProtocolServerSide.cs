using System;
using System.Threading.Tasks;
using AlephVault.Unity.Meetgard.Auth.Protocols.Simple;
using AlephVault.Unity.Meetgard.Auth.Types;
using AlephVault.Unity.Meetgard.Types;
using AlephVault.Unity.RemoteStorage.Types.Results;
using GGJUIO2020.Server.Authoring.Behaviours.External;
using GGJUIO2020.Types.Models;
using GGJUIO2020.Types.Protocols.Definitions;
using GGJUIO2020.Types.Protocols.Messages;
using UnityEngine;
using Exception = System.Exception;
using String = AlephVault.Unity.Binary.Wrappers.String;


namespace GGJUIO2020.Server
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace Protocols
            {
                [RequireComponent(typeof(Storage))]
                public class LoginProtocolServerSide : SimpleAuthProtocolServerSide<
                    LoginProtocolDefinition, Nothing, LoginFailed, Kicked, string, String, UserAccount
                >
                {
                    public Storage Storage { get; private set; }
                    
                    protected override void Setup()
                    {
                        Storage = GetComponent<Storage>();
                    }

                    protected override async Task<UserAccount> FindAccount(string id)
                    {
                        try
                        {
                            return new UserAccount(await RunInMainThread(() => Storage.GetUserByLogin(id)));
                        }
                        catch (Storage.UserException e)
                        {
                            if (e.ErrorCode == ResultCode.DoesNotExist) return null;
                            throw;
                        }
                    }

                    protected override AccountAlreadyLoggedManagementMode IfAccountAlreadyLoggedIn()
                    {
                        return AccountAlreadyLoggedManagementMode.Ghost;
                    }

                    protected override async Task OnSessionError(ulong clientId, SessionStage stage, Exception error)
                    {
                        Debug.LogError($"For client id: {clientId}, stage: {stage}");
                        Debug.LogException(error);
                    }

                    protected override void SetLoginMessageHandlers()
                    {
                        // <LoginBody>("Login:Community");
                        AddLoginMessageHandler<LoginBody>("Login:Community", async (body) =>
                        {
                            try
                            {
                                UserAccount userAccount = await FindAccount(body.Login);
                                if (body.Password == userAccount.Model.Password)
                                {
                                    return new Tuple<bool, Nothing, LoginFailed, string>(true, new Nothing(), null, userAccount.Login);
                                }
                                return new Tuple<bool, Nothing, LoginFailed, string>(false, null, new LoginFailed() { Reason = "Password Invalido" }, "");
                            }
                            catch (Exception e)
                            {
                                return new Tuple<bool, Nothing, LoginFailed, string>(false, null, new LoginFailed() { Reason = "Error Interno" }, "");
                            }
                        });
                    }
                }
            }
        }
    }
}