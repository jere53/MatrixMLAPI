using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empleada1 : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) //tecla c activa el trigger hablar
        {
            anim.SetTrigger("Hablar");
        }
    }
}
