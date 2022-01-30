using System.Collections;
using UnityEngine;

public class Weapon2 : MonoBehaviour
{
    [Header("Throwing")]
    public float ThrowForce;
    public float ThrowExtraForce;
    public float RotationForce;

    [Header("Shooting")]
    public int MaxAmmo;
    public int FireRate;
    public float ReloadSpeed;
    public float HitForce;
    public float Range;
    public bool Tapable;

    [Header("Data")]
    public int WeaponGfxLayer;
    public GameObject[] WeaponGfxs;
    public Collider[] GfxColliders;
    
    private bool _held;
    private bool _reloading;
    private bool _shooting;
    private int _ammo;
    private Rigidbody _rigidbody;
    private Transform _playerCamera;

    private void Start()
    {
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.mass = 0.1f;
        _ammo = MaxAmmo;
    }

    private void Update()
    {
        if (!_held)
            return;

        if (Input.GetKeyDown(KeyCode.R) && !_reloading && _ammo < MaxAmmo)
        {
            StartCoroutine(ReloadingCooldown());
        }

        if ((Tapable ? Input.GetKeyDown(KeyCode.Mouse0) : Input.GetKey(KeyCode.Mouse0))
            && !_shooting && !_reloading)
        {
            --_ammo;
            Shoot();
            StartCoroutine(_ammo > 0 ? ShootigCooldown() : ReloadingCooldown());
        }
    }

    private void Shoot()
    {
        if (Physics.Raycast(_playerCamera.position, _playerCamera.forward, out var hitInfo, Range))
        {
            var rigidBody = hitInfo.transform.GetComponent<Rigidbody>();
            if (rigidBody != null)
                rigidBody.velocity += _playerCamera.forward * HitForce;
        }
    }

    private IEnumerator ShootigCooldown()
    {
        _shooting = true;
        yield return new WaitForSeconds(1f / FireRate);
        _shooting = false;
    }

    private IEnumerator ReloadingCooldown()
    {
        _reloading = true;
        yield return new WaitForSeconds(ReloadSpeed);
        _ammo = MaxAmmo;
        _reloading = false;
    }



    public void PickUp(Transform weaponHolder, Transform playerCamera)
    {
        if (_held) return;

        Destroy(_rigidbody);
        transform.parent = weaponHolder;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        foreach (var col in GfxColliders)
            col.enabled = false;

        foreach (var gdx in WeaponGfxs)
            gdx.layer = WeaponGfxLayer;
        _held = true;
        _playerCamera = playerCamera;
    }

    public void Drop(Transform playerCamera)
    {
        if (!_held) return;
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.mass = 0.1f;
        var forward = playerCamera.forward;
        forward.y = 0f;
        _rigidbody.velocity = forward * ThrowForce;
        _rigidbody.velocity += Vector3.up * ThrowExtraForce;
        _rigidbody.angularVelocity = Random.onUnitSphere * RotationForce;
        foreach (var col in GfxColliders)
            col.enabled = true;

        foreach (var gdx in WeaponGfxs)
            gdx.layer = 0;
        transform.parent = null;
        _held = false;
    }
}
