using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChosenOneController : HumanController
{
    public void Update()
    {
        base.GatherInputs();
        GatherInputs();
        
    }
    new void GatherInputs()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ((AccionesNeo) m_Acciones).ActivateFlight();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ((AccionesNeo) m_Acciones).DeactivateFlight();
        }

        float z = this.movimientoVertical();
        ((AccionesNeo) m_Acciones).Volar(z);
    }

    private float movimientoVertical()
    {
        if (Input.GetKey(KeyCode.Space))
            return 1.0f;
        if (Input.GetKey(KeyCode.LeftShift))
            return -1.0f;
        return 0.0f;
    }
}
