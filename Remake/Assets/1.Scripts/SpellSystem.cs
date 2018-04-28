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

    public void GoPatron(int n, Transform circleT)
    {
        List<int> lista = new List<int>();
        List<int> lista2 = new List<int>();
        lista.Add(n);
        for (var x = 0; x < n - 1; x++) lista2.Add(x);
        lista2 = lista2.OrderBy(x => Random.value).ToList();
        foreach (int i in lista2) lista.Add(i);
        Test(circleT, lista);
    }

    void Test(Transform circle, List<int> code)
    {
        //circle.GetChild(code[0] - 1).gameObject.SetActive(false);
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
