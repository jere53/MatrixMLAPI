using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionesNeo : Acciones
{
    // Mismo start que el padre.

    public void ActivateFlight()
    {
        Debug.Log(AvatarServerState.isFlying.Value);
        AvatarServerState.ActivateFlightServerRpc();
    }

    public void DeactivateFlight()
    {
        Debug.Log(AvatarServerState.isFlying.Value);
        AvatarServerState.DeactivateFlightServerRpc();
    }

    public void Volar(float z)
    {
        AvatarServerState.VolarServerRpc(z);
    }
}