using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

public class AvatarServerState : NetworkBehaviour
//Contiene el estado del avatar en el mundo central, es decir la matrix. Tambien contiene las Client -> Server RPCs
//que modifican ese estado.
{
    public NetworkVariableVector3 NetworkPosition { get; } = new NetworkVariableVector3();

    public NetworkVariableFloat NetworkRotationY { get; } = new NetworkVariableFloat();

    public NetworkVariableFloat AxisY { get; } = new NetworkVariableFloat();

    public NetworkVariableFloat AxisX { get; } = new NetworkVariableFloat();

    public NetworkVariableFloat AxisZ { get; } = new NetworkVariableFloat();

    public NetworkVariableFloat MouseX { get; } = new NetworkVariableFloat();
    
    public NetworkVariableFloat Speed { get; } = new NetworkVariableFloat(2.0f);

    public NetworkVariableBool isFlying { get; } = new NetworkVariableBool();

    private Animator _animator;
    public void InitNetworkPositionAndRotationY(Vector3 initPosition, float initRotationY)
    {
        NetworkPosition.Value = initPosition;
        NetworkRotationY.Value = initRotationY;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //sincronizamos el estado de los objetos en el cliente con el estado de los objetos en el server
        if (IsClient)
        {
            transform.position = NetworkPosition.Value;
            transform.rotation = Quaternion.Euler(0, NetworkRotationY.Value, 0);
            _animator.SetFloat("VelY", AxisY.Value);
            //GetComponent<Animator>().SetFloat("VelX", MouseX.Value);
        }
    }
    
    [ServerRpc]
    public void MoveAvatarServerRpc(float x, float y, float mouseX)
    {
        AxisX.Value = x;
        AxisY.Value = y;
        MouseX.Value = mouseX;
    }

    [ServerRpc]
    public void JumpAvatarServerRpc()
    {
        if (isFlying.Value != true)
            GetComponent<Rigidbody>().AddForce(new Vector3(0f, 2f, 0f), ForceMode.Impulse);
    }

    [ServerRpc]
    public void ActivateFlightServerRpc()
    {
        isFlying.Value = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    [ServerRpc]
    public void DeactivateFlightServerRpc()
    {
        isFlying.Value = false;
        GetComponent<Rigidbody>().useGravity = true;
    }

    [ServerRpc]
    public void VolarServerRpc(float z)
    {
        if (isFlying.Value)
        {
            AxisZ.Value = z;
        }
        else
            AxisZ.Value = 0.0f;
    }
    
    [ServerRpc]
    public void JumpAgentServerRpc()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0f, 10f, 0f), ForceMode.Impulse);
    }
    /*[ServerRpc]
    public void AtacarServerRpc()
    {
        //Podríamos empezar con una animación por el lado del agente y si llegamos agregar la reacción
        //al avatar que le llegue el ataque
        throw new System.NotImplementedException();
    }*/
    [ServerRpc]
    public void AtravesarParedServerRpc()
    {
        //Desactivo el collider para poder atraversar una pared (aunque podría atravesar cualquier cosa).
        //Una mejor implementación sería taggear las paredes y que se desactiven los colliders de los edificios, supongo
        Rigidbody m_rigidbody = GetComponent<Rigidbody>();
        Collider m_Collider = GetComponent<Collider>();
        
        //desactivo gravedad porque sino cae (provisorio)
        m_rigidbody.useGravity = !m_rigidbody.useGravity;
        
        //Se setea el collider al valor contrario al que tiene por lo que se puede usar para activar y desactivar
        m_Collider.enabled = !m_Collider.enabled;
    }

}