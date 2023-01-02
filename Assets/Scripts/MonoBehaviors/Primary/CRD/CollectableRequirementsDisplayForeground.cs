using DTO.Visuals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

/// <summary>
/// Class governing the behavior of the Collectable Requirements Display Foreground
/// </summary>
public class CollectableRequirementsDisplayForeground : MonoBehaviour
{

    /// <summary>
    /// Instantiate all of the objects in the foreground.
    /// Essentially all the objects are spawned in positive
    /// coordinates and then the parent transform is moved backwards.
    /// </summary>
    /// <param name="requirements">The requirements of the gate to display.</param>
     public void SpawnForeground(Inventory requirements)
    {

        float runningEndpoint = 0.2f;

        foreach(CollectableType type in requirements.TypesRequired)
        {

            var CRD = transform.parent.GetComponent<CollectableRequirementsDisplay>();

            //i have no clue don't look at me
            GameObject collectableIcon = (GameObject) Instantiate(CRD.UICollectable, parent: transform, instantiateInWorldSpace: false);
            collectableIcon.transform.localPosition = new Vector3(runningEndpoint + 0.5f, 0);                           

            collectableIcon.GetComponent<SpriteRenderer>().color = ThemeHandler.Accord(type);

            runningEndpoint += (1f + CRD.Spacing);
        }

        transform.localPosition = new Vector3(-runningEndpoint / 2, 0);

    }

}
