using System;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void Start()
    {
        this.playerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void Damage()
    {
        if (this.playerHealth) this.playerHealth.Health -= 1;
    }
}
