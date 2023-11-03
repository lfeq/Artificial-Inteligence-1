Agent agentSeek, agentFlee, agentFollowLeader1, agentFollowLeader2, agentFollowLeader3;
PVector target;
Path path;
ArrayList<Agent> agents;

void setup() {
  size(800, 800);
  pathCreation();
  agents = new ArrayList<Agent>(); //<>//
  agentSeek = new Agent(new PVector(0, 0), 10, 20, 3, 180, path, agents); 
  agentFlee = new Agent(new PVector(width/2, height/2), 10, 10, 3, 180, path, agents);
  agentFollowLeader1 = new Agent(new PVector(random(0, 800), random(0, 800)), 10, 10, 3, 180, path, agents);
  agentFollowLeader2 = new Agent(new PVector(random(0, 800), random(0, 800)), 10, 10, 3, 180, path, agents);
  agentFollowLeader3 = new Agent(new PVector(random(0, 800), random(0, 800)), 10, 10, 3, 180, path, agents);
  agents.add(agentFlee);
  agents.add(agentFollowLeader1);
  agents.add(agentFollowLeader2);
  agents.add(agentFollowLeader3);
}

void draw() {
  background(100);
  //circle(mouseX, mouseY, 30);
  //pursuitAndEvade();
  //pathFollowing();
  followLeader();
}

void pursuitAndEvade(){
  agentSeek.paint();
  agentSeek.pursuit(agentFlee);
  agentFlee.paint();
  agentFlee.evade(agentSeek);
}

void pathFollowing(){
  path.paintPoints();
  agentSeek.paint();
  agentSeek.pathFollowing();
}

void followLeader(){
  agentSeek.targetPosition = new PVector(mouseX, mouseY);
  agentSeek.paint();
  agentSeek.seek();
  agentFlee.paint();
  agentFlee.followLeader(agentSeek);
  agentFollowLeader1.paint();
  agentFollowLeader1.followLeader(agentSeek);
  agentFollowLeader2.paint();
  agentFollowLeader2.followLeader(agentSeek);
  agentFollowLeader3.paint();
  agentFollowLeader3.followLeader(agentSeek);
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
