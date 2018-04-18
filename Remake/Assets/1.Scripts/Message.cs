using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour {

    public static Message instance{ get; set; }
    public float fadeVelocity = 12.0f;
    Text text;
    Text text_shadow;
    Color fadeOutTextColor;
    Color fadeOutShadowColor;

    void Awake()
    {
        instance = this;
        Initialize();
    }

    void Initialize()
    {
        text = transform.Find("Text").GetComponent<Text>();
        text_shadow = transform.Find("Text_Shadow").GetComponent<Text>();
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

}
