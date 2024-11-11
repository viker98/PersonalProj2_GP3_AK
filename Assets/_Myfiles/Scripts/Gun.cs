using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] int _Ammo = 8;
    [SerializeField] int maxAmmo = 8;
    [SerializeField] int reloadTime = 2;
    [SerializeField] ParticleSystem _gunshotParticle;
    public int damage { get; private set; } = 10;
    void Start()
    {
        _Ammo = maxAmmo;
    }

    public int GetGunAmmo()
    {
        return _Ammo;
    }
    public int GetReloadTime()
    {
        return reloadTime;
    }
    public void ShootGun()
    {
        _Ammo -= 1;
    }
    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        _Ammo = maxAmmo;
    }
}
