/**
 * A class that contains methods for different steering behaviors for an agent.
 */
class SteeringBehaviours {
  
  /**
   * Moves the agent towards the target position.
   * @param t_agent The agent that is being controlled.
   * @param t_targetPos The target position that the agent is trying to reach.
   */
  void seek(Agent t_agent, PVector t_targetPos) {
    calculateDesiredVelocity(t_agent, t_targetPos, t_agent.currentPosition);
    calculateSteering(t_agent);
  }
  
  /**
   * Moves the agent away from the target position.
   * @param t_agent The agent that is being controlled.
   * @param t_targetPos The target position that the agent is trying to move away from.
   */
  void flee(Agent t_agent, PVector t_targetPos){
    calculateDesiredVelocity(t_agent, t_agent.currentPosition, t_targetPos);
    calculateSteering(t_agent);
  }
  
  /**
   * Moves the agent towards the target position and slows down when it gets close.
   * @param t_agent The agent that is being controlled.
   * @param t_targetPos The target position that the agent is trying to reach.
   */
  void arrival(Agent t_agent, PVector t_targetPos){
    calculateDesiredVelocity(t_agent, t_targetPos, t_agent.currentPosition);
    float distance = PVector.dist(t_targetPos, t_agent.currentPosition);
    float slowing_factor = distance / t_agent.slowingRadius;
    if(distance < t_agent.slowingRadius){
      t_agent.desiredVelocity.mult(slowing_factor);
    }
    calculateSteering(t_agent);
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