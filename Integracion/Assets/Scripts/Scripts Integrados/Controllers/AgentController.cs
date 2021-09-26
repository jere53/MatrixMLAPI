using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : AvatarController
{
    private AccionesAgente m_AccionesAgente;
    private Rigidbody _rigidbody;

    void Start()
    {
        if (!IsClient || !IsOwner)
        {
            enabled = false;
        }

        m_AccionesAgente = GetComponent<AccionesAgente>();
        _rigidbody = GetComponent<Rigidbody>();
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
            m_AccionesAgente.JumpAgente();
        }
        //probar en fixed update porque a veces desactiva el collider y otras veces no
        if (Input.GetKey(KeyCode.Q))
            m_AccionesAgente.AtravesarPared();
        
        /*
        if (Input.GetMouseButtonDown(0))
            m_AccionesAgente.Atacar();
        */
        
        //otra acción: teletransportarse cierta distancia según a donde apunte el mouse
    }
}
