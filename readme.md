# Robots vs Wizards

![gamescreenshot](readmeImages/Game%20Screenshot.png)

## Overview

Robots vs Wizards is a simple top-down shooter developed in Unity for an artificial intelligence class. In this game, you control a robot and must navigate through the environment, destroy four towers, and fend off warriors and archers to emerge victorious.

## Features

- **Basic Mechanics:** Control the robot using AWSD keys, aim with the mouse, and shoot with the left-click.
- **Enemy AI:** Warriors and archers utilize the A* algorithm for pathfinding, chasing the player.
- **Tower Challenges:** Four towers shoot projectiles that the player can destroy. The player must eliminate all towers to win.
- **Dynamic AI Perception:** Enemies use a perception manager to detect the player through sight, hearing, and tact, contributing to their decision-making process.

## Gameplay

- **Towers:**  Destroy all four towers to win.
- **Enemies:** Warriors and archers will chase and attack the player. Watch out for their projectiles!
- **Robot Health:** The player has a health bar. Avoid enemy attacks to survive.

## Code Snippets

### Enemy AI Perception Manager
```csharp
    /// <summary>
    /// Manages the perception of enemies through sight, hearing, and tact.
    /// </summary>
    private void perceptionManager() {
        // Sight
        m_enemiesPercibed.Clear();
        Collider[] percibed = Physics.OverlapSphere(m_agent.getEyePosition(), m_agent.getEyeRadius());
        RaycastHit hit;
        foreach (Collider col in percibed) {
            if (col.CompareTag(enemyTag)) {
                m_enemiesPercibed.Add(col.gameObject);
            }
        }
        // Hearing
        percibed = Physics.OverlapSphere(m_agent.getEarsPosition(), m_agent.getHearingRadius());
        foreach (Collider col in percibed) {
            if (col.CompareTag(enemyTag)) {
                Vector3 directionToEnemy = col.transform.position - m_agent.getEarsPosition();
                if (Physics.Raycast(m_agent.getEarsPosition(), directionToEnemy, out hit, m_agent.getHearingRadius())) {
                    if (hit.collider == col) {
                        m_enemiesPercibed.Add(col.gameObject);
                    }
                }
            }
        }
        // Tact
        percibed = Physics.OverlapSphere(m_agent.getTactPosition(), m_agent.getTactRadius());
        foreach (Collider col in percibed) {
            if (col.CompareTag(enemyTag)) {
                m_enemiesPercibed.Add(col.gameObject);
            }
        }
    }
```

### Enemy AI Decision Manager
```csharp
    /// <summary>
    /// Manages AI decision-making based on perceived enemies.
    /// </summary>
    private void decisonManager() {
        int closestEnemy = 0;
        float minDistance = 1000f;
        if (m_enemiesPercibed.Count > 0) {
            for (int i = 0; i < m_enemiesPercibed.Count; i++) {
                float distanceToEnemy = Vector3.Distance(transform.position, m_enemiesPercibed[i].transform.position);
                if (distanceToEnemy < minDistance) {
                    closestEnemy = i;
                    minDistance = distanceToEnemy;
                }
            }
            m_target = m_enemiesPercibed[closestEnemy].transform;
        }
        if (minDistance > attackRange) {
            m_meleeState = MeleeAgentState.Seeking;
        }
        if (minDistance < attackRange) {
            m_meleeState = MeleeAgentState.Attacking;
        }
        if (m_enemiesPercibed.Count == 0) {
            m_meleeState = MeleeAgentState.Seeking;
            m_target = mainTarget;
        }
        switch (m_meleeState) {
            case MeleeAgentState.None:
                movementManager();
                break;
            case MeleeAgentState.Wandering:
                movementManager();
                break;
            case MeleeAgentState.Seeking:
                movementManager();
                break;
            case MeleeAgentState.Attacking:
                actionManager();
                break;
        }
    }
```

## Customization

In the Unity game editor, users can customize the following agent parameters:

- Attack Range
- Speed
- Target

## Performance

The AI system has been tested on a machine with 150+ FPS while running 10 agents in the scene. Further optimizations may be considered for larger-scale scenarios.

## Testing

The AI agents have been tested within the game environment, with successful gameplay scenarios where all agents must be defeated, and towers destroyed to win.

## Future Improvements

While the project meets the initial requirements, potential improvements could include:

- Enhanced performance optimizations for larger agent counts.
- Additional AI behaviors or strategies.
- Bug fixes and general improvements.

## Contributing

Contributions are welcome. If you have suggestions, improvements, or find issues, please open a pull request or submit an issue.

## Acknowledgments

- This project was developed as part of a school assignment.
- [SebLague](https://github.com/SebLague) for the A* algorithm and pathfinding implementation.
