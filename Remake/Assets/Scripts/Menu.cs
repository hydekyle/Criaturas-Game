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
            case 1: visor_headgear.sprite = sprite; FixHeadScale(id); break;
            case 2: visor_body.sprite = sprite; ColocarBody(id); break;
            case 3: visor_arm_left.sprite = visor_arm_right.sprite = sprite; FixArmScale(id); break;
            case 4: visor_leg_left.sprite = visor_leg_right.sprite = sprite; break;
            case 5: visor_back.sprite = sprite; break;
        }
        
    }

    void FixHeadScale(int bigID)
    {
        int id = int.Parse(bigID.ToString().Substring(0, 3));
        print(id);
        if      (id == 101) { visor_headgear.rectTransform.localScale = new Vector3(0.8f, 0.9f, 1); }
        else if (id == 102) { visor_headgear.rectTransform.localScale = new Vector3(0.8f, 0.95f, 1); }
        else if (id == 104) { visor_headgear.rectTransform.localScale = new Vector3(0.8f, 1, 1); }
        else if (id == 105) { visor_headgear.rectTransform.localScale = new Vector3(0.9f, 0.82f, 1); }
        else if (id == 106) { visor_headgear.rectTransform.localScale = new Vector3(0.9f, 0.7f, 1); }
        else if (id == 107) { visor_headgear.rectTransform.localScale = new Vector3(1.3f, 0.65f, 1); }
        else if (id == 108) { visor_headgear.rectTransform.localScale = new Vector3(0.8f, 1.4f, 1); }
        else if (id == 109) { visor_headgear.rectTransform.localScale = new Vector3(1, 0.8f, 1); }
        else if (id == 110) { visor_headgear.rectTransform.localScale = new Vector3(0.76f, 0.76f, 1); }
        else if (id == 111) { visor_headgear.rectTransform.localScale = new Vector3(0.9f, 0.8f, 1); }
        else if (id == 112) { visor_headgear.rectTransform.localScale = new Vector3(0.74f, 0.8f, 1); }
        else if (id == 113) { visor_headgear.rectTransform.localScale = new Vector3(0.7f, 0.7f, 1); }
        else if (id == 115) { visor_headgear.rectTransform.localScale = new Vector3(0.75f, 0.8f, 1); }
        else if (id == 118) { visor_headgear.rectTransform.localScale = new Vector3(1, 0.8f, 1); }
        else if (id == 119) { visor_headgear.rectTransform.localScale = new Vector3(0.7f, 0.8f, 1); }
        else if (id == 120) { visor_headgear.rectTransform.localScale = new Vector3(0.8f, 0.9f, 1); }
        else if (id == 121) { visor_headgear.rectTransform.localScale = new Vector3(0.9f, 0.7f, 1); }
        else if (id == 122) { visor_headgear.rectTransform.localScale = new Vector3(0.7f, 1f, 1); }
        else if (id == 123) { visor_headgear.rectTransform.localScale = new Vector3(1, 0.7f, 1); }
        else if (id == 126) { visor_headgear.rectTransform.localScale = new Vector3(1.1f, 0.9f, 1); }
        else if (id == 127) { visor_headgear.rectTransform.localScale = new Vector3(0.9f, 0.9f, 1); }
        else if (id == 129) { visor_headgear.rectTransform.localScale = new Vector3(0.65f, 0.8f, 1); }
        else if (id == 130) { visor_headgear.rectTransform.localScale = new Vector3(0.86f, 0.7f, 1); }
        else if (id == 131) { visor_headgear.rectTransform.localScale = new Vector3(1, 1.6f, 1); }
        else if (id == 133) { visor_headgear.rectTransform.localScale = new Vector3(0.75f, 0.75f, 1); }
        else if (id == 134) { visor_headgear.rectTransform.localScale = new Vector3(0.87f, 1, 1); }
        else if (id == 135) { visor_headgear.rectTransform.localScale = new Vector3(0.75f, 0.8f, 1); }
        else if (id == 137) { visor_headgear.rectTransform.localScale = new Vector3(1, 0.8f, 1); }
        else if (id == 138) { visor_headgear.rectTransform.localScale = new Vector3(0.8f, 1.5f, 1); }
        else if (id == 139) { visor_headgear.rectTransform.localScale = new Vector3(1, 0.9f, 1); }
        else if (id == 141) { visor_headgear.rectTransform.localScale = new Vector3(1.1f, 1.8f, 1); }
        else if (id == 142) { visor_headgear.rectTransform.localScale = new Vector3(0.7f, 0.7f, 1); }
        else if (id == 143) { visor_headgear.rectTransform.localScale = new Vector3(0.74f, 0.74f, 1); }
        else if (id == 144) { visor_headgear.rectTransform.localScale = new Vector3(0.9f, 0.9f, 1); }

        else { visor_headgear.rectTransform.localScale = new Vector3(0.8f, 0.8f, 1); }
            
    }

    void FixArmScale(int bigID)
    {
        int id = int.Parse(bigID.ToString().Substring(0, 3));
        if (id == 301) visor_arm_left.rectTransform.localScale = visor_arm_right.rectTransform.localScale = new Vector3(0,0,0);
    }

    void ColocarBody(int bigID)
    {
        int id = int.Parse(bigID.ToString().Substring(0, 3));
        ColocarPiezas(Database.instance.LeerBodyBounds(id));
    }

    void ColocarPiezas(BodyBounds bounds)
    {
        visor_headgear.rectTransform.position = bounds.head_POS;
        visor_arm_right.rectTransform.position = bounds.arm_right_POS;
        visor_arm_left.rectTransform.position = bounds.arm_left_POS;
        visor_leg_right.rectTransform.position = bounds.leg_right_POS;
        visor_leg_left.rectTransform.position = bounds.leg_left_POS;
        visor_back.rectTransform.position = bounds.back_POS;
    }

    void SaveBounds()
    {
        BodyBounds bounds = new BodyBounds()
        {
            head_POS = visor_headgear.rectTransform.position,
            arm_left_POS = visor_arm_left.rectTransform.position,
            arm_right_POS = visor_arm_right.rectTransform.position,
            leg_left_POS = visor_leg_left.rectTransform.position,
            leg_right_POS = visor_leg_right.rectTransform.position,
            back_POS = visor_back.rectTransform.position
        };
        Database.instance.GuardarBodyBounds(bounds);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SaveBounds();
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
