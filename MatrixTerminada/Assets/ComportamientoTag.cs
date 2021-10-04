using System.Collections;
using System.Collections.Generic;
using MLAPI;
using TMPro;
using UnityEngine;

public class ComportamientoTag : NetworkBehaviour
{
    private TextMeshPro nametag;
    // Start is called before the first frame update
    void Start()
    {
        nametag = GetComponent<TextMeshPro>();
        if (IsLocalPlayer)
        {
            //Si es el jugador local, tiene la referencia al nombre que escribio, entonces lo setteamos en el AvatarSync
            //para que el resto de los jugadores lo vean
            string name;
            name = GameObject.Find("/----- UI -----/LogInUI").GetComponent<LogInUI>().GetName(); //conseguimos el nombre
            gameObject.transform.parent.gameObject.GetComponent<AvatarSync>().SetNameServerRpc(name);
            nametag.text = name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer) return;
        string nombre = gameObject.transform.parent.gameObject.GetComponent<AvatarSync>().Name.Value;
        
        if (nombre != "")
        {
            nametag.text = nombre;
            enabled = false;
        }
    }
}