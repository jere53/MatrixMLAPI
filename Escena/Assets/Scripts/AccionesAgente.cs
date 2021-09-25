using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AccionesAgente : Acciones
{
    // Start is called before the first frame update
    void Start()
    {
        AvatarServerState = GetComponent<AvatarServerState>();
    }
   public void JumpAgente()
    {
        AvatarServerState.JumpAgentServerRpc();
    }
   /*public void Atacar()
    {
        AvatarServerState.AtacarServerRpc();
    }*/

    public void AtravesarPared()
    {
        AvatarServerState.AtravesarParedServerRpc();
    }
 
}
