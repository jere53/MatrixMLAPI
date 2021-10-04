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

    private AvatarSync _mAvatarSync;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        _mAvatarSync = GetComponent<AvatarSync>();
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
        
        m_Rigidbody = GetComponent<Rigidbody>();
        _mAvatarSync = GetComponent<AvatarSync>();
        _mAvatarSync.InitNetworkPositionAndRotationY(transform.position, transform.rotation.eulerAngles.y);
    }

    private void FixedUpdate()
    {
        HacerMovimiento();
        
        //actualizamos las variables de posicion para que las vean los clientes
        _mAvatarSync.NetworkPosition.Value = transform.position;
        _mAvatarSync.NetworkRotationY.Value = transform.rotation.eulerAngles.y;
    }

    public void HacerMovimiento()
    {
        //la direccion en la que nos queremos mover. La input horizontal dice si nos movemos a la derecha, y la vertical
        //si nos movemos hacia adelante.
        Vector3 movement = transform.right * _mAvatarSync.AxisX.Value + 
                           transform.forward * _mAvatarSync.AxisY.Value +
                           transform.up * _mAvatarSync.AxisZ.Value;

        //multiplicamos la direccion de movimiento por la velocidad (en m/s, para eso multiplicamos por T.fdt)
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement * (_mAvatarSync.Speed.Value * Time.fixedDeltaTime));
        
        //rotamos alrededor de Y segun movamos el mouse para el costado.
        m_Rigidbody.rotation = Quaternion.Euler(m_Rigidbody.rotation.eulerAngles + 
                                                new Vector3(0f, _mAvatarSync.MouseX.Value, 0f));
    }
    
}
