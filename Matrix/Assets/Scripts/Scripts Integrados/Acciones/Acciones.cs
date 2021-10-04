using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class Acciones : NetworkBehaviour
{
    protected AvatarSync AvatarSync;
    // Start is called before the first frame update
    void Start()
    {
        //se asigna aca para no tener que pedir el componente cada vez que realizamos una accion.
        AvatarSync = GetComponent<AvatarSync>();
    }

    public void Move(float x, float y, float mouseX)
    {
        //le pedimos al servidor que nos mueva. La RPC se deja en otro script para que quede mas organizado
        AvatarSync.MoveAvatarServerRpc(x,y, mouseX);
        //Le decimos al server nuestros valores de input. El server los usa para calcular nuestro moviento.
        //Nos mueve y luego sincroniza nuestra posicion
    }

    public void Jump()
    {
        AvatarSync.JumpAvatarServerRpc();
    }

    public void Disparar()
    {
        AvatarSync.DispararServerRpc();
    }

    public void Agarrar()
    {
        AvatarSync.AgarrarServerRpc();
    }

    public void ToggleApuntarArma()
    {
        AvatarSync.ToggleApuntarArmaServerRpc();
    }

    public void TalkToBot(){
        AvatarSync.TalkToBotServerRPC(NetworkManager.LocalClientId);
    }

    public void StopTalk(){
        AvatarSync.StopTalkServerRpc(NetworkManager.LocalClientId);
    }
    
    public void AgarrarCabeza()
    {
        AvatarSync.AgarrarCabezaServerRpc();
    }

    public void EstirarBrazos()
    {
        AvatarSync.EstirarBrazosServerRpc();
    }

    public void EstirarEspalda()
    {
        AvatarSync.EstirarEspaldaServerRpc();
    }
}
