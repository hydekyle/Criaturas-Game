using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BattleIA : MonoBehaviour {



    /* 
     * Comienza pelea. Se cargan estadísticas y se decide quién comienza.
     * Comienza turno. (Check condiciones pre-turno: efectos aplicados, cargar nuevas habilidades, +1 de maná, check habilidades usables).
     * 
     * 
     * 
     *      
     */

    Player jugador1;
    Player jugador2;

    void OnEnable()
    {
        jugador1 = GameManager.instance.player;
        jugador2 = ConstruirIA();
        StartCoroutine(Menu.instance.VisualizarEquipamiento(jugador2.criatura.equipment, 2));
        Menu.instance.InitializeVisor(Menu.instance.GetPlayerVisor(2), new Vector3(120,0,0), false);
    }



    Player ConstruirIA()
    {
        return new Player()
        {
            nombre = "Dios",
            ID = "0000",
            criatura = new Criatura()
            {
                nombre = "Jesus Christ",
                equipment = new Equipment()
                {
                    head = Items.instance.ItemByID(GetRandomItemID(1)),
                    body = Items.instance.ItemByID(GetRandomItemID(2)),
                    arms = Items.instance.ItemByID(GetRandomItemID(3)),
                    legs = Items.instance.ItemByID(GetRandomItemID(4)),
                    back = Items.instance.ItemByID(GetRandomItemID(5))
                }
            }
        };
    }

    Equipment GetRandomEquip()
    {
        Equipment e = new Equipment()
        {

        };
        return e;
    }

    int GetRandomItemID(int listNumber)
    {
        string randomID = "";
        switch (listNumber)
        {
            case 1: randomID = Random.Range(0, Items.instance.headgear_list.Count).ToString(); break;
            case 2: randomID = Random.Range(0, Items.instance.bodies_list.Count).ToString(); break;
            case 3: randomID = Random.Range(0, Items.instance.arms_list.Count).ToString(); break;
            case 4: randomID = Random.Range(0, Items.instance.legs_list.Count).ToString(); break;
            case 5: randomID = Random.Range(0, Items.instance.backs_list.Count).ToString(); break;
        }
        if (randomID.ToString().Length < 2) randomID = "0" + randomID;
        string final = listNumber.ToString() + randomID + "000";
        return int.Parse(final);
    }

}
