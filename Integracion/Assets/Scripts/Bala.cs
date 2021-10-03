using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class Bala : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!IsServer) return; //solo el servidor debe manejar las colisiones
        if (other.gameObject.CompareTag("Pistola")) return; // si la bala choco con la pistola, que no haga nada
        
        if (other.gameObject.CompareTag("Player")) //si es un jugador, se muere.
        {
            other.gameObject.GetComponent<AvatarServerState>().DieClientRpc();
        }
        
        GetComponent<NetworkObject>().Despawn(); //la bala debe dejar de existir.
        Destroy(gameObject);
    }
}
