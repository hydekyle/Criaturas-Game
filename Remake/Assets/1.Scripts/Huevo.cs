using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Huevo : MonoBehaviour {

    bool eggDone;

	public void BTN_HUEVO()
    {
        transform.Find("Huevo").GetComponent<Button>().interactable = false;

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        string rHeadgear = Items.instance.GetRandomItemID(1);
        string rBody     = Items.instance.GetRandomItemID(2);
        string rArms     = Items.instance.GetRandomItemID(3);
        string rLegs     = Items.instance.GetRandomItemID(4);

        Equipment equipment = new Equipment()
        {
            head = Items.instance.ItemByID(rHeadgear),
            body = Items.instance.ItemByID(rBody),
            arms = Items.instance.ItemByID(rArms),
            legs = Items.instance.ItemByID(rLegs)
        };

        

        List<string> itemList = new List<string>();
        itemList.Add(rHeadgear);
        itemList.Add(rBody);
        itemList.Add(rArms);
        itemList.Add(rLegs);

        UserDB userData = new UserDB()
        {
            chests = 0,
            chests_VIP = 0,
            gold = 800,
            gold_VIP = 100,
            victorias = 0,
            derrotas = 0
        };

        EquipDB equip = new EquipDB()
        {
            head = rHeadgear,
            body = rBody,
            arms = rArms,
            legs = rLegs
        };

        objetos userInfo = new objetos() {
            items = itemList,
            data = userData,
            equipamiento = equip
        };

        string jsonObjetos = JsonUtility.ToJson(userInfo);
        string jsonEquip = JsonUtility.ToJson(equip);

        reference.Child("Inventario").Child(Social.localUser.id).SetRawJsonValueAsync(jsonObjetos).ContinueWith((obj2) =>
        {
            if (obj2.IsCompleted)
            {
                transform.Find("Huevo").gameObject.SetActive(false);
                Menu.instance.InitializeVisor(Menu.instance.GetPlayerVisor(1), Vector3.zero, false);
                StartCoroutine(Menu.instance.VisualizarEquipamiento(equipment, 1));
                Message.instance.NewMessage("¡Saluda a tu nueva mascota!");
                eggDone = true;
            }

            if (obj2.IsFaulted)
            {
                transform.Find("Huevo").GetComponent<Button>().interactable = true;
            }
        });
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && eggDone)
        {
            GoMainMenu();
        }
    }

    void GoMainMenu()
    {
        Destroy(GameManager.instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
