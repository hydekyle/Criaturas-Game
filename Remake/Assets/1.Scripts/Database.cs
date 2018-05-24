using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



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
        string json = JsonUtility.ToJson(equipment);
        File.WriteAllText(Application.persistentDataPath+"/Equip_Setting.txt", json);
    }

    public Equipment ObtenerEquipSetting()
    {
        Equipment e = new Equipment();
        e = JsonUtility.FromJson<Equipment>(File.ReadAllText(Application.persistentDataPath + "/Equip_Setting.txt"));
        return e;
    }

    void OnApplicationQuit()
    {
        GuardarEquipSetting(GameManager.instance.player.criatura.equipment);
    }
#endregion
}
