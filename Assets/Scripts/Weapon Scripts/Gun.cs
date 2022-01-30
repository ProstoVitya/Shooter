using System.Collections;
using UnityEngine;

public class Gun : Weapon
{
    [Header("Gun Characteristics")]
    [Min(1)] public int MaxAmmo;
    [Min(1)] public int MaxAmmoInTheChamber;
    [Min(1f)] public float ReloadTime;
    public bool Tapable;
    public DataHandler.AmmoType AmmoType;

    [Header("Effects")]
    public ParticleSystem AttackEffect;

    private bool _isReloading = false;

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
        transform.parent.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void Update()
    {
        if (!_isReloading)
        {
            if (Input.GetKey(KeyCode.R) && CurrentAmmoInTheChamber < MaxAmmoInTheChamber && TotalCurrentAmmo > 0)
            {
                StartCoroutine(Reloading());
                return;
            }

            if ((Tapable ? Input.GetKeyDown(KeyCode.Mouse0) : Input.GetKey(KeyCode.Mouse0))
                && !_isAttacking && !_isReloading && CurrentAmmoInTheChamber > 0)
            {
                AttackEffect.Play();
                Shoot();
                StartCoroutine(AttackCooldown());
            }
        }
    }

    private IEnumerator Reloading()
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

    public void TakeAmmo(Ammo ammo)
    {
        if (AmmoType == ammo.AmmoType && TotalCurrentAmmo != MaxAmmo)
        {
            if (TotalCurrentAmmo + ammo.AmmoCount <= MaxAmmo)
                TotalCurrentAmmo += ammo.AmmoCount;
            else
                TotalCurrentAmmo = MaxAmmo;
            Destroy(ammo.gameObject);
        }
    }

    protected void Shoot()
    {
        --CurrentAmmoInTheChamber;
        Attack();
    }
}
