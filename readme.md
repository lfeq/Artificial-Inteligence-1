# Robots vs Wizards

![gamescreenshot](readmeImages/Game%20Screenshot.png)

## Overview

This project showcases the implementation of artificial intelligence for agents in a Unity game environment. The primary objective is to create autonomous agents that pursue and attack the player when in close proximity. Additionally, a playable level is introduced where the player must destroy enemy towers to win.

## Usage Instructions

1. **Project Download:** Clone or download the project from the repository.

2. **Open in Unity:** Open Unity and load the downloaded project.

3. **Run Scene:** Run the main menu scene and press the play button.


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

## Performance

The AI system has been tested on a machine with 150+ FPS while running 10 agents in the scene. Further optimizations may be considered for larger-scale scenarios.

## Testing

The AI agents have been tested within the game environment, with successful gameplay scenarios where all agents must be defeated, and towers destroyed to win.

## Future Improvements

While the project meets the initial requirements, potential improvements could include:

- Enhanced performance optimizations for larger agent counts.
- Additional AI behaviors or strategies.
- Bug fixes and general improvements.

## Play the Game!
You can play the game on [itch.io](https://oopolo.itch.io/robots-vs-wizards).

## Project Requirements
- Unity (version 2022.3.10f or higher)

## Acknowledgments

- This project was developed as part of a school assignment.
- [SebLague](https://github.com/SebLague) for the A* algorithm and pathfinding implementation.
