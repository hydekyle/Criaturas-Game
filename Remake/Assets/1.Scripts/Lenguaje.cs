using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lenguaje : MonoBehaviour {

    [HideInInspector]
    public static Lenguaje Instance;

    bool spanish_language;

    /// TEXTOS DEL JUEGO:
    [HideInInspector]
    public string play, nest, shop, exit, 
    health, attack, skill, luck;


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
                case 10: name = "Tortazo"; break;
                case 11: name = "Puñetazo"; break;
                case 12: name = "Golpe mortal"; break;
                case 13: name = "Embestida"; break;
            }
        } else {
            switch (ID)
            {
                case 10: name = "Swipe"; break;
                case 11: name = "Punch"; break;
                case 12: name = "Death blow"; break;
                case 13: name = "Charge"; break;
            }
        }
        
        return name;
    }

}
