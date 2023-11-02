Agent agentSeek, agentFlee;
PVector target;
Path path;

void setup() {
  size(800, 800);
  pathCreation();
  agentSeek = new Agent(new PVector(0, 0), 10, 20, 3, 180, path); //<>//
  agentFlee = new Agent(new PVector(width/2, height/2), 10, 10, 2, 180, path);
}

void draw() {
  background(100);
  //circle(mouseX, mouseY, 30);
  //pursuitAndEvade();
  pathFollowing();
}

void pursuitAndEvade(){
  agentSeek.targetPosition = agentFlee.currentPosition;
  agentFlee.targetPosition = agentSeek.currentPosition;
  agentSeek.paint();
  agentSeek.pursuit(agentFlee);
  agentSeek.seek();
  agentFlee.paint();
  agentFlee.flee();
  agentFlee.evade(agentSeek);
}

void pathFollowing(){
  path.paintPoints();
  agentSeek.paint();
  agentSeek.pathFollowing();
}

void pathCreation(){
  path = new Path();
  path.addNode(new PVector(0, 0));
  path.addNode(new PVector(100, 0));
  path.addNode(new PVector(0, 200));
  path.addNode(new PVector(100, 500));
  path.addNode(new PVector(200, 400));
  path.addNode(new PVector(400, 600));
}
