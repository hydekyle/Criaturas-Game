using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using Firebase.Auth;

public class CanvasBase : MonoBehaviour {

    Transform start_menu;
    Transform equipment;
    Transform battleIA;

    bool GP_isConnected;
    Invitation invi;
    static InvitationReceivedDelegate invitationDelegate;
    static MatchDelegate matchDelegate;

    public class objetos { public List<string> items; public string nombre; }

    void Awake()
    {
        /*invitationDelegate = RecibirInvitacion;
        matchDelegate = RandomMatch;
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .EnableSavedGames()
        .WithInvitationDelegate(invitationDelegate)
        .WithMatchDelegate(matchDelegate)
        .RequestServerAuthCode(false)
        .Build();
        PlayGamesPlatform.InitializeInstance(config);*/
        PlayGamesPlatform.Activate();
        start_menu = transform.Find("Start_Menu");
        equipment = transform.Find("Equipamiento");
        battleIA = transform.Find("BattleIA");
        ConectarseGooglePlay();
    }

    void LogFirebaseTEST()
    {
        Message.instance.NewMessage("Intentandolo");
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync("hydekyle706@gmail.com", "adrian").ContinueWith((obj) =>
        {
            Message.instance.NewMessage("Conectado");
            List<string> temp = new List<string>();
            temp.Add("1111111111");
            temp.Add("2222222222");

            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

            objetos userInfo = new objetos() { items = temp, nombre = "Ayoze from Android" };
            string json = JsonUtility.ToJson(userInfo);

            reference.Child("Inventario").Child(Social.localUser.id).SetRawJsonValueAsync(json).ContinueWith((obj2) =>
            {
                if (obj2.IsCompleted)
                {
                    Message.instance.NewMessage("Info Db completed");
                }

            });
        });

    }

    bool IsUserLogged()
    {
        return FirebaseAuth.DefaultInstance.CurrentUser != null;
    }


    void ConectarseGooglePlay()
    {
        Social.Active.Authenticate(Social.localUser, (bool success) => {
            if (success) Message.instance.NewMessage("Hola " + Social.localUser.userName); else Message.instance.NewMessage("No conectado");
            LogFirebaseTEST();
        });
    }

    public void RandomMatch(TurnBasedMatch match, bool autoJoin)
    {
        Message.instance.NewMessage("Toca jugar");
    }

    public void RecibirInvitacion(Invitation invitation, bool autoAcept)
    {
        Message.instance.NewMessage("Hola, "+invitation.Inviter);
    }

    public void BTN_EQUIP()
    {
        equipment.gameObject.SetActive(true);
        start_menu.gameObject.SetActive(false);
    }

	public void BTN_VERSUS()
    {
#if UNITY_EDITOR
        battleIA.gameObject.SetActive(!battleIA.gameObject.activeSelf);
        start_menu.gameObject.SetActive(false);
#endif

        if (Social.localUser.authenticated)
        {
            battleIA.gameObject.SetActive(!battleIA.gameObject.activeSelf);
            start_menu.gameObject.SetActive(false);
        }else
        {
            Message.instance.NewMessage("No hay conexión");
        }
    }

    public void BTN_LOGROS()
    {
        //Social.ShowAchievementsUI();
        
        if (Social.localUser.authenticated)
        {
            LogFirebaseTEST();
        }
    }

    public void BTN_OK_Equipment()
    {
        Database.instance.GuardarEquipSetting(GameManager.instance.player.criatura.equipment);
        equipment.gameObject.SetActive(false);
        start_menu.gameObject.SetActive(true);
    }
}
