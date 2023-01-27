using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DTO.Setup;
using DTO.Storage;
using DTO.Visuals;

/// <summary>
/// Class governing the behavior of Floors.
/// </summary>
public class Floor : LevelComponent
{

    //Properties set in the inspector
    #region Inspector Properties

    /// <summary>
    /// The size in units of the floor.
    /// </summary>
    public static int size = 20;

    /// <summary>
    /// The wall prefab that this object will instantiate on its borders
    /// </summary>
    public GameObject wall;

    /// <summary>
    /// The gate prefab that this object will instantiate on its border if necessary.
    /// </summary>
    public GameObject gate;

    /// <summary>
    /// The movement sensor prefab that this object will instantiate on its border if necessary.
    /// </summary>
    public GameObject movementSensor;

    #endregion

    //Properties set in code
    #region Code Properties

    /// <summary>
    /// The theme this floor accords to.
    /// </summary>
    public Theme Theme { get => visuals.Theme; }

    /// <summary>
    /// The background color of the floor.
    /// </summary>
    public Color MainColor { get; set; }

    /// <summary>
    /// The neighbors of this floor.
    /// </summary>
    public Neighbors Neighbors { get; set; } = new Neighbors();

    /// <summary>
    /// A list of all gates attached to this floor.
    /// </summary>
    public List<Gate> AttachedGates { get => GetAllAttachedGates(); }

    /// <summary>
    /// Gets all the gates attached to this floor.
    /// </summary>
    private List<Gate> GetAllAttachedGates()
    {
        var output = new List<Gate>();
        foreach (Vector2Int direction in StoredConstants.UDLR)
        {
            foreach (var gate in StoredComponents.LevelMetaData.Registry.Gates)
            {
                    var gateComponent = gate.GetComponent<Gate>();
                    if (gateComponent.MatrixWorldPosition == MatrixWorldPosition + (0.5f * (Vector2)direction))
                    {
                        output.Add(gateComponent);
                    }
            }
        }
        return output;
    }

    /// <summary>
    /// A UUID for identification purposes.
    /// </summary>
    public Guid UUID = Guid.NewGuid();

    /// <summary>
    /// The visual components of the floor.
    /// </summary>
    public FloorVisuals visuals = new FloorVisuals();

    #endregion

    //Initialization
    #region Initialization

    /// <summary>
    /// Populates some of the code properties. Essentially this is a constructor,
    /// but it can't be a constructor because Floor is a MonoBehaviour.
    /// </summary>
    /// <param name="theme">The desired theme.</param>
    /// <param name="matrixWorldCoordinates">The world coordinates of the floor in the <see cref="LevelInitializationMatrix"/>.</param>
    public void PopulateProperties(Theme theme, Vector2Int matrixWorldCoordinates)
    {
        //This ensures that whenever the theme changes, we update the visuals.
        visuals.ThemeSet += OnThemeChanged;

        visuals.Theme = theme;
        MatrixWorldPosition = matrixWorldCoordinates;
        Neighbors = new Neighbors();
    }

    #endregion

    //Built-in Unity Functions
    #region Unity Functions

    public void Start()
    {
        transform.localScale = new Vector3(size, size, 1);
        Neighbors = new Neighbors();
    }


    #endregion

    //Event Handlers
    #region Event Handlers

    private void OnThemeChanged()
    {
        MainColor = visuals.BackgroundColor;
        var sr = gameObject.GetComponent<SpriteRenderer>();
        
    }

    #endregion

    //Adding walls
    #region Walls

    /// <summary>
    /// Adds walls to the floor based on the location of the neighbor floors;.
    /// </summary>
    public void AddWallsAndGates()
    {
        foreach (Vector2 direction in StoredConstants.UDLR)
        {
            if (Neighbors.NeighborsReference[direction]){
                CreateGatedWall(direction);
            }
            else
            {
                CreateFullWall(direction);
            }
        }
    }

    /// <summary>
    /// Creates a wall with a gate.
    /// </summary>
    /// <param name="direction">Which edge the wall occupies.</param>
    private void CreateGatedWall(Vector2 direction)
    {

        Vector3 eulerAngles = Utilities.Math.Vector.GetEulerAnglesPerpendicularToVector2(direction);

        //Instantiating Level Components
        GameObject rightWall = Instantiate(wall, gameObject.transform);
        GameObject leftWall = Instantiate(wall, gameObject.transform);
        GameObject middleGate = Instantiate(gate, gameObject.transform);
        GameObject sensor = Instantiate(movementSensor, gameObject.transform);

        //Setting Wall Types
        rightWall.GetComponent<Wall>().Type = WallType.righthalf;
        leftWall.GetComponent<Wall>().Type = WallType.lefthalf;

        //Setting Local Positions
        rightWall.transform.localPosition = (direction * 0.5f) + (new Vector2(direction.y, direction.x) * 0.3f);
        leftWall.transform.localPosition = (direction * 0.5f) - (new Vector2(direction.y, direction.x) * 0.3f);
        middleGate.transform.localPosition = direction * 0.5f;
        sensor.transform.localPosition = direction * 0.5f;

        //Setting Local Scales
        rightWall.transform.localScale = new Vector3(0.4125f, 0.0125f, 1);
        leftWall.transform.localScale = new Vector3(0.4125f, 0.0125f, 1);
        middleGate.transform.localScale = new Vector3(0.2f, 0.0125f, 1);

        //Setting Local Rotations
        rightWall.transform.localEulerAngles = eulerAngles;
        leftWall.transform.localEulerAngles = eulerAngles;
        middleGate.transform.localEulerAngles = eulerAngles;
        sensor.transform.localEulerAngles = eulerAngles;

        //Setting the Matrix World Position
        rightWall.GetComponent<Wall>().MatrixWorldPosition = MatrixWorldPosition + (0.5f * direction);
        leftWall.GetComponent<Wall>().MatrixWorldPosition = MatrixWorldPosition + (0.5f * direction);
        middleGate.GetComponent<Gate>().MatrixWorldPosition = MatrixWorldPosition + (0.5f * direction);
        sensor.GetComponent<BoundarySensor>().MatrixWorldPosition = MatrixWorldPosition + (0.5f * direction);

        //Updating the Registry
        StoredComponents.LevelMetaData.Registry.Walls.Add(rightWall.GetComponent<Wall>());
        StoredComponents.LevelMetaData.Registry.Walls.Add(leftWall.GetComponent<Wall>());   
        StoredComponents.LevelMetaData.Registry.Gates.Add(middleGate.GetComponent<Gate>());
        StoredComponents.LevelMetaData.Registry.Sensors.Add(sensor.GetComponent<BoundarySensor>());

    }

    /// <summary>
    /// Creates a full wall.
    /// </summary>
    /// <param name="direction">Which edge the wall occupies.</param>
    private void CreateFullWall(Vector2 direction)
    {
        Vector3 eulerAngles = Utilities.Math.Vector.GetEulerAnglesPerpendicularToVector2(direction);

        GameObject fullWall = Instantiate(wall, gameObject.transform);
        fullWall.GetComponent<Wall>().Type = WallType.full;
        fullWall.transform.localPosition = direction * 0.5f;
        fullWall.transform.localScale = new Vector3(1, 0.0125f, 1);
        fullWall.transform.localEulerAngles = eulerAngles;

    }

    #endregion

}
