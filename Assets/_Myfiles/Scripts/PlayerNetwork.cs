using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<int> _health = new NetworkVariable<int>(100);

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
    private void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                _health.Value -= 10;
                Debug.Log($"{OwnerClientId} Health is now {_health.Value}");
            }
        }

    }
}
