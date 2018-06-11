using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System;
using Enums;

public class CanvasBase : MonoBehaviour {

    public class CameraPos
    {
        public Vector3 leftPos = new Vector3(-56, 0, -500);
        public Vector3 midPos = new Vector3(0, 0, -500);
        public Vector3 rightPos = new Vector3(138, 0, -500);
    }

    public Material dissolve_material;

    StatsWindow stats_window = new StatsWindow();

    Transform start_menu;
    Transform equipment;
    Transform battleIA;
    Transform treasures;

    Text goldText;
    Text gold_VIP;
    EquipMenu equip_menu;

    Camera camara;
    bool camMovementEnabled = true;

    GameObject sub_menu_play;

    

    CameraPos camPosition = new CameraPos();
    Vector3 camVectorPoint;

    public static CanvasBase instance;

    void Awake()
    {
        instance = this;
        camara = Camera.main;
        camVectorPoint = camPosition.midPos;
    }

    void Start()
    {
        Inicialize();

        if(Application.platform == RuntimePlatform.Android)
        {
            ConectarseGooglePlay();
        }
        else
        {
            LogFirebaseTEST();
        }

    }

    void Inicialize()
    {
        sub_menu_play = transform.Find("Start_Menu").Find("SubPanel_Versus").gameObject;
        start_menu = transform.Find("Start_Menu");
        equipment = transform.Find("Equipamiento");
        battleIA = transform.Find("BattleIA");
        treasures = transform.Find("Treasures");
        goldText = transform.Find("Treasures").Find("Cofres").Find("Money").Find("Text").GetComponent<Text>();
        gold_VIP = transform.Find("Treasures").Find("Cofres").Find("Money_VIP").Find("Text").GetComponent<Text>();
        equip_menu = equipment.GetComponent<EquipMenu>();

        Transform statsT = transform.Find("Equipamiento").Find("STATS");
        stats_window.alpha = statsT.Find("Value_Alpha").GetComponent<Text>();
        stats_window.assassin = statsT.Find("Value_Assassin").GetComponent<Text>();
        stats_window.charming = statsT.Find("Value_Charming").GetComponent<Text>();
        stats_window.pacifist = statsT.Find("Value_Pacifist").GetComponent<Text>();

        stats_window.health = statsT.Find("Value_Life").GetComponent<Text>();
        stats_window.strenght = statsT.Find("Value_Attack").GetComponent<Text>();
        stats_window.skill = statsT.Find("Value_Skill").GetComponent<Text>();
        stats_window.luck = statsT.Find("Value_Luck").GetComponent<Text>();
    }

