using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Super class for shared behaviors of <see cref="Wall"/>, <see cref="Gate"/>, <see cref="BoundarySensor"/>, and <see cref="Floor"/> objects.
/// </summary>
public class LevelComponent : MonoBehaviour, IEquatable<LevelComponent>
{

    /// <summary>
    /// The coordinates of the matrix in world position [origin at (0,0)]
    /// </summary>
    public Vector2 MatrixWorldPosition { get; set; }

    /// <summary>
    /// The coordinates of the matrix in raw position [origin at (2,2)].
    /// </summary>
    public Vector2 MatrixRawPosition { get { return MatrixWorldPosition + new Vector2(2, 2); } }

    /// <summary>
    /// Sets two LevelComponents equal if they share the same Subtype (wall, gate, boundarySensor, floor)
    /// and are in the same position in the matrix.
    /// </summary>
    public virtual bool Equals(LevelComponent other)
    {
        if(GetSubType(other) != GetMySubType())
        {
            return false;
        }
        if(MatrixWorldPosition == other.MatrixWorldPosition)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the Subtype of a given LevelComponent.
    /// </summary>
    private Type GetSubType(LevelComponent levelComponent)
    {
        if(levelComponent.gameObject.GetComponent<Floor>() != null)
        {
            return typeof(Floor);
        }
        if (levelComponent.gameObject.GetComponent<Wall>() != null)
        {
            return typeof(Wall);
        }
        if (levelComponent.gameObject.GetComponent<Gate>() != null)
        {
            return typeof(Gate);
        }
        if (levelComponent.gameObject.GetComponent<BoundarySensor>() != null)
        {
            return typeof(BoundarySensor);
        }
        return null;
    }

    /// <summary>
    /// Gets the Subtype of this LevelComponent.
    /// </summary>
    private Type GetMySubType()
    {
        return GetSubType(this);
    }
}
