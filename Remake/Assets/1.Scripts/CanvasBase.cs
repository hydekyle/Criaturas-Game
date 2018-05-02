using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;

public class CanvasBase : MonoBehaviour {

    Transform start_menu;
    Transform equipment;
    Transform battleIA;

    bool GP_isConnected;
    Invitation invi;
    static InvitationReceivedDelegate invitationDelegate;
    static MatchDelegate matchDelegate;

    void Start()
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
        Conectarse();
    }

    void Conectarse()
    {
        Social.Active.Authenticate(Social.localUser, (bool success) => {
            if (success) Message.instance.NewMessage("Hola " + Social.localUser.userName); else Message.instance.NewMessage("No conectado");
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
        Social.ShowAchievementsUI();
        if (Social.localUser.authenticated)
        {
            
        }
    }

    public void BTN_OK_Equipment()
    {
        Database.instance.GuardarEquipSetting(GameManager.instance.player.criatura.equipment);
        equipment.gameObject.SetActive(false);
        start_menu.gameObject.SetActive(true);
    }
}
