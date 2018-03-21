using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Menu : MonoBehaviour {

    public static Menu instance;
    Image background;
    bool b = true;
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
        visor_headgear = visor.Find("HEADGEAR").GetComponent<Image>();
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FadeBackground(b);
            b = !b;
        }
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

    public void SetImageVisor(Sprite sprite, int listNumber)
    {
        switch (listNumber)
        {
            case 1: visor_headgear.sprite = sprite; break;
            case 2: visor_body.sprite = sprite; break;
            case 3: visor_arm_left.sprite = visor_arm_right.sprite = sprite; break;
            case 4: visor_leg_left.sprite = visor_leg_right.sprite = sprite; break;
            case 5: visor_back.sprite = sprite; break;
            case 6: visor_weapon.sprite = sprite; break;
        }
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
