class SteeringBehaviours{
  public PVector seek(Agent t_agent, PVector t_target){
    PVector desiredVel = PVector.sub(t_target, t_agent.position);
    desiredVel.normalize();
    desiredVel.mult(t_agent.maxVel);
    return desiredVel;
  }
}
