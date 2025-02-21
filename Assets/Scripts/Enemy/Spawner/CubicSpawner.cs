using Core;
using UnityEngine;

public class CubicSpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    [SerializeField] private Transform startingPoint;
    [SerializeField] private Transform control1;
    [SerializeField] private Transform control2;
    [SerializeField] private Transform endPoint;

    [SerializeField] private float centerSpeed = 5f;
    [SerializeField] private float speedVariation = 1f;
    
    [SerializeField] private float spawnRate = 1f;
    private float spawnTime;

    private void Update()
    {
        this.spawnTime += Time.deltaTime;

        if (this.spawnTime >= this.spawnRate)
        {
            this.spawnTime = 0f;
            GameObject instantiatedEnemy = Instantiate(this.enemy, this.startingPoint.position, Quaternion.identity);
            CubicMovement cubicMovement = instantiatedEnemy.GetComponent<CubicMovement>();
            cubicMovement.startingPoint = this.startingPoint;
            cubicMovement.control0 = this.control1;
            cubicMovement.control1 = this.control2;
            cubicMovement.endPoint = this.endPoint;
            
            cubicMovement.StartEnemy(Random.Range(this.centerSpeed - this.speedVariation, this.centerSpeed + this.speedVariation));
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 lastPosition = this.startingPoint?.position ?? Vector3.zero;
        float i = 0f;
        
        while (i <= 100f)
        {
            i += 1f;
            Vector3 position = Lerp.CubicBezier(this.startingPoint.position, this.control1.position, this.control2.position, this.endPoint.position, i/100);
            
            Debug.DrawLine(lastPosition, position, i <= 1f ? Color.red : Color.magenta);
            lastPosition = position;
        }
    }
}
