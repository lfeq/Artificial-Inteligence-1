Agent a;
PVector target;

void setup() {
  size(800, 800);
  a = new Agent(new PVector(width/2, height/2), 10, 20, 12, 100);
}

void draw() {
  background(100);
  
  circle(mouseX, mouseY, 30);
  a.target_pos = new PVector(mouseX, mouseY);
  
  a.paint();
  a.arrival();
}
