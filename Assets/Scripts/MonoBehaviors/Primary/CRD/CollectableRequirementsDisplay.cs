using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRequirementsDisplay : MonoBehaviour
{

    #region Code Properties

    /// <summary>
    /// The buffer to place on the lengthwise edges of the display. 
    /// </summary>
    public float lengthwiseEdgeBuffer;

    /// <summary>
    /// The buffer to place on the heightwise edges of the display. 
    /// </summary>
    public float heightwiseEdgeBuffer;

    /// <summary>
    /// The spacing in between collectable icons in the foreground.
    /// </summary>
    public float Spacing = 0.2f;

    /// <summary>
    /// The <see cref="GameObject"/> to spawn in the foreground of the display.
    /// </summary>
    public GameObject UICollectable;

    #endregion 

    //Built-in Unity Functions
    #region Unity Functions

    private void Start()
    {
        InitializeDisplay();
    }

    #endregion

    //Initialization
    #region Initialization

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
    /// Displays the ... well, display.
    /// </summary>
    /// <param name="requirements">The requirements of the gate.</param>
    public void DisplaySelf(Inventory requirements)
    {
        ShapeBackground(requirements);
        SpawnForeground(requirements);
    }

    /// <summary>
    /// Resizes the background to be in the correct orientation and capable of holding all requirements.
    /// </summary>
    /// <param name="requirements">The requirements of the gate.</param>
    public void ShapeBackground(Inventory requirements)
    {
        var background = transform.GetChild(0).GetComponent<CollectableRequirementsDisplayBackground>();
        background.ShapeBorder(requirements);
    }

    /// <summary>
    /// Spawns the foreground of the display.
    /// </summary>
    /// <param name="requirements">The requirements of the gate.</param>
    public void SpawnForeground(Inventory requirements)
    {
        var foregroundAnchor = transform.GetChild(1).GetComponent<CollectableRequirementsDisplayForeground>();
        foregroundAnchor.SpawnForeground(requirements);
    }

    #endregion Initialization
}
