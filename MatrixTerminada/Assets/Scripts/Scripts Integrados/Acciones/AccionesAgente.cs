using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AccionesAgente : Acciones
{
    public new void Jump()
    {
        if (isTalking) return;

        AvatarSync.JumpAgentServerRpc();
    }

    public void AtravesarPared()
    {
        if (isTalking) return;

        AvatarSync.AtravesarParedServerRpc();
    }
 
}
