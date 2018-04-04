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
    }

    

    public Equipable_Item ItemByID(int bigID)
    {
        Equipable_Item item = new Equipable_Item();
        int listID = int.Parse(bigID.ToString().Substring(0, 1));
        int basicID = int.Parse(bigID.ToString().Substring(0, 3));
        try                                                                         //BUSCAR OBJETO EN LA LISTA CORRESPONDIENTE
        {
            switch (listID)
            {
                case 1: item = headgear_list.Find(e => e.ID == basicID); break;
                case 2: item = bodies_list.Find(e => e.ID == basicID); break;
                case 3: item = arms_list.Find(e => e.ID == basicID); break;
                case 4: item = legs_list.Find(e => e.ID == basicID); break;
                case 5: item = backs_list.Find(e => e.ID == basicID); break;
                case 6: item = weapons_list.Find(e => e.ID == basicID); break;
            }
        } catch { print("¡¡Item no encontrado!!"); }
        try                                                                         //AÑADIR PARÁMETROS ALEATORIOS DEL OBJETO
        {
            int randomStats = int.Parse(bigID.ToString().Substring(3, 6));
            if (randomStats > 0) item.addStat.Add(MejoraAleatoria(randomStats));
        } catch { }
        
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
            byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + localizador);
            Texture2D texture = new Texture2D(1, 1);
            yield return texture.LoadImage(bytes);
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


    //MEJORAS
    public GiveStat MejoraAleatoria(int randomValue)
    {
        int list = int.Parse(randomValue.ToString().Substring(0, 1));
        int value = int.Parse(randomValue.ToString().Substring(1));
        GiveStat mejora = new GiveStat();
        switch (list)
        {
            case 1: mejora = GiveDamage(value); break;
            case 2: mejora = GiveHealth(value); break;
            case 3: mejora = GiveSkill(value); break;
            case 4: mejora = GiveLuck(value); break;
        }
        return mejora;
    }

    private GiveStat GiveDamage(int value)
    {
        GiveStat mejora = new GiveStat() {
            stat_type = Stat.Damage,
            value = value
        };
        return mejora;
    }
    private GiveStat GiveHealth(int value)
    {
        GiveStat mejora = new GiveStat()
        {
            stat_type = Stat.Health,
            value = value
        };
        return mejora;
    }
    private GiveStat GiveSkill(int value)
    {
        GiveStat mejora = new GiveStat()
        {
            stat_type = Stat.Skill,
            value = value
        };
        return mejora;
    }
    private GiveStat GiveLuck(int value)
    {
        GiveStat mejora = new GiveStat()
        {
            stat_type = Stat.Luck,
            value = value
        };
        return mejora;
    }

    

    
}
