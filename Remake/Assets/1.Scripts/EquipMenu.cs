using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Enums;

public class EquipMenu : MonoBehaviour {

    Transform inventarioT;
    Image[] item_image;
    int[] storage_ID = new int[12];


    void Start()
    {
        inventarioT = transform.Find("Inventario");
        Inicialize();
    }

    void Inicialize()
    {
        item_image = new Image[inventarioT.childCount];
        for (var x = 0; x < inventarioT.childCount; x++)
        {
            Transform t = inventarioT.GetChild(x);
            item_image[x] = t.Find("Image").GetComponent<Image>();
            t.gameObject.name = x.ToString();
        }
    }


    IEnumerator Visualizar(Equip_Position equipList)
    {
        List<Equipable_Item> e = new List<Equipable_Item>();
        switch (equipList)
        {
            case Equip_Position.Head: e = Items.instance.headgear_list; break;
            case Equip_Position.Body: e = Items.instance.bodies_list; break;
            case Equip_Position.Arms: e = Items.instance.arms_list; break;
            case Equip_Position.Legs: e = Items.instance.legs_list; break;
        }
        for (var x = 0; x < 12; x++)
        {
            storage_ID[x] = e[x].ID;
            yield return Items.instance.ItemSpriteByID(e[x].ID, result => item_image[x].sprite = result);
        }
    }

    void MostrarEquipo(Equip_Position equipList)
    {
        switch (equipList)
        {
            case Equip_Position.Head: print("head"); break;
        }
    }

    public void BTN_HEAD()
    {
        StartCoroutine(Visualizar(Equip_Position.Head));
    }

    public void BTN_BODY()
    {
        StartCoroutine(Visualizar(Equip_Position.Body));
    }

    public void BTN_ARMS()
    {
        StartCoroutine(Visualizar(Equip_Position.Arms));
    }

    public void BTN_LEGS()
    {
        StartCoroutine(Visualizar(Equip_Position.Legs));
    }

    public void BTN_ITEM()
    {
        int id = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        GameManager.instance.player.criatura.equipment.legs = Items.instance.ItemByID(storage_ID[id]);
        StartCoroutine(Menu.instance.VisualizarEquipamiento(GameManager.instance.player.criatura.equipment, 1));
    }

}
