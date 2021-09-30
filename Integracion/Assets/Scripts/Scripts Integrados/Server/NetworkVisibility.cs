using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Spawning;
using UnityEngine;

public class NetworkVisibility : NetworkBehaviour
{

    private void FixedUpdate()
    {
        if (!IsServer) return;
        //Debug.Log(NetworkManager.Singleton.ConnectedClients.Count);
        foreach (var participante in NetworkManager.Singleton.ConnectedClients)
        {
            ulong participanteClientID = participante.Key;
            //Debug.Log("Un cliente: " + participanteClientID);
            NetworkObject avatarParticipante = participante.Value.PlayerObject;
            //Debug.Log(NetworkSpawnManager.SpawnedObjects.Count + " Objetos Spawneados");
            foreach (var networkObject in NetworkSpawnManager.SpawnedObjects.Values)    
            {
                //Debug.Log("Caluclamos distancia para el cliente " + participanteClientID);
                if (Vector3.Distance(networkObject.transform.position, 
                    avatarParticipante.gameObject.transform.position) > 10f)
                {
                    //Debug.Log("Deberiamos esconderlo");
                    //Para cada NetworkObject en la escena, si ese objeto esta a mas de 100 metros de
                    //distancia de un avatar, lo escondemos de ese avatar. Sino, lo mostramos.
                    if (networkObject.IsNetworkVisibleTo(participanteClientID))
                    {
                        //solo lo escondamos si no esta escondido
                        //Debug.Log("Lo escondemos");
                        networkObject.NetworkHide(participanteClientID);
                    }
                }
                else
                {
                    //Debug.Log("Deberiamos mostrarlo: " + networkObject.name);
                    if (!networkObject.IsNetworkVisibleTo(participanteClientID))
                    {
                        networkObject.NetworkShow(participanteClientID);
                        //Debug.Log("Lo mostramos");
                    }
                }
            }
        }

    }

}
