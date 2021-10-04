using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Boton_Selector : MonoBehaviour
{
    public int id;
    public String prefab;

    // Start is called before the first frame update
    void Start()
    {
        Acciones_Botones.evento_press += cambiar_Color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void cambiar_Color(int entrada)
    {
        if (entrada == id)
        {
            transform.GetComponent<Image>().color = new Color32(93, 221, 62, 255);
            transform.parent.GetComponent<Prefab_picked>().picked = prefab;
;
        }
        else
        {
            transform.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void presionado()
    {
        Acciones_Botones.evento_press(id);
    }

}

public static class Acciones_Botones
{
    public static Action<int> evento_press;
}
