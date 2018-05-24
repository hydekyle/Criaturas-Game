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

public class BattleSystem : MonoBehaviour, RealTimeMultiplayerListener {

    public static BattleSystem instance { get; set; }

    Player player1;
    Player player2;

    public List<GiveStat> totalStats_player1 = new List<GiveStat>();
    public List<GiveStat> totalStats_player2 = new List<GiveStat>();

    RectTransform rect_skills;
    Image fadeScreen;
    public SkillsButtons skill_buttons;
    Text textoMyHP, textoEnemyHP;

    public int minigameFails = 0;
    public int lastSkill_ID;
    public int lastSkillOponent_ID;

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
        textoMyHP = transform.GetChild(0).Find("MyHP").GetComponent<Text>();
        textoEnemyHP = transform.GetChild(0).Find("EnemyHP").GetComponent<Text>();
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
        player1.status = new Stats()
        {
            health_base = 1000,
            skill_base = 10
        };
        player2.status = new Stats()
        {
            dizziness = 4,
            health_base = 999,
            health_now = 999,
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

        textoEnemyHP.text = enemigo.status.health_now.ToString();
        print(enemigo.status.bleed);
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
        } else
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
        Message.instance.NewMessage(Lenguaje.Instance.Text_YourTurn());
        yourTurn = true;
        if(Application.platform == RuntimePlatform.Android)
        {
            //TestOnline();
        }
        
    }

    public void Go_back()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }

    #region GOOGLE_PLAY_ONLINE
    public void TestOnline()
    {
        PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(1, 1, 0, this);
    }

    public void InvitationReceived(Invitation invitation)
    {
        Message.instance.NewMessage("HAS RECIBIDO ALGO");
        PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitation.InvitationId, this);
    }

    public void GO_MATCHMAKING()
    {
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(1, 1, 0, this);
        //PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(this);
    }

    public void CheckInvitations()
    {
        PlayGamesPlatform.Instance.RealTime.GetAllInvitations((listener) => {
            Message.instance.NewMessage("Tienes " + listener.Length + " invitaciones");
        });
    }

    public void OnRoomConnected(bool success)
    {
        if (success)
        {
            Message.instance.NewMessage("CONECTADO A ROOM");
            string info = "Mirame y dime";
            byte[] data = System.Text.Encoding.Default.GetBytes(info);
            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, data);
            List<Participant> listaPersonas = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
            Message.instance.NewMessage("Room conectada: " + listaPersonas.Count);
        }
        else
        {
            Message.instance.NewMessage("Room es " + PlayGamesPlatform.Instance.RealTime.IsRoomConnected().ToString());
        }
        
    }

    public void OnRoomSetupProgress(float percent)
    {
        Message.instance.NewMessage("F: " + percent.ToString());
    }

    public void OnLeftRoom()
    {
        Message.instance.NewMessage("Abandono");
    }

    public void OnParticipantLeft(Participant p)
    {
        Message.instance.NewMessage(p.DisplayName + " se ha ido");
    }

    public void OnPeersConnected(string[] s)
    {
        Message.instance.NewMessage("Peer: " + s.Length.ToString());
    }

    public void OnPeersDisconnected(string[] s)
    {
        Message.instance.NewMessage("Peer fuera");
    }

    public void OnRealTimeMessageReceived(bool isReliable, string sender, byte[] byteArray)
    {
        string mensj = System.Convert.ToBase64String(byteArray);
        Message.instance.NewMessage("MENSAJE RECIBIDO: "+mensj);
        
    }
    #endregion



}
