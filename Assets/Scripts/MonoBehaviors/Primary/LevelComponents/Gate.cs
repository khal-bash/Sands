using UnityEngine;
using DTO.Storage;
using DTO.Delegates;
using System;
using System.Collections.Generic;

/// <summary>
/// Gives an Impassable object an inventory-checking requirement behavior.
/// </summary>
public class Gate : LevelComponent
{

    // Properties set in the Inspector
    #region Inspector Properties

    /// <summary>
    /// The required number of <see cref="Inventory.CollectableType.diamond"/>
    /// objects required to open the gate.
    /// </summary>
    public int diamond;

    /// <summary>
    /// The required number of <see cref="Inventory.CollectableType.seashell"/>
    /// objects required to open the gate.
    /// </summary>
    public int seashell;

    /// <summary>
    /// The required number of <see cref="Inventory.CollectableType.lavender"/>
    /// objects required to open the gate.
    /// </summary>
    public int lavender;

    /// <summary>
    /// The required number of <see cref="Inventory.CollectableType.ruby"/>
    /// objects required to open the gate.
    /// </summary>
    public int ruby;

    /// <summary>
    /// The required number of <see cref="Inventory.CollectableType.coal"/>
    /// objects required to open the gate.
    /// </summary>
    public int coal;

    /// <summary>
    /// The object to display the gate requirements.
    /// </summary>
    public GameObject UIDisplayPrefab;

    #endregion

    // Properties set in code
    #region Code Properties

    /// <summary>
    /// Whether or not the requirements have been satisfied.
    /// </summary>
    private bool Satisfied = false;

    /// <summary>
    /// The requirements set in the Inspector in <see cref="Inventory"/> form.
    /// </summary>
    public Inventory requirements;

    /// <summary>
    /// The display spawned in <see cref="DisplayRequirements"/>
    /// </summary>
    private GameObject UIDisplay;

    /// <summary>
    /// The floors on either side of the gate.
    /// </summary>
    public List<Floor> Neighbors { get => GetAllNeighboringFloors(); }

    /// <summary>
    /// Gets the two floors on either side of this gate.
    /// </summary>
    /// <returns></returns>
    private List<Floor> GetAllNeighboringFloors()
    {
        var floorMatrix = GameObject.Find("Generator").GetComponent<LevelMetaData>().Registry.FloorMatrix;
        return new List<Floor>() { floorMatrix[Utilities.Math.Vector.Ceil(MatrixWorldPosition)],
                                       floorMatrix[Utilities.Math.Vector.Floor(MatrixWorldPosition)]};

    }

    #endregion

    //Events and Handlers
    #region Events

    /// <summary>
    /// Event that triggers when the gate is opened.
    /// </summary>
    public static event InventoryManagementAction GateOpened;

    virtual protected void OnGateOpened()
    {
        GateOpened?.Invoke(requirements);
    }

    #endregion

    // Built-in Unity Functions
    #region Unity Functions

    private void Awake()
    {
        InitializeCodeProperties();
    }

    private void Update()
    {
        Satisfied = (Inventory.IsSubInventory(requirements, StoredClasses.Player_Inventory));
        DisplayRequirements();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Satisfied)
        {
            OnGateOpened();
            Destroy(gameObject);
            StoredClasses.Player_Inventory -= requirements;
        }
    }

    #endregion

    //User Interfacing
    #region UI

    /// <summary>
    /// Initializes a <see cref="CollectableRequirementsDisplay"/> to show the gate's requirements.
    /// </summary>
    private void DisplayRequirements()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            UIDisplay = Instantiate(UIDisplayPrefab, transform);
            UIDisplay.GetComponent<CollectableRequirementsDisplay>().DisplaySelf(requirements);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            try { Destroy(UIDisplay); }
            catch (NullReferenceException) { }
        }
    }

    #endregion

    // Initializes Code Properties
    #region Initialization

    // Inititalizes the Code Properties
    private void InitializeCodeProperties()
    {
        requirements = new Inventory(diamond, seashell, lavender, ruby);
    }

    #endregion
}
