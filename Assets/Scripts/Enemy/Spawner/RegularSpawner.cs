using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class RegularSpawner : MonoBehaviour
{
    
    [SerializeField] private GameObject enemy;
    
    [SerializeField] private Transform startingPoint;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform endPoint;

    [SerializeField] private float speedCenter = 10f;
    [SerializeField] private float speedDeviation = 2f;
    [SerializeField] private float spawnRate = 1f;
    private float spawnTime;

    private void Update()
    {
        this.spawnTime += Time.deltaTime;

        if (this.spawnTime >= this.spawnRate)
        {
            this.spawnTime = 0f;
            GameObject instantiatedEnemy = Instantiate(this.enemy, this.startingPoint.position, Quaternion.identity);
            WaypointMovement movement = instantiatedEnemy.GetComponent<WaypointMovement>();
            movement.endPoint = this.endPoint;
            movement.speed = this.speedCenter + Random.Range(-this.speedDeviation, this.speedDeviation);
            movement.StartEnemy(this.waypoints);
        }
    }

    private void OnDrawGizmos()
    {   
        Vector3 lastPosition = this.startingPoint.position;
        for (int i = 0; i < this.waypoints.Length; i++)
        {
            Transform waypoint = this.waypoints[i];
            Debug.DrawLine(lastPosition, waypoint.position, Color.magenta);
            Handles.Label(waypoint.position, $"Waypoint: {i}");
            
            lastPosition = waypoint.position;
        }
        
        Debug.DrawLine(lastPosition, endPoint.position, Color.magenta);
    }
}
