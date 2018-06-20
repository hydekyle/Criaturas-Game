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
        public Vector3 leftPos = new Vector3(-135, 0, -500);
        public Vector3 midPos = new Vector3(0, 0, -500);
        public Vector3 rightPos = new Vector3(224, -125, -500);
        public Vector3 upPos = new Vector3(185, 142, -500);
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

    CameraPos camPosition = new CameraPos();
    Camera camara;
    bool camMovementEnabled = true;
    public float cam_velocity = 10f;
    public float cam_size = 300;
    public Vector3 camVectorPoint;

    GameObject sub_menu_play;
    
    public static CanvasBase instance;

    void Awake()
    {
        instance = this;
        camara = Camera.main;
        camVectorPoint = camPosition.midPos;
        SetCamSize(300);
    }

    public void BTN_TIENDA()
    {
        transform.Find("Tienda").gameObject.SetActive(true);
        transform.Find("Treasures").gameObject.SetActive(false);
        camVectorPoint = camPosition.upPos;
        SetCamSize(150);
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
        //FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("data").GetValueAsync().ContinueWith(task => {
        //    if (task.IsCompleted)
        //    {
        //        string json = task.Result.GetRawJsonValue();
        //        UserDB user = JsonUtility.FromJson<UserDB>(json);
        //        goldText.text = user.gold.ToString();
        //        gold_VIP.text = user.gold_VIP.ToString();
        //    }
        //});
        goldText.text = GameManager.instance.userdb.gold.ToString();
        gold_VIP.text = GameManager.instance.userdb.gold_VIP.ToString();
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

        reference.Child("Inventario").Child(Social.localUser.id).Child("data").ChildChanged += OnUserDataChanged; //Listener para userdata changed

        reference.Child("Inventario").Child(Social.localUser.id).GetValueAsync().ContinueWith(task => {
            if (task.Result.Exists) //Si ya has jugado
            {
                string json = task.Result.Child("data").GetRawJsonValue();
                UserDB user = JsonUtility.FromJson<UserDB>(json);
                GameManager.instance.userdb = user;
                LogSuccessful();
            }
            else 
            {
                start_menu.gameObject.SetActive(false);
                transform.Find("Loading").gameObject.SetActive(false);
                camMovementEnabled = false;
                SceneManager.LoadScene(1);

            }
        });
    }

    private void OnUserDataChanged(object sender, ChildChangedEventArgs e)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("data").GetValueAsync().ContinueWith(task => {
            string json = task.Result.GetRawJsonValue();
            GameManager.instance.userdb = JsonUtility.FromJson<UserDB>(json);
        });
        print("Cambio: " + e.Snapshot);
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
        SetCamSize(150);
    }

    public void BTN_OK_Equipment()
    {
        Database.instance.GuardarEquipSetting(GameManager.instance.player.criatura.equipment);
        equipment.gameObject.SetActive(false);
        start_menu.gameObject.SetActive(true);
        camVectorPoint = camPosition.midPos;
        SetCamSize(300);
    }

    public void BTN_EQUIP()
    {
        StatsRefresh();
        equipment.gameObject.SetActive(true);
        start_menu.gameObject.SetActive(false);
        camVectorPoint = camPosition.leftPos;
        SetCamSize(400);
    }

    public void BackToMenu()
    {
        start_menu.gameObject.SetActive(true);
        camVectorPoint = camPosition.midPos;
        SetCamSize(300);
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

    void LogSuccessful()
    {
        Database.instance.ObtenerEquipSetting();
        UpdateGoldView();
        LoadYourItems();
        //CreateWaitingRoom();
        FirebaseAuth.DefaultInstance.StateChanged += TryRelog;
        Message.instance.MostrarCofresVIP();
        StartCoroutine(LagSpikeMenuFixer(onEnded => transform.Find("Loading").gameObject.SetActive(false)));

        
    }

    IEnumerator LagSpikeMenuFixer(Action<bool> onEnded)
    {
        treasures.gameObject.SetActive(true);
        equipment.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        treasures.gameObject.SetActive(false);
        equipment.gameObject.SetActive(false);
        onEnded(true);

    }

    void Update()
    {
        if (camMovementEnabled)
        {
            camara.transform.position = Vector3.Lerp(camara.transform.position, camVectorPoint, Time.deltaTime * cam_velocity);
            camara.orthographicSize = Mathf.Lerp(camara.orthographicSize, cam_size, Time.deltaTime * cam_velocity);
            //if (Input.GetKeyDown(KeyCode.I)) StartCoroutine(C_Inspeccionar(1));
            //if (Input.GetKeyDown(KeyCode.O)) StartCoroutine(C_Inspeccionar(2));
            if (Input.GetKeyDown(KeyCode.K)) SetCamTarget((Posi(Equip_Position.Body, 2) + Posi(Equip_Position.Body, 1)) / 2);
            if (Input.GetKeyDown(KeyCode.L)) SetCamTarget(Posi(Equip_Position.Legs, 2));

            if (Input.GetKeyDown(KeyCode.N)) SetCamVelocity(Velocity.Slow);
            if (Input.GetKeyDown(KeyCode.M)) SetCamVelocity(Velocity.Fast);
        }
    }

    void SetCamSize(float newSize)
    {
        cam_size = newSize;
    }

    void SetCamVelocity(float newVelocity)
    {
        cam_velocity = newVelocity;
    }

    void SetCamVelocity(Velocity vel)
    {
        switch (vel)
        {
            case Velocity.Fast: cam_velocity = 8f; break;
            case Velocity.Normal: cam_velocity = 5f; break;
            case Velocity.Slow: cam_velocity = 1.5f; break;
        }
    }

    public void SetCamTarget(Vector3 targetPOS)
    {
        camVectorPoint = new Vector3(targetPOS.x, targetPOS.y, -500);
    }

    public IEnumerator C_Inspeccionar(int playerN, Action<bool> onEnded)
    {
        SetCamVelocity(Velocity.Normal);
        SetCamSize(300);
        SetCamTarget(Posi(Equip_Position.Legs, playerN));
        while (!CamIsOnTarget()) yield return null;
        SetCamVelocity(Velocity.Slow);
        SetCamSize(120);
        SetCamTarget((Posi(Equip_Position.Body, playerN) + Posi(Equip_Position.Head, playerN)) / 2);
        while (!CamIsOnTarget()) yield return null;
        onEnded(true);
    }

    Vector3 Posi(Equip_Position equip_position, int playerN)
    {
        Vector3 v = Vector3.zero;

        switch (equip_position)
        {
            case Equip_Position.Head: v = Menu.instance.GetPlayerVisor(playerN).headgear.transform.position; break;
            case Equip_Position.Body: v = Menu.instance.GetPlayerVisor(playerN).body.transform.position; break;
            case Equip_Position.Arms: v = (Menu.instance.GetPlayerVisor(playerN).arm_right.transform.position + Menu.instance.GetPlayerVisor(playerN).arm_left.transform.position) /2; break;
            case Equip_Position.Legs: v = (Menu.instance.GetPlayerVisor(playerN).leg_right.transform.position + Menu.instance.GetPlayerVisor(playerN).leg_left.transform.position)/2; break;
            default: v = Menu.instance.GetPlayerVisor(playerN).body.transform.position; break;
        }
        return v;
    }

    public IEnumerator PointReached(Action<bool> onEnded)
    {
        while (!CamIsOnTarget())
        {
            yield return new WaitForSeconds(0.1f);
        }
        onEnded(true);
    }

    bool CamIsOnTarget()
    {
        return Mathf.Approximately((int)camara.transform.position.x, (int)camVectorPoint.x) && Mathf.Approximately((int)camara.transform.position.y, (int)camVectorPoint.y);
    }

}
