using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public Slider healthSlider;
    public GameObject gameOverPanel;

    private void Start()
    {
    currentHealth = maxHealth;
    healthSlider.maxValue = maxHealth;
    UpdateHealthUI();
    gameOverPanel.SetActive(false);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(maxHealth * 0.1f);  // 10% damage
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
        float healthPercent = currentHealth / maxHealth;
        if (healthPercent > 0.5f)
            healthSlider.fillRect.GetComponent<Image>().color = Color.green;
        else if (healthPercent > 0.2f)
            healthSlider.fillRect.GetComponent<Image>().color = Color.yellow;
        else
            healthSlider.fillRect.GetComponent<Image>().color = Color.red;
    }

    private void Die()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
