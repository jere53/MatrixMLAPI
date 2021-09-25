using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEditor;
using UnityEngine;

public class ServerMovement : NetworkBehaviour
{
    //mueve a un avatar del lado del servidor (la matrix). Se separa aca para estar mas organizado.

    private Rigidbody m_Rigidbody;

    private AvatarServerState m_AvatarServerState;
    // Start is called before the first frame update
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AvatarServerState = GetComponent<AvatarServerState>();
    }

    public override void NetworkStart()
    {
        base.NetworkStart();
        if (!IsServer)
        {
            //solo el servidor va a usar este componente.
            enabled = false;
            return;
        }
        
        m_AvatarServerState.InitNetworkPositionAndRotationY(transform.position, transform.rotation.eulerAngles.y);
    }

    private void FixedUpdate()
    {
        HacerMovimiento();
        
        //actualizamos las variables de posicion para que las vean los clientes
        m_AvatarServerState.NetworkPosition.Value = transform.position;
        m_AvatarServerState.NetworkRotationY.Value = transform.rotation.eulerAngles.y;
    }

    public void HacerMovimiento()
    {
        //la direccion en la que nos queremos mover. La input horizontal dice si nos movemos a la derecha, y la vertical
        //si nos movemos hacia adelante.
        Vector3 movement = transform.right * m_AvatarServerState.AxisX.Value + 
                           transform.forward * m_AvatarServerState.AxisY.Value;

        //multiplicamos la direccion de movimiento por la velocidad (en m/s, para eso multiplicamos por T.fdt)
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement * (m_AvatarServerState.Speed.Value * Time.fixedDeltaTime));
        
        //rotamos alrededor de Y segun movamos el mouse para el costado.
        m_Rigidbody.rotation = Quaternion.Euler(m_Rigidbody.rotation.eulerAngles + 
                                                new Vector3(0f, m_AvatarServerState.MouseX.Value, 0f));
    }
    
}
