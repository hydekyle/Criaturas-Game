using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class SpellSystem : MonoBehaviour {

    public Sprite spr_on;

    GraphicRaycaster raycaster;
    EventSystem eventSystem;
    PointerEventData pointerData;
    Transform spellGameT;
    string finalCode;

    public static SpellSystem instance;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        raycaster = raycaster ?? GetComponent<GraphicRaycaster>();
        eventSystem = eventSystem ?? GetComponent<EventSystem>();
        spellGameT = spellGameT ?? transform.Find("SpellGame");
    }

    public void Iniciar()
    {
        spellGameT.gameObject.SetActive(true);
    }


    public void GoPatron(int n, Transform circleT, Transform pointerT, int dificultad, GameObject trazo)
    {
        List<int> lista2 = new List<int>();
        for (var x = 0; x < n - 1; x++) lista2.Add(x);
        lista2 = lista2.OrderBy(x => Random.value).ToList();
        StartCoroutine(Trazado(circleT, lista2, pointerT, dificultad, trazo));
    }

    IEnumerator Trazado(Transform circle, List<int> code, Transform pointer, int dificultad, GameObject trazo)
    {
        dificultad = Mathf.Clamp(dificultad, 2, 9);
        pointer.localPosition = circle.GetChild(code.Count).localPosition;
        float t = 0f;
        Vector3 posInicial = circle.GetChild(code.Count).localPosition;
        Encender(code.Count, circle);
        for (var x = 0; x < code.Count; x++)
        {
            Vector3 posFinal;
            GameObject newTrazo;

            finalCode += code.IndexOf(x).ToString();
            posFinal = circle.GetChild(code.IndexOf(x)).localPosition;
            newTrazo = Instantiate(trazo);
            newTrazo.transform.SetParent(circle.parent.Find("Trazado"), false);
            newTrazo.transform.localPosition = Vector3.zero;
            newTrazo.SetActive(true);

            while (t < 1.0f)
            {
                pointer.localPosition = Vector3.Lerp(posInicial, posFinal, t);
                t += Time.deltaTime * dificultad;
                float angle = Mathf.Atan2(pointer.localPosition.y - posFinal.y, pointer.localPosition.x - posFinal.x) * Mathf.Rad2Deg; //ÁNGULO ENTRE DOS PUNTOS
                newTrazo.transform.localRotation = Quaternion.Euler(Vector3.forward * angle);
                newTrazo.transform.localPosition = (pointer.localPosition + posInicial) / 2;
                newTrazo.transform.localScale = new Vector3(Vector3.Distance(pointer.localPosition, posInicial), newTrazo.transform.localScale.y, newTrazo.transform.localScale.z);
                yield return new WaitForEndOfFrame();
            }
            Encender(code.IndexOf(x), circle);
            t = 0f;
            posInicial = posFinal;
        }

    }

    void Encender(int n, Transform circle)
    {
        try
        {
            circle.Find(n.ToString()).GetComponent<Image>().sprite = spr_on;
        }catch
        {
            print("Final del circuito");
        }
        
    }

    void Update()
    {
        if (spellGameT.gameObject.activeSelf)
        {
            if (Input.GetMouseButton(0))
            {
                pointerData = new PointerEventData(eventSystem);
                pointerData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerData, results);
                foreach (RaycastResult hit in results)
                {
                    LeerPuntero(hit.gameObject.name);
                }
            }
        }
    }

    void LeerPuntero(string name)
    {
        if(name.Length < 2)
        {
            print(name + "    " + finalCode);
        }
    }

}
