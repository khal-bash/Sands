using UnityEngine;
using DTO.Storage;
using System;

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
    private Inventory requirements;

    public bool isVertical { get => DetermineWhetherVertical(); }

    private bool DetermineWhetherVertical()
    {
        Transform left = gameObject.transform.GetChild(0);
        Transform right = gameObject.transform.GetChild(1);
        return (Math.Abs(left.position.y - right.position.y) < 0.1);
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

    // Initializes Code Properties
    #region Initialization

    // Inititalizes the Code Properties
    private void InitializeCodeProperties()
    {
        requirements = new Inventory(diamond, seashell, lavender, ruby);
    }

    #endregion
}
