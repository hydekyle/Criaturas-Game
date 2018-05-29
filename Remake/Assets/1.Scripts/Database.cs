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
        FirebaseDatabase.DefaultInstance.RootReference.Child("Equipamiento").Child(Social.localUser.userName).SetRawJsonValueAsync(json);
    }



    public Equipment ObtenerEquipSetting()
    {
        Equipment e = new Equipment();
        EquipDB equip = new EquipDB();
        FirebaseDatabase.DefaultInstance.RootReference.Child("Equipamiento").Child(Social.localUser.userName).GetValueAsync().ContinueWith(task => {
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
                StartCoroutine(GameManager.instance.MostrarJugador(player, 1, new Vector3(10, 66, -1), false));
            }
        });
        return e;
    }

#endregion
}
