using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour {



    /* 
     * Comienza pelea. Se cargan estadísticas y se decide quién comienza.
     * Comienza turno. (Check condiciones pre-turno: efectos aplicados, cargar nuevas habilidades, +1 de maná, check habilidades usables).
     * 
     * 
     * 
     *      
     */

    public static BattleSystem instance { get; set; }

    Player player1;
    Player player2;

    public List<GiveStat> totalStats_player1 = new List<GiveStat>();
    public List<GiveStat> totalStats_player2 = new List<GiveStat>();

    RectTransform rect_skills;
    Image fadeScreen;
    public SkillsButtons skill_buttons;

    public Stats myStats;
    public Stats enemyStats;

    public int minigameFails = 0;
    int lastSkill_ID;

    bool yourTurn;
    float fadeToAphaValue = 0;

    void Start()
    {
        instance = this;
        Initialize();
        StartTurn();
    }

    void Initialize()
    {
        #region Transforms
        fadeScreen = transform.GetChild(0).Find("FadeScreen").GetComponent<Image>();
        Transform habilidadesT = transform.Find("Canvas").Find("[Habilidades]");
        Transform headT = habilidadesT.Find("Skill_Head");
        Transform bodyT = habilidadesT.Find("Skill_Body");
        Transform armsT = habilidadesT.Find("Skill_Arms");
        Transform legsT = habilidadesT.Find("Skill_Legs");
        rect_skills = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
        rect_skills.localPosition = rect_skills.localPosition + Vector3.down * 50;
        rect_skills.gameObject.SetActive(true);

        #endregion
        #region Skill_Buttons
        skill_buttons = new SkillsButtons()
        {
            head_button = new SkillButton()
            {
                myImage = headT.GetComponent<Image>(),
                myText = headT.GetChild(0).GetComponent<Text>()
            },
            body_button = new SkillButton()
            {
                myImage = bodyT.GetComponent<Image>(),
                myText = bodyT.GetChild(0).GetComponent<Text>()
            },
            arms_button = new SkillButton()
            {
                myImage = armsT.GetComponent<Image>(),
                myText = armsT.GetChild(0).GetComponent<Text>()
            },
            legs_button = new SkillButton()
            {
                myImage = legsT.GetComponent<Image>(),
                myText = legsT.GetChild(0).GetComponent<Text>()
            }
        };
        #endregion
        player1 = GameManager.instance.player;
        player2 = ConstruirIA();
        

        StartCoroutine(GameManager.instance.MostrarJugador(player2, 2, new Vector3(120, 40, 0), false)); //Visualizar oponente
        Menu.instance.SetVisorPosition(1, new Vector3(-120, 40, -1), true); //Recolocar jugador 1

        LeerStats();
    }

    #region Engine
    public List<Equipable_Item> ObtenerListaEquipamiento(Player player)
    {
        List<Equipable_Item> lista = new List<Equipable_Item>();
        lista.Add(player.criatura.equipment.head);
        lista.Add(player.criatura.equipment.body);
        lista.Add(player.criatura.equipment.arms);
        lista.Add(player.criatura.equipment.legs);
        lista.Add(player.criatura.equipment.back);
        return lista;
    }

    public void LeerStats() //Lee stats, y habilidades de los jugadores
    {
        List<Equipable_Item> equipamiento_list = ObtenerListaEquipamiento(player1);
        foreach (Equipable_Item e in equipamiento_list)
        {
            for (var x = 0; x < e.addStat.Count; x++)
            {
                totalStats_player1.Add(e.addStat[x]);
            }
        }
        equipamiento_list = ObtenerListaEquipamiento(player2);
        foreach (Equipable_Item e in equipamiento_list)
        {
            for (var x = 0; x < e.addStat.Count; x++)
            {
                totalStats_player2.Add(e.addStat[x]);
            }
        }
        player1.criatura.skills = new MySkylls();
        foreach (int sID in player1.criatura.equipment.head.skills_ID) { player1.criatura.skills.head.Add(sID); }
        foreach (int sID in player1.criatura.equipment.body.skills_ID) { player1.criatura.skills.body.Add(sID); }
        foreach (int sID in player1.criatura.equipment.arms.skills_ID) { player1.criatura.skills.arms.Add(sID); }
        foreach (int sID in player1.criatura.equipment.legs.skills_ID) { player1.criatura.skills.legs.Add(sID); }
        player2.criatura.skills = new MySkylls();
        foreach (int sID in player2.criatura.equipment.head.skills_ID) { player2.criatura.skills.head.Add(sID); }
        foreach (int sID in player2.criatura.equipment.body.skills_ID) { player2.criatura.skills.body.Add(sID); }
        foreach (int sID in player2.criatura.equipment.arms.skills_ID) { player2.criatura.skills.arms.Add(sID); }
        foreach (int sID in player2.criatura.equipment.legs.skills_ID) { player2.criatura.skills.legs.Add(sID); }
        UpdateSkillButtons();
        myStats = new Stats()
        {
            health_base = 1000,
            skill_now = 10
        };
    }

    public void UpdateSkillButtons()
    {
        try
        {
            skill_buttons.head_activable_skill_ID = player1.criatura.skills.head[Random.Range(0, player1.criatura.skills.head.Count)];
            skill_buttons.body_activable_skill_ID = player1.criatura.skills.body[Random.Range(0, player1.criatura.skills.body.Count)];
            skill_buttons.arms_activable_skill_ID = player1.criatura.skills.arms[Random.Range(0, player1.criatura.skills.arms.Count)];
            skill_buttons.legs_activable_skill_ID = player1.criatura.skills.legs[Random.Range(0, player1.criatura.skills.legs.Count)];

            skill_buttons.head_button.myText.text = Lenguaje.Instance.SkillNameByID(skill_buttons.head_activable_skill_ID);
            skill_buttons.body_button.myText.text = Lenguaje.Instance.SkillNameByID(skill_buttons.body_activable_skill_ID);
            skill_buttons.arms_button.myText.text = Lenguaje.Instance.SkillNameByID(skill_buttons.arms_activable_skill_ID);
            skill_buttons.legs_button.myText.text = Lenguaje.Instance.SkillNameByID(skill_buttons.legs_activable_skill_ID);
        }catch
        {
            print("Falta setear habilidades");
        }
        
    }  //Realizar al cambiar de turno

    Player ConstruirIA()
    {
        return new Player()
        {
            nombre = "Dios",
            ID = "0000",
            criatura = new Criatura()
            {
                nombre = "Jesus Christ",
                equipment = new Equipment()
                {
                    head = Items.instance.ItemByID(GetRandomItemID(1)),
                    body = Items.instance.ItemByID(GetRandomItemID(2)),
                    arms = Items.instance.ItemByID(GetRandomItemID(3)),
                    legs = Items.instance.ItemByID(GetRandomItemID(4)),
                    back = Items.instance.ItemByID(GetRandomItemID(5))
                }
            }
        };
    }

    Equipment GetRandomEquip()
    {
        Equipment e = new Equipment()
        {

        };
        return e;
    }

    int GetRandomItemID(int listNumber)
    {
        string randomID = "";
        switch (listNumber)
        {
            case 1: randomID = Random.Range(0, Items.instance.headgear_list.Count).ToString(); break;
            case 2: randomID = Random.Range(0, Items.instance.bodies_list.Count).ToString(); break;
            case 3: randomID = Random.Range(0, Items.instance.arms_list.Count).ToString(); break;
            case 4: randomID = Random.Range(0, Items.instance.legs_list.Count).ToString(); break;
            case 5: randomID = Random.Range(0, Items.instance.backs_list.Count).ToString(); break;
        }
        if (randomID.ToString().Length < 2) randomID = "0" + randomID;
        string final = listNumber.ToString() + randomID + "000";
        return int.Parse(final);
    }

    void FadeAlpha(float alphaValue)
    {
        fadeToAphaValue = alphaValue;
    }

    #endregion

    #region Botones_Activables
    public void Head_BTN()
    {
        LanzarSkill(skill_buttons.head_activable_skill_ID);
    }

    public void Body_BTN()
    {
        LanzarSkill(skill_buttons.body_activable_skill_ID);
    }

    public void Arms_BTN()
    {
        LanzarSkill(skill_buttons.arms_activable_skill_ID);
    }

    public void Legs_BTN()
    {
        LanzarSkill(skill_buttons.legs_activable_skill_ID);
    }
    #endregion

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T)) StartTurn();

        if (yourTurn)
        {
            rect_skills.localPosition = Vector3.Lerp(rect_skills.localPosition, Vector3.zero, Time.deltaTime * 5);
        }else
        {
            rect_skills.localPosition = Vector3.Lerp(rect_skills.localPosition, new Vector3(0, -50, 0), Time.deltaTime * 5);
        }
        if(!Mathf.Approximately(fadeScreen.color.a, fadeToAphaValue))
        {
            fadeScreen.color = Color.Lerp(fadeScreen.color, new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, fadeToAphaValue), Time.deltaTime * 2);
        }

    }

    void StartTurn()
    {
        Message.instance.NewMessage(Lenguaje.Instance.Text_YourTurn());
        yourTurn = true;
    }

    public void EndMinigame()
    {
        FadeAlpha(0);
        print("Errores: " + minigameFails.ToString());
        DoSkill(Skills.instance.SkillResolve(lastSkill_ID, myStats, enemyStats, minigameFails));
        minigameFails = 0;
        StartTurn();
    }

    void LanzarSkill(int ID_skill)
    {
        if (yourTurn) {
            Message.instance.NewMessage(Lenguaje.Instance.SkillNameByID(ID_skill));
            yourTurn = false;
            FadeAlpha(0.5f);
            switch (Skills.instance.SkillClassByID(ID_skill))
            {
                case Skill_Class.Assassin: OsuSystem.instance.Bolas(); break;
                case Skill_Class.Alpha:    TapFast.instance.Iniciar(); break;
                case Skill_Class.Charming: AccuracySystem.instance.Iniciar(); break;
                default: OsuSystem.instance.Bolas(); break;
            }
            
            lastSkill_ID = ID_skill;
        } 
    }

    void DoSkill(Skill_Result result)
    {
        switch (result.s_type)
        {
            case Skill_Type.Heal: ApplyHeal(result.value); break;
        }
    }

    void ApplyHeal(float value)
    {
        float newHealthValue = myStats.health_now + value;
        myStats.health_now = Mathf.Clamp(newHealthValue, 0, myStats.health_base);
    }



    

}
