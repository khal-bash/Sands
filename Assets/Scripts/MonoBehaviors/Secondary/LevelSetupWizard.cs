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

    /// <summary>
    /// The probability of adding a possible requirement to any gate.
    /// 0 would mean no gates have requirements, and 1 would mean gates
    /// have every possible requirement.
    /// </summary>
    public float requirementProbability = 0.5f;

    #endregion

    //Properties set in code
    #region Code Properties

    /// <summary>
    /// The <see cref="LevelMetaData"/> the wizard should use.
    /// </summary>
    private LevelMetaData MetaData { get; set; }

    #endregion

    //Initialization
    #region Initialization

    public void Awake()
    {
        InitializeCodeProperties();
        PopulateLevelWithRandomFloors(MetaData.numberOfFloors, MetaData.Random);
        GenerateGateRequirements(MetaData.Random);
    }

    /// <summary>
    /// Initializes various code properties.
    /// </summary>
    private void InitializeCodeProperties()
    {
        MetaData = gameObject.GetComponent<LevelMetaData>();
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
        StoredComponents.Player.Floors_Visited = new List<Floor>() { StoredComponents.LevelMetaData.Registry.FloorMatrix.Origin };
        StoredComponents.Player.Current_Location = StoredComponents.LevelMetaData.Registry.FloorMatrix.Origin;
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
    /// Adds a floor to the <see cref="FloorMatrix"/>
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
        foreach (Floor existingFloor in MetaData.Registry.Floors)
        {
            VotesMatrix[(int) existingFloor.MatrixRawPosition.x, (int) existingFloor.MatrixRawPosition.y] = 0;
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
                    if (MetaData.Registry.FloorMatrix[selected + direction] != null || ballot.Count == 1)
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

        MetaData.Registry.Floors.Add(floorComponent);
        MetaData.Registry.FloorMatrix[selectedLocation] = floorComponent;
    }

    /// <summary>
    /// Picks a random theme that is not the same as one of its neighbors
    /// </summary>
    /// <param name="floorMatrixPosition">The location of the floor in the <see cref="FloorMatrix"/></param>
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
                possibleThemes.Remove(MetaData.Registry.FloorMatrix[test_position].visuals.Theme);
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
        foreach (Floor floor in MetaData.Registry.Floors)
        {

            foreach (Vector2Int direction in StoredConstants.UDLR)
            {
                floor.Neighbors.NeighborsReference[direction] = MetaData.Registry.FloorMatrix.CheckForNeighbor(Utilities.Math.Vector.Floor(floor.MatrixWorldPosition), direction);
            }

            floor.gameObject.transform.position = (Vector2) floor.MatrixWorldPosition * Floor.size;
            floor.AddWallsAndGates();
        }

        //RemoveDuplicateWallsAndGates();
    }

    /// <summary>
    /// Adds a new floor to the list of those discovered.
    /// </summary>
    /// <param name="previousSelection">The gate just opened by the virtual player.</param>
    /// <param name="floorsDiscovered">The list of floors discovered.</param>
    /// <returns></returns>
    private Floor DiscoverNewFloor(Gate previousSelection, List<Floor> floorsDiscovered)
    {
        Floor newFloor = previousSelection.Neighbors[0];
        if (floorsDiscovered.Contains(previousSelection.Neighbors[0])) { newFloor = previousSelection.Neighbors[1]; }

        floorsDiscovered.Add(newFloor);

        return newFloor;
    }

    #endregion

    //Instantiating Gate Requirements
    #region Gate Requirements

    /// <summary>
    /// Generates random requirements for all gates in the level.
    /// </summary>
    /// <param name="random">The random generator to be used.</param>
    private void GenerateGateRequirements(Random random)
    {

        var floorsDiscovered = new List<Floor>() { MetaData.Registry.FloorMatrix.Origin };
        var gatesAssignedTo = new List<Gate>();

        var collectableTypesDiscovered = new List<Inventory.CollectableType>() 
        {
            ThemeHandler.Set(MetaData.Registry.FloorMatrix.Origin.Theme)
        };

        var gateHorizon = EnumerateFirstGateHorizon();

        while (gateHorizon.Count > 0)
        {
            AssignRequirementsToNextRandomGate(random, floorsDiscovered, gatesAssignedTo, collectableTypesDiscovered, gateHorizon);
        }
    }

    /// <summary>
    /// Chooses a random gate and then assigns it requirements.
    /// </summary>
    /// <param name="random">The random generator to be used.</param>
    /// <param name="floorsDiscovered">The floors that are reachable by the player.</param>
    /// <param name="gatesAssignedTo">The gates that already have items assigned to them.</param>
    /// <param name="collectableTypesDiscovered">The types of collectables that the player has discovered.</param>
    /// <param name="gateHorizon">The possible gates the the player could reach at this point and are therefore candidates for
    /// selection.</param>
    private void AssignRequirementsToNextRandomGate(Random random, List<Floor> floorsDiscovered, List<Gate> gatesAssignedTo, List<Inventory.CollectableType> collectableTypesDiscovered, List<Gate> gateHorizon)
    {
        var selection = gateHorizon[random.Next(0, gateHorizon.Count - 1)];

        AssignRequirementsToGate(random, collectableTypesDiscovered, selection);
        gatesAssignedTo.Add(selection);
        UpdateGateHorizon(gateHorizon, selection, gatesAssignedTo, floorsDiscovered, collectableTypesDiscovered);
    }

    /// <summary>
    /// Decides which items the gate should require.
    /// </summary>
    /// <param name="random">The random generator to be used.</param>
    /// <param name="collectableTypesDiscovered">The types of collectables that the player has discovered.</param>
    /// <param name="selection">The gate to assign to.</param>
    private void AssignRequirementsToGate(Random random, List<Inventory.CollectableType> collectableTypesDiscovered, Gate selection)
    {
        foreach (var type in collectableTypesDiscovered)
        {
            if (random.Next(1, 1000) < (1000 * requirementProbability))
            {
                selection.requirements.AddItem(type);
            }
        }
    }

    /// <summary>
    /// Gets the gate horizon for the first iteration.
    /// </summary>
    private List<Gate> EnumerateFirstGateHorizon()
    {
        return MetaData.Registry.FloorMatrix[Vector2Int.zero].AttachedGates;
    }

    /// <summary>
    /// Update the gate horizon (the list of all gates we can see and
    /// therefore can be assigned to next).
    /// </summary>
    /// <param name="oldGateHorizon">The current gate horizon.</param>
    /// <param name="previousSelection">The gate that was just assigned to</param>
    /// <param name="gatesSet">The gates that have been assigned to.</param>
    /// <param name="floorsDiscovered">The floors that the player has discovered.</param>
    /// <param name="typesAvailable">The collectable types that the player has discovered.</param>
    private void UpdateGateHorizon(List<Gate> oldGateHorizon,
                                      Gate previousSelection,
                                      List<Gate> gatesSet,
                                      List<Floor> floorsDiscovered,
                                      List<Inventory.CollectableType> typesAvailable)
    {
        oldGateHorizon.Remove(previousSelection);
        Floor newFloor = DiscoverNewFloor(previousSelection, floorsDiscovered);

        var floorCollectableType = ThemeHandler.Set(newFloor.Theme);
        if (!typesAvailable.Contains(floorCollectableType)) { typesAvailable.Add(floorCollectableType); }

        CreateGateHorizonWithoutDuplicates(oldGateHorizon, gatesSet, newFloor);
    }

    /// <summary>
    /// Determines which gates to add to the gate horizon to avoid duplicates.
    /// </summary>
    /// <param name="oldGateHorizon">The current gate horizon.</param>
    /// <param name="gatesSet">The gates that have been assigned to.</param>
    /// <param name="newFloor">The new floor that the player just discovered.</param>
    private void CreateGateHorizonWithoutDuplicates(List<Gate> oldGateHorizon, List<Gate> gatesSet, Floor newFloor)
    {
        foreach (Gate newGate in newFloor.AttachedGates)
        {
            if (gatesSet.Contains(newGate) || oldGateHorizon.Contains(newGate)) { continue; }
            oldGateHorizon.Add(newGate);
        }
    }

    #endregion
}