    public void StatsRefresh()
    {
        BaseStats totalStats = Items.instance.CalcularTotalBasePoints();
        stats_window.assassin.text = totalStats.assassin.ToString();
        stats_window.alpha.text = totalStats.alpha.ToString();
        stats_window.charming.text = totalStats.charming.ToString();
        stats_window.pacifist.text = totalStats.pacifist.ToString();

        stats_window.strenght.text = totalStats.strenght.ToString();
        stats_window.health.text = totalStats.health.ToString();
        stats_window.skill.text = totalStats.skill.ToString();
        stats_window.luck.text = totalStats.luck.ToString();
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

    public void UpdateGoldView()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("data").Child("gold").GetValueAsync()
            .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snap = task.Result;
                    goldText.text = snap.Value.ToString();
                }
            });
        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("data").Child("gold_VIP").GetValueAsync()
            .ContinueWith(task2 => {
                if (task2.IsCompleted)
                {
                    DataSnapshot snap = task2.Result;
                    gold_VIP.text = snap.Value.ToString();
                }
            });

    }

    public void UpdateGoldViewNoDB(string value)
    {
        goldText.text = value;
    }

    public void ShowItemInfo(string id)
    {
        StartCoroutine(equip_menu.ViewItemInfo(id));
    }

    void LogFirebaseTEST()
    {
        string username = Social.localUser.id + "@evolution.com";
        string password = "ANonSecurePassword!ñ";
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(username, password).ContinueWith((obj) =>
        {
            if (obj.IsFaulted)
            {
                FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance).CreateUserWithEmailAndPasswordAsync(username, password).ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        CargarUsuario();
                    }

                    if (task.IsFaulted)
                    {
                        print("Error al intentar conectar con la base de datos");
                    }

                });
            }
            else
            {
                CargarUsuario();
            }
        });

    }

    void CargarUsuario()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child("Inventario").Child(Social.localUser.id).GetValueAsync().ContinueWith(task => {
            if (!task.Result.Exists) //Si es la primera vez que juegas
            {
                start_menu.gameObject.SetActive(false);
                transform.Find("Loading").gameObject.SetActive(false);
                camMovementEnabled = false;
                SceneManager.LoadScene(1);
            }
            else
            {
                LogSuccessful();
                print("Todo cargado correctamente");
            }
        });
    }

    bool IsUserLogged()
    {
        return FirebaseAuth.DefaultInstance.CurrentUser != null;
    }

    public void GoBattle()
    {
#if UNITY_EDITOR
        battleIA.gameObject.SetActive(!battleIA.gameObject.activeSelf);
        start_menu.gameObject.SetActive(false);
#endif

        if (Social.localUser.authenticated)
        {
            battleIA.gameObject.SetActive(!battleIA.gameObject.activeSelf);
            start_menu.gameObject.SetActive(false);
        }
        else
        {
            Message.instance.NewMessage("No hay conexión");
        }
    }

    public void BTN_VERSUS()
    {
        //GoBattle();
        sub_menu_play.SetActive(!sub_menu_play.activeSelf);
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
        camVectorPoint = camPosition.rightPos;
    }

    public void BTN_OK_Equipment()
    {
        Database.instance.GuardarEquipSetting(GameManager.instance.player.criatura.equipment);
        equipment.gameObject.SetActive(false);
        start_menu.gameObject.SetActive(true);
        camVectorPoint = camPosition.midPos;
    }

    public void BTN_EQUIP()
    {
        StatsRefresh();
        equipment.gameObject.SetActive(true);
        start_menu.gameObject.SetActive(false);
        camVectorPoint = camPosition.leftPos;
    }

    public void BackToMenu()
    {
        start_menu.gameObject.SetActive(true);
        camVectorPoint = camPosition.midPos;
    }

    private void TryRelog(object sender, EventArgs e)
    {
        FirebaseUser fireUser = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance).CurrentUser;
        if (fireUser == null)
        {
            print("TRYING TO RECONECT DB");
            LogFirebaseTEST();
        }
    }

    public void CreateWaitingRoom()
    {

        FirebaseDatabase.DefaultInstance.RootReference.Child("Private Rooms").GetValueAsync().ContinueWith(task => { //Leer cuantas salas hay
            if (task.IsCompleted)
            {
                WaitingRoom room = new WaitingRoom()
                {
                    ID = "" + task.Result.ChildrenCount,
                    owner = Social.localUser.userName,
                    guest = "",
                    status = "waiting"
                };
                FirebaseDatabase.DefaultInstance.RootReference.Child("Private Rooms").Child(room.ID).SetRawJsonValueAsync(JsonUtility.ToJson(room)).ContinueWith(task2 =>
                {
                    if (task2.IsCompleted)
                    {
                        print("Completo");
                    }
                });
            }
        });
    }

    public void LoadYourItems()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("items");
        reference.GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                string jsonData = task.Result.GetRawJsonValue();
                string[] items = jsonData.Replace('"', ' ').Replace('[', ' ').Replace(']', ' ').Replace(" ", string.Empty).Split(',');
                Items.instance.StoreClear();
                foreach (string s in items)
                {
                    Items.instance.StoreItem(s);
                }
            }
        });
    }

    public IEnumerator MostrarJugador() //EFECTO DISOLVER
    {
        dissolve_material.SetFloat("_Level", 1f);
        Menu.instance.SetMaterialVisor(Menu.instance.GetPlayerVisor(1), dissolve_material);
        yield return StartCoroutine(DisolverAnim());
        Menu.instance.SetMaterialVisor(Menu.instance.GetPlayerVisor(1), null);
        dissolve_material.SetFloat("_Level", 1f);

    }

    public IEnumerator MostrarPieza(Equip_Position posi, Action<bool> ended)
    {
        dissolve_material.SetFloat("_Level", 1f);
        switch (posi)
        {
            case Equip_Position.Head: Menu.instance.visor_player1.headgear.material = dissolve_material; break;
            case Equip_Position.Body: Menu.instance.visor_player1.body.material = dissolve_material; break;
            case Equip_Position.Arms: Menu.instance.visor_player1.arm_left.material = dissolve_material;
                                        Menu.instance.visor_player1.arm_right.material = dissolve_material; break;
            case Equip_Position.Legs: Menu.instance.visor_player1.leg_left.material = dissolve_material;
                                            Menu.instance.visor_player1.leg_right.material = dissolve_material; break;
        }
        yield return StartCoroutine(DisolverAnim());
        switch (posi)
        {
            case Equip_Position.Head: Menu.instance.visor_player1.headgear.material = null; break;
            case Equip_Position.Body: Menu.instance.visor_player1.body.material = null; break;
            case Equip_Position.Arms:
                Menu.instance.visor_player1.arm_left.material = null;
                Menu.instance.visor_player1.arm_right.material = null; break;
            case Equip_Position.Legs:
                Menu.instance.visor_player1.leg_left.material = null;
                Menu.instance.visor_player1.leg_right.material = null; break;
        }
        ended(true);

    }

    IEnumerator DisolverAnim()
    {
        float t = 1.0f;
        while (t > 0.01f)
        {
            dissolve_material.SetFloat("_Level", t);
            t -= Time.deltaTime;
            t = Mathf.Clamp(t, 0f, 1f);
            yield return null;
        }
    }

    void Update()
    {
        if (camMovementEnabled) camara.transform.position = Vector3.Lerp(camara.transform.position, camVectorPoint, Time.deltaTime * 10);

        if (Input.GetKeyDown(KeyCode.J)) StartCoroutine(MostrarPieza(Equip_Position.Body, ended => { if (ended) print("ENDED"); }));
    }


    void LogSuccessful()
    {
        Database.instance.ObtenerEquipSetting();
        transform.Find("Loading").gameObject.SetActive(false);
        UpdateGoldView();
        LoadYourItems();
        //CreateWaitingRoom();
        FirebaseAuth.DefaultInstance.StateChanged += TryRelog;
        
    }

}
