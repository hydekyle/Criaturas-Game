using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Firebase.Database;


public class Database : MonoBehaviour {

    public string[] items;
    public static Database instance { get; set; }
    
    void Awake()
    {
        instance = this;
        StartCoroutine(GetRealTime());
    }

    public DatabaseReference ReferenceDB()
    {
        return FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id);
    }

    public IEnumerator GetRealTime()
    {
        WWW www = new WWW("http://evolution-battle.com/EvolutionPortable/GetTime.php");
        yield return www;
        if (www.isDone)
        {
            string[] info = www.text.Split('T');
            string[] fecha = info[0].Split('-');
            string[] hora = info[1].Replace("Z", string.Empty).Split(':');
            Fecha fechaActual = new Fecha()
            {
                month = int.Parse(fecha[1]),
                day = int.Parse(fecha[2]),
                hour = int.Parse(hora[0]),
                min = int.Parse(hora[1])
            };
            print(string.Format("Son las {0} y {1} minutos. Día {2}", fechaActual.hour, fechaActual.min, fechaActual.day));
            GetLastTimeReward();
        }
    }

    public void GetLastTimeReward()
    {
        ReferenceDB().Child("data").Child("last_time_reward").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                string[] info = task.Result.Value.ToString().Split('/');
                string[] time = info[2].Split(':');

                Fecha fechaLast = new Fecha()
                {
                    month = int.Parse(info[0]),
                    day = int.Parse(info[1]),
                    hour = int.Parse(time[0]),
                    min = int.Parse(time[1])
                };

                print(string.Format("Último reward fue el día {0} a las {1} y {2} minutos. Mes: {3}", fechaLast.day, fechaLast.hour, fechaLast.min, fechaLast.month));

            }
        });
    }

    public void CompareDates(Fecha actual_fecha, Fecha reward_fecha)
    {
        
    }


    #region TEMP
    public void GuardarBodyBounds(BodyBounds bounds)
    {
        if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            string json = JsonUtility.ToJson(bounds);
            File.WriteAllText(Resources.Load("BodyBounds/") + GameManager.instance.player.criatura.equipment.body.ID.ToString() + ".txt", json);
            print("Guardado BodyBounds de " + "ID: " + GameManager.instance.player.criatura.equipment.body.ID);
        }
        
    }
    public BodyBounds LeerBodyBounds(int bodyID)
    {
        BodyBounds bounds = new BodyBounds();
        string ruta = "BodyBounds/" + bodyID.ToString();
        TextAsset txt = (TextAsset)Resources.Load(ruta);
        try { bounds = JsonUtility.FromJson<BodyBounds>(txt.text); }
        catch { print("BODY BOUNDS NOT FOUND"); }
        return bounds;
    }

    public void GuardarEquipSetting(Equipment equipment)
    {
        EquipDB equip = new EquipDB()
        {
            head = equipment.head.ID_string,
            body = equipment.body.ID_string,
            arms = equipment.arms.ID_string,
            legs = equipment.legs.ID_string
        };
        string json = JsonUtility.ToJson(equip);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("equipamiento").SetRawJsonValueAsync(json);
    }



    public Equipment ObtenerEquipSetting()
    {
        Equipment e = new Equipment();
        EquipDB equip = new EquipDB();
        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("equipamiento").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                equip = JsonUtility.FromJson<EquipDB>(task.Result.GetRawJsonValue());
                e = new Equipment()
                {
                    head = Items.instance.ItemByID(equip.head),
                    body = Items.instance.ItemByID(equip.body),
                    arms = Items.instance.ItemByID(equip.arms),
                    legs = Items.instance.ItemByID(equip.legs)
                };

                Player player = new Player()
                {
                    nombre = Social.localUser.userName,
                    ID = Social.localUser.id,
                    criatura = new Criatura()
                    {
                        nombre = Social.localUser.userName,
                        attack_att = 0,
                        defense_att = 0,
                        luck_att = 0,
                        skill_att = 0,
                        equipment = e
                    }
                };

                Menu.instance.loadedEquipment = new Equipment()
                {
                    head = new Equipable_Item()
                    {
                        ID_string = e.head.ID_string
                    },
                    arms = new Equipable_Item()
                    {
                        ID_string = e.arms.ID_string
                    },
                    back = new Equipable_Item()
                    {
                        ID_string = e.back.ID_string
                    },
                    body = new Equipable_Item()
                    {
                        ID_string = e.body.ID_string
                    },
                    legs = new Equipable_Item()
                    {
                        ID_string = e.legs.ID_string
                    }
                };
                GameManager.instance.player = player;
                if(!Menu.instance.inicialized) //Visor iniciado
                {
                    StartCoroutine(GameManager.instance.MostrarJugador(player, 1, new Vector3(8, 35, -1), false));
                }
            }
        });
        return e;
    }

#endregion
}
