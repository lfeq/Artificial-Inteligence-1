/**
 * A class that contains methods for different steering behaviors for an agent.
 */
class SteeringBehaviours {
  
  /**
   * Moves the agent towards the target position.
   * @param t_agent The agent that is being controlled.
   * @param t_targetPos The target position that the agent is trying to reach.
   */
  PVector seek(Agent t_agent, PVector t_targetPos) {
    calculateDesiredVelocity(t_agent, t_targetPos, t_agent.currentPosition);
    //calculateSteering(t_agent);
    PVector steering = PVector.sub(t_agent.desiredVelocity, t_agent.currentVelocity); 
    steering.limit(t_agent.maxForce);
    steering.div(t_agent.mass);
    return steering;
  }
  
  /**
   * Moves the agent away from the target position.
   * @param t_agent The agent that is being controlled.
   * @param t_targetPos The target position that the agent is trying to move away from.
   */
  PVector flee(Agent t_agent, PVector t_targetPos){
    calculateDesiredVelocity(t_agent, t_agent.currentPosition, t_targetPos);
    //calculateSteering(t_agent);
    PVector steering = PVector.sub(t_agent.desiredVelocity, t_agent.currentVelocity); 
    steering.limit(t_agent.maxForce);
    steering.div(t_agent.mass);
    return steering;
  }
  
  /**
   * Moves the agent towards the target position and slows down when it gets close.
   * @param t_agent The agent that is being controlled.
   * @param t_targetPos The target position that the agent is trying to reach.
   */
  PVector arrival(Agent t_agent, PVector t_targetPos){
    t_agent.desiredVelocity = PVector.sub(t_targetPos, t_agent.currentPosition);
    /*
    float distance = PVector.dist(t_targetPos, t_agent.currentPosition);
    t_agent.desiredVelocity.normalize().mult(t_agent.maxSpeed);
    if(distance < t_agent.slowingRadius){
      t_agent.desiredVelocity.mult(distance / t_agent.slowingRadius);
    }
    */
    t_agent.desiredVelocity.normalize().mult(t_agent.maxSpeed);
    PVector steering = PVector.sub(t_agent.desiredVelocity, t_agent.currentVelocity);
    steering.limit(t_agent.maxForce);
    steering.div(t_agent.mass);
    return steering;
    /*
    t_agent.currentVelocity.add(t_agent.steering);
    t_agent.currentVelocity.limit(t_agent.maxSpeed);
    float distance = PVector.dist(t_targetPos, t_agent.currentPosition);
    if(distance < t_agent.slowingRadius){
      t_agent.currentVelocity.mult(distance / t_agent.slowingRadius);
    }
    */
  }
  
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
  
  PVector evade(Agent t_agent, Agent t_agentToEvade){
    float distanceToTarget = PVector.dist(t_agentToEvade.currentPosition, t_agent.currentPosition);
    float positionPrediction = distanceToTarget / t_agent.maxSpeed;
    PVector futurePosition = PVector.add(t_agentToEvade.currentPosition, t_agentToEvade.currentVelocity.copy().mult(positionPrediction));
    return flee(t_agent, futurePosition);
  }
  
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
    force.add(arrival(t_agent, behind));
    force.add(separation(t_agent));
    return force;
  }
  
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
   * Calculates the steering force.
   * @param t_agent The agent that is being controlled.
   */
  private void calculateSteering(Agent t_agent){
    t_agent.steering = PVector.sub(t_agent.desiredVelocity, t_agent.currentVelocity); 
    t_agent.steering.limit(t_agent.maxForce);
    t_agent.steering.div(t_agent.mass);
    t_agent.currentVelocity.add(t_agent.steering);
    t_agent.currentVelocity.limit(t_agent.maxSpeed);
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
