using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBolt : MonoBehaviour
{
    public float speed = 15f; 
    public float damage = 10f;
    public float homingStrength = 0.5f; // Adjust homing curve
    public float areaDamageRadius = 0f; // Becomes nonzero at Level 2
    public int splitCount = 0; // Splitting at Level 4
    public bool chainLightning = false; // Chain at Level 5
    private Transform target;
    private Vector3[] bezierPoints = new Vector3[3];

    private void Start()
    {
        if (target)
            InitializeBezierPath();
    }

    private void Update()
    {
        if (target)
        {
            FollowBezierCurve();
        }
        else
        {
            //transform.position += transform.up * speed * Time.deltaTime;
        }
    }

    public void FireHoming(Transform newTarget, float speedMultiplier, float homingAccuracy, float damageBoost)
    {
        target = newTarget;
        speed *= speedMultiplier;
        homingStrength *= homingAccuracy;
        damage *= damageBoost;
        InitializeBezierPath();
    }

    private void InitializeBezierPath()
    {
        bezierPoints[0] = transform.position;
        bezierPoints[1] = (transform.position + target.position) / 2 + (Vector3)Random.insideUnitCircle * 2f;
        bezierPoints[2] = target.position;
        StartCoroutine(UpdateBezierCurve());
    }

    private void FollowBezierCurve()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, homingStrength * Time.deltaTime);
    }

    private IEnumerator UpdateBezierCurve()
    {
        while (target)
        {
            bezierPoints[1] = (transform.position + target.position) / 2 + (Vector3)Random.insideUnitCircle * 2f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth)
                enemyHealth.TakeDamage(damage);

            if (areaDamageRadius > 0) ApplyAreaDamage(collision.transform.position);
            if (splitCount > 0) SplitProjectile();
            if (chainLightning) ChainLightning(collision.gameObject);

            Destroy(gameObject);
        }
    }

    private void ApplyAreaDamage(Vector3 position)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(position, areaDamageRadius);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<EnemyHealth>()?.TakeDamage(damage / 2);
            }
        }
    }

    private void SplitProjectile()
    {
        for (int i = -1; i <= 1; i += 2)
        {
            GameObject split = Instantiate(gameObject, transform.position, Quaternion.identity);
            ArcaneBolt newBolt = split.GetComponent<ArcaneBolt>();
            if (newBolt)
            {
                newBolt.speed = speed;
                newBolt.transform.rotation = Quaternion.Euler(0, 0, i * 15f);
            }
        }
    }

    private void ChainLightning(GameObject firstTarget)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(firstTarget.transform.position, 5f);
        int chainHits = 3;

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.gameObject == firstTarget) continue;

            enemy.GetComponent<EnemyHealth>()?.TakeDamage(damage / 2);
            chainHits--;

            if (chainHits <= 0) break;
        }
    }
}
