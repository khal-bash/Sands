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

    public Floor locale { get => gameObject.transform.parent.gameObject.GetComponent<Floor>(); }

    public void SetVisuals(Theme theme)
    {
        gameObject.GetComponent<SpriteRenderer>().color = ThemeHandler.collectableColor(theme);
        Type = ThemeHandler.collectableType(theme);
    }

    #endregion

}
