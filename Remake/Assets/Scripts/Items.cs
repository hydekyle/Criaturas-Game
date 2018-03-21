using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Items : MonoBehaviour {

    string texturesWebFolder = "https://www.evolution-battle.com/EvolutionPortable/Texturas";


    public static Items instance;
    public List<Equipable_Item> headgear_list;
    public List<Equipable_Item> bodies_list;
    public List<Equipable_Item> arms_list;
    public List<Equipable_Item> legs_list;
    public List<Equipable_Item> backs_list;
    public List<Equipable_Item> weapons_list;

    public Sprite spriteBuscado;

    void Start()
    {
        instance = this;
        CheckGameFolders();
        StartCoroutine(SendImage(101));
        StartCoroutine(SendImage(201));
        StartCoroutine(SendImage(301));
        StartCoroutine(SendImage(401));
        StartCoroutine(SendImage(501));
        StartCoroutine(SendImage(601));
    }

    IEnumerator SendImage(int id) {
        int listNumber = int.Parse(id.ToString().Substring(0, 1));
        Sprite spriteBuscado = null;
        yield return StartCoroutine(ItemSpriteByID(id, value => spriteBuscado = value));
        Menu.instance.SetImageVisor(spriteBuscado, listNumber);
    }


    public Equipable_Item ItemByID(int id)
    {
        Equipable_Item item = new Equipable_Item();
        int listID;
        int itemID;
        int.TryParse(id.ToString().Substring(0, 1), out listID);
        int.TryParse(id.ToString().Substring(1, 2), out itemID);
        switch (listID)
        {
            case 1: item = headgear_list[itemID]; break;
            case 2: item = bodies_list[itemID]; break;
            case 3: item = arms_list[itemID]; break;
            case 4: item = legs_list[itemID]; break;
            case 5: item = backs_list[itemID]; break;
            case 6: item = weapons_list[itemID]; break;
        }
        return item;
    }

    public IEnumerator ItemSpriteByID(int id, Action<Sprite> result)
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
            print("El archivo existe");
            byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + localizador);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite mySprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
            result(mySprite);
        }
        else
        {
            print("Creando archivo de "+ texturesWebFolder + localizador);
            WWW www = new WWW(texturesWebFolder + localizador);
            yield return www;
            Texture2D texture = www.texture;
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + localizador, bytes);
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
