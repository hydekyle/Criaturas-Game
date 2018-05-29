using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public Player player;
    bool b;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
        
    }

    void Start()
    {
        //ConstruirJugador();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (b)
            {
                SceneManager.LoadScene(0);
            }else
            {
                SceneManager.LoadScene(1);
            }
            b = !b;
        }
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
