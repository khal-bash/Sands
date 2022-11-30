using UnityEngine;

/// <summary>
/// Governs the behavior of the Player-spawned beam.
/// </summary>
public class Beam_Player : Beam
{

    // Built-in Unity Functions
    #region Unity Functions

    private void Start()
    {
        Archivable = true;
        LayerMask mask = LayerMask.GetMask("Collectable");
        MineObjects(mask);
    }

    #endregion

}
