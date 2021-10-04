using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

public class AvatarSync : NetworkBehaviour
//Contiene el estado del avatar en el mundo central, es decir la matrix. Tambien contiene las Client -> Server RPCs
//que modifican ese estado.
{
    public GameObject weaponMountPoint;
    public GameObject arma;
    public NetworkVariableVector3 NetworkPosition { get; } = new NetworkVariableVector3();

    public NetworkVariableFloat NetworkRotationY { get; } = new NetworkVariableFloat();

    public NetworkVariableFloat AxisY { get; } = new NetworkVariableFloat();

    public NetworkVariableFloat AxisX { get; } = new NetworkVariableFloat();

    public NetworkVariableFloat AxisZ { get; } = new NetworkVariableFloat();

    public NetworkVariableFloat MouseX { get; } = new NetworkVariableFloat();
    
    public NetworkVariableFloat Speed { get; } = new NetworkVariableFloat(2.0f);

    public NetworkVariableBool IsFlying { get; } = new NetworkVariableBool();

    public NetworkVariableBool IsAiming { get; } = new NetworkVariableBool(false);

    public NetworkVariableString Name;

    private Animator _animator;
    public void InitNetworkPositionAndRotationY(Vector3 initPosition, float initRotationY)
    {
        NetworkPosition.Value = initPosition;
        NetworkRotationY.Value = initRotationY;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //sincronizamos el estado de los objetos en el cliente con el estado de los objetos en el server
        if (IsClient)
        {
            transform.position = NetworkPosition.Value;
            transform.rotation = Quaternion.Euler(0, NetworkRotationY.Value, 0);
            //GetComponent<Animator>().SetFloat("VelX", MouseX.Value);
        }
        //sincronizamos las animaciones de movimiento
        _animator.SetFloat("VelY", AxisY.Value);
        _animator.SetBool("ApuntadoArma", IsAiming.Value);
        _animator.SetBool("Volando", IsFlying.Value);
    }

    #region movimiento

    [ServerRpc]
    public void MoveAvatarServerRpc(float x, float y, float mouseX)
    {
        AxisX.Value = x;
        AxisY.Value = y;
        MouseX.Value = mouseX;
    }

    [ServerRpc]
    public void JumpAvatarServerRpc()
    {
        if (IsFlying.Value) return;
        
        GetComponent<Rigidbody>().AddForce(new Vector3(0f, 250f, 0f), ForceMode.Impulse);
        _animator.SetTrigger("Saltar");
        PlayJumpAnimationClientRpc();
    }

    [ServerRpc]
    public void VolarServerRpc(float z)
    {
        if (IsFlying.Value)
        {
            AxisZ.Value = z;
        }
        else
            AxisZ.Value = 0.0f;
    }
    
    [ServerRpc]
    public void JumpAgentServerRpc()
    {
        if (IsFlying.Value) return;
        
        GetComponent<Rigidbody>().AddForce(new Vector3(0f, 500f, 0f), ForceMode.Impulse);
        _animator.SetTrigger("Saltar");
        PlayJumpAnimationClientRpc();
    }

    #endregion

    #region estado
    
    [ServerRpc]
    public void ActivateFlightServerRpc()
    {
        IsFlying.Value = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    [ServerRpc]
    public void DeactivateFlightServerRpc()
    {
        IsFlying.Value = false;
        GetComponent<Rigidbody>().useGravity = true;
    }

    [ServerRpc]
    public void AtravesarParedServerRpc()
    {
        //Desactivo el collider para poder atraversar una pared (aunque podría atravesar cualquier cosa).
        //Una mejor implementación sería taggear las paredes y que se desactiven los colliders de los edificios, supongo
        Rigidbody m_rigidbody = GetComponent<Rigidbody>();
        Collider m_Collider = GetComponent<Collider>();
        
        //desactivo gravedad porque sino cae (provisorio)
        m_rigidbody.useGravity = !m_rigidbody.useGravity;
        
        //Se setea el collider al valor contrario al que tiene por lo que se puede usar para activar y desactivar
        m_Collider.enabled = !m_Collider.enabled;
    }
    
    [ClientRpc]
    public void DieClientRpc()
    {
        //Nos pegaron un tiro, morimos:
        _animator.SetTrigger("HitByBullet");
        GetComponent<AvatarController>().enabled = false;
    }
    
    #endregion

    #region armas
    
    [ServerRpc]
    public void DispararServerRpc()
    {
        if (arma == null) return;
        arma.GetComponent<Pistola>().Shoot();
    }

    [ServerRpc]
    public void AgarrarServerRpc()
    {
        //creamos una esfera al rededor del avatar y tomamos un collider correspondiente a una pistola (layer 6)

        var colliders = Physics.OverlapSphere(transform.position, 10f);
        if (colliders != null)
        {
            foreach (var c in colliders)
            {
                if (c.gameObject.CompareTag("Pistola"))
                {
                    arma = c.gameObject;
                    break;
                }
            }
        } else return;
        
        if (arma == null) return;
        //llevamos el arma a la mano del avatar, y hacemos que su transform se vuelva hija de la transform de la mano
        //asi se mueven juntas.
        Transform armaTransform = arma.transform;
        armaTransform.parent = weaponMountPoint.transform;
        armaTransform.position = weaponMountPoint.transform.position;
        armaTransform.rotation = weaponMountPoint.transform.rotation;
    }

    [ServerRpc]
    public void ToggleApuntarArmaServerRpc()
    {
        IsAiming.Value = !IsAiming.Value;
    }
    
    #endregion

    #region interaccion_chatbot
    
    [ServerRpc]
    public void TalkToBotServerRPC(ulong clientID) {
        var colliders = Physics.OverlapSphere(transform.position, 5f);
        if (colliders != null) {
            foreach (var c in colliders) {
                if (c.gameObject.CompareTag("Bot")) {
                    var chatManager = c.gameObject.GetComponent<ChatManager>();
                    if (chatManager.talkingPartner != 0) return; //ya esta hablando con alguien
                    chatManager.talkingPartner = clientID; 
                    ActivateBotUIClientRpc(clientID);
                    break;
                }
            }
        } else return;
    }

    [ServerRpc] 
    public void StopTalkServerRpc(ulong clientID) {
        var colliders = Physics.OverlapSphere(transform.position, 5f);
        if (colliders != null) {
            foreach (var c in colliders) {
                if (c.gameObject.CompareTag("Bot")) {
                    var chatManager = c.gameObject.GetComponent<ChatManager>();
                    if (chatManager.talkingPartner != clientID) return; //esta hablando con alguien mas
                    chatManager.talkingPartner = 0; 
                    DeactivateBotUIClientRpc(clientID);
                    break;
                }
            }
        } else return;

    }

    [ClientRpc]
    public void ActivateBotUIClientRpc(ulong clientID){
        if (NetworkManager.LocalClientId != clientID) {
            return;
        }
        GameObject botUI = GameObject.Find("----- UI -----/Rasa/BotUI");
        botUI.GetComponent<BotUI>().UICanvas.SetActive(true);
    }

    [ClientRpc]
    public void DeactivateBotUIClientRpc(ulong clientID){
        if (NetworkManager.LocalClientId != clientID) {
            return;
        }
        GameObject botUI = GameObject.Find("----- UI -----/Rasa/BotUI");
        botUI.GetComponent<BotUI>().UICanvas.SetActive(false);
    }
    
    #endregion
    
    #region animaciones

    [ServerRpc]
    public void AgarrarCabezaServerRpc()
    {
        _animator.SetTrigger("AgarrarCabeza");
        AgarrarCabezaClientRpc(); //para que los clientes tambien reproduzcan la animacion
    }

    [ClientRpc]
    private void AgarrarCabezaClientRpc()
    {
        _animator.SetTrigger("AgarrarCabeza");
    }

    [ServerRpc]
    public void EstirarBrazosServerRpc()
    {
        _animator.SetTrigger("EstirarBrazos");
        EstirarBrazosClientRpc();
    }

    [ClientRpc]
    private void EstirarBrazosClientRpc()
    {
        _animator.SetTrigger("EstirarBrazos");
    }

    [ServerRpc]
    public void EstirarEspaldaServerRpc()
    {
        _animator.SetTrigger("EstirarEspalda");
        EstirarEspaldaClientRpc();
    }

    [ClientRpc]
    private void EstirarEspaldaClientRpc()
    {
        _animator.SetTrigger("EstirarEspalda");
    }

    [ClientRpc]
    public void PlayJumpAnimationClientRpc()
    {
        _animator.SetTrigger("Saltar");
    }
    
    #endregion

    [ServerRpc]
    public void SetNameServerRpc(string name)
    {
        Name.Value = name;
    }
}