using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.IO;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public Player player;
    

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ConstruirJugador();
    }

    void ConstruirJugador()  //TEST MODE
    {
        if (File.Exists(Application.persistentDataPath + "/Equip_Setting.txt"))
        {
            Equipment e = new Equipment();
            e = Database.instance.ObtenerEquipSetting();
            player = new Player()
            {
                nombre = "Hyde",
                ID = "666",
                criatura = new Criatura()
                {
                    nombre = "Hyde Criatura",
                    equipment = new Equipment()
                    {
                        head = Items.instance.ItemByID(e.head.ID),
                        body = Items.instance.ItemByID(e.body.ID),
                        arms = Items.instance.ItemByID(e.arms.ID),
                        legs = Items.instance.ItemByID(e.legs.ID),
                        back = Items.instance.ItemByID(e.back.ID)
                    }
                }
            };
        }
        else
        {
            player = new Player()
            {
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
        }
        StartCoroutine(MostrarJugador(player, 1, new Vector3(120, 40, 0), false));
    }

    public IEnumerator MostrarJugador(Player playerP, int visorN, Vector3 visorPosition, bool flip)
    {
        StartCoroutine(Menu.instance.VisualizarEquipamiento(playerP.criatura.equipment, visorN));
        yield return null;
        Menu.instance.InitializeVisor(Menu.instance.GetPlayerVisor(visorN), visorPosition, flip);
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

    }

    void AdelanteHeadgear()
    {
        if (Items.instance.ItemByID(player.criatura.equipment.head.ID + 1).ID != 0)
        {
            player.criatura.equipment.head = Items.instance.ItemByID(player.criatura.equipment.head.ID + 1);
            StartCoroutine(Menu.instance.VisualizarEquipamiento(player.criatura.equipment, 1));
        }
    }

    void AdelanteBody()
    {
        if (Items.instance.ItemByID(player.criatura.equipment.body.ID + 1).ID != 0)
        {
            player.criatura.equipment.body = Items.instance.ItemByID(player.criatura.equipment.body.ID + 1);
            StartCoroutine(Menu.instance.VisualizarEquipamiento(player.criatura.equipment, 1));
        }
    }

    void AdelanteArms()
    {
        if (Items.instance.ItemByID(player.criatura.equipment.arms.ID + 1).ID != 0)
        {
            player.criatura.equipment.arms = Items.instance.ItemByID(player.criatura.equipment.arms.ID + 1);
            StartCoroutine(Menu.instance.VisualizarEquipamiento(player.criatura.equipment, 1));
        }
        print(player.criatura.equipment.arms.ID);
    }

    void AdelanteLegs()
    {
        if (Items.instance.ItemByID(player.criatura.equipment.legs.ID + 1).ID != 0)
        {
            player.criatura.equipment.legs = Items.instance.ItemByID(player.criatura.equipment.legs.ID + 1);
            StartCoroutine(Menu.instance.VisualizarEquipamiento(player.criatura.equipment, 1));
            print(player.criatura.equipment.legs.ID);
        }
    }

    void AdelanteBack()
    {
        if (Items.instance.ItemByID(player.criatura.equipment.back.ID + 1).ID != 0)
        {
            player.criatura.equipment.back = Items.instance.ItemByID(player.criatura.equipment.back.ID + 1);
            StartCoroutine(Menu.instance.VisualizarEquipamiento(player.criatura.equipment, 1));
        }
    }


}
