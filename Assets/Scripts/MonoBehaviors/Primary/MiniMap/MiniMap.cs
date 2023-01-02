using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTO.Storage;
using DTO.Delegates;
using Utilities;
using DTO.Visuals;

/// <summary>
/// Class governing the MiniMap Display.
/// </summary>
public class MiniMap : MonoBehaviour
{

    //Properties Set in Code
    #region Code Properties

    /// <summary>
    /// Whether the MiniMap has been refreshed for the first time.
    /// </summary>
    private bool initialized = false;

    #endregion

    //Events and handlers
    #region Events

    /// <summary>
    /// Event announcing that a floor color has been set.
    /// </summary>
    public event ChangeGameObjectColorAction FloorColorSet;

    /// <summary>
    /// Event Handler for <see cref="FloorColorSet"/>.
    /// </summary>
    /// <param name="color">The color being set</param>
    /// <param name="x">The x-coordinate of the floor being set.</param>
    /// <param name="y">The y-coordinate of the floor being set.</param>
    protected virtual void SetFloorColor(Color color, int x, int y)
    {
        FloorColorSet?.Invoke(this[x, y].gameObject, color);
    }

    #endregion

    //Custom Indexers
    #region Indexers

    public GameObject this[int x, int y]
    {
        get
        {
            foreach (var childTransform in transform.GetComponentsInChildren<Transform>())
            {
                if (childTransform.gameObject.name.Contains("Side") ||
                    childTransform.gameObject.name == "MiniMapBackground" ||
                    childTransform.gameObject.name == "MiniMapPlayerTracker")
                {
                    continue;
                }

                if (childTransform.localPosition == new Vector3(x, y))
                {
                    return childTransform.gameObject;
                }
            }
            return null;
        }
    }

    #endregion

    //Built-in Unity Functions
    #region Unity Functions

    private void Update()
    {
        if (initialized) { return; }
        RefreshMiniMap();
        initialized = true;
    }

    #endregion

    //Sets the visual appearance of the MiniMap
    #region Optics

    /// <summary>
    /// Refreshes the minimap to display all revealed squares.
    /// </summary>
    public void RefreshMiniMap()
    {
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                UpdateFloor(x, y);
            }
        }

        GameObject.Find("MiniMapPlayerTracker").transform.localPosition = StoredComponents.Player.Current_Location.MatrixWorldPosition;
    }

    /// <summary>
    /// Updates a <see cref="MiniMapFloor"/> given its coordinates
    /// </summary>
    /// <param name="x">The x-coordinate of the floor.</param>
    /// <param name="y">The y-coordinate of the floor.</param>
    private void UpdateFloor(int x, int y)
    {
        var analogousFloor = StoredComponents.LevelMetaData.Registry.FloorMatrix[x, y];
        if (analogousFloor == null)
        {
            MakeFloorTransparent(x, y);
        }

        else if (StoredComponents.Player.Floors_Visited.Contains(analogousFloor))
        {
            Reveal(analogousFloor, x, y);
        }
    }

    /// <summary>
    /// Makes a floor transparent.
    /// </summary>
    /// <param name="x">The x-coordinate of the floor.</param>
    /// <param name="y">The y-coordinate of the floor.</param>
    private void MakeFloorTransparent(int x, int y)
    {
        var transparency = Visual.ChangeOpacity(this[x, y].GetComponent<SpriteRenderer>().color, 0f);
        SetFloorColor(transparency, x, y);
    }

    /// <summary>
    /// Removes the fog from the minimap.
    /// </summary>
    /// <param name="analogousFloor">The analagous floor in the level.</param>
    /// <param name="x">The x-coordinate of the floor</param>
    /// <param name="y">The y-coordinate of the floor</param>
    private void Reveal(Floor analogousFloor, int x, int y)
    {
        SetFloorColor(ThemeHandler.Accord(analogousFloor.Theme, "minimap"), x, y);
    }

    #endregion

}
