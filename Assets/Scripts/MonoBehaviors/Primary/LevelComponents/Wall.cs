using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The two possible types of wall.
/// </summary>
public enum WallType
{
    full,
    lefthalf,
    righthalf,
}

/// <summary>
/// Class governing the behaviour of the wall GameObject.
/// </summary>
public class Wall : LevelComponent
{

    /// <summary>
    /// Two walls are equality if they are equal under <see cref="LevelComponent.Equals(LevelComponent)"/>
    /// AND the wall type of the two walls are equal.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool Equals(LevelComponent other)
    {
        if (!base.Equals(other)) { return false; }
        return (other.gameObject.GetComponent<Wall>().Type == Type);
    }

    //Properties set in code.
    #region Code Properties

    /// <summary>
    /// The type of wall.
    /// </summary>
    public WallType Type { get; set; }

    #endregion

}
