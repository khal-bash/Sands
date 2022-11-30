using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTO.Setup;
using DTO.Storage;

/// <summary>
/// Class governing the behavior of Floors.
/// </summary>
public class Floor : MonoBehaviour
{

    //Properties set in the inspector
    #region Inspector Properties

    /// <summary>
    /// The background color of the floor.
    /// </summary>
    public Color mainColor = Color.white;

    /// <summary>
    /// The size in units of the floor.
    /// </summary>
    public int size = 20;

    /// <summary>
    /// The x-coordinate of this floor in the <see cref="DTO.Setup.LevelInitializationMatrix"/>
    /// </summary>
    public int matrix_X;

    /// <summary>
    /// The x-coordinate of this floor in the <see cref="DTO.Setup.LevelInitializationMatrix"/>
    /// </summary>
    public int matrix_Y;

    /// <summary>
    /// The wall prefab that this object will instantiate on its borders
    /// </summary>
    public GameObject wall;

    /// <summary>
    /// The gate prefab that this object will instantiate on its border if necessary.
    /// </summary>
    public GameObject gate;

    #endregion

    //Properties set in code
    #region Code Properties

    public Neighbors neighbors { get; set; } = new Neighbors();

    #endregion

    //Initialization
    #region Initialization

    private void Start()
    {
        var sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = mainColor;

        transform.localScale = new Vector3(size, size, 1);
        neighbors = new Neighbors();
    }

    #endregion

    //Adding walls
    #region Walls

    /// <summary>
    /// Adds walls to the floor based on the location of the neighbor floors;.
    /// </summary>
    public void AddWalls()
    {

        foreach (Vector2 direction in StoredMisc.UDLR)
        {
            if (neighbors.neighbors[direction]){
                CreateEdge(WallType.half, direction);
            }
            else
            {
                CreateEdge(WallType.full, direction);
            }
        }

    }

    /// <summary>
    /// Creates the edge of a floor.
    /// </summary>
    /// <param name="type">Whether the wall is a full wall or half-wall.</param>
    /// <param name="direction">Which edge of the floor is being created.</param>
    private void CreateEdge(WallType type, Vector2 direction)
    { 
        if (type == WallType.half){CreateGatedWall(direction);}
        else{CreateFullWall(direction);}

    }

    /// <summary>
    /// Creates a wall with a gate.
    /// </summary>
    /// <param name="direction">Which edge the wall occupies.</param>
    private void CreateGatedWall(Vector2 direction)
    {

        Vector3 eulerAngles = Utilities.Vector.GetEulerAnglesPerpendicularToVector2(direction);

        GameObject rightWall = Instantiate(wall, gameObject.transform);
        rightWall.GetComponent<Wall>().type = WallType.half;
        rightWall.transform.localPosition = (direction * 0.5f) + (new Vector2(direction.y, direction.x) * 0.3f);
        rightWall.transform.localScale = new Vector3(0.4125f, 0.0125f, 1);
        rightWall.transform.localEulerAngles = eulerAngles;

        GameObject leftWall = Instantiate(wall, gameObject.transform);
        leftWall.GetComponent<Wall>().type = WallType.half;
        leftWall.transform.localPosition = (direction * 0.5f) - (new Vector2(direction.y, direction.x) * 0.3f);
        leftWall.transform.localScale = new Vector3(0.4125f, 0.0125f, 1);
        leftWall.transform.localEulerAngles = eulerAngles;

        GameObject middleGate = Instantiate(gate, gameObject.transform);
        middleGate.transform.localPosition = direction * 0.5f;
        middleGate.transform.localScale = new Vector3(0.2f, 0.0125f, 1);
        middleGate.transform.localEulerAngles = eulerAngles;
    }

    /// <summary>
    /// Creates a full wall.
    /// </summary>
    /// <param name="direction">Which edge the wall occupies.</param>
    private void CreateFullWall(Vector2 direction)
    {
        Vector3 eulerAngles = Utilities.Vector.GetEulerAnglesPerpendicularToVector2(direction);

        GameObject fullWall = Instantiate(wall, gameObject.transform);
        fullWall.GetComponent<Wall>().type = WallType.full;
        fullWall.transform.localPosition = direction * 0.5f;
        fullWall.transform.localScale = new Vector3(1, 0.0125f, 1);
        fullWall.transform.localEulerAngles = eulerAngles;

    }

    #endregion

}
