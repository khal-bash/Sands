using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

using UnityEngine;
using DTO.Storage;

public class CollectablesSpawner : MonoBehaviour
{

    public GameObject collectable;

    public float respawn_time;

    private Random random { get => StoredComponents.LevelMetaData.random; }

    // Start is called before the first frame update
    void Start()
    {

        Beam.ItemCollected += OnItemCollected;

        foreach (Floor floor in StoredComponents.LevelMetaData.floors)
        {
            RandomlyPlaceCollectableOnFloor(floor);
        }
    }

    private void RandomlyPlaceCollectableOnFloor(Floor floor)
    {
        float x = -0.45f + (0.9f * (float)random.NextDouble());
        float y = -0.45f + (0.9f * (float)random.NextDouble());

        float scaleCorrection = (float)(decimal.Divide(1, floor.size));

        var instantiatedCollectable = Instantiate(collectable, floor.transform);

        Vector3 position = new Vector3(x, y);

        if ((floor.matrixWorldPosition == Vector2Int.zero) & (position.magnitude < 0.05f))
        {
            position = position.normalized * 0.1f;
        }

        instantiatedCollectable.transform.localPosition = position;
        instantiatedCollectable.transform.localScale = new Vector3(scaleCorrection, scaleCorrection, 1);
        instantiatedCollectable.GetComponent<Collectable>().SetVisuals(floor.theme);
    }

    IEnumerator RandomlyPlaceCollectableOnFloorDuringPlay(Floor floor)
    {
        yield return new WaitForSeconds(respawn_time);
        RandomlyPlaceCollectableOnFloor(floor);
    }

    private void OnItemCollected(GameObject item)
    {
        StartCoroutine(RandomlyPlaceCollectableOnFloorDuringPlay(item.GetComponent<Collectable>().locale));
    }
}
