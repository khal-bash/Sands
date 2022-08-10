using UnityEngine;
using DTO.Storage;

/// <summary>
/// Governs the behavior of the Spawn-spawned beam.
/// </summary>
public class Beam_Spawn : Beam
{

    // Properties set in the Inspector
    #region Inspector Properties

    /// <summary>
    /// The amount of damage the beam will do to the player.
    /// </summary>
    public int Damage;

    #endregion

    // Built-in Unity Functions
    #region Unity Functions

    private void Start()
    {
        DealDamageToPlayerIfHit();
    }

    #endregion

    // Dealing Damage
    #region Dealing Damage

    /// <summary>
    /// Deals damge to the Player if they are hit.
    /// </summary>
    private void DealDamageToPlayerIfHit()
    {
        LayerMask mask = LayerMask.GetMask("Player");

        RaycastHit2D hit = DetectObjects(mask);

        if (hit.collider != null)
        {
            StoredClasses.Player_HP.ChangeHP(-Damage);
        }
    }

    #endregion

}
