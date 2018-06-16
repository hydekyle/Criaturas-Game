using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour {

    public static BattleSystem instance { get; set; }

    Player player1;
    Player player2;

    public List<GiveStat> totalStats_player1 = new List<GiveStat>();
    public List<GiveStat> totalStats_player2 = new List<GiveStat>();

    RectTransform rect_skills;
    Image fadeScreen;
    [HideInInspector]
    public SkillsButtons skill_buttons;
    int selectedButton = 0;
    Text textoMyHP, textoEnemyHP;

    [HideInInspector]
    public int minigameFails = 0;
    [HideInInspector]
    public int lastSkill_ID;
    [HideInInspector]
    public int lastSkillOponent_ID;

    bool yourTurn;
    float fadeToAphaValue = 0;

    public int yMax = 50;
    public int yMin = -50;

    Transform headT, bodyT, armsT, legsT;


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
        textoMyHP = transform.GetChild(0).Find("MyHP").GetComponent<Text>();
        textoEnemyHP = transform.GetChild(0).Find("EnemyHP").GetComponent<Text>();
        Transform habilidadesT = transform.Find("Canvas").Find("[Habilidades]");
        headT = habilidadesT.Find("Skill_Head");
        bodyT = habilidadesT.Find("Skill_Body");
        armsT = habilidadesT.Find("Skill_Arms");
        legsT = habilidadesT.Find("Skill_Legs");
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
                myText = headT.Find("Text").GetComponent<Text>(),
                orb = headT.Find("Orb").GetComponent<Image>(),
                descriptionText = headT.Find("Description").GetComponent<Text>()
            },
            body_button = new SkillButton()
            {
                myImage = bodyT.GetComponent<Image>(),
                myText = bodyT.Find("Text").GetComponent<Text>(),
                orb = bodyT.Find("Orb").GetComponent<Image>(),
                descriptionText = bodyT.Find("Description").GetComponent<Text>()
            },
            arms_button = new SkillButton()
            {
                myImage = armsT.GetComponent<Image>(),
                myText = armsT.Find("Text").GetComponent<Text>(),
                orb = armsT.Find("Orb").GetComponent<Image>(),
                descriptionText = armsT.Find("Description").GetComponent<Text>()
            },
            legs_button = new SkillButton()
            {
                myImage = legsT.GetComponent<Image>(),
                myText = legsT.Find("Text").GetComponent<Text>(),
                orb = legsT.Find("Orb").GetComponent<Image>(),
                descriptionText = legsT.Find("Description").GetComponent<Text>()
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
        player1.criatura.skills = new MySkylls();
        player1.criatura.skills.head.Add(int.Parse(player1.criatura.equipment.head.ID_string.Substring(8, 3)));
        player1.criatura.skills.head.Add(int.Parse(player1.criatura.equipment.head.ID_string.Substring(11, 3)));

        player1.criatura.skills.body.Add(int.Parse(player1.criatura.equipment.body.ID_string.Substring(8, 3)));
        player1.criatura.skills.body.Add(int.Parse(player1.criatura.equipment.body.ID_string.Substring(11, 3)));

        player1.criatura.skills.arms.Add(int.Parse(player1.criatura.equipment.arms.ID_string.Substring(8, 3)));
        player1.criatura.skills.arms.Add(int.Parse(player1.criatura.equipment.arms.ID_string.Substring(11, 3)));

        player1.criatura.skills.legs.Add(int.Parse(player1.criatura.equipment.legs.ID_string.Substring(8, 3)));
        player1.criatura.skills.legs.Add(int.Parse(player1.criatura.equipment.legs.ID_string.Substring(11, 3)));

        player2.criatura.skills = new MySkylls();
        player2.criatura.skills.head.Add(int.Parse(player2.criatura.equipment.head.ID_string.Substring(8, 3)));
        player2.criatura.skills.head.Add(int.Parse(player2.criatura.equipment.head.ID_string.Substring(11, 3)));

        player2.criatura.skills.body.Add(int.Parse(player2.criatura.equipment.body.ID_string.Substring(8, 3)));
        player2.criatura.skills.body.Add(int.Parse(player2.criatura.equipment.body.ID_string.Substring(11, 3)));

        player2.criatura.skills.arms.Add(int.Parse(player2.criatura.equipment.arms.ID_string.Substring(8, 3)));
        player2.criatura.skills.arms.Add(int.Parse(player2.criatura.equipment.arms.ID_string.Substring(11, 3)));

        player2.criatura.skills.legs.Add(int.Parse(player2.criatura.equipment.legs.ID_string.Substring(8, 3)));
        player2.criatura.skills.legs.Add(int.Parse(player2.criatura.equipment.legs.ID_string.Substring(11, 3)));

        UpdateSkillButtons();
        player1.status = new Stats()
        {
            health_base = 100,
            skill_base = 10
        };
        player2.status = new Stats()
        {
            dizziness = 4,
            health_base = 100,
            health_now = 100,
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
            skill_buttons.head_button.descriptionText.text = Lenguaje.Instance.SkillDescriptionByID(skill_buttons.head_activable_skill_ID);
            skill_buttons.head_button.orb.sprite = Items.instance.SphereBySkillID(skill_buttons.head_activable_skill_ID);

            skill_buttons.body_button.myText.text = Lenguaje.Instance.SkillNameByID(skill_buttons.body_activable_skill_ID);
            skill_buttons.body_button.descriptionText.text = Lenguaje.Instance.SkillDescriptionByID(skill_buttons.body_activable_skill_ID);
            skill_buttons.body_button.orb.sprite = Items.instance.SphereBySkillID(skill_buttons.body_activable_skill_ID);

            skill_buttons.arms_button.myText.text = Lenguaje.Instance.SkillNameByID(skill_buttons.arms_activable_skill_ID);
            skill_buttons.arms_button.descriptionText.text = Lenguaje.Instance.SkillDescriptionByID(skill_buttons.arms_activable_skill_ID);
            skill_buttons.arms_button.orb.sprite = Items.instance.SphereBySkillID(skill_buttons.arms_activable_skill_ID);

            skill_buttons.legs_button.myText.text = Lenguaje.Instance.SkillNameByID(skill_buttons.legs_activable_skill_ID);
            skill_buttons.legs_button.descriptionText.text = Lenguaje.Instance.SkillDescriptionByID(skill_buttons.legs_activable_skill_ID);
            skill_buttons.legs_button.orb.sprite = Items.instance.SphereBySkillID(skill_buttons.legs_activable_skill_ID);


        } catch
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
                    head = Items.instance.ItemByID(Items.instance.GetRandomItemID(1)),
                    body = Items.instance.ItemByID(Items.instance.GetRandomItemID(2)),
                    arms = Items.instance.ItemByID(Items.instance.GetRandomItemID(3)),
                    legs = Items.instance.ItemByID(Items.instance.GetRandomItemID(4))
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

    void FadeAlpha(float alphaValue)
    {
        fadeToAphaValue = alphaValue;
    }

    void LanzarSkill(int ID_skill)
    {
        if (yourTurn)
        {
            Message.instance.NewMessage(Lenguaje.Instance.SkillNameByID(ID_skill));
            yourTurn = false;
            FadeAlpha(0.5f);
            switch (Skills.instance.SkillClassByID(ID_skill))
            {
                case Skill_Class.Assassin: OsuSystem.instance.Bolas(); break;
                case Skill_Class.Alpha: TapFast.instance.Iniciar(); break;
                case Skill_Class.Charming: AccuracySystem.instance.Iniciar(); break;
                case Skill_Class.Pacifist: SpellSystem.instance.Iniciar(); break;
                default: OsuSystem.instance.Bolas(); break;
            }

            lastSkill_ID = ID_skill;
        }
    }

    public void EndMinigame()
    {
        FadeAlpha(0);
        print("Errores: " + minigameFails.ToString());
        Skill_Result result = Skills.instance.SkillResolve(lastSkill_ID, MySelf().status, YourEnemy().status, minigameFails); //RESOLVER SKILL
        ApplyResult(result);
        minigameFails = 0;
        StartTurn();
    }

    void ApplyResult(Skill_Result result)
    {
        Player myself = MySelf();
        Player enemigo = YourEnemy();
        enemigo.status.health_now -= (int)result.value;
        enemigo.status.bleed = result.bleed;
        enemigo.status.dizziness = result.dizziness;
        enemigo.status.poison = result.poison;
        enemigo.status.confusion = result.confusion;
        myself.status.buff_attack = result.buff_attack;
        myself.status.buff_skill = result.buff_skill;
        myself.status.buff_luck = result.buff_luck;
        myself.status.buff_shield = result.buff_shield;
        myself.status.buff_barrier = result.buff_barrier;
        myself.status.bleed = result.myself_bleed;
        myself.status.health_now -= result.myself_damage;
        
        for (var x = 0; x < result.cleans; x++)
        {
            switch(Random.Range(0, 4))
            {
                case 0: myself.status.bleed--; break;
                case 1: myself.status.dizziness--; break;
                case 2: myself.status.poison--; break;
                case 3: myself.status.confusion--; break;
            }
        }

        int vidaEnemiga = enemigo.status.health_now;
        textoEnemyHP.text = enemigo.status.health_now.ToString();
        if (vidaEnemiga <= 0) GanarPartida();
    }

    public Player MySelf()
    {
        return player1;
    }

    public Player YourEnemy()
    {
        return player2;
    }

    #endregion

    #region Botones_Activables
    public void Head_BTN()
    {
        OnButtonClick(1);
    }

    public void Body_BTN()
    {
        OnButtonClick(2);
    }

    public void Arms_BTN()
    {
        OnButtonClick(3);
    }

    public void Legs_BTN()
    {
        OnButtonClick(4);
    }
    #endregion

    private void OnButtonClick(int n)
    {
        if(n == selectedButton)
        {
            switch (n)
            {
                case 1: LanzarSkill(skill_buttons.head_activable_skill_ID); break;
                case 2: LanzarSkill(skill_buttons.body_activable_skill_ID); break;
                case 3: LanzarSkill(skill_buttons.arms_activable_skill_ID); break;
                case 4: LanzarSkill(skill_buttons.legs_activable_skill_ID); break;
            }
            selectedButton = 0;
        }else
        {
            selectedButton = n;
        }
    }

    void Update()
    {
        
        if(selectedButton == 1) headT.localPosition = Vector3.Lerp(headT.localPosition, new Vector3(headT.localPosition.x, yMax, 0), Time.deltaTime * 10);
        else headT.localPosition = Vector3.Lerp(headT.localPosition, new Vector3(headT.localPosition.x, yMin, 0), Time.deltaTime * 10);

        if (selectedButton == 2) bodyT.localPosition = Vector3.Lerp(bodyT.localPosition, new Vector3(bodyT.localPosition.x, yMax, 0), Time.deltaTime * 10);
        else bodyT.localPosition = Vector3.Lerp(bodyT.localPosition, new Vector3(bodyT.localPosition.x, yMin, 0), Time.deltaTime * 10);

        if (selectedButton == 3) armsT.localPosition = Vector3.Lerp(armsT.localPosition, new Vector3(armsT.localPosition.x, yMax, 0), Time.deltaTime * 10);
        else armsT.localPosition = Vector3.Lerp(armsT.localPosition, new Vector3(armsT.localPosition.x, yMin, 0), Time.deltaTime * 10);

        if (selectedButton == 4) legsT.localPosition = Vector3.Lerp(legsT.localPosition, new Vector3(legsT.localPosition.x, yMax, 0), Time.deltaTime * 10);
        else legsT.localPosition = Vector3.Lerp(legsT.localPosition, new Vector3(legsT.localPosition.x, yMin, 0), Time.deltaTime * 10);

        RectPositions();

    }

    void RectPositions()
    {
        if (yourTurn)
        {
            rect_skills.localPosition = Vector3.Lerp(rect_skills.localPosition, Vector3.zero, Time.deltaTime * 5);
        }
        else
        {
            rect_skills.localPosition = Vector3.Lerp(rect_skills.localPosition, new Vector3(0, -100, 0), Time.deltaTime * 5);
        }
        if (!Mathf.Approximately(fadeScreen.color.a, fadeToAphaValue))
        {
            fadeScreen.color = Color.Lerp(fadeScreen.color, new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, fadeToAphaValue), Time.deltaTime * 2);
        }
    }

    void StartTurn()
    {
        UpdateSkillButtons();
        Message.instance.NewMessage(Lenguaje.Instance.Text_YourTurn());
        yourTurn = true;
    }

    public void Go_back()
    {
        Destroy(GameManager.instance.gameObject);
        SceneManager.LoadScene(0);
    }

    private void GanarPartida()
    {
        Database.instance.ReferenceDB().Child("data").Child("coronas").SetValueAsync(GameManager.instance.userdb.coronas + 1);
        Destroy(GameManager.instance.gameObject);
        SceneManager.LoadScene(0);
    }

}
