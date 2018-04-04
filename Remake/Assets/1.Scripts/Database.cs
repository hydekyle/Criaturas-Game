using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Database : MonoBehaviour {

    public string[] Items;
    public static Database instance { get; set; }
    void Awake()
    {
        instance = this;
        CheckGameFolders();
    }

    IEnumerator Start()
    {
        WWW itemsData = new WWW("http://localhost/CoolGame/users.php");
        yield return itemsData;
        string itemsDataString = itemsData.text;
        //print(itemsDataString);
        Items = itemsData.text.Split(';');
        //print(GetDataValue(Items[0], "Name:"));
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }


    //ARCHIVOS LOCALES
    void CheckGameFolders()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Headgear")) CrearDirectorios();
    }
    void CrearDirectorios()
    {
        Directory.CreateDirectory(Application.persistentDataPath + "/Headgear");
        Directory.CreateDirectory(Application.persistentDataPath + "/Bodies");
        Directory.CreateDirectory(Application.persistentDataPath + "/Arms");
        Directory.CreateDirectory(Application.persistentDataPath + "/Legs");
        Directory.CreateDirectory(Application.persistentDataPath + "/Backs");
        Directory.CreateDirectory(Application.persistentDataPath + "/Weapons");
    }

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

}
