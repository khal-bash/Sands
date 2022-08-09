using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Governs the behavior of collectables.
/// </summary>
public class Collectable : MonoBehaviour
{
    //Properties set in the Inspector
    #region Inspector Properties

    /// <summary>
    /// The <see cref="Inventory.CollectableType"/> of the Collectable.
    /// </summary>
    public Inventory.CollectableType Type;

    #endregion

}
