using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class Acciones : NetworkBehaviour
{
    protected AvatarServerState AvatarServerState;
    // Start is called before the first frame update
    void Start()
    {
        //se asigna aca para no tener que pedir el componente cada vez que realizamos una accion.
        AvatarServerState = GetComponent<AvatarServerState>();
    }

    public void Move(float x, float y, float mouseX)
    {
        //le pedimos al servidor que nos mueva. La RPC se deja en otro script para que quede mas organizado
        AvatarServerState.MoveAvatarServerRpc(x,y, mouseX);
        //Le decimos al server nuestros valores de input. El server los usa para calcular nuestro moviento.
        //Nos mueve y luego sincroniza nuestra posicion
    }

    public void Jump()
    {
        AvatarServerState.JumpAvatarServerRpc();
    }

    public void Disparar()
    {
        AvatarServerState.DispararServerRpc();
    }

    public void Agarrar()
    {
        AvatarServerState.AgarrarServerRpc();
    }

    public void ToggleApuntarArma()
    {
        AvatarServerState.ToggleApuntarArmaServerRpc();
    }

    public void TalkToBot(){
        AvatarServerState.TalkToBotServerRPC(NetworkManager.LocalClientId);
    }

    public void StopTalk(){
        AvatarServerState.StopTalkServerRpc(NetworkManager.LocalClientId);
    }
}
