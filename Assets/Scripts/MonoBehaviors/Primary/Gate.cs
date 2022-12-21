using UnityEngine;
using DTO.Storage;
using System;
using System.Collections.Generic;

/// <summary>
/// Gives an Impassable object an inventory-checking requirement behavior.
/// </summary>
public class Gate : MonoBehaviour
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
    /// The position of the gate in terms of the floor matrix. Note 
    /// that the gate isn't actually contained in the floor matrix, since it
    /// isn't a floor, but rather this value encodes the gate's position
    /// relative to those floors using 0.5 to mean in between two floors.
    /// </summary>
    public Vector2 matrixWorldPosition;

    /// <summary>
    /// The floors on either side of the gate.
    /// </summary>
    public List<Floor> neighbors { get => GetAllNeighboringFloors(); }

    /// <summary>
    /// Gets the two floors on either side of this gate.
    /// </summary>
    /// <returns></returns>
    private List<Floor> GetAllNeighboringFloors()
    {
        var floorMatrix = GameObject.Find("Generator").GetComponent<LevelSetupWizard>().floorMatrix;
        return new List<Floor>() { floorMatrix[Utilities.Math.Vector.Ceil(matrixWorldPosition)],
                                       floorMatrix[Utilities.Math.Vector.Floor(matrixWorldPosition)]};

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
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            UIDisplay = Instantiate(UIDisplayPrefab, transform);
            UIDisplay.GetComponent<CollectableRequirementsDisplay>().ShapeBackground(requirements);
        }

        if (Input.GetKeyUp(KeyCode.RightShift))
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
