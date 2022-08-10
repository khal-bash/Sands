using UnityEngine;
using DTO.Storage;

/// <summary>
/// Gives an Impassable object an inventory-checking requirement behavior.
/// </summary>
public class Gate : MonoBehaviour
{

    // Properties set in the Inspector
    #region Inspector Properties

    /// <summary>
    /// The required number of <see cref="Inventory.CollectableType.type0"/>
    /// objects required to open the gate.
    /// </summary>
    public int type0;

    /// <summary>
    /// The required number of <see cref="Inventory.CollectableType.type1"/>
    /// objects required to open the gate.
    /// </summary>
    public int type1;

    /// <summary>
    /// The required number of <see cref="Inventory.CollectableType.type2"/>
    /// objects required to open the gate.
    /// </summary>
    public int type2;

    /// <summary>
    /// The required number of <see cref="Inventory.CollectableType.type3"/>
    /// objects required to open the gate.
    /// </summary>
    public int type3;

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
        requirements = new Inventory(type0, type1, type2, type3);
    }

    #endregion
}
