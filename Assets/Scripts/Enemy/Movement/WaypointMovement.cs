using System.Collections;
using System.Collections.Generic;
using Core;
using Tween;
using UnityEngine;

public class WaypointMovement : MonoBehaviour, IEnemyMovement
{
    public Transform endPoint;
    [SerializeField] private Transform[] waypoints;
    private EnemyBehavior enemyBehavior;

    private float originalSpeed;
    public float speed = 5f;
    private bool isFrozen = false;

    private void Start()
    {
        this.enemyBehavior = GetComponent<EnemyBehavior>();
        this.originalSpeed = speed;
    }

    public IEnumerator Move()
    {
        Vector3 lastPosition = this.transform.position;
        for (int i = 0; i < this.waypoints.Length; i++)
        {
            Transform waypoint = this.waypoints[i];
            float distance = (lastPosition - waypoint.position).magnitude;

            TweenObject tweenObject = new TweenObject(this.gameObject, this.transform, distance / this.speed, EasingStyle.Quadratic,
                i % 2 == 0 ? EasingDirection.In : EasingDirection.Out,
                new Dictionary<string, object>()
                {
                    {"position", waypoint.position}
                });

            TweenManager.instance.AddTweenObject(tweenObject);
            yield return new WaitForSeconds(tweenObject.duration);
        }

        float lastDist = (lastPosition - this.endPoint.position).magnitude;
        TweenObject endTween = new TweenObject(this.gameObject, this.transform, lastDist / this.speed,
            EasingStyle.Quadratic, EasingDirection.Out,
            new Dictionary<string, object>()
            {
                {"position", this.endPoint.position}
            });

        TweenManager.instance.AddTweenObject(endTween);
    }

    public void StartEnemy(Transform[] givenWaypoints)
    {
        this.waypoints = givenWaypoints;
        StartCoroutine(Move());
    }

    private void Update()
    {
        if ((this.transform.position - this.endPoint.position).magnitude <= 5f)
        {
            this.enemyBehavior.Damage();
            Destroy(this.gameObject);
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
