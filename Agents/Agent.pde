class Agent{
  SteeringBehaviours sb;
  PVector position, velocity;
  float maxVel, mass;
  
  public Agent(float t_maxVel, float t_mass){
    position = new PVector(0, 0);
    velocity = new PVector(0, 0);
    maxVel = t_maxVel;
    mass = t_mass;
    sb = new SteeringBehaviours();
  }
  
  public void seek(PVector t_target){
    position.add(sb.seek(this, t_target));
  }
  
  public void paint(){
    circle(position.x, position.y, mass);
  }
}
