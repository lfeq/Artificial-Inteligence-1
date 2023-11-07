/**
 * A class that handles an agent behaviour.
 */
class Agent {
  PVector currentPosition, targetPosition, currentVelocity, desiredVelocity, steering;
  float maxForce, mass, maxSpeed, slowingRadius, wanderDisplacement, wanderRadius, 
  pathRadius, pathDirection, leaderBehindDistance, separationRadius, maxSeparation,
  leaderSightRadius, maxQueueAhead, maxQueueRadius;
  SteeringBehaviours steerings;
  Path path;
  int currentNode;
  ArrayList<Agent> agents;
    
  /**
   * Constructor for the Agent class.
   * @param t_targetPos The target position for the agent.
   * @param t_maxForce The maximum force that can be applied to the agent.
   * @param t_mass The mass of the agent.
   * @param t_maxSpeed The maximum speed of the agent.
   * @param t_slowing_radius The radius within which the agent starts to slow down.
   * @param t_path The path for the agent to follow.
   * @param t_agents The list of other agents for separation and leader following.
   */
  Agent(PVector t_targetPos, float t_maxForce, float t_mass, float t_maxSpeed, float t_slowing_radius, Path t_path,
  ArrayList<Agent> t_agents) {
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
    path = t_path;
    currentNode = 0;
    pathRadius = 50;
    pathDirection = 1;
    leaderBehindDistance = 30f;
    agents = t_agents;
    separationRadius = 25;
    maxSeparation = 50;
    leaderSightRadius = 240;
    maxQueueAhead = 50;
    maxQueueRadius = 30;
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
    currentVelocity.add(steerings.seek(this, targetPosition));
    currentVelocity.limit(maxSpeed);
    updateVelocity();
  }
  
  /**
   * Makes the agent move away from the target position.
   */
  void flee(){
    currentVelocity.add(steerings.flee(this, targetPosition));
    currentVelocity.limit(maxSpeed);
    updateVelocity();
  }
  
  /**
   * Makes the agent arrive at the target position.
   */
  void arrival(){
    currentVelocity.add(steerings.seek(this, targetPosition));
    currentVelocity.limit(maxSpeed);
    float distance = PVector.dist(targetPosition, currentPosition);
    if(distance < slowingRadius){
      currentVelocity.mult(distance / slowingRadius);
    }
    updateVelocity();
  }
  
  /**
   * Simulates wandering behavior for the agent.
   */
  void wander(){
    steerings.wander(this);
    updateVelocity();
  }

  /**
   * Makes the agent pursue another agent.
   * @param t_agent The agent to pursue.
   */
  void pursuit(Agent t_agent){
    float distanceToTarget = PVector.dist(t_agent.currentPosition, currentPosition);
    float positionPrediction = distanceToTarget / maxSpeed;
    PVector futurePosition = PVector.add(t_agent.currentPosition, t_agent.currentVelocity.copy().mult(positionPrediction));
    //fill(39, 43, 203);
    //circle(futurePosition.x, futurePosition.y, 32);   
    currentVelocity.add(steerings.seek(this, futurePosition));
    currentVelocity.limit(maxSpeed);
    updateVelocity();
  }
  
  /**
   * Makes the agent evade another agent.
   * @param t_agent The agent to evade.
   */
  void evade(Agent t_agent){
    float distanceToTarget = PVector.dist(t_agent.currentPosition, currentPosition);
    float positionPrediction = distanceToTarget / maxSpeed;
    PVector futurePosition = PVector.add(t_agent.currentPosition, t_agent.currentVelocity.copy().mult(positionPrediction)); 
    //fill(211, 38, 189);
    //circle(futurePosition.x, futurePosition.y, 32);
    currentVelocity.add(steerings.flee(this, futurePosition));
    currentVelocity.limit(maxSpeed);
    updateVelocity();
  }

  /**
   * Makes the agent follow a path.
   */
  void pathFollowing(){
    PVector target;
    ArrayList<PVector> nodes = path.getNodes();
    target = nodes.get(currentNode);
    if (PVector.dist(target, currentPosition) < pathRadius){
      currentNode += pathDirection;;
      if(currentNode >= nodes.size() || currentNode < 0){
        pathDirection *= -1;
        currentNode += pathDirection;
      }
    }
    currentVelocity.add(steerings.seek(this, target));
    currentVelocity.limit(maxSpeed);
    updateVelocity();
  }

  /**
   * Makes the agent follow a leader agent.
   * @param t_leader The leader agent to follow.
   */
  void followLeader(Agent t_leader){
    steering = new PVector();
    steering.add(steerings.followLeader(t_leader, this));
    steering.div(mass);
    currentVelocity.add(steering);
    currentVelocity.limit(maxSpeed);
    updateVelocity();
  }
  
  /**
   * Makes the agent simulate queue behavior.
   */
  void queue(){
    steering = steerings.seek(this, targetPosition);
    steering.add(steerings.queue(this));
    steering.div(mass);
    currentVelocity.add(steering);
    currentVelocity.limit(maxSpeed);
    updateVelocity();
  }
  
  void queue2(){
    steerings.leaderQueue(this);
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
    } if(currentPosition.y > height){
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
