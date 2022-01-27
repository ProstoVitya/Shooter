using UnityEngine;

public class Target : MonoBehaviour
{
    [Min(1f)] public float heath = 50f;

    public void TakeDamage(float amount)
    {
        heath -= amount;

        if (heath <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
