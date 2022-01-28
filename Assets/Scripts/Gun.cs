using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Weapon Characteristics")]
    [Min(0f)] public float Damage = 10f;
    [Min(1f)] public float Range = 100f;
    [Min(0.1f)] public float FireRate;
    [Min(0f)] public float ImpactForce;
    [Min(1)] public int MaxAmmo = 10;
    [Min(1)] public int MaxAmmoInTheChamber = 10;
    [Min(1f)] public float ReloadTime = 1f;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private GameObject _impactEffect;

    private Camera _playerCamera;
    private bool _isReloading = false;
    private float _nextTimeToFire;

    public int TotalCurrentAmmo { get; private set; }
    public int CurrentAmmoInTheChamber { get; private set; }

    private void Start()
    {
        _playerCamera = GetComponentInParent<Camera>();
        TotalCurrentAmmo = MaxAmmo;
        CurrentAmmoInTheChamber = MaxAmmoInTheChamber;
    }

    private void OnEnable()
    {
        _isReloading = false;
    }

    private void Update()
    {
        if (!_isReloading)
        {
            if (Input.GetKey(KeyCode.R) && CurrentAmmoInTheChamber < MaxAmmoInTheChamber && TotalCurrentAmmo > 0)
            {
                StartCoroutine(Reload());
                return;
            }

            if (Input.GetButton("Fire1") && CurrentAmmoInTheChamber > 0 && Time.time >= _nextTimeToFire)
            {
                Shoot();
                _nextTimeToFire = Time.time + 1f / FireRate;
            }
        }
    }
    private void Shoot()
    {
        _muzzleFlash.Play();
        --CurrentAmmoInTheChamber;
        RaycastHit hit;
        if (Physics.Raycast(_playerCamera.transform.position,
            _playerCamera.transform.forward, out hit, Range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
                target.TakeDamage(Damage);

            if (hit.rigidbody != null)
                hit.rigidbody.AddForce(-hit.normal * ImpactForce);

            Instantiate(_impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        yield return ReloadingAnimation();
        if (TotalCurrentAmmo + CurrentAmmoInTheChamber >= MaxAmmoInTheChamber)
        {
            TotalCurrentAmmo -= MaxAmmoInTheChamber - CurrentAmmoInTheChamber;
            CurrentAmmoInTheChamber = MaxAmmoInTheChamber;
        }
        else
        {
            CurrentAmmoInTheChamber = TotalCurrentAmmo;
            TotalCurrentAmmo = 0;
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
