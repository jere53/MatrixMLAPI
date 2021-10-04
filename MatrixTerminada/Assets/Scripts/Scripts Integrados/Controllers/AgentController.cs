using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : AvatarController
{
    private AccionesAgente m_AccionesAgente;

    void Start()
    {
        if (!IsClient || !IsOwner)
        {
            enabled = false;
        }

        m_AccionesAgente = GetComponent<AccionesAgente>();
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

        m_AccionesAgente.Move(x, y, mouseX);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_AccionesAgente.Jump();
        }
        
        if (Input.GetKey(KeyCode.Q))
            m_AccionesAgente.AtravesarPared();
        
        if (Input.GetMouseButtonDown(0)) //si hace click izquierdo
        {
            m_AccionesAgente.Disparar();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_AccionesAgente.Agarrar();
        }

        if (Input.GetMouseButtonDown(1))
        {
            m_AccionesAgente.ToggleApuntarArma();
        }
    }
}
