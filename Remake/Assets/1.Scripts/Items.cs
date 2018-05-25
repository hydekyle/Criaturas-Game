using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Enums;

public class Items : MonoBehaviour {

    //string texturesWebFolder = "https://www.evolution-battle.com/EvolutionPortable/Texturas";


    public static Items instance;
    [SerializeField]
    public List<Equipable_Item> headgear_list;
    public List<Equipable_Item> bodies_list;
    public List<Equipable_Item> arms_list;
    public List<Equipable_Item> legs_list;
    public List<Equipable_Item> backs_list;
    public List<Equipable_Item> weapons_list;

    public List<Equipable_Item> inventory_headgear;
    public List<Equipable_Item> inventory_bodies;
    public List<Equipable_Item> inventory_arms;
    public List<Equipable_Item> inventory_legs;


    void Awake()
    {
        instance = this;
    }

    public void StoreItem(string bigID)
    {
        int list = int.Parse(bigID.Substring(0, 1));
        switch (list)
        {
            case 1: inventory_headgear.Add(ItemByID(bigID)); break;
            case 2: inventory_bodies.Add(ItemByID(bigID)); break;
            case 3: inventory_arms.Add(ItemByID(bigID)); break;
            case 4: inventory_legs.Add(ItemByID(bigID)); break;
        }
    }

    public Equipable_Item ItemByID(string bigID)
    {
        print(bigID);
        Equipable_Item item = new Equipable_Item();
        int listID = int.Parse(bigID.Substring(0, 1));
        int basicID = int.Parse(bigID.Substring(0, 3));
        int rarity = int.Parse(bigID.Substring(3, 1));
        print("Rarity " + rarity);
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
        switch (rarity)
        {
            case 1: item.quality = Quality.Common; break;
            case 2: item.quality = Quality.Rare; break;
            case 3: item.quality = Quality.Epic; break;
            case 4: item.quality = Quality.Legendary; break;
        }
        item.ID = basicID;
        item.ID_string = bigID;
        return item;
    }


    public IEnumerator ItemSpriteByID(int id, System.Action<Sprite> result)
    {
        int listNumber = int.Parse(id.ToString().Substring(0, 1));
        string spriteNumber = int.Parse(id.ToString().Substring(1, 2)).ToString();
        string localizador = "";

        switch (listNumber)
        {
            case 1: localizador = "/Headgear/Cabeza" + spriteNumber; break;
            case 2: localizador = "/Bodies/Cuerpo"   + spriteNumber; break;
            case 3: localizador = "/Arms/Brazo"      + spriteNumber; break;
            case 4: localizador = "/Legs/Piernas"    + spriteNumber; break;
            case 5: localizador = "/Backs/Traseras"  + spriteNumber; break;
            case 6: localizador = "/Weapons/Arma"    + spriteNumber; break;
        }

        Texture2D texture = (Texture2D)Resources.Load("Piezas" + localizador);
        Sprite mySprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
        result(mySprite);
        yield return new WaitForEndOfFrame();
        /*  CARGAR TEXTURAS ONLINE SI NO EXISTEN (FUNCIÓN DESCARTADA)
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
        }*/
    }

    public string GetRandomItemID()
    {
        Equipable_Item newItem = new Equipable_Item();
        switch(Random.Range(1, 5)) //Equip Position
        {
            case 1: newItem = headgear_list[Random.Range(0, headgear_list.Count - 1)]; break;
            case 2: newItem = bodies_list[Random.Range(0, bodies_list.Count - 1)]; break;
            case 3: newItem = arms_list[Random.Range(0, arms_list.Count - 1)]; break;
            case 4: newItem = legs_list[Random.Range(0, legs_list.Count - 1)]; break;
        }
        int random = Random.Range(0, 101);
        int rarity = 1;
        if      (random >= 95) rarity = 4;
        else if (random >= 85) rarity = 3;
        else if (random >= 60) rarity = 2;
        int fuerza = 1;
        int vida = 1;
        int skill = 1;
        int luck = 1;
        for (var x = 0; x < 5; x++)
        {
            switch (Random.Range(1, 5))
            {
                case 1: fuerza++; break;
                case 2: vida++; break;
                case 3: skill++; break;
                case 4: luck++; break;
            }
        }
        int skill1ID = GetRandomSkillID();
        int skill2ID = GetRandomSkillID();
        if (skill1ID == skill2ID) if(int.Parse(skill2ID.ToString().Substring(1, 2)) == 1) skill2ID++; else skill2ID--; //Evitar duplicado skills

        newItem.ID_string = newItem.ID.ToString() + rarity.ToString() + vida.ToString() + fuerza.ToString() + 
                                 skill.ToString() + luck.ToString() +skill1ID.ToString() + skill2ID.ToString();

        //CanvasBase.instance.ShowItemInfo(newItem.ID_string);
        return newItem.ID_string;
    }

