using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth
    {
        get => this.maxHealth;
        private set => this.maxHealth = value;
    }
    private float maxHealth = 4;
    
    public UnityEvent HealthChanged = new UnityEvent();
    public float Health
    {
        get => this.health;
        set
        {
            this.health = Mathf.Clamp(value, 0, this.maxHealth);
            this.HealthChanged.Invoke();
        }
    }
    private float health = 4;
}
