using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour {

    public string[] Items;

    IEnumerator Start()
    {
        WWW itemsData = new WWW("http://localhost/CoolGame/users.php");
        yield return itemsData;
        string itemsDataString = itemsData.text;
        print(itemsDataString);
        Items = itemsData.text.Split(';');
        print(GetDataValue(Items[0], "Name:"));
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

    //void ReadJSON()
    //{
    //    //Equipable_Item e1 = new Equipable_Item();
    //    //e1 = JsonUtility.FromJson<Equipable_Item>(File.ReadAllText(Application.persistentDataPath + "/texto.txt"));
    //}

    //void TestJSON()
    //{
    //    Equipable_Item e1 = new Equipable_Item() { nombre = "AyozeReaper", ID = 111, addStat = null, quality = Quality.Rare }; headgear_list[0] = e1;
    //    string json = JsonUtility.ToJson(headgear_list);
    //    File.WriteAllText(Application.persistentDataPath + "/textoList.txt", json);
    //}
}
