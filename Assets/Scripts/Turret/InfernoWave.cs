using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfernoWave: MonoBehaviour
{
    [Header("Tower Stats")]
    public float damage = 10f;
    public float range = 5f;
    public float burnDamage = 2f;
    public float burnDuration = 3f;
    public float fireRate = 2f;
    public float coneAngle = 45f; // The spread of the wave attack

    private int level = 1;
    private float nextFireTime = 0f;

    [Header("Wave Settings")]
    public GameObject fireWavePrefab; // The visual effect of the fire wave
    public Transform firePoint; // The point where waves are launched

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireWave();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void FireWave()
    {
        // Spawn fire wave effect
        if (fireWavePrefab)
        {
            GameObject fireWave = Instantiate(fireWavePrefab, firePoint.position, firePoint.rotation);
            fireWave.transform.localScale = Vector3.one * (1f + 0.2f * (level - 1)); // Scale dynamically with upgrades
            Destroy(fireWave, 1f); // Remove visual effect after a short duration
        }

        // Find all enemies in range
        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, range);
        foreach (Collider enemy in enemiesHit)
        {
            if (IsInCone(enemy.transform))
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth)
                {
                    enemyHealth.TakeDamage(damage);
                    StartCoroutine(BurnEffect(enemyHealth));
                }
            }
        }
    }

    private bool IsInCone(Transform enemy)
    {
        Vector3 directionToEnemy = (enemy.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToEnemy);
        return angle < coneAngle / 2f;
    }

    private IEnumerator BurnEffect(EnemyHealth enemyHealth)
    {
        float elapsed = 0f;
        while (elapsed < burnDuration)
        {
            enemyHealth.TakeDamage(burnDamage);
            elapsed += 1f;
            yield return new WaitForSeconds(1f);
        }
    }

    public void Upgrade()
    {
        if (level < 5)
        {
            level++;
            switch (level)
            {
                case 2:
                    range += 2f;
                    burnDamage += 1f;
                    break;
                case 3:
                    damage += 5f;
                    break;
                case 4:
                    coneAngle += 15f;
                    break;
                case 5:
                    damage += 10f;
                    burnDuration += 2f;
                    break;
            }
        }
    }
}
