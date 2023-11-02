class Path{
  private ArrayList<PVector> nodes;
  
  Path(){
    nodes = new ArrayList<PVector>();
  }
  
  void addNode(PVector t_node){
    nodes.add(t_node);
  }
  
  ArrayList<PVector> getNodes(){
    return nodes;
  }
  
  void paintPoints(){
    for(int i = 0; i < nodes.size(); i++){
      fill(153, 165, 2);
      circle(nodes.get(i).x, nodes.get(i).y, 5);
    }
  }
}
