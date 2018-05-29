using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class Huevo : MonoBehaviour {

	public void BTN_HUEVO()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        string rHeadgear = Items.instance.GetRandomItemID(1);
        string rBody     = Items.instance.GetRandomItemID(2);
        string rArms     = Items.instance.GetRandomItemID(3);
        string rLegs     = Items.instance.GetRandomItemID(4);

        List<string> itemList = new List<string>();
        itemList.Add(rHeadgear);
        itemList.Add(rBody);
        itemList.Add(rArms);
        itemList.Add(rLegs);

        UserDB userData = new UserDB()
        {
            chests = 0,
            gold = 100,
            victorias = 0,
            derrotas = 0
        };

        objetos userInfo = new objetos() {
            items = itemList,
            data = userData
        };

        EquipDB equip = new EquipDB()
        {
            head = rHeadgear,
            body = rBody,
            arms = rArms,
            legs = rLegs
        };

        string jsonObjetos = JsonUtility.ToJson(userInfo);
        string jsonEquip = JsonUtility.ToJson(equip);

        reference.Child("Equipamiento").Child(Social.localUser.userName).SetRawJsonValueAsync(jsonEquip);

        reference.Child("Inventario").Child(Social.localUser.userName).SetRawJsonValueAsync(jsonObjetos).ContinueWith((obj2) =>
        {
            if (obj2.IsCompleted)
            {
                Destroy(GameManager.instance.gameObject);
                SceneManager.LoadScene(0);
            }
        });
    }
}
