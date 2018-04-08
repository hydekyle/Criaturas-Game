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

    Player jugador1;
    Player jugador2;

    public List<GiveStat> totalStats_player1 = new List<GiveStat>();
    public List<GiveStat> totalStats_player2 = new List<GiveStat>();

    public SkillsButtons skill_buttons;


    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        Transform habilidadesT = transform.Find("Canvas").Find("[Habilidades]");
        Transform headT = habilidadesT.Find("Skill_Head");
        Transform bodyT = habilidadesT.Find("Skill_Body");
        Transform armsT = habilidadesT.Find("Skill_Arms");
        Transform legsT = habilidadesT.Find("Skill_Legs");
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
        jugador1 = GameManager.instance.player;
        jugador2 = ConstruirIA();
        

        StartCoroutine(GameManager.instance.MostrarJugador(jugador2, 2, new Vector3(120, 40, 0), false)); //Visualizar oponente
        Menu.instance.SetVisorPosition(1, new Vector3(-120, 40, -1), true); //Recolocar jugador 1

        LeerStats();
    }

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

    public void LeerStats() //Lee el equipamiento, y habilidades de los jugadores
    {
        List<Equipable_Item> equipamiento_list = ObtenerListaEquipamiento(jugador1);
        foreach (Equipable_Item e in equipamiento_list)
        {
            for (var x = 0; x < e.addStat.Count; x++)
            {
                totalStats_player1.Add(e.addStat[x]);
            }
        }
        equipamiento_list = ObtenerListaEquipamiento(jugador2);
        foreach (Equipable_Item e in equipamiento_list)
        {
            for (var x = 0; x < e.addStat.Count; x++)
            {
                totalStats_player2.Add(e.addStat[x]);
            }
        }
        jugador1.criatura.skills = new MySkylls();
        foreach (int sID in jugador1.criatura.equipment.head.skills_ID) { jugador1.criatura.skills.head.Add(sID); }
        foreach (int sID in jugador1.criatura.equipment.body.skills_ID) { jugador1.criatura.skills.body.Add(sID); }
        foreach (int sID in jugador1.criatura.equipment.arms.skills_ID) { jugador1.criatura.skills.arms.Add(sID); }
        foreach (int sID in jugador1.criatura.equipment.legs.skills_ID) { jugador1.criatura.skills.legs.Add(sID); }
        jugador2.criatura.skills = new MySkylls();
        foreach (int sID in jugador2.criatura.equipment.head.skills_ID) { jugador2.criatura.skills.head.Add(sID); }
        foreach (int sID in jugador2.criatura.equipment.body.skills_ID) { jugador2.criatura.skills.body.Add(sID); }
        foreach (int sID in jugador2.criatura.equipment.arms.skills_ID) { jugador2.criatura.skills.arms.Add(sID); }
        foreach (int sID in jugador2.criatura.equipment.legs.skills_ID) { jugador2.criatura.skills.legs.Add(sID); }
        UpdateSkillButtons();
    }

    public void UpdateSkillButtons()
    {
        try
        {
            skill_buttons.head_activable_skill_ID = jugador1.criatura.skills.head[Random.Range(0, jugador1.criatura.skills.head.Count)];
            skill_buttons.body_activable_skill_ID = jugador1.criatura.skills.body[Random.Range(0, jugador1.criatura.skills.body.Count)];
            skill_buttons.arms_activable_skill_ID = jugador1.criatura.skills.arms[Random.Range(0, jugador1.criatura.skills.arms.Count)];
            skill_buttons.legs_activable_skill_ID = jugador1.criatura.skills.legs[Random.Range(0, jugador1.criatura.skills.legs.Count)];

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


    //BOTONES
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


    void LanzarSkill(int ID_skill)
    {
        Skill_Result result = Skills.Instance.SkillResolve(ID_skill);
        print(Lenguaje.Instance.SkillNameByID(ID_skill));
    }

}
