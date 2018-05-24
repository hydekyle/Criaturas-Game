using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofres_System : MonoBehaviour {

    Transform cofre;

    void OnEnable()
    {
        cofre = cofre ?? transform.Find("Chest");
        cofre.GetComponent<Animator>().Play("Chest_Caer");
    }

	public void VOLVER()
    {
        transform.gameObject.SetActive(false);
        CanvasBase.instance.BackToMenu();
    }
}
