using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Governs the behavior of the Portal.
/// </summary>
public class Portal : MonoBehaviour
{

    // Properties set in the Inspector
    #region Inspector Properties

    /// <summary>
    /// The <see cref="GameObject"/> that the Portal Spawns.
    /// </summary>
    public GameObject spawn;

    /// <summary>
    /// The number of frames it takes for the <see cref="spawn"/> to spawn. 
    /// Evaluated as modulo on the Update.
    /// </summary>
    public int spawn_rate;

    #endregion

    // Built-in Unity Functions
    #region Unity Functions

    void Update()
    {
        SpawnObjectIfNeeded();
    }

    #endregion

    // Spawning Objects
    #region Spawning

    /// <summary>
    /// Spawn an object if: <code>Time.frameCount % spawn_rate == spawn_rate - 1</code>
    /// </summary>
    void SpawnObjectIfNeeded()
    {
        if (Time.frameCount % spawn_rate == spawn_rate - 1)
        {
            Instantiate(spawn, transform);
        }
    }

    #endregion

}
