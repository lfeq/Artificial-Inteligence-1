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

    #endregion Serializable variables

    #region Private variables

    private float attackRange;
    private Agent agent;
    private List<GameObject> enemiesPercibed = new List<GameObject>();
    private TowerAgentState towerState;
    private Transform target;
    private Rigidbody rb;
    private float shootTimer = 0;

    #endregion Private variables

    #region Unity functions

    private void Start() {
        agent = GetComponent<Agent>();
        rb = GetComponent<Rigidbody>();
        attackRange = agent.getEyeRadius();
    }

    private void Update() {
        shootTimer -= Time.deltaTime;
        if (shootTimer < 0) {
        }
    }

    private void FixedUpdate() {
        perceptionManager();
        decisonManager();
    }

    #endregion Unity functions

    #region Private functions

    /// <summary>
    /// Manages the perception of enemies through sight.
    /// </summary>
    private void perceptionManager() {
        // Sight
        enemiesPercibed.Clear();
        Collider[] percibed = Physics.OverlapSphere(agent.getEyePosition(), agent.getEyeRadius());
        foreach (Collider col in percibed) {
            if (col.CompareTag(enemyTag)) {
                enemiesPercibed.Add(col.gameObject);
            }
        }
    }

    /// <summary>
    /// Manages AI decision-making based on perceived enemies.
    /// </summary>
    private void decisonManager() {
        if (enemiesPercibed.Count == 0) {
            return;
        }
        int closestEnemy = 0;
        float minDistance = 1000f;
        for (int i = 0; i < enemiesPercibed.Count; i++) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemiesPercibed[i].transform.position);
            if (distanceToEnemy < minDistance) {
                closestEnemy = i;
                minDistance = distanceToEnemy;
            }
        }
        target = enemiesPercibed[closestEnemy].transform;
        if (minDistance < attackRange) {
            towerState = TowerAgentState.Attacking;
        } else {
            towerState = TowerAgentState.None;
        }
        switch (towerState) {
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
        switch (towerState) {
            case TowerAgentState.Attacking:
                shoot();
                break;
        }
    }

    /// <summary>
    /// Initiates an attack if the attack timer allows it.
    /// </summary>
    private void shoot() {
        if (shootTimer > 0) {
            return;
        }
        GameObject tempBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        BulletAgent bulletAgent = tempBullet.GetComponent<BulletAgent>();
        bulletAgent.setTarget(target);
        shootTimer = shootCooldown;
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