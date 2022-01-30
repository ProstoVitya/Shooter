using UnityEngine;

[RequireComponent(typeof(Target), typeof(Rigidbody), typeof(Collider))]
public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject _destroyetVersion;

    [Header("Drop Parameters")]
    [SerializeField] private GameObject _drop;
    [SerializeField, Range(0f, 1f)] private float _dropChance;

    public void Destroy()
    {
        Instantiate(_destroyetVersion, transform.position, transform.rotation);
        Destroy(gameObject);

        if(Random.Range(0f, 1f) <= _dropChance)
            Instantiate(_drop, transform.position, Quaternion.identity);
    }
}
