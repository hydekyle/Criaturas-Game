using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Firebase.Database;

public class Cofres_System : MonoBehaviour {

    public static Cofres_System instance;
    public Sprite s_chest, s_chestVIP, s_shadow_chest, s_shadow_chestVIP;
    public Transform chest, buy_chest_VIP, buy_chest, resplandor;
    public Text text_chest, text_chest_VIP;

    void Awake()
    {
        instance = instance ?? this;
    }

    void OnEnable()
    {
        chest.gameObject.SetActive(true);
        SetChestAmount(GameManager.instance.userdb.chests.ToString());
        SetChestAmount_VIP(GameManager.instance.userdb.chests_VIP.ToString());
        DefaultSetup();
        CanvasBase.instance.UpdateGoldView();
    }

    void OnDisable()
    {
        chest.gameObject.SetActive(false);
    }

    private void DefaultSetup()
    {
        buy_chest_VIP.gameObject.SetActive(false);
        buy_chest_VIP.gameObject.SetActive(true);
        SetChestType(false);
    }

    void Update()
    {
        if (gameObject.activeSelf) resplandor.Rotate(Vector3.forward * 80 * Time.deltaTime);
    }

	public void VOLVER()
    {
        transform.gameObject.SetActive(false);
        CanvasBase.instance.BackToMenu();
    }

    public void BTN_ABRIR_COMPRAS()
    {
        transform.parent.Find("Compras").gameObject.SetActive(true);
        transform.gameObject.SetActive(false);
    }

    public void BTN_CERRAR_COMPRAS()
    {
        transform.parent.Find("Compras").gameObject.SetActive(false);
        transform.gameObject.SetActive(true);
    }

    public void BTN_ABRIR_COFRE()
    {
        AbrirCofre();
    }

    public void UpdateChestsAmount()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("data").Child("chests").GetValueAsync()
            .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snap = task.Result;
                    SetChestAmount(snap.Value.ToString());
                }
            });
    }

    public void SetChestAmount(string amount)
    {
        text_chest.text = amount;
    }

    void SetChestAmount_VIP(string amount)
    {
        text_chest_VIP.text = amount;
    }

    public void COMPRAR_COFRE()
    {
        Button cofre = transform.Find("Buy_Chest").GetComponent<Button>();
        cofre.interactable = false;
        int gold;
        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("data").Child("gold").GetValueAsync()
            .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snap = task.Result;
                    gold = int.Parse(snap.Value.ToString());
                    if(gold >= 100)
                    {
                        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("data").Child("gold")
                        .SetValueAsync(gold - 100);
                        AñadirCofre(gold - 100);
                    }else
                    {
                        Message.instance.NewMessage("Necesitas más gemas");
                    }
                    cofre.interactable = true;
                }

            });   
    }

    private void AñadirCofre(int goldNow)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("data").Child("chests").GetValueAsync()
            .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snap = task.Result;
                    int nCofres = int.Parse(snap.Value.ToString());

                    FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id).Child("data").Child("chests")
                    .SetValueAsync(nCofres + 1);
                    UpdateChestsAmount();
                    CanvasBase.instance.UpdateGoldViewNoDB(goldNow.ToString());
                    transform.Find("Cofres").Find("Buy_Chest").GetComponent<Button>().interactable = true;
                }
            });
    }

    private void AbrirCofre()
    {
        List<string> inventario = Items.instance.GetYourItems();
        inventario.Add(Items.instance.GetRandomItemID());
        inventario.Add(Items.instance.GetRandomItemID());
        inventario.Add(Items.instance.GetRandomItemID());
        inventario.Add(Items.instance.GetRandomItemID());

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.id);

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                objetos userData = JsonUtility.FromJson<objetos>(task.Result.GetRawJsonValue());

                if(userData.data.chests > 0)
                {
                    EquipDB eDB = userData.equipamiento;
                    userData.data.chests--;
                    objetos userInfo = new objetos() { items = inventario, data = userData.data, equipamiento = eDB};
                    string json = JsonUtility.ToJson(userInfo);

                    reference.SetRawJsonValueAsync(json).ContinueWith((obj2) =>
                    {
                        if (obj2.IsCompleted)
                        {
                            CanvasBase.instance.LoadYourItems();
                            UpdateChestsAmount();
                            Message.instance.NewMessage("Objetos añadidos");
                        }

                    });
                }
                
            }

        });
        

    }

    public void BTN_CHEST_NORMAL()
    {
        SetChestType(false);
    }

    public void BTN_CHEST_VIP()
    {
        SetChestType(true);
    }

    private void SetChestType(bool VIP)
    {
        chest.gameObject.SetActive(false);

        if (VIP) PutChestSprites(s_chestVIP, s_shadow_chestVIP);
        else PutChestSprites(s_chest, s_shadow_chest);

        buy_chest.gameObject.SetActive(!VIP);
        buy_chest_VIP.gameObject.SetActive(VIP);

        chest.gameObject.SetActive(true);
        chest.GetComponent<Animator>().Play("Chest_Caer");
    }

    private void PutChestSprites(Sprite chestSprite, Sprite shadow)
    {
        chest.GetComponent<Image>().sprite = shadow;
        chest.Find("Chest").GetComponent<Image>().sprite = chestSprite;
    }

}
