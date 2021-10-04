using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionesNeo : AccionesAgente
{
    // Mismo start que el padre.

    public void ActivateFlight()
    {
        Debug.Log(AvatarSync.IsFlying.Value);
        AvatarSync.ActivateFlightServerRpc();
    }

    public void DeactivateFlight()
    {
        Debug.Log(AvatarSync.IsFlying.Value);
        AvatarSync.DeactivateFlightServerRpc();
    }

    public void Volar(float z)
    {
        AvatarSync.VolarServerRpc(z);
    }
}