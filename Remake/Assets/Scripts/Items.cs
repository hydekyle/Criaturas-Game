using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {

    public static Items Instance;
    public List<Equipable_Item> headgear_list;
    public List<Equipable_Item> bodies_list;
    public List<Equipable_Item> arms_list;
    public List<Equipable_Item> legs_list;
    public List<Equipable_Item> backs_list;
    public List<Equipable_Item> weapons_list;

    void Awake()
    {
        Instance = this;
        Equipable_Item e = ItemByID(601);
        print(e.nombre);
    }

    public Equipable_Item ItemByID(int id)
    {
        Equipable_Item item = new Equipable_Item();
        int listID;
        int itemID;
        int.TryParse(id.ToString().Substring(0, 1), out listID);
        int.TryParse(id.ToString().Substring(1, 2), out itemID);
        switch (listID)
        {
            case 1: item = headgear_list[itemID];
                break;
            case 2: item = bodies_list[itemID];
                break;
            case 3: item = arms_list[itemID];
                break;
            case 4: item = legs_list[itemID];
                break;
            case 5: item = backs_list[itemID];
                break;
            case 6: item = weapons_list[itemID];
                break;
        }
        return item;
    }
}
