using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Enums;

public class EquipMenu : MonoBehaviour {

    public float clampLeft;
    public float distanciaItemsScrollBar = 80f;
    Transform inventarioT;
    Image[] item_image;
    Image[] rarity_image;
    string[] storage_ID;
    Equip_Position currentEquipView = Equip_Position.None;
    List<Equipable_Item> actualList = new List<Equipable_Item>();
    List<int> equip_list_id = new List<int>();
    public GameObject buttonPrefab;

    //Item Info Menu:
    Image retrato;
    Text rarity_text;
    Text rarity_shadow;
    Text skill1_name;
    Image skill1_orb;
    Text skill2_name;
    Image skill2_orb;
    Text skill3_name;
    Image skill3_orb;
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
        skill1_orb = infoT.Find("Skill1").Find("Orbe").GetComponent<Image>();
        skill2_name = infoT.Find("Skill2").Find("Name").GetComponent<Text>();
        skill2_orb = infoT.Find("Skill2").Find("Orbe").GetComponent<Image>();
        skill3_name = infoT.Find("Skill3").Find("Name").GetComponent<Text>();
        skill3_orb = infoT.Find("Skill3").Find("Orbe").GetComponent<Image>();
        rarity_text = infoT.Find("Rarity_Color").GetComponent<Text>();
        rarity_shadow = infoT.Find("Rarity_Shadow").GetComponent<Text>();
        value_life = infoT.Find("Value_Life").GetComponent<Text>();
        value_attack = infoT.Find("Value_Attack").GetComponent<Text>();
        value_skill = infoT.Find("Value_Skill").GetComponent<Text>();
        value_luck = infoT.Find("Value_Luck").GetComponent<Text>();

        Transform inventario = transform.Find("Panel_Inventario").Find("Inventario");
        inventarioT = inventario.Find("Scroll").Find("Botones_Items");
        //buttonPrefab = inventarioT.Find("Button").gameObject;
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
        Item item = Items.instance.GetItem(id);

        Sprite itemSprite = null;
        yield return Items.instance.ItemSpriteByID(item.ID, result => itemSprite = result);
        string textRarity = Lenguaje.Instance.Text_RarityID(item.rarity);
        rarity_text.text = textRarity;
        rarity_shadow.text = textRarity;
        rarity_text.color = ColorByQuality(item.rarity);
        retrato.sprite = itemSprite;

        value_attack.text = item.attack.ToString();
        value_life.text = item.health.ToString();
        value_skill.text = item.skill.ToString();
        value_luck.text = item.luck.ToString();

        skill1_orb.sprite = Items.instance.SphereBySkillID(item.skill_1.ID);
        skill2_orb.sprite = Items.instance.SphereBySkillID(item.skill_2.ID);
        skill3_orb.sprite = Items.instance.SphereBySkillID(item.skill_3.ID);

        if (Lenguaje.Instance.spanish_language)
        {
            skill1_name.text = item.skill_1.name_spanish;
            skill2_name.text = item.skill_2.name_spanish;
            skill3_name.text = item.skill_3.name_spanish;
        }
        else
        {
            skill1_name.text = item.skill_1.name_english;
            skill2_name.text = item.skill_2.name_english;
            skill3_name.text = item.skill_3.name_english;
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

    IEnumerator VisualizarScroll(Equip_Position equipList)
    {
        if(equipList != currentEquipView)
        {

            string equipedID = "";

            foreach(Transform t in inventarioT) //Limpiar casillas previas
            {
                Destroy(t.gameObject);
            }

            switch (equipList)
            {
                case Equip_Position.Head: actualList = SortListByQuality(Items.instance.inventory_headgear);
                    equipedID = GameManager.instance.player.criatura.equipment.head.ID_string; break;
                case Equip_Position.Body: actualList = SortListByQuality(Items.instance.inventory_bodies);
                    equipedID = GameManager.instance.player.criatura.equipment.body.ID_string; break;
                case Equip_Position.Arms: actualList = SortListByQuality(Items.instance.inventory_arms);
                    equipedID = GameManager.instance.player.criatura.equipment.arms.ID_string; break;
                case Equip_Position.Legs: actualList = SortListByQuality(Items.instance.inventory_legs);
                    equipedID = GameManager.instance.player.criatura.equipment.legs.ID_string; break;
            }
            storage_ID = new string[actualList.Count];
            int n = 0;
            foreach (Equipable_Item e in actualList)
            {
                yield return Colocar_Pieza_Slider(e.ID_string, n);
                n++;
            }
            if(n > 1)
            {
                float distance = Vector3.Distance(inventarioT.GetChild(0).localPosition, inventarioT.GetChild(1).localPosition);
                clampLeft = -distance * n;
            }else
            {
                clampLeft = 0;
            }

            currentEquipView = equipList;
        }

    }


    IEnumerator Colocar_Pieza_Slider(string itemID, int n)
    {
        GameObject go = Instantiate(buttonPrefab, inventarioT);
        Quality itemQuality = Quality.Common;
        int rarity = int.Parse(itemID.Substring(3, 1));

        switch (rarity)
        {
            case 1: itemQuality = Quality.Common; break;
            case 2: itemQuality = Quality.Rare; break;
            case 3: itemQuality = Quality.Epic; break;
            case 4: itemQuality = Quality.Legendary; break;
        }

        go.GetComponent<Image>().color = ColorByQuality(itemQuality);
        go.name = n.ToString();
        yield return Items.instance.ItemSpriteByID(int.Parse(itemID.Substring(0,3)), result => {
            go.transform.Find("Image").GetComponent<Image>().sprite = result;
        });
        Vector3 pos0 = new Vector3(0, 0, 0);
        Vector3 posPlus = new Vector3(distanciaItemsScrollBar * n, 0, 0);
        go.transform.localPosition = pos0 + posPlus;
        go.GetComponent<Button>().onClick.AddListener(BTN_ITEM);
        storage_ID[n] = itemID;
        go.SetActive(true);
    }

 

    public void BTN_HEAD()
    {
        StartCoroutine(VisualizarScroll(Equip_Position.Head));
        CanvasBase.instance.ShowItemInfo(GameManager.instance.player.criatura.equipment.head.ID_string);
    }

    public void BTN_BODY()
    {
        StartCoroutine(VisualizarScroll(Equip_Position.Body));
        CanvasBase.instance.ShowItemInfo(GameManager.instance.player.criatura.equipment.body.ID_string);
    }

    public void BTN_ARMS()
    {
        StartCoroutine(VisualizarScroll(Equip_Position.Arms));
        CanvasBase.instance.ShowItemInfo(GameManager.instance.player.criatura.equipment.arms.ID_string);
    }

    public void BTN_LEGS()
    {
        StartCoroutine(VisualizarScroll(Equip_Position.Legs));
        CanvasBase.instance.ShowItemInfo(GameManager.instance.player.criatura.equipment.legs.ID_string);
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
                CanvasBase.instance.StatsRefresh();
                //MarcarEquipado(id);
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

    void Update()
    {
        if (gameObject.activeSelf && !Input.GetMouseButton(0))
        {
            Vector3 vectorLimite = new Vector3(Mathf.Clamp(inventarioT.localPosition.x, clampLeft, 10), inventarioT.localPosition.y, inventarioT.localPosition.z);
            inventarioT.localPosition = Vector3.Lerp(inventarioT.localPosition, vectorLimite, Time.deltaTime * 10);
        }
    }

}
