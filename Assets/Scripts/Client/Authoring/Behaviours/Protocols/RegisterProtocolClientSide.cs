using System;
using System.Threading.Tasks;
using AlephVault.Unity.Meetgard.Authoring.Behaviours.Client;
using AlephVault.Unity.Support.Utils;
using GGJUIO2020.Types.Protocols.Definitions;
using GGJUIO2020.Types.Protocols.Messages;
using UnityEngine;


namespace GGJUIO2020.Client
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace Protocols
            {
                public class RegisterProtocolClientSide : ProtocolClientSide<RegisterProtocolDefinition>
                {
                    private Func<UserBody, Task> SendRegister;
                    
                    /// <summary>
                    ///   The login.
                    /// </summary>
                    public string Login;

                    /// <summary>
                    ///   The nickname.
                    /// </summary>
                    public string NickName;

                    /// <summary>
                    ///   The password.
                    /// </summary>
                    public string Password;

                    /// <summary>
                    ///   The password confirmation.
                    /// </summary>
                    public string PasswordConfirm;
                    
                    /// <summary>
                    ///   What happens on register successful.
                    /// </summary>
                    public event Func<Task> OnRegistered;

                    /// <summary>
                    ///   What happens on invalid input.
                    /// </summary>
                    public event Func<Task> OnInvalid;

                    /// <summary>
                    ///   What happens on duplicate nick/login.
                    /// </summary>
                    public event Func<Task> OnDuplicate;

                    /// <summary>
                    ///   What happens on an unexpected error.
                    /// </summary>
                    public event Func<Task> OnUnexpectedError;

                    /// <summary>
                    ///   What happens on a password mismatch.
                    /// </summary>
                    public event Func<Task> OnPasswordMismatch;
                    
                    protected override void Initialize()
                    {
                        SendRegister = MakeSender<UserBody>("Register");
                    }

                    protected override void SetIncomingMessageHandlers()
                    {
                        AddIncomingMessageHandler("Welcome", async protocol =>
                        {
                            Debug.Log("Registering");
                            await SendRegister(new UserBody()
                            {
                                Login = Login,
                                Password = Password,
                                PasswordConfirm = PasswordConfirm,
                                NickName = NickName
                            });
                        });
                        AddIncomingMessageHandler("Ok", async protocol =>
                        {
                            Debug.Log("Register OK");
                            await (OnRegistered?.InvokeAsync() ?? Task.CompletedTask);
                        });
                        AddIncomingMessageHandler("Duplicate", async protocol =>
                        {
                            Debug.Log("Register error: Duplicate");
                            await (OnDuplicate?.InvokeAsync() ?? Task.CompletedTask);
                        });
                        AddIncomingMessageHandler("PasswordMismatch", async protocol =>
                        {
                            Debug.Log("Register error: PWD Mismatch");
                            await (OnPasswordMismatch?.InvokeAsync() ?? Task.CompletedTask);
                        });
                        AddIncomingMessageHandler("Invalid", async protocol =>
                        {
                            Debug.Log("Register error: Validation errors");
                            await (OnInvalid?.InvokeAsync() ?? Task.CompletedTask);
                        });
                        AddIncomingMessageHandler("UnexpectedError", async protocol =>
                        {
                            Debug.Log("Register error: Unexpected error");
                            await (OnUnexpectedError?.InvokeAsync() ?? Task.CompletedTask);
                        });
                    }
                }
            }
        }
    }
}