using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class HumanController : AvatarController
{
    protected Acciones m_Acciones;

    // Start is called before the first frame update
    void Start()
    {
        //si no somos el cliente dueño de este avatar, apagamos la input porque no tenemos que controlarlo.
        if (!IsClient || !IsOwner)
        {
            enabled = false;
        }
        
        m_Acciones = GetComponent<Acciones>();
    }

    public override void NetworkStart()
    {
        base.NetworkStart();
        if (IsClient)
        {
            Physics.autoSimulation = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GatherInputs();
    }

    protected void GatherInputs()
    {
        float x = Input.GetAxisRaw("Horizontal"); //adelante/atras
        float y = Input.GetAxisRaw("Vertical"); //strafe.
        float mouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime; //rotacion. 100f es la sensibilidad del mouse
        
        m_Acciones.Move(x, y, mouseX);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Acciones.Jump();
        }

        if (Input.GetMouseButtonDown(0)) //si hace click izquierdo
        {
            m_Acciones.Disparar();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_Acciones.Agarrar();
        }

        if (Input.GetMouseButtonDown(1))
        {
            m_Acciones.ToggleApuntarArma();
        }

        if (Input.GetKeyDown(KeyCode.H)){
            m_Acciones.TalkToBot();
        }

        if (Input.GetKeyDown(KeyCode.Escape)){
            m_Acciones.StopTalk();
        }

    }
}
