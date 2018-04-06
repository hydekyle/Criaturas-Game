using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBase : MonoBehaviour {

    Transform start_menu;
    Transform equipment;
    Transform battleIA;

    void Start()
    {
        start_menu = transform.Find("Start_Menu");
        equipment = transform.Find("Equipamiento");
        battleIA = transform.Find("BattleIA");
    }

    public void BTN_EQUIP()
    {
        equipment.gameObject.SetActive(true);
        start_menu.gameObject.SetActive(false);
    }

	public void BTN_VERSUS()
    {
        battleIA.gameObject.SetActive(!battleIA.gameObject.activeSelf);
        start_menu.gameObject.SetActive(false);
    }

    public void BTN_OK_Equipment()
    {
        Database.instance.GuardarEquipSetting(GameManager.instance.player.criatura.equipment);
        equipment.gameObject.SetActive(false);
        start_menu.gameObject.SetActive(true);
    }
}
