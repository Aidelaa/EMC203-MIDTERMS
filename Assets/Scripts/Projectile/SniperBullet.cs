using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    private Transform target;
    private float speed = 10f;
    private float damage = 25f;
    private float slowEffect = 0.2f;
    private float slowDuration = 2f;
    private bool canPierce = false;
    private bool canFreeze = false;
    private bool canCrit = false;

    public void SetTarget(Transform newTarget, int towerLevel)
    {
        target = newTarget;
        ApplyUpgrades(towerLevel);
    }

    private void ApplyUpgrades(int level)
    {
        switch (level)
        {
            case 2:
                damage *= 1.5f;
                slowDuration += 1f;
                break;
            case 3:
                canCrit = true;
                break;
            case 4:
                canPierce = true;
                break;
            case 5:
                canFreeze = true;
                break;
        }
    }

    private void Update()
    {
        if (!target) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();

        if (enemyHealth)
        {
            float finalDamage = damage;
            if (canCrit && Random.value < 0.2f) // 20% chance for critical hit
            {
                finalDamage *= 2;
            }

            enemyHealth.TakeDamage(finalDamage);

            if (canFreeze)
            {
                enemyHealth.ApplyFreezeEffect(2f);
            }
            else
            {
                enemyHealth.ApplySlow(slowEffect, slowDuration);
            }
        }

        if (!canPierce)
        {
            Destroy(gameObject);
        }
    }
}
