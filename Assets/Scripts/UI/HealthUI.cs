using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private PlayerHealth playerHealth;

    private void Start()
    {
        this.playerHealth = FindFirstObjectByType<PlayerHealth>();

        this.playerHealth.HealthChanged.AddListener(() =>
        {
            float health = this.playerHealth.Health;
            
            float healthCompute = (health / this.playerHealth.MaxHealth) * 100;
            
            Debug.Log($"`HealthChanged: {health} {this.playerHealth.MaxHealth} {healthCompute} {health/ this.playerHealth.MaxHealth}%");
            foreach (Image heart in this.hearts)
            {
                heart.color = Int32.Parse(heart.name) >= healthCompute ? new Color(.4f, 0, 0) : Color.white;
            }
        });
    }
}
