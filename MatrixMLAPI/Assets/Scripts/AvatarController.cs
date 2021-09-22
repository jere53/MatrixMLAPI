using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class AvatarController : NetworkBehaviour
{
    public override void NetworkStart()
    {
        base.NetworkStart();
        if (IsClient)
        {
            Physics.autoSimulation = false;
        }
    }
    
}
