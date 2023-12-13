using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PathFinding class provides pathfinding functionality using the A* algorithm on a grid.
/// </summary>
[RequireComponent(typeof(GridAStar))]
public class PathFinding : MonoBehaviour {

    #region Public variables

    public static PathFinding instance;

    #endregion Public variables

    #region Private variables

    private GridAStar m_grid;

    #endregion Private variables

    #region Unity functions

    private void Awake() {
        m_grid = GetComponent<GridAStar>();
        if (FindObjectOfType<PathFinding>() != null &&
            FindObjectOfType<PathFinding>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Finds a path from the start position to the target position.
    /// </summary>
    /// <param name="startPos">The starting position in world coordinates.</param>
    /// <param name="targetPos">The target position in world coordinates.</param>
    /// <returns>A list of Vector3 representing the path from start to target.</returns>
    public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos) {
        Node startNode = m_grid.NodeFromWorldPoint(startPos);
        Node targetNode = m_grid.NodeFromWorldPoint(targetPos);
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while (openSet.Count > 0) {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }
            openSet.Remove(node);
            closedSet.Add(node);
            if (node == targetNode) {
                return RetracePath(startNode, targetNode);
            }
            foreach (Node neighbour in m_grid.GetNeighbours(node)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }
                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return new List<Vector3>();
    }

    #endregion Public functions

    #region Private functions

    /// <summary>
    /// Retraces the path from the start node to the end node.
    /// </summary>
    /// <param name="startNode">The starting node.</param>
    /// <param name="endNode">The target node.</param>
    /// <returns>A list of Vector3 representing the retrace path.</returns>
    private List<Vector3> RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        List<Vector3> pathVector3 = new List<Vector3>();
        Node currentNode = endNode;
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        m_grid.path = path;
        foreach (Node tmp in path) {
            pathVector3.Add(tmp.worldPosition);
        }
        return pathVector3;
    }

    /// <summary>
    /// Calculates the distance between two nodes.
    /// </summary>
    /// <param name="nodeA">The first node.</param>
    /// <param name="nodeB">The second node.</param>
    /// <returns>The distance between the nodes.</returns>
    private int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (dstX > dstY) {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }

    #endregion Private functions
}