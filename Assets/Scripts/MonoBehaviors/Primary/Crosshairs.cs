using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTO.Storage;

/// <summary>
/// Governs the behavior of the player's crosshairs.
/// </summary>
public class Crosshairs : MonoBehaviour
{

    /// Properties set in the Inspector
    #region Inspector Properties

    /// <summary>
    /// Gets or sets the maximum allowed distance the beam can travel.
    /// </summary>
    public float Fire_Range;

    #endregion

    /// Built-in Unity Functions
    #region Unity Functions

    void Update()
    {
        FollowMouse();
    }

    #endregion

    /// Automated Crosshair Movement
    #region Movement

    /// <summary>
    /// Follows the cursor to the allowable range set by <see cref="Fire_Range"/>.
    /// </summary>
    void FollowMouse()
    {
        
        Vector2 player_position = StoredComponents.Player_Transform.position;

        Vector2 cursor_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 player_to_cursor = cursor_position - player_position;

        Vector2 final_position = cursor_position;

        if (player_to_cursor.magnitude > Fire_Range)
        {
            Vector2 max_range_vector = Fire_Range * player_to_cursor.normalized;
            final_position = player_position + max_range_vector;
        }

        transform.position = final_position;
    }

    #endregion

}
