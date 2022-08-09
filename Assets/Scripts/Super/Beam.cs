using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// Gets the Player <c>GameObject</c>.
    /// </summary>
    Player Player { get; set; }

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
        Player.Game_Log.Updating -= GameLogUpdating;
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
        Player = GameObject.Find("Player").GetComponent<Player>();
        Player.Game_Log.Updating += GameLogUpdating;
        Archived = false;
    }

    #endregion

    // Detecting and collecting hit objects
    #region Mining

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
            Player.Inventory.AddItem(collectable.Type);
            Destroy(collectable.gameObject);
        }
    }
    #endregion

    void GameLogUpdating()
    {
        if (!Archived)
        {
            Player.Game_Log.Last.AddBeam(transform.position,
                                         transform.rotation,
                                         transform.localScale);
            Archived = true;
        }
    }

}
