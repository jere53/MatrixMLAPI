using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Elector : MonoBehaviour
{

    public bool picked = false;

    public void HumanController()
    {
        picked = true;
        Debug.Log("Yo soy humano!");
    }

    public void ChosenOneController()
    {
        picked = true;
        Debug.Log("Yo soy el ELEGIDO");
    }

}