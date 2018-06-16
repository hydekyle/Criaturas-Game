using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public Player player;
    public UserDB userdb;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
        
    }

    public IEnumerator MostrarJugador(Player playerP, int visorN, Vector3 visorPosition, bool flip)
    {
        Menu.instance.InitializeVisor(Menu.instance.GetPlayerVisor(visorN), visorPosition, flip);
        yield return null;
        StartCoroutine(Menu.instance.VisualizarEquipamiento(playerP.criatura.equipment, visorN));
        StartCoroutine(CanvasBase.instance.MostrarJugador());
    }

    public void ErrorGeneral()
    {
        Debug.LogError("Ha ocurrido un error grave");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) CanvasBase.instance.SetCamTarget(Posi(0));
        if (Input.GetKeyDown(KeyCode.O)) CanvasBase.instance.SetCamTarget(Posi(1));
        if (Input.GetKeyDown(KeyCode.P)) CanvasBase.instance.SetCamTarget(Posi(2));
    }

    Vector3 Posi(int n)
    {
        Vector3 v = Vector3.zero;

        switch (n)
        {
            case 0: v = Menu.instance.visor_player1.body.transform.position; break;
            case 1: v = Menu.instance.visor_player2.body.transform.position; break;
            case 2: v = Menu.instance.visor_player2.leg_left.transform.position; break;
        }


        return v;
    }
}
