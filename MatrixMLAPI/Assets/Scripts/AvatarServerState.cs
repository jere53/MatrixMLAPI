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
    
    public void InitNetworkPositionAndRotationY(Vector3 initPosition, float initRotationY)
    {
        NetworkPosition.Value = initPosition;
        NetworkRotationY.Value = initRotationY;
    }
    
    // Update is called once per frame
    void Update()
    {
        //sincronizamos el estado de los objetos en el cliente con el estado de los objetos en el server
        if (IsClient)
        {
            transform.position = NetworkPosition.Value;
            transform.rotation = Quaternion.Euler(0, NetworkRotationY.Value, 0);
            GetComponent<Animator>().SetFloat("VelY", AxisY.Value);
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
        Debug.Log("holaaa");
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
            Debug.Log(z);
        }
        else
            AxisZ.Value = 0.0f;
    }

}