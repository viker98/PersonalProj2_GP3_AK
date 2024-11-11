using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{
    int _health = 100;

    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private AudioListener audioListener;


    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            audioListener.enabled = true;
            vc.Priority = 1;
        }
        else
        {
            vc.Priority = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Their health is now {_health}");


            //GamePlayUI.Instance.SetHealthSlider(_health);
   
        
            
        if(_health <= 0)
        {
            RpcRespawnClientRpc();
        }

    }

    [ClientRpc]
    void RpcRespawnClientRpc()
    {
        if (IsLocalPlayer)
        {
            transform.position = Vector3.zero;
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(5);
    }
}
