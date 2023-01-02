using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTO.Visuals;

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

    /// <summary>
    /// The floor that the collectable inhabits.
    /// </summary>
    public Floor Locale { get => gameObject.transform.parent.gameObject.GetComponent<Floor>(); }

    /// <summary>
    /// Sets the collectable's visual appearance.
    /// </summary>
    /// <param name="theme"></param>
    public void SetVisuals(Theme theme)
    {
        gameObject.GetComponent<SpriteRenderer>().color = ThemeHandler.Accord(theme, "collectable");
        Type = ThemeHandler.Accord(theme);
    }

    #endregion

}
