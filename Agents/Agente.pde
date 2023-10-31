Agent agentSeek, agentFlee;
PVector target;

void setup() {
  size(800, 800);
  agentSeek = new Agent(new PVector(0, 0), 10, 20, 10, 180);
  agentFlee = new Agent(new PVector(width/2, height/2), 10, 10, 7, 180);
}

void draw() {
  background(100);
  //circle(mouseX, mouseY, 30);
  agentSeek.targetPosition = agentFlee.currentPosition;
  agentFlee.targetPosition = agentSeek.currentPosition;
  agentSeek.paint();
  agentSeek.seek();
  agentSeek.wander();
  agentFlee.paint();
  agentFlee.flee();
}
