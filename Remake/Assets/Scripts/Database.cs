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
}
