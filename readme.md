## Overview

This project implements an artificial intelligence system for a simple top-down shooter game in Unity using C#. The AI-controlled agents exhibit behavior based on sensory perception, decision-making, and pathfinding to chase and attack the player.

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Setup](#setup)
4. [Usage](#usage)
5. [Customization](#customization)
6. [Performance](#performance)
7. [Testing](#testing)
8. [Future Improvements](#future-improvements)
9. [Acknowledgments](#acknowledgments)

## Introduction

The AI system is designed to enhance the gameplay experience in a top-down shooter. Agents use vision, hearing, and tact to perceive the environment and make decisions accordingly. The project aims to showcase AI behaviors in a simple game scenario, where enemies pursue the player and engage in melee attacks when close enough.

## Features

- **Sensory Perception:** Agents use vision, hearing, and tact to detect enemies.
- **Decision-Making:** The AI employs a decision manager to determine actions based on perceived enemies.
- **Pathfinding:** A* algorithm is implemented for seeking behavior, allowing agents to navigate the game environment.
- **Customizable Parameters:** Users can adjust attack range, speed, and other parameters for each agent in the game editor.

## Setup

1. Clone the repository.
2. Open the Unity project in the Unity editor.
3. Customize agent parameters in the game editor if needed.
4. Run the game to observe AI behaviors.

## Usage

- Agents automatically chase and attack the player when within the attack range.
- The game scene includes towers that need to be destroyed to win.

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

## Acknowledgments

This project was developed as part of a school assignment.

