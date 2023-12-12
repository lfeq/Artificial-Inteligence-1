using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {
    [SerializeField] private float health = 100;
    [SerializeField] private Image healthBar;

    public void takeDamage(float t_damage) {
        health -= t_damage;
        healthBar.fillAmount = health / 100;
        if (health < 0) {
            Destroy(gameObject);
        }
    }
}