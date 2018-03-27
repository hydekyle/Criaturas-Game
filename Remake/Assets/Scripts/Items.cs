using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Enums;

public class Items : MonoBehaviour {

    string texturesWebFolder = "https://www.evolution-battle.com/EvolutionPortable/Texturas";


    public static Items instance;
    [SerializeField]
    public List<Equipable_Item> headgear_list;
    public List<Equipable_Item> bodies_list;
    public List<Equipable_Item> arms_list;
    public List<Equipable_Item> legs_list;
    public List<Equipable_Item> backs_list;
    public List<Equipable_Item> weapons_list;

    void Awake()
    {
        instance = this;
        CheckGameFolders();
        //TestJSON();
    }

    void ReadJSON()
    {
        Equipable_Item e1 = new Equipable_Item();
        e1 = JsonUtility.FromJson<Equipable_Item>(File.ReadAllText(Application.persistentDataPath + "/texto.txt"));
    }

    void TestJSON()
    {
        Equipable_Item e1 = new Equipable_Item() { nombre = "AyozeReaper", ID = 111, addStat = null, quality = Quality.Rare  }; headgear_list[0] = e1;
        string json = JsonUtility.ToJson(headgear_list);
        File.WriteAllText(Application.persistentDataPath + "/textoList.txt", json);
    }

    public IEnumerator SendImage(int id) {
        int listNumber = int.Parse(id.ToString().Substring(0, 1));
        Sprite spriteBuscado = null;
        yield return StartCoroutine(ItemSpriteByID(id, value => spriteBuscado = value));
        Menu.instance.SetImageVisor(spriteBuscado, listNumber);
    }

    public Equipable_Item ItemByID(int id)
    {
        Equipable_Item item = new Equipable_Item();
        int listID;
        int.TryParse(id.ToString().Substring(0, 1), out listID);
        try
        {
            switch (listID)
            {
                case 1: item = headgear_list.Find(e => e.ID == id); break;
                case 2: item = bodies_list.Find(e => e.ID == id); break;
                case 3: item = arms_list.Find(e => e.ID == id); break;
                case 4: item = legs_list.Find(e => e.ID == id); break;
                case 5: item = backs_list.Find(e => e.ID == id); break;
                case 6: item = weapons_list.Find(e => e.ID == id); break;
            }
        }
        catch { print("¡¡Item no encontrado!!"); }
        return item;
    }

    private IEnumerator ItemSpriteByID(int id, Action<Sprite> result)
    {
        int listNumber = int.Parse(id.ToString().Substring(0, 1));
        string spriteNumber = int.Parse(id.ToString().Substring(1, 2)).ToString();
        string localizador = "";

        switch (listNumber)
        {
            case 1: localizador = "/Headgear/Cabeza" + spriteNumber + ".png"; break;
            case 2: localizador = "/Bodies/Cuerpo" + spriteNumber + ".png"; break;
            case 3: localizador = "/Arms/Brazo" + spriteNumber + ".png"; break;
            case 4: localizador = "/Legs/Piernas" + spriteNumber + ".png"; break;
            case 5: localizador = "/Backs/Traseras" + spriteNumber + ".png"; break;
            case 6: localizador = "/Weapons/Arma" + spriteNumber + ".png"; break;
        }
        

        if (File.Exists(Application.persistentDataPath + localizador))
        {
            byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + localizador);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite mySprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
            result(mySprite);
        }
        else
        {
            WWW www = new WWW(texturesWebFolder + localizador);
            yield return www;
            Texture2D texture = www.texture;
            byte[] bytes = texture.EncodeToPNG();
            try { File.WriteAllBytes(Application.persistentDataPath + localizador, bytes);} catch { print("Espacio en disco insuficiente"); }
            Sprite mySprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
            result(mySprite);
        }
        yield return new WaitForEndOfFrame();
    }

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
}
