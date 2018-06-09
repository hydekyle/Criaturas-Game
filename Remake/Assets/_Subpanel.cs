using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _Subpanel : MonoBehaviour {

    Transform online, vsIA;
    Image capa_extra, emblema;

    Vector3 pos_IA = new Vector3(-55, 71, 0);
    Vector3 pos_online = new Vector3(56, 71, 0);
    Vector3 final_scale = new Vector3(1.6f, 1.6f, 1);
    public Color green_flojo, green_fuerte;

    void OnEnable()
    {
        online = online ?? transform.Find("Online");
        vsIA = vsIA ?? transform.Find("vsIA");
        capa_extra = capa_extra ?? transform.parent.Find("Button_Versus").Find("Capa_Extra").GetComponent<Image>();
        emblema = emblema ?? transform.parent.Find("Button_Versus").Find("Capa_Extra").Find("Image").Find("Emblema").GetComponent<Image>();
        online.localPosition = vsIA.localPosition = Vector3.zero; //Colocarlas en su sitio al iniciar
        online.localScale = vsIA.localScale = Vector3.zero;
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            float t = Time.deltaTime * 10;
            online.localPosition = Vector3.Lerp(online.localPosition, pos_online, t);
            vsIA.localPosition = Vector3.Lerp(vsIA.localPosition, pos_IA, t);
            online.localScale = Vector3.Lerp(online.localScale, final_scale, t);
            vsIA.localScale = Vector3.Lerp(vsIA.localScale, final_scale, t);
            capa_extra.color = Color.Lerp(capa_extra.color, green_fuerte, t);
            emblema.color = Color.Lerp(emblema.color, green_flojo, t / 5);
        }
    }

    void OnDisable()
    {
        capa_extra.color = emblema.color = Color.white;
        gameObject.SetActive(false);
    }
}