    public string GetRandomItemID(int nLista)
    {
        Equipable_Item newItem = new Equipable_Item();
        int lista = Mathf.Clamp(nLista, 1, 5);
        switch (lista) //Equip Position
        {
            case 1: newItem = headgear_list[Random.Range(0, headgear_list.Count - 1)]; break;
            case 2: newItem = bodies_list[Random.Range(0, bodies_list.Count - 1)]; break;
            case 3: newItem = arms_list[Random.Range(0, arms_list.Count - 1)]; break;
            case 4: newItem = legs_list[Random.Range(0, legs_list.Count - 1)]; break;
        }
        int random = Random.Range(0, 101);
        int rarity = 1;
        if (random >= 95) rarity = 4;
        else if (random >= 85) rarity = 3;
        else if (random >= 60) rarity = 2;
        int fuerza = 1;
        int vida = 1;
        int skill = 1;
        int luck = 1;
        for (var x = 0; x < 5; x++)
        {
            switch (Random.Range(1, 5))
            {
                case 1: fuerza++; break;
                case 2: vida++; break;
                case 3: skill++; break;
                case 4: luck++; break;
            }
        }
        int skill1ID = GetRandomSkillID();
        int skill2ID = GetRandomSkillID();
        if (skill1ID == skill2ID) if (int.Parse(skill2ID.ToString().Substring(1, 2)) == 1) skill2ID++; else skill2ID--; //Evitar duplicado skills

        newItem.ID_string = newItem.ID.ToString() + rarity.ToString() + vida.ToString() + fuerza.ToString() +
                                 skill.ToString() + luck.ToString() + skill1ID.ToString() + skill2ID.ToString();

        //CanvasBase.instance.ShowItemInfo(newItem.ID_string);
        return newItem.ID_string;
    }

    int GetRandomSkillID()
    {
        int skill1ID = 0;
        int skillList = Random.Range(1, 5);
        switch (skillList)
        {
            case 1: skill1ID = Skills.instance.skill_list_assassin[Random.Range(0, Skills.instance.skill_list_assassin.Count - 1)].ID; break;
            case 2: skill1ID = Skills.instance.skill_list_alpha[Random.Range(0, Skills.instance.skill_list_alpha.Count - 1)].ID; break;
            case 3: skill1ID = Skills.instance.skill_list_charming[Random.Range(0, Skills.instance.skill_list_charming.Count - 1)].ID; break;
            case 4: skill1ID = Skills.instance.skill_list_pacifist[Random.Range(0, Skills.instance.skill_list_pacifist.Count - 1)].ID; break;
        }
        return skill1ID;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GetRandomItemID();
        }
    }





    //MEJORAS
    public GiveStat MejoraAleatoria(int randomValue)
    {
        int list = int.Parse(randomValue.ToString().Substring(0, 1));
        int value = int.Parse(randomValue.ToString().Substring(1));
        GiveStat mejora = new GiveStat();
        switch (list)
        {
            case 1: mejora = GiveStrenght(value); break;
            case 2: mejora = GiveHealth(value); break;
            case 3: mejora = GiveSkill(value); break;
            case 4: mejora = GiveLuck(value); break;
        }
        return mejora;
    }

    private GiveStat GiveStrenght(int value)
    {
        GiveStat mejora = new GiveStat() {
            stat_type = Stat.Strenght,
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
            stat_type = Stat.Dextery,
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
