class Agent {
  PVector current_pos, target_pos, current_vel, desired_vel, steering;
  float max_force, mass, max_speed, slowing_radius;
  SteeringBehaviours steerings;
    
  Agent(PVector pos, float max_f, float mss, float max_s, float t_slowing_radius) {
    current_pos = pos;
    max_force = max_f;
    mass = mss;
    max_speed = max_s;
    slowing_radius = t_slowing_radius;
    target_pos = new PVector(0, 0);
    current_vel = new PVector(0, 0);
    desired_vel = new PVector(0, 0);
    steering = new PVector(0, 0);
    steerings = new SteeringBehaviours();
  }
  
  
  void paint() {
    fill(200, 34, 45);
    circle(current_pos.x, current_pos.y, mass);
  }
  
  void seek() { /*
    desired_vel = PVector.sub(target_pos, current_pos);
    desired_vel.normalize().mult(max_speed);
    steering = PVector.sub(desired_vel, current_vel); 
    steering.limit(max_force);
    steering.div(mass);
    current_vel.add(steering);
    current_vel.limit(max_speed);*/
    steerings.seek(this, target_pos);
    current_pos.add(current_vel);
  }
  
  void flee(){
    steerings.seek(this, target_pos);
    current_pos.sub(current_vel);
  }
  
  void arrival(){
    desired_vel = PVector.sub(target_pos, current_pos);
    float distance = PVector.dist(target_pos, current_pos);
    float slowing_factor = distance / slowing_radius;
    if(distance < slowing_radius){
      desired_vel.normalize().mult(max_speed);
      desired_vel.mult(slowing_factor);
    } else {
      desired_vel.normalize().mult(max_speed);
    }
    steering = PVector.sub(desired_vel, current_vel); 
    //steering.limit(max_force);
    //steering.div(mass);
    current_vel.add(steering);
    current_vel.limit(max_speed);
    current_pos.add(current_vel);
  }
}
