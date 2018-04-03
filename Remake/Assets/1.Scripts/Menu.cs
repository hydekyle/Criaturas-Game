using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Menu : MonoBehaviour {

    public static Menu instance;
    Visor visor_player1;
    Visor visor_player2;
    

    void Awake()
    {
        instance = this;
        visor_player1 = new Visor();
        visor_player2 = new Visor();
        Initialize();
    }

    void Initialize()
    {
        
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
        yield return new WaitForSeconds(0.1f);
        visor.localPosition = localPos;
        if (flip) visor.localRotation = Quaternion.Euler(0, 180, 0);
        visor.gameObject.SetActive(true);
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
            case 4: visor.leg_left.sprite = visor.leg_right.sprite = sprite; FixLegScale(id, visor);  break;
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
        Vector3 fixScale = Vector3.one;
        if (id == 301) fixScale = new Vector3(1.5f, 1, 1);
        if (id == 303) fixScale = new Vector3(1, 1.2f, 1);
        if (id == 304) fixScale = new Vector3(0.9f, 1, 1);
        if (id == 305) fixScale = new Vector3(1.3f, 1, 1);
        if (id == 306) fixScale = new Vector3(1.5f, 1.1f, 1);
        if (id == 307) fixScale = new Vector3(1.3f, 1.1f, 1);
        if (id == 308) fixScale = new Vector3(1, 1.3f, 1);
        if (id == 309) fixScale = new Vector3(1.5f, 1, 1);
        if (id == 310) fixScale = new Vector3(1.5f, 1.1f, 1);
        if (id == 314) fixScale = new Vector3(1, 1.6f, 1);
        if (id == 318) fixScale = new Vector3(1.1f, 1.1f, 1);
        if (id == 319) fixScale = new Vector3(1.2f, 1.2f, 1);
        if (id == 320) fixScale = new Vector3(1.2f, 1.2f, 1);
        if (id == 321) fixScale = new Vector3(1.2f, 1.2f, 1);
        if (id == 322) fixScale = new Vector3(1.2f, 1.2f, 1);
        if (id == 323) fixScale = new Vector3(1.2f, 1.2f, 1);
        if (id == 324) fixScale = new Vector3(1.2f, 1.2f, 1);
        if (id == 325) fixScale = new Vector3(1.3f, 1.4f, 1);
        if (id == 326) fixScale = new Vector3(1.7f, 1, 1);
        if (id == 327) fixScale = new Vector3(1.7f, 1, 1);
        if (id == 328) fixScale = new Vector3(1.5f, 1.2f, 1);
        if (id == 329) fixScale = new Vector3(1.2f, 1.1f, 1);
        if (id == 330) fixScale = new Vector3(1.27f, 1.17f, 1);
        if (id == 331) fixScale = new Vector3(1.28f, 1.22f, 1);
        if (id == 332) fixScale = new Vector3(1.3f, 1.2f, 1);
        if (id == 332) fixScale = new Vector3(1.3f, 1.2f, 1);
        if (id == 333) fixScale = new Vector3(1.3f, 1.2f, 1);
        if (id == 334) fixScale = new Vector3(1.5f, 1.3f, 1);
        if (id == 335) fixScale = new Vector3(1.3f, 1.3f, 1);
        if (id == 336) fixScale = new Vector3(1.2f, 1.4f, 1);
        if (id == 337) fixScale = new Vector3(1.7f, 1.1f, 1);
        if (id == 338) fixScale = new Vector3(1.34f, 1.25f, 1);
        if (id == 339) fixScale = new Vector3(1.3f, 1.3f, 1);
        if (id == 340) fixScale = new Vector3(1.25f, 1.15f, 1);
        if (id == 340) fixScale = new Vector3(1.25f, 1.15f, 1);
        if (id == 341) fixScale = new Vector3(1.1f, 1.24f, 1);
        if (id == 342) fixScale = new Vector3(1.1f, 1.23f, 1);
        if (id == 343) fixScale = new Vector3(0.9f, 1.3f, 1);
        if (id == 344) fixScale = new Vector3(1.28f, 1.2f, 1);



        visor.arm_left.rectTransform.localScale = visor.arm_right.rectTransform.localScale = fixScale;
    }

    void FixLegScale(int bidID, Visor visor)
    {
        int id = int.Parse(bidID.ToString().Substring(0, 3));
        Vector3 fixScale = new Vector3(1.2f, 1.1f, 1);
        if (id == 400) fixScale = new Vector3(1.1f, 1, 1);
        if (id == 401) fixScale = new Vector3(1.4f, 1.1f, 1);
        if (id == 402) fixScale = new Vector3(1.2f, 1.1f, 1);
        if (id == 405) fixScale = new Vector3(1, 1.2f, 1);
        if (id == 406) fixScale = new Vector3(0.75f, 1f, 1);
        if (id == 407) fixScale = new Vector3(0.9f, 1.7f, 1);
        if (id == 408) fixScale = new Vector3(2, 1.2f, 1);
        if (id == 409) fixScale = new Vector3(1.5f, 1.5f, 1);
        if (id == 410) fixScale = new Vector3(2f, 1.5f, 1);
        if (id == 411) fixScale = new Vector3(0.8f, 1.3f, 1);
        if (id == 412) fixScale = new Vector3(0.8f, 1.3f, 1);
        if (id == 413) fixScale = new Vector3(0.8f, 1.3f, 1);
        if (id == 414) fixScale = new Vector3(0.8f, 1.3f, 1);
        if (id == 415) fixScale = new Vector3(0.8f, 1.2f, 1);
        if (id == 416) fixScale = new Vector3(0.8f, 1.2f, 1);
        if (id == 417) fixScale = new Vector3(1, 1.3f, 1);
        if (id == 418) fixScale = new Vector3(1.5f, 1.2f, 1);

        visor.leg_right.rectTransform.localScale = visor.leg_left.rectTransform.localScale = fixScale;
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
