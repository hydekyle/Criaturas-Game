using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class Cofres_System : MonoBehaviour {

    void OnEnable()
    {
        transform.Find("Chest").GetComponent<Animator>().Play("Chest_Caer");
        Transform cofresCount = transform.Find("Cofres");
        //cofresCount.GetComponent<Animator>().Play("Chest_Count");
        UpdateChestsAmount();
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

    void UpdateChestsAmount()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.userName).Child("data").Child("chests").GetValueAsync()
            .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snap = task.Result;
                    transform.Find("Cofres").Find("Text").GetComponent<Text>().text = snap.Value.ToString();
                }
            });
    }

    public void COMPRAR_COFRE()
    {
        transform.Find("Cofres").Find("Buy_Chest").GetComponent<Button>().interactable = false;
        int gold;
        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.userName).Child("data").Child("gold").GetValueAsync()
            .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snap = task.Result;
                    gold = int.Parse(snap.Value.ToString());
                    if(gold >= 100)
                    {
                        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.userName).Child("data").Child("gold")
                        .SetValueAsync(gold - 100);
                        AñadirCofre(gold - 100);
                    }else
                    {
                        Message.instance.NewMessage("Necesitas más gemas");
                        transform.Find("Cofres").Find("Buy_Chest").GetComponent<Button>().interactable = true;
                    }
                }

            });   
    }

    private void AñadirCofre(int goldNow)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.userName).Child("data").Child("chests").GetValueAsync()
            .ContinueWith(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snap = task.Result;
                    int nCofres = int.Parse(snap.Value.ToString());

                    FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.userName).Child("data").Child("chests")
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

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Inventario").Child(Social.localUser.userName);

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                objetos userData = JsonUtility.FromJson<objetos>(task.Result.GetRawJsonValue());

                if(userData.data.chests > 0)
                {
                    userData.data.chests--;
                    objetos userInfo = new objetos() { items = inventario, data = userData.data };
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
}
