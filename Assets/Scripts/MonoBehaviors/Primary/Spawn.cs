using UnityEngine;
using DTO.Storage;

/// <summary>
/// Governs the behavior of the spawn.
/// </summary>
public class Spawn : MonoBehaviour
{

    // Properties set in the Inspector
    #region Inspector Properties

    /// <summary>
    /// The Beam that the Spawn Fires.
    /// </summary>
    public GameObject Beam;

    /// <summary>
    /// The damage the Spawn will do to the player on collision.
    /// </summary>
    public int Collision_Damage;

    #endregion

    // Properties set in Code
    #region Code Properties 

    /// <summary>
    /// The number of <see cref="FixedUpdate"/> ticks since this object was instantiated.
    /// </summary>
    private int Fixed_Frame = 0;

    #endregion

    // Built-in Unity Functions
    #region Unity Functions

    private void Start()
    {
        SubscribeToEvents();
    }

    private void FixedUpdate()
    {
        ReadHistory();
    }

    #endregion

    // Initialization
    #region Initialization

    /// <summary>
    /// Subscribes to relevant events.
    /// </summary>
    private void SubscribeToEvents()
    {
        StoredComponents.Player.Damage += DoDamageToPlayer;
    }

    #endregion

    // Event Handlers
    #region Events

    /// <summary>
    /// Deals damage to the player on collision.
    /// </summary>
    /// <param name="enemy">The enemy that caused the damage.</param>
    private void DoDamageToPlayer(GameObject enemy)
    {
        if (enemy != gameObject) { return; }

        int multiplier = 1;
        if (StoredClasses.Player_History.Game[Fixed_Frame].Is_Dashing) { multiplier = 2; }

        StoredClasses.Player_HP.ChangeHP(-Collision_Damage * multiplier);
        StoredComponents.Player.Damage -= DoDamageToPlayer;
        Destroy(gameObject);
    }

    #endregion

    // Reads Archives
    #region History

    /// <summary>
    /// Reads the <see cref="StoredClasses.Player_History"/> and takes the appropriate actions.
    /// </summary>
    private void ReadHistory()
    {
        SetVelocity();
        TrySpawnBeam();

        Fixed_Frame++;
    }

    #endregion

    // Spawn Movement
    #region Movement

    /// <summary>
    /// Sets the spawn velocity.
    /// </summary>
    private void SetVelocity()
    {
        History.FrameHistory next_frame = StoredClasses.Player_History.Game[Fixed_Frame + 1];

        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        Vector3 delta = next_frame.Player_Position - transform.position;

        rigidbody2D.velocity = delta / Time.fixedDeltaTime;
    }

    #endregion

    // Shooting
    #region Shooting

    /// <summary>
    /// Spawns a beam if the current frame contains one.
    /// </summary>
    private void TrySpawnBeam()
    {
        History.FrameHistory current_frame = StoredClasses.Player_History.Game[Fixed_Frame];
        if (!current_frame.Beam_Spawned) { return; }

        GameObject beam = Instantiate(Beam, current_frame.Beam_Position, current_frame.Beam_Rotation);
        beam.transform.localScale = current_frame.Beam_Scale;
    }

    #endregion

}
