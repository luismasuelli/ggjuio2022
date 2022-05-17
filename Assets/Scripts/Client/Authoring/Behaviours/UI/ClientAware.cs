using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlephVault.Unity.Meetgard.Authoring.Behaviours.Client;
using AlephVault.Unity.Support.Utils;
using GameMeanMachine.Unity.GabTab.Authoring.Behaviours;
using GameMeanMachine.Unity.GabTab.Authoring.Behaviours.Interactors;
using GGJUIO2020.Client.Authoring.Behaviours.Protocols;
using GGJUIO2020.Types.Models;
using UnityEngine;
using UnityEngine.UI;


namespace GGJUIO2020.Client
{
    namespace Authoring
    {
        namespace Behaviours
        {
            namespace UI
            {
                public class ClientAware : MonoBehaviour
                {
                    [SerializeField]
                    private RegisterProtocolClientSide registerProtocol;

                    [SerializeField]
                    private LoginProtocolClientSide loginProtocol;

                    [SerializeField]
                    private MainProtocolClientSide mainProtocol;

                    private NetworkClient registerClient;
                    private NetworkClient loginClient;

                    private GameObject welcomePanel;
                    private GameObject logoutPanel;

                    private Button registerButton;
                    private Button loginButton;
                    private Button logoutButton;

                    private InputField loginLogin;
                    private InputField loginPassword;

                    private InputField registerLogin;
                    private InputField registerPassword;
                    private InputField registerPasswordConfirm;
                    private InputField registerNickName;

                    private InteractiveInterface interactiveInterface;
                    private Queue<Func<InteractorsManager, InteractiveMessage, Task>> interactions;
                        
                    // Start is called before the first frame update
                    private void Awake()
                    {
                        welcomePanel = transform.Find("WelcomePanel").gameObject;
                        loginLogin = transform.Find("WelcomePanel/Login_Login").GetComponent<InputField>();
                        loginPassword = transform.Find("WelcomePanel/Login_Password").GetComponent<InputField>();
                        loginButton = transform.Find("WelcomePanel/LoginButton").GetComponent<Button>();
                        registerLogin = transform.Find("WelcomePanel/Register_Login").GetComponent<InputField>();
                        registerPassword = transform.Find("WelcomePanel/Register_Password").GetComponent<InputField>();
                        registerPasswordConfirm = transform.Find("WelcomePanel/Register_PasswordConfirm").GetComponent<InputField>();
                        registerNickName = transform.Find("WelcomePanel/Register_NickName").GetComponent<InputField>();
                        registerButton = transform.Find("WelcomePanel/RegisterButton").GetComponent<Button>();
                        logoutPanel = transform.Find("LogoutPanel").gameObject;
                        logoutButton = transform.Find("LogoutPanel/Logout").GetComponent<Button>();
                        interactiveInterface = transform.Find("InteractionPanel").GetComponent<InteractiveInterface>();
                        interactions = new Queue<Func<InteractorsManager, InteractiveMessage, Task>>();
                        registerClient = registerProtocol.GetComponent<NetworkClient>();
                        loginClient = loginProtocol.GetComponent<NetworkClient>();
                    }

