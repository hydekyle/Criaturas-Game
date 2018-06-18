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

}
