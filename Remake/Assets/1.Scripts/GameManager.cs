using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.IO;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public Player player;

    [SerializeField]
    public UserDB usuario;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //ConstruirJugador();
    }

    public void ConstruirJugador()  //TEST MODE
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
                attack_att = 10,
                defense_att = 10,
                luck_att = 10,
                skill_att = 10,
                equipment = e
            }
        };

        Menu.instance.loadedEquipment = new Equipment() {
            head = new Equipable_Item() {
                ID = 0
            },
            arms = new Equipable_Item()
            {
                ID = 0
            },
            back = new Equipable_Item()
            {
                ID = 0
            },
            body = new Equipable_Item()
            {
                ID = 0
            },
            legs = new Equipable_Item()
            {
                ID = 0
            }
        };
        StartCoroutine(MostrarJugador(player, 1, new Vector3(10, 66, -1), false));
    }

    public IEnumerator MostrarJugador(Player playerP, int visorN, Vector3 visorPosition, bool flip)
    {
        Menu.instance.InitializeVisor(Menu.instance.GetPlayerVisor(visorN), visorPosition, flip);
        yield return null;
        StartCoroutine(Menu.instance.VisualizarEquipamiento(playerP.criatura.equipment, visorN));
        
    }

    public void ErrorGeneral()
    {
        Debug.LogError("Ha ocurrido un error grave");
    }
}
