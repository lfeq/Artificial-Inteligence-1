PVector v2;float maxVel; //<>//
Agent a1;

void setup(){
  size(800,800);
  a1 = new Agent(5, 50); //<>//
  v2 = new PVector(500, 500);
}

void draw(){
  background(30);
  v2 = new PVector(mouseX, mouseY);
  a1.seek(v2);
  a1.paint();
  circle(v2.x, v2.y, 30);
}