                    private void Start()
                    {
                        OpenWelcomeCanvas();
                        loginButton.onClick.AddListener(OnLoginClick);
                        logoutButton.onClick.AddListener(OnLogoutClick);
                        registerButton.onClick.AddListener(OnRegisterClick);
                        loginProtocol.OnLoggedOut += async () =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor)manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder().Clear().Write(
                                    $"Sesión cerrada exitosamente"
                                ).Wait().End());
                            });
                            OpenWelcomeCanvas();
                        };
                        loginProtocol.OnKicked += async (r) =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor)manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder().Clear().Write(
                                    $"Te desconectaron por la siguiente razón: {r.Reason}"
                                ).Wait().End());
                            });
                            OpenWelcomeCanvas();
                        };
                        loginProtocol.OnLoginOK += async (ok) =>
                        {
                            OpenLogoutCanvas();
                        };
                        loginProtocol.OnTimeout += async () =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor)manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder().Clear().Write(
                                    $"Se terminó el tiempo para el login - intenta de nuevo"
                                ).Wait().End());
                            });
                            OpenWelcomeCanvas();
                        };
                        loginProtocol.OnLoginFailed += async (e) =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor)manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder().Clear().Write(
                                    $"Imposible conectarse: {e.Reason}"
                                ).Wait().End());
                            });
                            OpenWelcomeCanvas();
                        };
                        registerProtocol.OnRegistered += async () =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor) manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder()
                                    .Clear().Write($"Registro exitoso. Hora de loguearte!").Wait().End());
                            });
                            OpenWelcomeCanvas();
                        };
                        registerProtocol.OnDuplicate += async () =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor) manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder()
                                    .Clear().Write($"Ya existe ese nick o usuario").Wait().End());
                            });
                            OpenWelcomeCanvas();
                        };
                        registerProtocol.OnInvalid += async () =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor) manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder()
                                    .Clear().Write($"Datos inválidos").Wait().End());
                            });
                            
                            OpenWelcomeCanvas();
                        };
                        registerProtocol.OnPasswordMismatch += async () =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor) manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder()
                                    .Clear().Write($"Los passwords no encajan").Wait().End());
                            });
                            OpenWelcomeCanvas();
                        };
                        registerProtocol.OnUnexpectedError += async () =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor) manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder()
                                    .Clear().Write($"Hubo un error interno!!!!!!").Wait().End());
                            });
                            OpenWelcomeCanvas();
                        };
                        mainProtocol.CurrentMission += async (provinceIndex, questionType) =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor) manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder()
                                    .Clear().Write(ProvinceData.Data[provinceIndex].CurrentMission(questionType)).Wait().End());
                            });
                        };
                        mainProtocol.AlreadyComplete += async () =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor) manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder()
                                    .Clear().Write("Ya estás completo en todas tus misiones :)").Wait().End());
                            });
                        };
                        mainProtocol.Info += async provinceIndex =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor) manager["null"];
                                ButtonsInteractor buttonsInteractor = (ButtonsInteractor) manager["informant"];
                                await buttonsInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder()
                                    .Clear().Write($"Bienvenido a la provincia de {ProvinceData.Data[provinceIndex].Name}. " +
                                                   $"Hay algo que te gustaría aprender de aquí?").Wait().End());
                                while (true)
                                {
                                    bool saidNo = false;
                                    switch (buttonsInteractor.Result)
                                    {
                                        case "no":
                                            saidNo = true;
                                            await nullInteractor.RunInteraction(message,
                                                new InteractiveMessage.PromptBuilder()
                                                    .Clear().Write($"Vuelve cuando quieras!").Wait().End());
                                            break;
                                        case "cuisine":
                                        case "regional":
                                        case "culture":
                                            await nullInteractor.RunInteraction(message,
                                                new InteractiveMessage.PromptBuilder()
                                                    .Clear().Write(ProvinceData.Data[provinceIndex].Info(buttonsInteractor.Result)).Wait().End());
                                            await buttonsInteractor.RunInteraction(message,
                                                new InteractiveMessage.PromptBuilder()
                                                    .Clear().Write($"Te interesaria saber algo mas?").Wait().End());
                                            break;
                                    }
                                    if (saidNo) break;
                                }
                            });
                        };
                        mainProtocol.StepComplete += async provinceIndex =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor) manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder()
                                    .Clear().Write("Felicidades! Encontraste el lugar correcto!. Ve por tu próxima misión").Wait().End());
                            });
                        };
                        mainProtocol.YouJustCompleted += async () =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor) manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder()
                                    .Clear().Write("Felicidades! Acabas de completar todas tus misiones!").Wait().End());
                            });
                        };
                        mainProtocol.TheyJustCompleted += async (nickname) =>
                        {
                            QueueInteraction(async (manager, message) =>
                            {
                                NullInteractor nullInteractor = (NullInteractor) manager["null"];
                                await nullInteractor.RunInteraction(message, new InteractiveMessage.PromptBuilder()
                                    .Clear().Write($"{nickname} acaba de completar todas tus misiones!").Wait().End());
                            });
                        };
                        RunInteractions();
                    }
                    
                    private void OnRegisterClick()
                    {
                        registerProtocol.Login = registerLogin.text;
                        registerProtocol.Password = registerPassword.text;
                        registerProtocol.PasswordConfirm = registerPasswordConfirm.text;
                        registerProtocol.NickName = registerNickName.text;
                        registerClient.Connect("localhost", 16666);
                    }

                    private async void OnLoginClick()
                    {
                        loginProtocol.Login = loginLogin.text;
                        loginProtocol.Password = loginPassword.text;
                        loginClient.Connect("localhost", 16667);
                    }

                    private async void RunInteractions()
                    {
                        while (true)
                        {
                            if (!gameObject) break;

                            while (interactions.Count > 0)
                            {
                                await interactiveInterface.RunInteraction(interactions.Dequeue());
                            }

                            await Tasks.Blink();
                        }
                    }

                    public void QueueInteraction(Func<InteractorsManager, InteractiveMessage, Task> interaction)
                    {
                        interactions.Enqueue(interaction);
                    }

                    private async void OnLogoutClick()
                    {
                        await loginProtocol.Logout();
                        OpenWelcomeCanvas();
                    }

                    private void OpenWelcomeCanvas()
                    {
                        if (registerClient.IsConnected) registerClient.Close();
                        if (loginClient.IsConnected) loginClient.Close();
                        welcomePanel.SetActive(true);
                        logoutPanel.SetActive(false);
                    }

                    private void OpenLogoutCanvas()
                    {
                        if (registerClient.IsConnected) registerClient.Close();
                        welcomePanel.SetActive(false);
                        logoutPanel.SetActive(true);
                    }
                }
            }
        }
    }
}
