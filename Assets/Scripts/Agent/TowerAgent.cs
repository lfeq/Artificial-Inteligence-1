using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TowerAgent class represents an agent that behaves like a tower.
/// </summary>
[RequireComponent(typeof(Agent))]
public class TowerAgent : MonoBehaviour {

    #region Serializable variables

    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float damage = 10f;

    #endregion Serializable variables

    #region Private variables

    private float m_attackRange;
    private Agent m_agent;
    private List<GameObject> m_enemiesPercibed = new List<GameObject>();
    private TowerAgentState m_towerState;
    private Transform m_target;
    private Rigidbody m_rb;
    private float m_shootTimer = 0;

    #endregion Private variables

    #region Unity functions

    private void Start() {
        m_agent = GetComponent<Agent>();
        m_rb = GetComponent<Rigidbody>();
        m_attackRange = m_agent.getEyeRadius();
    }

    private void Update() {
        m_shootTimer -= Time.deltaTime;
        if (m_shootTimer < 0) {
        }
    }

    private void FixedUpdate() {
        perceptionManager();
        decisonManager();
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Called when the tower is destroyed, triggering a level event and destroying the game object.
    /// </summary>
    public void onTowerDestroy() {
        LevelManager.instance.towerDestroyed();
        Destroy(gameObject);
    }

    #endregion Public functions

    #region Private functions

    /// <summary>
    /// Manages the perception of enemies through sight.
    /// </summary>
    private void perceptionManager() {
        // Sight
        m_enemiesPercibed.Clear();
        Collider[] percibed = Physics.OverlapSphere(m_agent.getEyePosition(), m_agent.getEyeRadius());
        foreach (Collider col in percibed) {
            if (col.CompareTag(enemyTag)) {
                m_enemiesPercibed.Add(col.gameObject);
            }
        }
    }

    /// <summary>
    /// Manages AI decision-making based on perceived enemies.
    /// </summary>
    private void decisonManager() {
        if (m_enemiesPercibed.Count == 0) {
            return;
        }
        int closestEnemy = 0;
        float minDistance = 1000f;
        for (int i = 0; i < m_enemiesPercibed.Count; i++) {
            float distanceToEnemy = Vector3.Distance(transform.position, m_enemiesPercibed[i].transform.position);
            if (distanceToEnemy < minDistance) {
                closestEnemy = i;
                minDistance = distanceToEnemy;
            }
        }
        m_target = m_enemiesPercibed[closestEnemy].transform;
        if (minDistance < m_attackRange) {
            m_towerState = TowerAgentState.Attacking;
        } else {
            m_towerState = TowerAgentState.None;
        }
        switch (m_towerState) {
            case TowerAgentState.None:
                break;
            case TowerAgentState.Attacking:
                actionManager();
                break;
        }
    }

    /// <summary>
    /// Manages movement based on the current AI state.
    /// </summary>
    private void actionManager() {
        switch (m_towerState) {
            case TowerAgentState.Attacking:
                shoot();
                break;
        }
    }

    /// <summary>
    /// Initiates an attack if the attack timer allows it.
    /// </summary>
    private void shoot() {
        if (m_shootTimer > 0) {
            return;
        }
        GameObject tempBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        BulletAgent bulletAgent = tempBullet.GetComponent<BulletAgent>();
        bulletAgent.setTarget(m_target, damage);
        m_shootTimer = shootCooldown;
    }

    #endregion Private functions

    #region Enums

    /// <summary>
    /// Enumeration representing different states of the TowerAgent.
    /// </summary>
    public enum TowerAgentState {
        None,
        Attacking,
    }

    #endregion Enums
}