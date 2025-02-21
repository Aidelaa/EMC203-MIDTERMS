using UnityEngine;
using Core;

public class CubicMovement : MonoBehaviour, IEnemyMovement
{
    [SerializeField] public Transform startingPoint;
    [SerializeField] public Transform control0;
    [SerializeField] public Transform control1;
    [SerializeField] public Transform endPoint;

    private float originalSpeed;
    [SerializeField] private float speed = 5f;
    private bool isFrozen = false;

    private EnemyBehavior enemyBehavior;

    private void Start()
    {
        this.enemyBehavior = GetComponent<EnemyBehavior>();
        this.originalSpeed = speed;
    }

    public void StartEnemy(float? optionalSpeed)
    {
        this.speed = optionalSpeed ?? this.speed;
    }

    private void Update()
    {
        if (!isFrozen)
        {
            transform.position = Lerp.CubicBezier(startingPoint.position, control0.position, control1.position, endPoint.position, speed * Time.deltaTime);
        }

        if ((transform.position - endPoint.position).magnitude < 0.1f)
        {
            this.enemyBehavior.Damage();
            Destroy(gameObject);
        }
    }

    // ✅ Implement Interface Methods
    public void ApplySlow(float slowAmount, float duration)
    {
        if (!isFrozen)
        {
            speed = originalSpeed * (1 - slowAmount);
            Invoke(nameof(ResetSpeed), duration);
        }
    }

    public void ApplyFreezeEffect(float duration)
    {
        isFrozen = true;
        speed = 0;
        Invoke(nameof(Unfreeze), duration);
    }

    private void ResetSpeed()
    {
        speed = originalSpeed;
    }

    private void Unfreeze()
    {
        isFrozen = false;
        speed = originalSpeed;
    }
}
