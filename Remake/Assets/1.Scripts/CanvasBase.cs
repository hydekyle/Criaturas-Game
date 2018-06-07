﻿using System.Collections;
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
using UnityEngine.SceneManagement;
using System;

public class CanvasBase : MonoBehaviour {

    Transform start_menu;
    Transform equipment;
    Transform battleIA;
    Transform treasures;

    Text goldText;
    Text gold_VIP;
    EquipMenu equip_menu;

    Camera camara;
    bool camMovementEnabled = true;

    public class CameraPos {
        public Vector3 leftPos = new Vector3(-34, 0, -500);
        public Vector3 midPos = new Vector3(0, 0, -500);
        public Vector3 rightPos = new Vector3(138, 0, -500);
    }

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
        start_menu = transform.Find("Start_Menu");
        equipment = transform.Find("Equipamiento");
        battleIA = transform.Find("BattleIA");
        treasures = transform.Find("Treasures");
        goldText = transform.Find("Treasures").Find("Cofres").Find("Money").Find("Text").GetComponent<Text>();
        gold_VIP = transform.Find("Treasures").Find("Cofres").Find("Money_VIP").Find("Text").GetComponent<Text>();
        equip_menu = equipment.GetComponent<EquipMenu>();

        if(Application.platform == RuntimePlatform.Android)
        {
            ConectarseGooglePlay();
        }
        else
        {
            LogFirebaseTEST();
        }
        

    }

    void Update()
    {
        if (camMovementEnabled) camara.transform.position = Vector3.Lerp(camara.transform.position, camVectorPoint, Time.deltaTime * 10);
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


    public void CheckFirebaseLogin()
    {
        FirebaseUser fireUser = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance).CurrentUser;
        if(fireUser == null)
        {
            LogFirebaseTEST();
        }
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
        }
        else
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
        equipment.gameObject.SetActive(true);
        start_menu.gameObject.SetActive(false);
        camVectorPoint = camPosition.leftPos;
    }

    public void BackToMenu()
    {
        start_menu.gameObject.SetActive(true);
        camVectorPoint = camPosition.midPos;
    }


    void LogSuccessful()
    {
        Database.instance.ObtenerEquipSetting();
        transform.Find("Loading").gameObject.SetActive(false);
        UpdateGoldView();
        LoadYourItems();
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

}
