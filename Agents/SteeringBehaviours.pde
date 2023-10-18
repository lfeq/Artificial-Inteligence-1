class SteeringBehaviours {
  
  void seek(Agent a, PVector target_pos) {
    a.desired_vel = PVector.sub(target_pos, a.current_pos);
    a.desired_vel.normalize().mult(a.max_speed);
    a.steering = PVector.sub(a.desired_vel, a.current_vel); 
    a.steering.limit(a.max_force);
    a.steering.div(a.mass);
    a.current_vel.add(a.steering);
    a.current_vel.limit(a.max_speed);
    //current_pos.add(current_vel);
  }
}
