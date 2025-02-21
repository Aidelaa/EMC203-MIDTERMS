using System.Collections;
using UnityEngine;

public class FrostSniper : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float range = 5f;
    [SerializeField] private int level = 1;

    private bool onCooldown = false;

    private void Update()
    {
        if (!onCooldown)
        {
            Transform target = FindTarget();
            if (target != null)
            {
                Shoot(target);
            }
        }
    }

    private Transform FindTarget()
    {
        Transform closestEnemy = null;
        float closestDistance = range;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }

    private void Shoot(Transform target)
    {
        GameObject bullet = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        SniperBullet sniperBullet = bullet.GetComponent<SniperBullet>();

        if (sniperBullet)
        {
            sniperBullet.SetTarget(target, level);
        }

        StartCoroutine(ShootCooldown());
    }

    private IEnumerator ShootCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(1 / fireRate);
        onCooldown = false;
    }

    public void UpgradeTower()
    {
        if (level < 5) level++;
    }

    public interface IEnemyMovement
    {
        void ApplySlow(float slowAmount, float duration);
        void ApplyFreezeEffect(float duration);
    }

}
