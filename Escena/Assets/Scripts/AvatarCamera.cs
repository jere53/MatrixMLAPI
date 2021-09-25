using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class AvatarCamera : NetworkBehaviour
{
    public GameObject cameraMountPoint;

    private Rigidbody avatarBody;

    private float xRot = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        //solo nos interesa que este habilitado para el cliente que controla el avatar.
        if (!IsClient || !IsOwner)
        {
            enabled = false;
            return;
        }
        
        //si es el avatar asociado al cliente, le metemos la camara principal en la cara.
        Transform cameraTransform = Camera.main.gameObject.transform;
        cameraTransform.parent = cameraMountPoint.transform;
        cameraTransform.position = cameraMountPoint.transform.position;
        cameraTransform.rotation = cameraMountPoint.transform.rotation;

        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //miramos arriba o abajo. Esto se hace en el cliente porque solo modifica su camara (su vista) no toca ningun
        //dato que le interese al servidor.
        float mouseY = Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;
        //el 100f es el valor de sensibilidad del mouse.
        
        
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRot, 0, 0);
    }
}
