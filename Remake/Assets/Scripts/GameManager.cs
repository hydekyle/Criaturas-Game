using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public Player player;
    public Equipment player_equipment;
    public List<GiveStat> totalStats_list = new List<GiveStat>();

    void Start()
    {
        instance = this;
        player_equipment = new Equipment()
        {
            head = Items.instance.ItemByID(100000),
            body = Items.instance.ItemByID(200000),
            arms = Items.instance.ItemByID(300000),
            legs = Items.instance.ItemByID(400000),
            back = Items.instance.ItemByID(500000),
            weapon = Items.instance.ItemByID(600366)
        };
        LeerStats();
        StartCoroutine(VisualizarEquipamiento());
    }

    IEnumerator VisualizarEquipamiento()
    {
        yield return StartCoroutine(Items.instance.SendImage(player_equipment.head.ID));
        yield return StartCoroutine(Items.instance.SendImage(player_equipment.body.ID));
        yield return StartCoroutine(Items.instance.SendImage(player_equipment.arms.ID));
        yield return StartCoroutine(Items.instance.SendImage(player_equipment.legs.ID));
        yield return StartCoroutine(Items.instance.SendImage(player_equipment.back.ID));
        yield return StartCoroutine(Items.instance.SendImage(player_equipment.weapon.ID));
    }

    void LeerStats()
    {
        List<Equipable_Item> equipamiento_list = ObtenerListaEquipamiento();
        foreach(Equipable_Item e in equipamiento_list)
        {
            for(var x = 0; x < e.addStat.Count; x++)
            {
                totalStats_list.Add(e.addStat[x]);
            }
        }
        
    }

    List<Equipable_Item> ObtenerListaEquipamiento()
    {
        List<Equipable_Item> lista = new List<Equipable_Item>();
        lista.Add(player_equipment.head);
        lista.Add(player_equipment.body);
        lista.Add(player_equipment.arms);
        lista.Add(player_equipment.legs);
        lista.Add(player_equipment.back);
        lista.Add(player_equipment.weapon);
        return lista;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            AdelanteHeadgear();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            AdelanteBody();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            AdelanteArms();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            AdelanteLegs();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AdelanteBack();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AdelanteWeapon();
        }

    }

    void AdelanteHeadgear()
    {
        if (Items.instance.ItemByID(player_equipment.head.ID + 1).ID != 0)
        {
            player_equipment.head = Items.instance.ItemByID(player_equipment.head.ID + 1);
            StartCoroutine(VisualizarEquipamiento());
        }
    }

    void AdelanteBody()
    {
        if (Items.instance.ItemByID(player_equipment.body.ID + 1).ID != 0)
        {
            player_equipment.body = Items.instance.ItemByID(player_equipment.body.ID + 1);
            StartCoroutine(VisualizarEquipamiento());
        }
    }

    void AdelanteArms()
    {
        if (Items.instance.ItemByID(player_equipment.arms.ID + 1).ID != 0)
        {
            player_equipment.arms = Items.instance.ItemByID(player_equipment.arms.ID + 1);
            StartCoroutine(VisualizarEquipamiento());
        }
    }

    void AdelanteLegs()
    {
        if (Items.instance.ItemByID(player_equipment.legs.ID + 1).ID != 0)
        {
            player_equipment.legs = Items.instance.ItemByID(player_equipment.legs.ID + 1);
            StartCoroutine(VisualizarEquipamiento());
        }
    }

    void AdelanteBack()
    {
        if (Items.instance.ItemByID(player_equipment.back.ID + 1).ID != 0)
        {
            player_equipment.back = Items.instance.ItemByID(player_equipment.back.ID + 1);
            StartCoroutine(VisualizarEquipamiento());
        }
    }

    void AdelanteWeapon()
    {
        if (Items.instance.ItemByID(player_equipment.weapon.ID + 1).ID != 0)
        {
            player_equipment.weapon = Items.instance.ItemByID(player_equipment.weapon.ID + 1);
            StartCoroutine(VisualizarEquipamiento());
        }
    }

}
