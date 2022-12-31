using System.Collections;
using System.Collections.Generic;
using DTO.Delegates;
using DTO.Storage;
using UnityEngine;
using static Utilities.Math.Vector;

/// <summary>
/// Class governing the behavior of boundary sensor.
/// </summary>
public class BoundarySensor : LevelComponent
{

    //Properties Set In Code
    #region Code Properties

    /// <summary>
    /// Whether or not the sensor has been released.
    /// </summary>
    private bool released = true;

    /// <summary>
    /// Which side the player entered the sensor on.
    /// </summary>
    private Side enteringSide = Side.none;

    /// <summary>
    /// Which side the player exited the sensor on.
    /// </summary>
    private Side exitingSide = Side.none;

    /// <summary>
    /// The two floors on either side of the sensor/
    /// </summary>
    public List<Floor> Neighbors {
        get 
        {
            var floorMatrix = GameObject.Find("Generator").GetComponent<LevelMetaData>().Registry.FloorMatrix;
            return new List<Floor>()
            {
              floorMatrix[Utilities.Math.Vector.Ceil(MatrixWorldPosition)],
              floorMatrix[Utilities.Math.Vector.Floor(MatrixWorldPosition)]
            };
        }
    }

    #endregion

    //Built-in Unity Functions
    #region Unity Functions

    private void Update()
    {
        CheckForSensorCondition();

    }

    #endregion

    //Determine what condition the sensor is in.
    #region Calculation

    /// <summary>
    /// Determine where the player is in relation to the sensor.
    /// </summary>
    private void CheckForSensorCondition()
    {
        Vector3 _direction = transform.GetChild(1).position - transform.GetChild(0).position;

        RaycastHit2D detector = Physics2D.Raycast(origin: transform.GetChild(0).position,
                                     direction: _direction,
                                     distance: (transform.GetChild(1).position - transform.GetChild(0).position).magnitude,
                                     layerMask: LayerMask.GetMask("Player"));

        if (detector.collider == null)
        {
            if (released) { return; }
            released = true;
        }

        //in other words, the collider is not null
        if (released)
        {
            DetermineEnteringOrExitingSide();
            ReportChangeOfSide();
        }
    }

    /// <summary>
    /// Determine whether the player is entering or exiting the sensor, and which side they are entering/exiting on.
    /// </summary>
    private void DetermineEnteringOrExitingSide()
    {
        if (enteringSide == Side.none)
        {
            enteringSide = GetSideOfHalfSpace(transform.GetChild(0).position, transform.GetChild(1).position, StoredComponents.Player_Transform.position);
            released = false;
        }
        else
        {
            exitingSide = GetSideOfHalfSpace(transform.GetChild(0).position, transform.GetChild(1).position, StoredComponents.Player_Transform.position);
        }
    }

    /// <summary>
    /// Report that the player has crossed the sensor if that has occured.
    /// </summary>
    private void ReportChangeOfSide()
    {
        if (enteringSide != Side.none & exitingSide != Side.none)
        {
            if (enteringSide != exitingSide)
            {
                StoredComponents.Player.OnSensorCrossed(this);
            }
            enteringSide = Side.none;
            exitingSide = Side.none;
        }
    }

    #endregion

}
