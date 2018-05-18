using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lenguaje : MonoBehaviour {

    [HideInInspector]
    public static Lenguaje Instance;

    bool spanish_language;

    void Awake()
    {
        Instance = this;
        CheckAndSetLanguage();
    }

    void CheckAndSetLanguage()
    {
        if (Application.systemLanguage == SystemLanguage.Spanish) SetLanguage(true); else SetLanguage(false);
    }

    void SetLanguage(bool spanish)
    {
        spanish_language = spanish;
    }


    public string SkillNameByID(int ID)
    {
        string name = "";
        try
        {
            if (spanish_language) name = Skills.instance.SkillByID(ID).name_spanish;
                             else name = Skills.instance.SkillByID(ID).name_english;
        }catch { name = "Null"; }
        
        return name;
    }

    public string Text_YourTurn()
    {
        string text = "";
        if (spanish_language) text = "¡Tu turno!"; else text = "Your turn!";
        return text;
    }

    public string Text_InfoButton(bool activeStatus)
    {
        string text = "";
        if (activeStatus)
        {
            if (spanish_language) text = "Cerrar info"; else text = "Close info";
        }else
        {
            if (spanish_language) text = "Abrir info"; else text = "Open info";
        }
        return text;
    }

    public string Text_HabilidadFallada()
    {
        string text = "";
        if (spanish_language) text = "La habilidad ha fallado"; else text = "The skill has failed";
        return text;
    }

}
