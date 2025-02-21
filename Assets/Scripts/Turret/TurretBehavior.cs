using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class TurretBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetPosition;

    [SerializeField] private float findUpdateRate = 0.16f;
    [SerializeField] private float defaultFireRate = 4f;
    [SerializeField] private float fireRate = 4f; // (1/fireRate)
    [SerializeField] private float defaultRange = 4f;
    [SerializeField] private float range = 4f;
    [SerializeField] private float defaultRotationSpeed = 3f;
    private float rotationSpeed = 3f;

    [SerializeField] private bool onCooldown = false;
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzle;

    private float updateElapsed;
    private GameObject gunBarrel;
    private UpgradeSystem upgradeSystem;

    
    private void DetermineIfUpgraded()
    {
        if (!this.upgradeSystem) return;

        this.range = this.defaultRange * (1.2f * (.3f * this.upgradeSystem.rangeUpgraded));
        this.fireRate = this.defaultFireRate * (1.1f * (.4f * this.upgradeSystem.turretSpeedUpgraded));
        this.rotationSpeed = this.defaultRotationSpeed * (1.5f * (.2f * this.upgradeSystem.turretSpeedUpgraded));
    }
    
    private void Awake()
    {
        this.gunBarrel = this.transform.Find("Gun").gameObject;
        this.upgradeSystem = FindFirstObjectByType<UpgradeSystem>();

        this.upgradeSystem?.UpgradeCall?.AddListener((string name) =>
        {
            DetermineIfUpgraded();
        });
        DetermineIfUpgraded();
    }

    private void FindTarget()
    {
        if (this.target) return;
        
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Transform enemyTransform = enemy.transform;

            if ((enemyTransform.position - this.transform.position).magnitude < this.range)
            {
                this.target = enemyTransform;
            }
        }
    }

    private void RotateTurret(Vector3? targetPosition, float smoothness)
    {
        float angle = Core.VectorCalculations.LookAt(this.transform.position, targetPosition ?? (this.transform.position + Vector3.up));
        
        this.targetPosition = targetPosition ?? Vector3.zero;
        this.gunBarrel.transform.rotation = Quaternion.Lerp(this.gunBarrel.transform.rotation, Quaternion.Euler(0f, 0f, angle), smoothness);
    }

    private void CheckIfTargetValid()
    {
        if (!this.target) return;
        if ((this.transform.position - this.target.position).magnitude > this.range)
            this.target = null;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(this.bulletPrefab, new Vector3(this.muzzle.transform.position.x, this.muzzle.transform.position.y, 0f), this.gunBarrel.transform.rotation);
        bullet.transform.parent = null;
        Bullet bulletHandler = bullet.GetComponent<Bullet>();
        
        int randomizer = Random.Range(0, 8);
        if (randomizer == 0)
            bulletHandler.FireHoming(this.target.transform, null, null);
        else
            bulletHandler.Fire(null, null);
        
        StartCoroutine(ShootCooldown());
    }

    private void Update()
    {
        if (!this.target)
        {
            this.updateElapsed += Time.deltaTime;
            RotateTurret(this.transform.position + Vector3.up, Mathf.Clamp01(this.updateElapsed/2f));

            if (this.updateElapsed >= this.findUpdateRate)
            {
                FindTarget();
                this.updateElapsed = 0;
            }
        }
        else
        {
            CheckIfTargetValid();
            RotateTurret(this.target?.position, this.rotationSpeed * Time.deltaTime);
            
            if (!this.onCooldown)
                Shoot();
        }
        
    }

    private IEnumerator ShootCooldown()
    {
        this.onCooldown = true;
        yield return new WaitForSeconds(1/ this.fireRate);
        this.onCooldown = false;
    }

    private void OnDrawGizmos()
    {
        DebugExtension.DrawCircle(this.transform.position, this.range, 32, Color.yellow);
    }
}
