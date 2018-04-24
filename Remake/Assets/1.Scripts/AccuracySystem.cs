using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracySystem : MonoBehaviour {

    public static AccuracySystem instance;


    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Iniciar();
        }
    }

    public void Iniciar()
    {
        transform.Find("LunaGame").gameObject.SetActive(true);
    }
}
