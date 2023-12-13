using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GridAStar class represents a grid for A* pathfinding.
/// </summary>
public class GridAStar : MonoBehaviour {

    #region Public variables

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public List<Node> path;

    #endregion Public variables

    #region Private variables

    private Node[,] m_grid;
    private float m_nodeDiameter;
    private int m_gridSizeX, m_gridSizeY;

    #endregion Private variables

    #region Unity functions

    private void Awake() {
        m_nodeDiameter = nodeRadius * 2;
        m_gridSizeX = Mathf.RoundToInt(gridWorldSize.x / m_nodeDiameter);
        m_gridSizeY = Mathf.RoundToInt(gridWorldSize.y / m_nodeDiameter);
        CreateGrid();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (m_grid != null) {
            foreach (Node n in m_grid) {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (m_nodeDiameter - .1f));
            }
        }
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Gets the neighbors of the specified node.
    /// </summary>
    /// <param name="node">The node for which neighbors are requested.</param>
    /// <returns>The list of neighboring nodes.</returns>
    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < m_gridSizeX && checkY >= 0 && checkY < m_gridSizeY) {
                    neighbours.Add(m_grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    /// <summary>
    /// Converts a world position to the corresponding node on the grid.
    /// </summary>
    /// <param name="worldPosition">The world position to convert.</param>
    /// <returns>The node at the specified world position.</returns>
    public Node NodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((m_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((m_gridSizeY - 1) * percentY);
        return m_grid[x, y];
    }

    #endregion Public functions

    #region Private functions

    /// <summary>
    /// Creates the grid by initializing nodes based on the provided parameters.
    /// </summary>
    private void CreateGrid() {
        m_grid = new Node[m_gridSizeX, m_gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < m_gridSizeX; x++) {
            for (int y = 0; y < m_gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * m_nodeDiameter + nodeRadius) + Vector3.forward * (y * m_nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                m_grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    #endregion Private functions
}