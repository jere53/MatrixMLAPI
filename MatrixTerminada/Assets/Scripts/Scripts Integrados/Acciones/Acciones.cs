using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class Acciones : NetworkBehaviour
{
    protected AvatarSync AvatarSync;

    protected bool isTalking = false; //para apagar las acciones mientras hablamos con el bot.
    // Start is called before the first frame update
    void Start()
    {
        //se asigna aca para no tener que pedir el componente cada vez que realizamos una accion.
        AvatarSync = GetComponent<AvatarSync>();
    }

    public void Move(float x, float y, float mouseX)
    {
        if (isTalking) return;
        //le pedimos al servidor que nos mueva. La RPC se deja en otro script para que quede mas organizado
        AvatarSync.MoveAvatarServerRpc(x,y, mouseX);
        //Le decimos al server nuestros valores de input. El server los usa para calcular nuestro moviento.
        //Nos mueve y luego sincroniza nuestra posicion
    }

    public void Jump()
    {
        if (isTalking) return;
        AvatarSync.JumpAvatarServerRpc();
    }

    public void Disparar()
    {
        if (isTalking) return;

        AvatarSync.DispararServerRpc();
    }

    public void Agarrar()
    {
        if (isTalking) return;

        AvatarSync.AgarrarServerRpc();
    }

    public void ToggleApuntarArma()
    {
        if (isTalking) return;

        AvatarSync.ToggleApuntarArmaServerRpc();
    }

    public void TalkToBot(){
        if (isTalking) return;

        AvatarSync.TalkToBotServerRPC(NetworkManager.LocalClientId);

        isTalking = true;
    }

    public void StopTalk(){
        AvatarSync.StopTalkServerRpc(NetworkManager.LocalClientId);
        isTalking = false;
    }
    
    public void AgarrarCabeza()
    {
        if (isTalking) return;

        AvatarSync.AgarrarCabezaServerRpc();
    }

    public void EstirarBrazos()
    {
        if (isTalking) return;

        AvatarSync.EstirarBrazosServerRpc();
    }

    public void EstirarEspalda()
    {
        if (isTalking) return;

        AvatarSync.EstirarEspaldaServerRpc();
    }
}
