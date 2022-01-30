using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Characteristics")]
    [Min(1f)] public float Range;
    [Min(0f)] public float Damage;
    [Min(0.1f)] public float AttackTemp;
    [Min(0f)] public float ImpactForce;

    protected bool _isAttacking;
    protected Camera _playerCamera;

    protected void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(_playerCamera.transform.position,
            _playerCamera.transform.forward, out hit, Range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
                target.TakeDamage(Damage);

            if (hit.rigidbody != null)
                hit.rigidbody.velocity += _playerCamera.transform.forward * ImpactForce;
        }
    }

    protected IEnumerator AttackCooldown()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(1f / AttackTemp);
        _isAttacking = false;
    }
}
