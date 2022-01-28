using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BulletsCounter : MonoBehaviour
{
    [SerializeField] private Transform _weaponHandler;
    private Gun _currentWeapon;
    private Text _bulletsCounterText;

    private void Start()
    {
        _bulletsCounterText = GetComponent<Text>();
        FindActiveWeapon();
        _bulletsCounterText.text = $"{_currentWeapon.MaxAmmo}/{_currentWeapon.MaxAmmoInTheChamber}";
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
                _currentWeapon = weapon.GetComponent<Gun>();
                return;
            }
        }
    }

    private void SetBulletsCounterText()
    {
        _bulletsCounterText.text = $"{_currentWeapon.TotalCurrentAmmo}/{_currentWeapon.CurrentAmmoInTheChamber}";
    }

    private IEnumerator WaitForReloadAnimation()
    {
        yield return new WaitForSeconds(_currentWeapon.ReloadTime);
        SetBulletsCounterText();
    }
}
