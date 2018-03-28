using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Menu : MonoBehaviour {

    public static Menu instance;
    Image background;
    Image visor_headgear;
    Image visor_body;
    Image visor_back;
    Image visor_arm_left;
    Image visor_arm_right;
    Image visor_leg_left;
    Image visor_leg_right;
    Image visor_weapon;

    void Awake()
    {
        instance = this;
        Initialize();
    }

    void Initialize()
    {
        background = transform.Find("Background").GetComponent<Image>();
        Transform visor = transform.Find("VISOR");
        visor_headgear = visor.Find("BODY").Find("HEADGEAR").GetComponent<Image>();
        visor_body = visor.Find("BODY").GetComponent<Image>();
        visor_back = visor.Find("BACK").GetComponent<Image>();
        visor_arm_left = visor.Find("ARM_LEFT").GetComponent<Image>();
        visor_arm_right = visor.Find("ARM_RIGHT").GetComponent<Image>();
        visor_leg_left = visor.Find("LEG_LEFT").GetComponent<Image>();
        visor_leg_right = visor.Find("LEG_RIGHT").GetComponent<Image>();
        visor_weapon = visor.Find("WEAPON").GetComponent<Image>();
    }


    void Start()
    {
        SetStringsMainMenu();
    }


    void FadeBackground(bool fadeState)
    {
        Color lerpTo = fadeState ? Color.grey : Color.white;
        background.color = lerpTo;
    }

    void SetStringsMainMenu()
    {
        Transform mainMenu_transform = transform.Find("MAIN_MENU");
        mainMenu_transform.Find("play_BTN").Find("Text").GetComponent<Text>().text = Lenguaje.Instance.play;
        mainMenu_transform.Find("nest_BTN").Find("Text").GetComponent<Text>().text = Lenguaje.Instance.nest;
        mainMenu_transform.Find("shop_BTN").Find("Text").GetComponent<Text>().text = Lenguaje.Instance.shop;
    }

    public void SetImageVisor(Sprite sprite, int id)
    {
        int listNumber = int.Parse(id.ToString().Substring(0, 1));
        switch (listNumber)
        {
            case 1: visor_headgear.sprite = sprite; break;
            case 2: visor_body.sprite = sprite; ColocarBody(id); break;
            case 3: visor_arm_left.sprite = visor_arm_right.sprite = sprite; break;
            case 4: visor_leg_left.sprite = visor_leg_right.sprite = sprite; break;
            case 5: visor_back.sprite = sprite; break;
            case 6: visor_weapon.sprite = sprite; break;
        }
        
    }

    void ColocarBody(int bigID)
    {
        int id = int.Parse(bigID.ToString().Substring(0, 3));
        switch (id)
        {
            case 200: ColocarPiezas(new Vector3(-11, 67, 0)) ; break;
            case 201: ColocarPiezas(new Vector3(-23, 29, 0)); break;
            case 202: ColocarPiezas(new Vector3(6, 67, 0)); break;
            case 203: ColocarPiezas(new Vector3(8, 66, 0)); break;
            case 204: ColocarPiezas(new Vector3(0, 64, 0)); break;
            case 205: ColocarPiezas(new Vector3(0, 64, 0)); break;
            case 206: ColocarPiezas(new Vector3(-2, 68, 0)); break;
            case 207: ColocarPiezas(new Vector3(-2, 62, 0)); break;
            case 208: ColocarPiezas(new Vector3(-5, 65, 0)); break;
            case 209: ColocarPiezas(new Vector3(-8, 66, 0)); break;
            case 210: ColocarPiezas(new Vector3(-10, 68, 0)); break;
            case 211: ColocarPiezas(new Vector3(-3, 66, 0)); break;
            case 212: ColocarPiezas(new Vector3(-3, 64, 0)); break;
            case 213: ColocarPiezas(new Vector3(-1, 64, 0)); break;
            case 214: ColocarPiezas(new Vector3(-3, 67, 0)); break;
            case 215: ColocarPiezas(new Vector3(-3, 62, 0)); break;
            case 216: ColocarPiezas(new Vector3(-3, 62, 0)); break;
            case 217: ColocarPiezas(new Vector3(-25, 70, 0)); break;
            case 218: ColocarPiezas(new Vector3(-3, 65, 0)); break;
            case 219: ColocarPiezas(new Vector3(-3, 65, 0)); break;
            case 220: ColocarPiezas(new Vector3(-6, 60, 0)); break;
            case 221: ColocarPiezas(new Vector3(-1, 60, 0)); break;
            case 222: ColocarPiezas(new Vector3(-10, 65, 0)); break;
            case 223: ColocarPiezas(new Vector3(-16, 60, 0)); break;
            case 224: ColocarPiezas(new Vector3(-3, 61, 0)); break;
            case 225: ColocarPiezas(new Vector3(-21, 63, 0)); break;
            case 226: ColocarPiezas(new Vector3(-3, 62, 0)); break;
            case 227: ColocarPiezas(new Vector3(-3, 73, 0)); break;
            case 228: ColocarPiezas(new Vector3(1, 68, 0)); break;
            case 229: ColocarPiezas(new Vector3(-16, 62, 0)); break;
            case 230: ColocarPiezas(new Vector3(9, 67, 0)); break;
            case 231: ColocarPiezas(new Vector3(-4, 64, 0)); break;
            case 232: ColocarPiezas(new Vector3(-5, 71, 0)); break;



        }
        print(id);
    }

    void ColocarPiezas(Vector3 posiHead)
    {
        visor_headgear.rectTransform.localPosition = posiHead;
    }


    public void BTN_Play()
    {
        print("play");
    }

    public void BTN_Nest()
    {
        print("nest");
    }

    public void BTN_Shop()
    {
        print("shop");
    }

}
