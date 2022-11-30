using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The two possible types of wall.
/// </summary>
public enum WallType
{
    full,
    half
}

/// <summary>
/// Class governing the behaviour of the wall GameObject.
/// </summary>
public class Wall : MonoBehaviour
{

    #region Code Properties

    /// <summary>
    /// The type of wall.
    /// </summary>
    public WallType type { get; set; }

    #endregion

}
