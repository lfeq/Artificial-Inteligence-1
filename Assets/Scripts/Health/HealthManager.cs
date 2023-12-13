using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// HealthManager class handles the health and death events of a game object.
/// </summary>
public class HealthManager : MonoBehaviour {

    #region Serializable variables

    [SerializeField] private float health = 100;
    [SerializeField] private Image healthBar;
    [SerializeField] private UnityEvent deathEvent;

    #endregion Serializable variables

    #region Public functions

    /// <summary>
    /// Inflicts damage to the game object's health.
    /// </summary>
    /// <param name="t_damage">The amount of damage to be inflicted.</param>
    public void takeDamage(float t_damage) {
        health -= t_damage;
        healthBar.fillAmount = health / 100;
        if (health < 0) {
            deathEvent.Invoke();
        }
    }

    #endregion Public functions
}