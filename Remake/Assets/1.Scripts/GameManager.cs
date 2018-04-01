using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public Player player;
    public List<GiveStat> totalStats_list = new List<GiveStat>();

    void Awake()
    {
        instance = this;
        ConstruirJugador();
    }

    void Start()
    {
        
    }

    void ConstruirJugador()  //TEST MODE
    {
        player = new Player() {
            nombre = "Hyde",
            ID = "666",
            criatura = new Criatura()
            {
                nombre = "Hyde Criatura",
                equipment = new Equipment()
                {
                    head = Items.instance.ItemByID(100000),
                    body = Items.instance.ItemByID(200000),
                    arms = Items.instance.ItemByID(300000),
                    legs = Items.instance.ItemByID(400000),
                    back = Items.instance.ItemByID(500000)
                }
            }

        };
        StartCoroutine(VisualizarEquipamiento(1));
        LeerStats();
    }

    IEnumerator VisualizarEquipamiento(int playerNumber)
    {
        yield return StartCoroutine(Menu.instance.SendImage(player.criatura.equipment.head.ID, Menu.instance.GetPlayerVisor(playerNumber)));
        yield return StartCoroutine(Menu.instance.SendImage(player.criatura.equipment.body.ID, Menu.instance.GetPlayerVisor(playerNumber)));
        yield return StartCoroutine(Menu.instance.SendImage(player.criatura.equipment.arms.ID, Menu.instance.GetPlayerVisor(playerNumber)));
        yield return StartCoroutine(Menu.instance.SendImage(player.criatura.equipment.legs.ID, Menu.instance.GetPlayerVisor(playerNumber)));
        yield return StartCoroutine(Menu.instance.SendImage(player.criatura.equipment.back.ID, Menu.instance.GetPlayerVisor(playerNumber)));
    }

    public void LeerStats()
    {
        totalStats_list = new List<GiveStat>();
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
        lista.Add(player.criatura.equipment.head);
        lista.Add(player.criatura.equipment.body);
        lista.Add(player.criatura.equipment.arms);
        lista.Add(player.criatura.equipment.legs);
        lista.Add(player.criatura.equipment.back);
        return lista;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            AdelanteHeadgear();
            LeerStats();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            AdelanteBody();
            LeerStats();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            AdelanteArms();
            LeerStats();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            AdelanteLegs();
            LeerStats();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AdelanteBack();
            LeerStats();
        }

    }

    void AdelanteHeadgear()
    {
        if (Items.instance.ItemByID(player.criatura.equipment.head.ID + 1).ID != 0)
        {
            player.criatura.equipment.head = Items.instance.ItemByID(player.criatura.equipment.head.ID + 1);
            StartCoroutine(VisualizarEquipamiento(1));
        }
    }

    void AdelanteBody()
    {
        if (Items.instance.ItemByID(player.criatura.equipment.body.ID + 1).ID != 0)
        {
            player.criatura.equipment.body = Items.instance.ItemByID(player.criatura.equipment.body.ID + 1);
            StartCoroutine(VisualizarEquipamiento(1));
        }
    }

    void AdelanteArms()
    {
        if (Items.instance.ItemByID(player.criatura.equipment.arms.ID + 1).ID != 0)
        {
            player.criatura.equipment.arms = Items.instance.ItemByID(player.criatura.equipment.arms.ID + 1);
            StartCoroutine(VisualizarEquipamiento(1));
        }
    }

    void AdelanteLegs()
    {
        if (Items.instance.ItemByID(player.criatura.equipment.legs.ID + 1).ID != 0)
        {
            player.criatura.equipment.legs = Items.instance.ItemByID(player.criatura.equipment.legs.ID + 1);
            StartCoroutine(VisualizarEquipamiento(1));
        }
    }

    void AdelanteBack()
    {
        if (Items.instance.ItemByID(player.criatura.equipment.back.ID + 1).ID != 0)
        {
            player.criatura.equipment.back = Items.instance.ItemByID(player.criatura.equipment.back.ID + 1);
            StartCoroutine(VisualizarEquipamiento(1));
        }
    }


}
