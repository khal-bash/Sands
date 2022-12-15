using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRequirementsDisplay : MonoBehaviour
{
    //Built-in Unity Functions
    #region Unity Functions

    private void Start()
    {
        InitializeDisplay();
    }

    #endregion

    //Initialization
    #region

    /// <summary>
    /// Initializes the CRD.
    /// </summary>
    private void InitializeDisplay()
    {
        GameObject gate = transform.parent.gameObject;

        transform.localScale = Utilities.Math.Vector.CorrectForParentScale(gate.transform.localScale * Floor.size);
        LocateDisplayToPlayerSide(5f);
    }

    /// <summary>
    /// Places the display on the side of the gate that the player is on.
    /// </summary>
    private void LocateDisplayToPlayerSide(float distanceFromGate)
    {
        transform.localPosition = new Vector3(0, distanceFromGate);
        var distanceOnPositiveSide = (DTO.Storage.StoredComponents.Player_Transform.position - transform.position).magnitude;

        transform.localPosition = new Vector3(0, -distanceFromGate);
        var distanceOnNegativeSide = (DTO.Storage.StoredComponents.Player_Transform.position - transform.position).magnitude;

        if (distanceOnPositiveSide < distanceOnNegativeSide)
        {
            transform.localPosition = new Vector3(0, distanceFromGate);
        }
    }

    /// <summary>
    /// Resizes the background to be in the correct orientation and capable of holding all requirements.
    /// </summary>
    /// <param name="requirements">The requirements of the gate.</param>
    public void ShapeBackground(Inventory requirements)
    {
        var background = transform.GetChild(0).GetComponent<DisplayBorder>();
        background.ShapeBorder(requirements);
    }

    #endregion
}
