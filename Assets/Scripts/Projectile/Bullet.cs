using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool started = false;
    private float updateRate = .05f;
    private float updateElapsed = 0f;
    private float despawn = 5f;
    
    public float speed = 18f;
    public float defaultRange = 1f;
    public float range = 1f;

    public bool isHoming = false;

    private Wallet wallet;
    private SpriteRenderer sprite;
    private Transform target;
   
    private UpgradeSystem upgradeSystem;

    private void Start()
    {
        this.sprite = GetComponent<SpriteRenderer>();
        this.wallet = Wallet.instance;
        this.upgradeSystem = FindFirstObjectByType<UpgradeSystem>();
        
        StartCoroutine(Despawn());

        this.range = this.defaultRange * (1 + (.2f * this.upgradeSystem.bulletStrengthUpgraded));
    }
    public void Fire(float? overwriteSpeed, float? overwriteRange)
    {
        this.speed = overwriteSpeed ?? this.speed;
        this.range = overwriteRange ?? this.range;

        this.started = true;
    }

    public void FireHoming(Transform target, float? overwriteSpeed, float? overwriteRange)
    {
        this.isHoming = true;
        
        this.target = target;
        this.transform.localScale = new Vector3(.5f, .5f, .5f);
        // this.sprite.color = Color.magenta;
        
        Fire(overwriteSpeed, overwriteRange);

        this.speed = this.speed / 3f;
    }

    private void Move() => this.transform.position -= this.transform.right * (this.speed * Time.deltaTime);

    private void MoveToTarget()
    {
        if (!this.target)
        {
            Destroy(this.gameObject);
            return;
        };

        this.transform.position = Vector3.Lerp(this.transform.position, this.target.position, this.speed * Time.deltaTime);
    }

    private void CheckIfHit()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if ((enemy.transform.position - this.transform.position).magnitude < this.range)
            {
                this.wallet.SpawnGold(5, enemy.transform.position);
                Destroy(enemy);
                Destroy(this.gameObject);
            }
        }
    }

    private void Update()
    {
        if (!this.isHoming)
            Move();
        else
            MoveToTarget();

        this.updateElapsed += Time.deltaTime;
        if (this.updateElapsed >= this.updateRate)
            CheckIfHit();
    }
    
    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(this.despawn);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        DebugExtension.DrawCircle(this.transform.position, this.range, 32, Color.red);
        Debug.DrawRay(this.transform.position, this.transform.right * -this.range, Color.green);
    }
}
