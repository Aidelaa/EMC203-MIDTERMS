using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    private IEnemyMovement enemyMovement;

    private void Start()
    {
        enemyMovement = GetComponent<IEnemyMovement>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void ApplySlow(float slowMultiplier, float duration)
    {
        enemyMovement?.ApplySlow(slowMultiplier, duration);
    }

    public void ApplyFreezeEffect(float duration)
    {
        enemyMovement?.ApplyFreezeEffect(duration);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
