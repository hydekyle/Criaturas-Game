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

    void Start()
    {
        jugador1 = GameManager.instance.player;
        jugador2 = ConstruirIA();
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
                    head = Items.instance.ItemByID(101000),
                    body = Items.instance.ItemByID(202000),
                    arms = Items.instance.ItemByID(303000),
                    legs = Items.instance.ItemByID(404000),
                    back = Items.instance.ItemByID(500000)
                }
            }
        };
    }
}
