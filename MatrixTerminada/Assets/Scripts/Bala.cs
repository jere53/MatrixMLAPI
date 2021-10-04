using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class Bala : NetworkBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!IsServer) return; //solo el servidor debe manejar las colisiones
        if (other.gameObject.CompareTag("Pistola")) return; // si la bala choco con la pistola, que no haga nada
        
        if (other.gameObject.CompareTag("Player")) //si es un jugador, se muere.
        {
            other.gameObject.GetComponent<AvatarSync>().DieClientRpc();
        }
        
        GetComponent<NetworkObject>().Despawn(); //la bala debe dejar de existir.
        Destroy(gameObject);
    }
}
