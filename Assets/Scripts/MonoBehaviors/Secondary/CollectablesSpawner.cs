using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

using UnityEngine;
using DTO.Storage;

/// <summary>
/// Randomly spawns collectables in zones.
/// </summary>
public class CollectablesSpawner : MonoBehaviour
{

    //Properties set in the inspector.
    #region Inspector Properties

    /// <summary>
    /// The collectable prefab to spawn
    /// </summary>
    public GameObject collectable;

    /// <summary>
    /// The time to wait after a zone is emptied to spawn a new collectable.
    /// </summary>
    public float respawn_time;

    #endregion

    //Properties set in code
    #region Code Properties
    
    /// <summary>
    /// The random number generator specified in the <see cref="LevelMetaData"/>.
    /// </summary>
    private Random Random { get => StoredComponents.LevelMetaData.Random; }

    #endregion

    //Built-in Unity Functions
    #region Unity Functions

    void Start()
    {

        Beam.ItemCollected += OnItemCollected;

        foreach (Floor floor in StoredComponents.LevelMetaData.Registry.Floors)
        {
            RandomlyPlaceCollectableOnFloor(floor);
        }
    }

    #endregion

    //Placing Collectables
    #region Collectables

    /// <summary>
    /// Randomly places a collectable in the given floor.
    /// </summary>
    /// <param name="floor">The floor in which to place the item.</param>
    private void RandomlyPlaceCollectableOnFloor(Floor floor)
    {
        float x = -0.45f + (0.9f * (float)Random.NextDouble());
        float y = -0.45f + (0.9f * (float)Random.NextDouble());

        float scaleCorrection = (float)(decimal.Divide(1, Floor.size));

        var instantiatedCollectable = Instantiate(collectable, floor.transform);

        Vector3 position = new Vector3(x, y);

        if ((floor.MatrixWorldPosition == Vector2Int.zero) & (position.magnitude < 0.05f))
        {
            position = position.normalized * 0.1f;
        }

        instantiatedCollectable.transform.localPosition = position;
        instantiatedCollectable.transform.localScale = new Vector3(scaleCorrection, scaleCorrection, 1);
        instantiatedCollectable.GetComponent<Collectable>().SetVisuals(floor.Theme);
    }

    /// <summary>
    /// Waits for <see cref="respawn_time"/> and then places a collectable on the floor.
    /// </summary>
    /// <param name="floor">The floor in which to place the item.</param>
    IEnumerator RandomlyPlaceCollectableOnFloorDuringPlay(Floor floor)
    {
        yield return new WaitForSeconds(respawn_time);
        RandomlyPlaceCollectableOnFloor(floor);
    }

    #endregion

    //Event Subscribers
    #region Events

    private void OnItemCollected(GameObject item)
    {
        StartCoroutine(RandomlyPlaceCollectableOnFloorDuringPlay(item.GetComponent<Collectable>().Locale));
    }

    #endregion
}
