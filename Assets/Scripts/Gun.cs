using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Weapon Characteristics")]
    [Min(0f)] public float Damage = 10f;
    [Min(1f)] public float Range = 100f;
    [Min(0.1f)] public float FireRate;
    [Min(1)] public int MaxAmmo = 10;
    [Min(1)] public int MaxAmmoInTheChamber = 10;
    [Min(1f)] public float ReloadTime = 1f;


    [Header("Particles")]
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private GameObject _impactEffect;

    private Camera _playerCamera;
    private int _totalCurrentAmmo;
    private int _currentAmmoInTheChamber;
    private bool _isReloading = false;
    private float _nextTimeToFire;

    private void Start()
    {
        _playerCamera = GetComponentInParent<Camera>();
        _totalCurrentAmmo = MaxAmmo;
        _currentAmmoInTheChamber = MaxAmmoInTheChamber;
    }

    private void OnEnable()
    {
        _isReloading = false;
    }

    private void Update()
    {
        if (!_isReloading)
        {
            if (Input.GetKey(KeyCode.R))
            {
                StartCoroutine(Reload());
                return;
            }

            if (Input.GetButton("Fire1") && _currentAmmoInTheChamber > 0 && Time.time >= _nextTimeToFire)
            {
                Shoot();
                _nextTimeToFire = Time.time + 1f / FireRate;
            }
        }
    }
    private void Shoot()
    {
        _muzzleFlash.Play();
        --_currentAmmoInTheChamber;
        RaycastHit hit;
        if (Physics.Raycast(_playerCamera.transform.position,
            _playerCamera.transform.forward, out hit, Range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(Damage);
            }
            Instantiate(_impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        yield return ReloadingAnimation();
        if (_totalCurrentAmmo + _currentAmmoInTheChamber >= MaxAmmoInTheChamber)
        {
            _totalCurrentAmmo -= MaxAmmoInTheChamber - _currentAmmoInTheChamber;
            _currentAmmoInTheChamber = MaxAmmoInTheChamber;
            
        }
        else
        {
            _currentAmmoInTheChamber = _totalCurrentAmmo;
            _totalCurrentAmmo = 0;
        }
        _isReloading = false;
    }

    private IEnumerator ReloadingAnimation()
    {
        var weaponHandler = transform.parent;
        for (int i = 0; i < 10; i++)
        {
            weaponHandler.transform.Rotate(1f, 0f, 0f);
            yield return null;
        }
        yield return new WaitForSeconds(ReloadTime);
        for (int i = 0; i < 10; i++)
        {

            weaponHandler.transform.Rotate(-1f, 0f, 0f);
            yield return null;
        }
    }
}
