using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BulletsCounter : MonoBehaviour
{
    [SerializeField] private Transform _weaponHandler;
    private Weapon _currentWeapon;
    private Text _bulletsCounterText;

    private void Start()
    {
        _bulletsCounterText = GetComponent<Text>();
        FindActiveWeapon();
        if(_currentWeapon.GetType().Equals(typeof(Gun)))
            _bulletsCounterText.text = $"{((Gun)_currentWeapon).MaxAmmo}/{((Gun)_currentWeapon).MaxAmmoInTheChamber}";
        else
            _bulletsCounterText.text = $"";
    }

    private void LateUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            FindActiveWeapon();
            SetBulletsCounterText();
        }
        else if (Input.GetButton("Fire1"))
            SetBulletsCounterText();
        else if (Input.GetKey(KeyCode.R))
            StartCoroutine(WaitForReloadAnimation());
    }

    private void FindActiveWeapon()
    {
        foreach (Transform weapon in _weaponHandler)
        {
            if (weapon.gameObject.activeSelf)
            {
                _currentWeapon = weapon.GetComponent<Weapon>();
                return;
            }
        }
    }

    private void SetBulletsCounterText()
    {
        if (_currentWeapon.GetType().Equals(typeof(Gun)))
            _bulletsCounterText.text = $"{((Gun)_currentWeapon).TotalCurrentAmmo}/{((Gun)_currentWeapon).CurrentAmmoInTheChamber}";
        else _bulletsCounterText.text = $"";
    }

    private IEnumerator WaitForReloadAnimation()
    {
        yield return new WaitForSeconds(((Gun)_currentWeapon).ReloadTime);
        SetBulletsCounterText();
    }
}
