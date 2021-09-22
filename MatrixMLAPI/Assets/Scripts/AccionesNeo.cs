using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionesNeo : Acciones
{
    // Mismo start que el padre.

    public void ActivateFlight()
    {
        AvatarServerState.ActivateFlightServerRpc();
    }

    public void DeactivateFlight()
    {
        AvatarServerState.DeactivateFlightServerRpc();
    }
}
