using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Enums;

public class EquipMenu : MonoBehaviour {

    Transform inventarioT;
    Image[] item_image;
    Image[] rarity_image;
    int[] storage_ID;
    Equip_Position currentEquipView = Equip_Position.None;
    int currentPage = 0;
    Button btn_back;
    Button btn_forw;
    List<Equipable_Item> actualList = new List<Equipable_Item>();
    

    void Start()
    {
        Transform inventario = transform.Find("Panel_Inventario").Find("Inventario");
        inventarioT = inventario.Find("Botones_Items");
        btn_back = inventario.Find("Arrow_Left").GetComponent<Button>();
        btn_forw = inventario.Find("Arrow_Right").GetComponent<Button>();
        storage_ID = new int[inventarioT.childCount];
        Inicialize();
    }

    void Inicialize()
    {
        item_image = new Image[inventarioT.childCount];
        rarity_image = new Image[inventarioT.childCount];
        for (var x = 0; x < inventarioT.childCount; x++)
        {
            Transform t = inventarioT.GetChild(x);
            rarity_image[x] = t.GetComponent<Image>();
            item_image[x] = t.Find("Image").GetComponent<Image>();
            t.gameObject.name = x.ToString();
        }
        StartCoroutine(Visualizar(Equip_Position.Head, 0));
    }

    List<Equipable_Item> SortListByQuality(List<Equipable_Item> list)
    {
        List<Equipable_Item> sortedList = new List<Equipable_Item>();
        List<Equipable_Item> legendary = new List<Equipable_Item>();
        List<Equipable_Item> epic = new List<Equipable_Item>();
        List<Equipable_Item> rare = new List<Equipable_Item>();
        List<Equipable_Item> common = new List<Equipable_Item>();
        foreach(Equipable_Item e in list)
        {
            switch (e.quality)
            {
                case Quality.Common: common.Add(e); break;
                case Quality.Rare: rare.Add(e); break;
                case Quality.Epic: epic.Add(e); break;
                case Quality.Legendary: legendary.Add(e); break;
            }
        }
        foreach (Equipable_Item e in legendary) sortedList.Add(e);
        foreach (Equipable_Item e in epic) sortedList.Add(e);
        foreach (Equipable_Item e in rare) sortedList.Add(e);
        foreach (Equipable_Item e in common) sortedList.Add(e);
        return sortedList;
    }

    Color ColorByQuality(Quality q)
    {
        Color color = new Color();
        switch (q)
        {
            case Quality.Legendary: color = Color.yellow; break;
            case Quality.Epic: color = Color.blue; break;
            case Quality.Rare: color = Color.green; break;
            case Quality.Common: color = Color.white; break;
        }
        return color;
    }

    IEnumerator Visualizar(Equip_Position equipList, int pagNumber)
    {
        if (pagNumber == 0) btn_back.interactable = false; else btn_back.interactable = true;
        int totalSlots = inventarioT.childCount;
        int y = 0 + (totalSlots * pagNumber);
        storage_ID = new int[totalSlots];
        Equip_Position lastView = currentEquipView;
        currentEquipView = equipList;
        currentPage = pagNumber;
        for (var x = 0; x < totalSlots; x++)
        {
            item_image[x].sprite = null;
        }
        if(lastView != currentEquipView)
        {
            switch (equipList)
            {
                case Equip_Position.Head: actualList = SortListByQuality(Items.instance.headgear_list); break;
                case Equip_Position.Body: actualList = SortListByQuality(Items.instance.bodies_list); break;
                case Equip_Position.Arms: actualList = SortListByQuality(Items.instance.arms_list); break;
                case Equip_Position.Legs: actualList = SortListByQuality(Items.instance.legs_list); break;
            }
        }
        for (var x = 0; x < totalSlots; x++)
        {
            if(y < actualList.Count)
            {
                storage_ID[x] = actualList[y].ID;
                rarity_image[x].color = ColorByQuality(actualList[y].quality);
                yield return Items.instance.ItemSpriteByID(actualList[y].ID, result => item_image[x].sprite = result);
                y++;
            }else
            {
                btn_forw.interactable = false;
                break;
            }
            
        }
        
        
        if (storage_ID[inventarioT.childCount - 1] > 0) btn_forw.interactable = true; else btn_forw.interactable = false;
        
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
        StartCoroutine(Visualizar(Equip_Position.Head, 0));
    }

    public void BTN_BODY()
    {
        StartCoroutine(Visualizar(Equip_Position.Body, 0));
    }

    public void BTN_ARMS()
    {
        StartCoroutine(Visualizar(Equip_Position.Arms, 0));
    }

    public void BTN_LEGS()
    {
        StartCoroutine(Visualizar(Equip_Position.Legs, 0));
    }

    public void BTN_ITEM()
    {
        int id = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        if(storage_ID[id] > 0)
        {
            EquiparItem(storage_ID[id]);
            StartCoroutine(Menu.instance.VisualizarEquipamiento(GameManager.instance.player.criatura.equipment, 1));
        }
        
    }

    public void BTN_BACK()
    {
        if(currentPage > 0) StartCoroutine(Visualizar(currentEquipView, --currentPage));
    }

    public void BTN_NEXT()
    {
        if (storage_ID[inventarioT.childCount - 1] > 0) StartCoroutine(Visualizar(currentEquipView, ++currentPage));
    }

    void EquiparItem(int id)
    {
        int list = int.Parse(id.ToString().Substring(0, 1));
        switch (list)
        {
            case 1: GameManager.instance.player.criatura.equipment.head = Items.instance.ItemByID(id); break;
            case 2: GameManager.instance.player.criatura.equipment.body = Items.instance.ItemByID(id); break;
            case 3: GameManager.instance.player.criatura.equipment.arms = Items.instance.ItemByID(id); break;
            case 4: GameManager.instance.player.criatura.equipment.legs = Items.instance.ItemByID(id); break;
        }
    }

}
