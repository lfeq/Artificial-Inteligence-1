Agent agentSeek, agentFlee, agentFollowLeader1, agentFollowLeader2, agentFollowLeader3;
PVector target, doorPosition;
Path path;
ArrayList<Agent> agents;

void setup() {
  size(800, 800);
  pathCreation();
  agents = new ArrayList<Agent>();
  agentSeek = new Agent(new PVector(0, 0), 10, 20, 3, 30, path, agents); 
  agentFlee = new Agent(new PVector(width/2, height/2), 10, 10, 3, 180, path, agents);
  agentFollowLeader1 = new Agent(new PVector(random(0, 800), random(0, 800)), 10, 10, 3, 30, path, agents);
  agentFollowLeader2 = new Agent(new PVector(random(0, 800), random(0, 800)), 10, 10, 3, 30, path, agents);
  agentFollowLeader3 = new Agent(new PVector(random(0, 800), random(0, 800)), 10, 10, 3, 30, path, agents);
  agents.add(agentFlee);
  agents.add(agentFollowLeader1);
  agents.add(agentFollowLeader2);
  agents.add(agentFollowLeader3);
  doorPosition = new PVector(400, 700);
}

void draw() {
  background(100);
  //pursuitAndEvade();
  //pathFollowing();
  followLeader();
  /*
  fill(129, 68, 3);
  circle(doorPosition.x, doorPosition.y, 20);
  agentFlee.targetPosition = doorPosition;
  agentFollowLeader1.targetPosition = doorPosition;
  agentFollowLeader2.targetPosition = doorPosition;
  agentFollowLeader3.targetPosition = doorPosition;
  agentFlee.paint();
  agentFlee.seek();
  agentFlee.queue();
  agentFollowLeader1.paint();
  agentFollowLeader1.seek();
  agentFollowLeader1.queue();
  agentFollowLeader2.paint();
  agentFollowLeader2.seek();
  agentFollowLeader2.queue();
  agentFollowLeader3.paint();
  agentFollowLeader3.seek();
  agentFollowLeader3.queue();
  */
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
