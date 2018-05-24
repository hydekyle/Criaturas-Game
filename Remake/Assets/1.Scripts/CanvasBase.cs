using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using Firebase.Auth;

public class CanvasBase : MonoBehaviour {

    Transform start_menu;
    Transform equipment;
    Transform battleIA;
    Transform treasures;

    Text goldText;
    EquipMenu equip_menu;

    public static CanvasBase instance;

    

    void Awake()
    {
        instance = this;
        
    }

    void Start()
    {
        start_menu = transform.Find("Start_Menu");
        equipment = transform.Find("Equipamiento");
        battleIA = transform.Find("BattleIA");
        treasures = transform.Find("Treasures");
        goldText = transform.Find("UI").Find("Money").Find("Text").GetComponent<Text>();
        equip_menu = equipment.GetComponent<EquipMenu>();

        ConectarseGooglePlay();

    }

    void ConectarseGooglePlay()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        
        Social.Active.Authenticate(Social.localUser, (bool success) => {
            if (success) {
                Message.instance.NewMessage("Hola " + Social.localUser.userName);
                LogFirebaseTEST();
            }else
            {
                Message.instance.NewMessage("Error del bueno");
            }
        });
        
    }

    void UpdateGoldView()
    {
        goldText.text = FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.userName).Child("dataUser").Child("username").Key;
    }

    public void BackToMenu()
    {
        start_menu.gameObject.SetActive(true);
    }

    public void ShowItemInfo(string id)
    {
        StartCoroutine(equip_menu.ViewItemInfo(id));
    }

    void LogFirebaseTEST()
    {
        Message.instance.NewMessage("Intentandolo");
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync("hydekyle706@gmail.com", "adrian").ContinueWith((obj) =>
        {
            Message.instance.NewMessage("Conectado");
            List<string> temp = new List<string>();
            temp.Add(Items.instance.GetRandomItemID());
            temp.Add(Items.instance.GetRandomItemID());
            temp.Add(Items.instance.GetRandomItemID());

            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            UserDB userData = new UserDB()
            {
                username = Social.localUser.userName,
                chests = 3,
                gold = 666,
                victorias = 0,
                derrotas = 0
            };
            objetos userInfo = new objetos() { items = temp, username = userData.username, gold = userData.gold, chests = userData.chests };
            string json = JsonUtility.ToJson(userInfo);

            reference.Child("Inventario").Child(Social.localUser.userName).SetRawJsonValueAsync(json).ContinueWith((obj2) =>
            {
                if (obj2.IsCompleted)
                {
                    Message.instance.NewMessage("Info Db completed");
                    UpdateGoldView();
                }

            });
        });

    }

    bool IsUserLogged()
    {
        return FirebaseAuth.DefaultInstance.CurrentUser != null;
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

    public void BTN_COFRES()
    {
        treasures.gameObject.SetActive(true);
        start_menu.gameObject.SetActive(false);
    }

    public void BTN_OK_Equipment()
    {
        Database.instance.GuardarEquipSetting(GameManager.instance.player.criatura.equipment);
        equipment.gameObject.SetActive(false);
        start_menu.gameObject.SetActive(true);
    }

}
