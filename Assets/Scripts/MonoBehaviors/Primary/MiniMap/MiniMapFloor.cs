using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class governing the behavior of Mini Map Floors
/// </summary>
public class MiniMapFloor : MonoBehaviour
{

    //Built-in Unity Functions
    #region Unity Functions
    
    private void Start()
    {
        var miniMap = GameObject.Find("MiniMap").GetComponent<MiniMap>();
        miniMap.FloorColorSet += SetSpecificMiniMapFloorColor;
    }

    #endregion

    //Setting Colors
    #region Color

    /// <summary>
    /// Sets this GameObject's color if it is the correct floor.
    /// </summary>
    private void SetSpecificMiniMapFloorColor(GameObject targetMiniMapFloor, Color color)
    {
        if (targetMiniMapFloor == gameObject)
        {
            SetColor(color);
        }
    }

    /// <summary>
    /// Sets this GameObject's Color
    /// </summary>
    /// <param name="color"></param>
    private void SetColor(Color color)
    {
        if (color.a == 0f)
        {
            SpriteRenderer[] childRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

            foreach (var renderer in childRenderers)
            {
                renderer.color = color;
            }
        }

        gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    #endregion

}
