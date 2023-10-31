/**
 * A class that handles an agent behaviour.
 */
class Agent {
  PVector currentPosition, targetPosition, currentVelocity, desiredVelocity, steering;
  float maxForce, mass, maxSpeed, slowingRadius, wanderDisplacement, wanderRadius;
  SteeringBehaviours steerings;
    
  /**
   * Constructor for the Agent class.
   * @param t_targetPos The target position for the agent.
   * @param t_maxForce The maximum force that can be applied to the agent.
   * @param t_mass The mass of the agent.
   * @param t_maxSpeed The maximum speed of the agent.
   * @param t_slowing_radius The radius within which the agent starts to slow down.
   */
  Agent(PVector t_targetPos, float t_maxForce, float t_mass, float t_maxSpeed, float t_slowing_radius) {
    currentPosition = t_targetPos;
    maxForce = t_maxForce;
    mass = t_mass;
    maxSpeed = t_maxSpeed;
    slowingRadius = t_slowing_radius;
    targetPosition = new PVector(0, 0);
    currentVelocity = new PVector(0, 0);
    desiredVelocity = new PVector(0, 0);
    wanderDisplacement = 100;
    wanderRadius = 100;
    steering = new PVector(0, 0);
    steerings = new SteeringBehaviours();
  }
  
  /**
   * Paints the agent on the screen.
   */
  void paint() {
    fill(200, 34, 45);
    circle(currentPosition.x, currentPosition.y, mass);
    resetPosition();
  }

  /**
   * Makes the agent move towards the target position.
   */
  void seek() { 
    steerings.seek(this, targetPosition);
    updateVelocity();
  }
  
  /**
   * Makes the agent move away from the target position.
   */
  void flee(){
    steerings.flee(this, targetPosition);
    updateVelocity();
  }
  
  /**
   * Makes the agent arrive at the target position.
   */
  void arrival(){
    steerings.arrival(this, targetPosition);
    updateVelocity();
  }
  
  void wander(){
    steerings.wander(this);
    updateVelocity();
  }

  /**
   * Adds current velocity to current position.
   */
  private void updateVelocity(){
    currentPosition.add(currentVelocity);
  }

  /**
   * Resets agent position if the agent goes off screen.
   */
   //Se queda moviendose de un lado al otro en vez de seguir su camino ninja.
   /*
  private void resetPosition(){
    if(currentPosition.x > width){
      currentPosition.x = 1;
    } else if(currentPosition.x < 0){
      currentPosition.x = width;
    } else if(currentPosition.y > height){
      currentPosition.y = 1;
    } else if(currentPosition.y < 0){
      currentPosition.y = height;
    }
  }
  */
  private void resetPosition(){
    if(currentPosition.x > width){
      currentPosition.x = random(0, width);
      currentPosition.y = random(0, height);
    } else if(currentPosition.x < 0){
      currentPosition.x = random(0, width);
      currentPosition.y = random(0, height);
    } else if(currentPosition.y > height){
      currentPosition.x = random(0, width);
      currentPosition.y = random(0, height);
    } else if(currentPosition.y < 0){
      currentPosition.x = random(0, width);
      currentPosition.y = random(0, height);
    }
  }
}
