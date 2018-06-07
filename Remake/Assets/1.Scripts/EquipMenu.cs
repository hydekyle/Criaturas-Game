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
    string[] storage_ID;
    Equip_Position currentEquipView = Equip_Position.None;
    int currentPage = 0;
    Button btn_back;
    Button btn_forw;
    List<Equipable_Item> actualList = new List<Equipable_Item>();
    RectTransform tic_new;
    RectTransform tic_equip;
    Vector3 pos_tic_new = new Vector3(17, 17, 0);
    Vector3 pos_tic_equip = new Vector3(15, -11, 0);
    List<int> equip_list_id = new List<int>();

    //Item Info Menu:
    Image retrato;
    Text rarity_text;
    Text rarity_shadow;
    Text skill1_name;
    Text skill1_Description;
    Text skill2_name;
    Text skill2_description;
    Text value_life;
    Text value_attack;
    Text value_skill;
    Text value_luck;

    void Awake()
    {
        Inicialize();
    }

    void Inicialize()
    {
        Transform infoT = transform.Find("Info");
        retrato = infoT.Find("Retrato").GetComponent<Image>();
        skill1_name = infoT.Find("Skill1").Find("Name").GetComponent<Text>();
        skill1_Description = infoT.Find("Skill1").Find("Description").GetComponent<Text>();
        skill2_name = infoT.Find("Skill2").Find("Name").GetComponent<Text>();
        skill2_description = infoT.Find("Skill2").Find("Description").GetComponent<Text>();
        rarity_text = infoT.Find("Rarity_Color").GetComponent<Text>();
        rarity_shadow = infoT.Find("Rarity_Shadow").GetComponent<Text>();
        value_life = infoT.Find("Value_Life").GetComponent<Text>();
        value_attack = infoT.Find("Value_Attack").GetComponent<Text>();
        value_skill = infoT.Find("Value_Skill").GetComponent<Text>();
        value_luck = infoT.Find("Value_Luck").GetComponent<Text>();

        Transform inventario = transform.Find("Panel_Inventario").Find("Inventario");
        inventarioT = inventario.Find("Botones_Items");
        btn_back = inventario.Find("Arrow_Left").GetComponent<Button>();
        btn_forw = inventario.Find("Arrow_Right").GetComponent<Button>();
        tic_new = inventario.Find("Tic_New").GetComponent<RectTransform>();
        tic_equip = inventario.Find("Tic_Equip").GetComponent<RectTransform>();
        storage_ID = new string[inventarioT.childCount];
        item_image = new Image[inventarioT.childCount];
        rarity_image = new Image[inventarioT.childCount];
        for (var x = 0; x < inventarioT.childCount; x++)
        {
            Transform t = inventarioT.GetChild(x);
            rarity_image[x] = t.GetComponent<Image>();
            item_image[x] = t.Find("Image").GetComponent<Image>();
            t.gameObject.name = x.ToString();
        }
    }

    void OnEnable()
    {
        CanvasBase.instance.CheckFirebaseLogin();
        currentEquipView = Equip_Position.None;
        BTN_HEAD();
    }

    public IEnumerator ViewItemInfo()
    {
        Sprite itemSprite = null;
        int itemID = 0;
        switch (currentEquipView)
        {
            case Equip_Position.Head: itemID = GameManager.instance.player.criatura.equipment.head.ID; break;
            case Equip_Position.Body: itemID = GameManager.instance.player.criatura.equipment.body.ID; break;
            case Equip_Position.Arms: itemID = GameManager.instance.player.criatura.equipment.arms.ID; break;
            case Equip_Position.Legs: itemID = GameManager.instance.player.criatura.equipment.legs.ID; break;
        }
        yield return Items.instance.ItemSpriteByID(itemID, result => itemSprite = result);
        print(itemID);
        retrato.sprite = itemSprite;
        retrato.preserveAspect = true;
    }

    public IEnumerator ViewItemInfo(string id)
    {
        Sprite itemSprite = null;
        int itemID = int.Parse(id.Substring(0, 3));
        int rarity = int.Parse(id.Substring(3, 1));
        int health = int.Parse(id.Substring(4, 1));
        int attack = int.Parse(id.Substring(5, 1));
        int skill  = int.Parse(id.Substring(6, 1));
        int luck   = int.Parse(id.Substring(7, 1));
        Skill skill1 = Skills.instance.GetSkillByID(id.Substring(8, 3));
        Skill skill2 = Skills.instance.GetSkillByID(id.Substring(11, 3));
        yield return Items.instance.ItemSpriteByID(itemID, result => itemSprite = result);
        string textRarity = Lenguaje.Instance.Text_RarityID(rarity);
        rarity_text.text = textRarity;
        rarity_shadow.text = textRarity;
        rarity_text.color = ColorByQuality(rarity);
        retrato.sprite = itemSprite;

        attack *= rarity;
        health *= rarity;
        skill *= rarity;
        luck *= rarity;

        value_attack.text = attack.ToString();
        value_life.text = health.ToString();
        value_skill.text = skill.ToString();
        value_luck.text = luck.ToString();

        if (Lenguaje.Instance.spanish_language)
        {
            skill1_name.text = skill1.name_spanish;
            skill1_Description.text = skill1.description_spanish;

            skill2_name.text = skill2.name_spanish;
            skill2_description.text = skill2.description_spanish;
        }else
        {
            skill1_name.text = skill1.name_english;
            skill1_Description.text = skill1.description_english;

            skill2_name.text = skill2.name_english;
            skill2_description.text = skill2.description_english;
        }
        
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
            case Quality.Epic: color = new Color(255f, 0f, 255f); break;
            case Quality.Rare: color = Color.green; break;
            case Quality.Common: color = Color.white; break;
        }
        return color;
    }

    Color ColorByQuality(int i)
    {
        Color color = new Color();
        switch (i)
        {
            case 4: color = Color.yellow; break;
            case 3: color = new Color(255f, 0f, 255f); break;
            case 2: color = Color.green; break;
            case 1: color = Color.white; break;
        }
        return color;
    }

    bool CheckEquipedID(int id)
    {
        int lista = int.Parse(id.ToString().Substring(0, 1));
        Equipment e = GameManager.instance.player.criatura.equipment;
        return true;
    }

    IEnumerator Visualizar(Equip_Position equipList, int pagNumber)
    {
        if (pagNumber == 0) btn_back.interactable = false; else btn_back.interactable = true;
        int totalSlots = inventarioT.childCount;
        int y = 0 + (totalSlots * pagNumber); //ITERATOR
        string equipedID = "";
        storage_ID = new string[totalSlots];
        Equip_Position lastView = currentEquipView;
        currentEquipView = equipList;
        currentPage = pagNumber;
        tic_equip.transform.gameObject.SetActive(false);
        for (var x = 0; x < totalSlots; x++) //Limpia las imagenes antes de cargar las nuevas.
        {
            item_image[x].sprite = null;
            rarity_image[x].color = ColorByQuality(Quality.Common);
        }
        if(lastView != currentEquipView) //Lee lista de objetos y los ordena.
        {
            switch (equipList)
            {
                case Equip_Position.Head: actualList = SortListByQuality(Items.instance.inventory_headgear); break;
                case Equip_Position.Body: actualList = SortListByQuality(Items.instance.inventory_bodies); break;
                case Equip_Position.Arms: actualList = SortListByQuality(Items.instance.inventory_arms);  break;
                case Equip_Position.Legs: actualList = SortListByQuality(Items.instance.inventory_legs);  break;
            }
        }
        switch (equipList) //Lee ID equipado según lista.
        {
            case Equip_Position.Head: equipedID = GameManager.instance.player.criatura.equipment.head.ID_string; break;
            case Equip_Position.Body: equipedID = GameManager.instance.player.criatura.equipment.body.ID_string; break;
            case Equip_Position.Arms: equipedID = GameManager.instance.player.criatura.equipment.arms.ID_string; break;
            case Equip_Position.Legs: equipedID = GameManager.instance.player.criatura.equipment.legs.ID_string; break;
        }
        for (var x = 0; x < totalSlots; x++) //Por cada slot de equipamiento.
        {
            if(y < actualList.Count) //Lee elementos y si no hay acaba el bucle
            {
                storage_ID[x] = actualList[y].ID_string;
                rarity_image[x].color = ColorByQuality(actualList[y].quality);
                yield return Items.instance.ItemSpriteByID(actualList[y].ID, result => {
                    item_image[x].sprite = result;
                    item_image[x].preserveAspect = true;
                    });
                if (storage_ID[x] == equipedID) Colocar_Tic_Equip(x);
                y++;
            }else
            {
                btn_forw.interactable = false;
                break;
            }  
        }
        if (storage_ID[inventarioT.childCount - 1].Length > 0) btn_forw.interactable = true; else btn_forw.interactable = false;
        StartCoroutine(ViewItemInfo());
    }

    void Colocar_Tic_New(int id)
    {
        tic_new.position = item_image[id].rectTransform.position + pos_tic_new;
        tic_new.transform.gameObject.SetActive(true);
    }

    void Colocar_Tic_Equip(int id)
    {
        tic_equip.position = item_image[id].rectTransform.position + pos_tic_equip;
        tic_equip.transform.gameObject.SetActive(true);
    }

    public void BTN_HEAD()
    {
        StartCoroutine(Visualizar(Equip_Position.Head, 0));
        CanvasBase.instance.ShowItemInfo(GameManager.instance.player.criatura.equipment.head.ID_string);
    }

    public void BTN_BODY()
    {
        StartCoroutine(Visualizar(Equip_Position.Body, 0));
        CanvasBase.instance.ShowItemInfo(GameManager.instance.player.criatura.equipment.body.ID_string);
    }

    public void BTN_ARMS()
    {
        StartCoroutine(Visualizar(Equip_Position.Arms, 0));
        CanvasBase.instance.ShowItemInfo(GameManager.instance.player.criatura.equipment.arms.ID_string);
    }

    public void BTN_LEGS()
    {
        StartCoroutine(Visualizar(Equip_Position.Legs, 0));
        CanvasBase.instance.ShowItemInfo(GameManager.instance.player.criatura.equipment.legs.ID_string);
    }

    public void BTN_BACK()
    {
        StopCoroutine("Visualizar");
        if (currentPage > 0) StartCoroutine(Visualizar(currentEquipView, --currentPage));
    }

    public void BTN_NEXT()
    {
        try
        {
            StopCoroutine("Visualizar");
            if (storage_ID[inventarioT.childCount - 1].Length > 0) StartCoroutine(Visualizar(currentEquipView, ++currentPage));
        }catch { print("Oops"); }
        
    }

    public void BTN_ITEM()
    {
        try
        {
            int id = int.Parse(EventSystem.current.currentSelectedGameObject.name);
            if (storage_ID[id].Length > 0)
            {
                EquiparItem(storage_ID[id]);
                StartCoroutine(Menu.instance.VisualizarEquipamiento(GameManager.instance.player.criatura.equipment, 1));
                StartCoroutine(ViewItemInfo());
                Colocar_Tic_Equip(id);
            }
        }catch { print("Oops"); }
        
    }


    void EquiparItem(string id)
    {
        CanvasBase.instance.ShowItemInfo(id);
        int list = int.Parse(id.ToString().Substring(0, 1));
        switch (list)
        {
            case 1: GameManager.instance.player.criatura.equipment.head = Items.instance.ItemByID(id); break;
            case 2: GameManager.instance.player.criatura.equipment.body = Items.instance.ItemByID(id); break;
            case 3: GameManager.instance.player.criatura.equipment.arms = Items.instance.ItemByID(id); break;
            case 4: GameManager.instance.player.criatura.equipment.legs = Items.instance.ItemByID(id); break;
        }
    }

    public void BTN_EQUIP_INFO()
    {
        GameObject info_window = transform.Find("Info").gameObject;
        info_window.SetActive(info_window.activeSelf ? false : true);
    }

}
