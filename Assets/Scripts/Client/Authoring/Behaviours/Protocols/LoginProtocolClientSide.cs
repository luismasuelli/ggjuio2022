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


namespace GGJUIO2020.Client
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace Protocols
            {
                public class LoginProtocolClientSide : SimpleAuthProtocolClientSide<
                    LoginProtocolDefinition, Nothing, LoginFailed, Kicked
                >
                {
                    private Func<UserBody, Task> SendLogin;
                    
                    /// <summary>
                    ///   The login.
                    /// </summary>
                    public string Login;

                    /// <summary>
                    ///   The password.
                    /// </summary>
                    public string Password;
                    
                    protected override void Setup()
                    {
                        base.Setup();
                        OnWelcome += LoginProtocol_OnWelcome;
                        OnTimeout += LoginProtocol_OnTimeout;
                        OnLoginOK += LoginProtocol_OnLoginOK;
                        OnLoginFailed += LoginProtocol_OnLoginFailed;
                        OnKicked += LoginProtocol_OnKicked;
                        OnForbidden += LoginProtocol_OnForbidden;
                        OnLoggedOut += LoginProtocol_OnLoggedOut;
                        OnNotLoggedIn += LoginProtocol_OnNotLoggedIn;
                        OnAlreadyLoggedIn += LoginProtocol_OnAlreadyLoggedIn;
                        OnAccountAlreadyInUse += LoginProtocol_OnAccountAlreadyInUse;
                    }
                    
                    void OnDestroy()
                    {
                        OnWelcome -= LoginProtocol_OnWelcome;
                        OnTimeout -= LoginProtocol_OnTimeout;
                        OnLoginOK -= LoginProtocol_OnLoginOK;
                        OnLoginFailed -= LoginProtocol_OnLoginFailed;
                        OnKicked -= LoginProtocol_OnKicked;
                        OnForbidden -= LoginProtocol_OnForbidden;
                        OnLoggedOut -= LoginProtocol_OnLoggedOut;
                        OnNotLoggedIn -= LoginProtocol_OnNotLoggedIn;
                        OnAlreadyLoggedIn -= LoginProtocol_OnAlreadyLoggedIn;
                        OnAccountAlreadyInUse -= LoginProtocol_OnAccountAlreadyInUse;
                    }

                    private async Task LoginProtocol_OnWelcome()
                    {
                        Debug.Log($"SSAPClient({Login}) :: welcome");
                        _ = SendLogin(new UserBody() { Login = Login, Password = Password });
                    }
        
                    private async Task LoginProtocol_OnTimeout()
                    {
                        Debug.Log($"SSAPClient({Login}) :: timeout");
                    }
        
                    private async Task LoginProtocol_OnLoginOK(Nothing arg)
                    {
                        Debug.Log($"SSAPClient({Login}) :: login ok");
                    }
        
                    private async Task LoginProtocol_OnLoginFailed(LoginFailed arg)
                    {
                        Debug.Log($"SSAPClient({Login}) :: login failed: {arg}");
                    }
        
                    private async Task LoginProtocol_OnKicked(Kicked arg)
                    {
                        Debug.Log($"SSAPClient({Login}) :: Kicked: {arg}");
                    }
        
                    private async Task LoginProtocol_OnForbidden()
                    {
                        Debug.Log($"SSAPClient({Login}) :: forbidden");
                    }
        
                    private async Task LoginProtocol_OnLoggedOut()
                    {
                        Debug.Log($"SSAPClient({Login}) :: logged out");
                    }
        
                    private async Task LoginProtocol_OnNotLoggedIn()
                    {
                        Debug.Log($"SSAPClient({Login}) :: not logged in");
                    }
        
                    private async Task LoginProtocol_OnAlreadyLoggedIn()
                    {
                        Debug.Log($"SSAPClient({Login}) :: already logged in");
                    }
        
                    private async Task LoginProtocol_OnAccountAlreadyInUse()
                    {
                        Debug.Log($"SSAPClient({Login}) :: account already in use");
                    }

                    protected override void MakeLoginRequestSenders()
                    {
                        SendLogin = MakeLoginRequestSender<UserBody>("Login:Community");
                    }
                }
            }
        }
    }
}