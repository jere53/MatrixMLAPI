using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;

public class Pistola : NetworkBehaviour
{
    public NetworkVariableVector3 NetworkPosition { get; } = new NetworkVariableVector3();

    public NetworkVariableVector3 NetworkRotation { get; } = new NetworkVariableVector3();
    
    public GameObject bulletPrefab;
    [SerializeField] private Transform barrelLocation; //de donde sale la bala
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 200f;//con cuanta fuerza sale la bala
    // Start is called before the first frame update
    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;
    }


    private void FixedUpdate()
    {
        if (IsServer)
        {
            NetworkPosition.Value = transform.position;
            NetworkRotation.Value = transform.rotation.eulerAngles;
        } else if (IsClient)
        {
            transform.position = NetworkPosition.Value;
            transform.rotation = Quaternion.Euler(NetworkRotation.Value);
        }
    }

    // Update is called once per frame
    public void Shoot()
    {
        //instanciamos una bala en la salida del barril 
        NetworkObject n = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation)
            .GetComponent<NetworkObject>();
        
        n.Spawn(); //que se replique en la red
        
        //aplicamos la fuerza de salida en la direccion del barril
        n.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
    }
}
