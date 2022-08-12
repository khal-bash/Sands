using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTO.Storage;

/// <summary>
/// Governs the behavior of the spawned Beam on fire
/// </summary>
public class Beam : MonoBehaviour
{

    // Properties set in the Inspector
    #region Inspector Properties

    /// <summary>
    /// Gets or sets the amount of time in frames a beam remains on screen.
    /// </summary>
    public int Life_Span;

    #endregion

    // Properties set in code
    #region Code Properties

    /// <summary>
    /// Gets the current frame.
    /// </summary>
    int Frame { get; set; }

    /// <summary>
    /// Whether the beam has been reported to <see cref="Player.History"/>
    /// </summary>
    private bool Archived { get; set; }


    #endregion

    // Built-in Unity Functions
    #region Unity Functions

    private void Awake()
    {
        InitializeProperties();
    }

    private void Update()
    {
        Frame++;

        if (Frame == Life_Span)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnDestroy()
    {
        StoredClasses.Player_History.Updating -= GameLogUpdating;
    }

    #endregion

    // Initialization
    #region Initialization

    ///<summary>
    /// Initializes all properties in the Code Properties region.
    ///</summary>
    private void InitializeProperties()
    {
        Frame = 0;
        StoredClasses.Player_History.Updating += GameLogUpdating;
        Archived = false;
    }

    #endregion

    // Event Subscribers
    #region Events

    /// <summary>
    /// Subscirber to the <see cref="History.Updating"/> event.
    /// </summary>
    void GameLogUpdating()
    {
        if (Archived) { return; }
        StoredClasses.Player_History.Last.AddBeam(transform.position,
                                                  transform.rotation,
                                                  transform.localScale);
        Archived = true;

    }

    #endregion

    // Detecting and collecting hit objects
    #region Mining

    /// <summary>
    /// Collects all detected objects.
    /// </summary>
    /// <param name="mask"> The layers on which to detect collectable objects. </param>
    protected void MineObjects(LayerMask mask)
    {
        CollectObjectsHit(DetectObjects(mask));
    }

    ///<summary>
    /// Detects all collectable objects in contact with the center line of the beam.
    ///</summary>
    ///<returns>
    /// A <see cref="RaycastHit2D"/> of detected colliders.
    ///</returns>
    protected RaycastHit2D DetectObjects(LayerMask mask)
    {
        Vector2 target_endpoint = transform.GetChild(0).position;
        Vector2 firing_endpoint = transform.GetChild(1).position;
        Vector2 line_of_fire = target_endpoint - firing_endpoint;

        RaycastHit2D hit = Physics2D.Raycast(firing_endpoint,
                                direction: line_of_fire,
                                distance: line_of_fire.magnitude,
                                layerMask: mask); 

        return hit;
    }

    /// <summary>
    /// Adds all collectable objects in the <c>hit</c> to the player's <see cref="Inventory"/>.
    /// </summary>
    /// <param name="hit">
    /// The detected objects.
    /// </param>
    protected void CollectObjectsHit(RaycastHit2D hit)
    {
        if (hit.collider == null) { return; }

        Collectable collectable = hit.collider.gameObject.GetComponent<Collectable>();
        if (collectable != null)
        {
            StoredClasses.Player_Inventory.AddItem(collectable.Type);
            Destroy(collectable.gameObject);
        }
    }
    #endregion

}
