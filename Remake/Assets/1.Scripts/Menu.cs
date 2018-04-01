using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Menu : MonoBehaviour {

    public static Menu instance;
    Visor visor_player1;
    Visor visor_player2;
    Image background;

    void Awake()
    {
        instance = this;
        visor_player1 = new Visor();
        visor_player2 = new Visor();
        Initialize();
    }

    void Initialize()
    {
        background = transform.Find("Background").GetComponent<Image>();
        visor_player1.myTransform = transform.Find("VISOR");
        visor_player2.myTransform = transform.Find("VISOR2");
        
    }

    public void InitializeVisor(Visor visor, Vector3 visorPos, bool flip)
    {
        visor.myTransform.gameObject.SetActive(false);
        visor.myTransform.localPosition = Vector3.zero;
        visor.headgear = visor.myTransform.Find("HEADGEAR").GetComponent<Image>();
        visor.body = visor.myTransform.Find("BODY").GetComponent<Image>();
        visor.back = visor.myTransform.Find("BACK").GetComponent<Image>();
        visor.arm_left = visor.myTransform.Find("ARM_LEFT").GetComponent<Image>();
        visor.arm_right = visor.myTransform.Find("ARM_RIGHT").GetComponent<Image>();
        visor.leg_left = visor.myTransform.Find("LEG_LEFT").GetComponent<Image>();
        visor.leg_right = visor.myTransform.Find("LEG_RIGHT").GetComponent<Image>();
        StartCoroutine(Visualizar(visor.myTransform, visorPos, flip));
    }

    IEnumerator Visualizar(Transform visor, Vector3 localPos, bool flip)
    {
        yield return new WaitForSeconds(1);
        visor.localPosition = localPos;
        if (flip) visor.localRotation = Quaternion.Euler(0, 180, 0);
        visor.gameObject.SetActive(true);
    }

    void FadeBackground(bool fadeState)
    {
        Color lerpTo = fadeState ? Color.grey : Color.white;
        background.color = lerpTo;
    }

    public IEnumerator SendImage(int id, Visor visor)
    {
        Sprite spriteBuscado = null;
        yield return StartCoroutine(Items.instance.ItemSpriteByID(id, value => spriteBuscado = value));
        SetImageVisor(spriteBuscado, id, visor);
    }

    public IEnumerator VisualizarEquipamiento(Equipment e , int playerNumber)
    {
        yield return StartCoroutine(SendImage(e.head.ID, GetPlayerVisor(playerNumber)));
        yield return StartCoroutine(SendImage(e.body.ID, GetPlayerVisor(playerNumber)));
        yield return StartCoroutine(SendImage(e.arms.ID, GetPlayerVisor(playerNumber)));
        yield return StartCoroutine(SendImage(e.legs.ID, GetPlayerVisor(playerNumber)));
        yield return StartCoroutine(SendImage(e.back.ID, GetPlayerVisor(playerNumber)));
    }

    public void SetImageVisor(Sprite sprite, int id, Visor visor)
    {
        int listNumber = int.Parse(id.ToString().Substring(0, 1));
        switch (listNumber)
        {
            case 1: visor.headgear.sprite = sprite; FixHeadScale(id, visor); break;
            case 2: visor.body.sprite = sprite; ColocarBody(id, visor); break;
            case 3: visor.arm_left.sprite = visor.arm_right.sprite = sprite; FixArmScale(id, visor); break;
            case 4: visor.leg_left.sprite = visor.leg_right.sprite = sprite; break;
            case 5: visor.back.sprite = sprite; break;
        }
    }

    void FixHeadScale(int bigID, Visor visor)
    {
        int id = int.Parse(bigID.ToString().Substring(0, 3));
        Vector3 fixScale;
        if      (id == 101) { fixScale = new Vector3(0.8f, 0.9f, 1); }
        else if (id == 102) { fixScale = new Vector3(0.8f, 0.95f, 1); }
        else if (id == 104) { fixScale = new Vector3(0.8f, 1, 1); }
        else if (id == 105) { fixScale = new Vector3(0.9f, 0.82f, 1); }
        else if (id == 106) { fixScale = new Vector3(0.9f, 0.7f, 1); }
        else if (id == 107) { fixScale = new Vector3(1.3f, 0.65f, 1); }
        else if (id == 108) { fixScale = new Vector3(0.8f, 1.4f, 1); }
        else if (id == 109) { fixScale = new Vector3(1, 0.8f, 1); }
        else if (id == 110) { fixScale = new Vector3(0.76f, 0.76f, 1); }
        else if (id == 111) { fixScale = new Vector3(0.9f, 0.8f, 1); }
        else if (id == 112) { fixScale = new Vector3(0.74f, 0.8f, 1); }
        else if (id == 113) { fixScale = new Vector3(0.7f, 0.7f, 1); }
        else if (id == 115) { fixScale = new Vector3(0.75f, 0.8f, 1); }
        else if (id == 118) { fixScale = new Vector3(1, 0.8f, 1); }
        else if (id == 119) { fixScale = new Vector3(0.7f, 0.8f, 1); }
        else if (id == 120) { fixScale = new Vector3(0.8f, 0.9f, 1); }
        else if (id == 121) { fixScale = new Vector3(0.9f, 0.7f, 1); }
        else if (id == 122) { fixScale = new Vector3(0.7f, 1f, 1); }
        else if (id == 123) { fixScale = new Vector3(1, 0.7f, 1); }
        else if (id == 126) { fixScale = new Vector3(1.1f, 0.9f, 1); }
        else if (id == 127) { fixScale = new Vector3(0.9f, 0.9f, 1); }
        else if (id == 129) { fixScale = new Vector3(0.65f, 0.8f, 1); }
        else if (id == 130) { fixScale = new Vector3(0.86f, 0.7f, 1); }
        else if (id == 131) { fixScale = new Vector3(1, 1.6f, 1); }
        else if (id == 133) { fixScale = new Vector3(0.75f, 0.75f, 1); }
        else if (id == 134) { fixScale = new Vector3(0.87f, 1, 1); }
        else if (id == 135) { fixScale = new Vector3(0.75f, 0.8f, 1); }
        else if (id == 137) { fixScale = new Vector3(1, 0.8f, 1); }
        else if (id == 138) { fixScale = new Vector3(0.8f, 1.5f, 1); }
        else if (id == 139) { fixScale = new Vector3(1, 0.9f, 1); }
        else if (id == 141) { fixScale = new Vector3(1.1f, 1.8f, 1); }
        else if (id == 142) { fixScale = new Vector3(0.7f, 0.7f, 1); }
        else if (id == 143) { fixScale = new Vector3(0.74f, 0.74f, 1); }
        else if (id == 144) { fixScale = new Vector3(0.9f, 0.9f, 1); }
        else { fixScale = new Vector3(0.8f, 0.8f, 1); }

        visor.headgear.rectTransform.localScale = fixScale;



    }

    void FixArmScale(int bigID, Visor visor)
    {
        int id = int.Parse(bigID.ToString().Substring(0, 3));
        if (id == 399) visor.arm_left.rectTransform.localScale = visor.arm_right.rectTransform.localScale = new Vector3(0,0,0);
        else { visor.arm_left.rectTransform.localScale = visor.arm_right.rectTransform.localScale = Vector3.one; }
    }

    void ColocarBody(int bigID, Visor visor)
    {
        int id = int.Parse(bigID.ToString().Substring(0, 3));
        ColocarPiezas(Database.instance.LeerBodyBounds(id), visor);
    }

    private void ColocarPiezas(BodyBounds bounds, Visor visor)
    {
        visor.headgear.rectTransform.position = bounds.head_POS;
        visor.arm_right.rectTransform.position = bounds.arm_right_POS;
        visor.arm_left.rectTransform.position = bounds.arm_left_POS;
        visor.leg_right.rectTransform.position = bounds.leg_right_POS;
        visor.leg_left.rectTransform.position = bounds.leg_left_POS;
        visor.back.rectTransform.position = bounds.back_POS;
    }

    void SaveBounds()
    {
        BodyBounds bounds = new BodyBounds()
        {
            head_POS = visor_player1.headgear.rectTransform.position,
            arm_left_POS = visor_player1.arm_left.rectTransform.position,
            arm_right_POS = visor_player1.arm_right.rectTransform.position,
            leg_left_POS = visor_player1.leg_left.rectTransform.position,
            leg_right_POS = visor_player1.leg_right.rectTransform.position,
            back_POS = visor_player1.back.rectTransform.position
        };
        Database.instance.GuardarBodyBounds(bounds);
    }

    public Visor GetPlayerVisor(int playerNumber)
    {
        Visor visor;
        if (playerNumber == 1) visor = visor_player1;
        else { visor = visor_player2; }
        return visor;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SaveBounds();
        }
    }



    //BOTONES MENÚ

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


    void SetStringsMainMenu()
    {
        Transform mainMenu_transform = transform.Find("MAIN_MENU");
        mainMenu_transform.Find("play_BTN").Find("Text").GetComponent<Text>().text = Lenguaje.Instance.play;
        mainMenu_transform.Find("nest_BTN").Find("Text").GetComponent<Text>().text = Lenguaje.Instance.nest;
        mainMenu_transform.Find("shop_BTN").Find("Text").GetComponent<Text>().text = Lenguaje.Instance.shop;
    }
}
