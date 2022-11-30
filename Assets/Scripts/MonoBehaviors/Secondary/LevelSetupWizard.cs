using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTO.Setup;
using DTO.Storage;
using System;

public class LevelSetupWizard : MonoBehaviour
{
    //Properties set in the inspector
    #region Inspector Properties

    /// <summary>
    /// Floor object for the Wizard to instantiate
    /// </summary>
    public GameObject floorObject;

    #endregion

    //Properties set in code
    #region Code Properties

    /// <summary>
    /// Matrix codifying the locations of each floor.
    /// </summary>
    public LevelInitializationMatrix floorMatrix { get; set; } = new LevelInitializationMatrix();

    /// <summary>
    /// List tracking the floors in the Level.
    /// </summary>
    private List<Floor> floors { get; set; } = new List<Floor>();

    #endregion

    //Initialization
    #region Initialization

    public void Awake()
    {
        CreateSampleLevel();
    }

    #endregion

    //Sample Level
    #region Sample Level

    /// <summary>
    /// Function that creates a demo level. Likely unnecessary in production,
    /// just being used as a demo for development purposes.
    /// </summary>
    private void CreateSampleLevel()
    {
        var desert = Instantiate(floorObject);
        var ice = Instantiate(floorObject);
        var jungle = Instantiate(floorObject);
        var savannah = Instantiate(floorObject);

        var desertFloor = desert.GetComponent<Floor>();
        var iceFloor = ice.GetComponent<Floor>();
        var jungleFloor = jungle.GetComponent<Floor>();
        var savannahFloor = savannah.GetComponent<Floor>();

        floors = new List<Floor> { desertFloor, iceFloor, jungleFloor, savannahFloor };

        floorMatrix.AddFloorFromWorldCoordinates(0, 0, desertFloor);
        floorMatrix.AddFloorFromWorldCoordinates(1, 0, iceFloor);
        floorMatrix.AddFloorFromWorldCoordinates(0, 1, jungleFloor);
        floorMatrix.AddFloorFromWorldCoordinates(1, 1, savannahFloor);

        desertFloor.mainColor = Color.red;
        iceFloor.mainColor = Color.cyan;
        jungleFloor.mainColor = Color.green;
        savannahFloor.mainColor = Color.yellow;

        foreach (Floor floor in floors)
        {

            Vector2 currentMatrixPosition = new Vector2(floor.matrix_X - 3, floor.matrix_Y - 3);

            foreach (Vector2 direction in StoredMisc.UDLR)
            {
                floor.neighbors.neighbors[direction] = CheckForNeighbor(currentMatrixPosition, direction);
            }

            floor.gameObject.transform.position = new Vector2(floor.matrix_X - 3, floor.matrix_Y - 3) * floor.size;
            floor.AddWalls();
        }
    }

    #endregion

    //Helper Functions for Level Setup
    #region Setup Functions

    /// <summary>
    /// Checks to see if a floor has a neighbor in the given direction.
    /// </summary>
    /// <param name="location">The location of the floor.</param>
    /// <param name="direction">The direction of the neighbor.</param>
    /// <returns></returns>
    private bool CheckForNeighbor(Vector2 location, Vector2 direction)
    {
        try
        {
            if (floorMatrix.IndexFromWorldCoordinatesVector(location + direction) != null)
            {
                return true;
            }
        }
        catch (IndexOutOfRangeException) {}
        return false;
    }

    #endregion

}
