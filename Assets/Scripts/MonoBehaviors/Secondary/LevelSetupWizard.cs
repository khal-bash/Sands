using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using System.Linq;
using UnityEngine;
using DTO.Setup;
using DTO.Storage;
using DTO.Visuals;
using System;

/// <summary>
/// Class that generates a level.
/// </summary>
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
    /// The <see cref="LevelMetaData"/> the wizard should use.
    /// </summary>
    private LevelMetaData metaData { get; set; }

    /// <summary>
    /// Matrix codifying the locations of each floor.
    /// </summary>
    public LevelInitializationMatrix floorMatrix { get; set; } = new LevelInitializationMatrix();

    /// <summary>
    /// List tracking the floors in the Level.
    /// </summary>
    public List<Floor> floors { get; set; } = new List<Floor>();

    /// <summary>
    /// List tracking the walls and gates in the Level.
    /// </summary>
    public List<GameObject> wallsAndGates { get; set; } = new List<GameObject>();

    #endregion

    //Initialization
    #region Initialization

    public void Awake()
    {
        InitializeCodeProperties();
        PopulateLevelWithRandomFloors(metaData.numberOfFloors, metaData.random);
        // CreateSampleLevel();
    }

    /// <summary>
    /// Initializes various code properties.
    /// </summary>
    private void InitializeCodeProperties()
    {
        metaData = gameObject.GetComponent<LevelMetaData>();
    }

    /// <summary>
    /// Generates a random floor matrix and instantiates it in the level.
    /// </summary>
    /// <param name="numberOfFloors">The number of floors desired.</param>
    /// <param name="random">The random generator.</param>
    public void PopulateLevelWithRandomFloors(int numberOfFloors, Random random)
    {
        GenerateRandomFloorMatrix(numberOfFloors, random);
        SetUpFloors();
    }

    #endregion

    //Populating the Floor Matrix
    #region Matrix Generation

    /// <summary>
    /// Generates a floor matrix to create the level from.
    /// </summary>
    /// <param name="numberOfFloors">The number of floors to create.</param>
    /// <param name="random">The random generator.</param>
    private void GenerateRandomFloorMatrix(int numberOfFloors, Random random)
    {
        int[,] MinesweeperNeighborsVotesMatrix = new int[5, 5];
        MinesweeperNeighborsVotesMatrix[2, 2] = 1;

        int floorPlaced = 1;
        while (floorPlaced <= numberOfFloors)
        { 

            GenerateFloor(random, MinesweeperNeighborsVotesMatrix);

            floorPlaced++;
        }
    }

    /// <summary>
    /// Adds a floor to the <see cref="floorMatrix"/>
    /// </summary>
    /// <param name="random">The random generator.</param>
    /// <param name="MinesweeperNeighborsVotesMatrix">A matrix containing the votes for each square</param>
    private void GenerateFloor(Random random, int[,] MinesweeperNeighborsVotesMatrix)
    {
        Vector2Int selectedLocation = Vote(MinesweeperNeighborsVotesMatrix, random);
        CreateNextFloor(selectedLocation, random);
        UpdateVotesMatrix(MinesweeperNeighborsVotesMatrix, selectedLocation);
    }

    //Handles the voting procedure for selecting the location of the next floor.
    #region Voting

    /// <summary>
    /// Updates the voting matrix after adding a floor.
    /// </summary>
    /// <param name="VotesMatrix">The old voting matrix.</param>
    /// <param name="selectedLocation">The location of the added floor.</param>
    private void UpdateVotesMatrix(int[,] VotesMatrix, Vector2Int selectedLocation)
    {
        foreach (Vector2Int direction in StoredConstants.Adjacencies)
        {
            AddVoteToNeighbor(VotesMatrix, selectedLocation, direction);
        }
        foreach (Floor existingFloor in floors)
        {
            VotesMatrix[existingFloor.matrixRawPosition.x, existingFloor.matrixRawPosition.y] = 0;
        }


    }

    /// <summary>
    /// Adds a vote to a neighboring direction.
    /// </summary>
    /// <param name="VotesMatrix">The old voting matrix.</param>
    /// <param name="selectedLocation">The location of the added floor.</param>
    /// <param name="direction">The direction of the neighbor.</param>
    private static void AddVoteToNeighbor(int[,] VotesMatrix, Vector2Int selectedLocation, Vector2Int direction)
    {
        Vector2Int location = selectedLocation + direction;
        try
        {
            VotesMatrix[location.x + 2, location.y + 2]++;
        }
        catch (IndexOutOfRangeException)
        { }
    }

    /// <summary>
    /// Draws a vote.
    /// </summary>
    /// <param name="votesMatrix">The voting matrix.</param>
    /// <param name="random">The random generator.</param>
    /// <returns></returns>
    private Vector2Int Vote(int[,] votesMatrix, Random random)
    {
        return DrawVoteAndValidateSelection(random, CreateBallot(votesMatrix));
    }

    /// <summary>
    /// Creates a ballot from a voting matrix.
    /// </summary>
    /// <param name="votesMatrix">The voting matrix.</param>
    private static List<Vector2Int> CreateBallot(int[,] votesMatrix)
    {
        var ballot = new List<Vector2Int>();

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int votes = 0; votes < votesMatrix[x, y]; votes++)
                {
                    ballot.Add(new Vector2Int(x - 2, y - 2));
                }
            }
        }

        return ballot;
    }

    /// <summary>
    /// Draws a vote from a ballot and validates that that floor would be reachable by the player.
    /// </summary>
    /// <param name="random">The random generator.</param>
    /// <param name="ballot">The ballot of possible floors.</param>
    /// <returns></returns>
    private Vector2Int DrawVoteAndValidateSelection(Random random, List<Vector2Int> ballot)
    {
        while (true)
        {

            int indexSelected = random.Next(0, ballot.Count - 1);
            Vector2Int selected = ballot[indexSelected];
            bool selectionHasValidNeighbor = false;

            foreach (Vector2Int direction in StoredConstants.UDLR)
            {

                try
                {
                    if (floorMatrix[selected + direction] != null || ballot.Count == 1)
                    {
                        selectionHasValidNeighbor = true;
                        break;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    bool selectionIsOnBorder = ((selected.x == -2) || (selected.x == 2) || (selected.y == -2) || (selected.y == 2));
                    if (!selectionIsOnBorder) { throw; }
                }

            }

            if (selectionHasValidNeighbor)
            {
                return selected;
            }
            else
            {
                ballot.RemoveAt(indexSelected);
            }
        }
    }

    #endregion

    #endregion

    //Creating Floors and Defining their properties
    #region Floor Creation

    /// <summary>
    /// Instantiates a floor and sets up the visuals.
    /// </summary>
    /// <param name="selectedLocation">The location of the floor in the matrix</param>
    /// <param name="random">The random generator.</param>
    private void CreateNextFloor(Vector2Int selectedLocation, Random random)
    {
        var floor = Instantiate(floorObject);
        var floorComponent = floor.GetComponent<Floor>();

        floorComponent.PopulateProperties(PickRandomTheme(selectedLocation, random), selectedLocation);

        floors.Add(floorComponent);
        floorMatrix[selectedLocation] = floorComponent;
    }

    /// <summary>
    /// Picks a random theme that is not the same as one of its neighbors
    /// </summary>
    /// <param name="floorMatrixPosition">The location of the floor in the <see cref="floorMatrix"/></param>
    /// <param name="random">The random generator.</param>
    /// <returns>The chosen theme.</returns>
    private Theme PickRandomTheme(Vector2Int floorMatrixPosition, Random random)
    {

        List<Theme> possibleThemes = FloorVisuals.AllSpecifiedThemes;

        foreach (Vector2Int direction in StoredConstants.UDLR)
        {
            Vector2Int test_position = floorMatrixPosition + direction;
            try
            {
                possibleThemes.Remove(floorMatrix[test_position].visuals.theme);
            }
            catch (IndexOutOfRangeException) { }
            catch (NullReferenceException) { }
        }

        return possibleThemes[random.Next(0, possibleThemes.Count)];
    }


    /// <summary>
    /// Positions the floors from the floor matrix in the actual level and loads the walls.
    /// </summary>
    private void SetUpFloors()
    {
        foreach (Floor floor in floors)
        {

            foreach (Vector2Int direction in StoredConstants.UDLR)
            {
                floor.neighbors.neighbors[direction] = floorMatrix.CheckForNeighbor(floor.matrixWorldPosition, direction);
            }

            floor.gameObject.transform.position = (Vector2)floor.matrixWorldPosition * Floor.size;
            wallsAndGates.AddRange(floor.AddWallsAndGates());
        }

        RemoveDuplicateWallsAndGates();
    }

    /// <summary>
    /// Removes all duplicate walls and gates.
    /// </summary>
    private void RemoveDuplicateWallsAndGates()
    {

        var observedPositions = new List<Vector3>();

        foreach (GameObject wallOrGate in wallsAndGates)
        {
            Vector3 objectPosition = wallOrGate.transform.position;
            if(observedPositions.Contains(objectPosition))
            {
                Destroy(wallOrGate);
                continue;
            }
            observedPositions.Add(objectPosition);
        }
    }

    #endregion

}


