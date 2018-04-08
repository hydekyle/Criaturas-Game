using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Menu : MonoBehaviour {

    public static Menu instance;
    public Equipment loadedEquipment = new Equipment();
    Visor visor_player1;
    Visor visor_player2;
    

    void Awake()
    {
        instance = this;
        visor_player1 = new Visor();
        visor_player2 = new Visor();
    }

    void Start()
    {
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

    public void SetVisorPosition(int playerN, Vector3 newPos, bool flip)
    {
        Visor visor = new Visor();

        if (playerN == 1) visor = visor_player1; else visor = visor_player1;
        visor.myTransform.localPosition = newPos;
        if (flip) visor.myTransform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    IEnumerator Visualizar(Transform visor, Vector3 localPos, bool flip)
    {
        visor.localPosition = localPos;
        yield return new WaitForSeconds(0.1f); // ¿?
        if (flip) visor.rotation = Quaternion.Euler(0, 180, 0);
        visor.gameObject.SetActive(true);
    }

    public IEnumerator SendImage(int id, Visor visor)
    {
        Sprite spriteBuscado = null;
        yield return StartCoroutine(Items.instance.ItemSpriteByID(id, value => spriteBuscado = value));
        SetImageVisor(spriteBuscado, id, visor);
    }

    public void SetImageVisor(Sprite sprite, int id, Visor visor)
    {
        int listNumber = int.Parse(id.ToString().Substring(0, 1));
        switch (listNumber)
        {
            case 1: visor.headgear.sprite = sprite; break;
            case 2: visor.body.sprite = sprite; break;
            case 3: visor.arm_left.sprite = visor.arm_right.sprite = sprite; break;
            case 4: visor.leg_left.sprite = visor.leg_right.sprite = sprite; break;
            case 5: visor.back.sprite = sprite; break;
        }
    }

    public IEnumerator VisualizarEquipamiento(Equipment e, int playerNumber)
    {
        //Actuar solo si el equipamiento ha cambiado. Para el jugador 2 se muestra sin checkear.
        if (loadedEquipment.body.ID != e.body.ID || playerNumber == 2)
        {
            yield return StartCoroutine(SendImage(e.body.ID, GetPlayerVisor(playerNumber)));
        }
        if (loadedEquipment.head.ID != e.head.ID || playerNumber == 2)
        {
            yield return StartCoroutine(SendImage(e.head.ID, GetPlayerVisor(playerNumber)));
        }
        if (loadedEquipment.arms.ID != e.arms.ID || playerNumber == 2)
        {
            yield return StartCoroutine(SendImage(e.arms.ID, GetPlayerVisor(playerNumber)));
        }
        if (loadedEquipment.legs.ID != e.legs.ID || playerNumber == 2)
        {
            yield return StartCoroutine(SendImage(e.legs.ID, GetPlayerVisor(playerNumber)));
        }
        if (loadedEquipment.back.ID != e.back.ID || playerNumber == 2)
        {
            yield return StartCoroutine(SendImage(e.back.ID, GetPlayerVisor(playerNumber)));
        }

        loadedEquipment = e;
        ColocarPiezas(e, GetPlayerVisor(playerNumber));

    }

    void ColocarPiezas(Equipment e, Visor visor)
    {
        ColocarBody(e.body.ID, visor);
        FixScales(e, visor);
    }

    void ColocarBody(int bigID, Visor visor)
    {
        int id = int.Parse(bigID.ToString().Substring(0, 3));
        ColocarPiezas(Database.instance.LeerBodyBounds(id), visor);
    }

    void FixScales(Equipment e, Visor visor)
    {
        int id = int.Parse(e.head.ID.ToString().Substring(0, 3));
        string ruta = "Assets/Resources/FixedScale/" + id.ToString() + ".txt";
        if (File.Exists(ruta))  //HEADGEAR
        {
            FixedScale fix = JsonUtility.FromJson<FixedScale>(File.ReadAllText(ruta));
            Vector3 fixScale = fix.customScale;
            Vector3 fixPosition = fix.customPosition;
            visor.headgear.rectTransform.localScale    = fixScale;
            visor.headgear.rectTransform.localPosition = Database.instance.LeerBodyBounds(e.body.ID).head_POS + fixPosition;
        }else
        {
            visor.headgear.rectTransform.localScale    = new Vector3(0.8f, 0.8f, 0.8f);
            visor.headgear.rectTransform.localPosition = Database.instance.LeerBodyBounds(e.body.ID).head_POS;
        }

        id = int.Parse(e.arms.ID.ToString().Substring(0, 3));
        ruta = "Assets/Resources/FixedScale/" + id.ToString() + ".txt";
        if (File.Exists(ruta))  //ARMS
        {
            FixedScale fix = JsonUtility.FromJson<FixedScale>(File.ReadAllText(ruta));
            Vector3 fixScale = fix.customScale;
            Vector3 fixPosition = fix.customPosition;
            visor.arm_left.rectTransform.localScale     = visor.arm_right.rectTransform.localScale = fixScale;
            visor.arm_left.rectTransform.localPosition  = Database.instance.LeerBodyBounds(e.body.ID).arm_left_POS + fixPosition;
            visor.arm_right.rectTransform.localPosition = Database.instance.LeerBodyBounds(e.body.ID).arm_right_POS + fixPosition;
        }
        else
        {
            visor.arm_left.rectTransform.localScale     = Vector3.one;
            visor.arm_right.rectTransform.localScale    = Vector3.one;
            visor.arm_left.rectTransform.localPosition  = Database.instance.LeerBodyBounds(e.body.ID).arm_left_POS;
            visor.arm_right.rectTransform.localPosition = Database.instance.LeerBodyBounds(e.body.ID).arm_right_POS;
        }

        id = int.Parse(e.legs.ID.ToString().Substring(0, 3));
        ruta = "Assets/Resources/FixedScale/" + id.ToString() + ".txt";
        if (File.Exists(ruta))  //LEGS
        {
            FixedScale fix = JsonUtility.FromJson<FixedScale>(File.ReadAllText(ruta));
            Vector3 fixScale = fix.customScale;
            Vector3 fixPosition = fix.customPosition;
            visor.leg_left.rectTransform.localScale     = visor.leg_right.rectTransform.localScale = fixScale;
            visor.leg_left.rectTransform.localPosition  = Database.instance.LeerBodyBounds(e.body.ID).leg_left_POS + fixPosition;
            visor.leg_right.rectTransform.localPosition = Database.instance.LeerBodyBounds(e.body.ID).leg_right_POS + fix.customPosition_right;
            visor.leg_right.rectTransform.localRotation = Quaternion.Euler(fix.customRotation_right);
            
            
        }
        else
        {
            visor.leg_left.rectTransform.localScale     = Vector3.one;
            visor.leg_right.rectTransform.localScale    = Vector3.one;
            visor.leg_left.rectTransform.localPosition  = Database.instance.LeerBodyBounds(e.body.ID).leg_left_POS;
            visor.leg_right.rectTransform.localPosition = Database.instance.LeerBodyBounds(e.body.ID).leg_right_POS;
            visor.leg_right.rectTransform.localRotation = Quaternion.Euler(Vector3.zero);
        }



    }

    void SaveScale()
    {
        int id = int.Parse(GameManager.instance.player.criatura.equipment.legs.ID.ToString().Substring(0, 3));
        int bodyID = int.Parse(GameManager.instance.player.criatura.equipment.body.ID.ToString().Substring(0, 3));
        string ruta = "Assets/Resources/FixedScale/" + id.ToString() + ".txt";
        FixedScale f = new FixedScale()
        {
            customRotation_right = Mathf.Approximately(visor_player1.leg_right.rectTransform.rotation.eulerAngles.y, 0f) ? Vector3.zero : visor_player1.leg_right.rectTransform.rotation.eulerAngles,
            customPosition = visor_player1.leg_left.rectTransform.localPosition - Database.instance.LeerBodyBounds(bodyID).leg_left_POS,
            customPosition_right = visor_player1.leg_right.rectTransform.localPosition - Database.instance.LeerBodyBounds(bodyID).leg_right_POS,
            customScale = visor_player1.leg_right.rectTransform.localScale
        };
        File.WriteAllText(ruta, JsonUtility.ToJson(f));
        print("Saving " + id);
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

    

    private void ColocarPiezas(BodyBounds bounds, Visor visor)
    {
        visor.headgear.rectTransform.localPosition = visor.body.rectTransform.localPosition + bounds.head_POS;
        visor.arm_right.rectTransform.localPosition = visor.body.rectTransform.localPosition + bounds.arm_right_POS;
        visor.arm_left.rectTransform.localPosition = visor.body.rectTransform.localPosition + bounds.arm_left_POS;
        visor.leg_right.rectTransform.localPosition = visor.body.rectTransform.localPosition + bounds.leg_right_POS;
        visor.leg_left.rectTransform.localPosition = visor.body.rectTransform.localPosition + bounds.leg_left_POS;
        visor.back.rectTransform.localPosition = visor.body.rectTransform.localPosition + bounds.back_POS;
    }

    void SaveBounds()
    {
    #if UNITY_EDITOR
        BodyBounds bounds = new BodyBounds()
        {
            head_POS = visor_player1.headgear.rectTransform.localPosition,
            arm_left_POS = visor_player1.arm_left.rectTransform.localPosition,
            arm_right_POS = visor_player1.arm_right.rectTransform.localPosition,
            leg_left_POS = visor_player1.leg_left.rectTransform.localPosition,
            leg_right_POS = visor_player1.leg_right.rectTransform.localPosition,
            back_POS = visor_player1.back.rectTransform.localPosition
        };
        Database.instance.GuardarBodyBounds(bounds);
    #endif
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveScale();
        }
    } //ONLY FOR EDITION PURPOSE


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
