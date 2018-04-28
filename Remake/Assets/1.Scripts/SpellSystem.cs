using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class SpellSystem : MonoBehaviour {

    GraphicRaycaster raycaster;
    EventSystem eventSystem;
    PointerEventData pointerData;

    public static SpellSystem instance;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        raycaster = raycaster ?? GetComponent<GraphicRaycaster>();
        eventSystem = eventSystem ?? GetComponent<EventSystem>();
    }

    public void GoPatron(int n, Transform circleT, Transform pointerT, int dificultad)
    {
        List<int> lista2 = new List<int>();
        for (var x = 0; x < n - 1; x++) lista2.Add(x);
        lista2 = lista2.OrderBy(x => Random.value).ToList();
        StartCoroutine(Test(circleT, lista2, pointerT, dificultad));
    }

    IEnumerator Test(Transform circle, List<int> code, Transform pointer, int dificultad)
    {
        dificultad = Mathf.Clamp(dificultad, 2, 4);
        pointer.localPosition = circle.GetChild(code.Count).localPosition;
        float t = 0f;
        Vector3 posInicial = circle.GetChild(code.Count).localPosition;

        for (var x = 0; x < code.Count; x++)
        {
            Vector3 posFinal = circle.GetChild(code.IndexOf(x)).localPosition;

            while (t < 1.0f)
            {
                pointer.localPosition = Vector3.Lerp(posInicial, posFinal, t);
                t += Time.deltaTime * dificultad;
                yield return new WaitForEndOfFrame();
            }
            t = 0f;
            posInicial = posFinal;
        }

        
        
    }



    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);
            foreach (RaycastResult hit in results)
            {
                print(hit.gameObject.name);
            }
        }
    }


}
