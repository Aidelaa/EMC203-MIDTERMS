using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        this.playerHealth = FindFirstObjectByType<PlayerHealth>();

        this.playerHealth.HealthChanged.AddListener(() =>
        {
            if (this.playerHealth.Health <= 0)
            {
                this.gameOverPanel.SetActive(true);
            }
        });   
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
