using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionInput : NetworkBehaviour, PlayerControls.IActionsActions
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] Camera playerCam; 
    [SerializeField] private int damage;
    [SerializeField] ParticleSystem _gunParticleSystem;

    bool isReloading = false;
  
    PlayerControls _PlayerControls;

    [SerializeField] Gun currentGun;
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        _PlayerControls = new PlayerControls();
        _PlayerControls.Enable();

        _PlayerControls.Actions.Enable();
        _PlayerControls.Actions.SetCallbacks(this);
    }

    private void OnDisable()
    {
        _PlayerControls.Actions.Disable();
        _PlayerControls.Actions.RemoveCallbacks(this);

    }
    public void OnReload(InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            StartCoroutine(currentGun.Reload());
            StartCoroutine(ReloadTimer());
        }
    }

    IEnumerator ReloadTimer()
    {
        isReloading = true;
        yield return new WaitForSeconds(currentGun.GetReloadTime());
        isReloading = false;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            if (context.performed)
            {
                _gunParticleSystem.Play();
                currentGun.ShootGun();
                if(currentGun.GetGunAmmo() > 0 && !isReloading)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit,50,layerMask))
                    {
                         hit.transform.gameObject.GetComponent<PlayerNetwork>().TakeDamage(currentGun.damage);
                    }
                }
            }
        }    
    }
}
