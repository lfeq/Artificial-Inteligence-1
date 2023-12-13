using UnityEngine;

/// <summary>
/// Node class represents a single node in a grid used for pathfinding.
/// </summary>
public class Node {

    #region Public variables

    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public Node parent;

    #endregion Public variables

    #region Public functions

    /// <summary>
    /// Initializes a new instance of the Node class.
    /// </summary>
    /// <param name="_walkable">Indicates whether the node is walkable or not.</param>
    /// <param name="_worldPos">The world position of the node.</param>
    /// <param name="_gridX">The x-coordinate of the node in the grid.</param>
    /// <param name="_gridY">The y-coordinate of the node in the grid.</param>
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY) {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    /// <summary>
    /// The total cost of the node, calculated as the sum of gCost and hCost.
    /// </summary>
    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    #endregion Public functions
}