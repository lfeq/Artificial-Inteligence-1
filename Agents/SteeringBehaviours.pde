/**
 * A class that contains methods for different steering behaviors for an agent.
 */
class SteeringBehaviours {
  
  /**
   * Moves the agent towards the target position.
   * @param t_agent The agent that is being controlled.
   * @param t_targetPos The target position that the agent is trying to reach.
   * @return A PVector representing the steering force.
   */
  PVector seek(Agent t_agent, PVector t_targetPos) {
    calculateDesiredVelocity(t_agent, t_targetPos, t_agent.currentPosition);
    return calculateSteering(t_agent);
  }
  
  /**
   * Moves the agent away from the target position.
   * @param t_agent The agent that is being controlled.
   * @param t_targetPos The target position that the agent is trying to move away from.
   * @return A PVector representing the steering force.
   */
  PVector flee(Agent t_agent, PVector t_targetPos){
    calculateDesiredVelocity(t_agent, t_agent.currentPosition, t_targetPos);
    return calculateSteering(t_agent);
  }
  
  /**
   * Simulates a wandering behavior for the agent.
   * @param t_agent The agent that is being controlled.
   */
  void wander(Agent t_agent){
    PVector wheel = new PVector(t_agent.currentVelocity.x, t_agent.currentVelocity.y);
    wheel.normalize();
    wheel.mult(t_agent.wanderDisplacement);
    wheel.add(t_agent.currentPosition);
    PVector newDirection = new PVector(random(-1f, 1f), random(-1f, 1f));
    newDirection.normalize();
    newDirection.mult(t_agent.wanderRadius);
    wheel.add(newDirection);
  }
  
  /**
   * Moves the agent to evade another agent.
   * @param t_agent The agent that is being controlled.
   * @param t_agentToEvade The agent to evade.
   * @return A PVector representing the steering force.
   */
  PVector evade(Agent t_agent, Agent t_agentToEvade){
    float distanceToTarget = PVector.dist(t_agentToEvade.currentPosition, t_agent.currentPosition);
    float positionPrediction = distanceToTarget / t_agent.maxSpeed;
    PVector futurePosition = PVector.add(t_agentToEvade.currentPosition, t_agentToEvade.currentVelocity.copy().mult(positionPrediction));
    return flee(t_agent, futurePosition);
  }
  
  /**
   * Causes the agent to follow a leader agent.
   * @param t_leader The leader agent that the agent is following.
   * @param t_agent The agent that is following the leader.
   * @return A PVector representing the steering force.
   */
  PVector followLeader(Agent t_leader, Agent t_agent){
    PVector leaderVelocity = t_leader.currentVelocity.copy();
    PVector force = new PVector();
    PVector behind;
    PVector ahead;
    leaderVelocity.normalize();
    leaderVelocity.mult(t_agent.leaderBehindDistance);
    ahead = t_leader.currentPosition.copy();
    ahead.add(leaderVelocity);
    leaderVelocity.mult(-1);
    behind = t_leader.currentPosition.copy();
    behind.add(leaderVelocity);
    if(PVector.dist(ahead, t_agent.currentPosition) <= t_agent.leaderSightRadius || PVector.dist(t_leader.currentPosition, t_agent.currentPosition) <= t_agent.leaderSightRadius){
      force.add(evade(t_agent, t_leader));
    }
    force.add(seek(t_agent, behind));
    force.add(separation(t_agent));
    return force;
  }
  
  /**
   * Calculates a steering force for separation from nearby agents.
   * @param t_agent The agent that is being controlled.
   * @return A PVector representing the steering force.
   */
  PVector separation(Agent t_agent){
    PVector force = new PVector();
    int neighbourCount = 0;
    for(int i = 0; i < t_agent.agents.size(); i++){
      Agent tempAgent = t_agent.agents.get(i);
      if(tempAgent != t_agent && PVector.dist(tempAgent.currentPosition, t_agent.currentPosition) <= t_agent.separationRadius){
        force.x += tempAgent.currentPosition.x - t_agent.currentPosition.x;
        force.y += tempAgent.currentPosition.y - t_agent.currentPosition.y;
        neighbourCount++;
      }
    }
    if(neighbourCount != 0){
      force.x /= neighbourCount;
      force.y /= neighbourCount;
      force.mult(-1);
    }
    force.normalize();
    force.mult(t_agent.maxSeparation);
    return force;
  }
  
  /**
   * Calculates a steering force for separation from nearby agents.
   * @param t_agent The agent that is being controlled.
   * @return A PVector representing the steering force.
   */
  Agent getNeighborAhead(Agent t_agent){
    Agent neighbourAhead = null;
    PVector currentVelocityCopy = t_agent.currentVelocity.copy();
    PVector ahead = t_agent.currentPosition.copy();
    currentVelocityCopy.normalize().mult(t_agent.maxQueueAhead);
    ahead.add(currentVelocityCopy);
    for(int i = 0; i < t_agent.agents.size(); i++){
      Agent neighbor = t_agent.agents.get(i);
      float distance = PVector.dist(ahead, neighbor.currentPosition);
      if(neighbor != t_agent && distance <= t_agent.maxQueueRadius){
        neighbourAhead = neighbor;
        return neighbourAhead;
      }
    }
    return neighbourAhead;
  }
  
  /**
   * Calculates a steering force for queue behavior.
   * @param t_agent The agent that is following the leader.
   * @return A PVector representing the braking force.
   */
  PVector queue(Agent t_agent){
    Agent neighbor = getNeighborAhead(t_agent);
    PVector velocityCopy = t_agent.currentVelocity.copy();
    PVector brake = new PVector(0, 0);
    if(neighbor != null){
      brake.x = -t_agent.steering.x * 0.8f;
      brake.y = -t_agent.steering.y * 0.8f;
      velocityCopy.mult(-1);
      brake.add(velocityCopy);
      brake.add(separation(t_agent));
      if(PVector.dist(t_agent.currentPosition, neighbor.currentPosition) <= t_agent.maxQueueRadius){
        t_agent.currentVelocity.mult(0.3f);
      }
    }
    return brake;
  }
  
  void leaderQueue(Agent t_agent){
    for(int  i = 0; i < t_agent.agents.size(); i++){
      Agent tempAgent =  t_agent.agents.get(i);
      if(i == 0){
        tempAgent.seek();
        continue;
      }
      tempAgent.targetPosition = t_agent.agents.get(i - 1).currentPosition;
      tempAgent.seek();
    }
  }
  
  /**
   * Calculates the steering force.
   * @param t_agent The agent that is being controlled.
   */
  private PVector calculateSteering(Agent t_agent){
    PVector steering = PVector.sub(t_agent.desiredVelocity, t_agent.currentVelocity); 
    steering.limit(t_agent.maxForce);
    steering.div(t_agent.mass);
    return steering;
  }
  
  /**
   * Calculates the desired velocity based on the target position and the agent's current position.
   * @param t_agent The agent that is being controlled.
   * @param t_targetPos The target position that the agent is trying to reach.
   * @param t_currentPosition The current position of the agent.
   */
  private void calculateDesiredVelocity(Agent t_agent, PVector t_targetPos, PVector t_currentPosition){
    t_agent.desiredVelocity = PVector.sub(t_targetPos, t_currentPosition);
    t_agent.desiredVelocity.normalize().mult(t_agent.maxSpeed);
  }
}
