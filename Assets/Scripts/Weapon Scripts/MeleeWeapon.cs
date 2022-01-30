using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    private void Start()
    {
        _playerCamera = GetComponentInParent<Camera>();
    }

    private void OnEnable()
    {
        transform.localPosition = new Vector3(0f, 0f, 0.5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && ! _isAttacking)
        {
            Attack();
            
            StartCoroutine(AttackAnimation());
        }
    }

    IEnumerator AttackAnimation()
    {
        _isAttacking = true;
        for (int i = 0; i < 30; i++)
        {
            transform.position += _playerCamera.transform.forward * 0.01f;
            yield return null;
        }

        for (int i = 0; i < 30; i++)
        {
            transform.position -= _playerCamera.transform.forward * 0.01f;
            yield return null;
        }
        _isAttacking = false;
    }
}
