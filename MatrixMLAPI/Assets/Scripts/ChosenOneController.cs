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
    }
}
