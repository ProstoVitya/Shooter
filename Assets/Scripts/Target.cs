using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    [Min(1f)] public float heath = 50f;
    [SerializeField] private UnityEvent _onDestroy;

    public void TakeDamage(float amount)
    {
        heath -= amount;

        if (heath <= 0f)
        {
            if (_onDestroy != null)
                _onDestroy.Invoke();
        }
    }
}
