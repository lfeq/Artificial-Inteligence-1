Agent agentSeek, agentFlee;
PVector target;

void setup() {
  size(800, 800);
  agentSeek = new Agent(new PVector(0, 0), 10, 20, 3, 180);
  agentFlee = new Agent(new PVector(width/2, height/2), 10, 10, 2, 180);
}

void draw() {
  background(100);
  //circle(mouseX, mouseY, 30);
  agentSeek.targetPosition = agentFlee.currentPosition;
  agentFlee.targetPosition = agentSeek.currentPosition;
  agentSeek.paint();
  agentSeek.pursuit(agentFlee);
  agentSeek.seek();
  agentFlee.paint();
  agentFlee.flee();
  agentFlee.evade(agentSeek);
}
