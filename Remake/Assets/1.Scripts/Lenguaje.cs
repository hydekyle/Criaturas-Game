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
        if (spanish_language){
            switch (ID)
            {
                case 0  : name = "Tortazo";         break;
                case 1  : name = "Puñetazo";        break;
                case 2  : name = "Golpe mortal";    break;
                case 3  : name = "Embestida";       break;
                case 11 : name = "Golpe Testeo";    break;
                case 40 : name = "Curación";        break;
            }
        } else {
            switch (ID)
            {
                case 0  : name = "Swipe";       break;
                case 1  : name = "Punch";       break;
                case 2  : name = "Death blow";  break;
                case 3  : name = "Charge";      break;
                case 40 : name = "Heal";        break;
            }
        }
        
        return name;
    }

    public string Text_YourTurn()
    {
        string text = "";
        if (spanish_language) text = "¡Tu turno!"; else text = "Your turn!";
        return text;
    }

}
