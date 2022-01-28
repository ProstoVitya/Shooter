using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Target), typeof(Rigidbody), typeof(Collider))]
public class Distructible : MonoBehaviour
{
    [SerializeField] private GameObject _destroyetVersion;

    [SerializeField] private GameObject _drop;
    [SerializeField, Range(0f, 1f)] private float _dropChance;

    public void Destroy()
    {
        Instantiate(_destroyetVersion, transform.position, transform.rotation);
        Destroy(gameObject);

        foreach (Transform item in transform)
        {
            item.GetComponent<Rigidbody>().AddForce(transform.position.normalized, ForceMode.Impulse);
        }

        if(Random.Range(0f, 1f) <= _dropChance)
            Instantiate(_drop, transform.position, transform.rotation);
    }
}
