using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class Message : MonoBehaviour {

    public static Message instance{ get; set; }
    public float fadeVelocity = 12.0f;
    Text text;
    Text text_shadow;
    Color fadeOutTextColor;
    Color fadeOutShadowColor;
    Transform t_cofreVIP;

    void Awake()
    {
        instance = this;
        Initialize();
    }

    void Initialize()
    {
        text = transform.Find("Text").GetComponent<Text>();
        text_shadow = transform.Find("Text_Shadow").GetComponent<Text>();
        t_cofreVIP = transform.Find("Cofre_VIP");
    }

    public void NewMessage(string message)
    {
        text.text = message;
        text_shadow.text = message;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 255);
        text_shadow.color = new Color(text_shadow.color.r, text_shadow.color.g, text_shadow.color.b, 255);
    }

    void LateUpdate()
    {
        text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), Time.deltaTime * fadeVelocity);
        text_shadow.color = Color.Lerp(text_shadow.color, new Color(text_shadow.color.r, text_shadow.color.g, text_shadow.color.b, 0), Time.deltaTime * fadeVelocity);
    }

    public void MostrarCofresVIP()
    {
        int coronas = GameManager.instance.userdb.coronas;
        if (coronas < 4)
        {
            t_cofreVIP.Find("Text").GetComponent<Text>().text = string.Format("{0} / 4", coronas);
            t_cofreVIP.GetComponent<Animator>().Play("Cofre_VIP_show");
        }else
        {
            UserDB userdb = GameManager.instance.userdb;
            UserDB newData = new UserDB()
            {
                chests = userdb.chests,
                chests_VIP = userdb.chests_VIP + 1,
                coronas = 0,
                derrotas = userdb.derrotas,
                gold = userdb.gold,
                gold_VIP = userdb.gold_VIP,
                victorias = userdb.victorias
            };

            Database.instance.ReferenceDB().Child("data").SetRawJsonValueAsync(JsonUtility.ToJson(newData)).ContinueWith(task => {
                t_cofreVIP.Find("Text").GetComponent<Text>().text = "¡Cofre obtenido!";
                t_cofreVIP.GetComponent<Animator>().Play("Cofre_VIP_show");
            });

        }
        
    }

}
