using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AccionesAgente : Acciones
{
    // Start is called before the first frame update
    public new void Jump()
    {
        AvatarSync.JumpAgentServerRpc();
    }
   /*public void Atacar()
    {
        AvatarSync.AtacarServerRpc();
    }*/

    public void AtravesarPared()
    {
        AvatarSync.AtravesarParedServerRpc();
    }
 
}
