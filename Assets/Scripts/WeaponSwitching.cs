using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    private int _selectedWeapon = 0;

    private void Update()
    {
        int previousWeapon = _selectedWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            _selectedWeapon = (_selectedWeapon + 1) % (transform.childCount);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (_selectedWeapon-- <= 0)
                _selectedWeapon = transform.childCount -1;
        }
        if (previousWeapon != _selectedWeapon)
            SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        int index = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(index == _selectedWeapon);
            ++index;
        }
    }

}
